namespace Omnia.Pie.Vtm.Framework.Interface
{
	using System;

	public interface ILogger
	{
		void Info(string message);
		void Warning(string message);
		void Error(string message);
		void Exception(Exception e);
	}
}
