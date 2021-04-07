using Cms.Contracts.Auth;
using Cms.Orls.Core.Query.CmsSerializer;
using Cms.Orls.Core.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using Orleans.Providers.MongoDB.StorageProviders;
using System.Linq;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Query.AccountQuery
{
    public class AccountQueryHandler : IAccountQuery
    {
        IDataService service;
        ICmsSerializer serializer;

        public AccountQueryHandler(IDataService service, ICmsSerializer serializer)
        {
            this.service = service;
            this.serializer = serializer;
        }

        public async Task<Account> ByLogin(string login)
        {
            var collection = service.GetCollection<Account>();
            var cursor = await collection.FindAsync(Builders<BsonDocument>.Filter.Eq("_doc.Login", login));

            var accountBson = cursor.FirstOrDefault();

            if (accountBson == null)
            {
                return null;
            }

            return serializer.Deserialize<Account>(accountBson.ToJToken());
        }
    }
}
