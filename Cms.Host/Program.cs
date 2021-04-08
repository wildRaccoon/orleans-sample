using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Threading.Tasks;
using Cms.Orls;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Cms.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostConfig = BuildConfig(args);

            var host = new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseDashboard(conf =>
                        {
                            conf.Port = 8080;
                            conf.HostSelf = true;
                        })
                        .ConfigureCms(hostConfig.MongoDb.Database)
                        .Configure<ClusterOptions>(opt =>
                        {
                            opt.ClusterId = hostConfig.ClusterId;
                            opt.ServiceId = hostConfig.ServiceId;
                        })
                        .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                        .UseMongoDBClient(hostConfig.MongoDb.ConnectionString)
                        .AddMongoDBGrainStorage("Cms", config =>
                        {
                            config.DatabaseName = hostConfig.MongoDb.Database;
                        });

                    if (hostConfig.UseLocalCluster)
                    {
                        siloBuilder.UseLocalhostClustering();
                    }
                    else
                    {
                        siloBuilder
                            .UseMongoDBClustering(config =>
                            {
                                config.DatabaseName = hostConfig.MongoDb.ClusteringDatabase;
                                config.CreateShardKeyForCosmos = hostConfig.CreateShardKey;
                            })
                            .UseMongoDBReminders(config =>
                            {
                                config.CreateShardKeyForCosmos = hostConfig.CreateShardKey;
                                config.DatabaseName = hostConfig.MongoDb.ClusteringDatabase;
                            });
                    }
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();

            await host.RunAsync();
        }

        private static HostConfiguration BuildConfig(string[] args)
        {
            var envName = args.FirstOrDefault() ?? "Dev";

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{envName}.json", optional:true)
                .Build();

            var hostConfig = new HostConfiguration();
            config.GetSection("CmsHost").Bind(hostConfig);

            return hostConfig;
        }
    }
}
