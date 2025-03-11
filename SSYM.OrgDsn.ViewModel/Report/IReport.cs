using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public class ReportEventArgs : EventArgs
    {
        public System.Collections.IEnumerable Data { get; set; }
    }

    public interface IReport
    {
        event EventHandler<ReportEventArgs> ReportResultCreated;

        string ReportTitle { get; }
    }
}
