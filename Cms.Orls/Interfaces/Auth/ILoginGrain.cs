using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface ILoginGrain : IGrainWithStringKey
    {
        Task<string> PerformLogin(string password);
        Task<bool> CheckToken(string token);
    }
}
