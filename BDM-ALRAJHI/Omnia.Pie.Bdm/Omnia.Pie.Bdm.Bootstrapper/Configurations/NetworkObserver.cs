using Omnia.Pie.Vtm.Bootstrapper.Interface;
using Omnia.Pie.Client.Journal.Interface;
using Omnia.Pie.Vtm.Workflow;
using Omnia.Pie.Vtm.Workflow.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Omnia.Pie.Vtm.Framework.Interface;

namespace Omnia.Pie.Bdm.Bootstrapper.Configurations
{
    public class NetworkObserver: INetworkObserver
    {

        //public IDataContext Context { get; set; }
        private bool isNetworkAvailable = false;
        IJournal _journal;
        //public bool isTransactionStarted { get; set; } = false;

        public NetworkObserver(IResolver container)
        {
            _journal = container.Resolve<IJournal>();
            //Context = ctx;
            isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
            NetworkChange.NetworkAvailabilityChanged +=
                new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
            //NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }


        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                Console.WriteLine("Network available.");
                _journal.Write("Network available.");
                isNetworkAvailable = true;
            }
            else
            {
                Console.WriteLine("No Network.");
                _journal.Write("No Network.");
                isNetworkAvailable = false;
            }
        }

        public bool isConnected()
        {
            return isNetworkAvailable;
            //return false;
        }
    }
}
