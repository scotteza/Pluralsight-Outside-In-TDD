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
        private HttpSelfHostConfiguration config;
        private HttpSelfHostServer server;

        [SetUp]
        public void SetUp()
        {
            baseAddress = new Uri("http://localhost:9876");
            config = new HttpSelfHostConfiguration(baseAddress);
            new Bootstrap().Configure(config);
            server = new HttpSelfHostServer(config);
        }

        [Test]
        public void GetResponseReturnsCorrectStatusCode()
        {
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