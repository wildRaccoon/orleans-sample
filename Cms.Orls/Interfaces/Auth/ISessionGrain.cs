using Cms.Core.Rights;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface ISessionGrain
    {
        Task<string> InitFor(IAccountGrain account);
        Task<bool> IsAllowed(UserRights requiredRights);
        Task<bool> IsLive();
    }
}
