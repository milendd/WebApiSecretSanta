﻿using Autofac;
using Autofac.Integration.WebApi;
using SantaSystem.Data;
using SantaSystem.Data.Repositories;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;

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
            
            builder.RegisterGeneric(typeof(GenericRepository<>))
                   .As(typeof(IGenericRepository<>))
                   .InstancePerRequest();
            
            //Set the dependency resolver to be Autofac.  
            container = builder.Build();

            return container;
        }
    }
}