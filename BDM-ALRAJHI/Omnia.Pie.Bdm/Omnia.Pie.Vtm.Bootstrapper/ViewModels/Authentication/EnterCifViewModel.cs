namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.ComponentModel.DataAnnotations;

	public class EnterCifViewModel : ExpirableBaseViewModel, IEnterCifViewModel
	{
		private string cif;

		[Required(ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationRequired))]
		[MinLength(4, ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationInvalidCif))]
		[MaxLength(12, ErrorMessageResourceType = typeof(Properties.Resources), ErrorMessageResourceName = nameof(Properties.Resources.ValidationInvalidCif))]
		public string Cif
		{
			get { return cif; }
			set { SetProperty(ref cif, value); }
		}

		public void Dispose()
		{

		}
	}
}