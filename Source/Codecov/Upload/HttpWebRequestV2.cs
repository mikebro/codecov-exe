using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class HttpWebRequestV2 : UploadV2
    {
        public HttpWebRequestV2(IUrl url, IReport report)
            : base(url, report)
        {
        }

        protected override string Post()
        {
            Log.Verboase("Trying to upload using HttpWebRequest.");

            var postRequest = (System.Net.HttpWebRequest)WebRequest.Create(Uri);
            postRequest.ContentType = "text/plain";
            postRequest.Method = "POST";
            postRequest.Headers["Content-Encoding"] = "gzip";
            postRequest.Headers["X-Content-Encoding"] = "gzip";
            postRequest.Headers["Accept"] = "text/plain";

            using (var putStreamWriter = new GZipStream(postRequest.GetRequestStreamAsync().Result, CompressionLevel.Optimal))
            {
                var content = Encoding.UTF8.GetBytes(Report.Reporter);
                putStreamWriter.Write(content, 0, content.Length);
            }

            var postResponse = (HttpWebResponse)postRequest.GetResponseAsync().Result;

            if (postResponse.StatusCode != HttpStatusCode.OK)
            {
                Log.Verboase("Failed to upload the report.");
                return string.Empty;
            }

            using (var postStreamReader = new StreamReader(postResponse.GetResponseStream()))
            {
                return postStreamReader.ReadToEnd();
            }
        }
    }
}
