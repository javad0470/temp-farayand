using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using System.Data.Entity;
using System.Data.Objects;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcIntViewModel : PopupViewModel
    {

        #region ' Fields '

        private Tuple<IObjRst, TblEvtSrt> sentToMeObjectsSelectedItem;
        private Tuple<IObjRst, TblEvtSrt> sentToMyCurrentActObjectsSelectedItem;
        private bool isSentTooMeSelected;
        private bool isSentToMyCurrentActSelected;
        private TblAct previousActivity;
        ListCollectionView sentToMeObjects;
        ListCollectionView sentToMyCurrentActObjects;
        TblAct actCnt;

        #endregion

        #region ' Initialaizer '

        public SlcIntViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            InputDoesntExistCommand = new DelegateCommand(ExecuteInputDoesntExist);
        }



        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// شناسه فعالیت تولید کننده ورودی
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
            get
            {
                return actCnt;
            }
            set
            {
                actCnt = value;// bpmnEty.TblActs.Single(a => a.FldCodAct == value.FldCodAct);
            }
        }

        /// <summary>
        /// رخداد آغازگر مقصد ورودی جاری
        /// </summary>
        public TblEvtSrt EvtSrt { get; set; }

        /// <summary>
        /// شناسه ورودی که هم اکنون در حالیت انتخاب قرار دارد
        /// </summary>
        public int CurrentObjID { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> SentToMyCurrentActObjectsSelectedItem
        {
            get { return sentToMyCurrentActObjectsSelectedItem; }
            set
            {
                sentToMyCurrentActObjectsSelectedItem = value;
                RaisePropertyChanged("SentToMyCurrentActObjectsSelectedItem");
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// خروجی های فرستاده شده برای فعالیت جاری من
        /// </summary>
        public ListCollectionView SentToMyCurrentActObjects
        {
            get { return sentToMyCurrentActObjects; }
            set
            {
                sentToMyCurrentActObjects = value;
                RaisePropertyChanged("SentToMyCurrentActObjects");
            }
        }

        /// <summary>
        /// is selected inputs radio button selected
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

        /// <summary>
        /// رادیو باتن فرستنده به این فعالیت من انتخاب شده است
        /// </summary>
        public bool IsSentToMyCurrentActSelected
        {
            get { return isSentToMyCurrentActSelected; }
            set
            {
                isSentToMyCurrentActSelected = value;

                if (value)
                {
                    DetectSentToMyCurrentActObjects();
                }
                RaisePropertyChanged("IsSentToMyCurrentActSelected");
            }
        }

        /// <summary>
        /// input doesnt exist button
        /// </summary>
        public ICommand InputDoesntExistCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از فعالیت جاری  هستند
        /// </summary>
        public void DetectSentToMeObjects()
        {
            //Pdr9053(2)
            if (
                this.EvtSrt.WayAwrs.Count > 1
                ||
                this.PreviousActivity != null


                )
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblObjs);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.TblNod.AllObjRstSentToNod.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&

                    (this.EvtSrt.EvtRstEqualToEvtSrt == null || (this.EvtSrt.EvtRstEqualToEvtSrt != null && m.Item1.EvtRst.FldCodEvtRst == this.EvtSrt.EvtRstEqualToEvtSrt.FldCodEvtRst))
                    &&
                    (this.EvtSrt.PreviousActivity == null || (this.EvtSrt.PreviousActivity != null && this.EvtSrt.PreviousActivity.FldCodAct == m.Item1.ActSrc.FldCodAct))
                    &&
                    m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblObj)).ToList();

                SentToMeObjects = new ListCollectionView(lst);

                SentToMeObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
            else
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblObjs);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.TblNod.AllObjRstSentToNod.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                     m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblObj)).ToList();

                SentToMeObjects = new ListCollectionView(lst);

                SentToMeObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
        }

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که فرستنده به فعالیت جاری هستند
        /// </summary>
        public void DetectSentToMyCurrentActObjects()
        {
            if (this.PreviousActivity != null)
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblObjs);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.AllObjRstSentToAct.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                     m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&

                     (this.EvtSrt.EvtRstEqualToEvtSrt == null || (this.EvtSrt.EvtRstEqualToEvtSrt != null && m.Item1.EvtRst.FldCodEvtRst == this.EvtSrt.EvtRstEqualToEvtSrt.FldCodEvtRst))

                     &&

                     (this.EvtSrt.PreviousActivity == null || (this.EvtSrt.PreviousActivity != null && this.EvtSrt.PreviousActivity.FldCodAct == m.Item1.ActSrc.FldCodAct))
                     &&
                    m.Item1.GetType() == typeof(TblObj)).ToList();

                SentToMyCurrentActObjects = new ListCollectionView(lst);

                SentToMyCurrentActObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
            else
            {
                PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblObjs);

                List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.AllObjRstSentToAct.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                     m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct &&
                    m.Item1.GetType() == typeof(TblObj)).ToList();

                SentToMyCurrentActObjects = new ListCollectionView(lst);

                SentToMyCurrentActObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));
            }
        }

        #endregion

        #region ' Private Methods '

        private void ExecuteInputDoesntExist()
        {
            this.Result = PopupResult.Yes;
        }

        /// <summary>
        /// آبجکت های حذف شده و نیز تکراری را از ویو خارج می کند
        /// </summary>
        /// <param name="lst">لیست تمام آبجکت ها</param>
        private ObservableCollection<VwAllUsedOutWithSourceName> ExcludeDeletedAndRepeatedObjects(ObservableCollection<VwAllUsedOutWithSourceName> lst)
        {
            ObservableCollection<VwAllUsedOutWithSourceName> lst1 = new ObservableCollection<VwAllUsedOutWithSourceName>();
            List<TblObj> tblObj = this.bpmnEty.TblObjs.AsEnumerable().Where(m => m.EntityState == EntityState.Deleted).ToList<TblObj>();
            int[] i1 = new int[tblObj.Count];
            List<int> i2 = new List<int>();
            for (int i = 0; i < tblObj.Count; i++)
            {
                i1[i] = tblObj[i].FldCodObj;
            }
            foreach (var item in lst)
            {
                if (!i1.Contains(item.FldCodObj) && !i2.Contains(item.FldCodObj))
                {
                    lst1.Add(item);
                    i2.Add(item.FldCodObj);
                }
            }
            return lst1;
        }

        //protected override bool CanOKExecute()
        //{
        //    return false;
        //}
        /// <summary>
        /// این متد ورودی های متصل به یک رخداد آغازگر را یکتا می کند.
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        private void FilterObjEvtSrt(ObservableCollection<VwAllUsedOutWithSourceName> tbl)
        {
            ObservableCollection<Model.VwAllUsedOutWithSourceName> temp = new ObservableCollection<VwAllUsedOutWithSourceName>(tbl.Where(E => E.FldCodEvtSrt == EvtSrt.FldCodEvtSrt));

            foreach (VwAllUsedOutWithSourceName item1 in temp)
            {
                foreach (VwAllUsedOutWithSourceName item2 in tbl)
                {
                    if (item1.FldCodObj == item2.FldCodObj)
                        tbl.Remove(item2);
                }
            }

        }

        protected override bool CanOKExecute()
        {
            return SentToMyCurrentActObjectsSelectedItem != null
                || SentToMeObjectsSelectedItem != null;
            //RaiseOKCanExecute();

        }
        #endregion
    }
}
