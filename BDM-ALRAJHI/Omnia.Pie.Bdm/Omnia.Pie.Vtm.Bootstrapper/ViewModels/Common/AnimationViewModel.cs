namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;

	public class AnimationViewModel : BaseViewModel, IAnimationViewModel
	{
		public AnimationViewModel()
		{
			
		}

		public AnimationType AnimationType { get; set; }
		public string DisplayMessage { get; set; }
		
		public void Type(AnimationType type)
		{
			AnimationType = type;
			DisplayMessage = Properties.Resources.ResourceManager.GetString($"Message{type}", Properties.Resources.Culture);
		}

		public void Dispose()
		{

		}
	}
}