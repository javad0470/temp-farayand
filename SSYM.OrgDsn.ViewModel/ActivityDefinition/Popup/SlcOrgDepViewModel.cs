using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcOrgDepViewModel : PopupViewModel
    {
        #region ' Fields '

        ListCollectionView _orgSubCV;
        List<Model.TblOrg> _allOrgs;
        string _txtSrchOrg;
        #endregion

        #region ' Initialaizer '
        public SlcOrgDepViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            //_allOrgs
        }

        #endregion

        #region ' Properties / Commands '

        public string TxtSrchOrg
        {
            get { return _txtSrchOrg; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = _txtSrchOrg = value;

                if (_allOrgs == null)
                {
                    return;
                }

                RaisePropertyChanged("TxtSch");

                _allOrgs.ForEach(p => p.RefreshRec());

                OrgSubCV.Refresh();
            }
        }

        public ListCollectionView OrgSubCV
        {
            get
            {
                if (_orgSubCV == null)
                {
                    if (_allOrgs == null)
                    {
                        _allOrgs = this.bpmnEty.TblOrgs.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg).ToList();
                    }
                    _allOrgs.ForEach(o =>
                    {
                        o.SetFilterMethodRec(Utility.SearchAgnt.TreeSearch);
                    });
                    _allOrgs.First().GetSubOrgs().ForEach(o =>
                        {
                            o.PropertyChanged -= o_PropertyChanged;
                            o.PropertyChanged += o_PropertyChanged;
                        });

                    _orgSubCV = new ListCollectionView(_allOrgs);
                    _orgSubCV.Filter = Utility.SearchAgnt.TreeSearch;
                }

                return _orgSubCV;

            }
        }

        #endregion

        #region ' Public Methods '

        public void SelectItems(List<TblOrg> selectedOrgs)
        {
            if (_allOrgs == null)
            {
                _allOrgs = this.bpmnEty.TblOrgs.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg).ToList();
            }

            var allOrgs = _allOrgs.First().GetSubOrgs();

            allOrgs.ForEach(o =>
            {
                o.PropertyChanged -= o_PropertyChanged;
                o.IsSelectedInTree = false;
            });

            allOrgs.ForEach(o =>
                        {
                            o.PropertyChanged += o_PropertyChanged;
                        });
            allOrgs.Intersect(selectedOrgs).ToList().ForEach(o => o.IsSelectedInTree = true);


        }

        #endregion

        #region ' Private Methods '

        void o_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelectedInTree")
            {
                raiseSelectionChanged(sender as TblOrg);
            }
        }

        void raiseSelectionChanged(TblOrg org)
        {
            if (OrgSelectionChanged != null)
            {
                OrgSelectionChanged(org);
            }
        }

        #endregion

        #region ' Events '

        public event Action<TblOrg> OrgSelectionChanged;

        #endregion

    }
}
