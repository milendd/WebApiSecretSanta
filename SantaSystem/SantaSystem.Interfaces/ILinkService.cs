using SantaSystem.Models.DomainModels;
using System.Linq;

namespace SantaSystem.Interfaces
{
    public interface ILinkService
    {
        IQueryable<Link> GetLinks(int groupId);

        void StartLink(int groupId);
    }
}
