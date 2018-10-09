using System.Web.Http;
using System.Web.Http.SelfHost;
using Newtonsoft.Json.Serialization;

namespace RunningJournalApi
{
    public class BootStrap
    {
        public void Configure(HttpSelfHostConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "{controller}/{id}",
                defaults: new
                {
                    controller = "Journal",
                    id = RouteParameter.Optional
                });

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }
}