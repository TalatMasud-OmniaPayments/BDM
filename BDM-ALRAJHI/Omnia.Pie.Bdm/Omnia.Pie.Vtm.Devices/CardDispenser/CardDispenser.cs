namespace Omnia.Pie.Vtm.Devices.CardDispenser
{
	//using Omnia.Pie.Vtm.Devices.Interface;
	//using System;
	//using System.Threading.Tasks;
	//using Omnia.Pie.Client.Journal.Interface;
	//using System.Windows.Forms;
	//using Omnia.Pie.Vtm.Devices.Interface.Constants;
	//using AxNXCARDDISPENSERX_2Lib;
	//using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	//using Omnia.Pie.Vtm.Devices.Interface.Entities;
	//using Omnia.Pie.Vtm.DataAccess.Interface;
	//using Omnia.Pie.Vtm.Framework.Interface;
	//using Omnia.Pie.Vtm.Framework.Extensions;

	//public class CardDispenser : Device, ICardDispenser
	//{
	//	internal readonly DeviceOperation<bool> EjectCardAndWaitTakenOperation;
	//	internal readonly DeviceOperation<bool> DispenseCardOperation;

	//	internal readonly DeviceOperation<bool> RetainCardOperation;
	//	//internal readonly DeviceOperation<bool> WriteRawDataOperation;
	//	//internal readonly DeviceOperation<bool> ReadRawDataOperation;

	//	public CardDispenser(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights) :
	//		base(deviceErrorStore, logger, journal, guideLights)
	//	{
	//		Operations.AddRange(new DeviceOperation[] {
	//			EjectCardAndWaitTakenOperation = new DeviceOperation<bool>(nameof(EjectCardAndWaitTakenOperation), Logger, Journal),
	//			RetainCardOperation = new DeviceOperation<bool>(nameof(RetainCardOperation), Logger, Journal),
	//			DispenseCardOperation = new DeviceOperation<bool>(nameof(DispenseCardOperation), Logger, Journal),
	//			//WriteRawDataOperation = new DeviceOperation<bool>(nameof(WriteRawDataOperation), Logger, Journal),
	//			//ReadRawDataOperation= new DeviceOperation<bool>(nameof(ReadRawDataOperation), Logger, Journal),
	//		});
	//	}

	//	AxNXCardDispenserX_2 ax;
	//	protected override AxHost CreateAx() => ax = new AxNXCardDispenserX_2();
	//	protected override int OpenSessionSync(int timeout) => ax.OpenSessionSync(timeout);
	//	protected override int CloseSessionSync() => ax.CloseSessionSync();
	//	protected override string GetDeviceStatus() => ax.DeviceStatus;

	//	bool awaitTaken;

	//	public event EventHandler MediaChanged;
	//	public bool HasMediaInserted => throw new NotImplementedException();

	//	protected override void OnInitialized()
	//	{
	//		//ax.DeviceError += Ax_DeviceError;
	//		//ax.FatalError += Ax_FatalError;
	//		ax.Timeout += Ax_Timeout;

	//		ax.MediaRemoved += Ax_MediaRemoved;
	//		//ax.MediaDetected += Ax_MediaDetected;
	//		//ax.WriteComplete += Ax_WriteComplete;
	//		//ax.InvalidTrackData += Ax_InvalidTrackData;
	//		//ax.ReadComplete += Ax_ReadComplete;
	//		ax.RetainComplete += Ax_RetainComplete;
	//		ax.EjectComplete += Ax_EjectComplete;
	//	}

	//	protected override void OnDisposing()
	//	{
	//		//ax.DeviceError -= Ax_DeviceError;
	//		//ax.FatalError -= Ax_FatalError;
	//		ax.Timeout -= Ax_Timeout;

	//		ax.MediaRemoved -= Ax_MediaRemoved;
	//		//ax.MediaDetected -= Ax_MediaDetected;
	//		//ax.WriteComplete -= Ax_WriteComplete;
	//		//ax.ReadComplete -= Ax_ReadComplete;
	//		ax.RetainComplete -= Ax_RetainComplete;
	//		ax.EjectComplete -= Ax_EjectComplete;
	//	}

	//	//private void Ax_ReadComplete(object sender, EventArgs e)
	//	//{
	//	//	ReadRawDataOperation.Stop(true);
	//	//}

	//	//private void Ax_WriteComplete(object sender, EventArgs e)
	//	//{
	//	//	WriteRawDataOperation.Stop(true);
	//	//}

	//	//void Ax_FatalError(object sender, _DNXCardDispenserXEvents_FatalErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
	//	//void Ax_DeviceError(object sender, _DNXCardDispenserXEvents_DeviceErrorEvent e) => OnError(new DeviceMalfunctionException(e.action, e.result));
	//	//private void Ax_MediaDetected(object sender, _DNXCardDispenserXEvents_MediaDetectedEvent e)
	//	//{

	//	//}

	//	private void Ax_Timeout(object sender, EventArgs e)
	//	{
	//		//Journal.CardReaderTimeout();
	//		OnError(new DeviceTimeoutException(nameof(CardDispenser)));
	//	}

	//	private void Ax_MediaRemoved(object sender, EventArgs e)
	//	{
	//		OnUserAction();
	//		//Journal.CardReaderCardTaken();
	//		EjectCardAndWaitTakenOperation.Stop(true);
	//		RetainCardOperation.Stop(false);
	//	}

	//	private void Ax_ResetComplete(object sender, EventArgs e) =>
	//		ResetOperation.Stop(true);

	//	private void Ax_RetainComplete(object sender, EventArgs e)
	//	{
	//		RetainCardOperation.Stop(true);
	//		//Journal.CardReaderCardCaptured();
	//	}

	//	private void Ax_EjectComplete(object sender, EventArgs e)
	//	{
	//		if (!awaitTaken)
	//			try
	//			{
	//				EjectCardAndWaitTakenOperation.Stop(true);
	//			}
	//			finally
	//			{
	//				awaitTaken = false;
	//			}
	//	}

	//	public async Task DispenseCardAsync()
	//	{
	//		await DispenseCardOperation.StartAsync(() =>
	//		{
	//			//GuideLight.TurnOff();
	//			return ax.Dispense(1, 1, Timeout.Operation);
	//		});
	//	}

	//	public Task EjectCardAndWaitTakenAsync() => EjectCardAsync(true);
	//	async Task EjectCardAsync(bool awaitTaken)
	//	{
	//		await EjectCardAndWaitTakenOperation.StartAsync(() =>
	//		{
	//			this.awaitTaken = awaitTaken;
	//			return ax.Eject(Timeout.AwaitTaken);
	//		});
	//	}

	//	public async Task RetainCardAsync()
	//	{
	//		await RetainCardOperation.StartAsync(() =>
	//		{
	//			GuideLight.TurnOff();
	//			return ax.RetainCard(0);
	//		});
	//	}

	//	//public short GetMaxRetainCount() => ax.MaxRetainCount;
	//	public short GetRetainCount() => (short)ax.UnitRetainCount;

	//	public MediaUnit[] GetMediaInfo()
	//	{
	//		var mediaUnits = new[]
	//		{
	//			new MediaUnit
	//			{
	//				MediaDevice = this,
	//				Id = 1,
	//				Type = "RETRACT",
	//				Count = (int)ax.UnitRetainCount,
	//				TotalCount = (int)ax.UnitRetainCount
	//			}
	//		};

	//		foreach (var mediaUnit in mediaUnits)
	//		{
	//			Logger.Properties(mediaUnit);
	//		}

	//		return mediaUnits;
	//	}

	//	public void SetMediaInfo(int[] ids, int[] counts)
	//	{
	//		try
	//		{
	//			ax.SetCardUnitInfo();
	//		}
	//		catch (Exception ex)
	//		{
	//			Logger.Exception(new DeviceMalfunctionException($"{GetType().Name}.{nameof(SetMediaInfo)}:", ex));
	//			throw;
	//		}
	//	}

	//	//public async Task ReadRawDataAsync()
	//	//{
	//	//	await RetainCardOperation.StartAsync(() =>
	//	//	{
	//	//		GuideLight.TurnOff();
	//	//		return ax.ReadRawData("ISO1,ISO2,ISO3", Timeout.Operation);
	//	//	});
	//	//}

	//	//public async Task WriteRawDataAsync(string track1, string track2, string track3)
	//	//{
	//	//	await WriteRawDataOperation.StartAsync(() =>
	//	//	{
	//	//		GuideLight.TurnOff();
	//	//		return ax.WriteRawData("ISO1,ISO2,ISO3", track1 + "," + track2 + "," + track3);
	//	//	});
	//	//}
	//}
}