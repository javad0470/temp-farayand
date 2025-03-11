using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefIdxViewModel : PopupViewModel
    {
        #region ' Fields '
        
        ObservableCollection<TblSbjMsrt> sbjMsrt;

        TblSbjMsrt selectedSbjMsrt;

        TblIdx newIdx;

        string namIdx;


        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// 
        /// </summary>
        public DefIdxViewModel()
            : base(new BPMNDBEntities())
        {

            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            DetectAllSbjMsrt();
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public TblSbjMsrt SelectedSbjMsrt
        {
            get { return selectedSbjMsrt; }
            set
            {
                selectedSbjMsrt = value;

                RaiseOKCanExecute();

                RaisePropertyChanged("SelectedSbjMsrt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblIdx NewIdx
        {
            get { return newIdx; }
            set
            {
                newIdx = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NamIdx
        {
            get { return namIdx; }
            set
            {
                namIdx = value;

                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblSbjMsrt> SbjMsrt
        {
            get { return sbjMsrt; }
            set
            {
                sbjMsrt = value;

                RaisePropertyChanged("SbjMsrt");
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void DetectAllSbjMsrt()
        {
            this.SbjMsrt = new ObservableCollection<TblSbjMsrt>(bpmnEty.TblSbjMsrts);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OKExecute()
        {
            base.OKExecute();

            using (Model.BPMNDBEntities contex = new Model.BPMNDBEntities())
            {
                this.NewIdx = new TblIdx() { FldNamIdx = NamIdx, FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldCodSbjMsrt = SelectedSbjMsrt.FldCodSbjMsrt };
                contex.TblIdxes.AddObject(this.NewIdx);
                PublicMethods.SaveContext(contex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {
            if (this.SelectedSbjMsrt != null && this.NamIdx != null && this.NamIdx != "")
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
