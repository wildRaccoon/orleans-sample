using Cms.Contracts.Auth;
using Cms.Core.Rights;
using Cms.Orls.Interfaces.Auth;
using Cms.Orls.Interfaces.Rights;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class AccountGrain : Grain<Account>, IAccountGrain, IAccountManagerGrain
    {
        public AccountGrain()
        {
        }

        #region IAccountGrain
        public Task<bool> CheckPass(string passwordHash)
        {
            var check = State.PasswordHash == passwordHash;

            if (State.PasswordHash == passwordHash)
            {
                State.LastSuccessLogin = DateTime.Now;
                return Task.FromResult(true);
            }

            State.FailedRetries++;
            State.IsLocked = State.FailedRetries > 3;

            return Task.FromResult(false);
        }

        public Task<string> GetId()
        {
            return Task.FromResult(State.Id);
        }

        public Task<string> GetLogin()
        {
            return Task.FromResult(State.Login);
        }

        public Task<UserRights> GetRights()
        {
            //TBD
            return Task.FromResult(UserRights.MaxValue);
        }

        public Task<bool> IsLocked()
        {
            return Task.FromResult(State.IsLocked);
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
            State.IsLocked = false;
            State.FailedRetries = 0;
            return WriteStateAsync();
        }

        public Task UpdateLogin(string login)
        {
            if (login == State.Login)
            {
                return Task.CompletedTask;
            }

            State.Login = login;
            return WriteStateAsync();
        }
         
        public Task UpdatePassword(string passwordHash)
        {
            if (passwordHash == State.PasswordHash)
            {
                return Task.CompletedTask;
            }

            State.PasswordHash = passwordHash;
            return WriteStateAsync();
        }
        #endregion
    }
}
