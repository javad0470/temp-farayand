using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcSrcAndDstViewModel : PopupViewModel
    {
        #region ' Fields '

        private bool isSelectionModeSingle;
        private string _txtSrchRols;
        private string _txtSrchOrg;
        string _srchOutOrgs;

        private ListCollectionView _posPstOrgCV;
        private List<TblPosPstOrg> _posPsts;
        private ObservableCollection<Model.TblRol> tblRol;
        private ObservableCollection<Model.TblRol> tblRolOut;
        private ObservableCollection<Model.TblPsn> tblPsn;
        private ObservableCollection<TblOrg> tblOrg;
        ListCollectionView _outOrgsCV;

        ListCollectionView _orgSubCV;
        List<Model.TblOrg> _allOrgs;

        private int entityCode;
        ObservableCollection<Model.TblRol> tblRolSelectedItems;
        Model.TblRol tblRolSelectedItem;

        ObservableCollection<Model.TblRol> tblRolOutSelectedItems;
        Model.TblRol tblRolOutSelectedItem;

        ObservableCollection<Model.TblPsn> tblPsnSelectedItems;
        Model.TblPsn tblPsnSelectedItem;

        ObservableCollection<Model.TblPosPstOrg> tblPosPstOrgSelectedItems;
        Model.TblPosPstOrg tblPosPstOrgSelectedItem;

        ObservableCollection<Model.TblOrg> tblOrgSelectedItems;
        Model.TblOrg tblOrgSelectedItem;

        ObservableCollection<Model.TblOrg> tblOrgSubSelectedItems;
        Model.TblOrg tblOrgSubSelectedItem;

        private List<IEtyNod> allSelectedItems;
        private IEtyNod _selectedItem;

        private bool isInsideOrganization;
        private bool isPosPstOrg;
        private bool isRolInsideOrganization;
        private bool isOrgSub;
        private bool isOrg;
        private bool isRolOutsideOrganization;
        private bool isPerson;
        private bool isOutOrgVisible = true;
        private bool isDepOrgVisible = true;

        private string txtSrchPosPst;

        List<TblOrg> _outOrgs;


        /// <summary>
        /// ایتمی که قرار است در لیست نمایش داده نشود
        /// </summary>
        private Tuple<int, FldTypEty> _excludedItem;

        #endregion

        #region ' Initialaizer '


        public SlcSrcAndDstViewModel()
            : base(new BPMNDBEntities())
        {
            Utility.SearchAgnt.SerachTerm = null;

            AllSelectedItems = new List<IEtyNod>();
            //Context = context;
            RaisePropertyChanged("TblPosPstOrg");
            RaisePropertyChanged("TblRol");
            RaisePropertyChanged("TblRolOut");
            RaisePropertyChanged("TblPsn");
            //OkCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand);
            //CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            TblRolCV = new ListCollectionView(this.bpmnEty.TblRols.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg && E.FldIsdOrg == true).ToList());

            TblRolCV.Filter = filterRols;
        }



        #endregion

        #region ' Properties / Commands '

        public string TxtSrchPosPst
        {
            get { return txtSrchPosPst; }
            set
            {
                if (txtSrchPosPst != value)
                {
                    txtSrchPosPst = value;
                    Utility.SearchAgnt.SerachTerm = value;
                    foreach (var item in PosPstOrgCV.SourceCollection)
                    {
                        (item as ISearchableTree).RefreshRec();
                    }
                    //_posPsts.ForEach(p => p.RefreshRec());
                    PosPstOrgCV.Refresh();
                }
            }
        }

        public string TxtSrchOrg
        {
            get { return _txtSrchOrg; }
            set
            {
                _txtSrchOrg = value;

                Utility.SearchAgnt.SerachTerm = value;

                _allOrgs.ForEach(o => o.RefreshRec());

                OrgSubCV.Refresh();
            }
        }


        public string SrchOutOrgs
        {
            get { return _srchOutOrgs; }
            set
            {
                _srchOutOrgs = value;
                Utility.SearchAgnt.SerachTerm = value;
                _outOrgs.ForEach(o => o.RefreshRec());
                OutOrgsCV.Refresh();
            }
        }


        /// <summary>
        /// ایتمی که قرار است در لیست نمایش داده نشود
        /// </summary>
        //public Tuple<int, FldTypEty> ExcludedItem
        //{
        //    get { return _excludedItem; }
        //    set
        //    {
        //        _excludedItem = value;

        //        switch (value.Item2)
        //        {
        //            case FldTypEty.PosPst:
        //                //tblPosPstOrg
        //                List<TblPosPstOrg> lst = new List<Model.TblPosPstOrg>
        //                                   (this.bpmnEty.TblPosPstOrgs.Where(E => E.FldCodPosPst != value.Item1 &&
        //                                   E.FldCodOrg == UserManager.CurrentUser.FldCodOrg &&
        //                                   !E.FldCodUpl.HasValue));

        //                removeFromPosPstRecurcive(lst, bpmnEty.TblPosPstOrgs.Single(m => m.FldCodPosPst == value.Item1));

        //                _posPstOrgCV = new ListCollectionView(lst);

        //                RaisePropertyChanged("TblPosPstOrg");

        //                break;
        //            case FldTypEty.Rol:

        //                tblRol = new ObservableCollection<Model.TblRol>(this.bpmnEty.TblRols.Where(E => E.FldCodRol != value.Item1 &&
        //                    E.FldCodOrg == UserManager.CurrentUser.FldCodOrg &&
        //                    E.FldIsdOrg == true));

        //                RaisePropertyChanged("TblRol");

        //                break;
        //            case FldTypEty.Psn:

        //                tblPsn = new ObservableCollection<Model.TblPsn>(
        //                    this.bpmnEty.TblPsns.Where(E => E.FldCodPsn == value.Item1));

        //                RaisePropertyChanged("TblPsn");

        //                break;
        //            case FldTypEty.Org:
        //                _outOrgsCV = new ObservableCollection<Model.TblOrg>(
        //                    this.bpmnEty.TblOrgs.Where(E => E.FldCodOrg != value.Item1 &&
        //                    E.FldCodOrg != UserManager.CurrentUser.FldCodOrg &&
        //                    !E.FldCodUpl.HasValue));

        //                RaisePropertyChanged("TblOrg");

        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        public IEtyNod SelectedItem
        {
            get { return _selectedItem; }
            set
            {

                _selectedItem = value;
                
                RaisePropertyChanged("SelectedItem");

                RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// IsSelectionModeSingle
        /// </summary>
        public bool IsSelectionModeSingle
        {
            get { return isSelectionModeSingle; }
            set
            {
                isSelectionModeSingle = value;
                RaisePropertyChanged("IsSelectionModeSingle");
            }
        }

        ///// <summary>
        ///// data context for this user control
        ///// </summary>
        //public SSYM.OrgDsn.Model.BPMNDBEntities Context { get; set; }

        /// <summary>
        /// gets all position and posts defined in the organization
        /// </summary>
        public ListCollectionView PosPstOrgCV
        {
            get
            {
                if (_posPstOrgCV == null)
                {
                    _posPsts = this.bpmnEty.TblPosPstOrgs.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg && !E.FldCodUpl.HasValue).ToList();

                    _posPsts.ForEach(p => PublicMethods.DetectSubOrgLevel(p, 100));

                    _posPsts.ForEach(p => p.SetFilterMethodRec(Utility.SearchAgnt.TreeSearch));

                    _posPstOrgCV = new ListCollectionView(_posPsts);

                    _posPstOrgCV.Filter = Utility.SearchAgnt.TreeSearch;

                    return _posPstOrgCV;
                }
                else
                {
                    return _posPstOrgCV;
                }
            }
        }

        /// <summary>
        /// قابل مشاهده بودن یا نبودن تب برون سازمانی
        /// </summary>
        public bool IsOutOrgVisible
        {
            get { return isOutOrgVisible; }
            set
            {
                isOutOrgVisible = value;
                RaisePropertyChanged("IsOutOrgVisible");
            }
        }


        /// <summary>
        /// قابل مشاهده بودن یا نبودن تب سازمان تابعه
        /// </summary>
        public bool IsDepOrgVisible
        {
            get { return isDepOrgVisible; }
            set
            {
                isDepOrgVisible = value;
                RaisePropertyChanged("IsDepOrgVisible");
            }
        }


        /// <summary>
        /// inside current organization roles
        /// </summary>
        public ListCollectionView TblRolCV { get; set; }

        /// <summary>
        /// outside current organization roles
        /// </summary>
        public ObservableCollection<Model.TblRol> TblRolOut
        {
            get
            {
                if (tblRolOut == null)
                {
                    return new ObservableCollection<Model.TblRol>(this.bpmnEty.TblRols.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg && E.FldIsdOrg == false));
                }
                else
                {
                    return tblRolOut;
                }
            }
        }

        public ObservableCollection<TblOrg> TblOrg
        {
            get
            {
                if (tblOrg == null)
                {
                    return new ObservableCollection<Model.TblOrg>(this.bpmnEty.TblOrgs.Where(e => e.TblOrg1.Count == 0 && e.TblOrg2 == null && e.FldCodOrg != 1));
                }
                else
                {
                    return tblOrg;
                }
            }
        }
        /// <summary>
        /// persons
        /// </summary>
        public ObservableCollection<Model.TblPsn> TblPsn
        {
            get
            {
                if (tblPsn == null)
                {
                    return tblPsn = new ObservableCollection<Model.TblPsn>(this.bpmnEty.TblPsns.Where(m => !m.FldIsdOrg));
                }
                else
                {
                    return tblPsn;
                }
            }
        }

        /// <summary>
        /// TblOrg
        /// </summary>
        public ListCollectionView OutOrgsCV
        {
            get
            {
                if (_outOrgsCV == null)
                {
                    _outOrgs = this.bpmnEty.TblOrgs.Where(E => E.FldCodOrg != UserManager.CurrentUser.FldCodOrg && !E.FldCodUpl.HasValue).ToList();
                    _outOrgs.ForEach(o => o.SetFilterMethodRec(Utility.SearchAgnt.TreeSearch));
                    _outOrgsCV = new ListCollectionView(_outOrgs);
                    _outOrgsCV.Filter = Utility.SearchAgnt.TreeSearch;
                }

                return _outOrgsCV;
            }
        }

        /// <summary>
        /// TblOrgSub
        /// </summary>
        public ListCollectionView OrgSubCV
        {
            get
            {
                if (_orgSubCV == null)
                {
                    _allOrgs = this.bpmnEty.TblOrgs.Where(E => E.FldCodUpl == UserManager.CurrentUser.FldCodOrg).ToList();

                    _allOrgs.ForEach(o => o.SetFilterMethodRec(Utility.SearchAgnt.TreeSearch));
                    _orgSubCV = new ListCollectionView(_allOrgs);
                    _orgSubCV.Filter = Utility.SearchAgnt.TreeSearch;
                }

                return _orgSubCV;

            }
        }

        /// <summary>
        /// TblRolOutSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblRol> TblRolOutSelectedItems
        {
            get
            {
                if (tblRolOutSelectedItems == null)
                {
                    tblRolOutSelectedItems = new ObservableCollection<Model.TblRol>();
                }
                return tblRolOutSelectedItems;
            }
            set
            {
                tblRolOutSelectedItems = value;
                RaisePropertyChanged("TblRolOutSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblRolOutSelectedItem
        /// </summary>
        //public Model.TblRol TblRolOutSelectedItem
        //{
        //    get
        //    {
        //        return tblRolOutSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblRolOutSelectedItem != value)
        //        {
        //            tblRolOutSelectedItem = value;
        //            RaisePropertyChanged("TblRolOutSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// TblRolOutSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblPsn> TblPsnSelectedItems
        {
            get
            {
                if (tblPsnSelectedItems == null)
                {
                    tblPsnSelectedItems = new ObservableCollection<Model.TblPsn>();
                }
                return tblPsnSelectedItems;
            }
            set
            {
                tblPsnSelectedItems = value;
                RaisePropertyChanged("TblPsnSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblRolOutSelectedItem
        /// </summary>
        //public Model.TblPsn TblPsnSelectedItem
        //{
        //    get
        //    {
        //        return tblPsnSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblPsnSelectedItem != value)
        //        {
        //            tblPsnSelectedItem = value;
        //            RaisePropertyChanged("TblPsnSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// TblRolSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblRol> TblRolSelectedItems
        {
            get
            {
                if (tblRolSelectedItems == null)
                {
                    tblRolSelectedItems = new ObservableCollection<Model.TblRol>();
                }
                return tblRolSelectedItems;
            }
            set
            {
                tblRolSelectedItems = value;
                RaisePropertyChanged("TblRolSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblRolSelectedItem
        /// </summary>
        //public Model.TblRol TblRolSelectedItem
        //{
        //    get
        //    {
        //        return tblRolSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblRolSelectedItem != value)
        //        {
        //            tblRolSelectedItem = value;
        //            RaisePropertyChanged("TblRolSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// TblPosPstOrgSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblPosPstOrg> TblPosPstOrgSelectedItems
        {
            get
            {
                if (tblPosPstOrgSelectedItems == null)
                {
                    tblPosPstOrgSelectedItems = new ObservableCollection<Model.TblPosPstOrg>();
                }
                return tblPosPstOrgSelectedItems;
            }
            set
            {
                tblPosPstOrgSelectedItems = value;
                RaisePropertyChanged("TblPosPstOrgSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblPosPstOrgSelectedItem
        /// </summary>
        //public Model.TblPosPstOrg TblPosPstOrgSelectedItem
        //{
        //    get
        //    {
        //        return tblPosPstOrgSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblPosPstOrgSelectedItem != value)
        //        {
        //            tblPosPstOrgSelectedItem = value;
        //            RaisePropertyChanged("TblPosPstOrgSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// TblOrgSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblOrg> TblOrgSelectedItems
        {
            get
            {
                if (tblOrgSelectedItems == null)
                {
                    tblOrgSelectedItems = new ObservableCollection<Model.TblOrg>();
                }
                return tblOrgSelectedItems;
            }
            set
            {
                tblOrgSelectedItems = value;
                RaisePropertyChanged("TblOrgSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblOrgSelectedItem
        /// </summary>
        //public Model.TblOrg TblOrgSelectedItem
        //{
        //    get
        //    {
        //        return tblOrgSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblOrgSelectedItem != value)
        //        {
        //            tblOrgSelectedItem = value;
        //            RaisePropertyChanged("TblOrgSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// TblOrgSubSelectedItems
        /// </summary>
        public ObservableCollection<Model.TblOrg> TblOrgSubSelectedItems
        {
            get
            {
                if (tblOrgSubSelectedItems == null)
                {
                    tblOrgSubSelectedItems = new ObservableCollection<Model.TblOrg>();
                }
                return tblOrgSubSelectedItems;
            }
            set
            {
                tblOrgSubSelectedItems = value;
                RaisePropertyChanged("TblOrgSubSelectedItems");
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
                //RebindAllSelectedItems();
            }
        }

        /// <summary>
        /// TblOrgSubSelectedItem
        /// </summary>
        //public Model.TblOrg TblOrgSubSelectedItem
        //{
        //    get
        //    {
        //        return tblOrgSubSelectedItem;
        //    }
        //    set
        //    {
        //        if (tblOrgSubSelectedItem != value)
        //        {
        //            tblOrgSubSelectedItem = value;
        //            RaisePropertyChanged("TblOrgSubSelectedItem");
        //        }
        //    }
        //}

        /// <summary>
        /// AllSelectedItems
        /// </summary>
        public List<Model.Base.IEtyNod> AllSelectedItems
        {
            get
            {
                return allSelectedItems;
            }
            set
            {
                allSelectedItems = value;
            }
        }

        ///// <summary>
        ///// ok command
        ///// </summary>
        //public ICommand OkCommand { get; set; }

        ///// <summary>
        ///// cancel command
        ///// </summary>
        //public ICommand CancelCommand { get; set; }

        /// <summary>
        /// IsPerson
        /// </summary>
        public bool IsPerson
        {
            get { return isPerson; }
            set
            {
                isPerson = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsRolOutsideOrganization
        /// </summary>
        public bool IsRolOutsideOrganization
        {
            get { return isRolOutsideOrganization; }
            set
            {
                isRolOutsideOrganization = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsOrg
        /// </summary>
        public bool IsOrg
        {
            get { return isOrg; }
            set
            {
                isOrg = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsOrgSub
        /// </summary>
        public bool IsOrgSub
        {
            get { return isOrgSub; }
            set
            {
                isOrgSub = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsRolInsideOrganization
        /// </summary>
        public bool IsRolInsideOrganization
        {
            get { return isRolInsideOrganization; }
            set
            {
                isRolInsideOrganization = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsPosPstOrg
        /// </summary>
        public bool IsPosPstOrg
        {
            get { return isPosPstOrg; }
            set
            {
                isPosPstOrg = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// IsInsideOrganization
        /// </summary>
        public bool IsInsideOrganization
        {
            get { return isInsideOrganization; }
            set
            {
                isInsideOrganization = value;
                (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }



        public string TxtSrchRols
        {
            get { return _txtSrchRols; }
            set
            {
                _txtSrchRols = value;
                this.TblRolCV.Refresh();
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool filterRols(object obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(TxtSrchRols))
            {
                return (obj as TblRol).Name.Trim().ToLower().Contains(TxtSrchRols.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void RebindAllSelectedItems()
        {
            //AllSelectedItems.Clear();
            //if (!IsSelectionModeSingle)
            //{
            //    AllSelectedItems.AddRange(TblPosPstOrgSelectedItems);

            //    AllSelectedItems.AddRange(TblRolSelectedItems);

            //    AllSelectedItems.AddRange(TblOrgSubSelectedItems);

            //    AllSelectedItems.AddRange(TblOrgSelectedItems);

            //    AllSelectedItems.AddRange(TblRolOutSelectedItems);

            //    AllSelectedItems.AddRange(TblPsnSelectedItems);
            //}
        }

        protected override void OKExecute()
        {
            base.OKExecute();
        }

        /// <summary>
        /// ExecuteOkCommand
        /// </summary>
        private void ExecuteOkCommand()
        {
            //AllSelectedItems.Clear();
            //if (!IsSelectionModeSingle)
            //{
            //    foreach (var item in TblPosPstOrgSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblRolSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblOrgSubSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblOrgSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblRolOutSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblPsnSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //}

            //AllSelectedItems.Clear();
            //if (!IsSelectionModeSingle)
            //{
            //    foreach (var item in TblPosPstOrgSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblRolSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblOrgSubSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblOrgSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblRolOutSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //    foreach (var item in TblPsnSelectedItems)
            //    {
            //        AllSelectedItems.Add(item);
            //    }
            //}
            //else
            //{
            //    if (IsInsideOrganization)
            //    {
            //        if (IsPosPstOrg)
            //        {
            //            AllSelectedItems.Add(TblPosPstOrgSelectedItems[0]);
            //        }
            //        if (IsRolInsideOrganization)
            //        {
            //            AllSelectedItems.Add(TblRolSelectedItems[0]);
            //        }
            //        if (IsOrgSub)
            //        {
            //            AllSelectedItems.Add(TblOrgSubSelectedItems[0]);
            //        }
            //    }
            //    else
            //    {
            //        if (IsOrg)
            //        {
            //            AllSelectedItems.Add(TblOrgSelectedItems[0]);
            //        }
            //        if (IsRolOutsideOrganization)
            //        {
            //            AllSelectedItems.Add(TblRolOutSelectedItems[0]);
            //        }
            //        if (IsPerson)
            //        {
            //            AllSelectedItems.Add(TblPsnSelectedItems[0]);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// ExecuteCancelCommand
        /// </summary>
        //private void ExecuteCancelCommand()
        //{
        //}

        /// <summary>
        /// CanExecuteOkCommand
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteOkCommand()
        {
            //if (IsSelectionModeSingle)
            //{
            //    if (IsInsideOrganization)
            //    {
            //        if (IsPosPstOrg)
            //        {
            //            if (TblOrgSelectedItems.Count > 0)
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            if (IsRolInsideOrganization)
            //            {
            //                if (TblRolSelectedItems.Count > 0)
            //                {
            //                    return true;
            //                }
            //                else
            //                {
            //                    return false;
            //                }
            //            }
            //            else
            //            {
            //                if (TblOrgSubSelectedItems.Count > 0)
            //                {
            //                    return true;
            //                }
            //                else
            //                {
            //                    return false;
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (IsOrg)
            //        {
            //            if (TblOrgSelectedItems.Count > 0)
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //        else
            //        {
            //            if (IsRolOutsideOrganization)
            //            {
            //                if (TblRolOutSelectedItems.Count > 0)
            //                {
            //                    return true;
            //                }
            //                else
            //                {
            //                    return false;
            //                }
            //            }
            //            else
            //            {
            //                if (TblPsnSelectedItems.Count > 0)
            //                {
            //                    return true;
            //                }
            //                else
            //                {
            //                    return false;
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (TblOrgSelectedItems.Count > 0 || TblOrgSubSelectedItems.Count > 0 || TblPosPstOrgSelectedItems.Count > 0 || TblPsnSelectedItems.Count > 0 || TblRolOutSelectedItems.Count > 0 || TblRolSelectedItems.Count > 0)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}

            return true;
        }


        /// <summary>
        /// این تابع به صورت بازگشتی جایگاه مورد نظر را از لیست حذف میکند
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="removingItem"></param>
        /// <returns></returns>
        private bool removeFromPosPstRecurcive(List<TblPosPstOrg> sourceList, TblPosPstOrg removingItem)
        {
            foreach (var item in sourceList)
            {
                if (item.FldCodPosPst == removingItem.FldCodPosPst)
                {
                    item.TblPosPstOrg2.TblPosPstOrg1.Remove(removingItem);
                    return true;
                }
                else
                {
                    bool removed = removeFromPosPstRecurcive(item.TblPosPstOrg1.ToList(), removingItem);
                    if (removed)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        #endregion



    }
}
