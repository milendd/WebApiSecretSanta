using SantaSystem.Common.Extensions;
using SantaSystem.Data.Repositories;
using SantaSystem.Interfaces;
using SantaSystem.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaSystem.Services
{
    public class LinkService : ILinkService
    {
        private readonly IGenericRepository<Link> linkRepository;
        private readonly IGenericRepository<Group> groupRepository;
        private readonly IGenericRepository<User> userRepository;

        public LinkService(IGenericRepository<Link> linkRepository,
                           IGenericRepository<Group> groupRepository,
                           IGenericRepository<User> userRepository)
        {
            this.linkRepository = linkRepository;
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        public IQueryable<Link> GetLinks(int groupId)
        {
            var result = this.linkRepository.GetAll().Where(x => x.GroupId == groupId);
            return result;
        }

        public void StartLink(int groupId)
        {
            var group = this.groupRepository.GetAll().FirstOrDefault(x => x.GroupId == groupId);
            
            var members = group.Members
                .Select(x => x.Id)
                .Union(new List<string> { group.CreatorId })
                .ToList();

            var ids = ListExtensions.Shuffle(members);

            var links = new List<Link>();
            for (int i = 0; i < ids.Count - 1; i++)
            {
                var fromId = ids[i];
                var toId = ids[i + 1];
                var link = new Link
                {
                    GroupId = groupId,
                    MemberFromId = fromId,
                    MemberToId = toId,
                };

                links.Add(link);
            }

            // And the last will link with the first
            links.Add(new Link
            {
                GroupId = groupId,
                MemberFromId = ids.Last(),
                MemberToId = ids[0],
            });

            var db = this.linkRepository.GetDbContext();
            foreach (var link in links)
            {
                db.Set<Link>().Add(link);
            }

            db.SaveChanges();
        }

        public string CheckLinkStarted(string userId, string groupName)
        {
            var group = this.groupRepository.GetAll().FirstOrDefault(x => x.Name == groupName);
            if (group == null)
            {
                return null;
            }

            var link = this.linkRepository.GetAll()
                .FirstOrDefault(x => x.MemberFromId == userId && x.GroupId == group.GroupId);

            if (link == null)
            {
                return null;
            }

            var result = link.MemberTo?.UserName;
            return result;
        }
    }
}
