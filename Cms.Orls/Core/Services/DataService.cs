using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Linq;

namespace Cms.Orls.Core.Services
{
    public class DataService : IDataService
    {
        IMongoClient client;
        CmsOptions options;

        public DataService(IMongoClient client, IOptions<CmsOptions> options)
        {
            this.client = client;
            this.options = options.Value;
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            var name = typeof(T).Name.Split('.', '+').Last();

            return client.GetDatabase(options.DataBase).GetCollection<T>(options.Prefix + name);
        }
    }
}
