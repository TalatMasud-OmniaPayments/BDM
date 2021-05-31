namespace Omnia.Pie.Vtm.Services.Interface
{
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Security;
	using System.Threading.Tasks;

	public interface IServiceManager
	{
		Acquirer Acquirer { get; set; }
		Terminal Terminal { get; set; }
		string SessionId { get; set; }
		SecureString AccessToken { get; set; }
		string TransactionNumber { get; set; }
		Task<string> StartTransactionAsync();
		void CloseTransaction();
	}
}