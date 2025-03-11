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
    public partial class TblWayAwr_RecvInt : IWayAwr
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
            get { return this.TblWayIfrm_SndOut.TblObj.FldNamObj; }
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
                return TypWayAwrIfrm.Obj;
            }
        }


        public TblAct ActSrc
        {
            get
            {
                if (this.TblWayIfrm_SndOut == null)
                {
                    return null;
                }
                return this.TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
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
            get
            {
                if (this.TblWayIfrm_SndOut != null)
                {
                    return this.TblWayIfrm_SndOut.TblObj;
                }
                else
                {
                    return null;
                }
            }
        }

        public IWayIfrm WayIfrm
        {
            get { return this.TblWayIfrm_SndOut; }
        }

        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_RecvInt; }
        }

    }
}
