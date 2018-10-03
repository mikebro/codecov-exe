﻿using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Terminal;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class WebClientV4 : UploadV4
    {
        public WebClientV4(IUrl url, IReport report, ITerminal powerShell)
            : base(url, report)
        {
            PowerShell = powerShell;
        }

        private ITerminal PowerShell { get; }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using WebClient in PowerShell.");

            var script = $@"
                $client = New-Object System.Net.WebClient;
                $client.Headers.add('X-Content-Type','application/x-gzip');
                $client.Headers.add('X-Reduced-Redundancy','false');
                $client.UploadString('{Uri}', 'POST', '');
            ";

            return PowerShell.RunScript(script);
        }

        protected override bool Put(Uri url)
        {
            var tempFilePath = WriteReport2TempFile();
            var command = $@"
                $source = '
                    using System.Net;

	                public class ExtendedWebClient : WebClient
	                {{
		                public int Timeout;

		                protected override WebRequest GetWebRequest(System.Uri address)
		                {{
			                WebRequest request = base.GetWebRequest(address);
			                if (request != null)
			                {{
				                request.Timeout = Timeout;
			                }}
			                return request;
                        }}

		                public ExtendedWebClient()
		                {{
			                Timeout = 1000000;
		                }}
	                }}
                ';

                Add-Type -TypeDefinition $source -Language CSharp

                $client = New-Object ExtendedWebClient;
                $client.Headers.add('Content-Type','application/x-gzip');
                $client.Headers.add('Content-Encoding','gzip');
                $client.Headers.add('x-amz-acl','public-read');
                $client.UploadFile('{url}', 'PUT', '{tempFilePath}');
            ";

            PowerShell.RunScript(command);
            return true;
        }

        private string WriteReport2TempFile()
        {
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new GZipStream(new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None), CompressionLevel.Optimal))
            {
                var content = Encoding.UTF8.GetBytes(Report.Reporter);
                stream.Write(content, 0, content.Length);
            }

            return tempFilePath;
        }
    }
}
