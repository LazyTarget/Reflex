using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Reflex.Browser
{
	public class LogHelper
	{
		public static LogLevel? LEVEL_ON_EXCEPTION = null;
		public static LogLevel? LEVEL_ON_FUSION_LOG = LogLevel.Trace;

		private readonly ILogger _logger;

		public LogHelper(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger(GetType());
		}

		public void Log(object message, LogLevel? logLevel = null, LogLevelDetect? logLevelDetect = null, bool format = true, bool logEmpty = true)
		{
			string formatted;
			if (message is Exception ex)
			{
				if (!logLevel.HasValue)
					logLevel = LEVEL_ON_EXCEPTION;
				formatted = LogException(ex, logLevel);
			}
			else
			{
				formatted = message?.ToString();
			}


			if (string.IsNullOrWhiteSpace(formatted) && !logEmpty)
			{
				return;
			}

			if (!logLevelDetect.HasValue)
				logLevelDetect = LogLevelDetect.DetectIfNotSet;
			var detectLogLevel =
				logLevelDetect == LogLevelDetect.None
					? false
					: logLevelDetect == LogLevelDetect.AlwaysDetect
						? true
						: logLevelDetect == LogLevelDetect.DetectIfNotSet && logLevel.HasValue;

			if (detectLogLevel && !string.IsNullOrWhiteSpace(formatted))
			{
				if (formatted.StartsWith("WARN") ||
					formatted.StartsWith("WRN"))
					logLevel = LogLevel.Warning;
				else if (formatted.StartsWith("ERROR") ||
						 formatted.StartsWith("ERR"))
					logLevel = LogLevel.Error;
			}

			if (!logLevel.HasValue)
				logLevel = LogLevel.Information;
			_logger.Log(logLevel.Value, formatted, args: null);
		}

		public string LogException(Exception ex, LogLevel? logLevel = null)
		{
			var formatted = $"Exception to string: {ex}";
			var ext = ex.GetType().Name;

			if (ex is FileLoadException fileLoad)
			{
				formatted = null;
				Log($"{ext} at fileName: {fileLoad.FileName}", logLevel);

				Log($"FusionLog :BEGIN:", LogLevel.Trace);
				LogMultiline(fileLoad.FusionLog, LogLevel.Trace, LogLevelDetect.AlwaysDetect);
				Log($"FusionLog :END:", LogLevel.Trace);
			}
			if (ex is FileNotFoundException fileNotFound)
			{
				formatted = null;
				Log($"{ext} at fileName: {fileNotFound.FileName}", logLevel);

				Log($"FusionLog :BEGIN:", LogLevel.Trace);
				LogMultiline(fileNotFound.FusionLog, LogLevel.Trace, LogLevelDetect.AlwaysDetect);
				Log($"FusionLog :END:", LogLevel.Trace);
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

			return formatted;
		}


		public void LogMultiline(string message, LogLevel? logLevel = null, LogLevelDetect? logLevelDetect = null)
		{
			var lines = message.Split(Environment.NewLine.ToCharArray());
			foreach (var line in lines)
			{
				Log(line, logLevel: logLevel, logLevelDetect: logLevelDetect, format: true, logEmpty: true);
			}
		}

		public enum LogLevelDetect
		{
			None,
			DetectIfNotSet,
			AlwaysDetect
		}

	}
}