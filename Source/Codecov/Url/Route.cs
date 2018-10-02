using System;

namespace Codecov.Url
{
    internal class Route : IRoute
    {
        public string GetRoute(ApiVersion version)
        {
            switch (version)
            {
                case ApiVersion.V2:
                    return "upload/v2";
                case ApiVersion.V4:
                    return "upload/v4";
                default:
                    throw new ArgumentException($"Version '{version}' not supported.", nameof(version));
            }
        }
    }
}
