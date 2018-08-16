using AutoMapper;
using SantaSystem.Models.DomainModels;
using SantaSystem.Models.DTOs;

namespace SantaSystem.Web.Utils
{
    public static class AutomapperConfig
    {
        public static void ConfigureMappings()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDTO>();                
            });
        }
    }
}
