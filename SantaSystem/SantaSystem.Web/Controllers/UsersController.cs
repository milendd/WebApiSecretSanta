﻿using SantaSystem.Common.Enums;
using SantaSystem.Interfaces;
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
            
            var users = this.userService.GetUsers(searchPhrase, sortDisplayName, pageNumber);
            return Ok(users);
        }

        [Route(nameof(FindUser))]
        public IHttpActionResult FindUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var user = this.userService.GetUser(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
