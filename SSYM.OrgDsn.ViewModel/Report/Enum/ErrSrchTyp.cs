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
    public enum ErrSrchTyp
    {
        [TypeAttribute(typeof(string))]
        [Display(Name = "نام")]
        Nam,

        [TypeAttribute(typeof(string))]
        [Display(Name = "نوع خطا")]
        TypErr,

        [TypeAttribute(typeof(string))]
        [Display(Name = "نام فعالیت")]
        NamAct,
    }
}
