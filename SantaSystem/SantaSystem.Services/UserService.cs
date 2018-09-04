using SantaSystem.Data.Repositories;
using SantaSystem.Interfaces;
using SantaSystem.Models.DomainModels;
using System.Linq;

namespace SantaSystem.Services
{
    public class UserService : IUserService
    {
        private IGenericRepository<User> userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IQueryable<User> GetAll()
        {
            var result = this.userRepository.GetAll();
            return result;
        }
    }
}
