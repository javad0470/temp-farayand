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
    public class DefPsnViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities _context;

        #endregion

        #region ' Initialaizer '

        public DefPsnViewModel()
        {
            DefPsnInsideOrgVM = new DefPsnInsideOrgViewModel();

            InsideSelected = true;


        }

        #endregion

        #region ' Properties / Commands '

        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }


        #region Access  
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterPsnIsdOrg
        {
            get
            {
                return this.Usr.AcsUsr["EnterPsnIsdOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterPsnOsdOrg
        {
            get
            {
                return this.Usr.AcsUsr["EnterPsnOsdOrg"];
            }
        }

        #endregion

        public DefPsnInsideOrgViewModel DefPsnInsideOrgVM { get; set; }

        public DefPsnOutsideOrgViewModel DefPsnOutsideOrgVM { get; set; }

        bool _insideSelected;

        public bool InsideSelected
        {
            get { return _insideSelected; }
            set
            {
                _insideSelected = value;
                if (!_insideSelected && DefPsnOutsideOrgVM == null)
                {
                    DefPsnOutsideOrgVM = new DefPsnOutsideOrgViewModel();
                    RaisePropertyChanged("DefPsnOutsideOrgVM");
                }
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
                DefPsnInsideOrgVM.SaveContext();
            }
            else
            {
                DefPsnOutsideOrgVM.SaveContext();
            }
        }

        public bool ConfirmAndClose()
        {
            if (InsideSelected)
            {
                return DefPsnInsideOrgVM.ConfirmAndClose();
            }
            else
            {
                return DefPsnOutsideOrgVM.ConfirmAndClose();
            }
        }
    }
}
