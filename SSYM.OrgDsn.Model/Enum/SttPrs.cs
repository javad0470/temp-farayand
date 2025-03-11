using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum SttPrs
    {
        [Display(Name = "تثبیت شده تایید شده")]
        ConsolidatedEndorsed = 3,

        [Display(Name = "تثبیت شده نایید نشده")]
        ConsolidatedNotEndorsed = 2,

        [Display(Name = "خام")]
        Raw = 1
    }
}
