namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.ComponentModel;

	public class ErrorViewModel : ExpirableBaseViewModel, IErrorViewModel
	{
		public ErrorViewModel()
		{
			
		}

		public string ErrorMessage { get; set; }

		public void Type(ErrorType errorType)
		{
			ErrorMessage = Properties.Resources.ResourceManager.GetString($"Error{errorType}", Properties.Resources.Culture);
			OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage"));
		}

		public void Dispose()
		{

		}
	}
}