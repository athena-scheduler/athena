using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Flurl.Http.Testing;
using Xunit;

namespace Athena.Importer.Tests
{
    public class ProgramTests : IDisposable
    {
        private static readonly string DataDir = GetDataDir();
        private const int MaxSearchDepth = 10;
        private const string ApiTestEndpoint = "http://localhost:5000/api";

        private static string GetDataDir()
        {
            var pwd = Path.GetDirectoryName(new Uri(typeof(ProgramTests).Assembly.CodeBase).LocalPath);
            
            for (var i = 0; i < MaxSearchDepth; i++)
            {
                if (File.Exists(Path.Combine(pwd, "Athena.sln")))
                {
                    return Path.Combine(pwd, "src", "Athena.Importer", "data");
                }

                pwd = Directory.GetParent(pwd).FullName;
            }

            throw new InvalidOperationException("Could not find test data. Expected it to be in <solutionRoot>/src/Athena.Importer/Data");
        }

        private readonly HttpTest _http;

        public ProgramTests() => _http = new HttpTest();

        [Fact]
        public async Task ReturnsNonZero_MissingArgs()
        {
            var result = await Program.Main(new string[0]);
            
            Assert.NotEqual(0, result);
        }

        [Theory, AutoData]
        public async Task ReturnsNonZero_BadUri(string endpoint)
        {
            var result = await Program.Main(new[] {"--api-endpoint", endpoint});
            
            Assert.NotEqual(0, result);
        }

        [Theory, AutoData]
        public async Task ReteurnsNonZero_TestDataNotFound(string testData)
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", $"/{testData}"});
            
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task ImportsCampuses()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/campus")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }
        
        [Fact]
        public async Task ImportsCoursees()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/course")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }
        
        [Fact]
        public async Task ImportsInstitutions()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/institution")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }
        
        [Fact]
        public async Task ImportsOfferings()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/offering")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }
        
        [Fact]
        public async Task ImportsPrograms()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/program")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }
        
        [Fact]
        public async Task ImportsRequirements()
        {
            var result = await Program.Main(new[] {"--api-endpoint", ApiTestEndpoint, "--data-path", DataDir});
            
            Assert.Equal(0, result);
            _http.ShouldHaveCalled($"{ApiTestEndpoint}/v1/requirement")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json");
        }

        public void Dispose() => _http.Dispose();
    }
}