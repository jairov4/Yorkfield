﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Yorkfield.Client;
using Yorkfield.Core;

namespace Yorkfield.ClientHost
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var container = Bootstrap())
			{
				try
				{
					Trace.Listeners.Add(new ConsoleTraceListener());
					var client = container.Resolve<IClient>();
					client.Start(container.Resolve<IServer>());
				}
				catch (CommunicationException e)
				{
					Trace.WriteLine("Communication error, check connectivity: "+ e.Message);
				}
			}
		}

		private static IContainer Bootstrap()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<MainClient>()
				.As<IClient>();
			
			builder.RegisterGeneric(typeof (ChannelFactory<>))
				.UsingConstructor(typeof(string))
				.WithParameter(new PositionalParameter(0, "*"))
				.InstancePerLifetimeScope();

			builder.Register(CreateWcfClient<IServer>)
				.As<IServer>();

			builder.Register(CreateWcfClient<ILog>)
				.As<ILog>();
			
			return builder.Build();
		}

		private static T CreateWcfClient<T>(IComponentContext scope)
		{
			var factory = scope.Resolve<ChannelFactory<T>>();
			return factory.CreateChannel();
		}
	}
}
