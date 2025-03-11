using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum ManualOrSoftware
    {
        [Display(Name = "دستی")]
        Manual = 1,

        [Display(Name = "نرم افزاری")]
        Software = 2
    }
}
