using Microsoft.Practices.EnterpriseLibrary.Logging;
using Omnia.Pie.Vtm.Framework.Interface;
using Omnia.PIE.VTA.ViewModels;
using System.Collections.Generic;

namespace Omnia.PIE.VTA.Core.Model
{
    public class WorkflowTracking : BaseViewModel
    {
        List<string> FlowStepList = new List<string>();

        private static WorkflowTracking wfTracking;
        public static WorkflowTracking Instance()
        {
            if (wfTracking == null)
            {
                wfTracking = new WorkflowTracking();
            }

            return wfTracking;
        }

        private string _CurrentActiveFlow;
        public string CurrentActiveFlow
        {
            get
            {
                return _CurrentActiveFlow;
            }
            set
            {
                if (value != _CurrentActiveFlow)
                {
                    _CurrentActiveFlow = value;
                    OnPropertyChanged(() => CurrentActiveFlow);

                    FlowStepList.Add(CurrentActiveFlow);
                }
            }
        }

        //private string _CurrentActiveFlowPreviousStep;
        //public string CurrentActiveFlowPreviousStep
        //{
        //    get
        //    {
        //        return _CurrentActiveFlowPreviousStep;
        //    }
        //    set
        //    {
        //        if (value != _CurrentActiveFlowPreviousStep)
        //        {
        //            _CurrentActiveFlowPreviousStep = value;
        //            OnPropertyChanged(() => CurrentActiveFlowPreviousStep);
        //        }
        //    }
        //}

        //private string _CurrentActiveFlowCurrentStep;
        //public string CurrentActiveFlowCurrentStep
        //{
        //    get
        //    {
        //        return _CurrentActiveFlowCurrentStep;
        //    }
        //    set
        //    {
        //        if (value != _CurrentActiveFlowCurrentStep)
        //        {
        //            _CurrentActiveFlowCurrentStep = value;
        //            OnPropertyChanged(() => CurrentActiveFlowCurrentStep);

        //            FlowStepList.Add(CurrentActiveFlowCurrentStep);
        //        }
        //    }
        //}

        //private string _CurrentActiveFlowNextStep;
        //public string CurrentActiveFlowNextStep
        //{
        //    get
        //    {
        //        return _CurrentActiveFlowNextStep;
        //    }
        //    set
        //    {
        //        if (value != _CurrentActiveFlowNextStep)
        //        {
        //            _CurrentActiveFlowNextStep = value;
        //            OnPropertyChanged(() => CurrentActiveFlowNextStep);
        //        }
        //    }
        //}

        public void ManageWorkflowStepTracking(StatusEnum enumType)
        {
            Instance().CurrentActiveFlow = enumType.ToString();
        }

        public void EndOfWorkflow()
        {
            foreach (var item in FlowStepList)
            {
                Logger.Writer.Info(item);
            }

            Instance().CurrentActiveFlow = string.Empty;
        }
    }
}
