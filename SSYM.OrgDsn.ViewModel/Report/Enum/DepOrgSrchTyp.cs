using SSYM.OrgDsn.Common;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.Report.Enum
{
    public enum OrgSrchTyp
    {
        [TypeAttribute(typeof(string))]
        [Display(Name = "نام سازمان")]
        Nam,

        [TypeAttribute(typeof(bool))]
        [Display(Name = "سازمان پدر دارد؟")]
        IsInner,
    }
}
