using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reflex.Framework.Abstractions.Attributes;

namespace Reflex.Framework.Commands.LoadInstantiateInvoke
{
	[Command("lii")]
	[Command("LoadInstantiateInvoke")]
	public class LoadInstantiateInvokeCommand
	{
		private readonly ILogger<LoadInstantiateInvokeCommand> _logger;
		private readonly LogHelper _logHelper;
		private readonly LoadInstantiateInvokeOptions _options;

		public LoadInstantiateInvokeCommand(IOptions<LoadInstantiateInvokeOptions> options, ILogger<LoadInstantiateInvokeCommand> logger, LogHelper logHelper)
		{
			_logger = logger;
			_logHelper = logHelper;
			_options = options?.Value ?? new LoadInstantiateInvokeOptions();
		}

		public void Run()
		{
			try
			{
				var filePath = _options.FilePath;
				var typename = _options.TypeName;

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


				//if (!string.IsNullOrWhiteSpace(methodnames))
				{
					//var methodsNames = methodnames.Split(',');
					foreach (var name in _options.Methods)
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
		}

		private void Log(object message, LogLevel? logLevel = null, bool format = true)
		{
			_logHelper.Log(message, logLevel: logLevel, format: format);
		}

	}
}
