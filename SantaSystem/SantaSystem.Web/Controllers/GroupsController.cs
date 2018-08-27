using AutoMapper;
using Microsoft.AspNet.Identity;
using SantaSystem.Data.Repositories;
using SantaSystem.Models.DomainModels;
using SantaSystem.Web.Models;
using SantaSystem.Web.Models.Groups;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Groups")]
    public class GroupsController : BaseAuthApiController
    {
        private readonly IGenericRepository<Group> groupRepository;
        private readonly IGenericRepository<Invitation> invitationRepository;

        public GroupsController(IGenericRepository<Group> groupRepository,
                                IGenericRepository<Invitation> invitationRepository,
                                IGenericRepository<User> userRepository) : base(userRepository)
        {
            this.groupRepository = groupRepository;
            this.invitationRepository = invitationRepository;
        }

        [HttpPost]
        [Route(nameof(CreateGroup))]
        public IHttpActionResult CreateGroup(CreateGroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool groupExists = this.groupRepository.GetAll().Any(x => x.Name == viewModel.GroupName);
            if (groupExists)
            {
                return Conflict();
            }
            
            var group = Mapper.Map<Group>(viewModel);
            group.CreatorId = User.Identity.GetUserId();
            group.CreatedAt = DateTime.Now;

            this.groupRepository.Add(group);

            var result = new CreateGroupResultModel
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                CreatedAt = group.CreatedAt,
                CreatedBy = this.GetCurrentUsername(),
            };

            return Created("", result); // TODO: url
        }

        [HttpPost]
        [Route(nameof(SendInvitation))]
        public IHttpActionResult SendInvitation(SendInvitationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = this.groupRepository.GetAll().FirstOrDefault(x => x.Name == viewModel.GroupName);
            if (group == null)
            {
                return NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var isAdmin = group.CreatorId == currentUserId;
            if (!isAdmin)
            {
                return this.Content(HttpStatusCode.Forbidden, "You don't have permissions to send invitation to this group");
            }
            
            var user = this.UserRepository.GetAll().FirstOrDefault(x => x.UserName == viewModel.ToUsername);
            if (user == null)
            {
                return NotFound();
            }

            var alreadyInvited = this.invitationRepository.GetAll()
                .Any(x => x.GroupId == group.GroupId && x.UserId == user.Id);
            if (alreadyInvited)
            {
                return Conflict();
            }

            var invitation = new Invitation
            {
                GroupId = group.GroupId,
                UserId = user.Id,
                CreatedAt = DateTime.Now,
            };

            this.invitationRepository.Add(invitation);
            
            var result = new SendInvitationResultModel
            {
                InvitationId = invitation.InvitationId,
                GroupId = invitation.GroupId,
                CreatedAt = invitation.CreatedAt,
                CreatedBy = this.GetCurrentUsername(),
            };
            return Created("", result); // TODO: url
        }
    }
}
