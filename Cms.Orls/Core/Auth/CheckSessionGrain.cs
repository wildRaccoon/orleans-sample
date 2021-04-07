using Cms.Orls.Interfaces.Auth;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class CheckSessionGrain : Grain, ICheckSessionGrain
    {
        string token;
        ISessionGrain sessionGrain;
        public override Task OnActivateAsync()
        {
            token = this.GetPrimaryKeyString();
            return base.OnActivateAsync();
        }

        public Task<bool> IsValid()
        {
            if (string.IsNullOrWhiteSpace(token) || sessionGrain == null)
            {
                return Task.FromResult(false);
            }

            return sessionGrain.CheckSession(token);
        }

        public Task For(ISessionGrain sessionGrain)
        {
            this.sessionGrain = sessionGrain;
            return Task.CompletedTask;
        }
    }
}
