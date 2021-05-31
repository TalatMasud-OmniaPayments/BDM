using Omnia.Pie.Vtm.Services.Interface.Entities;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface IReprintViewModel : IExpirableBaseViewModel
    {
        Action<object> PrintAction { get; set; }
        List<UserTransaction> transactions { get; set; }
        int TotalTransactions { get; set; }
        string accountNumber { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        //void StartUserActivityTimer(CancellationToken token);
    }
}
