namespace Omnia.Pie.Supervisor.Shell.ViewModels
{
	using Omnia.Pie.Vtm.Framework.Base;

	public class ConfigurationItemViewModel : BindableBase
	{
		public string Title { get; set; }

		public string Key { get; set; }

		private string _value;
		public string Value
		{
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}
	}
}