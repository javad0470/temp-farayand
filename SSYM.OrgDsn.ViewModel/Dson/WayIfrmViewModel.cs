using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class WayIfrmViewModel : NotificationObject
    {
        #region ' Fields '

        private TblAct act;
        private TblNod posPst;
        private IWayIfrm wayIfrm;
        private ObservableCollection<TblEvtRst> evtRsts;
        private ListCollectionView evtRstsCV;

        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act">نحوه های اگاه سازی با مبدا فعالیت مورد نظر</param>
        /// <param name="destPospst">نحوه های آگاه سازی با مقصد جایگاه مورد نظر</param>
        /// <param name="ety">نحوه آگاه سازی مورد نظر</param>
        public WayIfrmViewModel(TblAct act, TblNod srcNod, IWayIfrm wayIfrm)
        {
            if (wayIfrm == null)
            {
                return;
            }

            DeleteEvtRstCommand = new DelegateCommand<IObjRst>(DeleteRstExecute, CanDeleteRst);

            this.act = act;
            this.posPst = srcNod;
            this.wayIfrm = wayIfrm;
            evtRsts = new ObservableCollection<TblEvtRst>(act.TblEvtRsts);

            foreach (var evtRst in act.TblEvtRsts)
            {
                evtRst.ObjRsts = new ObservableCollection<IObjRst>();

                if (wayIfrm is TblWayIfrm_News)
                {
                    foreach (var news in evtRst.TblNews)
                    {
                        if (news.ActTarget.FirstOrDefault(m => m.FldCodAct == wayIfrm.ActDst.FldCodAct) != null)
                        {
                            evtRst.ObjRsts.Add(news);

                            if (news == this.wayIfrm.ObjRst)
                            {
                                news.HasDson = true;
                            }
                        }
                    }
                }

                if (wayIfrm is TblWayIfrm_Oral)
                {
                    foreach (var oral in evtRst.TblSbjOrals)
                    {
                        if (oral.ActTarget.FirstOrDefault(m => m.FldCodAct == wayIfrm.ActDst.FldCodAct) != null)
                        {
                            evtRst.ObjRsts.Add(oral);

                            if (oral == this.wayIfrm.ObjRst)
                            {
                                oral.HasDson = true;
                            }
                        }
                    }
                }

                if (wayIfrm is TblWayIfrm_SndOut)
                {
                    foreach (var obj in evtRst.TblObjs)
                    {
                        if (obj.ActTarget.FirstOrDefault(m => m.FldCodAct == wayIfrm.ActDst.FldCodAct) != null)
                        {
                            evtRst.ObjRsts.Add(obj);

                            if (obj == this.wayIfrm.ObjRst)
                            {
                                obj.HasDson = true;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region ' Properties / Commands '

        public ListCollectionView EvtRstsCV
        {
            get
            {
                evtRstsCV = new ListCollectionView(evtRsts);
                return evtRstsCV;
            }
        }

        public ICommand DeleteEvtRstCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// این متد زمانی فراخوانی میشود
        /// که کاربر گزینه به درستی نسبت داده شده را انتخاب کند
        /// </summary>
        internal void FilterByCurrentWayIfrm()
        {

            if (wayIfrm is TblWayIfrm_News)
            {
                TblWayIfrm_News n = wayIfrm as TblWayIfrm_News;
                evtRsts = new ObservableCollection<TblEvtRst>(evtRsts.Where(m => m.ObjRsts.SingleOrDefault(w => (w is TblNew) && (w as TblNew).FldCodNews == n.FldCodNews) != null));
            }
            else if (wayIfrm is TblWayIfrm_Oral)
            {
                TblWayIfrm_Oral o = wayIfrm as TblWayIfrm_Oral;
                evtRsts = new ObservableCollection<TblEvtRst>(evtRsts.Where(m => m.ObjRsts.SingleOrDefault(w => (w is TblSbjOral) && (w as TblSbjOral).FldCodSbjOral == o.FldCodSbjOral) != null));

            }
            else if (wayIfrm is TblWayIfrm_SndOut)
            {
                TblWayIfrm_SndOut s = wayIfrm as TblWayIfrm_SndOut;
                evtRsts = new ObservableCollection<TblEvtRst>(evtRsts.Where(m => m.ObjRsts.SingleOrDefault(w => (w is TblObj) && (w as TblObj).FldCodObj == s.TblObj.FldCodObj) != null));
            }

            if (evtRsts.Count > 0)
            {
                evtRsts[0].ObjRsts = new ObservableCollection<IObjRst>(evtRsts[0].ObjRsts.Where(m => m.HasDson));
            }

            RaisePropertyChanged("EvtRstsCV");

        }

        internal void RemoveWayAwrFilter()
        {
            evtRsts = new ObservableCollection<TblEvtRst>(act.TblEvtRsts);
            RaisePropertyChanged("EvtRstsCV");
        }

        /// <summary>
        /// خروجی مورد ادعا را از فعالیت جاری حذف میکند
        /// </summary>
        internal void WrongEvt()
        {
            evtRsts.Remove(wayIfrm.ObjRst.EvtRst);
            RaisePropertyChanged("EvtRstsCV");
        }

        public void AddWayIfrmToEvtRst(TblEvtRst evtRst, IWayIfrm wayIfrm)
        {
            if (evtRst == null || wayIfrm == null)
            {
                return;
            }

            if (evtRst.ObjRsts == null)
            {
                evtRst.ObjRsts = new ObservableCollection<IObjRst>();
            }

            if (evtRst.ObjRsts.SingleOrDefault(m => m.HasDson) != null)
            {
                // show error
                return;
            }

            IObjRst objRst = null;
            if (wayIfrm is TblWayIfrm_News)
            {
                TblNew news = new TblNew() { FldTtlNews = string.Empty };
                news.HasDson = true;
                news.IsAdded = true;
                objRst = news;
            }

            if (wayIfrm is TblWayIfrm_Oral)
            {
                TblSbjOral oral = new TblSbjOral();
                oral.HasDson = true;
                oral.IsAdded = true;
                objRst = oral;
            }

            if (wayIfrm is TblWayIfrm_SndOut)
            {
                TblObj obj = new TblObj() { FldNamObj = string.Empty };
                obj.HasDson = true;
                obj.IsAdded = true;
                objRst = obj;
            }

            evtRst.ObjRsts.Add(objRst);

            OnWayIfrmChanged(true);
        }

        public void Clear()
        {
            evtRsts.Clear();
            RaisePropertyChanged("EvtRstsCV");
        }

        #endregion

        #region ' Private Methods '

        private bool CanDeleteRst(IObjRst arg)
        {
            return true;
        }

        private void DeleteRstExecute(IObjRst obj)
        {
            if (obj.IsAdded)
            {
                foreach (var rst in evtRsts)
                {
                    foreach (var objRst in rst.ObjRsts)
                    {
                        if (objRst == obj)
                        {
                            rst.ObjRsts.Remove(obj);

                            OnWayIfrmChanged(false);
                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region ' Events '


        private void OnWayIfrmChanged(bool added)
        {
            if (WayIfrmChanged != null)
            {
                WayIfrmChanged(added);
            }
        }

        public event Action<bool> WayIfrmChanged;

        #endregion
    }
}
