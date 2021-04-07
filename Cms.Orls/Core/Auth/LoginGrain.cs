using Cms.Orls.Core.Query.AccountQuery;
using Cms.Orls.Interfaces.Auth;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class LoginGrain : Grain, ILoginGrain
    {
        private IAccountQuery accountQuery;
        private IGrainFactory factory;
        private IAccountGrain account;
        private string login;

        public LoginGrain(IAccountQuery accountQuery, IGrainFactory factory)
        {
            this.accountQuery = accountQuery;
            this.factory = factory;
        }

        public override async Task OnActivateAsync()
        {
            login = this.GetPrimaryKeyString();
            var data = await accountQuery.ByLogin(login);

            if (data != null)
            {
                account = factory.GetGrain<IAccountGrain>(data.Id);
            }

            await base.OnActivateAsync();
        }

        #region ILogin
        public async Task<string> PerformLogin(string password)
        {
            var validPassoword = await Validate(password);
            if (!validPassoword)
            {
                throw new Exception($"Unable to login account {login}");
            }

            return await GetToken();
        }

        public async Task<bool> CheckToken(string token)
        {
            if (account == null || account.IsLocked().Result)
            {
                return false;
            }

            var accId = await account.GetId();
            var session = factory.GetGrain<ISessionGrain>(accId);
            return await session.CheckSession(token);
        } 
        #endregion

        private async Task<string> GetToken()
        {
            var accId = await account.GetId();
            
            var session = factory.GetGrain<ISessionGrain>(accId);
            return await session.InitFor(account);
        }

        private Task<bool> Validate(string pass)
        {
            if (account == null || account.IsLocked().Result)
            {
                return Task.FromResult(false);
            }

            return account.CheckPass(pass);
        }
    }
}
