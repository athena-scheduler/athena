using System;
using Athena.Importer.Provider;
using Flurl.Http.Testing;
using Moq;

namespace Athena.Importer.Tests
{
    public class ObjectMapImporterTests : IDisposable
    {
        private const string ApiEndpoint = "http://localhost:5000/api";
        
        private readonly Mock<IObjectMapProvider> _data;
        private readonly ObjectMapImporter _sut;

        private readonly HttpTest _http;

        public ObjectMapImporterTests()
        {
            _data = new Mock<IObjectMapProvider>();
            _sut = new ObjectMapImporter(new Uri(ApiEndpoint), _data.Object);
            
            _http = new HttpTest();
        }
        
        

        public void Dispose() => _http.Dispose();
    }
}