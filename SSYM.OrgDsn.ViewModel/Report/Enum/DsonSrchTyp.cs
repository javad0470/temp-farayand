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
    public enum DsonSrchTyp
    {
        //[TypeAttribute(typeof(string))]
        //[Display(Name = "ورودی/خروجی")]
        //InOut,

        [TypeAttribute(typeof(string))]
        [Display(Name = "جایگاه/سمت/نقش")]
        PosPstRol,
    }
}
