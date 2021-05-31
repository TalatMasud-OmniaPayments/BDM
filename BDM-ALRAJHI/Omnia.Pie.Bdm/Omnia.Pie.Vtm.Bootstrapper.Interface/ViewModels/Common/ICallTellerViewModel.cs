namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
	using Omnia.Pie.Vtm.Communication.Interface;
	using System.Threading;
	using System.Threading.Tasks;

	public interface ICallTellerViewModel : IBaseViewModel
	{
		IESpaceTerminalCommunication Communicator { get; set; }
		void CallEndedStartVideos();
		void CancelCall(CancellationTokenSource tokenSource);
		Task<CallResult> StartCall(CancellationTokenSource tokenSource);
	}
}