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
    public partial class TblSbjOral : IObjRst
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

        /// <summary>
        /// لیست فعالیتهایی که این مطلب شفاهی به آن ها وارد می شود
        /// </summary>
        public List<TblAct> ActTarget
        {
            get
            {
                if (this.TblWayIfrm_Oral != null)
                {
                    return PublicMethods.DetectActTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        /// <summary>
        /// لیست گره هایی که این مطلب شفاهی به آن ها وارد می شود
        /// </summary>
        public List<TblNod> NodTarget
        {
            get
            {
                if (this.TblWayIfrm_Oral != null)
                {
                    return PublicMethods.DetectNodTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        public bool HasDson
        {
            get;
            set;
        }

        public bool IsAdded
        {
            get;
            set;
        }

        public TblEvtRst EvtRst
        {
            get
            {
                return this.TblEvtRst;
            }
        }

        /// <summary>
        /// لیستی از نحوه های آگاهسازی این شیء نتیجه را برمیگرداند
        /// </summary>
        public List<IWayIfrm> WayIfrms
        {
            get
            {
                return this.TblWayIfrm_Oral.ToList<IWayIfrm>();
            }
        }


        public string TypObj
        {
            get { return "TblSbjOral"; }
        }

        public yWorks.yFiles.UI.Model.INode Shp
        {
            get;
            set;
        }

        public int CodObj
        {
            get { return this.FldCodSbjOral; }
        }

        public yWorks.yFiles.UI.Model.INode Shp1
        {
            get;
            set;
        }

        public TblAct ActSrc
        {
            get { return this.TblEvtRst.TblAct; }
        }


        public string Name
        {
            get { return ""; }
        }


        List<TblEvtSrt> _evtSrtTarget;

        public List<TblEvtSrt> EvtSrtTarget
        {
            get
            {
                if (_evtSrtTarget == null)
                {
                    _evtSrtTarget = new List<TblEvtSrt>();

                    foreach (TblWayIfrm_Oral item in this.TblWayIfrm_Oral)
                    {
                        _evtSrtTarget.Add(item.TblWayAwr_Oral.TblEvtSrt);
                    }
                }

                return _evtSrtTarget;
            }
        }
    }
}
