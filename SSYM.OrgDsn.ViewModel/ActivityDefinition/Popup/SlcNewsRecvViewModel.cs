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
    public class SlcNewsRecvViewModel : PopupViewModel
    {
        #region ' Fields '

        ListCollectionView allNews;
        private Tuple<IObjRst, TblEvtSrt> selectedItem;
        private TblAct previousActivity;
        private bool isSentTooMeSelected;
        private bool isAllNewsSelected;

        private int codActPvs;
        ListCollectionView sentToMeObjects;
        private Tuple<IObjRst, TblEvtSrt> sentToMeObjectsSelectedItem;
        TblAct actCnt;

        #endregion

        #region ' Initialaizer '

        public SlcNewsRecvViewModel()
            : base(new BPMNDBEntities())
        {
            NewsDoesntExistCommand = new DelegateCommand(ExecuteNewsDoesntExist);
        }

        #endregion

        #region ' Properties / Commands '

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
        /// فعالیت تولید کننده ورودی
        /// </summary>
        public TblAct PreviousActivity
        {
            get
            {
                return previousActivity;
            }
            set
            {
                previousActivity = value;
            }
        }

        /// <summary>
        /// فعالیت جاری
        /// </summary>
        public TblAct ActCnt
        {
            get { return actCnt; }
            set { actCnt = value; }
        }

        /// <summary>
        /// رخداد آغازگر مقصد ورودی جاری
        /// </summary>
        public TblEvtSrt EvtSrt { get; set; }

        /// <summary>
        /// شناسه خبری که هم اکنون در حالت انتخاب قرار دارد
        /// </summary>
        public int CodNewsCnt { get; set; }

        /// <summary>
        /// gets all news
        /// </summary>
        public ListCollectionView AllNews
        {
            get { return allNews; }
            set
            {
                allNews = value;
                RaisePropertyChanged("AllNews");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSentTooMeSelected
        {
            get { return isSentTooMeSelected; }
            set
            {
                isSentTooMeSelected = value;
                if (value)
                {
                    DetectSentToMeObjects();
                }
                RaisePropertyChanged("IsSentTooMeSelected");
            }
        }

        ///
        public bool IsAllNewsSelected
        {
            get { return isAllNewsSelected; }
            set
            {
                isAllNewsSelected = value;
                if (value)
                {
                    DetectAllNews();
                }
                RaisePropertyChanged("IsAllNewsSelected");
            }
        }


        /// <summary>
        /// selected item from news data grid
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// input doesnt exist button
        /// </summary>
        public ICommand NewsDoesntExistCommand { get; set; }

        /// <summary>
        /// returns objects sent to current activity
        /// </summary>
        public ListCollectionView SentToMeObjects
        {
            get { return sentToMeObjects; }
            set
            {
                sentToMeObjects = value;
                RaisePropertyChanged("SentToMeObjects");
            }
        }

        /// <summary>
        /// selected item from sent to me data grid
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> SentToMeObjectsSelectedItem
        {
            get
            {
                if (sentToMeObjectsSelectedItem == null && SentToMeObjects != null)
                {
                    return sentToMeObjectsSelectedItem = (SentToMeObjects.SourceCollection as List<Tuple<IObjRst, TblEvtSrt>>).FirstOrDefault();
                }
                else
                {
                    return sentToMeObjectsSelectedItem;
                }
            }

            set
            {
                sentToMeObjectsSelectedItem = value;
                RaisePropertyChanged("SentToMeObjectsSelectedItem");
                RaiseOKCanExecute();
            }
        }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی آبجکتها
        /// </summary>
        private void DetectAllNews()
        {
            if (this.PreviousActivity != null)
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNews);

                List<Tuple<IObjRst, TblEvtSrt>> lst = PublicMethods.CurrentUser.TblOrg.DetectAllObjRstSentFromOrg(this.bpmnEty).Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                    (this.EvtSrt.EvtRstEqualToEvtSrt == null || (this.EvtSrt.EvtRstEqualToEvtSrt != null && m.Item1.EvtRst.FldCodEvtRst == this.EvtSrt.EvtRstEqualToEvtSrt.FldCodEvtRst)) &&

                    this.EvtSrt.PreviousActivity.FldCodAct == m.Item1.ActSrc.FldCodAct &&

                    m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblNew)).ToList();

                AllNews = new ListCollectionView(lst);

                AllNews.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
            else
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblObjs);

                List<Tuple<IObjRst, TblEvtSrt>> lst = PublicMethods.CurrentUser.TblOrg.DetectAllObjRstSentFromOrg(this.bpmnEty).Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                    m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblNew)).ToList();

                AllNews = new ListCollectionView(lst);

                AllNews.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectSentToMeObjects()
        {
            //Pdr9053(2)
            if (this.PreviousActivity != null)
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNews);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.TblNod.AllObjRstSentToNod.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&

                    //m.Item1.EvtRst.FldCodEvtRst == this.EvtSrt.EvtRstEqualToEvtSrt.FldCodEvtRst &&

                    (this.EvtSrt.EvtRstEqualToEvtSrt == null || (this.EvtSrt.EvtRstEqualToEvtSrt != null && m.Item1.EvtRst.FldCodEvtRst == this.EvtSrt.EvtRstEqualToEvtSrt.FldCodEvtRst)) &&

                    this.EvtSrt.PreviousActivity.FldCodAct == m.Item1.ActSrc.FldCodAct &&

                    m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblNew)).ToList();

                SentToMeObjects = new ListCollectionView(lst);

                SentToMeObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
            else
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblNews);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.TblNod.AllObjRstSentToNod.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                    m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblNew)).ToList();

                SentToMeObjects = new ListCollectionView(lst);

                SentToMeObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// ExecuteNewsDoesntExist
        /// </summary>
        private void ExecuteNewsDoesntExist()
        {
            this.Result = PopupResult.Yes;
        }

        protected override bool CanOKExecute()
        {
            return SelectedItem != null
                || SentToMeObjectsSelectedItem != null;
        }

        #endregion
    }
}
