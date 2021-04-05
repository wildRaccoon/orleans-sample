using Cms.Orls.Interfaces.Rights;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface IAccountManagerGrain : IGrainWithStringKey
    {
        Task AddGroup(IGroupGrain group);
        Task UpdateLogin(string login);
        Task UpdatePassword(string passwordHash);
        Task Unlock();
    }
}
