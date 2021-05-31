namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.DelegateCommand;
	using System;
	using System.Windows.Input;

	public class ActiveConfirmationViewModel : ExpirableBaseViewModel, IActiveConfirmationViewModel
	{
		public QuestionType QuestionType { get; set; }
		public string Question { get; set; }

		public void Type(QuestionType questionType)
		{
			QuestionType = questionType;
			Question = Properties.Resources.ResourceManager.GetString($"Question{questionType}", Properties.Resources.Culture);
		}

		public Action YesAction { get; set; }
		public Action NoAction { get; set; }

		private ICommand _yesCommand;
		public ICommand YesCommand
		{
			get
			{
				if (_yesCommand == null)
					_yesCommand = new DelegateCommand(
						() =>
						{
							StopTimer();
							YesAction?.Invoke();
						}
				);

				return _yesCommand;
			}
		}

		private ICommand _noCommand;
		public ICommand NoCommand
		{
			get
			{
				if (_noCommand == null)
					_noCommand = new DelegateCommand(
						() =>
						{
							StopTimer();
							NoAction?.Invoke();
						}
				);

				return _noCommand;
			}
		}

		public void Dispose()
		{
			
		}
	}
}