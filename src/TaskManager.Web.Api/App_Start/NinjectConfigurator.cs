using log4net.Config;
using Ninject;
using TaskManager.Common;
using TaskManager.Common.Logging;
using TaskManager.Common.TypeMapping;
using TaskManager.Web.Common;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using Ninject.Activation;
using Ninject.Web.Common;
using TaskManager.Data.SqlServer.Mapping;
using TaskManager.Web.Common.Security;
using TaskManager.Common.Security;

using TaskManager.Data.QueryProcessors;
using TaskManager.Data.SqlServer.QueryProcessors;
using TaskManager.Web.Api.AutoMappingConfiguration;
using TaskManager.Web.Api.MaintenanceProcessing;
using TaskManager.Web.Api.Security;
using TaskManager.Web.Api.InquiryProcessing;

namespace TaskManager.Web.Api
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        private void AddBindings(IKernel container)
        {
            container.Bind<IAddTaskMaintenanceProcessor>().To<AddTaskMaintenanceProcessor>().InRequestScope();

            ConfigureLog4net(container);
            ConfigureUserSession(container);
            ConfigureNHibernate(container);
            ConfigureAutoMapper(container);

            container.Bind<IDateTime>().To<DateTimeAdapter>().InSingletonScope();

            container.Bind<IAddTaskQueryProcessor>().To<AddTaskQueryProcessor>().InRequestScope();
            container.Bind<IBasicSecurityService>().To<BasicSecurityService>().InSingletonScope();

            container.Bind<ITaskByIdQueryProcessor>().To<TaskByIdQueryProcessor>().InRequestScope();
            container.Bind<IUpdateTaskStatusQueryProcessor>().To<UpdateTaskStatusQueryProcessor>().InRequestScope();
            container.Bind<IStartTaskWorkflowProcessor>().To<StartTaskWorkflowProcessor>().InRequestScope();
            container.Bind<ICompleteTaskWorkflowProcessor>().To<CompleteTaskWorkflowProcessor>().InRequestScope();
            container.Bind<IReactivateTaskWorkflowProcessor>().To<ReactivateTaskWorkflowProcessor>().InRequestScope();

            container.Bind<ITaskByIdInquiryProcessor>().To<TaskByIdInquiryProcessor>().InRequestScope();
        }

        private void ConfigureLog4net(IKernel container)
        {
            // we create a class because this is required to configure log4net.
            XmlConfigurator.Configure();
            var logManager = new LogManagerAdapter();
            container.Bind<ILogManager>().ToConstant(logManager);
        }

        private void ConfigureNHibernate(IKernel container)
        {
            var sessionFactory = Fluently.Configure()
            .Database(
                MsSqlConfiguration.MsSql2012.ConnectionString(
                    c => c.FromConnectionStringWithKey("TaskManagerDb")))
                          .CurrentSessionContext("web")
                          .Mappings(m => m.FluentMappings.AddFromAssemblyOf<TaskMap>())
                          .BuildSessionFactory();
            container.Bind<ISessionFactory>().ToConstant(sessionFactory);
            container.Bind<ISession>().ToMethod(CreateSession);
            container.Bind<IActionTransactionHelper>().To<ActionTransactionHelper>().InRequestScope();
        }

        private ISession CreateSession(IContext context)
        {
            var sessionFactory = context.Kernel.Get<ISessionFactory>();
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return sessionFactory.GetCurrentSession();
        }

        private void ConfigureUserSession(IKernel container)
        {
            var userSession = new UserSession();
            container.Bind<IUserSession>().ToConstant(userSession).InSingletonScope();
            container.Bind<IWebUserSession>().ToConstant(userSession).InSingletonScope();
        }

        private void ConfigureAutoMapper(IKernel container)
        {
            container.Bind<IAutoMapper>().To<AutoMapperAdapter>().InSingletonScope();

            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<StatusEntityToStatusAutoMapperTypeConfigurator>()
                        .InSingletonScope();
            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<StatusToStatusEntityAutoMapperTypeConfigurator>()
                        .InSingletonScope();
            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<UserEntityToUserAutoMapperTypeConfigurator>()
                        .InSingletonScope();
            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<UserToUserEntityAutoMapperTypeConfigurator>()
                        .InSingletonScope();
            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<NewTaskToTaskEntityAutoMapperTypeConfigurator>()
                        .InSingletonScope();
            container.Bind<IAutoMapperTypeConfigurator>()
                        .To<TaskEntityToTaskAutoMapperTypeConfigurator>()
                        .InSingletonScope();
        }
        
    }
}