using Omnia.Pie.Supervisor.Shell.Service;
using Microsoft.Practices.Unity;
using Omnia.Pie.Vtm.Devices.Interface.Entities;
using Omnia.Pie.Vtm.Framework.Base;
using Omnia.Pie.Vtm.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.Pie.Supervisor.Shell.Utilities
{
    public class CITSteps: BindableBase
    {
        public ILogger _logger;

        public ILogger Logger => _logger ?? (_logger = ServiceLocator.Instance.Resolve<ILogger>());

        public bool isSafeDoorOpened { get; set; }
        public bool isCitStarted { get; set; }
        public bool isReceiptPaperAvailable { get; set; }
        public bool shouldAutoValidateAndProcessCit { get; set; }
        //public bool isCassettesReplaced { get; set; }
        //public bool isSafedoorClosed { get; set; }
        public bool isStepFollowed { get; set; }
        public string oldCassettes { get; set; }
        //public bool isCassettesEmpty { get; set; }
        //public bool isCassettesMissing { get; set; }
        //public bool isStepFollowed { get; set; }

        //public MediaUnit[] oldMediaInfo;

        public CITSteps()
        {
            //isStepFollowed = false;


            Logger.Info("CITSteps Initialized");
            ResetSteps();
        }
        public void ResetSteps()
        {
            //oldMediaInfo = null;
            Logger.Info("CITSteps ResetSteps");

            if (!isCitStarted) { 
            this.isStepFollowed = this.isSafeDoorOpened = this.isCitStarted =this.shouldAutoValidateAndProcessCit = false;
            this.isReceiptPaperAvailable = true;
            this.oldCassettes = "";
            }
            //this.isStepFollowed = true;
        }
    }
}
