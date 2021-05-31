namespace Omnia.Pie.Vtm.Workflow.RequestLC.Steps
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.RequestLC.Context;
	using System.Threading.Tasks;

	public class SendSmsStep : WorkflowStep
	{
		public SendSmsStep(IResolver container) : base(container)
		{

		}

		public async Task ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Send SMS");

            LoadWaitScreen();
			await Task.Delay(100);

			var _communicationService = _container.Resolve<ICommunicationService>();
			await _communicationService.SendSmsAsync(_container.Resolve<ISessionContext>().CustomerIdentifier, 
													SmsType.LcSms, Context.Get<IRequestLCContext>().TSNno, string.Empty);
		}

		public override void Dispose()
		{

		}
	}
}