using System;
using System.Net.Http;
using System.Threading.Tasks;
using Athena.Importer.Provider;
using Flurl.Http;
using Serilog;

namespace Athena.Importer
{
    public class ObjectMapImporter : AbstractImporter
    {
        private readonly IObjectMapProvider _provider;

        public ObjectMapImporter(Uri apiEndpoint, string apiKey, IObjectMapProvider provider) :
            base(apiEndpoint, apiKey) =>
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        
        public override async Task Import()
        {
            Log.Information("Linking Objects...");

            var map = _provider.GetMap();

            await LinkInstitutionsToCampuses(map);
            await LinkOfferingsToCourses(map);
            await LinkRequirementsToCourses(map);
            await LinkPrereqsToCourses(map);
            await LinkConcurrentPrereqsToCourses(map);
            await LinkMeetingsToOfferings(map);
            await LinkRequirementsToProgram(map);
        }

        private async Task LinkInstitutionsToCampuses(ObjectMap map)
        {
            foreach (var campus in map.CampusInstitutions.Keys)
            {
                foreach (var institution in map.CampusInstitutions[campus])
                {
                    Log.Debug("Linking campus {campus} to institution {institution}", campus, institution);
                    await CreateRequest()
                        .AppendPathSegments("institution", institution, "campuses", campus)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }

        private async Task LinkOfferingsToCourses(ObjectMap map)
        {
            foreach (var course in map.CourseOfferings.Keys)
            {
                foreach (var offering in map.CourseOfferings[course])
                {
                    Log.Debug("Linking offering {offering} to course {course}", offering, course);
                    await CreateRequest()
                        .AppendPathSegments("course", course, "offering", offering)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }

        private async Task LinkRequirementsToCourses(ObjectMap map)
        {
            foreach (var course in map.CourseRequirements.Keys)
            {
                foreach (var req in map.CourseRequirements[course])
                {
                    Log.Debug("Linking reqirement {req} to course {course}", req, course);
                    await CreateRequest()
                        .AppendPathSegments("course", course, "reqirement", "satisfies", req)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }

        private async Task LinkPrereqsToCourses(ObjectMap map)
        {
            foreach (var course in map.CoursePrereqs.Keys)
            {
                foreach (var req in map.CoursePrereqs[course])
                {
                    Log.Debug("Linking requirement {req} as a prereq of course {course}", req, course);
                    await CreateRequest()
                        .AppendPathSegments("course", course, "requirement", "prereq", req)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }
        
        private async Task LinkConcurrentPrereqsToCourses(ObjectMap map)
        {
            foreach (var course in map.CourseConcurrentPrereqs.Keys)
            {
                foreach (var req in map.CourseConcurrentPrereqs[course])
                {
                    Log.Debug("Linking requirement {req} as a prereq of course {course}", req, course);
                    await CreateRequest()
                        .AppendPathSegments("course", course, "requirement", "prereq", "concurrent", req)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }

        private async Task LinkMeetingsToOfferings(ObjectMap map)
        {
            foreach (var offering in map.OfferingMeetings.Keys)
            {
                foreach (var meeting in map.OfferingMeetings[offering])
                {
                    Log.Debug("Linking meeting {meeting} to offering {offering}", meeting, offering);
                    await CreateRequest()
                        .AppendPathSegments("offering", offering, "meeting", meeting)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }

        private async Task LinkRequirementsToProgram(ObjectMap map)
        {
            foreach (var program in map.ProgramRequirements.Keys)
            {
                foreach (var req in map.ProgramRequirements[program])
                {
                    Log.Debug("Linking requirement {req} to program {program}", req, program);
                    await CreateRequest()
                        .AppendPathSegments("program", program, "requirement", req)
                        .SendAsync(HttpMethod.Post);
                }
            }
        }
    }
}