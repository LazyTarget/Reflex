using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Reflex.Framework.Diagnostics;

namespace Reflex.Framework
{
	public class AppFrame
	{
		public AppFrame()
		{

		}

		public IServiceCollection SetupServices(IServiceCollection services = null)
		{
			services = services ?? new ServiceCollection();

			new Registry.Framework().Register(services);
			return services;
		}

		public IServiceProvider SetupProvider(IServiceCollection services)
		{
			var provider = services.BuildServiceProvider(true);
			return provider;
		}

		public void Init(IServiceProvider provider)
		{
			provider.GetService<AssemblyListener>()?.Start();
		}
	}
}
