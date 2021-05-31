namespace Omnia.Pie.Vtm.Workflow.MainMenu
{
	using Omnia.Pie.Client.Journal.Interface.Extension;
	using Omnia.Pie.Vtm.Bootstrapper.Interface;
	using Omnia.Pie.Vtm.Devices.Interface;
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Services.Interface;
	using Omnia.Pie.Vtm.Services.Interface.Interfaces;
    using Omnia.Pie.Vtm.Workflow.Authentication;
    using Omnia.Pie.Vtm.Workflow.Common.Context;
	using Omnia.Pie.Vtm.Workflow.MainMenu.Steps;
	using System;

	internal class MainWorkflow : Workflow
	{
		private readonly ICardReader _cardReader;
		private readonly ICommunicationService _communicationService;
		private readonly IServiceManager _serviceManager;
        private readonly ILanguageObserver _languageObserver;

        private readonly LanguageSelectionStep _languageSelectionStep;
		
		public MainWorkflow(IResolver container) : base(container)
		{
			_cardReader = _container.Resolve<ICardReader>();
			_communicationService = _container.Resolve<ICommunicationService>();
			_serviceManager = _container.Resolve<IServiceManager>();
            _languageObserver = _container.Resolve<ILanguageObserver>();
            _languageSelectionStep = _container.Resolve<LanguageSelectionStep>();
		}

		
		public async void Execute()
		{
			DisposeSteps();
			_navigator.ClearStack();

			await _languageSelectionStep.ExecuteAsync();
            // removed menu selection screen
            //_serviceTypeSelectionStep.Execute();
            _serviceManager.Terminal.TerminalLanguage = (_languageObserver.Language == 0) ? "EN":"AR" ;
            var _flow = _container.Resolve<AuthenticationLoginWorkflow>();
            //_flow.Context.Get<IAuthDataContext>().isOnlineTran = _networkStatus.isConnected(); 
            _flow.Execute();

        }

	
		public override void Dispose()
		{
			// Dispose created resources and unsubscribe events
			
			//TODO need to test with some memory profiler
			GC.SuppressFinalize(this);
		}
	}
}