namespace Omnia.Pie.Supervisor.Shell.Service
{
	using Microsoft.Practices.Unity;

	public static class ServiceLocator
	{
        public static IUnityContainer Instance { get; set; }
	}
}