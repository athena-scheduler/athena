using Athena.Core.Models;
using Athena.Importer.Provider;
using Flurl.Http;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Athena.Importer
{
    public class OfferingImporter : GenericImporter<Offering>
    {
        public OfferingImporter(Uri apiEndpoint, string apiKey, IDataProvider<Offering> data) : base(apiEndpoint, apiKey, data)
        {
        }

        public override async Task Import()
        {
            await base.Import();

            foreach(var offering in _data.GetData())
            {
                foreach(var meeting in offering.Meetings)
                {
                    meeting.Offering = offering.Id;

                    Log.Debug("Importing {@obj}", meeting);
                    await CreateRequest()
                        .AppendPathSegment("meeting")
                        .PostJsonAsync(meeting);
                }
            }
        }
    }
}
