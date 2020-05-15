using System;
using System.Collections;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using Autofac;
using Networking.Network;

namespace Networking
{
    internal static class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {

            var serverProv = new BinaryServerFormatterSinkProvider {TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full};
            BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
            IDictionary props = new Hashtable();

            props["port"] = 55555;
            TcpChannel channel = new TcpChannel(props, clientProv, serverProv);
            ChannelServices.RegisterChannel(channel, false);
            
            
            DependencyInjector.InjectDependencies();
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs eventArgs)
            {
                QuitEvent.Set();
                eventArgs.Cancel = true;
            };
            new Thread(async () =>
            {
                var server = DependencyInjector.ServiceProvider.Resolve<Server>();
                await server.StartServer();
            }).Start();

            QuitEvent.WaitOne();
        }
    }
}