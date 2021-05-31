namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;

	public interface ILanguageSelectionViewModel : IBaseViewModel
	{
		Action StartAction { get; set; }
		string ApplicationVersion { get; set; }
	}
}