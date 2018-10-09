using Newtonsoft.Json.Serialization;
using RunningJournalApi.Properties;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.SelfHost;

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

        public void InstallDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" };
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;

                    var commandText = Resources.RunningDbSchema;
                    foreach (var sql in commandText.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UninstallDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" };
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;

                    cmd.CommandText = @"IF EXISTS
                                        (
                                            SELECT *
                                            FROM master.dbo.sysdatabases
                                            WHERE name = N'RunningJournal'
                                        )
                                            BEGIN
                                                DROP DATABASE RunningJournal
                                            END";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}