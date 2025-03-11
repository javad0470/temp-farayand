using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IViewModelBase
    {
        IView View { get; set; }
    }
}
