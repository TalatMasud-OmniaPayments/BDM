namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Bdm.Bootstrapper.Views.Call;
	using Omnia.Pie.Vtm.Communication.Interface;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	public class CallTellerViewModel : BaseViewModel, ICallTellerViewModel
	{
		public IESpaceTerminalCommunication Communicator { get; set; }
		private readonly ITopVideoObserver _topVideoService;
		private readonly IResolver _container;

		private readonly CustomerVideo terminalVideo;
		private readonly TellerVideo tellerVideo;

		public CallTellerViewModel(IResolver container, IESpaceTerminalCommunication communicator)
		{
			_container = container;
			Communicator = communicator;
			
			terminalVideo = _container.Resolve<CustomerVideo>();
			tellerVideo = _container.Resolve<TellerVideo>();
			_topVideoService = _container.Resolve<ITopVideoObserver>();
		}

		public async Task<CallResult> StartCall(CancellationTokenSource tokenSource)
		{
			_topVideoService.StopVideos();
			var result = CallResult.LoginFailed;

			try
			{
				result = await Communicator.CallTeller(tokenSource.Token, new CallRequest()
				{
					CallMode = CallMode.AudioAndVideo,
					LocalVideoHandle = terminalVideo.video.Handle.ToString(),
					RemoteVideoHandle = tellerVideo.video.Handle.ToString(),
				});

				switch (result)
				{
					case CallResult.NetWorkError:
					case CallResult.LoginFailed:
					case CallResult.NoTellerAvailable:
					case CallResult.NoTellerLoggedIn:
					case CallResult.CallCanceled:
						{
							_topVideoService.StartVideos();
							break;
						}
					case CallResult.Success:
						{
							tellerVideo.Show();
							terminalVideo.Show();
							terminalVideo.Activate();

							break;
						}
				}
			}
			catch (CallFailureException)
			{
				// Call failure
				_topVideoService.StartVideos();
			}
			catch (OperationCanceledException)
			{
				// Call cancelled
				_topVideoService.StartVideos();
			}
			catch (InvalidOperationException)
			{
				// Already in Call
				_topVideoService.StartVideos();
			}

			return result;
		}

		public void CallEndedStartVideos()
		{
			terminalVideo.Hide();
			tellerVideo.Hide();
			_topVideoService.StartVideos();
		}

		public void CancelCall(CancellationTokenSource tokenSource)
		{
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				tokenSource.Dispose();
				tokenSource = null;
			}

			_topVideoService.StartVideos();
		}

		public void Dispose()
		{
			
		}
	}
}