using System.Net;

using FluentAssertions;
using NUnit.Framework;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace WireMockDemo
{
    [TestFixture]
    public class WireMockTests
    {
        private WireMockServer? _server;

        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        [Test]
        public async Task Should_respond_to_request()
        {
            // Arrange (start WireMock.Net server)
            _server!
              .Given(Request.Create().WithPath("/foo").UsingGet())
              .RespondWith(
                Response.Create()
                  .WithStatusCode(200)
                  .WithBody(@"{ ""msg"": ""Hello world!"" }")
              );

            var response = await new HttpClient().GetAsync($"{_server.Urls[0]}/foo");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TearDown]
        public void ShutdownServer()
        {
            _server!.Stop();
        }
    }
}