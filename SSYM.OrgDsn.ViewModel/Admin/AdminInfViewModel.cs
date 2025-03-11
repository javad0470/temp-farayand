using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.Admin
{
    public class AdminInfViewModel:PopupViewModel
    {
        private TblPsn _psn;
        public BPMNDBEntities context;
        public AdminInfViewModel()
        {
            this.context = new BPMNDBEntities();
        }


        public TblPsn SelectedPerson
        {
            get
            {
                if (_psn == null)
                {
                    _psn = context.TblUsrs.SingleOrDefault(u => u.FldNamUsr == "admin").TblPsn;
                    
                    _psn.PropertyChanged += _psn_PropertyChanged;
                }
                return _psn;
            }
            set
            {
                _psn = value;
                _psn.PropertyChanged += _psn_PropertyChanged;
                (OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedPerson");
            }
        }

        void _psn_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //OKCommand.CanExecute(null);
            RaisePropertyChanged("OKEnabled");
        }



        protected override void OKExecute()
        {
            PublicMethods.SaveContext(context);
            Result=PopupResult.OK;
        }

        protected override bool CanOKExecute()
        {
            return ValidAdminInfo();
        }

        private bool ValidateNamePsn(string input)
        {
            if (string.IsNullOrEmpty(input) || input[0] == ' ')
            {
                return false;
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] <= '9' && input[i] >= '0')
                    return false;
            }

            return true;
        }

        public bool ValidAdminInfo()
        {
            bool temp = ValidateNamePsn(SelectedPerson.FldNam1stPsn) && ValidateNamePsn(SelectedPerson.FldNam2ndPsn);
            temp = temp &
                   (SelectedPerson.FldNam1stPsn != "راهبر" || SelectedPerson.FldNam2ndPsn != "سیستم" ||
                    !string.IsNullOrEmpty(SelectedPerson.FldNamFtr) || !string.IsNullOrEmpty(SelectedPerson.FldNumIdfn) ||
                    !string.IsNullOrEmpty(SelectedPerson.FldNumNtl));
            return temp;
        }
    }
}
