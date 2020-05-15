using Autofac;
using Data;
using Data.DataContexts.Implementations;
using Data.DataContexts.Interfaces;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Networking.Handlers.Implementations;
using Networking.Handlers.Interfaces;
using Networking.Services.Implementations;
using Networking.Network;
using Networking.Services.Interfaces;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;
using IContainer = Autofac.IContainer;
using IDbDataContext = Data.IDbDataContext;

#pragma warning disable 618

namespace Networking
{
    public class DependencyInjector
    {
        public static IContainer ServiceProvider { get; private set; }

        public static void InjectDependencies()
        {
            var builder = new ContainerBuilder();
            RegisterNLog(builder);
            RegisterDataContexts(builder);
            RegisterRepositories(builder);
            RegisterServices(builder);
            RegisterNetwork(builder);
            ServiceProvider = builder.Build();
        }

        private static void RegisterNLog(ContainerBuilder builder)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog().ConfigureNLog("NLog.config");
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }

        private static void RegisterDataContexts(ContainerBuilder builder)
        {
            builder.RegisterInstance<IConfiguration>(new ConfigurationBuilder().AddJsonFile("appsetings.json").Build());
            builder.RegisterType<DataContext>().As<IDbDataContext>().SingleInstance();
            builder.RegisterType<EntityDataContext>().As<IEntityDataContext>().SingleInstance();
            builder.RegisterType<UserDataContext>().As<IUserDataContext>().SingleInstance();
            builder.RegisterType<ContestTaskDataContext>().As<IContestTaskDataContext>().SingleInstance();
            builder.RegisterType<EntryDataContext>().As<IEntryDataContext>().SingleInstance();
            builder.RegisterType<ParticipantDataContext>().As<IParticipantDataContext>().SingleInstance();
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<ContestTaskRepository>().As<IContestTaskRepository>().SingleInstance();
            builder.RegisterType<EntryRepository>().As<IEntryRepository>().SingleInstance();
            builder.RegisterType<ParticipantRepository>().As<IParticipantRepository>().SingleInstance();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
            builder.RegisterType<ContestTaskService>().As<IContestTaskService>().SingleInstance();
            builder.RegisterType<EntryService>().As<IEntryService>().SingleInstance();
            builder.RegisterType<ParticipantService>().As<IParticipantService>().SingleInstance();
        }

        private static void RegisterNetwork(ContainerBuilder builder)
        {
            builder.RegisterType<LoginHandler>().As<ILoginHandler>().SingleInstance();
            builder.RegisterType<SearchContestTasksHandler>().As<ISearchContestTasksHandler>().SingleInstance();
            builder.RegisterType<SearchParticipantsHandler>().As<ISearchParticipantsHandler>().SingleInstance();
            builder.RegisterType<SearchParticipantsJoin>().As<ISearchParticipantsJoin>().SingleInstance();
            builder.RegisterType<AddParticipantHandler>().As<IAddParticipantHandler>().SingleInstance();
            builder.RegisterType<AddEntryHandler>().As<IAddEntryHandler>().SingleInstance();
            builder.RegisterType<Server>().AsSelf().SingleInstance();
        }
    }
}