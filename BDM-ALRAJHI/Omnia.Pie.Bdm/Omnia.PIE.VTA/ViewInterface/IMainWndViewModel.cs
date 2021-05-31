using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omnia.PIE.VTA.ViewModels
{
    public interface IMainWndViewModel
    {

        IPageViewModel CurrentPage { get; set; }

        void ChangePage(string pageID);

        List<IPageViewModel> Pages { get; }

    }
}
