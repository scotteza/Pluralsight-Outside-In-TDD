using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        private static readonly List<JournalEntryModel> JournalEntries = new List<JournalEntryModel>();

        // This naming (starting with "Get") defines this as a GET method
        public HttpResponseMessage Get()
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = JournalEntries.ToArray()
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            JournalEntries.Add(journalEntry);

            return this.Request.CreateResponse();
        }
    }
}
