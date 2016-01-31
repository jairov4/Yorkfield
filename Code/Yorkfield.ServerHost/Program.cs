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
				try
				{
					var log = container.Resolve<ILog>();
					Trace.Listeners.Add(new ConsoleTraceListener());
					var host1 = StartWcfServer<IServer, MainServer>(container);
					var host2 = StartWcfServer<ILog, LogServer>(container);
					var nancyHost = new NancyHost(new NancyBootstrapper(container), new Uri(Settings.Default.BaseAddress));
					log.Log(LogSeverity.Information, "Web host started");
					nancyHost.Start();
					Console.ReadKey();
					host1.Close();
					host2.Close();
					nancyHost.Stop();
					log.Log(LogSeverity.Information, "Web host stopped");
				}
				catch (Exception e)
				{
					Trace.WriteLine("Unexpected fatal error, the server was shutdown: " + e.Message);
				}
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
			var log = container.Resolve<ILog>();
			host.Opened += (s,e) => ServiceHost_Opened(host, log);
			host.Closed += (s,e) => ServiceHost_Closed(host, log);
			host.Open();
			return host;
		}

		private static void ServiceHost_Closed(ServiceHost host, ILog log)
		{
			log.Log(LogSeverity.Information, $"Service endpoint closed: {host.Description.ServiceType}");
		}

		private static void ServiceHost_Opened(ServiceHost host, ILog log)
		{
			var addrs = string.Join(", ", host.Description.Endpoints.Select(x => x.Address));
			log.Log(LogSeverity.Information, $"Service endpoint started: {host.Description.ServiceType}: {addrs}");
		}
	}
}