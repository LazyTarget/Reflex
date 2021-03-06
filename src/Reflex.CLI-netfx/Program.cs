﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reflex.Framework;
using Reflex.Framework.Commands.LoadInstantiateInvoke;

namespace Reflex.CLI_netfx
{
	class Program
	{
		private static readonly AppFrame AppFrame = new AppFrame();

		static void Main(string[] args)
		{
			var configuration = AppFrame.SetupConfiguration();
			var services = AppFrame.SetupServices(configuration);
			var provider = AppFrame.SetupProvider(services);
			var logger = provider.GetRequiredService<ILogger<Program>>();

			try
			{
				AppFrame.Init(provider);
				Run(provider);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error in Main()");
			}

			if (Environment.UserInteractive)
			{
				Console.WriteLine("Press [ENTER] to exit...");
				Console.ReadLine();
			}
		}

		static void Run(IServiceProvider provider)
		{
			var cmd = provider.GetRequiredService<LoadInstantiateInvokeCommand>();
			cmd.Run();
		}
	}
}
