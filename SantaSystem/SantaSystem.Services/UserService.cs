using AutoMapper.QueryableExtensions;
using SantaSystem.Common;
using SantaSystem.Common.Enums;
using SantaSystem.Data.Repositories;
using SantaSystem.Interfaces;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using System.Linq;

namespace SantaSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IQueryable<UserDTO> GetAll()
        {
            var result = this.userRepository.GetAll().ProjectTo<UserDTO>();
            return result;
        }

        public IQueryable<UserDTO> GetUsers(string searchPhrase = null, SortType? sortDisplayName = null, int pageNumber = 1)
        {
            var users = this.GetAll();
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

            return users;
        }

        public UserDTO GetUser(string username)
        {
            var user = this.GetAll().FirstOrDefault(x => x.Username == username);
            return user;
        }

        public string GetUsername(string userId)
        {
            var user = this.GetAll().FirstOrDefault(x => x.Id == userId);
            string username = user?.Username;
            return username;
        }
    }
}
