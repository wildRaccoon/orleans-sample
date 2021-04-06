using MongoDB.Bson;
using MongoDB.Driver;

namespace Cms.Orls.Core.Services
{
    public interface IDataService
    {
        IMongoCollection<BsonDocument> GetCollection<T>();
    }
}
