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
    public class WayAwrViewModel : NotificationObject
    {
        #region ' Fields '

        private TblAct act;
        private TblNod posPst;
        private IWayAwr wayAwr;
        private ObservableCollection<TblEvtSrt> evtSrts;
        private ListCollectionView evtSrtsCV;

        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act">نحوه های اگهی با مبدا فعالیت مورد نظر</param>
        /// <param name="srcPospst">نحوه های آگاهی با مقصد جایگاه مورد نظر
        /// در صورتی که نال فرستاده شود یعنی فقط باید رخداد آغازگر دارای ناهمسانی نمایش داده شود
        /// </param>
        /// <param name="ety">نحوه آگاهی مورد نظر</param>
        public WayAwrViewModel(TblAct act, TblNod srcNod, IWayAwr ety, bool isSpcf)
        {
            if (ety == null)
            {
                return;
            }


            DeleteEvtSrtCommand = new DelegateCommand<IWayAwr>(DeleteWayAwrExecute, CanDeleteWayAwr);

            IsSpcf = isSpcf;
            this.act = act;
            posPst = srcNod;
            wayAwr = ety;

            // امکان دراپ مطلب شفاهی بر روی رخداد آغازگر صرفا پس از آگاهی نباید وجود داشته باشد.
            bool notShowAfterAwr = wayAwr is TblWayAwr_Oral;

            var evtSrtLst = new List<TblEvtSrt>();

            if (isSpcf)
            {
                // رخداد های آغازگری که از نوع 'در مقاطع زمانی' و 'وقوع شرایط پس از فعالیت' نباشند و
                //حداقل یک نحوه آگاهی داشته باشند که فعالیت تولید کننده آن همان فعالیت تولید کننده نا همسانی باشد' 
                // یا هیچ فعالیت مبدائی نداشته باشد
                evtSrtLst = new List<TblEvtSrt>(act.TblEvtSrts.Where(m => m.TypSrt != Model.Enum.EvtSrtType.inSgmtTime && m.TypSrt != Model.Enum.EvtSrtType.aftrCdnEvtSrt &&
                    (m.AllWayAwr.Count == 0 || m.AllWayAwr.Any(x => x.ActSrc != null && x.ActSrc.FldCodAct == ety.ActSrc.FldCodAct))));
            }
            else
            {
                evtSrtLst = new List<TblEvtSrt>(act.TblEvtSrts.Where(m =>
                    (m.PreviousActivity == null || m.PreviousActivity.FldCodAct == wayAwr.ActSrc.FldCodAct) &&
                    m.TypSrt != Model.Enum.EvtSrtType.inSgmtTime &&
                    m.TypSrt != Model.Enum.EvtSrtType.aftrCdnEvtSrt));
            }

            // امکان دراپ مطلب شفاهی بر روی رخداد آغازگر صرفا پس از آگاهی نباید وجود داشته باشد.
            if (notShowAfterAwr)
            {
                evtSrtLst = evtSrtLst.Where(e => e.TypSrt != Model.Enum.EvtSrtType.aftrAwareEvtSrt).ToList();
            }

            evtSrts = new ObservableCollection<TblEvtSrt>(evtSrtLst);

            foreach (var evtSrt in evtSrts)
            {
                evtSrt.WayAwrs = new ObservableCollection<IWayAwr>();
                if (wayAwr is TblWayAwr_News)
                {
                    foreach (var news in evtSrt.TblWayAwr_News)
                    {

                        if (news.ActSrc.FldCodAct == wayAwr.ActSrc.FldCodAct)
                        {
                            if (news.FldCod == wayAwr.FldCod)
                            {
                                news.IsDson = true;
                            }

                            evtSrt.WayAwrs.Add(news);
                        }
                    }
                }

                if (wayAwr is TblWayAwr_Oral)
                {
                    foreach (var oral in evtSrt.TblWayAwr_Oral)
                    {
                        if (oral.ActSrc.FldCodAct == wayAwr.ActSrc.FldCodAct)
                        {
                            if (oral.FldCod == wayAwr.FldCod)
                            {
                                oral.IsDson = true;
                            }

                            evtSrt.WayAwrs.Add(oral);
                        }
                    }

                }

                if (wayAwr is TblWayAwr_RecvInt)
                {
                    foreach (var inpt in evtSrt.TblWayAwr_RecvInt)
                    {
                        if (inpt.ActSrc.FldCodAct == wayAwr.ActSrc.FldCodAct)
                        {
                            if (inpt.FldCod == wayAwr.FldCod)
                            {
                                inpt.IsDson = true;
                            }

                            evtSrt.WayAwrs.Add(inpt);
                        }
                    }
                }
            }
        }

        #endregion

        #region ' Properties / Commands '

        public bool IsSpcf { get; set; }

        public ListCollectionView EvtSrtsCV
        {
            get
            {
                evtSrtsCV = new ListCollectionView(evtSrts);
                return evtSrtsCV;
            }
        }

        public ICommand DeleteEvtSrtCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// فقط نحوه آگاهی نسبت داده شده به همراه رخدادآغازگر و فعالیت مورد نظر را نمایش میدهد
        /// </summary>
        internal void FilterByCurrentWayAwr()
        {
            evtSrts = new ObservableCollection<TblEvtSrt>(evtSrts.Where(m => m.WayAwrs.SingleOrDefault(w => w.FldCod == wayAwr.FldCod) != null));

            RaisePropertyChanged("EvtSrtsCV");
        }

        internal void RemoveWayAwrFilter()
        {
            evtSrts = new ObservableCollection<TblEvtSrt>(act.TblEvtSrts);
            RaisePropertyChanged("EvtSrtsCV");
        }

        /// <summary>
        /// ورودی مورد ادعا را از فعالیت جاری حذف میکند
        /// </summary>
        internal void WrongEvt()
        {
            evtSrts.Remove(wayAwr.EvtSrt_Temp);
            foreach (var evtSrt in evtSrts)
            {
                IWayAwr wa = evtSrt.WayAwrs.SingleOrDefault(m => m.ObjRst == wayAwr.ObjRst);
                if (wa != null)
                {
                    evtSrt.WayAwrs.Remove(wa);
                }
            }
        }

        /// <summary>
        /// clear all evtsrts
        /// </summary>
        internal void Clear()
        {
            evtSrts.Clear();
            RaisePropertyChanged("EvtSrtsCV");
        }

        /// <summary>
        /// this method will call from code behind when dragdrop
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="wayAwr"></param>
        public void AddWayAwrToEvtSrt(TblEvtSrt evt, IWayAwr wayAwr)
        {
            if (wayAwr == null)
            {
                return;
            }

            if (evt.WayAwrs == null)
            {
                evt.WayAwrs = new ObservableCollection<IWayAwr>();
            }
            if (evt.WayAwrs.SingleOrDefault(m => m.IsDson) == null)
            {
                IWayAwr newWayAwr = null;

                if (wayAwr is TblWayAwr_News)
                {
                    TblWayAwr_News newNews = new TblWayAwr_News();
                    newNews.EvtSrt_Temp = evt;
                    newWayAwr = newNews;

                }
                if (wayAwr is TblWayAwr_Oral)
                {
                    TblWayAwr_Oral newOral = new TblWayAwr_Oral();
                    newOral.EvtSrt_Temp = evt;
                    newWayAwr = newOral;
                }
                if (wayAwr is TblWayAwr_RecvInt)
                {
                    TblWayAwr_RecvInt newInt = new TblWayAwr_RecvInt();
                    newInt.EvtSrt_Temp = evt;
                    newWayAwr = newInt;
                }

                newWayAwr.IsAdded = true;
                newWayAwr.IsDson = true;

                evt.WayAwrs.Add(newWayAwr);

                OnWayAwrChanged(true);
            }
            else
            {
                // show error
            }
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// this method will call from view when delete wayAwr
        /// </summary>
        /// <param name="wayAwr"></param>
        private void DeleteWayAwr(IWayAwr wayAwr)
        {
            if (wayAwr.IsAdded)
            {
                wayAwr.EvtSrt_Temp.WayAwrs.Remove(wayAwr);
                OnWayAwrChanged(false);
            }
        }

        private bool CanDeleteWayAwr(IWayAwr arg)
        {
            return true;
        }

        private void DeleteWayAwrExecute(IWayAwr obj)
        {
            DeleteWayAwr(obj);
        }

        private void OnWayAwrChanged(bool added)
        {
            if (WayAwrChanged != null)
            {
                WayAwrChanged(added);
            }
        }

        #endregion

        #region ' Events '

        public event Action<bool> WayAwrChanged;

        #endregion
    }
}
