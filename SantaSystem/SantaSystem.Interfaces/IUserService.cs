using SantaSystem.Models.DomainModels;
using System.Linq;

namespace SantaSystem.Interfaces
{
    public interface IUserService
    {
        IQueryable<User> GetAll();
    }
}
