namespace Omnia.PIE.VTA
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using System;
	using System.Diagnostics;

	public static class LoggerExtentions
	{
		private const string GeneralCategory = "General";
		public static void Exception(this LogWriter logWriter, Exception e) => logWriter.Write(e, GeneralCategory, -1, 1, TraceEventType.Critical);
		public static void Info(this LogWriter logWriter, string message) => logWriter.Write(message, GeneralCategory, -1, 1, TraceEventType.Information);
	}
}