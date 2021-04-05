using Cms.Orls.Interfaces.Rights;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface IAccountManagerGrain
    {
        Task AddGroup(IGroupGrain group);
        Task UpdateLogin(string login);
        Task UpdatePassowrd(string passwordHash);
        Task Unlock();
    }
}
