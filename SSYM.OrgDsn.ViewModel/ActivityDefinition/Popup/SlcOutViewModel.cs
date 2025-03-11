using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.Entity;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SSYM.OrgDsn.Model;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcOutViewModel : PopupViewModel
    {
        #region ' Fields '

        private Tuple<IObjRst, TblEvtSrt> fomMeeSlcEedItm;
        private Tuple<IObjRst, TblEvtSrt> fomActCntSlcEedItm;

        ListCollectionView recvFomMee;
        ListCollectionView recvFomActCnt;

        bool isFomMeeSlcEed;
        bool isFomActCntSlcEed;
        TblAct actSrc;


        #endregion

        #region ' Initialaizer '

        public SlcOutViewModel()
            : base(new BPMNDBEntities())
        {
            OutputDoesnExistCommand = new DelegateCommand(ExecuteOutputDoesnExistCommand);

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
        /// خروجی جاری
        /// </summary>
        public TblObj ObjCnt { get; set; }

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
                this.RaiseOKCanExecute();
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
                this.RaiseOKCanExecute();
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

                RaisePropertyChanged("IsFomMeeSlcEed");
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

                RaisePropertyChanged("IsFomActCntSlcEed");
            }
        }

        /// <summary>
        /// OutputDoesnExistCommand
        /// </summary>
        public ICommand OutputDoesnExistCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از گره جاری  هستند
        /// </summary>
        public void DetectFomMeObj()
        {
            PublicMethods.ReloadEntity(bpmnEty, bpmnEty.TblObjs);

            List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActSrc.TblNod.AllObjRstSentFromNod.Where(m => m.Item1.CodObj != (ObjCnt != null ? ObjCnt.FldCodObj : 0) && m.Item1.GetType() == typeof(TblObj)).ToList();

            this.RecvFomMee = new ListCollectionView(lst);

            this.RecvFomMee.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            RaisePropertyChanged("RecvFomMee");
        }

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از فعالیت جاری  هستند
        /// </summary>
        public void DetectFomActCntObj()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = this.ActSrc.AllObjRstSentFromAct.Where(m => m.Item1.CodObj != (ObjCnt != null ? ObjCnt.FldCodObj : 0) && m.Item1.GetType() == typeof(TblObj)).ToList();

            this.RecvFomActCnt = new ListCollectionView(lst);

            this.RecvFomActCnt.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            RaisePropertyChanged("RecvFomActCnt");
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// ExecuteOutputDoesnExistCommand
        /// </summary>
        private void ExecuteOutputDoesnExistCommand()
        {
            this.Result = PopupResult.Yes;
        }

        /// <summary>
        /// تمامی فعالیت های مقصد تمامی خروجی های دریافت شده از من را به حالت عدم انتخاب تبدیل می کند
        /// </summary>
        private void ChangeIsSelectedOfFomMeToFalse()
        {
            foreach (TblAct act in this.RecvFomMee)
            {
                foreach (TblObj obj in act.AllObjOfAct)
                {
                    foreach (TblAct act1 in obj.ActTarget)
                    {
                        act1.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// تمامی فعالیت های مقصد تمامی خروجی های دریافت شده از فعالیت جاری را به حالت عدم انتخاب تبدیل می کند
        /// </summary>
        private void ChangeIsSelectedOfFomActCntToFalse()
        {
            foreach (TblAct act in this.RecvFomActCnt)
            {
                foreach (TblObj obj in act.AllObjOfAct)
                {
                    foreach (TblAct act1 in obj.ActTarget)
                    {
                        act1.IsSelected = false;
                    }
                }
            }
        }

        protected override bool CanOKExecute()
        {
            //return base.CanOKExecute();
            return FomActCntSlcEedItm != null
                || FomMeeSlcEedItm != null;
        }

        #endregion

        #region ' events '

        #endregion
    }
}
