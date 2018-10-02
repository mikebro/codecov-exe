using System;

namespace Codecov.Url
{
    internal class Url : IUrl
    {
        public Url(IHost host, IRoute route, IQuery query)
        {
            Host = host;
            Route = route;
            Query = query;
        }

        private IHost Host { get; }

        private IQuery Query { get; }

        private IRoute Route { get; }

        public Uri GetUrl(ApiVersion version)
        {
            var route = Route.GetRoute(version);
            return new Uri($"{Host.GetHost}/{route}?{Query.GetQuery}");
        }
    }
}
