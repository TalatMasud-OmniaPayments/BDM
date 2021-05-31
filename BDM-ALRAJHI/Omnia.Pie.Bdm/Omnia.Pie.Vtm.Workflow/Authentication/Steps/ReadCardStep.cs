namespace Omnia.Pie.Vtm.Workflow.Authentication
{
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Devices.Interface.Exceptions;
	using Omnia.Pie.Vtm.Framework.Interface;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
    using System;
	using System.Threading.Tasks;
	using System.Web.Script.Serialization;

	public class ReadCardStep : WorkflowStep
	{
		private readonly ICardReader _cardReader;

		public ReadCardStep(IResolver container) : base(container)
		{
			_cardReader = _container.Resolve<ICardReader>();
		}

		public async Task ReadCardAsync()
		{
            _logger?.Info($"Execute Step: Read Card");

            SetCurrentStep(nameof(ReadCardStep));

			try
			{
				Context.Get<IAuthDataContext>().Card = await _cardReader?.ReadCardAsync(true);
				_container.Resolve<Common.Context.ISessionContext>().CardUsed = Context.Get<IAuthDataContext>().Card;
				//_logger?.Info(new JavaScriptSerializer().Serialize(Context.Get<IAuthDataContext>()?.Card));
			}
			catch (DeviceOperationCanceledException)
			{
				// Device throws error if reading was in progress at the moment of cancellation. Ignore the error
			}
			catch (DeviceMalfunctionException ex)
			{
				_logger.Warning($"Card reading was cancelled: {ex}");
				throw;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		public override void Dispose()
		{

		}
	}
}