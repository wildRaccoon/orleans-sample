using Cms.Core.Rights;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface ISessionGrain : IGrainWithStringKey
    {
        Task<string> Init();
        Task<bool> IsAllowed(UserRights requiredRights);
        Task<bool> CheckSession(string token);
    }
}
