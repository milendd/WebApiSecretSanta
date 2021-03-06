﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.Identity;
using SantaSystem.Common;
using SantaSystem.Common.Enums;
using SantaSystem.Interfaces;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using SantaSystem.Web.Models.Groups;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        private readonly IGroupService groupService;
        private readonly IUserService userService;

        public GroupsController(IGroupService groupService,
                                IUserService userService)
        {
            this.groupService = groupService;
            this.userService = userService;
        }

        [HttpPost]
        [Route(nameof(CreateGroup))]
        public IHttpActionResult CreateGroup(CreateGroupViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool groupExists = this.groupService.GroupExists(viewModel.GroupName);
            if (groupExists)
            {
                return Conflict();
            }
            
            var group = Mapper.Map<Group>(viewModel);
            group.CreatorId = User.Identity.GetUserId();
            group.CreatedAt = DateTime.Now;
            
            this.groupService.AddGroup(group);
            var result = new CreateGroupResultModel
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                CreatedAt = group.CreatedAt,
                CreatedBy = User.Identity.GetUserName(),
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

            var group = this.groupService.GetGroup(viewModel.GroupName);
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
            
            var user = this.userService.GetUser(viewModel.ToUsername);
            if (user == null)
            {
                return NotFound();
            }

            var alreadyInvited = this.groupService.AlreadyInvited(group.GroupId, user.Id);
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

            this.groupService.AddInvitation(invitation);
            var result = new SendInvitationResultModel
            {
                InvitationId = invitation.InvitationId,
                GroupId = invitation.GroupId,
                CreatedAt = invitation.CreatedAt,
                CreatedBy = user.Username,
            };

            return Created("", result); // TODO: url
        }

        [HttpGet]
        [Route(nameof(ViewInvitations))]
        public IHttpActionResult ViewInvitations(SortType? sortDate = null, int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be positive");
            }

            var currentUserId = User.Identity.GetUserId();
            var invitations = this.groupService.GetInvitations(currentUserId, sortDate, pageNumber);

            return Ok(invitations);
        }

        [HttpPost]
        [Route(nameof(AcceptInvitation))]
        public IHttpActionResult AcceptInvitation(AcceptInvitationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = this.groupService.GetGroup(viewModel.GroupName);
            if (group == null)
            {
                return BadRequest("No such group");
            }

            var currentUserId = User.Identity.GetUserId();
            var username = User.Identity.GetUserName();

            var acceptedInvitation = this.groupService.AcceptInvitation(currentUserId, group.GroupId);
            if (!acceptedInvitation)
            {
                string message = $"No invitation for user '{username}' in group '{group.Name}'";
                return this.Content(HttpStatusCode.Forbidden, message);
            }

            var result = new AcceptInvitationResultModel
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                Username = username,
            };

            return Created("", result); // TODO: url
        }

        [HttpDelete]
        [Route(nameof(RejectInvitation))]
        public IHttpActionResult RejectInvitation(RejectInvitationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var group = this.groupService.GetGroup(viewModel.GroupName);
            if (group == null)
            {
                return this.Content(HttpStatusCode.NotFound, "No such group");
            }

            var currentUserId = User.Identity.GetUserId();
            var username = User.Identity.GetUserName();

            var rejectedInvitation = this.groupService.RejectInvitation(currentUserId, group.GroupId);
            if (!rejectedInvitation)
            {
                string message = $"No invitation for user '{username}' in group '{group.Name}'";
                return this.Content(HttpStatusCode.NotFound, message);
            }
            
            return this.Content(HttpStatusCode.NoContent, "");
        }

        [HttpGet]
        [Route(nameof(GetPersonalGroups))]
        public IHttpActionResult GetPersonalGroups(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                return BadRequest("PageNumber must be positive");
            }

            var currentUserId = User.Identity.GetUserId();
            var groups = this.groupService.GetPersonalGroups(currentUserId, pageNumber);
            return Ok(groups);
        }
    }
}
