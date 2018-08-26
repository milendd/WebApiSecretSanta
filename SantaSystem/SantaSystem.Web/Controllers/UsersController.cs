using AutoMapper.QueryableExtensions;
using SantaSystem.Common;
using SantaSystem.Common.Enums;
using SantaSystem.Data.Repositories;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using System.Linq;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private readonly IGenericRepository<User> userRepository;

        public UsersController(IGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        
        [Route(nameof(GetUsers))]
        public IHttpActionResult GetUsers(string searchPhrase = null, SortType? sortDisplayName = null, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                ModelState.AddModelError("", "PageNumber must be positive");
                return BadRequest(ModelState);
            }

            var users = this.userRepository.GetAll().ProjectTo<UserDTO>();
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                users = users.Where(x => x.DisplayName.Contains(searchPhrase));
            }

            if (sortDisplayName != null)
            {
                users = sortDisplayName == SortType.Ascending ?
                    users.OrderBy(x => x.DisplayName) :
                    users.OrderByDescending(x => x.DisplayName);
            }
            else
            {
                users = users.OrderBy(x => x.Id);
            }

            int skip = (pageNumber - 1) * Globals.UsersPageSize;
            users = users.Skip(skip).Take(Globals.UsersPageSize);

            return Ok(users);
        }
    }
}