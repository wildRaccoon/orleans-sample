using Cms.Contracts.Auth;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Auth;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class SessionGrain : Grain, ISessionGrain
    {
        IGrainFactory factory;
        IAccountGrain account;
        IPersistentState<Session> persistent;

        public SessionGrain(IGrainFactory factory, [PersistentState("Cms.Contracts.Auth.Session", "Cms")] IPersistentState<Session> persistent)
        {
            this.factory = factory;
            this.persistent = persistent;
        }

        public override Task OnActivateAsync()
        {
            account = factory.GetGrain<IAccountGrain>(this.GetPrimaryKeyString());

            if (!persistent.RecordExists)
            {
                persistent.State.AccountId = this.GetPrimaryKeyString();
                //
                //persistent.State.Rights = UserRights.MinValue;
                persistent.State.Token = string.Empty;
            }

            return base.OnActivateAsync();
        }

        #region ISessionGrain
        public async Task<bool> CheckSession(string token)
        {
            persistent.State.Expired = persistent.State.LastAccess <= DateTime.Now.AddMinutes(-2);

            if (persistent.State.Expired)
            {
                return false;
            }

            persistent.State.LastAccess = DateTime.Now;
            var tokenValid = persistent.State.Token == token;
            var isLocked = await account.IsLocked();

            return tokenValid && !isLocked;
        }

        public async Task<string> Init()
        {            
            persistent.State.LastAccess = DateTime.Now;
            //persistent.State.Rights = await account.GetRights();
            persistent.State.AccountId = await account.GetId();
            persistent.State.Expired = false;
            var token = persistent.State.Token = Guid.NewGuid().ToString("N");

            await persistent.WriteStateAsync();

            var checkSessionGrain = factory.GetGrain<ICheckSessionGrain>(token);
            await checkSessionGrain.For(this);

            return token;
        }

        public async Task<bool> IsAllowed(UserRights requiredRights)
        {
            var accountRights = await account.GetRights();
            return accountRights.HashRight(requiredRights);
        } 
        #endregion
    }
}
