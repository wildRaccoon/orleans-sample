using Cms.Core.Rights;
using Cms.Orls.Interfaces.Rights;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using Cms.Orls.Interfaces.Auth;
using System;

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

        public class CreateAccountRequest
        {
            public string Login { get; set; }
            public string PasswordHash { get; set; }
        }

        [HttpPost]
        [Route("[controller]/create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest();
            }

            var createAccountGrain = grainFactory.GetGrain<ICreateAccountGrain>(request.Login);
            await createAccountGrain.Create(request.Login, request.PasswordHash);

            return Json(new { login = request.Login });
        }

        public class LoginAccountRequest
        {
            public string Login { get; set; }
            public string PasswordHash { get; set; }
        }

        [HttpPost]
        [Route("[controller]/login")]
        public async Task<IActionResult> LoginAccount([FromBody] LoginAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest();
            }

            var loginGrain = grainFactory.GetGrain<ILoginGrain>(request.Login);
            var token = await loginGrain.PerformLogin(request.PasswordHash);

            return Json(new { token = token });
        }

        public class GetGroupByIdRequest
        {
            public string Login { get; set; }
            public string Token { get; set; }
            public string GroupId { get; set; }
        }

        [HttpPost]
        [Route("[controller]/GetGroup")]
        public async Task<IActionResult> GetGroup([FromBody] GetGroupByIdRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.GroupId))
            {
                return BadRequest();
            }

            var sessionGrain = grainFactory.GetGrain<ISessionGrain>(request.Login);
            var isValidToken = await sessionGrain.CheckSession(request.Token);
            if (!isValidToken)
            {
                return BadRequest();
            }

            var groupItem = grainFactory.GetGrain<IGroupGrain>(request.GroupId);

            var data = new
            {
                id = await groupItem.GetId(),
                name = await groupItem.GetName(),
                rights = (await groupItem.GetRights()).ToString()
            };

            return Json(data);
        }

        public class CreateGroupRequest
        {
            public string Login { get; set; }
            public string Token { get; set; }
            public string GroupName { get; set; }
        }

        [HttpPost]
        [Route("[controller]/CreateGroup")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.GroupName))
            {
                return BadRequest();
            }

            var sessionGrain = grainFactory.GetGrain<ISessionGrain>(request.Login);
            var isValidToken = await sessionGrain.CheckSession(request.Token);
            if (!isValidToken)
            {
                return BadRequest();
            }

            var newGroupId = Guid.NewGuid().ToString("N");

            var groupManageItem = grainFactory.GetGrain<IGroupManagerGrain>(newGroupId);
            await groupManageItem.UpdateName(request.GroupName);
            await groupManageItem.UpdateRights(UserRights.MinValue);

            var groupItem = grainFactory.GetGrain<IGroupGrain>(newGroupId);

            var data = new
            {
                Id = await groupItem.GetId(),
                Name = await groupItem.GetName()
            };

            return Json(data);
        }
    }
}
