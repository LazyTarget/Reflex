using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reflex.Framework.Diagnostics;

namespace Reflex.Framework.Registry
{
	public class ConfigurationRegistry
	{
		public ConfigurationRegistry()
		{
			
		}

		public void Register(IServiceCollection services)
		{
			var configurationBuilder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true)
				.AddEnvironmentVariables()
				.AddCommandLine(Environment.GetCommandLineArgs());

			var configuration = configurationBuilder.Build();
			services.AddSingleton<IConfiguration>(configuration);
			services.AddSingleton<IConfigurationRoot>(configuration);
		}
	}
}
