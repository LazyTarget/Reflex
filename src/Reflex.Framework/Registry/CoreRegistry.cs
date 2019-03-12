using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reflex.Framework.Diagnostics;

namespace Reflex.Framework.Registry
{
	public class CoreRegistry
	{
		public CoreRegistry()
		{
			
		}

		public void Register(IServiceCollection services)
		{
			services.AddLogging(l => l
				.AddConsole()
			);

			services.AddSingleton<LogHelper>();
			services.AddSingleton<AssemblyListener>();
		}
	}
}
