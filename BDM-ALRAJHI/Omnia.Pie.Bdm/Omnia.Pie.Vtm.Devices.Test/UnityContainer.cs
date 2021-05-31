using Microsoft.Practices.Unity.Configuration;

namespace Omnia.Pie.Vtm.Devices.Test
{
	public static class UnityContainer
	{
		static UnityContainer()
		{
			Container.LoadConfiguration();
		}

		public readonly static Microsoft.Practices.Unity.UnityContainer Container = new Microsoft.Practices.Unity.UnityContainer();
	}
}