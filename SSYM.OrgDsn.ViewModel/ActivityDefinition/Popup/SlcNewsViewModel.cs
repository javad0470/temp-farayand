using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcNewsViewModel : PopupViewModel
    {
        #region ' Fields '

        ListCollectionView allNews;       
        private Model.TblNew selectedItem;
        private int codActPvs;
        ListCollectionView sentToMeObjects;
        private Tuple<IObjRst, TblEvtSrt> sentToMeObjectsSelectedItem;

        bool isFomMeeSlcEed;
        bool isFomActCntSlcEed;
        TblAct actSrc;

        private Tuple<IObjRst, TblEvtSrt> fomMeeSlcEedItm;
        private Tuple<IObjRst, TblEvtSrt> fomActCntSlcEedItm;

        ListCollectionView recvFomMee;
        ListCollectionView recvFomActCnt;

        #endregion

        #region ' Initialaizer '

        public SlcNewsViewModel()
            : base(new BPMNDBEntities())
        {
            NewsDoesntExistCommand = new DelegateCommand(ExecuteNewsDoesntExist);
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// فعالیت جاری
        /// </summary>
        public TblAct ActSrc
        {
            get { return actSrc; }
            set { actSrc = value; }
        }

        /// <summary>
        /// خبر جاری
        /// </summary>
        public TblNew ObjCnt { get; set; }

        /// <summary>
        /// دریافت شده از من
        /// </summary>
        public ListCollectionView RecvFomMee
        {
            get { return recvFomMee; }
            set { recvFomMee = value; }
        }

        /// <summary>
        /// دریافت شده از فعالیت جاری من
        /// </summary>
        public ListCollectionView RecvFomActCnt
        {
            get { return recvFomActCnt; }
            set { recvFomActCnt = value; }
        }

        /// <summary>
        /// FromMeSelectedItem
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> FomMeeSlcEedItm
        {
            get
            {
                return fomMeeSlcEedItm;
            }
            set
            {
                fomMeeSlcEedItm = value;
                RaisePropertyChanged("FomMeeSlcEedItm");
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> FomActCntSlcEedItm
        {
            get
            {
                return fomActCntSlcEedItm;
            }
            set
            {
                fomActCntSlcEedItm = value;
                RaisePropertyChanged("FomActCntSlcEedItm");
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// is from me selected
        /// </summary>
        public bool IsFomMeeSlcEed
        {
            get { return isFomMeeSlcEed; }
            set
            {
                isFomMeeSlcEed = value;
                if (value)
                {
                    DetectFomMeObj();
                }
            }
        }

        /// <summary>
        /// دریافت شده از این فعالیت من در حالت انتخاب قرار دارد
        /// </summary>
        public bool IsFomActCntSlcEed
        {
            get { return isFomActCntSlcEed; }
            set
            {
                isFomActCntSlcEed = value;
                if (value)
                {
                    DetectFomActCntObj();
                }
            }
        }

        /// <summary>
        /// شناسه فعالیت قبلی
        /// </summary>
        public int CodActPvs
        {
            get { return codActPvs; }
            set
            {
                codActPvs = value;
                RaisePropertyChanged("AllNews");
            }
        }

        /// <summary>
        /// شناسه خبری که هم اکنون در حالت انتخاب قرار دارد
        /// </summary>
        public int CodNewsCnt { get; set; }

        ///// <summary>
        ///// gets all news
        ///// </summary>
        //public ListCollectionView AllNews
        //{
        //    get { return allNews; }
        //    set { allNews = value; }
        //}
        
        ///// <summary>
        ///// selected item from news data grid
        ///// </summary>
        //public Model.TblNew SelectedItem
        //{
        //    get { return selectedItem; }
        //    set { selectedItem = value; }
        //}
        
        /// <summary>
        /// input doesnt exist button
        /// </summary>
        public ICommand NewsDoesntExistCommand { get; set; }

        ///// <summary>
        ///// returns objects sent to current activity
        ///// </summary>
        //public ListCollectionView SentToMeObjects
        //{
        //    get { return sentToMeObjects; }
        //    set
        //    {
        //        sentToMeObjects = value;
        //        RaisePropertyChanged("SentToMeObjects");
        //    }
        //}

        ///// <summary>
        ///// selected item from sent to me data grid
        ///// </summary>
        //public Tuple<IObjRst, TblEvtSrt> SentToMeObjectsSelectedItem
        //{
        //    get
        //    {
        //        if (sentToMeObjectsSelectedItem == null && SentToMeObjects != null)
        //        {
        //            return sentToMeObjectsSelectedItem = (SentToMeObjects.SourceCollection as List<Tuple<IObjRst, TblEvtSrt>>).FirstOrDefault();
        //        }
        //        else
        //        {
        //            return sentToMeObjectsSelectedItem;
        //        }
        //    }

        //    set
        //    {
        //        sentToMeObjectsSelectedItem = value;
        //    }
        //}

        #endregion

        #region ' Public Methods '

        ///// <summary>
        ///// شناسایی تمامی آبجکتها
        ///// </summary>
        //public void DetectAllNews()
        //{
        //    if (this.CodActPvs != 0)
        //    {
        //        PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNews.Where(E => E.TblEvtRst.FldCodAct == this.CodActPvs)); 

        //        //List<object> lst = new List<object>();

        //        //foreach (var item in this.bpmnEty.TblNews.Where(E => E.TblEvtRst.FldCodAct == this.CodActPvs))
        //        //{
        //        //    if (item.EntityState != EntityState.Deleted)
        //        //    {
        //        //        var news = new { item.FldCodNews, item.FldTtlNews, item.FldNamAct, item.FldNamNod };

        //        //        lst.Add(news);
        //        //    }
        //        //}

        //        //this.AllNews = new ObservableCollection<object>(lst);

        //        this.AllNews = new ListCollectionView(this.bpmnEty.TblNews.Where(E => E.TblEvtRst.FldCodAct == this.CodActPvs).ToList());

        //        this.AllNews.GroupDescriptions.Add(new PropertyGroupDescription("TblEvtRst.TblAct"));

                
        //    }
        //    else
        //    {
        //        PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNews); 

        //        //List<object> lst = new List<object>();

        //        //foreach (var item in this.bpmnEty.TblNews)
        //        //{
        //        //    if (item.EntityState != EntityState.Deleted)
        //        //    {
        //        //        var news = new { item.FldCodNews, item.FldTtlNews, item.FldNamAct, item.FldNamNod };

        //        //        lst.Add(news);
        //        //    }
        //        //}

        //        //this.AllNews = new ObservableCollection<object>(lst);

        //        this.AllNews = new ListCollectionView(this.bpmnEty.TblNews.ToList());

        //        this.AllNews.GroupDescriptions.Add(new PropertyGroupDescription("TblEvtRst.TblAct.TblNod"));
        //    }

        //    RaisePropertyChanged("AllNews");
        //}

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از گره جاری  هستند
        /// </summary>
        private void DetectFomMeObj()
        {
            //PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNods);

            List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActSrc.TblNod.AllObjRstSentFromNod.Where(m => m.Item1.CodObj != (ObjCnt != null ? ObjCnt.FldCodNews : 0) && m.Item1.GetType() == typeof(TblNew)).ToList();

            this.RecvFomMee = new ListCollectionView(lst);

            this.RecvFomMee.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            RaisePropertyChanged("RecvFomMee");
        }

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از فعالیت جاری  هستند
        /// </summary>
        private void DetectFomActCntObj()
        {
            //this.RecvFomActCnt = new ObservableCollection<TblAct>(this.ActSrc.TblNod.TblActs.Where(m => m.FldCodAct == this.ActSrc.FldCodAct).ToList());

            List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActSrc.AllObjRstSentFromAct.Where(m => m.Item1.CodObj != (ObjCnt != null ? ObjCnt.FldCodNews : 0) && m.Item1.GetType() == typeof(TblNew)).ToList();

            this.RecvFomActCnt = new ListCollectionView(lst);

            this.RecvFomActCnt.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            RaisePropertyChanged("RecvFomActCnt");
        }

        /// <summary>
        /// ExecuteNewsDoesntExist
        /// </summary>
        private void ExecuteNewsDoesntExist()
        {
            this.Result = PopupResult.Yes;
        }

        protected override bool CanOKExecute()
        {
            return FomActCntSlcEedItm != null
                || FomMeeSlcEedItm != null;
        }

        #endregion
    }
}
