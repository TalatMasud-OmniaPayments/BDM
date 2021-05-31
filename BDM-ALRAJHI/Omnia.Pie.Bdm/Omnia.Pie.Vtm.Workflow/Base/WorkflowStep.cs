namespace Omnia.Pie.Vtm.Workflow
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Media;
	using System.Reflection;

	public abstract class WorkflowStep : BaseFlow
	{
		public Action BackAction { get; set; }
		public Action CancelAction { get; set; }
		public Action DefaultAction { get; set; }
		public Action ExpiredAction { get; set; }

		protected WorkflowStep(IResolver container) : base(container)
		{

		}

		protected void SetCurrentStep(string step)
		{
			if (_videoService.Steps != null)
			{
				_videoService.Steps.Select(c => { c.IsCurrentStep = false; return c; }).ToList();
				var currentStep = _videoService.Steps.Where(x => x.DisplayName.Trim() == step).FirstOrDefault();
				if (currentStep != null)
				{
					currentStep.AssociatedAdds = GetAssociatedAdds(step);
					currentStep.IsCurrentStep = true;
					_videoService.CurrentStep = currentStep;
				}
			}
		}

		private ObservableCollection<AssociatedAdd> GetAssociatedAdds(string item)
		{
			// TODO get adds from Server and display on the screen.
			var _adds = new ObservableCollection<AssociatedAdd>();
			return _adds;
		}

		protected void PlayWelcomeSound()
		{
			_videoService.StopVideos();

			try
			{
				var codeBase = Assembly.GetExecutingAssembly().CodeBase;
				var uri = new UriBuilder(codeBase);
				var path = Uri.UnescapeDataString(uri.Path);
				path = Path.GetDirectoryName(path);
				var filePath = Path.Combine(path, "Resources\\Sounds\\slam.wav");

				var _player = new SoundPlayer(filePath);
				_player.Play();
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
			finally
			{
				_videoService.StartVideos();
			}
		}
	}
}