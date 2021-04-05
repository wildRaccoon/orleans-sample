using Cms.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cms.Orls.Core.Query.AccountQuery
{
    public interface IAccountQuery
    {
        Task<Account> ByLogin(string login);
    }
}
