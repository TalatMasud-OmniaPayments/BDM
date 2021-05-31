using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.Interface;
using Microsoft.Practices.Unity;

namespace Omnia.Pie.Vtm.Devices.Test.ViewModels
{
	public class ViewModel : BindableBase
	{
		protected ILogger _logger;

		public ViewModel()
		{
			_logger = UnityContainer.Container.Resolve<ILogger>();
		}

		public virtual string Id { get; set; }
		public virtual void Load() { }
	}
}