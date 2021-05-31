using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Omnia.Pie.Vtm.Workflow;

namespace Omnia.Pie.Vtm.Bootstrapper.Interface
{
    public interface INetworkObserver
    {
        //IDataContext Context { get; set; }
        //bool isTransactionStarted { get; set; }

        bool isConnected();

    }
}
