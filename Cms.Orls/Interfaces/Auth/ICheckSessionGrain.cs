using Orleans;
using System.Threading.Tasks;


namespace Cms.Orls.Interfaces.Auth
{
    public interface ICheckSessionGrain : IGrainWithStringKey
    {
        Task For(ISessionGrain session);
        Task<bool> IsValid();
    }
}
