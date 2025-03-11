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
    public enum PrsSrchTyp
    {
        [TypeAttribute(typeof(int))]
        [Display(Name = "شناسه فرایند")]
        CodPrs,

        [TypeAttribute(typeof(string))]
        [Display(Name = "نام فرایند")]
        NamPrs,

        [TypeAttribute(typeof(SttPrs))]
        [Display(Name = "وضعیت فرایند")]
        StatusPrs,

        [TypeAttribute(typeof(string))]
        [Display(Name = "مالک فرایند")]
        NodPrs,
    }
}
