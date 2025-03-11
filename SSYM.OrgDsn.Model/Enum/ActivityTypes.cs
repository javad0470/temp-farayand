using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum ActivityTypes
    {
        [Display(Name = "انسانی-دستی")]
        Manual = 1,

        [Display(Name = "انسانی-نرم‏افزاری")]
        UserTask = 2,

        [Display(Name = "خودکار سخت افزاری")]
        ServiceTaskHard = 3,

        [Display(Name = "خودکار نرم افزاری")]
        ServiceTaskSoft = 4
    }
}
