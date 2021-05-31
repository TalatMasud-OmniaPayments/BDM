namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Base;
	using Omnia.Pie.Vtm.Framework.Configurations;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;

	public class TopVideoWindowViewModel : BindableBase, ITopVideoObserver
	{
		private ObservableCollection<Step> _steps;
		public ObservableCollection<Step> Steps
		{
			get { return _steps; }
			set
			{
				SetProperty(ref _steps, value);
			}
		}

		private Step _currentStep;
		public Step CurrentStep
		{
			get { return _currentStep; }
			set
			{
				if (value == _currentStep)
					return;

				if (_currentStep != null)
					_currentStep.IsCurrentStep = false;

				_currentStep = value;

				if (_currentStep != null)
					_currentStep.IsCurrentStep = true;

				RaisePropertyChanged(nameof(CurrentStep));
			}
		}

		private IEnumerable<Uri> _sources;
		public IEnumerable<Uri> Sources
		{
			get { return _sources; }
			set { SetProperty(ref _sources, value); }
		}

		private double _volume = 1.0;
		public double Volume
		{
			get { return _volume; }
			set { SetProperty(ref _volume, value); }
		}
		

		private bool _onHold;
		public bool OnHold
		{
			get { return _onHold; }
			set { SetProperty(ref _onHold, value); }
		}

		public void StartVideos()
		{
			StopVideos();

			var sourceFolder = TopVideoConfiguration.SourceFolder;

			if (string.IsNullOrEmpty(sourceFolder)) return;

			if (sourceFolder.StartsWith("\\"))
			{
				sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sourceFolder.TrimStart('\\'));
			}

			if (Directory.Exists(sourceFolder))
			{
				var sources = Directory
										.EnumerateFiles(sourceFolder)
										.Select(filePath => new Uri(filePath)).ToList();

				if (sources.Any())
				{
					Sources = sources;
				}
			}
		}

		public void StopVideos()
		{
			Sources = null;
		}

		public void ReduceVolume()
		{
			Volume = 0.0;
		}

		public void FullVolume()
		{
			Volume = 1.0;
		}
	}
}