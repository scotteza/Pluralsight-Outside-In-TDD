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
        private HttpSelfHostConfiguration httpSelfHostConfiguration;
        private HttpSelfHostServer httpSelfHostServer;

        [SetUp]
        public void SetUp()
        {
            baseAddress = new Uri("http://localhost:9876");
            httpSelfHostConfiguration = new HttpSelfHostConfiguration(baseAddress);
            httpSelfHostServer = new HttpSelfHostServer(httpSelfHostConfiguration);
        }

        [Test]
        public void GetResponseReturnsCorrectStatusCode()
        {
            using (var client = new HttpClient(httpSelfHostServer))
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