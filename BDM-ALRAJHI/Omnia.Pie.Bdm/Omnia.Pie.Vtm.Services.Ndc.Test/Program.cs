namespace Omnia.Pie.Vtm.Services.Ndc.Test
{
	using Microsoft.Practices.EnterpriseLibrary.Logging;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Unity.Configuration;
	using Omnia.Pie.Vtm.Framework.Exceptions;
	using Omnia.Pie.Vtm.ServicesNdc.Interface;
	using System;
	using System.Threading.Tasks;
	using static System.Console;

	class Program
	{
		private static INdcService _ndcService;

		static void Main(string[] args)
		{
			var container = new UnityContainer();
			container.LoadConfiguration();

			var logWriterFactory = new LogWriterFactory();
			var logWriter = logWriterFactory.Create();
			Logger.SetLogWriter(logWriter, throwIfSet: true);

			_ndcService = container.Resolve<INdcService>();

			CallAllServices(container);

			ReadKey();
		}

		private static async void CallAllServices(UnityContainer container) => await ExecuteAsync(async () =>
		{
			//await ExecuteAsync(async () => await TestEIDAFlow());
			await ExecuteAsync(async () => await CashDepositCC());
			//await ExecuteAsync(async () => await CashWithdrawalDebitCard());
			//await ExecuteAsync(async () => await CashWithdrawalReversal());
		});

		static async Task CashDepositCC()
		{
			var registerCall = await _ndcService.CashDepositCCAsync("4584610169572338=20082015409285200000", new System.Collections.Generic.List<Devices.Interface.Entities.DepositDenomination>() { new Devices.Interface.Entities.DepositDenomination() { Denomination = 100, Quantity = 10 } }, "1000");
		}

		static async Task CashWithdrawalReversal()
		{
			var registercall = await _ndcService.CashWithdrawalReversalAsync();
		}

		static async Task PinValidation()
		{
			var registercall = await _ndcService.ValidatePinAsync("4584610169572338=20082015409285200000", ";6<:329=63>23=?;");
		}
		static async Task CashWithdrawalDebitCard()
		{
			try
			{				
				#region "CashWithdrawal Debit Card Ndc"
				
				WriteLine();
				WriteLine("----------------Executing Update Call Record Service- Debit Card-----------------");
				WriteLine();
				WriteLine("----------------Request Start----------------------");
				WriteLine($"customerIdentifier : {"229887"}");
				WriteLine("----------------Request End------------------------");

				var registercall = await _ndcService.CashWithdrawalDebitCardAsync("4584610169572338=20082015409285200000", ";6<:329=63>23=?;", "9D77B9BF30AA0FF523CCEB43F6586CCEC30EF3203258B82779FA6D2CEE2DE6E766DBC7CD486C8340ECD403D37EC46CCD7FBBDEFABFF560EA7DBA93C7D81445548F01F19000", "100", "IAAB A B");

				WriteLine();
				WriteLine("----------------Response Start----------------------");
				WriteLine(registercall?.AuthCode ?? "ResponseCode null");
				WriteLine("----------------Response End------------------------");

				#endregion
			}
			catch (ServiceException ex)
			{
				WriteLine(ex.GetBaseException().Message);
			}
		}

		static async Task TestEIDAFlow()
		{
			//var res = await _ndcService.CashDepositCCAsync("5174000009560003", new System.Collections.Generic.List<Devices.Interface.Entities.DepositDenomination>()
			//{
			//	new Devices.Interface.Entities.DepositDenomination()
			//	{
			//		Denomination = 20,
			//		Quantity = 4
			//	}
			//}, "20");
			//var res = CashWithdrawalDebitCard();
			var cards = await _ndcService.GetEIDACardListAsync("784198459306811");
			//var cardSelectedResponse = await _ndcService.CardSelectedAsync("784198459306811", "I");
			//var preCashWithdrawalResponse = await _ndcService.PreCashWithdrawalEIDAsync("784198459306811", "I", "9;:84673819<9201");
			//var actualCashWithdrawalResponse = await _ndcService.ActualCashWithdrawalEIDAsync("9;:84673819<9201", "100", "784198459306811", "I");
			//var pinResponse = await _ndcService.ValidateEIDPinAsync("784198459306811", "I", "9;:84673819<9201");
		}

		#region Private Helpers

		private static async Task ExecuteAsync(Func<Task> func)
		{
			try
			{
				await func();
			}
			catch (Exception e)
			{
				WriteLine();
				WriteLine(e);
				WriteLine();
			}
		}

		#endregion
	}
}