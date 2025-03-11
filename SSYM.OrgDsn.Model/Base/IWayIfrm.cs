using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IWayIfrm : IWayAwrIfrm
    {
        //TblEvtRst EvtRst { get; }

        IWayAwr WayAwr { get; }
    }
}
