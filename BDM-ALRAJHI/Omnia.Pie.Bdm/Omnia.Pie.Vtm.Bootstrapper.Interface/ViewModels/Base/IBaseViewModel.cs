namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;

	public interface IBaseViewModel : IDisposable
	{
		Action BackAction { get; set; }
		Action CancelAction { get; set; }
		Action DefaultAction { get; set; }
		
		bool BackVisibility { get; set; }
		bool CancelVisibility { get; set; }
		bool DefaultVisibility { get; set; }
	}
}