using System;
using System.Threading.Tasks;
using Athena.Core.Models;
using Athena.Importer.Provider;
using Flurl.Http;

namespace Athena.Importer
{
    public class OfferingImporter : GenericImporter<Offering>
    {
        public OfferingImporter(Uri apiEndpoint, string apiKey, IDataProvider<Offering> data) : base(apiEndpoint, apiKey, data)
        {
        }

        public async Task LinkMeetings()
        {
            foreach (var offering in _data.GetData())
            {
                foreach (var meeting in offering.Meetings)
                {
                    await CreateRequest()
                        .AppendPathSegments("offering", offering.Id, "meetings", meeting.Id)
                        .PutAsync(null);
                }
            }
        }
    }
}