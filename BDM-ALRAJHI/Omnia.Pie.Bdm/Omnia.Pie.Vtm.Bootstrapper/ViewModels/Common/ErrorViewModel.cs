namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System.ComponentModel;

	public class ErrorViewModel : BaseViewModel, IErrorViewModel
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