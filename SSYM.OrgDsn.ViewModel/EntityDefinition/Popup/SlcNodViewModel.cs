using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.Popup
{

    public enum FormMode
    {
        SlcOrgMode,
        SlcPosPstMode,
        SlcRolMode,
        SlcPosPst_RolMode
    }


    public class SlcNodViewModel : PopupViewModel
    {
        #region ' Fields '

        public BPMNDBEntities context;

        ListCollectionView _org;

        ObservableCollection<IEtyNod> _posPst;

        ObservableCollection<IEtyNod> _rol;

        ObservableCollection<IEtyNod> _selectedItems;

        IEtyNod _selectedItm;

        TblOrg _selectedOrgForPosPst;

        TblOrg _selectedOrgForRol;

        FormMode _mode;

        string _searchRolText;


        #endregion

        #region ' Initialaizer '

        public SlcNodViewModel(FormMode mode)
        {
            SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = null;

            this.context = new BPMNDBEntities();

            DetectAllOrg();

            this.Mode = mode;
        }

        #endregion

        #region ' Properties / Commands '

        public FormMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;

                RaisePropertyChanged("Mode");
            }
        }

        public ObservableCollection<IEtyNod> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                    _selectedItems = new ObservableCollection<IEtyNod>();
                return _selectedItems;
            }
        }
        public void addToSelectedItems(IEtyNod Obj)
        {
            if (!SelectedItems.Contains(Obj))
            {
                _selectedItems.Add(Obj);
            }
        }
        public void deleteFromSelectedItems(IEtyNod Obj)
        {
            if (SelectedItems.Contains(Obj))
            {
                _selectedItems.Remove(Obj);
            }
        }


        public ObservableCollection<IEtyNod> Rol
        {
            get { return _rol; }
            set
            {
                _rol = value;

                RaisePropertyChanged("Rol");
            }
        }

        public ObservableCollection<IEtyNod> PosPst
        {
            get { return _posPst; }
            set
            {
                _posPst = value;

                RaisePropertyChanged("PosPst");
            }
        }

        public ListCollectionView OrgCV
        {
            get { return _org; }
            set
            {
                _org = value;

                RaisePropertyChanged("Org");
            }
        }

        public IEtyNod SelectedItm
        {
            get { return _selectedItm; }
            set
            {
                _selectedItm = value;

                RaisePropertyChanged("SelectedItm");
            }
        }

        public TblOrg SelectedOrgForPosPst
        {
            get { return _selectedOrgForPosPst; }
            set
            {
                _selectedOrgForPosPst = value;

                RaisePropertyChanged("SelectedOrgForPosPst");

                DetectPosPst();
            }
        }

        public TblOrg SelectedOrgForRol
        {
            get { return _selectedOrgForRol; }
            set
            {
                _selectedOrgForRol = value;

                RaisePropertyChanged("SelectedOrgForRol");

                DetectRol();
            }
        }

        public string SearchRolText
        {
            get { return _searchRolText; }
            set
            {
                _searchRolText = value;
                RolCV.Refresh();
            }
        }

        public ListCollectionView RolCV { get; set; }

        public string SrchTxt
        {
            get { return _srchTxt; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = _srchTxt = value;

               /* if (_allOrgs == null)
                {
                    return;
                }*/
                RaisePropertyChanged("SrchTxt");
                foreach (var item in OrgCV.SourceCollection)
                {
                    (item as ISearchableTree).RefreshRec();
                }
            //    _allOrgs.ForEach(p => p.RefreshRec());
                OrgCV.Refresh();
                
            }
        }



        string txtSch;

        public string TxtSchPosPst
        {
            get { return txtSch; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = txtSch = value;

                RaisePropertyChanged("TxtSchPosPst");

                foreach (var item in PosPstCV.SourceCollection)
                {
                    (item as ISearchableTree).RefreshRec();
                }

                PosPstCV.Refresh();
            }
        }

        public ListCollectionView PosPstCV { get; set; }

        
        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private void DetectAllOrg()
        {
            var orgs = context.TblOrgs.Where(o => o.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList();

            orgs.ForEach(o => o.SetFilterMethodRec(SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch));

            this.OrgCV = new ListCollectionView(orgs);

            this.OrgCV.Filter = Utility.SearchAgnt.TreeSearch;
        }

        private void DetectPosPst()
        {
            if (this.SelectedOrgForPosPst != null)
            {
                this.PosPst = new ObservableCollection<IEtyNod>(this.SelectedOrgForPosPst.TblPosPstOrgs.Where(m => !m.FldCodUpl.HasValue));

                PosPst.ToList().ForEach(o => (o as Model.Base.ISearchableTree).SetFilterMethodRec(SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch));

                PosPstCV = new ListCollectionView(this.PosPst);

                this.PosPstCV.Filter = Utility.SearchAgnt.TreeSearch;

                RaisePropertyChanged("PosPstCV");
            }
        }

        private void DetectRol()
        {
            if (this.SelectedOrgForRol != null)
            {
                this.Rol = new ObservableCollection<IEtyNod>(this.SelectedOrgForRol.TblRols.Where(r => r.FldIsdOrg));

                RolCV = new ListCollectionView(this.Rol);
                RolCV.Filter = filterRols;
                RaisePropertyChanged("RolCV");
            }
        }
        List<Model.TblOrg> _allOrgs;
        ListCollectionView _orgSubCV;
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
        public void SelectItems(List<TblOrg> selectedOrgs)
        {
            if(_allOrgs == null)
                _allOrgs=this.context.TblOrgs.Where(E => E.FldCodOrg==UserManager.CurrentUser.FldCodOrg).ToList();
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

        private bool filterRols(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchRolText))
            {
                return true;
            }

            if (obj == null)
            {
                return true;
            }

            var obj1 = obj as IEtyNod;

            return obj1.Name.Trim().ToLower().Contains(SearchRolText.Trim().ToLower());
        }


        #endregion

        #region ' Events '
        public event Action<TblOrg> OrgSelectionChanged;
        #endregion


        public string _srchTxt { get; set; }
    }
}
