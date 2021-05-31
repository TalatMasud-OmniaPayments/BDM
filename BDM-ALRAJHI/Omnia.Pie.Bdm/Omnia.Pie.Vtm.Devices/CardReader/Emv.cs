namespace Omnia.Pie.Vtm.Devices.Emv
{
	using AxNHMWIEMVLib;
	using AxNXCardReaderXLib;
	using Omnia.Pie.Client.Journal.Interface;
	using Omnia.Pie.Vtm.DataAccess.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Constants;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public class Emv : Device
	{
		private readonly AxNXCardReaderX _cardReaderCom;
		private AxNHMwiEmv ax;

		public Emv(IDeviceErrorStore deviceErrorStore, ILogger logger, IJournal journal, IGuideLights guideLights, AxNXCardReaderX cardReaderCom)
			: base(deviceErrorStore, logger, journal, guideLights)
		{
			_cardReaderCom = cardReaderCom;
		}
		
		protected override IGuideLight GuideLight => null;
		protected override AxHost CreateAx() => ax = new AxNHMwiEmv();
		protected override int CloseSessionSync() => DeviceResult.Ok;
		protected override int OpenSessionSync(int timeout) => DeviceResult.Ok;
		protected override string GetDeviceStatus() => "DEVONLINE";

		public async Task<IEmvData> GetEmvDataAsync(int amount, string transactionType)
		{
			var emvData = new EmvData(new ProcessEmv(Logger, ax, _cardReaderCom));
			await emvData.InitializeAsync(amount, transactionType);
			return emvData;
		}
	}
}
