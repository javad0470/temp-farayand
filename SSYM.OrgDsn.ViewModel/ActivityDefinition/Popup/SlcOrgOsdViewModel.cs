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
    public class SlcOrgOsdViewModel : PopupViewModel
    {
        #region ' Fields '

        ObservableCollection<TblOrg> _osdOrgs;
        ListCollectionView _osdOrgsCV;
        string _txtSrch;

        #endregion

        #region ' Initialaizer '

        public SlcOrgOsdViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = null;
        }


        #endregion

        #region ' Properties / Commands '

        public ListCollectionView OsdOrgsCV
        {
            get
            {
                loadOrgs();

                if (_osdOrgsCV == null)
                {
                    _osdOrgsCV = new ListCollectionView(_osdOrgs);
                    _osdOrgsCV.Filter = filterOrgs;
                }

                return _osdOrgsCV;
            }
        }

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = _txtSrch = value;

                if (OsdOrgsCV == null)
                {
                    return;
                }

                RaisePropertyChanged("TxtSrch");

                OsdOrgsCV.Refresh();
            }
        }



        #endregion

        #region ' Public Methods '

        public void SelectItems(List<TblOrg> orgs)
        {
            loadOrgs();

            _osdOrgs.ToList().ForEach(o =>
                {
                    //o.PropertyChanged -= o_PropertyChanged;
                    o.IsListSelected = false;
                });
            orgs.ToList().ForEach(o =>
                {
                    //o.PropertyChanged -= o_PropertyChanged;
                    //o.PropertyChanged += o_PropertyChanged;
                    o.IsListSelected = true;
                });
        }

        private void loadOrgs()
        {
            if (_osdOrgs == null)
            {
                var Orgs = PublicMethods.DetectOrgNotSubOrgOfOrg_2073(bpmnEty, bpmnEty.TblOrgs.SingleOrDefault(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg));

                Orgs.ForEach(p =>
                {
                    p.PropertyChanged -= o_PropertyChanged;
                    p.PropertyChanged += o_PropertyChanged;
                });
                _osdOrgs = new ObservableCollection<TblOrg>(Orgs);
            }
        }

        #endregion

        #region ' Private Methods '

        private void o_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsListSelected")
            {
                if (SelectionChanged != null)
                {
                    SelectionChanged(sender as TblOrg);
                }
            }
        }

        private bool filterOrgs(object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            return (obj as TblOrg).Name.Trim().Contains(TxtSrch);
        }


        #endregion

        #region ' Events '

        public event Action<TblOrg> SelectionChanged;

        #endregion

    }
}
