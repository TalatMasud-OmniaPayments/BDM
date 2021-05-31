namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using Omnia.Pie.Vtm.Framework.Base;
	using System;
	using System.Collections.ObjectModel;

	public interface ITopVideoObserver
	{
		ObservableCollection<Step> Steps { get; set; }
		Step CurrentStep { get; set; }
		void StartVideos();
		void StopVideos();
		void ReduceVolume();
		void FullVolume();
		bool OnHold { get; set; }
	}

	public class Step : BindableBase
	{
		private string _displayName;
		public string DisplayName
		{
			get { return _displayName; }
			set { SetProperty(ref _displayName, value); }
		}

		private int _stepNumber;
		public int StepNumber
		{
			get { return _stepNumber; }
			set { SetProperty(ref _stepNumber, value); }
		}

		private bool _isCurrentStep;
		public bool IsCurrentStep
		{
			get { return _isCurrentStep; }
			set { SetProperty(ref _isCurrentStep, value); }
		}

		private ObservableCollection<AssociatedAdd> _associatedAdds;
		public ObservableCollection<AssociatedAdd> AssociatedAdds
		{
			get { return _associatedAdds; }
			set { SetProperty(ref _associatedAdds, value); }
		}
	}

	public class AssociatedAdd : BindableBase
	{
		private Uri _source;
		public Uri Source
		{
			get { return _source; }
			set { SetProperty(ref _source, value); }
		}

		private int _durationSeconds;
		public int DurationSeconds
		{
			get { return _durationSeconds; }
			set { SetProperty(ref _durationSeconds, value); }
		}
	}
}