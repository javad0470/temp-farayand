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
    public partial class TblWayIfrm_News : IWayIfrm
    {
        public string Title
        {
            get
            {
                return this.TblNew.FldTtlNews;
            }
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
            get { return TypWayAwrIfrm.News; }
        }


        public TblAct ActSrc
        {
            get
            {
                return this.TblNew.TblEvtRst.TblAct;
            }
            
        }

        public TblAct ActDst
        {
            get
            {
                if (this.TblWayAwr_News != null && this.TblWayAwr_News.TblEvtSrt != null)
                {
                    return this.TblWayAwr_News.TblEvtSrt.TblAct;
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
            get { return this.TblNew; }
        }


        public IWayAwr WayAwr
        {
            get { return this.TblWayAwr_News; }
        }



        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_News; }
        }


    }
}
