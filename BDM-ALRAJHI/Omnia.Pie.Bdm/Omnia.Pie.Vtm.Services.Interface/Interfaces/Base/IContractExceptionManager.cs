namespace Omnia.Pie.Vtm.Services.Interface
{
	public interface IContractExceptionManager
	{
		string GetContractException(string contract, string errorCode, string errorMessage);
	}
}