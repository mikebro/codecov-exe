using System;

namespace Codecov.Url
{
    internal interface IUrl
    {
        Uri GetUrl(ApiVersion version);
    }
}
