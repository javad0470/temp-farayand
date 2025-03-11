using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcEvtRstAwrTypeViewModel : PopupViewModel
    {
        public SlcEvtRstAwrTypeViewModel()
        {
            Items = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 11).ToList();
        }
        public List<TblItmFixSfw> Items { get; set; }		
    }
}
