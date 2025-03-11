using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum EvtSrtType
    {
        [Display(Name = "در مقاطع زمانی")]
        inSgmtTime = 1,

        [Display(Name = "پس از وقوع شرایط")]
        aftrCdnEvtSrt = 2,

        [Display(Name = "صرفا پس از آگاهی")]
        aftrAwareEvtSrt = 3,

        [Display(Name = "وقوع هر شرایطی پس از  فعالیت قبلی")]
        aftrAnyCdnEvtSrt = 4,

        [Display(Name = "وقوع شرایط خاص پس از فعالیت قبلی")]
        spcCdnEvtSrtAftr = 5,

        [Display(Name = "بروز خطا در فعالیت قبلی")]
        errAccurEvtSrt = 6,

        [Display(Name = "لغو فعالیت قبلی")]
        cancelEvtSrt = 7,

        [Display(Name = "وقوع شرایط خاص هنگام فعالیت قبلی")]
        spcCdnEvtSrt = 8,

        [Display(Name = "کسب آگاهی جدید پس از فعالیت قبلی")]
        getNewAwrAftr = 9,

        [Display(Name = "کسب آگاهی جدید هنگام فعالیت قبلی")]
        getNewAwrInnTim = 10
    }
}
