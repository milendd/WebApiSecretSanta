using SantaSystem.Common.Enums;
using SantaSystem.Models.DTOs;
using System.Linq;

namespace SantaSystem.Interfaces
{
    public interface IUserService
    {
        IQueryable<UserDTO> GetAll();

        IQueryable<UserDTO> GetUsers(string searchPhrase = null, SortType? sortDisplayName = null, int pageNumber = 1);

        UserDTO GetUser(string username);

        string GetUsername(string userId);
    }
}
