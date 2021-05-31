namespace Omnia.Pie.Vtm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using System;

	public class AuthenticatedOffUsStandbyViewModel : BaseViewModel, IAuthenticatedOffUsStandbyViewModel
	{

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}