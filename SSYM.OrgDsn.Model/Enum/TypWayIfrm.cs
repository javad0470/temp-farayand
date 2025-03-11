using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypWayIfrm
    {
        [Display(Name = "انتشار خبر")]
        News = 3,

        [Display(Name = "آگاه سازی شفاهی")]
        SbjOral = 2,

        [Display(Name = "ارسال خروجی")]
        Obj = 1,
    }
}
