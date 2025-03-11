using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IWayAwr : IWayAwrIfrm
    {
        TblEvtSrt EvtSrt_Temp { get; set; }

        IWayIfrm WayIfrm { get; }

    }
}
