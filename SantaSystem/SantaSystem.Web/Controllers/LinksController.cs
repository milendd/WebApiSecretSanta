using Microsoft.AspNet.Identity;
using SantaSystem.Interfaces;
using SantaSystem.Web.Models.Links;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Links")]
    public class LinksController : ApiController
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;
        private readonly ILinkService linkService;

        public LinksController(IGroupService groupService,
                                IUserService userService,
                                ILinkService linkService)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.linkService = linkService;
        }

        [HttpPost]
        [Route(nameof(StartLink))]
        public IHttpActionResult StartLink(StartLinkViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = this.groupService.GetGroup(viewModel.GroupName);
            if (group == null)
            {
                return NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var isAdmin = group.CreatorId == currentUserId;
            if (!isAdmin)
            {
                return this.Content(HttpStatusCode.Forbidden, "You don't have permissions to start linking to this group");
            }

            if (!group.Members.Any())
            {
                // Except the creator, there are no other members
                return this.Content(HttpStatusCode.PreconditionFailed, "Cannot start linking for group with 1 member");
            }

            var links = this.linkService.GetLinks(group.GroupId);
            if (links.Any())
            {
                return this.Content(HttpStatusCode.PreconditionFailed, "Linking for this group has already started");
            }

            this.linkService.StartLink(group.GroupId);
            return Created("", ""); // TODO: no url and no body
        }
    }
}