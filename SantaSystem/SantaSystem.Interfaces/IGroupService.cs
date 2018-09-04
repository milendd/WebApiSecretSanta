using SantaSystem.Common.Enums;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using System.Linq;

namespace SantaSystem.Interfaces
{
    public interface IGroupService
    {
        IQueryable<Group> GetAllGroups();

        IQueryable<InvitationDTO> GetInvitations(string userId, SortType? sortDate, int pageNumber);

        bool GroupExists(string groupName);

        Group GetGroup(string groupName);
        
        bool AlreadyInvited(int groupId, string userId);

        void AddGroup(Group group);

        void AddInvitation(Invitation invitation);

        bool AcceptInvitation(string userId, int groupId);

        bool RejectInvitation(string userId, int groupId);

        IQueryable<GroupDTO> GetPersonalGroups(string userId, int pageNumber);
    }
}
