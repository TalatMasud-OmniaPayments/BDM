namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Windows.Input;

	public interface ICallRecordingConfirmationViewModel : IExpirableBaseViewModel
	{
		Action YesAction { get; set; }
		Action NoAction { get; set; }
		ICommand YesCommand { get; }
		ICommand NoCommand { get; }
	}
}