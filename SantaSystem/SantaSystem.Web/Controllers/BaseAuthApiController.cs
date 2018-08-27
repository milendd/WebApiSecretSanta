using Microsoft.AspNet.Identity;
using SantaSystem.Data.Repositories;
using SantaSystem.Models.DomainModels;
using System.Linq;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    public abstract class BaseAuthApiController : ApiController
    {
        protected IGenericRepository<User> UserRepository { get; private set; }

        public BaseAuthApiController(IGenericRepository<User> userRepository)
        {
            this.UserRepository = userRepository;
        }

        protected string GetCurrentUsername()
        {
            var userId = User.Identity.GetUserId();
            var user = this.UserRepository.GetAll().FirstOrDefault(x => x.Id == userId);
            return user?.UserName;
        }
    }
}
