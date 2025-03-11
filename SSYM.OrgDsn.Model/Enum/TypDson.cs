using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypDson
    {
        /// <summary>
        /// بدون ناهمسانی
        /// </summary>
        NoDson = 0,

        /// <summary>
        /// خروجی از فعالیت مشخص
        /// خروجی {0} را از شما دریافت میکند
        /// </summary>
        OutSpcf = 1,

        /// <summary>
        /// خروجی از فعالیت نامشخص
        /// خروجی {0} را از شما دریافت میکند
        /// </summary>
        OutUnspcf = 2,

        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت مشخص
        /// به صورت شفاهی توسط شما آگاه می شود
        /// </summary>
        SndOralFromSpcf = 3,

        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت نامشخص
        /// به صورت شفاهی توسط شما آگاه می شود
        /// </summary>
        SndOralFromUnspcf = 4,

        /// <summary>
        /// خبرارسالی از فعالیت مشخص
        /// خبر {0} را از شما دریافت می کند
        /// </summary>
        SndNewsFromSpcf = 5,

        /// <summary>
        /// خبرارسالی از فعالیت نامشخص
        /// خبر {0} را از شما دریافت می کند
        /// </summary>
        SndNewsFromUnspcf = 6,

        /// <summary>
        /// ورودی به فعالیت مشخص
        /// ورودی {0} را به شما ارسال میکند
        /// </summary>
        InSpcf = 7,

        /// <summary>
        /// ورودی به فعالیت نامشخص
        /// ورودی {0} را به شما ارسال میکند
        /// </summary>
        InUnspcf = 8,

        /// <summary>
        /// مطلب شفاهی دریافتی در فعالیت مشخص
        /// شما را به صورت شفاهی آگاه میکند
        /// </summary>
        RcvOralInSpcf = 9,

        /// <summary>
        /// مطلب شفاهی دریافتی در فعالیت نامشخص
        /// شما را به صورت شفاهی آگاه میکند
        /// </summary>
        RcvOralInUnspcf = 10,

        /// <summary>
        /// خروجی از فعالیت مشخص به فعالیت مشخص
        /// خروجی {0} را از شما دریافت نمیکند
        /// </summary>
        OutSpcfToSpcf = 11,

        /// <summary>
        /// خروجی از فعالیت مشخص به فعالیت نامشخص
        /// خروجی {0} را از شما دریافت نمیکند
        /// </summary>
        OutSpcfToUnspcf = 12,

        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت مشخص به فعالیت مشخص
        /// به صورت شفاهی توسط شما آگاه نمی شود
        /// </summary>
        SndOralFromSpcfToSpcf = 13,

        /// <summary>
        /// مطلب شفاهی ارسالی از فعالیت مشخص به فعالیت نامشخص
        /// به صورت شفاهی توسط شما آگاه نمی شود
        /// </summary>
        SndOralFromSpcfToUnspcf = 14,

        /// <summary>
        /// خبر دریافتی به فعالیت مشخص از فعالیت مشخص
        /// خبر {0} را ارسال نمی کند
        /// </summary>
        RcvNewsToSpcfFromSpcf = 15,

        /// <summary>
        /// خبر دریافتی به فعالیت مشخص از فعالیت نامشخص
        /// خبر {0} را ارسال نمی کند
        /// </summary>
        RcvNewsToSpcfFromUnspcf = 16,

        /// <summary>
        /// ورودی به فعالیت مشخص از فعالیت مشخص
        /// ورودی {0} را به شما ارسال نمیکند
        /// </summary>
        InSpcfFromSpcf = 17,

        /// <summary>
        /// ورودی به فعالیت مشخص از فعالیت نامشخص
        /// ورودی {0} را به شما ارسال نمیکند
        /// </summary>
        InSpcfFromUnspcf = 18,

        /// <summary>
        /// مطلب شفاهی دریافتی به فعالیت مشخص از فعالیت مشخص
        /// شما را به صورت شفاهی آگاه نمیکند
        /// </summary>
        RcvOralToSpcfFromSpcf = 19,

        /// <summary>
        /// مطلب شفاهی دریافتی به فعالیت مشخص از فعالیت نامشخص
        /// شما را به صورت شفاهی آگاه نمیکند
        /// </summary>
        RcvOralToSpcfFromUnspcf = 20
    }
}
