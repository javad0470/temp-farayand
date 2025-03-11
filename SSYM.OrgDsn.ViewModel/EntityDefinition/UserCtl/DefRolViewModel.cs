using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using System.Windows.Data;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefRolViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities _context;

        #endregion

        #region ' Initialaizer '

        public DefRolViewModel()
        {
            InsideSelected = true;

            DefRolOutsideOrgVM = new DefRolOsdViewModel();
            RaisePropertyChanged("DefRolOutsideOrgVM");
            
            DefRolInsideOrgVM = new DefRolIsdOrgViewModel();
            RaisePropertyChanged("DefRolInsideOrgVM");
        }

        #endregion

        #region ' Properties / Commands '



        public DefRolIsdOrgViewModel DefRolInsideOrgVM { get; set; }

        public DefRolOsdViewModel DefRolOutsideOrgVM { get; set; }

        bool _insideSelected;

        public bool InsideSelected
        {
            get { return _insideSelected; }
            set
            {
                _insideSelected = value;
                RaisePropertyChanged("InsideSelected");
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion


        public void SaveContext()
        {
            if (InsideSelected)
            {
                DefRolInsideOrgVM.SaveContext();
            }
            else
            {
                DefRolOutsideOrgVM.SaveContext();
            }
        }

        public bool ConfirmAndClose()
        {
            if (InsideSelected)
            {
                return DefRolInsideOrgVM.ConfirmAndClose();
            }
            else
            {
                return DefRolOutsideOrgVM.ConfirmAndClose();
            }
        }
    }
}
