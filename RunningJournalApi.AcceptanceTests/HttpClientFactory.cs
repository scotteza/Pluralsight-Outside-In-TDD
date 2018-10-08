﻿using System;
using System.Net.Http;
using System.Web.Http.SelfHost;

namespace RunningJournalApi.AcceptanceTests
{
    public static class HttpClientFactory
    {
        public static HttpClient Create()
        {
            var baseAddress = new Uri("http://localhost:9876");
            var config = new HttpSelfHostConfiguration(baseAddress);
            new BootStrap().Configure(config);
            var server = new HttpSelfHostServer(config);
            var client = new HttpClient(server);
            try
            {
                client.BaseAddress = baseAddress;
                return client;
            }
            catch
            {
                client.Dispose();
                throw;
            }
        }
    }
}
