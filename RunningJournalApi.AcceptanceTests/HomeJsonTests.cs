using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using NUnit.Framework.Constraints;
using Simple.Data;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        private DateTimeOffset time;
        private int distance;
        private TimeSpan duration;

        [SetUp]
        public void SetUp()
        {
            new BootStrap().InstallDatabase();

            time = DateTimeOffset.FromUnixTimeSeconds(1000);
            distance = 6000;
            duration = TimeSpan.FromMinutes(31);
        }

        [TearDown]
        public void TearDown()
        {
            new BootStrap().UninstallDatabase();
        }

        [Test]
        public void Get_Returns_Response_With_Correct_Status_Code()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.IsTrue(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Test]
        public void Post_Returns_Response_With_Correct_Status_Code()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time,
                    distance,
                    duration
                };

                var response = client.PostAsJsonAsync("", json).Result;

                Assert.IsTrue(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }

        [Test]
        public void Get_After_Post_Returns_Response_With_Correct_Posted_Entry()
        {
            using (var client = HttpClientFactory.Create())
            {
                var json = new
                {
                    time,
                    distance,
                    duration
                };


                client.PostAsJsonAsync("", json);
                var response = client.GetAsync("").Result;
                var journalEntries = GetJournalEntriesFromResponse(response);


                Assert.That(journalEntries.Length, Is.EqualTo(1));
                Assert.That(journalEntries[0].Time, Is.EqualTo(time));
                Assert.That(journalEntries[0].Distance, Is.EqualTo(distance));
                Assert.That(journalEntries[0].Duration, Is.EqualTo(duration));
            }
        }

        [Test]
        public void Get_Returns_Correct_Entry_From_Database()
        {
            dynamic entry = new ExpandoObject();
            entry.time = time;
            entry.distance = distance;
            entry.duration = duration;

            var expected = JObject.FromObject((object)entry);

            var connectionString = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var db = Database.OpenConnection(connectionString);
            var userId = db.Users.Insert(UserName: "foo").UserId;
            entry.userId = userId;
            db.JournalEntry.Insert(entry);


            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;
                var journalEntries = GetJournalEntriesFromResponse(response);

                Assert.That(journalEntries.Length, Is.EqualTo(1));
                Assert.That(journalEntries[0].Time, Is.EqualTo(time));
                Assert.That(journalEntries[0].Distance, Is.EqualTo(distance));
                Assert.That(journalEntries[0].Duration, Is.EqualTo(duration));
            }
        }

        private static JournalEntryModel[] GetJournalEntriesFromResponse(HttpResponseMessage response)
        {
            var actualString = response.Content.ReadAsStringAsync().Result;
            dynamic actual = JObject.Parse(actualString);
            JournalEntryModel[] journalEntries = JsonConvert.DeserializeObject<JournalEntryModel[]>(actual.entries.ToString());
            return journalEntries;
        }
    }
}
