namespace Omnia.Pie.Vtm.Services
{
	using Omnia.Pie.Vtm.Framework.Configurations;
	using Omnia.Pie.Vtm.Services.Interface;
	using System.Linq;
	using System.Configuration;

	public class ServiceEndpoint : IServiceEndpoint
	{
		private static EndpointsSection EndpointSection 
			=> (EndpointsSection)ConfigurationManager.GetSection(EndpointsSection.Name);

		public string BaseAddress => EndpointSection.BaseAddress;
		public int TimeoutSeconds => EndpointSection.TimeoutSeconds;

		public string GetContractAddress(string contract)
		{
			return EndpointSection
				.Elements.Cast<EndpointElement>()
				.Where(e => e.Contract == contract || $"{e.Contract}Async" == contract)
				.FirstOrDefault().Address;
		}
	}
}