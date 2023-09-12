using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using NTUST.BulkMail.EntityFramework.Infrastructure;
using NTUST.BulkMail.Web;
using NTUST.BulkMail.Repository;
using Microsoft.Owin.Security.OAuth;
using NTUST.BulkMail.Services;
using NTUST.BulkMail.Services.Interface;
using NTUST.BulkMail.EntityFramework;

namespace NTUST.BulkMail.Web
{
    /// <summary>
    ///     Autofac DI 注入設定
    /// </summary>
    public class AutofacConfig
    {



        /// <summary>
        ///     註冊DI注入物件資料
        /// </summary>
        public static IContainer Register()
        {
            // 容器建立者
            var builder = new ContainerBuilder();
            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // 註冊 Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();



            // WebAPIs
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            RegisterDependency(builder);

            // 建立容器
            var container = builder.Build();

            // 建立相依解析器
            // Set Up MVC Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // Set Up WebAPI Resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }

        private static void RegisterDependency(ContainerBuilder builder)
        {
            // ConnectionString
            //var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            // UnitOfWork
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            // Repositories
            builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(IRepository<,>));
            builder.RegisterAssemblyTypes(typeof(StaffmemberTempRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(typeof(PersonRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            //builder.RegisterAssemblyTypes(typeof(NotAccessCardRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            // Services

            builder.RegisterAssemblyTypes(typeof(BulkMailService).Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            // IOAuthAuthorizationServerProvider
            //builder.RegisterType<ApplicationOAuthProvider>().As<IOAuthAuthorizationServerProvider>().PropertiesAutowired().SingleInstance();
            //builder.RegisterType<DbHWHSession>().InstancePerRequest();
            // AutoMapper
            //var mapper = AutoMapperConfig.Configure();
            //builder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
        }
    }
}