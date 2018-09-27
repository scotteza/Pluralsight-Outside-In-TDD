using NUnit.Framework;
using System;
using System.Net.Http;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        private Uri clientBaseAddress;

        [SetUp]
        public void SetUp()
        {
            clientBaseAddress = new Uri("http://localhost:9876");
        }

        [Test]
        public void GetResponseReturnsCorrectStatusCode()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = clientBaseAddress;

                var response = client.GetAsync("").Result;

                Assert.IsTrue(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}