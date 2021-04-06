using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Threading.Tasks;
using Cms.Orls;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Cms.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "mongodb://admin:pass12345@localhost";
            var createShardKey = false;
            var dbName = "CmsAuth";

            var host = new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder                        
                        .UseDashboard(conf => {
                            conf.Port = 8080;
                            conf.HostSelf = true;
                        })
                        .ConfigureCms(dbName)
                        .Configure<ClusterOptions>(opt =>
                        {
                            opt.ClusterId = "debug";
                            opt.ServiceId = "cms_app";
                        })
                        .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                        .UseMongoDBClient(connectionString)
                        .AddMongoDBGrainStorage("Cms", config =>
                        {
                            config.DatabaseName = dbName;
                        })
                        .UseLocalhostClustering();
                        //.UseMongoDBClustering(config => {
                        //    config.DatabaseName = dbName;
                        //    config.CreateShardKeyForCosmos = createShardKey;
                        //})
                        //.UseMongoDBReminders(config => {
                        //    config.CreateShardKeyForCosmos = createShardKey;
                        //    config.DatabaseName = dbName;
                        //});
                })
                .ConfigureLogging(logBuilder => {
                    logBuilder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
