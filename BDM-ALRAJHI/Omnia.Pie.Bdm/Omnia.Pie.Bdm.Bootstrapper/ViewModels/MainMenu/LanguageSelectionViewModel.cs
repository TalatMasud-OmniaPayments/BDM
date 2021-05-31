namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Reflection;
	using System.Windows.Input;

	public class LanguageSelectionViewModel : BaseViewModel, ILanguageSelectionViewModel
	{
		public LanguageSelectionViewModel()
		{
			LanguageObserver.Language = Language.English;
		}

		private ILanguageObserver LanguageObserver { get; }

		public LanguageSelectionViewModel(ILanguageObserver languageObserver)
		{
			LanguageObserver = languageObserver;
			ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public Action StartAction { get; set; }

		private ICommand selectLanguageCommand;
		public ICommand SelectLanguageCommand
		{
			get
			{
				if (selectLanguageCommand == null)
				{
					selectLanguageCommand = new DelegateCommand<Language>(
						x =>
						{
							LanguageObserver.Language = x;
							StartAction?.Invoke();
						});
				}

				return selectLanguageCommand;
			}
		}

		public string ApplicationVersion { get; set; } 

		public void Dispose()
		{

		}
	}
}