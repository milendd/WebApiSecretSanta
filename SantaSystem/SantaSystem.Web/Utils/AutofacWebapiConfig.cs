using Autofac;
using Autofac.Integration.WebApi;
using SantaSystem.Data;
using SantaSystem.Data.Repositories;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Linq;
using SantaSystem.Services;
using SantaSystem.Interfaces;

namespace SantaSystem.Web.Utils
{
    public class AutofacWebapiConfig
    {
        public static IContainer container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<SantaSystemDbContext>()
                   .As<DbContext>()
                   .InstancePerRequest();
            
            builder.RegisterType<DbFactory>()
                   .As<IDbFactory>()
                   .InstancePerRequest();
            
            RegisterRepositories(builder);

            //Set the dependency resolver to be Autofac.  
            container = builder.Build();

            return container;
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            var repositoryAssembly = Assembly.GetAssembly(typeof(GenericRepository<>));
            var types = repositoryAssembly.GetTypes()
                .Where(x => x.IsClass && x.IsPublic && x.IsGenericType && x.GetInterfaces()?.Length > 0)
                .Where(x => x.Name.Contains("Repository"))
                .Select(x => new
                {
                    Type = x,
                    Interface = x.GetInterfaces().FirstOrDefault()
                })
                .ToList();

            types.ForEach(x =>
            {
                builder.RegisterGeneric(x.Type).As(x.Interface).InstancePerRequest();
            });

            Assembly servicesAssembly = Assembly.GetAssembly(typeof(UserService));
            Assembly interfacesAssembly = Assembly.GetAssembly(typeof(IUserService));
            Assembly[] arr = { servicesAssembly, interfacesAssembly };
            builder.RegisterAssemblyTypes(arr)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}