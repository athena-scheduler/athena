using System;
using System.Net.Http;
using System.Threading.Tasks;
using Athena.Importer.Provider;
using AutoFixture.Xunit2;
using Flurl.Http.Testing;
using Moq;
using Xunit;

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
            _sut = new ObjectMapImporter(new Uri(ApiEndpoint), null, _data.Object);
            
            _http = new HttpTest();
        }

        [Theory, AutoData]
        public async Task LinksInstitutionsToCampuses(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var institution in map.CampusInstitutions.Keys)
            {
                foreach (var campus in map.CampusInstitutions[institution])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/institution/{institution}/campuses/{campus}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksOfferingsToCoursese(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var course in map.CourseOfferings.Keys)
            {
                foreach (var offering in map.CourseOfferings[course])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/course/{course}/offering/{offering}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksRequirementsToCourses(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var course in map.CourseRequirements.Keys)
            {
                foreach (var req in map.CourseRequirements[course])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/course/{course}/requirements/satisfies/{req}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksPrereqsToCourse(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var course in map.CoursePrereqs.Keys)
            {
                foreach (var req in map.CoursePrereqs[course])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/course/{course}/requirements/prereq/{req}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksConcurrentPrereqsToCcourse(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var course in map.CourseConcurrentPrereqs.Keys)
            {
                foreach (var req in map.CourseConcurrentPrereqs[course])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/course/{course}/requirements/prereq/concurrent/{req}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksMeetingsToOfferings(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var offering in map.OfferingMeetings.Keys)
            {
                foreach (var meeting in map.OfferingMeetings[offering])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/offering/{offering}/meeting/{meeting}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        [Theory, AutoData]
        public async Task LinksReqsToProgram(ObjectMap map)
        {
            _data.Setup(d => d.GetMap()).Returns(map);

            await _sut.Import();

            foreach (var program in map.ProgramRequirements.Keys)
            {
                foreach (var req in map.ProgramRequirements[program])
                {
                    _http.ShouldHaveCalled($"{ApiEndpoint}/v1/program/{program}/requirement/{req}")
                        .WithVerb(HttpMethod.Put);
                }
            }
        }

        public void Dispose() => _http.Dispose();
    }
}