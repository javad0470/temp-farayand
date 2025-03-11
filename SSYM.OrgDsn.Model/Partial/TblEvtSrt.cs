using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model
{
    public partial class TblEvtSrt : IEvt
    {
        private TblAct previousActivity;
        //private string previousActivityPerformer;
        private string previousActivityName;

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

        public TblEvtRst EvtRstEqualToEvtSrt
        {
            get
            {
                return DetectEvtRstEqualToEvtSrt_527();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private TblEvtRst DetectEvtRstEqualToEvtSrt_527()
        {
            foreach (IWayAwr item in AllWayAwr)
            {
                if (item.WayIfrm != null && item.WayIfrm.ObjRst.EvtRst != null)
                {
                    return item.WayIfrm.ObjRst.EvtRst;
                }
            }

            return null;
        }

        List<IWayAwr> _alWayAwr;

        /// <summary>
        /// 
        /// </summary>
        public List<IWayAwr> AllWayAwr
        {
            get
            {
                if (_alWayAwr == null)
                {
                    _alWayAwr = new List<IWayAwr>();

                    _alWayAwr.AddRange(this.TblWayAwr_News);

                    _alWayAwr.AddRange(this.TblWayAwr_Oral);

                    _alWayAwr.AddRange(this.TblWayAwr_RecvInt);
                }

                return _alWayAwr;
            }
        }

        //BPMNShapes.ShpBase shp;

        //public BPMNShapes.ShpBase Shp
        //{
        //    get { return shp; }
        //    set
        //    {
        //        shp = value;
        //        shp.Id = this.FldCodEvtSrt;
        //    }
        //}

        INode shp;

        public INode Shp
        {
            get { return shp; }
            set { shp = value; }
        }

        public string PreviousActivityName
        {
            get
            {
                if (this.PreviousActivity != null)
                {
                    using (BPMNDBEntities context = new BPMNDBEntities())
                    {
                        return previousActivityName = context.TblActs.SingleOrDefault(E => E.FldCodAct == this.PreviousActivity.FldCodAct).FldNamAct;
                    }
                }
                return string.Empty;
            }
            //get { return previousActivityName; }
            //set { previousActivityName = value; }
        }

        public string PreviousActivityPerformer
        {
            get
            {
                if (this.PreviousActivity != null)
                {
                    return PublicMethods.ActivityPerformerName_951(this.PreviousActivity.FldCodAct);
                }
                return string.Empty;
            }
            //set { previousActivityPerformer = value; }
        }

        public TblAct PreviousActivity
        {
            get
            {
                TblAct prevAct = null;

                if (previousActivity != null)
                {
                    return previousActivity;
                }

                if (this.TblWayAwr_RecvInt.Count > 0)
                {
                    foreach (var item in this.TblWayAwr_RecvInt)
                    {
                        if (item.TblWayIfrm_SndOut != null && item.TblWayIfrm_SndOut.TblObj != null)
                        {
                            prevAct = item.TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
                            break;
                        }
                    }
                }
                if (this.TblWayAwr_Oral.Count > 0)
                {
                    foreach (var item in this.TblWayAwr_Oral)
                    {
                        if (item.TblWayIfrm_Oral != null && item.TblWayIfrm_Oral.TblSbjOral != null)
                        {
                            prevAct = item.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
                            break;
                        }
                    }
                }
                if (this.TblWayAwr_News.Count > 0)
                {
                    foreach (var item in this.TblWayAwr_News)
                    {
                        if (item.TblWayIfrm_News != null && item.TblWayIfrm_News.TblNew != null)
                        {
                            prevAct = item.TblWayIfrm_News.TblNew.TblEvtRst.TblAct;
                            break;
                        }
                    }
                }

                if (prevAct != previousActivity)
                {
                    previousActivity = prevAct;

                    //if (getEvtSrtInGrp().Count(evt => evt.PreviousActivity == prevAct) > 0)
                    //{
                    //    this.FldGrpEvt = Model.TblAct.GetNewEvtSrtGroupID(this.TblAct);

                    //    if (GrpChanged != null)
                    //    {
                    //        GrpChanged(this, null);
                    //    }
                    //}


                    OnPropertyChanged("PreviousActivity");
                }


                //previousActivity = prevAct;

                return previousActivity;
            }
            set
            {
                previousActivity = value;
                OnPropertyChanged("PreviousActivity");
            }
        }

        /// <summary>
        /// نوع رخداد آغازگر
        /// </summary>
        public EvtSrtType TypSrt
        {
            get
            {
                return (EvtSrtType)this.FldTypEvtSrt;
            }
        }

        /// <summary>
        ///  نام رخداد اغازگر بر اساس نوع آن
        /// </summary>
        public string NameSrt
        {
            get
            {
                using (BPMNDBEntities ctx = new BPMNDBEntities())
                {
                    return ctx.TblItmFixSfws.Single(m => m.FldCodSbj == 2 && m.FldCodItm == this.FldTypEvtSrt).FldNamItm;
                }
            }
        }


        public ObservableCollection<IWayAwr> WayAwrs { get; set; }

        public bool IsAddWayAwrEnabled
        {
            get
            {
                switch ((EvtSrtType)this.FldTypEvtSrt)
                {
                    case EvtSrtType.inSgmtTime:
                        return false;
                    case EvtSrtType.aftrCdnEvtSrt:
                        return false;
                    case EvtSrtType.aftrAwareEvtSrt:
                        if (WayAwrs != null)
                        {
                            return WayAwrs.Count == 0;
                        }
                        return true;
                    case EvtSrtType.aftrAnyCdnEvtSrt:
                        return true;
                    case EvtSrtType.spcCdnEvtSrtAftr:
                        return true;
                    case EvtSrtType.errAccurEvtSrt:
                        return true;
                    case EvtSrtType.cancelEvtSrt:
                        return true;
                    case EvtSrtType.spcCdnEvtSrt:
                        return true;
                    case EvtSrtType.getNewAwrAftr:
                        return true;
                    default:
                        return true;
                }
            }
        }

        public void WayAwrs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }
            foreach (var item in e.NewItems)
            {
                dynamic d = item;
                d.IsSelected = true;
            }
            this.OnPropertyChanged("IsAddWayAwrEnabled");
            this.OnPropertyChanged("WayAwrs");
        }

        public void evtSrt_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldTypEvtSrt")
            {
                OnPropertyChanged("IsAddWayAwrEnabled");
            }

            if (e.PropertyName == "PreviousActivity")
            {
                ChangeGrpBasedOnPrevAct();
            }

        }

        /// <summary>
        /// لیست ورودی های فرستاده شده برای رخداد جاری
        /// </summary>
        public List<TblObj> AllObjSentToEvtSrt
        {
            get
            {
                return DetectAllObjSentToEvtSrt();
            }
        }

        public TblAct GetPrevAct()
        {
            TblAct prevAct = null;

            if (this.TblWayAwr_RecvInt.Count > 0)
            {
                foreach (var item in this.TblWayAwr_RecvInt)
                {
                    if (item.TblWayIfrm_SndOut != null && item.TblWayIfrm_SndOut.TblObj != null)
                    {
                        prevAct = item.TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
                        break;
                    }
                }
            }
            if (this.TblWayAwr_Oral.Count > 0)
            {
                foreach (var item in this.TblWayAwr_Oral)
                {
                    if (item.TblWayIfrm_Oral != null && item.TblWayIfrm_Oral.TblSbjOral != null)
                    {
                        prevAct = item.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
                        break;
                    }
                }
            }
            if (this.TblWayAwr_News.Count > 0)
            {
                foreach (var item in this.TblWayAwr_News)
                {
                    if (item.TblWayIfrm_News != null && item.TblWayIfrm_News.TblNew != null)
                    {
                        prevAct = item.TblWayIfrm_News.TblNew.TblEvtRst.TblAct;
                        break;
                    }
                }
            }

            return prevAct;

        }

        public void ChangeGrpBasedOnPrevAct()
        {
            if (previousActivity != null && !previousActivity.FldActUspf)
            {
                if (this.TypSrt != EvtSrtType.aftrAwareEvtSrt) //bug 702
                {
                    if (getEvtSrtInGrp().Count(evt => evt.PreviousActivity == previousActivity) > 0)
                    {
                        this.FldGrpEvt = Model.TblAct.GetNewEvtSrtGroupID(this.TblAct);

                        if (GrpChanged != null)
                        {
                            GrpChanged(this, null);
                        }
                    }

                }

            }
        }


        /// <summary>
        /// شناسایی لیست تمام ورودی های فرستاده شده به رخداد جاری
        /// </summary>
        /// <returns></returns>
        private List<TblObj> DetectAllObjSentToEvtSrt()
        {
            List<TblObj> lst = new List<TblObj>();

            foreach (TblWayAwr_RecvInt item in this.TblWayAwr_RecvInt)
            {
                if (!lst.Contains(item.TblWayIfrm_SndOut.TblObj))
                {
                    lst.Add(item.TblWayIfrm_SndOut.TblObj);
                }
            }

            return lst;
        }

        private List<TblEvtSrt> getEvtSrtInGrp()
        {
            return this.TblAct.TblEvtSrts.Where(e => e.GetHashCode() != this.GetHashCode() && e.FldGrpEvt == this.FldGrpEvt).ToList();
        }


        public event EventHandler GrpChanged;
    }
}
