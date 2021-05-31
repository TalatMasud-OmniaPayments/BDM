namespace Omnia.Pie.Vtm.Services
{
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Services.Interface;
	using System.Configuration;
	using System.Linq;

	internal class ContractExceptionManager : IContractExceptionManager
	{
		private static ContractExceptionSection ContractExceptionSection => (ContractExceptionSection)ConfigurationManager.GetSection(ContractExceptionSection.Name);

		public string GetContractException(string contract, string errorCode, string errorMessage)
		{
			return ContractExceptionSection
											.Elements.Cast<OperationElement>()
											.Where(x => x.Contract == contract || $"{x.Contract}Async" == contract)
											.SelectMany(o => o.Elements.OfType<ErrorElement>())
											.Where(e => !string.IsNullOrEmpty(e.Code) && string.Compare(e.Code, errorCode) == 0 || !string.IsNullOrEmpty(e.Message) && string.Compare(e.Message, errorMessage) == 0)
											.Select(e => e.Exception)
											.FirstOrDefault();
		}
	}
}