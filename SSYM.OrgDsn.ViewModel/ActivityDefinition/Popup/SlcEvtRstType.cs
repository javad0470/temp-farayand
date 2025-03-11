using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcEvtRstType : PopupViewModel
    {
        public SlcEvtRstType()
        {
            try
            {
                Items = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 10 && m.FldCodItm != 3 && m.FldCodItm != 7).ToList();
            }
            catch (Exception)
            {
            }
        }
        public List<TblItmFixSfw> Items { get; set; }
    }
}
