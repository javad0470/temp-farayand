using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcPsnOsdOrgViewModel : PopupViewModel
    {
        #region ' Fields '

        ObservableCollection<TblPsn> _osdPsns;
        ListCollectionView _osdPsnsCV;
        string _txtSrch;

        #endregion

        #region ' Initialaizer '

        public SlcPsnOsdOrgViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = null;
        }

        #endregion

        #region ' Properties / Commands '

        public ListCollectionView OsdPsnsCV
        {
            get
            {
                loadPsns();

                if (_osdPsnsCV == null)
                {
                    _osdPsnsCV = new ListCollectionView(_osdPsns);
                    _osdPsnsCV.Filter = filterPsns;
                }

                return _osdPsnsCV;
            }
        }

        private void loadPsns()
        {
            if (_osdPsns == null)
            {
                var psns = PublicMethods.GetPsnOutsideOrg_22244(this.bpmnEty, PublicMethods.CurrentUser.FldCodOrg);
                psns.ForEach(p =>
                {
                    p.PropertyChanged -= p_PropertyChanged;
                    p.PropertyChanged += p_PropertyChanged;
                });
                _osdPsns = new ObservableCollection<TblPsn>(psns);
            }
        }

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = _txtSrch = value;

                if (_osdPsns == null)
                {
                    return;
                }

                RaisePropertyChanged("TxtSrch");

                OsdPsnsCV.Refresh();
            }
        }

        #endregion

        #region ' Public Methods '

        public void SelectItems(List<TblPsn> psns)
        {
            loadPsns();

            _osdPsns.ToList().ForEach(p =>
                {
                    //p.PropertyChanged -= p_PropertyChanged;
                    p.IsSelected = false;
                });
            psns.ToList().ForEach(p =>
            {
                //p.PropertyChanged -= p_PropertyChanged;
                //p.PropertyChanged += p_PropertyChanged;
                p.IsSelected = true;
            });
        }

        #endregion

        #region ' Private Methods '

        private bool filterPsns(object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            return (obj as TblPsn).Name.Trim().Contains(TxtSrch);
        }

        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (SelectionChanged != null)
            {
                if (e.PropertyName == "IsSelected")
                {
                    SelectionChanged(sender as TblPsn);
                }
            }
        }


        #endregion

        #region ' Events '

        public event Action<TblPsn> SelectionChanged;

        #endregion



    }
}
