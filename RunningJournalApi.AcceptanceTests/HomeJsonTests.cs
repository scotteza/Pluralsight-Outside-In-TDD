using System;
using System.Net.Http;
using NUnit.Framework;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        [Test]
        public void GetReturnsResponseWithCorrectStatusCode()
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
        public void PostReturnsResponseWithCorrectStatusCode()
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
    }
}