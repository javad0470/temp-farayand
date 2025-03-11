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
using System.Windows;
using System.Windows.Data;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefItmsViewModel : SSYM.OrgDsn.ViewModel.Base.BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities _context;

        #endregion

        #region ' Initialaizer '

        public DefItmsViewModel(BPMNDBEntities ctx)
        {
            _context = ctx;

            DefErrVM = new DefErrViewModel(new BPMNDBEntities());

            DefSfwVM = new DefSfwViewModel(new BPMNDBEntities());

            DefUntMsrtVM = new DefUntMsrtViewModel(new BPMNDBEntities());

            DefIdxVM = new DefIdxViewModel(new BPMNDBEntities());
        }

        #endregion

        #region ' Properties / Commands '

        #endregion

        #region ' Public Methods '


        public DefErrViewModel DefErrVM { get; set; }

        public DefSfwViewModel DefSfwVM { get; set; }

        public DefUntMsrtViewModel DefUntMsrtVM { get; set; }

        public DefIdxViewModel DefIdxVM { get; set; }

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion


        public bool ConfirmAndClose()
        {
            return true;
        }

        public void SaveContext()
        {
            PublicMethods.SaveContext(_context);
        }
    }
}
