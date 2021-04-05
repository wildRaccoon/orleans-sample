using Orleans;
using System.Threading.Tasks;

namespace Cms.Orls.Interfaces.Auth
{
    public interface ILogin : IGrainWithStringKey
    {
        Task<bool> Validate(string pass);
        Task<string> GetToken();
    }
}
