using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Reflex.Browser
{
	public class LogHelper
	{
		private readonly ILogger _logger;

		public LogHelper(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger(GetType());
		}

		public void Log(object message, LogLevel? logLevel = null, bool format = true, bool logEmpty = true)
		{
			string formatted;
			if (message is Exception ex)
			{
				if (!logLevel.HasValue)
					logLevel = LogLevel.Warning;

				formatted = $"Exception to string: {ex}";
				var ext = ex.GetType().Name;

				if (ex is FileLoadException fileLoad)
				{
					Log($"{ext} at fileName: {fileLoad.FileName}", logLevel);

					Log($"FusionLog :BEGIN:", logLevel);
					LogMultiline(fileLoad.FusionLog, logLevel);
					Log($"FusionLog :END:", logLevel);
				}
				if (ex is FileNotFoundException fileNotFound)
				{
					Log($"{ext} at fileName: {fileNotFound.FileName}", logLevel);

					Log($"FusionLog :BEGIN:", logLevel);
					LogMultiline(fileNotFound.FusionLog, logLevel);
					Log($"FusionLog :END:", logLevel);
				}
				if (ex is ReflectionTypeLoadException reflectionTypeLoad)
				{
					Log($"{ext} with types: {reflectionTypeLoad.Types.Length}", logLevel);

					Log($"LoaderExceptions :: {reflectionTypeLoad.LoaderExceptions.Length}", logLevel);
					for (var i = 0; i < reflectionTypeLoad.LoaderExceptions.Length; i++)
					{
						var loadEx = reflectionTypeLoad.LoaderExceptions[i];
						Log($"LoadExc {i + 1}/{reflectionTypeLoad.LoaderExceptions.Length} :BEGIN:", logLevel);
						Log(loadEx, logLevel);
						Log($"LoadExc {i + 1}/{reflectionTypeLoad.LoaderExceptions.Length} :END:", logLevel);
					}
				}


				if (ex is AggregateException agg)
				{
					Log($"AggregateExceptions :: {agg.InnerExceptions.Count}", logLevel);
					for (var i = 0; i < agg.InnerExceptions.Count; i++)
					{
						var aggInner = agg.InnerExceptions[i];
						Log($"AggExc {i + 1}/{agg.InnerExceptions.Count} :BEGIN:", logLevel);
						Log(aggInner, logLevel);
						Log($"AggExc {i + 1}/{agg.InnerExceptions.Count} :END:", logLevel);
					}
				}
				else if (ex.InnerException != null)
				{
					Log($"InnerException :BEGIN:", logLevel);
					Log(ex.InnerException, logLevel);
					Log($"InnerException :END:", logLevel);
				}
			}
			else
			{
				formatted = message?.ToString();
			}


			if (string.IsNullOrWhiteSpace(formatted) && !logEmpty)
			{
				return;
			}

			if (!logLevel.HasValue && string.IsNullOrWhiteSpace(formatted))
				logLevel = LogLevel.Information;

			if (!logLevel.HasValue)
			{
				if (formatted.StartsWith("WARN") ||
					formatted.StartsWith("WRN"))
					logLevel = LogLevel.Warning;
				else if (formatted.StartsWith("ERROR") ||
						 formatted.StartsWith("ERR"))
					logLevel = LogLevel.Error;
				else
					logLevel = LogLevel.Information;
			}
			_logger.Log(logLevel.Value, formatted, args: null);
		}

		public void LogMultiline(string message, LogLevel? logLevel = null)
		{
			var lines = message.Split(Environment.NewLine.ToCharArray());
			foreach (var line in lines)
			{
				Log(line, logLevel: logLevel, format: true, logEmpty: true);
			}
		}
	}
}