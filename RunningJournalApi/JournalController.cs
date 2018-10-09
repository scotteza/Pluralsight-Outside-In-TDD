using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.Http;
using Simple.Data;

namespace RunningJournalApi
{
    public class JournalController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connectionString);

            var entries = db.JournalEntry
                .FindAll(db.JournalEntry.Users.Username == "foo")
                .ToArray<JournalEntryModel>();

            return this.Request.CreateResponse(HttpStatusCode.OK, new JournalModel
            {
                Entries = entries
            });
        }

        public HttpResponseMessage Post(JournalEntryModel journalEntry)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connectionString);

            var userId = db.Users.Insert(UserName: "foo").UserId;

            db.JournalEntry.Insert(
                UserId: userId,
                Time: journalEntry.Time,
                Distance: journalEntry.Distance,
                Duration: journalEntry.Duration);

            return this.Request.CreateResponse();
        }
    }
}
