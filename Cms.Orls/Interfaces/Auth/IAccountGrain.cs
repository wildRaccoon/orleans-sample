using Cms.Core.Rights;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface IAccountGrain
    {
        Task<string> GetId();
        Task<bool> CheckPass(string passwordHash);
        Task<string> GetLogin();
        Task<UserRights> GetRights();
        Task<bool> IsLocked();
    }
}
