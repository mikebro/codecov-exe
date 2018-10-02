using System;
using Codecov.Url;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Codecov.Tests.Url
{
    public class UrlTests
    {
        [Fact]
        public void Should_Return_V4_Url()
        {
            // Given
            var host = Substitute.For<IHost>();
            host.GetHost.Returns("https://codecov.io");
            var route = Substitute.For<IRoute>();
            route.GetRoute(ApiVersion.V4).Returns("upload/v4");
            var query = Substitute.For<IQuery>();
            query.GetQuery.Returns("branch=develop&commit=123");
            var url = new Codecov.Url.Url(host, route, query);

            // When
            var getUrl = url.GetUrl(ApiVersion.V4);

            // Then
            getUrl.Should().Be(new Uri("https://codecov.io/upload/v4?branch=develop&commit=123"));
        }

        [Fact]
        public void Should_Return_V2_Url()
        {
            // Given
            var host = Substitute.For<IHost>();
            host.GetHost.Returns("https://codecov.io");
            var route = Substitute.For<IRoute>();
            route.GetRoute(ApiVersion.V2).Returns("upload/v2");
            var query = Substitute.For<IQuery>();
            query.GetQuery.Returns("branch=develop&commit=123");
            var url = new Codecov.Url.Url(host, route, query);

            // When
            var getUrl = url.GetUrl(ApiVersion.V2);

            // Then
            getUrl.Should().Be(new Uri("https://codecov.io/upload/v2?branch=develop&commit=123"));
        }
    }
}
