using System;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	public interface ITimedErrorViewModel : IBaseViewModel
	{
		void Type(ErrorType errorType);

        void StartTimer(TimeSpan interval);

        void StartTimer(TimeSpan interval, DateTime dt);

        Action ExpiredAction { get; set; }
    }
    
}