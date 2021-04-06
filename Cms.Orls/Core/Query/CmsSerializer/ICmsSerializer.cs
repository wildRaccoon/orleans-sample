using Newtonsoft.Json.Linq;

namespace Cms.Orls.Core.Query.CmsSerializer
{
    public interface ICmsSerializer
    {
        T Deserialize<T>(JObject token);
    }
}
