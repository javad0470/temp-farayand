using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum DmnAcs
    {
        [Display(Name = "خودش")]
        Itself=0,
        [Display(Name = "با یک سطح پایین تر")]
        OneLvlLower=1,
        [Display(Name = "با دو سطح پایین تر")]
        TwoLvlLowr=2,
        [Display(Name = "با تمام زیر مجموعه ها")]
        AllOfItsChilds=100
    }
}
