using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace ESpaceCommunication.Test
{
	public static class Container
	{
		static Container()
		{
			_.LoadConfiguration();
		}
		public static IUnityContainer Instance { get; set; }
		public readonly static IUnityContainer _ = new UnityContainer();
	}
}
