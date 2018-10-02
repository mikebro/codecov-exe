using Codecov.Url;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Url
{
    public class RouteTests
    {
        [Fact]
        public void Should_Be_Version_Four()
        {
            // Given
            var route = new Codecov.Url.Route();

            // When
            var getRoute = route.GetRoute(ApiVersion.V4);

            // Then
            getRoute.Should().Be("upload/v4");
        }

        [Fact]
        public void Should_Be_Version_Two()
        {
            // Given
            var route = new Codecov.Url.Route();

            // When
            var getRoute = route.GetRoute(ApiVersion.V2);

            // Then
            getRoute.Should().Be("upload/v2");
        }
    }
}
