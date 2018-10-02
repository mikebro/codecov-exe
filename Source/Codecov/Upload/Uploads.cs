using System;
using System.Collections.Generic;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Terminal;
using Codecov.Url;

namespace Codecov.Upload
{
    internal class Uploads : IUpload
    {
        private readonly Lazy<IEnumerable<IUpload>> _uploaders;

        public Uploads(IUrl url, IReport report, IDictionary<TerminalName, ITerminal> terminals, bool disableS3)
        {
            _uploaders = new Lazy<IEnumerable<IUpload>>(() => SetUploaders(url, report, terminals, disableS3));
        }

        private IEnumerable<IUpload> Uploaders => _uploaders.Value;

        public string Uploader()
        {
            foreach (var upload in Uploaders)
            {
                var response = upload.Uploader();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    return response;
                }

                Log.Verboase("Uploader failed.");
            }

            throw new Exception("Failed to upload the report.");
        }

        private static IEnumerable<IUpload> SetUploaders(IUrl url, IReport report, IDictionary<TerminalName, ITerminal> terminals, bool disableS3)
        {
            var uploaders = new List<IUpload>();

            if (disableS3)
            {
                uploaders.Add(new HttpWebRequestV2(url, report));
            }
            else
            {
                uploaders.Add(new HttpWebRequestV4(url, report));

                if (terminals[TerminalName.Powershell].Exists)
                {
                    uploaders.Add(new WebClientV4(url, report, terminals[TerminalName.Powershell]));
                }
            }

            return uploaders;
        }
    }
}
