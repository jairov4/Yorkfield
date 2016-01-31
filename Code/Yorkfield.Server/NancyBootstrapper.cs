using System;
using System.Collections.Generic;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Diagnostics;
using Yorkfield.Core;
using static Yorkfield.Core.CodeContracts;

namespace Yorkfield.Server
{
	public class NancyBootstrapper : AutofacNancyBootstrapper
	{
		private readonly ILifetimeScope rootContainer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NancyBootstrapper"/> class.
		/// </summary>
		/// <param name="rootContainer">The root container.</param>
		public NancyBootstrapper(IContainer rootContainer)
		{
			RequiresNotNull(rootContainer);
			// Overrides the default container creation
			this.rootContainer = rootContainer;
		}

		protected override ILifetimeScope GetApplicationContainer()
		{
			return rootContainer;
		}
		
		protected override void RegisterRequestContainerModules(ILifetimeScope container, IEnumerable<ModuleRegistration> moduleRegistrationTypes)
		{
			// Overrides the automatic module registration, our registration is done in app bootstrap
		}

		protected override INancyModule GetModule(ILifetimeScope container, Type moduleType)
		{
			RequiresNotNull(container);
			RequiresNotNull(moduleType);
			object module;
			return container.TryResolve(moduleType, out module) ? (INancyModule) module : null;
		}
	}
}
