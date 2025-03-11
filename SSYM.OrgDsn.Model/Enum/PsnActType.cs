using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypActPsn
    {
        [Display(Name = "تولیدی")]
        Producing = 1,

        [Display(Name = "بازرگانی")]
        Commercial = 2,

        [Display(Name = "پیمانکاری")]
        Peymankar = 3,

        [Display(Name = "خدماتی")]
        Servicing = 4,

        [Display(Name = "سایر")]
        Other = 5
    }
}
