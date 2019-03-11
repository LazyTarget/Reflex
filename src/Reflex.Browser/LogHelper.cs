using System;
using System.Diagnostics;
using System.IO;

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
				var ext = ex.GetType().Name;

				if (ex is FileLoadException fileLoad)
				{
					Log($"{ext} at fileName: {fileLoad.FileName}");
				}
				if (ex is FileNotFoundException fileNotFound)
				{
					Log($"{ext} at fileName: {fileNotFound.FileName}");
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

				formatted = $"Exception to string: {ex}";
			}


			Console.WriteLine(formatted);
			Debug.WriteLine(formatted);
		}
	}
}