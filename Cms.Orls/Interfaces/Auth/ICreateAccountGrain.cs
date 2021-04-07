using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface ICreateAccountGrain : IGrainWithStringKey
    {
        Task Create(string login, string passwordHash);
    }
}
