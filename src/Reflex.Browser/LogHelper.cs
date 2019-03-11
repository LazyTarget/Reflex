using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Reflex.Browser
{
	public class LogHelper
	{
		public LogHelper()
		{

		}

		public void Log(object message, bool format = true)
		{
			object formatted = message;
			if (message is Exception ex)
			{
				formatted = $"Exception to string: {ex}";
				var ext = ex.GetType().Name;

				if (ex is FileLoadException fileLoad)
				{
					Log($"{ext} at fileName: {fileLoad.FileName}");
					Log($"FusionLog: {fileLoad.FusionLog}");
				}
				if (ex is FileNotFoundException fileNotFound)
				{
					Log($"{ext} at fileName: {fileNotFound.FileName}");
					Log($"FusionLog: {fileNotFound.FusionLog}");
				}
				if (ex is ReflectionTypeLoadException reflectionTypeLoad)
				{
					Log($"{ext} with types: {reflectionTypeLoad.Types.Length}");

					Log($"LoaderExceptions :: {reflectionTypeLoad.LoaderExceptions.Length}");
					for (var i = 0; i < reflectionTypeLoad.LoaderExceptions.Length; i++)
					{
						var loadEx = reflectionTypeLoad.LoaderExceptions[i];
						Log($"LoadExc {i + 1}/{reflectionTypeLoad.LoaderExceptions.Length} :BEGIN:");
						Log(loadEx);
						Log($"LoadExc {i + 1}/{reflectionTypeLoad.LoaderExceptions.Length} :END:");
					}
				}


				if (ex is AggregateException agg)
				{
					Log($"AggregateExceptions :: {agg.InnerExceptions.Count}");
					for (var i = 0; i < agg.InnerExceptions.Count; i++)
					{
						var aggInner = agg.InnerExceptions[i];
						Log($"AggExc {i + 1}/{agg.InnerExceptions.Count} :BEGIN:");
						Log(aggInner);
						Log($"AggExc {i + 1}/{agg.InnerExceptions.Count} :END:");
					}
				}
				else if (ex.InnerException != null)
				{
					Log($"InnerException :BEGIN:");
					Log(ex.InnerException);
					Log($"InnerException :END:");
				}
			}


			Console.WriteLine(formatted);
			Debug.WriteLine(formatted);
		}
	}
}