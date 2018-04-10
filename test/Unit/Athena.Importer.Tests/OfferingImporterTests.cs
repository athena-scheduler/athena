using Athena.Core.Models;
using Athena.Importer.Provider;
using AutoFixture.Xunit2;
using Flurl.Http.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Athena.Importer.Tests
{
    public class OfferingImporterTests : NoOutputTest
    {
        private static readonly Uri DummyApiEndpoint = new Uri("http://localhost:5000/api");

        private readonly Mock<IDataProvider<Offering>> _data;
        private readonly OfferingImporter _sut;

        private readonly HttpTest _http;

        public OfferingImporterTests()
        {
            _http = new HttpTest();

            _data = new Mock<IDataProvider<Offering>>();
            _sut = new OfferingImporter(DummyApiEndpoint, null, _data.Object);
        }

        [Theory, AutoData]
        public async Task ImportMeetings_Valid(List<Offering> offerings)
        {
            _data.Setup(d => d.GetData()).Returns(offerings);

            await _sut.Import();

            foreach(var o in offerings)
            {
                foreach(var m in o.Meetings)
                {
                    m.Offering = o.Id;
                    _http.ShouldHaveCalled(DummyApiEndpoint.ToString().TrimEnd('/') + "/v1/meeting")
                       .WithVerb(HttpMethod.Post)
                       .WithRequestJson(m)
                       .Times(1);
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _http.Dispose();
        }
    }
}
