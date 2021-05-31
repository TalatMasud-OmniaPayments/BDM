namespace Omnia.Pie.Vtm.Services
{
	using System;
	using System.Threading.Tasks;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Entities;
	using System.Security;

	public class ServiceManager : IServiceManager
	{
		public Acquirer Acquirer { get; set; }
		public Terminal Terminal { get; set; }
		public string SessionId { get; set; }
		public SecureString AccessToken { get; set; }
		public string TransactionNumber { get; set; }

		public void CloseTransaction()
		{
			
		}

		public Task<string> StartTransactionAsync()
		{
			throw new NotImplementedException();
		}
	}
}