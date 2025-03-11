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
    public enum ActSrchTyp  
    {
        [TypeAttribute(typeof(int))]
        [Display(Name = "شناسه فعالیت")]
        CodAct,

        [TypeAttribute(typeof(string))]
        [Display(Name = "نام فعالیت")]
        NamAct,

        [TypeAttribute(typeof(HasOrDoesntHave))]
        [Display(Name = "وضعیت زیرمجموعه")]
        SubActStatus,

        [TypeAttribute(typeof(ActivityTypes))]
        [Display(Name = "نوع فعالیت")]
        TypeAct,

        [TypeAttribute(typeof(string))]
        [Display(Name = "مجری فعالیت")]
        PfrAct,

        [TypeAttribute(typeof(EvtSrtType))]
        [Display(Name = "رخداد آغازگر")]
        EvtSrtType,

        [TypeAttribute(typeof(EvtRstType))]
        [Display(Name = "رخداد نتیجه")]
        EvtRstType,

        [TypeAttribute(typeof(TypWayAwr))]
        [Display(Name = "نحوه آگاهی")]
        WayAwrType,


        [TypeAttribute(typeof(TypWayAwrIfrm))]
        [Display(Name = "نحوه آگاه سازی")]
        WayIfrmType,

        [TypeAttribute(typeof(string))]
        [Display(Name = "ورودی")]
        Input,

        [TypeAttribute(typeof(string))]
        [Display(Name = "خروجی")]
        Output,



        [TypeAttribute(typeof(string))]
        [Display(Name = "خبر دریافتی")]
        RcevNews,

        [TypeAttribute(typeof(string))]
        [Display(Name = "خبر ارسالی")]
        SentNews,

    }
}
