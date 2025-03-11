using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork;
using System.Windows;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefPosPstViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities context;
        PosPstChartViewModel posPstChartVM;
        DtlPosPstViewModel _selectedPosPstDtlVM;


        #endregion

        #region ' Initialaizer '

        public DefPosPstViewModel()
        {
            context = new BPMNDBEntities();

            PosPstChartVM = new PosPstChartViewModel(context);

            PosPstChartVM.PosPstAdded += PosPstChartVM_PosPstAdded;

            SelectedPosPstDtlVM = new DtlPosPstViewModel(context);

            PosPstChartVM.PropertyChanged += PosPstChartVM_PropertyChanged;

            PosPstChartVM.CanUsrEditPosPst = true;
        }


        #endregion

        #region ' Properties / Commands '

        public PosPstChartViewModel PosPstChartVM
        {
            get { return posPstChartVM; }
            set
            {
                posPstChartVM = value;

                RaisePropertyChanged("PosPstChartVM");
            }
        }

        public DtlPosPstViewModel SelectedPosPstDtlVM
        {
            get { return _selectedPosPstDtlVM; }
            set
            {
                _selectedPosPstDtlVM = value;
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        void PosPstChartVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPosPst")
            {
                if (SelectedPosPstDtlVM.SelectedOrg != PosPstChartVM.SelectedOrg)
                {
                    SelectedPosPstDtlVM.SelectedOrg = PosPstChartVM.SelectedOrg;
                }
                SelectedPosPstDtlVM.SelectedPosPst = PosPstChartVM.SelectedPosPst;
            }
            if (e.PropertyName == "SelectedOrg")
            {
                SelectedPosPstDtlVM.SelectedOrg = PosPstChartVM.SelectedOrg;
            }
        }

        void PosPstChartVM_PosPstAdded(TblPosPstOrg obj)
        {
            SelectedPosPstDtlVM.AddCurrentPsnAsAgntOfPosPst(obj);
        }

        #endregion

        #region ' events '

        #endregion


        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);

            if (SelectedPosPstDtlVM.AgntChanged)
            {
                Util.ConfirmAndRestartApp();
            }
        }

        public bool ConfirmAndClose()
        {
            if (Util.HasContextChanges(this.context))
            {
                if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                {
                    this.SaveContext();
                    return true;
                }
                else
                {
                    PublicMethods.RollBackContext(this.context);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

    }
}
