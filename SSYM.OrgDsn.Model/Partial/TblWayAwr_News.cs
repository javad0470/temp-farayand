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
    public partial class TblWayAwr_News : IWayAwr
    {
        bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string Title
        {
            get { return this.TblWayIfrm_News.TblNew.FldTtlNews; }
        }


        public TypDson DsonType
        {
            get { return (TypDson)this.FldStsDsonWayAwr; }
        }


        public int FldCod
        {
            get
            {
                return FldCodWayAwr;
            }
        }

        public TypWayAwrIfrm FldTypEtyCnvs
        {
            get
            {
                return TypWayAwrIfrm.News;
            }
        }


        public TblAct ActSrc
        {
            get
            {
                return this.TblWayIfrm_News != null ? this.TblWayIfrm_News.TblNew.TblEvtRst.TblAct : null;
            }

        }

        public TblAct ActDst
        {
            get
            {
                return this.TblEvtSrt.TblAct;
            }

        }

        private bool isDson;
        public bool IsDson
        {
            get { return isDson; }
            set
            {
                isDson = value;
                OnPropertyChanged("IsDson");
            }
        }

        //public bool IsDson
        //{
        //    get;
        //    set;
        //}

        public bool IsAdded
        {
            get;
            set;
        }

        public TblEvtSrt EvtSrt_Temp
        {
            get;
            set;
        }




        public IObjRst ObjRst
        {
            get { return this.TblWayIfrm_News != null ? this.TblWayIfrm_News.TblNew : null; }
        }


        public IWayIfrm WayIfrm
        {
            get { return this.TblWayIfrm_News; }
        }


        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_News; }
        }

    }
}
