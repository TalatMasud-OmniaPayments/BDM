//using Omnia.Pie.Client.Infrastructure.Threading;
using System;
using System.Threading;

namespace Omnia.Pie.Supervisor.Shell.Applications
{
	public class RequestListener
	{
		private readonly SynchronizationContext _synchonizationContext;
		private string _applicationName;
		//private InteropSignal _startSignal;

		public RequestListener()
		{
			_synchonizationContext = SynchronizationContext.Current;
		}

		public void Configure(string applicationName)
		{
			if (_applicationName != null)
			{
				throw new InvalidOperationException($"Can't configure request listener for application [{applicationName}]. It is already configured for application [{_applicationName}].");
			}

			_applicationName = applicationName;
			//_startSignal = new InteropSignal($"Omnia.Pie.Start.{_applicationName}");
			//_startSignal.OnSignal += StartSignal_OnSignal;
		}

		public event EventHandler<ApplicationStartRequestedEventArgs> ApplicationStartRequested;

		private void StartSignal_OnSignal(object sender, EventArgs e)
		{
			//var startSignal = (InteropSignal)sender;
			//if (startSignal.Value)
			//{
			//	var eventArgs = new ApplicationStartRequestedEventArgs(_applicationName);

			//	if (_synchonizationContext == null)
			//	{
			//		ApplicationStartRequested?.Invoke(this, eventArgs);
			//	}
			//	else
			//	{
			//		_synchonizationContext.Post((state) => { ApplicationStartRequested?.Invoke(this, eventArgs); }, null);
			//	}
			//}
		}
	}
}