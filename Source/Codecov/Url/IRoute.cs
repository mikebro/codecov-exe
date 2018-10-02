namespace Codecov.Url
{
    internal interface IRoute
    {
        string GetRoute(ApiVersion apiVersion);
    }
}
