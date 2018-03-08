using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Importer.Provider;
using AutoFixture.Xunit2;
using Flurl.Http.Testing;
using Moq;
using Xunit;

namespace Athena.Importer.Tests
{
    public class GenericImporterTests : IDisposable
    {
        private static readonly Uri DummyApiEndpoint = new Uri("http://localhost:5000/api");
        
        private readonly Mock<IDataProvider<Campus>> _data;
        private readonly GenericImporter<Campus> _sut;

        private readonly HttpTest _http;
        
        public GenericImporterTests()
        {
            _http = new HttpTest();
            
            _data = new Mock<IDataProvider<Campus>>();
            _sut = new GenericImporter<Campus>(DummyApiEndpoint, _data.Object);
        }

        [Fact]
        public async Task DoesNothingForNoData()
        {
            _data.Setup(d => d.GetData()).Returns(Enumerable.Empty<Campus>());

            await _sut.Import();
            
            _data.Verify(d => d.GetData(), Times.Once);
            _http.ShouldNotHaveMadeACall();
        }
        
        [Theory, AutoData]
        public async Task ImportsData(List<Campus> campuses)
        {
            _data.Setup(d => d.GetData()).Returns(campuses);

            await _sut.Import();
            
            _data.Verify(d => d.GetData(), Times.Once);
            foreach (var c in campuses)
            {
                _http.ShouldHaveCalled(DummyApiEndpoint.ToString().TrimEnd('/') + "/v1/campus")
                    .WithVerb(HttpMethod.Post)
                    .WithRequestJson(c)
                    .Times(1);
            }
        }

        [Fact]
        public void ThrowsForNullProvider() =>
            Assert.Throws<ArgumentNullException>(() => new GenericImporter<Campus>(DummyApiEndpoint, null));

        [Fact]
        public void ThrowsForNullEndpoint() =>
            Assert.Throws<ArgumentNullException>(() => new GenericImporter<Campus>(null, _data.Object));

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}