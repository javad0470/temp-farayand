using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcIdxViewModel : PopupViewModel
    {
        #region ' Fields '

        ObservableCollection<TblIdx> allIdx;

        TblIdx selectedIdx;

        #endregion

        #region ' Initialaizer '

        public SlcIdxViewModel()
            :base(new BPMNDBEntities())
        {
            DetectAllIdx();

            IdxDoesntExistCommand = new DelegateCommand(ExecuteIdxDoesntExistCommand);
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// لیست تمامی شاخص ها
        /// </summary>
        public ObservableCollection<TblIdx> AllIdx
        {
            get { return allIdx; }
            set { allIdx = value; }
        }

        /// <summary>
        /// شاخص انتخاب شده
        /// </summary>
        public TblIdx SelectedIdx
        {
            get { return selectedIdx; }
            set
            {
                selectedIdx = value;

                RaisePropertyChanged("SelectedIdx");
            }
        }

        /// <summary>
        /// IdxDoesntExistCommand
        /// </summary>
        public ICommand IdxDoesntExistCommand { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// شناسایی تمامی شاخص ها
        /// </summary>
        private void DetectAllIdx()
        {
            this.AllIdx = new ObservableCollection<TblIdx>(this.bpmnEty.TblIdxes.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteIdxDoesntExistCommand()
        {
            this.Result = PopupResult.Yes;
        }




        #endregion

        #region ' Events '

        #endregion

    }
}
