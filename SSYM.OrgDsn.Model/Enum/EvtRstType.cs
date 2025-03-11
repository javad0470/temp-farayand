using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum EvtRstType
    {
        [Display(Name = "وقوع هر شرایطی هنگام فعالیت")]
        anyCdnEvtRst = 1,

        [Display(Name = "وقوع شرایط خاص پس از فعالیت")]
        spcCdnEvtRstAftr = 2,

        [Display(Name = "کسب آگاهی جدید پس از فعالیت")]
        getNewAwrAftrAct = 3,

        [Display(Name = "وقوع خطا هنگام فعالیت")]
        errAccurEvtRst = 4,

        [Display(Name = "لغو فعالیت")]
        cancelEvtRst = 5,

        [Display(Name = "وقوع شرایط خاص هنگام فعالیت")]
        spcCdnEvtRstInnTim = 6,

        [Display(Name = "کسب آگاهی جدید هنگام فعالیت")]
        getNewAwr = 7
    }
}
