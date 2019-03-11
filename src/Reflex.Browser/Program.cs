using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reflex.Framework;

namespace Reflex.Browser
{
	public static class Program
	{
		private static readonly Assembly REFLEX_ASSEMBLY;
		private static readonly LogHelper _logHelper;

		static Program()
		{
			REFLEX_ASSEMBLY = typeof(Program).Assembly;

			var services = BuildServices();
			var serviceProvider = services.BuildServiceProvider(true);
			_logHelper = serviceProvider.GetRequiredService<LogHelper>();
		}

		public static IServiceCollection BuildServices()
		{
			var services = new ServiceCollection();
			services.AddLogging(
				l =>
					l.AddConsole());

			services.AddSingleton<LogHelper>();

			return services;
		}

		public static void Main(string[] args)
		{
			var argsSupplied = args != null && args.Length > 0;
			if (!argsSupplied)
			{
				Log($"Usage: {REFLEX_ASSEMBLY}.exe/.dll {{FILEPATH}} {{TYPENAME}}");
			}

			var filePath = args?.Length > 0 ? args[0] : GetInputArg($"Enter {{FILEPATH}}: ");
			var typename = args?.Length > 1 ? args[1] : GetInputArg($"Enter {{TYPENAME}}: ");
			var methodnames = args?.Length > 2 ? args[2] : GetInputArg($"Enter {{METHODS}}: ");

			try
			{
				Log("ARGS:");
				Log($"FilePath: {filePath}");
				Log($"TypeName: {typename}");

				Type type;
				if (!string.IsNullOrWhiteSpace(filePath))
				{
					AppDomain domain = null;
#if !NETCOREAPP
					//domain = AppDomain.CreateDomain("LOADER");
#endif

					var loader = new AssemblyFileLoader(domain);
					Log("Loading assembly...");
					var assembly = loader.Load(filePath);
					Log(assembly);

					var definedTypes = assembly.DefinedTypes;
					Log($"Defined types: {definedTypes?.Count()}");

					Log("Getting type...");
					type = assembly.GetType(typename, true, true);
					Log(type);
				}
				else
				{
					Log("Getting type...");
					type = Type.GetType(typename, true, true);
					Log(type);
				}


				Log("Creating instance...");
				var instance = Activator.CreateInstance(type);
				var instanceStr = instance?.ToString();
				if (instanceStr == type.FullName)
					instanceStr = $"[{type.FullName}]";
				Log(instanceStr);


				if (!string.IsNullOrWhiteSpace(methodnames))
				{
					var methodsNames = methodnames.Split(',');
					foreach (var name in methodsNames)
					{
						var method = type.GetMethod(name);
						if (method == null)
							throw new MissingMethodException(type.FullName, name);

						Log($"Invoking method '{name}'... ");
						var res = method.Invoke(instance, null);
						Log($"Result :: {res}");
					}
				}
			}
			catch (Exception ex)
			{
				Log($"Exception was thrown!");
				Log(ex, LogLevel.Error);
			}

			if (Environment.UserInteractive)
			{
				Log("Press [ENTER] to exit...");
				Console.ReadLine();
			}
		}

		private static void Log(object message, LogLevel? logLevel = null, bool format = true)
		{
			_logHelper.Log(message, logLevel: logLevel, format: format);
		}

		private static string GetInputArg(string message, Func<string, bool> validate = null)
		{
			if (validate == null)
			{
				validate = (s) => !string.IsNullOrWhiteSpace(s);
			}

			string result = null;
			var valid = false;
			while (!valid)
			{
				Console.WriteLine(message);
				result = Console.ReadLine();
				valid = validate(result);
			}
			return result;
		}
	}
}
