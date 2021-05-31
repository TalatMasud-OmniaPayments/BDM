namespace Omnia.Pie.Vtm.Framework.Base
{
	using Microsoft.Reporting.WinForms;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Framework.Interface.Reports;
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Drawing.Printing;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Printing;
	using System.Reflection;
	using System.Text;

	internal abstract class BaseReport<TReportData> : IReport, IReportInternal
	{
		private const string DefaultTemplateNameFormat = "{0}.rdlc";
		private const string TemplateNameFormat = "{0}_{1}.rdlc";
		private const string DefaultCultureName = "en-US";
		private const string EmfDeviceInfoFormat = @"<DeviceInfo>
														<OutputFormat>EMF</OutputFormat>
														<PageWidth>{0}</PageWidth>
														<PageHeight>{1}</PageHeight>
														<MarginLeft>{2}</MarginLeft>
														<MarginTop>{3}</MarginTop>
														<MarginRight>{4}</MarginRight>
														<MarginBottom>{5}</MarginBottom>
													</DeviceInfo>";

		private LocalReport LocalReport { get; set; }
		private List<Stream> Streams { get; set; }
		private int CurrentPageIndex { get; set; }

		#region IReport

		public Stream GetPage(int index) => Streams?.ElementAtOrDefault(index);

		public byte[] ExportToPdf() => LocalReport.Render("PDF", CreateEmfDeviceInfo());

		public void Print()
		{
			var printerName = ReportsConfiguration.PrinterName;
			var printerSettings = GetPrinterSettings(printerName);

			var paperKind = ReportsConfiguration.PaperKind;
			var paperMargin = ReportsConfiguration.PaperMargin;
			var pageSettings = GetPageSettings(printerSettings, paperKind, paperMargin);

			PrintInternal(printerSettings, pageSettings);
		}

		public void Print(PaperSourceKind pprSource)
		{
			var printerName = ReportsConfiguration.PrinterName;
			var printerSettings = GetPrinterSettings(printerName);
			var paperMargin = ReportsConfiguration.PaperMargin;
			var pageSettings = GetPageSettings(printerSettings, PaperKind.Custom, paperMargin);
			var paperSource = new PaperSource { RawKind = (int)pprSource };

			pageSettings.PaperSource = paperSource;
			PrintInternal(printerSettings, pageSettings);
		}

		public List<string> GetPaperSources()
		{
			var lst = new List<string>();
			var printerName = ReportsConfiguration.PrinterName;
			var printerSettings = GetPrinterSettings(printerName);

			var p = printerSettings.PaperSources;

			foreach (var item in p)
			{
				lst.Add(item.ToString());
			}

			return lst;
		}

		#endregion IReport

		#region IReportInternal

		public void SetData(object reportData, CultureInfo culture)
		{
			LocalReport = new LocalReport();
			LocalReport.ReportEmbeddedResource = GetManifestResourceName(culture);
			InitializeReport(LocalReport, (TReportData)reportData);
			RenderReport();
		}

		#endregion IReportInternal

		protected virtual void InitializeReport(LocalReport report, TReportData reportData)
		{
			report.SetParameters(new ReportParameter("DateFormat", ReportsConfiguration.DateFormat));
		}

		#region GetManifestResourceName

		private string GetManifestResourceName(CultureInfo culture)
		{
			return GetManifestResourceName(GetType(), culture?.Name ?? DefaultCultureName);
		}

		private static string GetManifestResourceName(Type reportType, string cultureName)
		{
			return GetManifestResourceName(string.Format(TemplateNameFormat, reportType.Name, cultureName))
				  ?? GetManifestResourceName(string.Format(DefaultTemplateNameFormat, reportType.Name));
		}

		private static string GetManifestResourceName(string templateName)
		{
			return Assembly
							.GetExecutingAssembly()
							.GetManifestResourceNames()
							.FirstOrDefault(n => n.Contains("." + templateName));
		}

		#endregion GetManifestResourceName

		#region RenderReport

		// Routine to provide to the report renderer, in order to save an image for each page of the report.
		private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
		{
			var result = new MemoryStream();
			Streams.Add(result);
			return result;
		}

		private void RenderReport()
		{
			var deviceInfo = CreateEmfDeviceInfo();
			Warning[] warnings;
			Streams = new List<Stream>();
			LocalReport.Render("IMAGE", deviceInfo, CreateStream, out warnings);
			foreach (var stream in Streams)
			{
				stream.Position = 0L;
			}
		}

		private string CreateEmfDeviceInfo() => string.Format(EmfDeviceInfoFormat,
			ReportsConfiguration.PageSize.Split().Concat(
				ReportsConfiguration.PageMargin.Split()).ToArray());

		#endregion RenderReport

		#region IDisposable

		public void Dispose()
		{
			if (Streams != null)
			{
				foreach (var stream in Streams)
				{
					stream.Close();
				}

				Streams = null;
			}

			if (LocalReport != null)
			{
				LocalReport.Dispose();
				LocalReport = null;
			}
		}

		#endregion IDisposable

		#region PrintInternal

		private void PrintInternal(PrinterSettings printerSettings, PageSettings pageSettings)
		{
			if (Streams == null || Streams.Count == 0) throw new InvalidOperationException("No stream to print.");
			if (printerSettings == null) throw new ArgumentNullException(nameof(printerSettings));
			if (!printerSettings.IsValid) throw new InvalidPrinterException(printerSettings);

			CheckPrinter(printerSettings);

			var printDocument = new PrintDocument
			{
				PrintController = new StandardPrintController(),
				PrinterSettings = printerSettings,
				DefaultPageSettings = pageSettings ?? throw new ArgumentNullException(nameof(pageSettings))
			};

			printDocument.PrintPage += new PrintPageEventHandler(PrintPage);
			CurrentPageIndex = 0;
			printDocument.Print();
		}

		private void PrintPage(object sender, PrintPageEventArgs ev)
		{
			var pageImage = new Metafile(Streams[CurrentPageIndex]);

			// Adjust rectangular area with printer margins.
			var adjustedRect = new System.Drawing.Rectangle(
				ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
				ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
				ev.PageBounds.Width,
				ev.PageBounds.Height);

			// Draw a white background for the report.
			ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

			// Draw the report content.
			ev.Graphics.DrawImage(pageImage, adjustedRect);

			// Prepare for the next page. Make sure we haven't hit the end.
			CurrentPageIndex++;
			ev.HasMorePages = (CurrentPageIndex < Streams.Count);
		}

		private static PrinterSettings GetPrinterSettings(string printerName)
		{
			PrinterSettings result;

			var printers = PrinterSettings.InstalledPrinters
				.Cast<string>()
				.Select(p => new PrinterSettings { PrinterName = p })
				.Where(s => s.IsValid)
				.ToList();

			if (string.Compare(printerName, "default", ignoreCase: true) == 0)
			{
				result = printers.FirstOrDefault(p => p.IsDefaultPrinter);
			}
			else
			{
				result = printers.FirstOrDefault(p => string.Compare(p.PrinterName, printerName, ignoreCase: true) == 0);
			}

			return result;
		}

		private static PageSettings GetPageSettings(PrinterSettings printerSettings, PaperKind paperKind, Margins paperMargin)
		{
			if (printerSettings == null) throw new ArgumentNullException(nameof(printerSettings));
			if (!printerSettings.IsValid) throw new InvalidPrinterException(printerSettings);
			if (paperMargin == null) throw new ArgumentNullException(nameof(paperMargin));

			var paperSize = printerSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(ps => ps.Kind == paperKind);
			//if (paperSize == null) throw new InvalidOperationException($"{paperKind.ToString()} paper size is not supported by the printer.");

			return new PageSettings
			{
				PaperSize = paperSize,
				Margins = paperMargin,
				PrinterSettings = printerSettings
			};
		}

		private static void CheckPrinter(PrinterSettings printerSettings)
		{
			var printServer = new PrintServer();
			var printQueue = printServer.GetPrintQueue(printerSettings.PrinterName);

			if (printQueue == null) throw new InvalidOperationException("The printer is not found.");
			if (printQueue.HasPaperProblem) throw new InvalidOperationException("The printer is having an unspecified paper problem.");
			if (printQueue.IsInError) throw new InvalidOperationException("The printer or device is in an error condition.");
			if (printQueue.IsNotAvailable) throw new InvalidOperationException("The printer is not available.");
			if (printQueue.IsOffline) throw new InvalidOperationException("The printer is offline.");
			if (printQueue.IsOutOfMemory) throw new InvalidOperationException("The printer is out of memory.");
			if (printQueue.IsOutOfPaper) throw new InvalidOperationException("The printer needs to be reloaded with paper of the size required for the current job.");
			if (printQueue.IsPaperJammed) throw new InvalidOperationException("The current sheet of paper is stuck in the printer.");
			if (printQueue.IsServerUnknown) throw new InvalidOperationException("The printer is in an error state.");
		}

		#endregion PrintInternal
	}
}