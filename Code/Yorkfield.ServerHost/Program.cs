using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using Nancy;
using Nancy.Hosting.Self;
using Yorkfield.Core;
using Yorkfield.Server;

namespace Yorkfield.ServerHost
{
	internal class Program
	{
		private static void Main()
		{
			using (var container = Bootstrap())
			{
				Trace.Listeners.Add(new ConsoleTraceListener());
				var host1 = StartWcfServer<IServer, MainServer>(container);
				var host2 = StartWcfServer<ILog, LogServer>(container);
				var nancyHost = new NancyHost(new NancyBootstrapper(container), new Uri(Settings.Default.BaseAddress));
				Trace.WriteLine("Web host started");
				nancyHost.Start();
				Console.ReadKey();
				host1.Close();
				host2.Close();
				nancyHost.Stop();
				Trace.WriteLine("Web host stopped");
			}
		}

		private static IContainer Bootstrap()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<MainServer>()
				.SingleInstance()
				.As<IServer>();

			builder.RegisterType<LogServer>()
				.As<ILog>();

			builder.RegisterType<SqlConnectionFactory>()
				.As<IDbConnectionFactory>();

			builder.RegisterType<MainServerWebMonitor>()
				.UsingConstructor(typeof (IServer), typeof (string))
				.WithParameter(new PositionalParameter(1, Settings.Default.MainServerWebMonitorAddress))
				.As<INancyModule>()
				.As<MainServerWebMonitor>();
				

			builder.RegisterType<LogWebMonitor>()
				.UsingConstructor(typeof (ILog), typeof (string))
				.WithParameter(new PositionalParameter(1, Settings.Default.LogWebMonitorAddress))
				.As<INancyModule>()
				.As<LogWebMonitor>();
			return builder.Build();
		}

		private static ServiceHost StartWcfServer<TContract, TConcrete>(ILifetimeScope container)
		{
			var host = new ServiceHost(typeof(TConcrete));
			host.AddDependencyInjectionBehavior<TContract>(container);
			host.AddDataContractResolver(new MyDataContractResolver());
			host.Opened += ServiceHost_Opened;
			host.Closed += ServiceHost_Closed;
			host.Open();
			return host;
		}

		private static void ServiceHost_Closed(object sender, EventArgs e)
		{
			var host = (ServiceHost) sender;
			Trace.WriteLine($"Service endpoint closed: {host.Description.ServiceType}");
		}

		private static void ServiceHost_Opened(object sender, EventArgs e)
		{
			var host = (ServiceHost) sender;
			var addrs = string.Join(", ", host.Description.Endpoints.Select(x => x.Address));
			Trace.WriteLine($"Service endpoint started: {host.Description.ServiceType}: {addrs}");
		}
	}
}