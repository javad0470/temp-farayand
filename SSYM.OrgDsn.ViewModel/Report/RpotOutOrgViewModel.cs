using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public class RpotOutOrgViewModel : BaseReportSearchViewModel<PsnInSrchTyp>
    {
        public override string ReportTitle
        {
            get { return "گزارش سازمان های خارجی"; }
        }

    }
}
