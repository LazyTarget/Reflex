using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Reflex.Framework.Diagnostics
{
	public class AssemblyListener
	{
		private readonly ILogger<AssemblyListener> _logger;

		public AssemblyListener(ILogger<AssemblyListener> logger)
		{
			_logger = logger ?? new NullLogger<AssemblyListener>();
		}
		

		public void Start()
		{
			Stop();
			AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_OnAssemblyLoad;
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_OnAssemblyResolve;
		}

		public void Stop()
		{
			AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomain_OnAssemblyLoad;
			AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_OnAssemblyResolve;
		}



		private void CurrentDomain_OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			_logger.LogInformation($"Loaded assembly: {args.LoadedAssembly}");
		}

		private Assembly CurrentDomain_OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			_logger.LogInformation($"Assembly resolve: '{args.Name}'");

			var assembly = Assembly.ReflectionOnlyLoad(args.Name);

			return assembly;
		}
	}
}
