using AutoMapper.QueryableExtensions;
using SantaSystem.Common;
using SantaSystem.Common.Enums;
using SantaSystem.Data.Repositories;
using SantaSystem.Interfaces;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using System.Data.Entity;
using System.Linq;

namespace SantaSystem.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> groupRepository;
        private readonly IGenericRepository<Invitation> invitationRepository;
        private readonly IGenericRepository<User> userRepository;

        public GroupService(IGenericRepository<Group> groupRepository,
                            IGenericRepository<Invitation> invitationRepository,
                            IGenericRepository<User> userRepository)
        {
            this.groupRepository = groupRepository;
            this.invitationRepository = invitationRepository;
            this.userRepository = userRepository;
        }

        public IQueryable<Group> GetAllGroups()
        {
            var result = this.groupRepository.GetAll();
            return result;
        }

        public IQueryable<InvitationDTO> GetInvitations(string userId, SortType? sortDate, int pageNumber)
        {
            var allInvitations = this.invitationRepository.GetAll().Where(x => x.UserId == userId);
            var allUsers = this.userRepository.GetAll().ProjectTo<UserDTO>();
            var allGroups = this.groupRepository.GetAll();

            var invitations = from invitation in allInvitations
                              join user in allUsers on invitation.UserId equals user.Id
                              join currentGroup in allGroups on invitation.GroupId equals currentGroup.GroupId

                              select new InvitationDTO
                              {
                                  InvitationId = invitation.InvitationId,
                                  GroupId = invitation.GroupId,
                                  CreatedAt = invitation.CreatedAt,
                                  GroupName = currentGroup.Name,
                                  Username = user.Username,
                              };

            if (sortDate != null)
            {
                invitations = sortDate == SortType.Ascending ?
                    invitations.OrderBy(x => x.CreatedAt) :
                    invitations.OrderByDescending(x => x.CreatedAt);
            }
            else
            {
                invitations = invitations.OrderBy(x => x.InvitationId);
            }

            int skip = (pageNumber - 1) * Globals.InvitationsPageSize;
            invitations = invitations.Skip(skip).Take(Globals.InvitationsPageSize);

            return invitations;
        } 

        public bool GroupExists(string groupName)
        {
            bool result = this.GetAllGroups().Any(x => x.Name == groupName);
            return result;
        }

        public Group GetGroup(string groupName)
        {
            var result = this.GetAllGroups().FirstOrDefault(x => x.Name == groupName);
            return result;
        }

        public bool AlreadyInvited(int groupId, string userId)
        {
            var result = this.invitationRepository.GetAll()
                .Any(x => x.GroupId == groupId && x.UserId == userId);

            return result;
        }

        public void AddGroup(Group group)
        {
            this.groupRepository.Add(group);
        }

        public void AddInvitation(Invitation invitation)
        {
            this.invitationRepository.Add(invitation);
        }

        public bool AcceptInvitation(string userId, int groupId)
        {
            var invitation = this.invitationRepository.GetAll()
                .FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            if (invitation == null)
            {
                return false;
            }

            var group = invitation.Group;
            var user = invitation.User;

            // Remove invitation
            this.invitationRepository.Remove(invitation);
            
            // Add member to group
            var db = this.groupRepository.GetDbContext();
            group.Members.Add(user);
            var item = db.Entry(group);
            item.State = EntityState.Modified;

            db.SaveChanges();
            return true;
        }

        public bool RejectInvitation(string userId, int groupId)
        {
            var invitation = this.invitationRepository.GetAll()
                .FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId);
            if (invitation == null)
            {
                return false;
            }

            // Remove invitation
            this.invitationRepository.Remove(invitation);
            return true;
        }

        public IQueryable<GroupDTO> GetPersonalGroups(string userId, int pageNumber)
        {
            var db = this.groupRepository.GetDbContext();

            var groups = db.Set<Group>().Include(x => x.Members)
                .OrderBy(x => x.Name)
                .Select(x => new GroupDTO
                {
                    GroupId = x.GroupId,
                    GroupName = x.Name,
                    Members = x.CreatorId == userId ? x.Members.Select(m => m.UserName) : null,
                });

            int skip = (pageNumber - 1) * Globals.GroupsPageSize;
            groups = groups.Skip(skip).Take(Globals.GroupsPageSize);

            return groups;
        }
    }
}
