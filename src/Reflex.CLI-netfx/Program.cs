using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			var services = AppFrame.SetupServices();
			var provider = AppFrame.SetupProvider(services);
			var logger = provider.GetRequiredService<ILogger<Program>>();

			try
			{
				Run(provider);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error in Main()");
			}
		}

		static void Run(IServiceProvider provider)
		{
			var cmd = provider.GetRequiredService<LoadInstantiateInvokeCommand>();
			cmd.Run();
		}
	}
}
