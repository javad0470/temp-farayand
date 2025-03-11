using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IWayAwrIfrm
    {
        string Title { get; }

        TypDson DsonType { get; }

        int FldCod { get; }

        TypWayAwrIfrm FldTypEtyCnvs { get; }

        TblAct ActSrc { get; }

        TblAct ActDst { get; }

        bool IsDson { get; set; }

        bool IsAdded { get; set; }

        IObjRst ObjRst { get; }

        TypEtyForCvsn EtyForCvsnTyp { get; }

    }
}
