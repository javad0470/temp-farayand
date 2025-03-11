using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcEvtSrtType : PopupViewModel
    {
        public SlcEvtSrtType()
        {
            try
            {
                //کسب آگاهی جدید پس از فعالیت
                //کسب آگاهی جدید هنگام فعالیت
                // از لیست نهایی حذف می شود
                Items = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 2 && m.FldCodItm != 9 && m.FldCodItm != 10).ToList();
            }
            catch (Exception)
            {
            }
        }
        public List<TblItmFixSfw> Items { get; set; }
    }
}
