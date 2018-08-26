using AutoMapper;
using Microsoft.AspNet.Identity;
using SantaSystem.Data.Repositories;
using SantaSystem.Models.DomainModels;
using SantaSystem.Web.Models.Groups;
using System.Linq;
using System.Web.Http;

namespace SantaSystem.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Groups")]
    public class GroupsController : BaseAuthApiController
    {
        private readonly IGenericRepository<Group> groupRepository;

        public GroupsController(IGenericRepository<Group> groupRepository,
                                IGenericRepository<User> userRepository) : base(userRepository)
        {
            this.groupRepository = groupRepository;
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

            this.groupRepository.Add(group);

            var result = new CreateGroupResultModel
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                CreatorName = this.GetCurrentUsername(),
            };

            return Created("", result); // TODO: url
        }
    }
}
