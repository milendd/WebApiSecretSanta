using AutoMapper;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;
using SantaSystem.Web.Models.Groups;

namespace SantaSystem.Web.Utils
{
    public static class AutomapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDTO>();
                config.CreateMap<CreateGroupViewModel, Group>()
                    .ForMember(dest => dest.Name, src => src.MapFrom(x => x.GroupName));
            });
        }
    }
}
