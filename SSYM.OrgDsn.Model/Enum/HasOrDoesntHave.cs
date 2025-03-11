using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum HasOrDoesntHave
    {
        [Display(Name = "دارد")]
        Has = 1,
        
        [Display(Name = "ندارد")]
        DoesntHave = 2
    }
}
