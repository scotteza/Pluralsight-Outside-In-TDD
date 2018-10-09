using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        [SetUp]
        public void SetUp()
        {
            new BootStrap().InstallDatabase();
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
                    time = DateTimeOffset.Now,
                    distance = 8500,
                    Duration = TimeSpan.FromMinutes(44)
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
                var time = DateTimeOffset.Now;
                var distance = 8100;
                var duration = TimeSpan.FromMinutes(41);
                var json = new
                {
                    time,
                    distance,
                    duration
                };


                client.PostAsJsonAsync("", json);
                var response = client.GetAsync("").Result;


                var actualString = response.Content.ReadAsStringAsync().Result;
                dynamic actual = JObject.Parse(actualString);

                JournalEntryModel[] journalEntries = JsonConvert.DeserializeObject<JournalEntryModel[]>(actual.entries.ToString());

                Assert.That(journalEntries.Length, Is.EqualTo(1));
                Assert.That(journalEntries[0].Time, Is.EqualTo(time));
                Assert.That(journalEntries[0].Distance, Is.EqualTo(distance));
                Assert.That(journalEntries[0].Duration, Is.EqualTo(duration));
            }
        }
    }
}
