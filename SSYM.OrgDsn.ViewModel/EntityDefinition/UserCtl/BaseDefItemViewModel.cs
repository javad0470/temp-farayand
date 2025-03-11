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
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public abstract class BaseDefItemViewModel<T1, T2> : SSYM.OrgDsn.ViewModel.Base.BaseViewModel
    {
        #region ' Fields '

        protected BPMNDBEntities _context;
        protected string _txtSrch;

        #endregion

        #region ' Initialaizer '

        public BaseDefItemViewModel(BPMNDBEntities ctx)
        {
            this._context = ctx;
            AddItmCommand = new DelegateCommand(addExecute, canAdd);
            DeleteCommand = new DelegateCommand<T1>(deleteExecute);
        }


        #endregion

        #region ' Properties / Commands '

        public DelegateCommand AddItmCommand { get; set; }

        public DelegateCommand<T1> DeleteCommand { get; set; }

        public ListCollectionView ItemsCV { get; set; }

        public ListCollectionView Items2CV { get; set; }

        public T1 SelectedItm { get; set; }

        public string TxtSrch
        {
            get
            {
                return _txtSrch;
            }
            set
            {
                _txtSrch = value;
                ItemsCV.CancelNew();
                ItemsCV.Refresh();
            }
        }

        public abstract string NamEty
        {
            get;
        }

        string _namItmAdding;

        public string NamItmAdding
        {
            get { return _namItmAdding; }
            set
            {
                _namItmAdding = value;
                AddItmCommand.RaiseCanExecuteChanged();
            }
        }

        string _namItmAdding2;

        public string NamItmAdding2
        {
            get { return _namItmAdding2; }
            set
            {
                _namItmAdding2 = value;
                AddItmCommand.RaiseCanExecuteChanged();
            }
        }


        public abstract string Lbl1
        {
            get;
        }

        public abstract string Lbl2
        {
            get;
        }


        public virtual Visibility ItmAdding2Visible
        {
            get
            {
                return Visibility.Visible;
            }
        }

        public T2 SelectedItmAdding2 { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        protected abstract bool filter(object obj);

        protected abstract void addExecute();

        protected virtual bool canAdd()
        {
            return !string.IsNullOrWhiteSpace(NamItmAdding)
    && !string.IsNullOrWhiteSpace(NamItmAdding2);
        }

        protected virtual void deleteExecute(T1 obj)
        {
            if (obj == null)
            {
                return;
            }
            if (Util.ShowMessageBox(2, (obj as INamedItm).Name) == MessageBoxResult.Yes)
            {
                try
                {
                    _context.DeleteObject(obj);
                    PublicMethods.SaveContext(_context);
                }
                catch (Exception)
                {
                    Util.ShowMessageBox(32, (obj as INamedItm).Name);
                    PublicMethods.RollBackContext(_context);
                    return;
                }
                ItemsCV.CancelNew();
                ItemsCV.Remove(obj);
            }
        }


        #endregion

        #region ' Events '

        #endregion

    }
}
