namespace Omnia.Pie.Vtm.Workflow
{
	using Omnia.Pie.Vtm.Framework.Interface;
	using Omnia.Pie.Vtm.Framework.Interface.Module;
	using Omnia.Pie.Vtm.Workflow.MainMenu;
	using System;

	public class Module : IModule
	{
		private IResolver _container { get; }
		private ILogger _logger;

		public Module(IResolver container)
		{
			_container = container ?? throw new ArgumentNullException(nameof(container));
			_logger = _container.Resolve<ILogger>();
		}

		public void Initialize()
		{
			try
			{
				//_logger?.Info("Creating new MainMenuWorkflow instance.");

				_container.Resolve<MainWorkflow>().Execute();
				
				//_logger?.Info("Created new MainMenuWorkflow instance.");
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}

			//_logger?.Info("Vtm.Workflow Initialized");
		}
	}
}