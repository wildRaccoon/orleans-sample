using Cms.Contracts.Auth;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Auth;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class SessionGrain : Grain<Session>, ISessionGrain
    {
        IGrainFactory factory;
        IAccountGrain account;

        public SessionGrain(IGrainFactory factory)
        {
            this.factory = factory;
        }

        public override Task OnActivateAsync()
        {
            if (!string.IsNullOrEmpty(State.AccountId))
            {
                account = factory.GetGrain<IAccountGrain>(State.AccountId);
            }
            return base.OnActivateAsync();
        }

        #region ISessionGrain
        public async Task<bool> CheckSession(string token)
        {
            State.Expired = State.LastAccess <= DateTime.Now.AddMinutes(2);

            if (State.Expired)
            {
                return false;
            }

            State.LastAccess = DateTime.Now;
            var tokenValid = State.Token == token;
            var isLocked = await account.IsLocked();

            return tokenValid && !isLocked;
        }

        public async Task<string> InitFor(IAccountGrain account)
        {
            this.account = account;
            State.LastAccess = DateTime.Now;
            State.Rights = await account.GetRights();
            State.AccountId = await account.GetId();
            State.Expired = false;
            State.Token = Guid.NewGuid().ToString("N");

            await WriteStateAsync();

            return State.Token;
        }

        public async Task<bool> IsAllowed(UserRights requiredRights)
        {
            var accountRights = await account.GetRights();
            return accountRights.HashRight(requiredRights);
        } 
        #endregion
    }
}
