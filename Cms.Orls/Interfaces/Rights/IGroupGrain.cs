using Cms.Core.Rights;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Rights
{
    public interface IGroupGrain : IGrainWithStringKey
    {
        Task<string> GetId();
        //Task<UserRights> GetRights();
        Task<string> GetName();
    }
}
