namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using System;
	using System.Threading;

	public interface IExpirableBaseViewModel : IBaseViewModel
	{
		void StartTimer(TimeSpan interval);
		void StartUserActivityTimer(CancellationToken cancellationToken);
		void StartTimer(TimeSpan interval, DateTime dt);
		Action ExpiredAction { get; set; }
	}
}