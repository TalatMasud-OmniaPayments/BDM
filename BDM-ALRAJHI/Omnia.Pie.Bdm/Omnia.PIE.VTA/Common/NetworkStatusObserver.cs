using Omnia.PIE.VTA.Views.WF;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;
using Timer = System.Threading.Timer;

namespace Omnia.PIE.VTA.Common
{
    public class NetworkStatusObserver
    {
        public event EventHandler<EventArgs> VPNDropped;
        public event EventHandler<EventArgs> InternetDropped;
        public event EventHandler<EventArgs> InternetOn;

        //private NetworkInterface[] oldInterfaces;
        private Timer observerTimer;

        public NetworkStatusObserver()
        {
            observerTimer = new Timer(UpdateNetworkStatus, null, new TimeSpan(0, 0, 0, 0, 2000), new TimeSpan(0, 0, 0, 0, 2000));
            //oldInterfaces = GetNetworkInterfaces();
        }

        private void UpdateNetworkStatus(object o)
        {
            if (eSpaceMediaOcx.IsInternetAvailable())
            {
                InternetOn?.Invoke(this, null);

                //var newInterfaces = GetNetworkInterfaces();
                //bool hasChanges = false;

                //if (newInterfaces.Length != oldInterfaces.Length)
                //{
                //    hasChanges = true;
                //}

                //if (!hasChanges)
                //{
                //    for (int i = 0; i < oldInterfaces.Length; i++)
                //    {
                //        if ((newInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Ppp) &&
                //            (newInterfaces[i].NetworkInterfaceType != NetworkInterfaceType.Loopback))
                //        {
                //            if (oldInterfaces[i].Name != newInterfaces[i].Name ||
                //            oldInterfaces[i].OperationalStatus != newInterfaces[i].OperationalStatus)
                //            {
                //                hasChanges = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                //oldInterfaces = newInterfaces;

                //if (hasChanges)
                //{
                //    if (VPNDropped != null)
                //    {
                //        VPNDropped.Invoke(this, null);
                //    }
                //}
            }
            else
            {
                if (InternetDropped != null)
                {
                    InternetDropped.Invoke(this, null);
                }
            }
        }

        private NetworkInterface[] GetNetworkInterfaces()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<NetworkInterface> result = new List<NetworkInterface>();

            foreach (var item in interfaces)
            {
                if ((item.NetworkInterfaceType == NetworkInterfaceType.Ppp) && 
                    (item.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        public void StopObserver()
        {
            observerTimer.Dispose();
        }
    }
}
