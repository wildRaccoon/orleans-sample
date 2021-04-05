using Cms.Orls.Core.Query.AccountQuery;
using Cms.Orls.Interfaces.Auth;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Auth
{
    public class LoginGrain : Grain, ILogin
    {
        private IAccountQuery accountQuery;
        private IGrainFactory factory;
        private IAccountGrain account;

        public LoginGrain(IAccountQuery accountQuery, IGrainFactory factory)
        {
            this.accountQuery = accountQuery;
            this.factory = factory;
        }

        public override async Task OnActivateAsync()
        {
            var login = this.GetPrimaryKeyString();
            var data = await accountQuery.ByLogin(login);

            if (data != null)
            {
                account = factory.GetGrain<IAccountGrain>(data.Id);
            }

            await base.OnActivateAsync();
        }

        #region MyRegion
        public async Task<string> GetToken()
        {
            var accId = await account.GetId();
            
            var session = factory.GetGrain<ISessionGrain>(accId);
            return await session.InitFor(account);
        }

        public Task<bool> Validate(string pass)
        {
            if (account == null || account.IsLocked().Result)
            {
                return Task.FromResult(false);
            }

            return account.CheckPass(pass);
        } 
        #endregion
    }
}
