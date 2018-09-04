using AutoMapper.QueryableExtensions;
using SantaSystem.Common;
using SantaSystem.Common.Enums;
using SantaSystem.Interfaces;
using SantaSystem.Models.DTOs;
using System.Linq;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        
        [Route(nameof(GetUsers))]
        public IHttpActionResult GetUsers(string searchPhrase = null, SortType? sortDisplayName = null, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be positive");
            }

            var users = this.userService.GetAll().ProjectTo<UserDTO>();
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

        [Route(nameof(FindUser))]
        public IHttpActionResult FindUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var user = this.userService.GetAll().FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
