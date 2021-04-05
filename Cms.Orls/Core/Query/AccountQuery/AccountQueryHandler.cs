using Cms.Contracts.Auth;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Query.AccountQuery
{
    public class AccountQueryHandler : IAccountQuery
    {
        private IMongoCollection<Account> collection;

        public AccountQueryHandler(IMongoCollection<Account> collection)
        {
            this.collection = collection;
        }

        public async Task<Account> ByLogin(string login)
        {
            var cursor = await collection.FindAsync(a => a.Login == login);
            
            if (!cursor.Any())
            {
                return null;
            }

            return cursor.First();
        }
    }
}
