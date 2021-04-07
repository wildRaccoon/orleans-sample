using Cms.Orls.Core.Auth;
using Cms.Orls.Core.Rights;
using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Cms.Orls.Core.Query.AccountQuery;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Cms.Orls.Core.Services;
using Cms.Orls.Core.Query.CmsSerializer;
using Cms.Orls.Core.Query.GroupQuery;

namespace Cms.Orls
{
    public static class RegistrationExtension
    {
        public static ISiloBuilder ConfigureCms(this ISiloBuilder builder, string dbName)
        {
            return builder.ConfigureServices(sc =>
                {
                    sc.AddTransient<ICmsSerializer, CmsSerializerImp>();
                    sc.AddTransient<IGroupQuery, GroupQueryHandler>();
                    sc.AddTransient<IAccountQuery, AccountQueryHandler>();

                    sc.Configure<CmsOptions>(config =>
                    {
                        config.DataBase = dbName;
                    });
                    sc.AddTransient<IDataService, DataService>();
                })
                .ConfigureApplicationParts(parts =>
                    parts
                    .AddApplicationPart(typeof(AccountGrain).Assembly)
                    .AddApplicationPart(typeof(GroupGrain).Assembly)
                    .AddApplicationPart(typeof(SessionGrain).Assembly)
                    .AddApplicationPart(typeof(LoginGrain).Assembly)
                    .WithReferences());
        }
    }
}
