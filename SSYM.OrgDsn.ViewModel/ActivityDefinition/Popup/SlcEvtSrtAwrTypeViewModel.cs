using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcEvtSrtAwrTypeViewModel : PopupViewModel
    {
        public SlcEvtSrtAwrTypeViewModel()
        {
            Items = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 5).ToList();
        }
        public List<TblItmFixSfw> Items { get; set; }
    }
}
