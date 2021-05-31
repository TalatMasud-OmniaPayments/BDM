using Microsoft.Practices.Unity;
using Omnia.Pie.Client.Journal.Interface.Extension;
using Omnia.Pie.Supervisor.Shell.Service;
using Omnia.Pie.Vtm.Devices.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.ViewModels.Pages
{
	public abstract class PageWithPrintViewModel : PageViewModel
	{
		private readonly IReceiptPrinter _printer = ServiceLocator.Instance.Resolve<IReceiptPrinter>();
		private readonly IReceiptFormatter _receiptFormatter = ServiceLocator.Instance.Resolve<IReceiptFormatter>();
		
		protected async Task PrintAsync<T>(T receipt)
		{
			var formattedReceipt = await _receiptFormatter.FormatAsync(receipt);
			
			await _journal.WriteReceiptAsync(_receiptFormatter, receipt);
			await _printer.PrintAndEjectAsync(formattedReceipt);
		}
	}
}