using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class TblWayIfrm_Oral : IWayIfrm
    {
        public string Title
        {
            get { return "آگاه سازی شفاهی"; }
        }

        public TypDson DsonType
        {
            get { return (TypDson)this.FldStsDsonWayIfrm; }
        }


        public int FldCod
        {
            get { return this.FldCodWayIfrm; }
        }

        public TypWayAwrIfrm FldTypEtyCnvs
        {
            get { return TypWayAwrIfrm.SbjOral; }
        }


        public TblAct ActSrc
        {
            get
            {
                return this.TblSbjOral.TblEvtRst.TblAct;
            }

        }

        public TblAct ActDst
        {
            get
            {
                if (this.TblWayAwr_Oral != null && this.TblWayAwr_Oral.TblEvtSrt != null)
                {
                    return this.TblWayAwr_Oral.TblEvtSrt.TblAct;
                }

                return null;
            }

        }

        public bool IsDson
        {
            get;
            set;
        }

        public bool IsAdded
        {
            get;
            set;
        }

        public IObjRst ObjRst
        {
            get { return this.TblSbjOral; }
        }

        public IWayAwr WayAwr
        {
            get { return this.TblWayAwr_Oral; }
        }

        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_Oral; }
        }
    }
}
