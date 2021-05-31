namespace Omnia.Pie.Vtm.Services.Interface
{
	public interface IServiceEndpoint
	{
		int TimeoutSeconds { get; }
		string BaseAddress { get; }
		string GetContractAddress(string contract);
	}
}