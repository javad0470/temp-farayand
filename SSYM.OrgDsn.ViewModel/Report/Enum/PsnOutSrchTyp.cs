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
    public enum PsnOutSrchTyp
    {
        [TypeAttribute(typeof(string))]
        [Display(Name = "نام")]
        Nam,


        [TypeAttribute(typeof(string))]
        [Display(Name = "نام خانوادگی")]
        Family,


        [TypeAttribute(typeof(TypActPsn))]
        [Display(Name = "نوع فعالیت")]
        ActType,

        [TypeAttribute(typeof(string))]
        [Display(Name = "موضوع فعالیت")]
        SubjAct,

    }
}
