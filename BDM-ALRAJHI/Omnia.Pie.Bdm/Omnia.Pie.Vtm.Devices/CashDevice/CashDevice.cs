namespace Omnia.Pie.Vtm.Devices.CashDevice
{
	using System.Linq;
	using System.Windows.Forms;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Devices.Interface.Entities;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;

	public class CashDevice : Device, ICashDevice
	{
		readonly ICashAcceptor cashAcceptor;
		readonly ICashDispenser cashDispenser;
		internal readonly DeviceOperation<CashCassette> GetCashCassettesInfoOperation;

		public CashDevice(IDeviceErrorStore deviceErrorStore, ILogger logger, IGuideLights guideLights,
							IJournal journal, ICashAcceptor cashAcceptor, ICashDispenser cashDispenser)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			this.cashAcceptor = cashAcceptor;
			this.cashDispenser = cashDispenser;
			Operations.AddRange(new[] {
				GetCashCassettesInfoOperation = new DeviceOperation<CashCassette>(nameof(GetCashCassettesInfoOperation), Logger, Journal)
			});
		}

		protected override AxHost CreateAx() => null;
		protected override string GetDeviceStatus() => null;
		protected override int OpenSessionSync(int timeout) => DeviceResult.Ok;
		protected override int CloseSessionSync() => DeviceResult.Ok;

		public override DeviceStatus Status
		{
			get
			{
                //return (cashAcceptor.Status == DeviceStatus.Online &&
                //		cashDispenser.Status == DeviceStatus.Online) ? DeviceStatus.Online : DeviceStatus.NoDevice;

                return (cashAcceptor.Status == cashDispenser.Status) ? cashAcceptor.Status : DeviceStatus.HwError;

            }
		}

		public CashCassette GetCashCassettesInfo()
		{
			return GetCashCassettesInfoOperation.Start(() =>
			{
				var accepted = cashAcceptor.GetMediaInfo();
				var dispensed = cashDispenser.GetMediaInfo();
				return new CashCassette
				{
					DepositedCount = accepted.Sum(i => i.Count),
					RetractedCount = accepted.Where(i => i.Type == CashAcceptorUnitType.Retract.ToString()).Sum(i => i.Count),
					DispensedCount = dispensed.Sum(i => i.Count),
					RejectedCount = dispensed.Where(i => i.Type == CashAcceptorUnitType.Reject.ToString()).Sum(i => i.Count),
					Bill100Count = dispensed.Where(i => i.Value == 100).Sum(i => i.Count),
					Bill200Count = dispensed.Where(i => i.Value == 200).Sum(i => i.Count),
					Bill500Count = dispensed.Where(i => i.Value == 500).Sum(i => i.Count),
					Bill1000Count = dispensed.Where(i => i.Value == 1000).Sum(i => i.Count),
				};
			});
		}
	}
}