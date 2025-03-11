using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcActSrcViewModel : PopupViewModel
    {
        #region ' Fields '

        private Tuple<IObjRst, TblEvtSrt> sentToMeObjectsSelectedItem;
        private string _firstTabHeader;
        private bool isSentTooMeSelected;
        private bool canUsrSlcAct;
        private int destinationActivityID;
        ListCollectionView sentToMeObjects;
        private bool _justShowOralSenders;

        TblAct actCnt;

        ObservableCollection<TblAct> _actOfNodCnt;

        TblAct _actOfNodCntSelectedItem;

        bool _isActsOfNodCnt;


        #endregion

        #region ' Initialaizer '


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="justShowOralSenders">فقط فعالیت هایی که به جایگاه فعلی مطلب شفاهی ارسال کردند را نمایش بده</param>
        public SlcActSrcViewModel(BPMNDBEntities context, bool justShowOralSenders = false)
            : base(context)
        {
            _firstTabHeader = "فرستنده به من";
            ActDoesnExistCommand = new DelegateCommand(ExecuteActDoesnExistCommand);
            _justShowOralSenders = justShowOralSenders;
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public bool IsActsOfNodCntSelected
        {
            get { return _isActsOfNodCnt; }
            set
            {
                _isActsOfNodCnt = value;
                if (value)
                {
                    DetectActsOfNodCnt();
                }

                RaisePropertyChanged("IsActsOfNodCntSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblAct> ActOfNodCnt
        {
            get { return _actOfNodCnt; }
            set
            {
                _actOfNodCnt = value;

                RaisePropertyChanged("ActOfNodCnt");
            }
        }


        public string FirstTabHeader
        {
            get { return _firstTabHeader; }
            set
            {
                _firstTabHeader = value;
                RaisePropertyChanged("FirstTabHeader");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TblAct ActOfNodCntSelectedItem
        {
            get { return _actOfNodCntSelectedItem; }
            set
            {
                _actOfNodCntSelectedItem = value;

                RaisePropertyChanged("ActOfNodCntSelectedItem");

                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// آیا کاربر قادر به انتخاب یک فعالیت جدید خواهد بود
        /// </summary>
        public bool CanUsrSlcAct
        {
            get { return canUsrSlcAct; }
            set
            {
                canUsrSlcAct = value;
            }
        }

        /// <summary>
        /// شناسه فعالیت جاری
        /// </summary>
        public int DestinationActivityID
        {
            get { return destinationActivityID; }
            set
            {
                destinationActivityID = value;
                RaisePropertyChanged("SentToMeObjects");
                RaisePropertyChanged("AllObjects");
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
        /// رخداد آغازگر مقصد مطلب شفاهی جاری
        /// </summary>
        public TblEvtSrt EvtSrt { get; set; }

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
                return sentToMeObjectsSelectedItem;
            }

            set
            {
                sentToMeObjectsSelectedItem = value;

                RaisePropertyChanged("SentToMeObjectsSelectedItem");

                RaiseOKCanExecute();
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
                    DetectSentToMeActs();
                }

                RaisePropertyChanged("IsSentTooMeSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ActDoesnExistCommand { get; set; }

        #endregion

        #region ' Public Methods '
        #endregion

        #region ' Private Methods '


        /// <summary>
        /// شناسایی تمامی فعالیتهایی که ارسال کننده به فعالیت جاری  هستند
        /// </summary>
        private void DetectSentToMeActs()
        {
            PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblSbjOrals);

            List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActCnt.TblNod.AllObjRstSentToNod.Where(m => (m.Item2 == null || m.Item1.EvtSrtTarget.Where(n => n.FldCodEvtSrt == this.EvtSrt.FldCodEvtSrt).Count() == 0) &&
                 m.Item1.EvtRst.FldCodAct != this.ActCnt.FldCodAct && !m.Item1.ActSrc.FldActUspf /*&&                m.Item1.GetType() == typeof(TblSbjOral)*/).ToList();

            if (_justShowOralSenders)
            {
                lst = lst.Where(i => i.Item1 is TblSbjOral).ToList();
            }

            SentToMeObjects = new ListCollectionView(lst);

            SentToMeObjects.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            //TblAct tblAct = this.bpmnEty.TblActs.SingleOrDefault(E => E.FldCodAct == this.DestinationActivityID);
            //if (tblAct != null)
            //{
            //    this.SentToMeObjects = new ObservableCollection<Model.SprAllRelatedActivities_Result>(this.bpmnEty.SprAllRelatedActivities().Where(E => E.FldCodNodDst == tblAct.TblNod.FldCodNod));
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectActsOfNodCnt()
        {
            var lst = this.ActCnt.TblNod.TblActs.Where(m => m.FldCodAct != this.ActCnt.FldCodAct && !m.FldActUspf).ToList();

            this.ActOfNodCnt = new ObservableCollection<TblAct>(lst);
        }



        /// <summary>
        /// 
        /// </summary>
        private void ExecuteActDoesnExistCommand()
        {
            this.Result = PopupResult.Yes;
        }

        protected override bool CanOKExecute()
        {
            return SentToMeObjectsSelectedItem != null
                || ActOfNodCntSelectedItem != null;
        }

        #endregion
    }
}
