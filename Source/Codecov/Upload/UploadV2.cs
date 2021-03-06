﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Codecov.Coverage.Report;
using Codecov.Logger;
using Codecov.Url;

namespace Codecov.Upload
{
    internal abstract class UploadV2 : IUpload
    {
        private readonly Lazy<IReport> _report;

        private readonly Lazy<IUrl> _url;

        protected UploadV2(IUrl url, IReport report)
        {
            _url = new Lazy<IUrl>(() => url);
            _report = new Lazy<IReport>(() => report);
        }

        protected IReport Report => _report.Value;

        protected Uri Uri => _url.Value.GetUrl(ApiVersion.V2);

        private string DisplayUrl
        {
            get
            {
                var url = Uri.ToString();
                var regex = new Regex(@"token=\w{8}-\w{4}-\w{4}-\w{4}-\w{12}&");
                return regex.Replace(url, string.Empty);
            }
        }

        public string Uploader()
        {
            Log.Information($"url: {Uri.Scheme}://{Uri.Authority}");
            Log.Verboase($"api endpoint: {Uri}");
            Log.Information($"query: {DisplayUrl}");

            try
            {
                var response = Post();
                if (string.IsNullOrWhiteSpace(response))
                {
                    Log.Verboase("Failed to ping codecov.");
                    return string.Empty;
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.VerboaseException(ex);
                return string.Empty;
            }
        }

        protected abstract string Post();
    }
}
