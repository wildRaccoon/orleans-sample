using Cms.Orls.Core.Query.CmsSerializer;
using Cms.Orls.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Orleans.Providers.MongoDB.StorageProviders;
using Cms.Contracts.Rights;

namespace Cms.Orls.Core.Query.GroupQuery
{
    public class GroupQueryHandler : IGroupQuery
    {
        IDataService service;
        ICmsSerializer serializer;

        public GroupQueryHandler(IDataService service, ICmsSerializer serializer)
        {
            this.service = service;
            this.serializer = serializer;
        }

        public async Task<List<Group>> AllAsync()
        {
            var collection = service.GetCollection<Group>();
            var cursor = await collection.FindAsync(new BsonDocument());
            var docs = await cursor.ToListAsync();


            return docs
                .Select(x => serializer.Deserialize<Group>(x.ToJToken()) )
                .ToList();
        }
    }
}
