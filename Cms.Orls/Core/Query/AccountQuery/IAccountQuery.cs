using Cms.Contracts.Auth;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Query.AccountQuery
{
    public interface IAccountQuery
    {
        Task<Account> ByLogin(string login);
    }
}
