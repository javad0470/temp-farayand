using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
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
    public class SlcUntViewModel : PopupViewModel
    {
        #region ' Fields '

        private ObservableCollection<Model.TblUntMsrt> tblUntMsrt;
        //private object parent;
        private Model.TblUntMsrt selectedItem;

        string _txtSrch;
        private ListCollectionView _untMsrtCv;


        #endregion

        #region ' Initialaizer '

        public SlcUntViewModel()
            :base(new BPMNDBEntities())
        {
            UnitDoesntExistCommand = new DelegateCommand(ExecuteUnitDoesntExistCommand);
        }



        #endregion

        #region ' Properties / Commands '

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                if (_txtSrch != value)
                {
                    _txtSrch = value;
                    UntMsrtCv.Refresh();
                }
            }
        }

        /// <summary>
        /// gets all softwares define in the current organization
        /// </summary>
        public ObservableCollection<Model.TblUntMsrt> TblUntMsrt
        {
            get
            {
                tblUntMsrt = new ObservableCollection<Model.TblUntMsrt>(this.bpmnEty.TblUntMsrts);
                return tblUntMsrt;
            }
        }

        /// <summary>
        /// selected item in software data grid
        /// </summary>
        public Model.TblUntMsrt SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        /// <summary>
        /// UnitDoesntExistCommand
        /// </summary>
        public ICommand UnitDoesntExistCommand { get; set; }

        public ListCollectionView UntMsrtCv
        {
            get
            {
                if (TblUntMsrt == null)
                {
                    return null;
                }
                if (_untMsrtCv == null)
                {
                    _untMsrtCv = new ListCollectionView(TblUntMsrt);
                    _untMsrtCv.Filter = new Predicate<object>(filterUnts);
                }
                return _untMsrtCv;
            }
            set { _untMsrtCv = value; }
        }

        

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '
        private bool filterUnts(object obj)
        {
            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            var unt = obj as TblUntMsrt;

            if (unt == null)
            {
                return true;
            }


            return unt.Name.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
        }
        private void ExecuteUnitDoesntExistCommand()
        {
            this.Result = PopupResult.Yes;
        }

        #endregion
    }
}
