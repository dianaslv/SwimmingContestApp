using Autofac;
using Microsoft.Extensions.Logging;
using Networking.Network;
using NLog.Extensions.Logging;

#pragma warning disable 618

namespace Presentation
{
    public static class DependencyInjector
    {
        public static IContainer ServiceProvider { get; private set; }

        public static void InjectDependencies()
        {
            var builder = new ContainerBuilder();
            RegisterNLog(builder);
            RegisterNetwork(builder);
            RegisterForms(builder);
            ServiceProvider = builder.Build();
        }

        private static void RegisterNLog(ContainerBuilder builder)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddNLog().ConfigureNLog("NLog.config");
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }

        private static void RegisterForms(ContainerBuilder builder)
        {
            builder.RegisterType<LoginForm>().AsSelf().SingleInstance();
            builder.RegisterType<ContestViewForm>().AsSelf().SingleInstance();
        }

        private static void RegisterNetwork(ContainerBuilder builder)
        {
            builder.RegisterType<Client>().AsSelf().SingleInstance();
        }
    }
}