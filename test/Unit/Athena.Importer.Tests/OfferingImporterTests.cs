using System;
using System.Collections.Generic;
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
    public class OfferingImporterTests : IDisposable
    {
        private static readonly Uri DummyApiEndpoint = new Uri("http://localhost:5000/api");
        
        private readonly Mock<IDataProvider<Offering>> _data;
        private readonly OfferingImporter _sut;

        private readonly HttpTest _http;

        public OfferingImporterTests()
        {
            _data = new Mock<IDataProvider<Offering>>();
            _sut = new OfferingImporter(DummyApiEndpoint, null, _data.Object);
            _http = new HttpTest();
        }

        [Theory, AutoData]
        public async Task MapsMeetingsToOfferings(List<Offering> offerings)
        {
            _data.Setup(o => o.GetData()).Returns(offerings);

            await _sut.LinkMeetings();

            foreach (var offering in offerings)
            {
                foreach (var meeting in offering.Meetings)
                {
                    _http.ShouldHaveCalled($"{DummyApiEndpoint}/v1/offering/{offering.Id}/meetings/{meeting.Id}")
                        .WithVerb(HttpMethod.Put)
                        .Times(1);
                }
            }
        }
        
        public void Dispose()
        {
            _http.Dispose();
        }
    }
}