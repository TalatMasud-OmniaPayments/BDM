namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;

	public interface IActivityObserver
	{
		DateTime LastActivityTime { get; }
	}
}