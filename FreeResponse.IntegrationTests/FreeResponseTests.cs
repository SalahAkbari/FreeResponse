using FreeResponse.IntegrationTests.Helpers;
using FreeResponse.Web.Host;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FreeResponse.IntegrationTests
{
    public class FreeResponseTests : IClassFixture<TestFixture<Startup>>
    {
        private static HttpClient Client;

        public FreeResponseTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetProjectsAsync()
        {
            // Arrange
            var request = "/api/v2/projects";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestGetProjectAsync()
        {
            // Arrange
            var request = "/api/v2/projects/6";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPostProjectAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v2/projects",
                Body = new
                {
                    Name = "project8",
                    ExternalId = "project8",
                    SdlcSystemId = 1
                }
            };

            // Act
            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task TestPatchProjectAsync()
        {
            // Arrange
            var request = new
            {
                Url = "/api/v2/projects/9",
                Body = new
                {
                    Name = "Name-Changed",
                    ExternalId = "ExternalId-Changed",
                    SdlcSystemId = 1
                }
            };

            // Act
            var response = await Client.PatchAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}

