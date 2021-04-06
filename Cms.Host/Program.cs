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
            var host = new HostBuilder()
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .ConfigureCms()
                        .Configure<ClusterOptions>(opt =>
                        {
                            opt.ClusterId = "debug";
                            opt.ServiceId = "cms_app";
                        })
                        .UseMongoDBClient("mongodb://localhost")
                        .AddMongoDBGrainStorage("Auth", config =>
                        {
                            config.DatabaseName = "CmsAuth";
                        });
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
