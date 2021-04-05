using Cms.Orls.Core.Auth;
using Cms.Orls.Core.Rights;
using Orleans;
using Orleans.Hosting;

namespace Cms.Orls
{
    public static class RegistrationExtension
    {
        public static ISiloHostBuilder ConfigureCms(this ISiloHostBuilder builder)
        {
            builder.ConfigureApplicationParts(parts =>
                parts
                .AddApplicationPart(typeof(AccountGrain).Assembly)
                .AddApplicationPart(typeof(GroupGrain).Assembly)
                .WithReferences());

            return builder;
        }
    }
}
