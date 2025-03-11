using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IObjRst
    {
        /// <summary>
        /// لیست فعالیت هایی که شی نتیجه به آنها وارد شده است
        /// </summary>
        List<TblAct> ActTarget { get; }

        /// <summary>
        /// فعالیت تولید کننده شی نتیجه
        /// </summary>
        TblAct ActSrc { get; }

        /// <summary>
        /// لیست رخدادهای آغازگر مقصد شی نتیجه
        /// </summary>
        List<TblEvtSrt> EvtSrtTarget { get; }

        /// <summary>
        /// مشخص میکند که ایا این شیء نتیجه
        /// نحوه آگاه سازی ناهمسان دارد یا خیر
        /// </summary>
        bool HasDson { get; set; }


        /// <summary>
        /// آیا به لیست ناهمسانی ها افزوده شده یا نه
        /// </summary>
        bool IsAdded { get; set; }


        /// <summary>
        /// رخداد نتیجه شیء نتیجه را برمیگرداند
        /// </summary>
        TblEvtRst EvtRst { get; }


        /// <summary>
        /// لیستی از نحوه های آگاهسازی این شیء نتیجه را برمیگرداند
        /// </summary>
        List<IWayIfrm> WayIfrms { get; }


        /// <summary>
        /// نوع شی نتیجه را به صورت یک رشته بر می گرداند
        /// </summary>
        string TypObj { get;}


        /// <summary>
        /// شکل متناظر با موجودیت در نمودار فرآیند
        /// </summary>
        INode Shp { get; set; }

        /// <summary>
        /// شکل دوم متناظر با موجودیت در نمودار فرآیند
        /// </summary>
        INode Shp1 { get; set; }

        /// <summary>
        /// شناسه شی نتیجه
        /// </summary>
        int CodObj { get; }


        /// <summary>
        /// نام شی نتیجه
        /// </summary>
        string Name { get; }
    }
}
