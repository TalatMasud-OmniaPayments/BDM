namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using Omnia.Pie.Vtm.Workflow.Common.Context;
	using System;
	using System.Threading.Tasks;

	public class SendSelectedCardStep : WorkflowStep
	{
		public SendSelectedCardStep(IResolver container) : base(container)
		{

		}

		public async Task<bool> ExecuteAsync()
		{
            _logger?.Info($"Execute Step: Send Selected Card");

            LoadWaitScreen();
			await Task.Delay(1000);

			var _ndcService = _container.Resolve<INdcService>();

			var result = await _ndcService.CardSelectedAsync(
				Context.Get<IAuthDataContext>().EIdNumber,
				Context.Get<IAuthDataContext>().SelectedCard?.CardFDK);//("784198459306811", "I");

			if (!result)
			{
				throw new Exception("Card selection failed.");
			}

			_journal.CardSelected(Context.Get<IAuthDataContext>().SelectedCard ?.CardNumber);

			return result;
		}

		public override void Dispose()
		{

		}
	}
}