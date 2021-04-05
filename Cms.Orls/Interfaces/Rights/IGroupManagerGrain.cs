using Cms.Core.Rights;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Rights
{
    public interface IGroupManagerGrain : IGrainWithStringKey
    {
        Task UpdateName(string name);
        Task UpdateRights(UserRights rights);
    }
}
