using NUnit.Framework;
using System;
using System.Net.Http;
using System.Web.Http.SelfHost;

namespace RunningJournalApi.AcceptanceTests
{
    [TestFixture]
    public class HomeJsonTests
    {
        private Uri baseAddress;

        [SetUp]
        public void SetUp()
        {
            baseAddress = new Uri("http://localhost:9876");
        }

        [Test]
        public void GetResponseReturnsCorrectStatusCode()
        {
            var config = new HttpSelfHostConfiguration(baseAddress);
            var server = new HttpSelfHostServer(config);

            using (var client = new HttpClient(server))
            {
                client.BaseAddress = baseAddress;

                var response = client.GetAsync("").Result;

                Assert.IsTrue(
                    response.IsSuccessStatusCode,
                    "Actual status code: " + response.StatusCode);
            }
        }
    }
}