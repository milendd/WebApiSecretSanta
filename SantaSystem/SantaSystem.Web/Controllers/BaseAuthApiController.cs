using Microsoft.AspNet.Identity;
using SantaSystem.Interfaces;
using System.Linq;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    public abstract class BaseAuthApiController : ApiController
    {
        protected IUserService UserService { get; private set; }

        public BaseAuthApiController(IUserService userService)
        {
            this.UserService = userService;
        }

        protected string GetCurrentUsername()
        {
            var userId = User.Identity.GetUserId();
            var user = this.UserService.GetAll().FirstOrDefault(x => x.Id == userId);
            return user?.Username;
        }
    }
}
