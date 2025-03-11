using System.Windows;
using System.Windows.Forms.VisualStyles;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.Process.UserCtl;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class SttPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        string owrPrs;
        string namPrs;
        bool isFirstVisible;
        bool isSecondVisible;
        bool isThirdVisible;
        TblPr currentPrs;
        DisPrsViewModel _parent;

        #endregion

        #region ' Initialaizer '

        public SttPrsViewModel(BPMNDBEntities context, DisPrsViewModel parent)
            : base(context)
        {
            _parent = parent;
            ConsolidatePrsCommand = new DelegateCommand(ExecuteConsolidatePrsCommand);
            UnConsolidatePrsCommand = new DelegateCommand(ExecuteUnConsolidatePrsCommand);
            ConfirmChgOfPrsCommand = new DelegateCommand(ExecuteConfirmChgOfPrsCommand);
        }




        #endregion

        #region ' Properties / Commands '

        #region Access

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_CnstPrsWthOwrPrs
        {
            get
            {
                if (this.CurrentPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.CurrentPrs, "CnstPrsWthOwr", Model.Enum.TypRlnEtyMjrWthEtyMom.OwnerProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["CnstPrsWthOwrPrs"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_CnlCnstPrsWthOwrPrs
        {
            get
            {
                if (this.CurrentPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.CurrentPrs, "CnlCnstPrsWthOwr", Model.Enum.TypRlnEtyMjrWthEtyMom.OwnerProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["CnlCnstPrsWthOwrPrs"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_OkyChgPrsWthOwrPrs
        {
            get
            {
                if (this.CurrentPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.CurrentPrs, "OkyChgPrsWthOwr", Model.Enum.TypRlnEtyMjrWthEtyMom.OwnerProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["OkyChgPrsWthOwrPrs"];
            }
        }

        #endregion

        /// <summary>
        /// مالک فرآیند
        /// </summary>
        public string OwrPrs
        {
            get { return owrPrs; }
            set
            {
                owrPrs = value;
                RaisePropertyChanged("OwrPrs");
            }
        }

        /// <summary>
        /// فرآیند جاری
        /// </summary>
        public TblPr CurrentPrs
        {
            get { return currentPrs; }
            set
            {
                currentPrs = value;
                RaisePropertyChanged("CurrentPrs");//, "Acs_CnstPrsWthOwrPrs", "Acs_CnlCnstPrsWthOwrPrs", "Acs_OkyChgPrsWthOwrPrs");
            }
        }

        public bool IsFirstVisible
        {
            get { return isFirstVisible; }
            set
            {
                isFirstVisible = value;
                RaisePropertyChanged("IsFirstVisible");
                if (isFirstVisible)
                {
                    RaisePropertyChanged("Acs_CnstPrsWthOwrPrs");
                }
            }
        }

        public bool IsSecondVisible
        {
            get { return isSecondVisible; }
            set
            {
                isSecondVisible = value;
                RaisePropertyChanged("IsSecondVisible");
                if (isSecondVisible)
                {
                    RaisePropertyChanged("Acs_CnlCnstPrsWthOwrPrs");
                }
            }
        }

        public bool IsThirdVisible
        {
            get { return isThirdVisible; }
            set
            {
                isThirdVisible = value;
                RaisePropertyChanged("IsThirdVisible");

                if (isThirdVisible)
                {
                    RaisePropertyChanged("Acs_OkyChgPrsWthOwrPrs");
                }
            }
        }

        public bool PrsDeleted { get; set; }

        public ICommand ConsolidatePrsCommand { get; set; }

        public ICommand UnConsolidatePrsCommand { get; set; }

        public ICommand ConfirmChgOfPrsCommand { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private void ExecuteConsolidatePrsCommand()
        {
            var dsons = PublicMethods.DetectDsonsClaimedByNod_19020(bpmnEty, this.CurrentPrs.TblNod);

            // ناهمسانی های جایگاه جاری نمایش داده نشود
            dsons.RemoveAll(d => d.Item2.FldCodNod == this.CurrentPrs.TblNod.FldCodNod);

            if (dsons.Count > 0)
            {
                if (Util.ShowMessageBox(82) == MessageBoxResult.Yes)
                {
                    this.CurrentPrs.FldSttPrs = (int)Model.Enum.SttPrs.ConsolidatedNotEndorsed;
                    PublicMethods.SaveContext(bpmnEty);
                    this.Result = PopupResult.OK;
                }
                return;
            }
            if (Util.ShowMessageBox(11) == System.Windows.MessageBoxResult.Yes)
            {
                
                this.CurrentPrs.FldSttPrs = (int)Model.Enum.SttPrs.ConsolidatedEndorsed;

                PublicMethods.SaveContext(bpmnEty);

                this.Result = PopupResult.OK;
            }
        }

        private void ExecuteUnConsolidatePrsCommand()
        {
            if (Util.ShowMessageBox(12) == System.Windows.MessageBoxResult.Yes)
            {
                this.CurrentPrs.FldSttPrs = (int)Model.Enum.SttPrs.Raw;

                PublicMethods.ChangePrsAftrChangeSttPrs_21953(bpmnEty, this.CurrentPrs);

                PrsDeleted = true;

                PublicMethods.SaveContext(bpmnEty);

                _parent.Rebind(null);

                this.Result = PopupResult.OK;
            }
        }

        private void ExecuteConfirmChgOfPrsCommand()
        {
            var dsons = PublicMethods.DetectDsonsClaimedByNod_19020(bpmnEty, this.CurrentPrs.TblNod);

            // ناهمسانی های جایگاه جاری نمایش داده نشود
            dsons.RemoveAll(d => d.Item2.FldCodNod == this.CurrentPrs.TblNod.FldCodNod);

            if (dsons.Count > 0)
            {
                TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 83);
                MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                    msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                    (r) => { },
                    null);
                return;
            }

            if (Util.ShowMessageBox(13) == System.Windows.MessageBoxResult.Yes)
            {

                this.CurrentPrs.FldSttPrs = (int)Model.Enum.SttPrs.ConsolidatedEndorsed;

                PublicMethods.SaveContext(bpmnEty);

                this.Result = PopupResult.OK;
            }
        }




        #endregion

        #region ' events '

        #endregion

    }
}
