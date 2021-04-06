using Cms.Core.Rights;
using Cms.Orls.Interfaces.Rights;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using Cms.Orls.Core.Query.GroupQuery;

namespace Cms.Host.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        IGrainFactory grainFactory;
        IGroupQuery groupQuery;

        public HomeController(IGrainFactory grainFactory, IGroupQuery groupQuery)
        {
            this.grainFactory = grainFactory;
            this.groupQuery = groupQuery;
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

        [HttpGet]
        [Route("[controller]/GetGroups")]
        public async Task<IActionResult> GetGroups()
        {
            var collection = await groupQuery.AllAsync();
            return Json(collection);
        }
    }
}
