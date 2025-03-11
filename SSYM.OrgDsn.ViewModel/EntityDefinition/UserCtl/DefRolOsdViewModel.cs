using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefRolOsdViewModel : NotificationObject, IViewModel
    {
        #region ' Fields '

        Model.BPMNDBEntities context;
        private ObservableCollection<Model.TblRol> tblRolOutsideOrg;
        DtlRolIsdOrgViewModel _dtlRolVM;
        TblRol _selectedRol;


        private string _searchRolText { get; set; }

        //private TblPsn _selectedPsn;
        //private TblOrg _selectedOrg;
        SlcPsnAndOrgOsdViewModel _slcPsnOrgOsdVM;


        #endregion

        #region ' Initialaizer '

        public DefRolOsdViewModel()
        {
            context = new Model.BPMNDBEntities();
            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            //SearchOrgs = new DelegateCommand(ExecuteSearchOrgs);
            //SearchPsns = new DelegateCommand(ExecuteSearchPsns);
            AddRol = new DelegateCommand(ExecuteAddRol);

            //OrgOutsideOrgCV = new ListCollectionView(PublicMethods.DetectOrgNotSubOrgOfOrg_2073(context, this.context.TblOrgs.SingleOrDefault(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg)));

            //OrgOutsideOrgCV.Filter = filterOrgs;

            //List<TblPsn> psns = PublicMethods.GetPsnOutsideOrg_22244(this.context, PublicMethods.CurrentUser.FldCodOrg);

            //PsnOutsideOrgCV = new ListCollectionView(psns);

            //PsnOutsideOrgCV.Filter = searchPsns;


            DeleteRoleCommand = new DelegateCommand<TblRol>(ExecuteDeleteRoleCommand);

            DeletePlyrRolCommand = new DelegateCommand<TblPlyrRol>(ExecuteDeletePlyrRolCommand);

            SlcPsnOrgCommand = new DelegateCommand<TblRol>(slcPsnExecute, canSlcPsnCommand);

            _slcPsnOrgOsdVM = new SlcPsnAndOrgOsdViewModel(context);

            DtlRolVM = new DtlRolIsdOrgViewModel(context) { IsIsd = false };
        }


        #endregion

        #region ' Properties / Commands '

        #region Access

        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterRolOsdOrg
        {
            get
            {
                //اگر شخص جاری نماینده سازمان جاری نیست
                if (!PublicMethods.GetAgntOfPsnIsdOrg_22230(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(a => a.TblNod.EtyNod.TypEty == AllTypEty.Org))
                {
                    return false;
                }
                else
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "RolOsdOrg", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    return this.Usr.AcsUsr["EnterRolOsdOrg"];
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddRol
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "RolOsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["AddRolOsdOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelRol
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "RolOsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["DelRolOsdOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditRol
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "RolOsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["EditRolOsdOrg"];
            }
        }

        #endregion

        /// <summary>
        /// سازمان های بیرونی
        /// </summary>
        //public ListCollectionView OrgOutsideOrgCV
        //{
        //    get;

        //    set;

        //}

        /// <summary>
        /// نقش های برون سازمانی
        /// </summary>
        public ObservableCollection<Model.TblRol> TblRolOutsideOrg
        {
            get
            {
                if (tblRolOutsideOrg == null)
                {
                    tblRolOutsideOrg = new ObservableCollection<Model.TblRol>(context.TblRols.Where(E => !E.FldIsdOrg && E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg));
                }

                return tblRolOutsideOrg;
            }
            set
            {
                tblRolOutsideOrg = value;

                if (RolOutsideCV == null)
                {
                    RolOutsideCV = new ListCollectionView(tblRolOutsideOrg);
                    RolOutsideCV.Filter = filterRols;

                    RaisePropertyChanged("RolOutsideCV");
                }
                SelectedRol = tblRolOutsideOrg.FirstOrDefault();
            }

        }

        public ICommand SlcPsnOrgCommand { get; set; }

        ListCollectionView _rolOutsideCV;
        public ListCollectionView RolOutsideCV
        {
            get
            {
                if (_rolOutsideCV == null)
                {
                    _rolOutsideCV = new ListCollectionView(TblRolOutsideOrg);
                    _rolOutsideCV.Filter = filterRols;
                }
                return _rolOutsideCV;
            }
            set { _rolOutsideCV = value; }
        }

        /// <summary>
        /// اشخاص برون سازمانی
        /// </summary>
        //public ListCollectionView PsnOutsideOrgCV { get; set; }

        //string _searchOrgText;

        ///// <summary>
        ///// متن جستجو در بین سازمان ها
        ///// </summary>
        //public string SearchOrgText
        //{
        //    get { return _searchOrgText; }
        //    set
        //    {
        //        _searchOrgText = value;
        //        ExecuteSearchOrgs();
        //    }
        //}

        ///// <summary>
        ///// متن جستجو در بین اشخاص
        ///// </summary>
        //public string SearchPsnText
        //{
        //    get { return _searchPsnText; }
        //    set
        //    {
        //        _searchPsnText = value;
        //        ExecuteSearchPsns();
        //    }
        //}
        ///// <summary>
        ///// جستجو در بین سازمانها
        ///// </summary>
        //public ICommand SearchOrgs { get; set; }

        /// <summary>
        /// جستجو در بین اشخاص
        /// </summary>
        //public ICommand SearchPsns { get; set; }

        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// اضافه کردن یک نقش جدید
        /// </summary>
        public ICommand AddRol { get; set; }

        public ICommand DeleteRoleCommand { get; set; }

        public ICommand DeletePlyrRolCommand { get; set; }

        public string SearchRolText
        {
            get { return _searchRolText; }
            set
            {
                if (_searchRolText != value)
                {
                    _searchRolText = value;
                    RolOutsideCV.Refresh();
                }
            }
        }

        //public DtlRolIsdOrgViewModel DtlRolVM { get; set; }


        public DtlRolIsdOrgViewModel DtlRolVM
        {
            get { return _dtlRolVM; }
            set
            {
                _dtlRolVM = value;
                RaisePropertyChanged("DtlRolVM");
            }
        }

        public TblRol SelectedRol
        {
            get { return _selectedRol; }
            set
            {
                _selectedRol = value;
                DtlRolVM.SelectedRol = _selectedRol;
                RaisePropertyChanged("SelectedRol");
            }
        }


        //public TblOrg SelectedOrg
        //{
        //    get { return _selectedOrg; }
        //    set
        //    {
        //        _selectedOrg = value;
        //        var rols = TblRolOutsideOrg.ToList();
        //        rols.ForEach(r => r.IsSelected = false);
        //        try
        //        {
        //            PublicMethods.DetectRolPlayedInNod_23213(value.Nod).ForEach(r => r.IsSelected = true);

        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}

        //public TblPsn SelectedPsn
        //{
        //    get { return _selectedPsn; }
        //    set
        //    {
        //        _selectedPsn = value;
        //        var rols = TblRolOutsideOrg.ToList();
        //        rols.ForEach(r => r.IsSelected = false);
        //        try
        //        {
        //            PublicMethods.DetectRolPlayedInNod_23213(value.Nod).ForEach(r => r.IsSelected = true);
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool canSlcPsnCommand(TblRol arg)
        {
            return true;
        }

        private void slcPsnExecute(TblRol obj)
        {
            if (obj == null)
            {
                return;
            }

            if (!Acs_EditRol)
            {
                Util.ShowNotification(55);
                return;
            }
            if (_slcPsnOrgOsdVM == null)
            {
                _slcPsnOrgOsdVM = new SlcPsnAndOrgOsdViewModel(this.context);
            }
            _slcPsnOrgOsdVM.SelectItems(obj.TblPlyrRols.Select(n => n.TblNod.EtyNod).ToList());

            Util.ShowPopup(_slcPsnOrgOsdVM);

            if (_slcPsnOrgOsdVM.Result == PopupResult.OK)
            {
                var plyers = obj.TblPlyrRols.ToList();

                foreach (var item in plyers)
                {
                    this.context.TblPlyrRols.DeleteObject(item);
                }

                foreach (var item in _slcPsnOrgOsdVM.SelectedItems)
                {
                    if (item.Nod != null)
                    {
                        if (!obj.TblPlyrRols.Any(p => p.FldCodNod == item.Nod.FldCodNod))
                        {
                            obj.TblPlyrRols.Add(new TblPlyrRol() { FldCodNod = item.Nod.FldCodNod });
                        }
                    }
                }
            }
        }


        //private bool filterOrgs(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return true;
        //    }

        //    if (!string.IsNullOrEmpty(SearchOrgText))
        //    {
        //        TblOrg org = obj as TblOrg;
        //        return org.FldNamOrg.Trim().ToLower().Contains(SearchOrgText.Trim().ToLower());
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        ///// <summary>
        ///// جستجو
        ///// </summary>
        //private void ExecuteSearchOrgs()
        //{
        //    this.OrgOutsideOrgCV.Refresh();
        //}


        //private bool searchPsns(object obj)
        //{
        //    if (obj == null)
        //    {
        //        return true;
        //    }
        //    if (!string.IsNullOrEmpty(SearchPsnText))
        //    {
        //        TblPsn psn = obj as TblPsn;
        //        return psn.FldNam1stPsn.Trim().ToLower().Contains(SearchPsnText.Trim().ToLower()) ||
        //            psn.FldNam2ndPsn.Trim().ToLower().Contains(SearchPsnText.Trim().ToLower());// ||
        //        //psn.FldTelPsn.Trim().ToLower().Contains(SearchPsnText.Trim().ToLower());
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// جستجو
        ///// </summary>
        //private void ExecuteSearchPsns()
        //{

        //    this.PsnOutsideOrgCV.Refresh();

        //}

        /// <summary>
        /// اضافه کردن یک نقش جدید
        /// </summary>
        private void ExecuteAddRol()
        {
            Model.TblRol tblRol = new Model.TblRol() { FldCodOrg = Base.UserManager.CurrentUser.FldCodOrg, FldTtlRol = "نقش برون سازمانی جدید", FldIsdOrg = false };
            this.context.TblRols.AddObject(tblRol);
            PublicMethods.SaveContext(this.context);
            this.TblRolOutsideOrg.Add(tblRol);
            Model.TblNod tbl = new TblNod() { FldCodEty = tblRol.FldCodRol, FldCodTypEty = 4 };
            this.context.TblNods.AddObject(tbl);
            TblAct actUspf = PublicMethods.CreateUnspsifiedAct();
            tbl.TblActs.Add(actUspf);
            //SelectedRol = tblRol;
            PublicMethods.SaveContext(this.context);
        }

        /// <summary>
        /// حذف یک نقش
        /// </summary>
        private void ExecuteDeleteRoleCommand(TblRol rol)
        {
            if (rol != null)
            {
                if (Util.ShowMessageBox(2, rol.Name) == System.Windows.MessageBoxResult.Yes)
                {
                    if (PublicMethods.DeleteRol_2267(this.context, rol))
                    {
                        TblRolOutsideOrg.Remove(rol);
                    }
                    else
                    {
                        Util.ShowMessageBox(16, rol.Name);
                    }
                }
            }
        }

        /// <summary>
        /// حذف اعضای نقش ها
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDeletePlyrRolCommand(TblPlyrRol obj)
        {
            if (obj == null)
            {
                return;
            }

            SelectedRol = obj.TblRol;

            if (!SelectedRol.Acs_EditRol)
            {
                Util.ShowNotification(55);
                return;
            }
            this.context.TblPlyrRols.DeleteObject(obj);
        }

        /// <summary>
        /// save
        /// </summary>
        private void ExecuteSaveChangesCommand()
        {
            //context must save two times

            PublicMethods.SaveContext(this.context);
            DefineNodForRols();
            PublicMethods.SaveContext(this.context);
            DefineUnknownActForRols();
            PublicMethods.SaveContext(this.context);
        }

        /// <summary>
        /// برای نقش هایی که دارای نود نیستند، نود تعریف می کند
        /// </summary>
        private void DefineNodForRols()
        {
            List<TblRol> tblRol = this.context.TblRols.Where(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg && !E.FldIsdOrg).ToList();
            foreach (TblRol item in tblRol)
            {
                if (this.context.TblNods.Where(E => E.FldCodTypEty == 4 && E.FldCodEty == item.FldCodRol).Count() == 0)
                {
                    Model.TblNod tbl = new TblNod() { FldCodEty = item.FldCodRol, FldCodTypEty = 4 };
                    this.context.TblNods.AddObject(tbl);
                }
            }
        }

        /// <summary>
        /// تعریف فعالیت نامشخص برای نقش ها
        /// </summary>
        private void DefineUnknownActForRols()
        {
            List<Model.TblNod> tblNod = this.context.TblNods.Where(E => E.TblActs.Where(M => M.FldActUspf == true).Count() == 0).ToList();
            foreach (Model.TblNod item in tblNod)
            {
                Model.TblAct tbl = new TblAct() { FldNamAct = "فعالیت نامشخص", FldTypAct = (int)Model.Enum.ActivityTypes.Manual, FldActUspf = true };
                item.TblActs.Add(tbl);
            }
        }

        ///// <summary>
        ///// حذف نقش برون سازمانی
        ///// </summary>
        //private void ExecuteDeleteRoleCommand(Model.TblRol obj)
        //{
        //    this.bpmnEty.TblRols.DeleteObject(obj);
        //    this.TblRolOutsideOrg.Remove(obj);
        //}

        private bool filterRols(object obj)
        {
            if (string.IsNullOrWhiteSpace(this.SearchRolText))
            {
                return true;
            }

            if (obj == null)
            {
                return true;
            }

            var rol = obj as TblRol;

            if (!string.IsNullOrWhiteSpace(rol.Name))
            {
                return rol.Name.Trim().ToLower().Contains(this.SearchRolText.Trim().ToLower());
            }

            return true;
        }


        #endregion

        #region ' events '

        #endregion

        public void SaveContext()
        {
            //int i = this.context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Added).Count();

            //int j = this.context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Deleted).Count();

            //int k = this.context.ObjectStateManager.GetObjectStateEntries(System.Data.EntityState.Modified).Count();

            PublicMethods.SaveContext(this.context);
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
