using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcSgmtTimViewModel : PopupViewModel
    {
        #region ' Fields '

        //ObservableCollection<SSYM.OrgDsn.Model.TblSgmtTim> tblSgmtTim;
        //SSYM.OrgDsn.Model.TblSgmtTim selectedRowTblSgmtTim;
        //SSYM.OrgDsn.Model.TblSgmtTim primaryValue;
        private EvtSrtInnSgmtTimViewModel parent;
        private Model.TblSgmtTim selectedDateTime;
        private Model.TblSgmtTim primaryValue;




        #endregion

        #region ' Initialaizer '

        public SlcSgmtTimViewModel()
        {
            //primaryValue = new Model.TblSgmtTim();
            //primaryValue = SelectedRowTblSgmtTim;
            OkAndCloseCommand = new DelegateCommand(ExecuteOkAndCloseCommand);
            CancelAndCloseCommand = new DelegateCommand(ExecuteCancelAndCloseCommand);
        }

        #endregion

        #region ' Properties / Commands '
                
        /// <summary>
        /// primary value for selected date and time
        /// </summary>
        public Model.TblSgmtTim PrimaryValue
        {
            get { return primaryValue; }
            set { primaryValue = value; }
        }

        /// <summary>
        /// selected date and time
        /// </summary>
        public Model.TblSgmtTim SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                selectedDateTime = value;
                RaisePropertyChanged("SelectedDateTime");
            }
        }

        /// <summary>
        /// parent of this user control
        /// </summary>
        public EvtSrtInnSgmtTimViewModel Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }
        
        /// <summary>
        /// تأیید
        /// </summary>
        public ICommand OkAndCloseCommand { get; set; }

        /// <summary>
        /// لغو
        /// </summary>
        public ICommand CancelAndCloseCommand { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// لغو و بستن این فرم
        /// </summary>
        private void ExecuteCancelAndCloseCommand()
        {
            if (PrimaryValue != null)
            {
                selectedDateTime.FldDteTim = PrimaryValue.FldDteTim;
            }
            Parent.PopupIsOpen = false;

        }

        /// <summary>
        /// تأیید و بستن این فرم
        /// </summary>
        private void ExecuteOkAndCloseCommand()
        {
            if (PrimaryValue == null)
            {
                Parent.TblEvtSrt_InnSgmtTim.TblSgmtTims.Add(this.SelectedDateTime);
            }
            Parent.PopupIsOpen = false;
        }


        #endregion
    }
}
