using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefSfwViewModel : PopupViewModel
    {
        #region ' Fields '

        string namSfw;

        #endregion

        #region ' Initialaizer '

        public DefSfwViewModel()
            : base(new BPMNDBEntities())
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            TblSfw = new TblSfw();
            this.TblSfw.PropertyChanged += TblSfw_PropertyChanged;
        }

        void TblSfw_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldNamSfw")
            {
                RaiseOKCanExecute();
            }
        }

        #endregion

        #region ' Properties / Commands '


        /// <summary>
        /// software
        /// </summary>
        public Model.TblSfw TblSfw
        {
            get;
            set;
        }


        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        protected override void OKExecute()
        {
            base.OKExecute();

            //this.TblSfw = new Model.TblSfw() { FldCodOrg = UserManager.CurrentUser.FldCodOrg, FldNamSfw = NamSfw };
            //bpmnEty.TblSfws.AddObject(this.TblSfw);
            //PublicMethods.SaveContext(this.bpmnEty);
        }

        protected override bool CanOKExecute()
        {
            return !this.TblSfw.HasErrors;
        }


        #endregion

        #region ' events '

        #endregion

    }
}
