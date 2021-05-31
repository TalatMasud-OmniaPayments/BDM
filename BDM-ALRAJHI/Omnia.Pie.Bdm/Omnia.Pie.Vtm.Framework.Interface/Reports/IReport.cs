namespace Omnia.Pie.Vtm.Framework.Interface.Reports
{
	using System;
	using System.Collections.Generic;
	using System.Drawing.Printing;
	using System.IO;

	public interface IReport : IDisposable
	{
		Stream GetPage(int index);
		byte[] ExportToPdf();
		void Print();
		void Print(PaperSourceKind src);
		List<string> GetPaperSources();
	}
}