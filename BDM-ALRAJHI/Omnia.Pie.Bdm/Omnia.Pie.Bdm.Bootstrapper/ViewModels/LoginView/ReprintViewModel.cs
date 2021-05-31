using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.Pie.Vtm.Services.Interface.Entities;
using Omnia.Pie.Vtm.Framework.DelegateCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnia.Pie.Bdm.Bootstrapper.ViewModels
{
    public class ReprintViewModel : ExpirableBaseViewModel, IReprintViewModel
    {

        public List<UserTransaction> transactions { get; set; }

        public int TotalTransactions { get; set; }
        public string accountNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Action<object> PrintAction { get; set; }

        private ICommand _printReceiptCommand;
        public ICommand PrintReceiptCommand
        {
            get
            {
                if (_printReceiptCommand == null)
                {
                    _printReceiptCommand = new DelegateCommand<UserTransaction>(
                        x =>
                        {
                            //LanguageObserver.Language = x;
                            //UserTransaction transaction = new UserTransaction();
                            PrintAction?.Invoke(x);
                        });
                }

                return _printReceiptCommand;
            }
        }

        public void Dispose()
        {

        }
    }
}
