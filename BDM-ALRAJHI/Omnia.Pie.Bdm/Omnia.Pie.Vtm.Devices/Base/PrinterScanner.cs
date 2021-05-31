namespace Omnia.Pie.Vtm.Devices
{
	using System;
	using System.Configuration;
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Client.Journal.Interface;

	public abstract class PrinterScanner : Device, IPrinter
	{
		public abstract string GetPaperStatus();

		public PrinterScanner(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			Operations.AddRange(new DeviceOperation[]
			{
				PrintTextOperation = new DeviceOperation<bool>(nameof(PrintTextOperation), Logger, Journal),
				PrintImageOperation = new DeviceOperation<bool>(nameof(PrintImageOperation), Logger, Journal),
				EjectOperation = new DeviceOperation<bool>(nameof(EjectOperation), Logger, Journal)
			});
		}

		public virtual PrinterStatus GetPrinterStatus()
		{
			return PrinterStatus.Unknown;
		}

		internal readonly DeviceOperation<bool> PrintTextOperation;
		internal readonly DeviceOperation<bool> PrintImageOperation;
		internal readonly DeviceOperation<bool> EjectOperation;

		protected abstract int sendRawData(string s);
		protected abstract int printForm(FileInfo image);
		protected abstract int controlMedia();
		protected abstract int reset();

		protected virtual string GetNibbleFromString(string s) => s;

		public Task PrintAsync(string text) => print(async () => await PrintTextOperation.StartAsync(() => sendRawData(GetNibbleFromString(text))));
		public Task PrintAsync(FileInfo image) => print(async () => await PrintImageOperation.StartAsync(() => printForm(image)));

		public Task PrintAsync(BitmapImage image) => print(async () =>
		{
			if (image != null)
			{
				using (var file = File.OpenWrite(imageFile))
				{
					var encoder = new BmpBitmapEncoder();
					encoder.Frames.Add(BitmapFrame.Create(image));
					encoder.Save(file);
					file.Close();
				}
				await PrintAsync(new FileInfo(imageFile));
			}
		});

		async Task print(Func<Task> f)
		{
			GuideLight?.TurnOn();
			await f();
		}

		// if more than two printerScanners has print image facility, the imageFile path should be distinguished between them
		string imageFile
		{
			get
			{
				try
				{
					var folder = ConfigurationManager.AppSettings["PrinterImageFolder"];
					if (!Directory.Exists(folder))
						Directory.CreateDirectory(folder);
					return Path.Combine(folder, "image.bmp");
				}
				catch (Exception ex)
				{
					throw new DeviceMalfunctionException($"{this}.{nameof(imageFile)}", ex);
				}
			}
		}

		public async Task PrintAndEjectAsync(string text)
		{
			try
			{
				await PrintAsync(text);
			}
			finally
			{
				await EjectAsync();
			}
		}

		public async Task PrintAndEjectAsync(FileInfo image)
		{
			try
			{
				await PrintAsync(image);
			}
			finally
			{
				await EjectAsync();
			}
		}

		public async Task PrintAndEjectAsync(BitmapImage image)
		{
			try
			{
				await PrintAsync(image);
			}
			finally
			{
				await EjectAsync();
			}
		}

		public async Task EjectAsync()
		{
			await EjectOperation.StartAsync(() => controlMedia());
			GuideLight?.TurnOff();
		}

		public override Task ResetAsync() => ResetOperation.StartAsync(() => reset());
	}
}