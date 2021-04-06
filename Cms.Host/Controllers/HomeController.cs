using Cms.Core.Rights;
using Cms.Orls.Interfaces.Rights;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;

namespace Cms.Host.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        IGrainFactory grainFactory;

        public HomeController(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }

        [HttpGet]
        [Route("[controller]/index")]
        public IActionResult Index()
        {
            return Json(new { message = "hello" });
        }

        [HttpGet]
        [Route("[controller]/GetGroup")]
        public async Task<IActionResult> GetGroup(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var groupManageItem = grainFactory.GetGrain<IGroupManagerGrain>(id);
            await groupManageItem.UpdateName("new name");
            await groupManageItem.UpdateRights(UserRights.MinValue);

            var groupItem = grainFactory.GetGrain<IGroupGrain>(id);

            var data = new {
                id = await groupItem.GetId(),
                name = await groupItem.GetName(),
                rights = (await groupItem.GetRights()).ToString()
            };

            return Json(data);
        }
    }
}
