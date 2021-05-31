namespace Omnia.Pie.Vtm.Devices.Interface
{
	using System.Threading.Tasks;

	public interface IEmvData
	{
		string ApplicationId { get; }
		string ApplicationLabel { get; }
		string CardNumber { get; }
		string Track2 { get; }
		string IccData { get; }
        bool MsrFallback { get; }
        string BuildIccData(string pinBlock);
		Task<bool> ValidateCardAutorizationAsync(string iccData);
	}
}