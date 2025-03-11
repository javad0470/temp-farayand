using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypeStsDson
    {
        /// <summary>
        /// بدون ناهمسانی
        /// </summary>
        NoDson = 0,

        /// <summary>
        /// خروجی از فعالیت
        /// </summary>
        OutAct = 1,

        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت
        /// </summary>
        SndOral = 2,

        /// <summary>
        /// خبرارسالی از فعالیت
        /// </summary>
        SndNews = 3,

        /// <summary>
        /// ورودی به فعالیت
        /// </summary>
        InAct = 4,


        /// <summary>
        /// مطلب شفاهی دریافتی در فعالیت
        /// </summary>
        RcvOralIn = 5,


        /// <summary>
        /// خروجی از فعالیت مشخص به فعالیت
        /// </summary>
        OutActSpcf = 6,


        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت مشخص به فعالیت
        /// </summary>
        SndOralFromSpcf = 7,

        /// <summary>
        /// خبر دریافتی به فعالیت مشخص از فعالیت
        /// </summary>
        RcvNewsToSpcf = 8,

        /// <summary>
        /// ورودی به فعالیت مشخص از فعالیت
        /// </summary>
        InSpcfFrom = 9,

        /// <summary>
        /// مطلب شفاهی دریافتی به فعالیت مشخص از فعالیت
        /// </summary>
        RcvOralToSpcf = 10
    }
}
