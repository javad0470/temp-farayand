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
    public partial class TblWayAwr_Oral : IWayAwr
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
            get { return "آگاهی شفاهی"; }
        }

        public TypDson DsonType
        {
            get { return (TypDson)this.FldStsDsonWayAwr; }
        }



        public int FldCod
        {
            get { return FldCodWayAwr; }
        }

        public TypWayAwrIfrm FldTypEtyCnvs
        {
            get
            {
                return TypWayAwrIfrm.SbjOral;
            }
        }


        public TblAct ActSrc
        {
            get
            {
                if (TblWayIfrm_Oral != null)
                {
                    return this.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
                }
                return null;
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
            get { return this.TblWayIfrm_Oral.TblSbjOral; }
        }

        public IWayIfrm WayIfrm
        {
            get { return this.TblWayIfrm_Oral; }
        }



        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_Oral; }
        }
    }
}
