using Cms.Orls.Core.Auth;
using Cms.Orls.Core.Rights;
using Orleans;
using Orleans.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Cms.Orls.Core.Query.AccountQuery;

namespace Cms.Orls
{
    public static class RegistrationExtension
    {
        public static ISiloBuilder ConfigureCms(this ISiloBuilder builder)
        {
            return builder.ConfigureServices(sc =>
                {
                    sc.AddTransient<IAccountQuery, AccountQueryHandler>();
                })
                .ConfigureApplicationParts(parts =>
                    parts
                    //.AddApplicationPart(typeof(AccountGrain).Assembly)
                    .AddApplicationPart(typeof(GroupGrain).Assembly)
                    //.AddApplicationPart(typeof(SessionGrain).Assembly)
                    //.AddApplicationPart(typeof(LoginGrain).Assembly)
                    .WithReferences());
        }
    }
}
