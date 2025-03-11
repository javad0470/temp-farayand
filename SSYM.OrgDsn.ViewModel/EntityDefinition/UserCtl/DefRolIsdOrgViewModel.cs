using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
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
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model.Enum;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefRolIsdOrgViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        Model.BPMNDBEntities context;
        private ObservableCollection<Model.TblRol> tblRolInsideOrg;
        private ObservableCollection<Model.TblOrg> tblOrg;
        private DtlRolIsdOrgViewModel _dtlRolVM;
        private TblRol _selectedRol;
        private string _searchRolText;
        SlcPosPstOrgViewModel _slcPosPstOrgVM;
        #endregion

        #region ' Initialaizer '

        public DefRolIsdOrgViewModel()
        {
            context = new BPMNDBEntities();

            // تمام نقش های سازمان جاری
            List<TblRol> rols = context.TblRols.Where(E => E.FldIsdOrg && E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg).ToList();


            //اگر شخص جاری نماینده سازمان جاری نیست
            if (!PublicMethods.GetAgntOfPsnIsdOrg_22230(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(a => a.TblNod.EtyNod.TypEty == AllTypEty.Org))
            {
                //فقط نقش هایی که شخص به صورت صریح نماینده آن هاست را نشان بده
                rols.RemoveAll(canViewRol);
            }


            //this.TblOrg = new ObservableCollection<Model.TblOrg>(bpmnEty.TblOrgs.Where(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg));
            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            AddRol = new DelegateCommand(ExecuteAddRol);
            //this.PosPstChartVM = new PosPstChartViewModel(this.context);
            //this.PosPstChartVM.SelectedOrg = context.TblOrgs.Single(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);
            //this.PosPstChartVM.PropertyChanged += PosPstChartVM_PropertyChanged;
            //this.PosPstChartVM.CanUsrEditPosPst = false;
            this.OrgChartVM = new OrgChartViewModel(this.context);
            this.OrgChartVM.PropertyChanged += OrgChartVM_PropertyChanged;
            this.OrgChartVM.CanUsrEditChart = false;
            DeleteRoleCommand = new DelegateCommand<TblRol>(ExecuteDeleteRoleCommand);
            DeletePlyrRolCommand = new DelegateCommand<TblPlyrRol>(ExecuteDeletePlyrRolCommand);
            DtlRolVM = new DtlRolIsdOrgViewModel(context) { IsIsd = true };

            this.TblRolInsideOrg = new ObservableCollection<Model.TblRol>(rols);
            SlcPosOrgCommand = new DelegateCommand<TblRol>(SlcPosOrgCommandExcecute, canSlcPosOrgCommandExcecute);

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
        public bool Acs_EnterRolIsdOrg
        {
            get
            {
                //اگر شخص جاری نماینده سازمان جاری نیست
                if (!PublicMethods.GetAgntOfPsnIsdOrg_22230(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(a => a.TblNod.EtyNod.TypEty == AllTypEty.Org))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "RolIsdOrg", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    return this.Usr.AcsUsr["EnterRolIsdOrg"];
                }

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddRol
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "RolIsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["AddRolIsdOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelRol
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "RolIsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["DelRolIsdOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_EditRol
        //{
        //    get
        //    {
        //        if (PublicMethods.CurrentUser.AcsUsr.EditRolAllowedByOrg)
        //        {
        //            return true;
        //        }

        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(SelectedRol.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
        //        return PublicMethods.CurrentUser.AcsUsr["EditRolIsdOrg"] || SelectedRol.NewlyAdded;
        //    }
        //}

        #endregion

        /// <summary>
        /// نقش های درون سازمانی
        /// </summary>
        public ObservableCollection<Model.TblRol> TblRolInsideOrg
        {
            get { return tblRolInsideOrg; }
            set
            {
                tblRolInsideOrg = value;
                if (RolInsideCV == null)
                {
                    RolInsideCV = new ListCollectionView(tblRolInsideOrg);
                    RolInsideCV.Filter = filterRols;

                    RaisePropertyChanged("RolInsideCV");
                }

                SelectedRol = tblRolInsideOrg.FirstOrDefault();

            }
        }

        public ListCollectionView RolInsideCV { get; set; }

        /// <summary>
        /// saves changes
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// اضافه کردن یک نقش جدید
        /// </summary>
        public ICommand AddRol { get; set; }

        public ICommand DeleteRoleCommand { get; set; }

        public ICommand DeletePlyrRolCommand { get; set; }

        //public PosPstChartViewModel PosPstChartVM { get; set; }

        public OrgChartViewModel OrgChartVM { get; set; }

        public DtlRolIsdOrgViewModel DtlRolVM { get; set; }

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

        public string SearchRolText
        {
            get { return _searchRolText; }
            set
            {
                _searchRolText = value;
                RolInsideCV.Refresh();
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool canSlcPosOrgCommandExcecute(TblRol arg)
        {
            return true;
        }

        private void SlcPosOrgCommandExcecute(TblRol obj)
        {


            if (obj == null)
            {
                return;
            }

            if (!obj.Acs_EditRol)
            {
                Util.ShowNotification(55);
                return;
            }

            if (_slcPosPstOrgVM == null)
            {
                _slcPosPstOrgVM = new SlcPosPstOrgViewModel(this.context);
            }

            _slcPosPstOrgVM.SelectItems(obj.TblPlyrRols.Select(n => n.TblNod.EtyNod).ToList());

            Util.ShowPopup(_slcPosPstOrgVM);

            if (_slcPosPstOrgVM.Result == PopupResult.OK)
            {
                var plyers = obj.TblPlyrRols.ToList();

                foreach (var item in plyers)
                {
                    this.context.TblPlyrRols.DeleteObject(item);
                }

                foreach (var item in _slcPosPstOrgVM.SelectedItems)
                {
                    if (!obj.TblPlyrRols.Any(p => p.FldCodNod == item.Nod.FldCodNod))
                    {
                        if (item.TypEty == AllTypEty.Org && item.Org.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg)
                        {
                            //امکان افزودن سازمان جاری به یک نقش وجود ندارد.
                            Util.ShowMessageBox(70, null);
                            continue;
                        }
                        obj.TblPlyrRols.Add(new TblPlyrRol() { FldCodNod = item.Nod.FldCodNod });
                    }
                }
            }




            //if (eo.GetType() == typeof(Model.TblOrg))
            //{
            //    Model.TblOrg tblOrg = (Model.TblOrg)eo;
            //    if (tblOrg.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg)
            //    {
            //        //امکان افزودن سازمان جاری به یک نقش وجود ندارد.
            //        UIUtil.ShowMessageBox(70, null);
            //        return;
            //    }
            //    System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
            //    Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblOrg.FldCodOrg && E.FldCodTypEty == 1);
            //    Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
            //    if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
            //    {
            //        tblPlyrRol.Add(tblPlyrRol1);
            //    }

            //}
            //else if (eo.GetType() == typeof(Model.TblPsn))
            //{
            //    System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
            //    Model.TblPsn tblPsn = (Model.TblPsn)eo;
            //    Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblPsn.FldCodPsn && E.FldCodTypEty == 3);
            //    Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
            //    if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
            //    {
            //        tblPlyrRol.Add(tblPlyrRol1);
            //    }
            //}

            //else if (eo.GetType() == typeof(Model.TblPosPstOrg))
            //{
            //    System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
            //    Model.TblPosPstOrg tblPosPstOrg = (Model.TblPosPstOrg)eo;
            //    Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblPosPstOrg.FldCodPosPst && E.FldCodTypEty == 2);
            //    Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
            //    if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
            //    {
            //        tblPlyrRol.Add(tblPlyrRol1);
            //    }
            //}
        }


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


        /// <summary>
        /// اضافه کردن یک نقش جدید
        /// </summary>
        private void ExecuteAddRol()
        {
            Model.TblRol tblRol = new Model.TblRol() { FldCodOrg = Base.UserManager.CurrentUser.FldCodOrg, FldTtlRol = "نقش درون سازمانی جدید", FldIsdOrg = true };
            tblRol.NewlyAdded = true;
            this.context.TblRols.AddObject(tblRol);
            this.TblRolInsideOrg.Add(tblRol);
            PublicMethods.SaveContext(this.context);
            Model.TblNod tbl = new TblNod() { FldCodEty = tblRol.FldCodRol, FldCodTypEty = 4 };
            this.context.TblNods.AddObject(tbl);
            TblAct actUspf = PublicMethods.CreateUnspsifiedAct();
            tbl.TblActs.Add(actUspf);
            PublicMethods.SaveContext(this.context);
            SelectedRol = tblRol;
            this.DtlRolVM.AddCurrentPsnAsAgntOfRol(tblRol);
        }


        public ICommand SlcPosOrgCommand { get; set; }

        /// <summary>
        /// حذف یک نقش
        /// </summary>
        private void ExecuteDeleteRoleCommand(TblRol rol)
        {
            if (rol == null)
            {
                return;
            }

            if (!PublicMethods.IsRolDeletable_2242(context, rol))
            {
                Util.ShowMessageBox(29, "این نقش");
                return;
            }


            if (Util.ShowMessageBox(2, rol.Name) == System.Windows.MessageBoxResult.Yes)
            {
                if (PublicMethods.DeleteRol_2267(this.context, rol))
                {
                    TblRolInsideOrg.Remove(rol);
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

            //اگر شخص جاری نماینده سازمان جاری نیست
            if (!PublicMethods.GetAgntOfPsnIsdOrg_22230(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(a => a.TblNod.EtyNod.TypEty == AllTypEty.Org))
            {
                if (!canEditRol(obj.TblRol))
                {
                    Util.ShowMessageBox(55);
                    return;
                }
            }

            var res = Util.ShowMessageBox(2, "این شرکت کننده");

            if (res == MessageBoxResult.Yes)
            {
                this.context.TblPlyrRols.DeleteObject(obj);
            }
        }

        /// <summary>
        /// save
        /// </summary>
        private void ExecuteSaveChangesCommand()
        {
            //context must save two times
            PublicMethods.SaveContext(context);
            DefineNodForRols();
            PublicMethods.SaveContext(context);
            DefineUnknownActForRols();
            PublicMethods.SaveContext(context);
        }

        /// <summary>
        /// برای نقش هایی که دارای نود نیستند، نود تعریف می کند
        /// </summary>
        private void DefineNodForRols()
        {
            List<TblRol> tblRol = this.context.TblRols.Where(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg && E.FldIsdOrg).ToList();
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

        /// <summary>
        /// بررسی دسترسی به یک نقش 
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        private bool canViewRol(TblRol rol)
        {
            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(rol.Nod, "Enter", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
            return !PublicMethods.CurrentUser.AcsUsr["EnterRolIsdOrg"];
        }

        /// <summary>
        /// بررسی دسترسی به ویرایش به یک نقش 
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        private bool canEditRol(TblRol rol)
        {
            if (PublicMethods.CurrentUser.AcsUsr.EditRolAllowedByOrg)
            {
                return true;
            }

            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(rol.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
            return PublicMethods.CurrentUser.AcsUsr["EditRolIsdOrg"];
        }


        void OrgChartVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedOrg")
            {
                if (OrgChartVM.SelectedOrg != null)
                {
                    // شناسایی نقش هایی که این سازمان در آن قرار دارد یا در آن بازی می کند
                    List<TblRol> rols = PublicMethods.DetectRolPlayedInNod_23213(OrgChartVM.SelectedOrg.Nod);
                    TblRolInsideOrg.ToList().ForEach(r => r.IsSelected = rols.Any(r1 => r1.FldCodRol == r.FldCodRol));
                }
            }
        }

        //void PosPstChartVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "SelectedPosPst")
        //    {
        //        if (PosPstChartVM.SelectedPosPst != null)
        //        {
        //            // شناسایی نقش هایی که این سازمان در آن قرار دارد یا در آن بازی می کند
        //            List<TblRol> rols = PublicMethods.DetectRolPlayedInNod_23213(PosPstChartVM.SelectedPosPst.Nod);
        //            TblRolInsideOrg.ToList().ForEach(r => r.IsSelected = rols.Any(r1 => r1.FldCodRol == r.FldCodRol));
        //        }
        //    }
        //}

        private void onAgntRolAdded(TblRol rol)
        {
            if (AgntAdded != null)
            {
                AgntAdded(rol);
            }
        }


        #endregion

        #region ' events '

        internal event Action<TblRol> AgntAdded;

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);

            if (DtlRolVM.AgntChanged)
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
