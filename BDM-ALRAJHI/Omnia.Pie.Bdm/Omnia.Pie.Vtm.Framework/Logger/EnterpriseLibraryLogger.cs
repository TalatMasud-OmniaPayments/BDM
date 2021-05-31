namespace Omnia.Pie.Vtm.Framework.Logger
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Diagnostics;

	public class EnterpriseLibraryLogger : ILogger
	{
		private string Category { get; }

		public EnterpriseLibraryLogger()
		{
			Category = null;
		}

		public EnterpriseLibraryLogger(string category)
		{
			Category = category;
		}

		public void Error(string message)
		{
			Logger.Writer.Write(message, Category, -1, 1, TraceEventType.Error);
		}

		public void Exception(Exception e)
		{
			Logger.Writer.Write(e, Category, -1, 1, TraceEventType.Critical);
		}

		public void Info(string message)
		{
			Logger.Writer.Write(message, Category, -1, 1, TraceEventType.Information);
		}

		public void Warning(string message)
		{
			Logger.Writer.Write(message, Category, -1, 1, TraceEventType.Warning);
		}
	}

	internal static class LoggerExtensions
	{
		public static void LogError(this ILogger logger, object ex, string message, int code)
		{
			logger.Exception(new DeviceMalfunctionException(message, code));
		}
	}
}