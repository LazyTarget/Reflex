using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reflex.Framework.Diagnostics;

namespace Reflex.Framework
{
	public class AppFrame
	{
		public AppFrame()
		{

		}

		public virtual IConfigurationRoot SetupConfiguration()
		{
			var configurationBuilder = new ConfigurationBuilder()
				//.AddJsonFile("appsettings.json", true)
				//.AddEnvironmentVariables()
				.AddCommandLine(Environment.GetCommandLineArgs());

			var configuration = configurationBuilder.Build();
			return configuration;
		}

		public virtual IServiceCollection SetupServices(IConfigurationRoot root = null, IServiceCollection services = null)
		{
			services = services ?? new ServiceCollection();
			root = root ?? SetupConfiguration();

			services.AddSingleton<IConfiguration>(root);
			services.AddSingleton<IConfigurationRoot>(root);

			new Registry.Framework().Register(services, root);

			return services;
		}

		public virtual IServiceProvider SetupProvider(IServiceCollection services)
		{
			var provider = services.BuildServiceProvider(true);
			return provider;
		}

		public virtual void Init(IServiceProvider provider)
		{
			provider.GetService<AssemblyListener>()?.Start();
		}
	}
}
