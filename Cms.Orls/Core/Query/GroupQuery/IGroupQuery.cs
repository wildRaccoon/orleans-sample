using Cms.Contracts.Rights;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Query.GroupQuery
{
    public interface IGroupQuery
    {
        Task<List<Group>> AllAsync();
    }
}
