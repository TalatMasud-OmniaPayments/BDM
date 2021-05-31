

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Framework.Base;
	using System.Reflection;

	public class MainWindowViewModel : BindableBase
	{
		public string ApplicationVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
	}
}