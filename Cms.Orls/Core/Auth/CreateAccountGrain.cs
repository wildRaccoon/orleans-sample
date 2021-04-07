using Cms.Orls.Core.Query.AccountQuery;
using Cms.Orls.Interfaces.Auth;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class CreateAccountGrain : Grain , ICreateAccountGrain
    {
        private IAccountQuery accountQuery;
        private IGrainFactory factory;
        private IAccountManagerGrain account;

        public CreateAccountGrain(IAccountQuery accountQuery, IGrainFactory factory)
        {
            this.accountQuery = accountQuery;
            this.factory = factory;
        }

        public async Task Create(string login, string passwordHash)
        {
            var accounExists = await IsExists(login);
            if (accounExists)
            {
                throw new Exception($"Account already exist {login}.");
            }

            //create new account
            account = factory.GetGrain<IAccountManagerGrain>(Guid.NewGuid().ToString("N"));
            await account.UpdateLogin(login);
            await account.UpdatePassword(passwordHash);
        }

        private async Task<bool> IsExists(string login)
        {
            var accountState = await accountQuery.ByLogin(login);
            return accountState != null;
        }
    }
}
