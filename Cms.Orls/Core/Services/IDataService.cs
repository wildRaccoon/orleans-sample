using MongoDB.Driver;

namespace Cms.Orls.Core.Services
{
    public interface IDataService
    {
        IMongoCollection<T> GetCollection<T>();
    }
}
