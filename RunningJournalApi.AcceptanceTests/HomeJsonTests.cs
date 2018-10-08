using NUnit.Framework;
using System;
using System.Net.Http;
using System.Web.Http.SelfHost;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        [Test]
        public void GetResponseReturnsCorrectStatusCode()
        {
            using (var client = HttpClientFactory.Create())
            {
                var response = client.GetAsync("").Result;

                Assert.IsTrue(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}