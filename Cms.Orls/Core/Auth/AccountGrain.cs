using Cms.Contracts.Auth;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Auth;
using Cms.Orls.Interfaces.Rights;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class AccountGrain : Grain, IAccountGrain, IAccountManagerGrain
    {

        IPersistentState<Account> persistent;
        public AccountGrain([PersistentState("Cms.Contracts.Auth.Account", "Cms")] IPersistentState<Account> persistent)
        {
            this.persistent = persistent;
        }

        public override async Task OnActivateAsync()
        {
            if (!persistent.RecordExists)
            {
                persistent.State.Id = this.GetPrimaryKeyString();
                persistent.State.LastSuccessLogin = DateTime.MinValue;
                persistent.State.IsLocked = false;
                persistent.State.PasswordHash = string.Empty;
                persistent.State.FailedRetries = 0;
            }

            await base.OnActivateAsync();
        }

        #region IAccountGrain
        public Task<bool> CheckPass(string passwordHash)
        {
            if (persistent.State.PasswordHash == passwordHash)
            {
                persistent.State.LastSuccessLogin = DateTime.Now;
                return Task.FromResult(true);
            }

            persistent.State.FailedRetries++;
            persistent.State.IsLocked = persistent.State.FailedRetries > 3;

            return Task.FromResult(false);
        }

        public Task<string> GetId()
        {
            return Task.FromResult(persistent.State.Id);
        }

        public Task<string> GetLogin()
        {
            return Task.FromResult(persistent.State.Login);
        }

        public Task<UserRights> GetRights()
        {
            //TBD
            return Task.FromResult(UserRights.MaxValue);
        }

        public Task<bool> IsLocked()
        {
            return Task.FromResult(persistent.State.IsLocked);
        }
        #endregion

        #region IAccountManagerGrain
        public Task AddGroup(IGroupGrain group)
        {
            //TBD
            return Task.CompletedTask;
        }

        public Task Unlock()
        {
            persistent.State.IsLocked = false;
            persistent.State.FailedRetries = 0;
            return persistent.WriteStateAsync();
        }

        public Task UpdateLogin(string login)
        {
            if (login == persistent.State.Login)
            {
                return Task.CompletedTask;
            }

            persistent.State.Login = login;
            return persistent.WriteStateAsync();
        }
         
        public Task UpdatePassword(string passwordHash)
        {
            if (passwordHash == persistent.State.PasswordHash)
            {
                return Task.CompletedTask;
            }

            persistent.State.PasswordHash = passwordHash;
            return persistent.WriteStateAsync();
        }
        #endregion
    }
}
