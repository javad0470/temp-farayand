using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Data;
using System.Reflection;
using System.Collections;
using SSYM.OrgDsn.ViewModel.Base;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;


namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class EvtSrtInnSgmtTimViewModel : UserControlViewModel
    {
        #region ' Fields '

        bool isRglr;
        bool isSgmtTimRnd;
        bool popupIsOpen;
        bool popupIsOpenForChange;

        Model.TblEvtSrt_InnSgmtTim tblEvtSrt_InnSgmtTim;

        ObservableCollection<SSYM.OrgDsn.Model.TblSgmtTim> tblSgmtTims;

        #endregion

        #region ' Initialaizer '

        public EvtSrtInnSgmtTimViewModel(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();

            SlcSgmtTim = new SlcSgmtTimViewModel();

            NewSgmtTimCommand = new DelegateCommand(ExcecuteNewSgmtTimCommand, CanExecuteNewSgmtTimCommand);

            ChangeSgmtTimCommand = new DelegateCommand<SSYM.OrgDsn.Model.TblSgmtTim>(ExcecuteChangeSgmtTimCommand, CanExecuteChangeSgmtTimCommand);

            DeleteCommand = new DelegateCommand<SSYM.OrgDsn.Model.TblSgmtTim>(ExcecuteDeleteCommand);

            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// منظم/ نامنظم
        /// </summary>
        public bool IsRglr
        {
            get
            {
                try
                {
                    isRglr = this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim == (int)Model.Enum.TypeOfSgmtTim.Regular;
                    return isRglr;
                }
                catch (Exception e)
                {
                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Error", e));
                    return true;
                }
            }
            set
            {
                try
                {
                    if (value && !isRglr && this.TblEvtSrt_InnSgmtTim.TblSgmtTims.Count > 0)
                    {
                        if (Util.ShowMessageBox(2, "مقاطع زمانی ثبت شده") == System.Windows.MessageBoxResult.Yes)
                        {
                            this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.Regular;
                            List<Model.TblSgmtTim> tbl = new List<TblSgmtTim>(this.TblEvtSrt_InnSgmtTim.TblSgmtTims);
                            for (int i = 0; i < tbl.Count; i++)
                            {
                                this.bpmnEty.DeleteObject(tbl[i]);
                            }
                            isRglr = value;
                            RaisePropertyChanged("IsRglr", "TblSgmtTims");
                            this.IsSgmtTimRnd = true;
                        }
                    }
                    else if (value && !isRglr && this.TblEvtSrt_InnSgmtTim.TblSgmtTims.Count == 0)
                    {
                        this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.Regular;
                        isRglr = value;
                        RaisePropertyChanged("IsRglr", "TblSgmtTims");
                        this.IsSgmtTimRnd = true;
                    }
                    else if (!value)
                    {
                        this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.RandomTimeSegments;
                        isRglr = value;
                        RaisePropertyChanged("IsRglr", "TblSgmtTims");

                    }

                }
                catch (System.Data.OptimisticConcurrencyException e)
                {

                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Error", e));
                }
                catch (Exception e)
                {

                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Error", e));
                }
            }
        }

        /// <summary>
        /// TblEvtSrt_InnSgmtTim
        /// </summary>
        public Model.TblEvtSrt_InnSgmtTim TblEvtSrt_InnSgmtTim
        {
            get { return Entity as TblEvtSrt_InnSgmtTim; }
            set
            {
                Entity = value;
                RaisePropertyChanged("TblEvtSrt_InnSgmtTim", "SelectedTime", "SelectedItem");
            }
        }

        /// <summary>
        /// زمان انتخاب شده در حالت منظم
        /// </summary>
        public TimeSpan SelectedTime
        {
            get
            {
                //return this.TblEvtSrt_InnSgmtTim.FldDteSrtSchm.Value.TimeOfDay;
                return new TimeSpan();
            }
            set
            {
                this.TblEvtSrt_InnSgmtTim.FldDteSrtSchm = new DateTime(this.TblEvtSrt_InnSgmtTim.FldDteSrtSchm.Value.Year, this.TblEvtSrt_InnSgmtTim.FldDteSrtSchm.Value.Month, this.TblEvtSrt_InnSgmtTim.FldDteSrtSchm.Value.Day, value.Hours, value.Minutes, value.Seconds);
            }
        }

        /// <summary>
        /// مقاطع زمانی تصادفی/ مقاطع زمانی معین
        /// </summary>
        public bool IsSgmtTimRnd
        {
            get
            {
                try
                {
                    isSgmtTimRnd = this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim == (int)Model.Enum.TypeOfSgmtTim.RandomTimeSegments;
                    return isSgmtTimRnd;
                }
                catch (Exception e)
                {
                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Error", e));
                    return true;
                }
            }
            set
            {
                if (value && !isSgmtTimRnd && this.TblEvtSrt_InnSgmtTim.TblSgmtTims.Count > 0)
                {
                    if (Util.ShowMessageBox(2, "مقاطع زمانی ثبت شده") == System.Windows.MessageBoxResult.Yes)
                    {
                        this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.RandomTimeSegments;
                        List<Model.TblSgmtTim> tbl = new List<TblSgmtTim>(this.TblEvtSrt_InnSgmtTim.TblSgmtTims);
                        for (int i = 0; i < tbl.Count; i++)
                        {
                            this.bpmnEty.DeleteObject(tbl[i]);
                        }
                        isSgmtTimRnd = value;
                        RaisePropertyChanged("IsSgmtTimRnd", "TblSgmtTims");

                    }
                }
                else if (value && !isSgmtTimRnd && this.TblEvtSrt_InnSgmtTim.TblSgmtTims.Count == 0)
                {
                    this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.RandomTimeSegments;
                    isSgmtTimRnd = value;
                    RaisePropertyChanged("IsSgmtTimRnd", "TblSgmtTims");
                }
                else if (!value)
                {
                    this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = (int)Model.Enum.TypeOfSgmtTim.SpecificTimeSegments;
                    RaisePropertyChanged("IsSgmtTimRnd", "TblSgmtTims");
                }
                //this.TblEvtSrt_InnSgmtTim.FldTypSgmtTim = value == true ? (int)Model.Enum.TypeOfSgmtTim.RandomTimeSegments : (int)Model.Enum.TypeOfSgmtTim.SpecificTimeSegments;
                //isSgmtTimRnd = value;
                //RaisePropertyChanged("IsSgmtTimRnd", "TblSgmtTims");
            }
        }

        /// <summary>
        /// بازه های زمانی
        /// </summary>
        public IEnumerable Time
        {
            get
            {
                try
                {
                    return bpmnEty.TblItmFixSfws.Where(I => I.FldCodSbj == 3).AsEnumerable();
                }
                catch (Exception e)
                {

                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Eror", e));
                    return null;
                }
            }
            set
            {
                TblItmFixSfw tbl = (TblItmFixSfw)value;
                this.TblEvtSrt_InnSgmtTim.FldTypDurtSchmTim = tbl.FldCodItm;
            }
        }

        /// <summary>
        /// نوع زمان انتخاب شده برای حالت منظم
        /// </summary>
        public TblItmFixSfw SelectedItem
        {
            get
            {
                return bpmnEty.TblItmFixSfws.SingleOrDefault(I => I.FldCodSbj == 3 && I.FldCodItm == this.TblEvtSrt_InnSgmtTim.FldTypDurtSchmTim);
            }
            set
            {
                this.TblEvtSrt_InnSgmtTim.FldTypDurtSchmTim = value.FldCodItm;
            }
        }

        /// <summary>
        /// مقاطع زمانی
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblSgmtTim> TblSgmtTims
        {
            get
            {
                return new ObservableCollection<TblSgmtTim>(this.TblEvtSrt_InnSgmtTim.TblSgmtTims);
            }
            set
            {
                tblSgmtTims = value;
                RaisePropertyChanged("TblSgmtTims");
            }
        }

        /// <summary>
        /// SlcSgmtTimViewModel
        /// </summary>
        public SlcSgmtTimViewModel SlcSgmtTim { get; set; }

        /// <summary>
        /// مقطع زمانی جدید
        /// </summary>
        public ICommand NewSgmtTimCommand { get; set; }

        /// <summary>
        /// تغییر مقطع زمانی
        /// </summary>
        public ICommand ChangeSgmtTimCommand { get; set; }

        /// <summary>
        /// PopupIsOpen
        /// </summary>
        public bool PopupIsOpen
        {
            get { return popupIsOpen; }
            set
            {
                popupIsOpen = value;
                if (!value)
                {
                    RaisePropertyChanged("TblSgmtTims");
                }
                RaisePropertyChanged("PopupIsOpen");
                (NewSgmtTimCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// حذف مقطع زمانی
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// ذخیره سازی تغییرات
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }


        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            SlcSgmtTim.Dispose();
        }

        #endregion

        #region ' Private Methods '

        private void ExcecuteDeleteCommand(Model.TblSgmtTim obj)
        {
            if (Util.ShowMessageBox(44) == System.Windows.MessageBoxResult.Yes)
            {
                bpmnEty.DeleteObject(obj);
                RaisePropertyChanged("TblSgmtTims");
            }
        }

        private void ExcecuteChangeSgmtTimCommand(Model.TblSgmtTim obj)
        {

            SlcSgmtTim.Parent = this;
            SlcSgmtTim.PrimaryValue = new TblSgmtTim() { FldDteTim = obj.FldDteTim };
            SlcSgmtTim.SelectedDateTime = obj;
            PopupIsOpen = true;
        }

        private bool CanExecuteChangeSgmtTimCommand(Model.TblSgmtTim arg)
        {
            return !PopupIsOpen;
        }

        private bool CanExecuteNewSgmtTimCommand()
        {
            return !PopupIsOpen;
        }

        private void ExcecuteNewSgmtTimCommand()
        {
            //SlcSgmtTim.Parent = this;
            //SlcSgmtTim.PrimaryValue = null;
            //SlcSgmtTim.SelectedDateTime = new TblSgmtTim();
            //PopupIsOpen = true;
            this.TblEvtSrt_InnSgmtTim.TblSgmtTims.Add(new TblSgmtTim() { FldDteTim = DateTime.Now });
            RaisePropertyChanged("TblSgmtTims");
        }

        private void ExecuteSaveChangesCommand()
        {
            PublicMethods.SaveContext(bpmnEty);
        }



        #endregion
    }
}
