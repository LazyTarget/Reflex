using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Reflex.Browser
{
	class Program
	{
		private static readonly Assembly REFLEX_ASSEMBLY;
		private static readonly LogHelper _logHelper;

		static Program()
		{
			REFLEX_ASSEMBLY = typeof(Program).Assembly;

			_logHelper = new LogHelper();
		}

		static void Main(string[] args)
		{
			var argsSupplied = args != null && args.Length > 0;
			if (!argsSupplied)
			{
				Log($"Usage: {REFLEX_ASSEMBLY}.exe/.dll {{FILEPATH}} {{TYPENAME}}");
			}

			var filePath = args?.Length > 0 ? args[0] : GetInputArg($"Enter {{FILEPATH}}: ");
			var typename = args?.Length > 1 ? args[1] : GetInputArg($"Enter {{TYPENAME}}: ");

			try
			{
				Log("ARGS:");
				Log($"FilePath: {filePath}");
				Log($"TypeName: {typename}");


				var loader = new AssemblyFileLoader();

				Log("Loading assembly...");
				var assembly = loader.Load(filePath);
				Log(assembly);

				Log("Getting type...");
				var type = assembly.GetType(typename, true);
				Log(type);


				Log("Creating instance...");
				var instance = Activator.CreateInstance(type);
				Log(instance);
			}
			catch (Exception ex)
			{
				Log($"Exception was thrown!");
				Log(ex);
			}

			if (Environment.UserInteractive)
			{
				Log("Press [ENTER] to exit...");
				Console.ReadLine();
			}
		}

		private static void Log(object message, bool format = true)
		{
			_logHelper.Log(message, format);
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
