using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypWayAwr
    {
        [Display(Name = "دریافت خبر")]
        News = 3,

        [Display(Name = "آگاهی شفاهی")]
        SbjOral = 2,

        [Display(Name = "دریافت ورودی")]
        Obj = 1,
    }
}
