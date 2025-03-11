using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.EntityDefinition.Popup;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{

    public enum FormMode
    {
        View,
        Edit,
        New
    }


    public class DefPsnInsideOrgViewModel : NotificationObject, IViewModel
    {
        #region ' Fields '

        FormMode mode;
        BPMNDBEntities context = new BPMNDBEntities();
        private Model.TblPsn selectedPsn;
        TblUsr user;
        string searchFilter;
        bool gridEnabled = true;
        ObservableCollection<Model.TblOrg> tblOrg;
        //TblPsn person;
        private Model.TblOrg _currentOrg;
        bool deleteUsrAndPsn = false;
        ObservableCollection<TblLvlAc> lvlAcs;
        bool _isSelectNodOpen;
        TblAgntNod _agntNodCnt;
        ObservableCollection<TblAgntNod> _agntNodOfPsnCntOfTypPosPst;
        ObservableCollection<TblAgntNod> _agntNodOfPsnCntOfTypRol;
        TblOrg selectedOrg;

        TblOrg currentOrg;

        TblUsr _selectedUsr;
        SlcNodViewModel _slcNodVM;
        ObservableCollection<TblItmFixSfw> _dmnAcs;
        bool _agntChanged = false;

        bool _rolSelected;
        bool _posSelected;
        bool _orgSelected;



        #endregion

        #region ' Initialaizer '

        public DefPsnInsideOrgViewModel()
        {
            DependencyObject dep = new DependencyObject();

            if (!DesignerProperties.GetIsInDesignMode(dep))
            {
                refreshList();

                currentOrg = context.TblOrgs.Single(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);

                this.AddNewPsnCommand = new DelegateCommand(ExecuteAddNewPsnCommand, CanAddNewPsn);

                this.DeletePsnCommand = new DelegateCommand<Model.TblPsn>(ExecuteDeletePsnCommand, canDeletePsn);

                this.SelectPsnCommand = new DelegateCommand<Model.TblPsn>(ExecuteSelectPersonCommand);

                this.EditPersonCommand = new DelegateCommand(ExecuteEditPerson, CanExecuteEditPerson);

                this.OKAddEditPsnCommand = new DelegateCommand(ExecuteOKAddEditPsn, CanOKAddEditPsn);

                this.RejectChangesCommand = new DelegateCommand(ExecuteRejectChanges, CanRejectChanges);

                this.SelectedPerson = new PsnInfoViewModel();

                OkCommand = new DelegateCommand(ExecuteOkCommand);

                CancelCommand = new DelegateCommand(ExecuteCancelCommand);

                ShowAsnItmsCommand = new DelegateCommand<TblOrg>(ShowAsnItmsExecute, canShowAsnItms);

                this.CurrentOrg = this.context.TblOrgs.SingleOrDefault(E => E.FldCodOrg == user.FldCodOrg);

                ShowAsnItmsCommand.RaiseCanExecuteChanged();

                EditPersonCommand.RaiseCanExecuteChanged();

                mode = FormMode.View;

                this.LvlAcs = new ObservableCollection<TblLvlAc>(this.context.TblLvlAcs);

                DelAgntCommand = new DelegateCommand<TblAgntNod>(ExecuteDelAgntCommand);

                this.SlcNodVM = new Popup.SlcNodViewModel(Popup.FormMode.SlcOrgMode);

                OpenSlcNodCommand = new DelegateCommand<string>(ExecuteOpenSlcNodCommand);

                DelAgntAndUsrCommand = new DelegateCommand(ExecuteDelAgntAndUsrCommand);

            }

            OrgSelected = true;
        }

        private void refreshList()
        {
            user = context.TblUsrs.Single(m => m.FldCodUsr == SSYM.OrgDsn.ViewModel.Base.UserManager.CurrentUser.FldCodUsr);

            TblOrg org = user.TblOrg;

            List<TblPsn> psns = PublicMethods.GetPsnInOrg(this.context);

            TblPsns = new ObservableCollection<TblPsn>(psns);

            TblPsnsCV = new ListCollectionView(TblPsns);

            TblPsnsCV.Filter = new Predicate<object>(TblPsnsCV_Filter);

            RaisePropertyChanged("TblPsns");
        }

        #endregion

        #region ' Properties / Commands '

        #region Access

        /// <summary>
        /// 
        /// </summary>

        #region ' Psn '
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddPsnIsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["AddPsnIsdOrg"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditPsnIsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["EditPsnIsdOrg"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelPsnIsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["DelPsnIsdOrg"];

            }
        }

        #endregion

        public bool Acs_ViewAgnt
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "Agnt", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return PublicMethods.CurrentUser.AcsUsr["ViewAgnt"];
            }
        }


        #region ' AgntPosPstOrg '
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddAgntPosPstOrg
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntPosPstOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["AddAgntPosPstOrg"] && !isAdmin();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAgntPosPstOrg
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntPosPstOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["DelAgntPosPstOrg"] && !isAdmin();

            }
        }

        public bool Acs_EditAgntPosPst
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntPosPstOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                if (isCurrOrgRoot())
                {
                    return PublicMethods.CurrentUser.AcsUsr["EditAgntPosPstOrg"] && !isAdmin();
                }
                else
                {
                    return PublicMethods.CurrentUser.AcsUsr["EditAgntPosPstOrg"];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewAgntPosPstOrg
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntPosPstOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["ViewAgntPosPstOrg"];
            }
        }

        #endregion

        #region ' AgntOrg '
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddAgntOrg
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["AddAgntOrg"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAgntOrg
        {
            get
            {
                if (isCurrOrgRoot(SelectedOrg))
                {
                    return PublicMethods.CurrentUser.AcsUsr["DelAgntOrg"] && !isAdmin();
                }
                else
                {
                    return PublicMethods.CurrentUser.AcsUsr["DelAgntOrg"];
                }


            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewAgntOrg
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["ViewAgntOrg"];
            }
        }

        public bool Acs_EditAgntOrg
        {
            get
            {

                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntOrg", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);


                if (SelectedOrg == null)
                {
                    return false;
                }

                if (isCurrOrgRoot(SelectedOrg))
                {
                    return PublicMethods.CurrentUser.AcsUsr["EditAgntOrg"] && !isAdmin();
                }
                else
                {
                    return PublicMethods.CurrentUser.AcsUsr["EditAgntOrg"];
                }
            }
        }

        #endregion

        #region ' AgntRol '
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewAgntRol
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntRol", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["ViewAgntRol"];
            }
        }


        public bool Acs_EditAgntRol
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntRol", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["EditAgntRol"];

            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddAgntRol
        {
            get
            {
                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntRol", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);

                return PublicMethods.CurrentUser.AcsUsr["AddAgntRol"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAgntRol
        {
            get
            {

                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntRol", Model.Enum.AllTypEty.Agnt, CurrentOrg.Nod);
                return PublicMethods.CurrentUser.AcsUsr["DelAgntRol"];

            }
        }

        #endregion


        #endregion

        public ObservableCollection<TblItmFixSfw> DmnAcs
        {
            get
            {
                if (_dmnAcs == null)
                {
                    return _dmnAcs = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 36));
                }

                return _dmnAcs;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblLvlAc> LvlAcs
        {
            get { return lvlAcs; }
            set
            {
                lvlAcs = value;

                RaisePropertyChanged("LvlAcs");
            }
        }

        public SlcNodViewModel SlcNodVM
        {
            get { return _slcNodVM; }
            set
            {
                _slcNodVM = value;

                RaisePropertyChanged("SlcNodVM");
            }
        }

        public bool IsSelectNodOpen
        {
            get { return _isSelectNodOpen; }
            set
            {
                _isSelectNodOpen = value;

                RaisePropertyChanged("IsSelectNodOpen");
            }
        }

        /// <summary>
        /// سازمان هایی شخص انتخاب شده نماینده آنهاست
        /// </summary>
        public ObservableCollection<TblOrg> AgntOrgs
        {
            get
            {
                if (SelectedPsn == null)
                {
                    return null;
                }

                if (tblOrg == null)
                {
                    List<TblOrg> lst = new List<TblOrg>();

                    //سازمان هایی که شخص جاری نماینده آنهاست و آن سازمان بالاسری سازمان جاری نیست
                    foreach (var agnt in SelectedPsn.TblAgntNods)
                    {
                        if (agnt.TblNod.FldCodTypEty == (int)FldTypEty.Org)
                        {
                            TblOrg org = agnt.TblNod.EtyNod as TblOrg;

                            if (PublicMethods.CurrentUser.TblOrg.FldCodOrg == org.FldCodOrg
                                || Model.TblOrg.IsOrgAnsestorOfThisOrg(PublicMethods.CurrentUser.TblOrg, org))
                            {
                                org.Agnt = SelectedPsn.TblAgntNods.Single(m => m.FldCodNod == org.Nod.FldCodNod);
                                org.Agnt.PropertyChanged += Agnt_PropertyChanged;
                                lst.Add(org);
                            }
                        }
                    }

                    tblOrg = new ObservableCollection<TblOrg>(lst);
                    tblOrg.CollectionChanged += this._agntNodOfPsnCntOfTypPosPst_CollectionChanged;
                }

                return tblOrg;
            }
        }

        public ObservableCollection<TblAgntNod> AgntNodOfPsnCntOfTypPosPst
        {
            get
            {
                if (SelectedPsn == null)
                {
                    return null;
                }

                TblPsn psn = SelectedPsn;

                if (_agntNodOfPsnCntOfTypPosPst == null)
                {
                    _agntNodOfPsnCntOfTypPosPst = new ObservableCollection<TblAgntNod>(psn.TblAgntNods.Where(m => m.TblNod.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg && m.TblNod != null && m.TblNod.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst));
                    _agntNodOfPsnCntOfTypPosPst.ToList().ForEach(a =>
                        {
                            a.PropertyChanged -= Agnt_PropertyChanged;
                            a.PropertyChanged += Agnt_PropertyChanged;
                        });
                    _agntNodOfPsnCntOfTypPosPst.CollectionChanged += _agntNodOfPsnCntOfTypPosPst_CollectionChanged;
                }

                return _agntNodOfPsnCntOfTypPosPst;
            }

            set
            {
                _agntNodOfPsnCntOfTypPosPst = value;

                RaisePropertyChanged("AgntNodOfPsnCntOfTypPosPst");
            }
        }


        public ObservableCollection<TblAgntNod> AgntNodOfPsnCntOfTypRol
        {

            get
            {
                if (SelectedPsn == null)
                {
                    return null;
                }

                TblPsn psn = SelectedPsn;

                if (_agntNodOfPsnCntOfTypRol == null)
                {
                    _agntNodOfPsnCntOfTypRol = new ObservableCollection<TblAgntNod>(psn.TblAgntNods.Where(m => m.TblNod.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg && m.TblNod != null && m.TblNod.FldCodTypEty == (int)Model.Enum.FldTypEty.Rol));
                    _agntNodOfPsnCntOfTypRol.ToList().ForEach(a =>
                    {
                        a.PropertyChanged -= Agnt_PropertyChanged;
                        a.PropertyChanged += Agnt_PropertyChanged;
                    });
                    _agntNodOfPsnCntOfTypRol.CollectionChanged += _agntNodOfPsnCntOfTypPosPst_CollectionChanged;
                }

                return _agntNodOfPsnCntOfTypRol;
            }
            set
            {
                _agntNodOfPsnCntOfTypRol = value;

                RaisePropertyChanged("AgntNodOfPsnCntOfTypRol");
            }
        }

        public Type DmnAcsType
        {
            get
            {
                return typeof(Model.Enum.DmnAcs);
            }
        }

        public bool GridEnabled
        {
            get { return gridEnabled; }
            set
            {
                gridEnabled = value;
                RaisePropertyChanged("GridEnabled");
            }
        }

        public FormMode Mode
        {
            get
            {
                return mode;
            }
            set
            {

                if (value != mode)
                {
                    mode = value;
                    this.SelectedPerson.IsEnabled = mode == FormMode.New || mode == FormMode.Edit;
                    GridEnabled = !this.SelectedPerson.IsEnabled;
                    AddNewPsnCommand.RaiseCanExecuteChanged();
                    EditPersonCommand.RaiseCanExecuteChanged();
                    OKAddEditPsnCommand.RaiseCanExecuteChanged();
                    RejectChangesCommand.RaiseCanExecuteChanged();
                }
                RaisePropertyChanged("IsOrgSlcEnable");
            }
        }

        public string SearchFilter
        {
            get
            {
                return searchFilter;
            }

            set
            {
                if (searchFilter != value)
                {
                    searchFilter = value;
                    TblPsnsCV.Refresh();
                }
            }
        }

        public PsnInfoViewModel SelectedPerson { get; set; }

        /// <summary>
        /// OrgChartViewModel
        /// </summary>
        /// <summary>
        /// اشخاص
        /// </summary>
        public ObservableCollection<Model.TblPsn> TblPsns
        {
            get;
            private set;
        }

        public ListCollectionView TblPsnsCV { get; set; }

        /// <summary>
        /// selected person
        /// </summary>
        public Model.TblPsn SelectedPsn
        {
            get { return selectedPsn; }
            set
            {
                selectedPsn = value;

                SelectedPerson.Person = selectedPsn;

                this.Mode = FormMode.View;

                if (selectedPsn == null)
                {
                    return;
                }

                //PsnOrgs = new ObservableCollection<TblOrg>(PublicMethods.GetOrgForPsn(value));

                value.PropertyChanged += value_PropertyChanged;

                tblOrg = null;
                _agntNodOfPsnCntOfTypPosPst = null;
                _agntNodOfPsnCntOfTypRol = null;

                RaisePropertyChanged("SelectedPsn");//, /*"PsnOrgs",*/ "AgntNodOfPsnCntOfTypPosPst", "AgntNodOfPsnCntOfTypRol", "AgntOrgs");

                SelectedOrg = AgntOrgs.FirstOrDefault();

                //RaisePropertyChanged("SelectedOrg", "Acs_DelAgntOrg", "Acs_EditAgntOrg");

                //, "Acs_EditAgntOrg", "Acs_ViewAgntOrg", "Acs_DelAgntOrg", "Acs_AddAgntOrg",
                //    "Acs_AddAgntPosPstOrg", "Acs_DelAgntPosPstOrg", "Acs_EditAgntPosPstOrg");

                EditPersonCommand.RaiseCanExecuteChanged();

                OrgSelected = true;

                PosSelected = false;

                RolSelected = false;
            }
        }

        public DelegateCommand RejectChangesCommand { get; set; }

        public DelegateCommand AddNewPsnCommand { get; set; }

        public DelegateCommand EditPersonCommand { get; set; }

        public DelegateCommand OKAddEditPsnCommand { get; set; }

        public DelegateCommand<TblPsn> DeletePsnCommand { get; set; }

        public DelegateCommand<TblPsn> SelectPsnCommand { get; set; }

        public DelegateCommand<TblOrg> ShowAsnItmsCommand { get; set; }

        public DelegateCommand<string> OpenSlcNodCommand { get; set; }

        public DelegateCommand<TblAgntNod> DelAgntCommand { get; set; }

        public DelegateCommand DelAgntAndUsrCommand { get; set; }

        public bool IsOrgSlcEnable
        {
            get
            {
                if (Mode == FormMode.Edit)
                    return true;

                return false;

            }
        }

        /// <summary>
        /// سازمان جاری
        /// </summary>
        public Model.TblOrg CurrentOrg
        {
            get { return _currentOrg; }
            set
            {

                _currentOrg = value;
                RaisePropertyChanged("CurrentOrg");
            }
        }

        /// <summary>
        /// سازمان های مربوط به کاربر جاری
        /// </summary>
        //public ObservableCollection<Model.TblOrg> TblOrg
        //{
        //    get
        //    {
        //        if (tblOrg == null)
        //        {
        //            return tblOrg = new ObservableCollection<TblOrg>(this.context.TblOrgs.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));
        //        }
        //        return tblOrg;
        //    }
        //}

        //public ObservableCollection<TblOrg> PsnOrgs { get; set; }

        /// <summary>
        /// سازمان انتخاب شده به عنوان نماینده
        /// </summary>
        public TblOrg SelectedOrg
        {
            get
            {
                return selectedOrg;
            }
            set
            {
                selectedOrg = value;
                RaisePropertyChanged("SelectedOrg");//, "Acs_EditAgntOrg", "Acs_ViewAgntOrg", "Acs_DelAgntOrg", "Acs_AddAgntOrg");
            }
        }
        private TblAgntNod _selectedPosPstOrg;
        public TblAgntNod SelectedPosPstOrg
        {
            get
            {
                return _selectedPosPstOrg;
            }
            set
            {
                _selectedPosPstOrg = value;
                RaisePropertyChanged("SelectedPosPstOrg");
            }
        }

        private TblAgntNod _selectedRol;
        public TblAgntNod SelectedRol
        {
            get
            {
                return _selectedRol;
            }
            set
            {
                _selectedRol = value;
                RaisePropertyChanged("SelectedRol");
            }
        }

        /// <summary>
        /// تایید بستن پنل انتخاب سازمان
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// لغو و بستن
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// is select organization panel open
        /// </summary>
        public bool IsSlcOrgOpen { get; set; }

        public bool ShowItmAsnToPsnVM { get; set; }

        public ItmAsnToPsnViewModel ItmAsnToPsnVM { get; set; }


        public bool RolSelected
        {
            get { return _rolSelected; }
            set
            {
                _rolSelected = value;
                if (_rolSelected)
                {
                    RaisePropertyChanged("AgntNodOfPsnCntOfTypRol");
                }
                RaisePropertyChanged("RolSelected");
            }
        }

        public bool OrgSelected
        {
            get { return _orgSelected; }
            set
            {
                _orgSelected = value;

                if (_orgSelected)
                {
                    RaisePropertyChanged("AgntOrgs");
                }

                RaisePropertyChanged("OrgSelected");
            }
        }

        public bool PosSelected
        {
            get
            {
                return _posSelected;

            }
            set
            {
                _posSelected = value;
                if (_posSelected)
                {
                    RaisePropertyChanged("AgntNodOfPsnCntOfTypPosPst");
                }

                RaisePropertyChanged("PosSelected");
            }
        }
        #endregion

        #region ' Public Methods '

        public void ClosePopup()
        {
            IsSlcOrgOpen = false;
            RaisePropertyChanged("IsSlcOrgOpen");
        }

        #endregion

        #region ' Private Methods '

        void _agntNodOfPsnCntOfTypPosPst_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _agntChanged = true;
        }

        void Agnt_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _agntChanged = true;
        }



        private bool CanAddNewPsn()
        {
            return Mode == FormMode.View && Acs_AddPsnIsdOrg;
        }

        /// <summary>
        /// Adds new user
        /// </summary>
        private void ExecuteAddNewPsnCommand()
        {
            SelectedPsn = new TblPsn();

            SelectedPsn.PropertyChanged -= psn_PropertyChanged;

            SelectedPsn.PropertyChanged += psn_PropertyChanged;

            this.SelectedPerson.Person = SelectedPsn;

            Mode = FormMode.New;
        }

        void psn_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OKAddEditPsnCommand.RaiseCanExecuteChanged();
        }



        /// <summary>
        /// select a person
        /// </summary>
        private void ExecuteSelectPersonCommand(Model.TblPsn obj)
        {
            this.SelectedPsn = obj;
        }

        private bool CanExecuteEditPerson()
        {
            return Mode == FormMode.View && SelectedPsn != null && Acs_EditPsnIsdOrg;
        }

        private void ExecuteEditPerson()
        {
            SelectedPsn.PropertyChanged -= psn_PropertyChanged;
            SelectedPsn.PropertyChanged += psn_PropertyChanged;
            Mode = FormMode.Edit;
        }

        private bool CanOKAddEditPsn()
        {
            //validation check goes here
            if (SelectedPsn != null)
            {
                return (Mode == FormMode.Edit || Mode == FormMode.New) && !SelectedPsn.HasErrors && ValidateNamePsn(SelectedPsn.FldNam1stPsn) && ValidateNamePsn(SelectedPsn.FldNam2ndPsn);
            }
            else
            {
                return (Mode == FormMode.Edit || Mode == FormMode.New);
            }
        }

        private bool CanRejectChanges()
        {
            return Mode == FormMode.Edit || Mode == FormMode.New;
        }

        private void ExecuteRejectChanges()
        {
            if (SelectedPsn == null)
            {
                Mode = FormMode.View;
                return;
            }
            switch (Mode)
            {
                case FormMode.View:
                    break;

                case FormMode.Edit:

                    PublicMethods.ReloadEntityWotSave(context, SelectedPsn);

                    RejectChanges();

                    break;

                case FormMode.New:

                    TblPsns.Remove(SelectedPsn);

                    if (TblPsns.Count() > 0)

                        SelectedPsn = TblPsns.FirstOrDefault();

                    else

                        SelectedPsn = null;

                    RejectChanges();

                    RaisePropertyChanged("SelectedPerson");

                    break;

                default:
                    break;
            }

            Mode = FormMode.View;
        }

        private void FillPsnList(List<TblPsn> obj)
        {
            foreach (var item in obj)
            {
                TblPsns.Add(item);
            }
            if (TblPsns != null && TblPsns.Count() > 0)
            {
                this.SelectedPsn = TblPsns[0];
            }
        }

        private bool TblPsnsCV_Filter(object obj)
        {
            if (!string.IsNullOrEmpty(SearchFilter))
            {
                return (obj as TblPsn).FldNam2ndPsn.Contains(SearchFilter) || (obj as TblPsn).FldNam1stPsn.Contains(SearchFilter);
            }
            return true;
        }


        void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OKAddEditPsnCommand.RaiseCanExecuteChanged();
        }

        private void ExecuteOkCommand()
        {
            //if (this.SelectedPsn != null)
            //{
            //    TblUsr user = this.SelectedPsn.TblUsrs.SingleOrDefault(m => m.FldCodOrg == selectedItem.FldCodOrg);
            //    if (user == null)
            //    {
            //        user = new TblUsr()
            //        {
            //            FldCodOrg = selectedItem.FldCodOrg,
            //            FldNamUsr = this.SelectedPsn.FldNam2ndPsn + "#" + SelectedPsn.FldCodPsn.ToString() + "#" +
            //            selectedItem.FldCodOrg.ToString(),
            //            FldNamUsrActv = false,
            //            FldPassUsr = "12346"
            //        };
            //        this.SelectedPsn.TblUsrs.Add(user);
            //        PsnOrgs.Add(selectedItem);
            //        RaisePropertyChanged("PsnOrgs");
            //    }
            //}
            this.IsSlcOrgOpen = false;
            RaisePropertyChanged("IsSlcOrgOpen");
            PublicMethods.SaveContext(context);
        }

        private void ExecuteCancelCommand()
        {
            this.IsSlcOrgOpen = false;
            RaisePropertyChanged("IsSlcOrgOpen", "PsnOrgs");
        }

        /// <summary>
        /// این متد زمانی فراخوانی میشود کا کاربر بر روی انصراف کلیک کند
        /// تمام تغییراتی که روی کاربر انتخاب شده جاری انجام شده حذف خواهد شد
        /// </summary>
        private void RejectChanges()
        {
            try
            {
                //PsnOrgs.Clear();
                PublicMethods.RollBackContext(this.context);
                //PsnOrgs = new ObservableCollection<TblOrg>(PublicMethods.GetOrgForPsn(selectedPsn));
                RaisePropertyChanged("PsnOrgs");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// این متد زمانی فراخوانی میشود که کاربر بخواهد موارد تخصیصی شخص انتخاب شده را 
        /// در سازمان انتخاب شده مدیریت نماید.
        /// </summary>
        /// <param name="obj"></param>
        private void ShowAsnItmsExecute(TblOrg obj)
        {
            if (obj == null)
            {
                return;
            }
            ItmAsnToPsnVM = new ItmAsnToPsnViewModel(selectedPsn, obj);
            ShowItmAsnToPsnVM = true;
            RaisePropertyChanged("ShowItmAsnToPsnVM");
            ShowItmAsnToPsnVM = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool canShowAsnItms(Model.TblOrg arg)
        {
            return SelectedOrg != null;
        }

        /// <summary>
        /// P1516
        /// ایجاد شخص جدید یا ذخیره تغییرات مربوط به شخص فعلی
        /// </summary>
        private void ExecuteOKAddEditPsn()
        {
            if (Mode == FormMode.New)
            {
                TblOrg org = SSYM.OrgDsn.ViewModel.Base.UserManager.CurrentUser.TblOrg;

                AddNewPsnIsdOrg_22150(org);

                PublicMethods.SaveContext(context);
            }
            else
            {
                PublicMethods.SaveContext(context);
            }

            Mode = FormMode.View;
        }


        /// <summary>
        /// افزودن شخص جدید در سازمان داده شده
        /// </summary>
        /// <param name="org"></param>
        private void AddNewPsnIsdOrg_22150(TblOrg org)
        {
            TblUsr user = PublicMethods.CreateNewUser_1550(this.context, this.SelectedPerson.Person.FldNam2ndPsn, org);

            user.TblPsn = this.SelectedPerson.Person;

            user.TblOrg = context.TblOrgs.Single(m => m.FldCodOrg == org.FldCodOrg);

            SelectedPerson.Person.FldIsdOrg = true;

            context.TblPsns.AddObject(this.SelectedPerson.Person);

            TblPsns.Add(this.SelectedPerson.Person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ValidateNamePsn(string input)
        {
            if (input == null || input.Length == 0 || input[0] == ' ')
            {
                return false;
            }
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] <= '9' && input[i] >= '0')
                    return false;
            }

            return true;
        }

        private void ExecuteOpenSlcNodCommand(string obj)
        {
            Popup.FormMode md = Popup.FormMode.SlcOrgMode;

            if (obj == "org")
            {
                TblOrg tblorg=new TblOrg();
                _slcNodVM.SelectItems(tblorg.TblOrg1.ToList());
                if (!Acs_AddAgntOrg)
                {
                    Util.ShowMessageBox(17, "افزودن نمایندگی سازمان برای این شخص");
                    return;
                }
            }


            //if (obj == "org")
            //{
            //    this.SlcNodVM = new Popup.SlcNodViewModel(Popup.FormMode.SlcOrgMode);

            //}

            if (obj == "posPst")
            {
                if (!Acs_AddAgntPosPstOrg)
                {
                    Util.ShowMessageBox(17, "افزودن نمایندگی جایگاه و سمت برای این شخص");
                    return;
                }

                md = Popup.FormMode.SlcPosPstMode;

                //if (this.Acs_AddAgntPosPstOrg && this.Acs_AddAgntRol)
                //{
                //    md = Popup.FormMode.SlcPosPst_RolMode;
                //}

                //else if (this.Acs_AddAgntPosPstOrg && !this.Acs_AddAgntRol)
                //{
                //    md = Popup.FormMode.SlcPosPstMode;
                //}

                //else if (!this.Acs_AddAgntPosPstOrg && this.Acs_AddAgntRol)
                //{
                //    md = Popup.FormMode.SlcRolMode;
                //}

            }

            else if (obj == "rol")
            {
                if (!Acs_AddAgntRol)
                {
                    Util.ShowMessageBox(17, "افزودن نمایندگی نقش برای این شخص");
                    return;
                }
                md = Popup.FormMode.SlcRolMode;
            }


            this.SlcNodVM = new Popup.SlcNodViewModel(md);

            this.SlcNodVM.SelectedOrgForPosPst = this.SlcNodVM.context.TblOrgs.SingleOrDefault(m => m.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg);

            this.SlcNodVM.SelectedOrgForRol = this.SlcNodVM.context.TblOrgs.SingleOrDefault(m => m.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg);

            Util.ShowPopup(this.SlcNodVM);

            if (this.SlcNodVM.Result == PopupResult.OK)
            {
                addAgnt();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDelAgntCommand(TblAgntNod obj)
        {
            if (obj == null)
            {
                return;
            }
            if (obj.TblNod.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst && !this.Acs_DelAgntPosPstOrg)
            {
                Util.ShowMessageBox(17, "حذف این نمایندگی شخص انتخاب شده");
                return;
            }
            if (obj.TblNod.FldCodTypEty == (int)Model.Enum.FldTypEty.Rol && !this.Acs_DelAgntRol)
            {
                Util.ShowMessageBox(17, "حذف این نمایندگی شخص انتخاب شده");
                return;
            }

            if (Util.ShowMessageBox(2, string.Format("نمایندگی {0}", obj.TblNod.Name)) == MessageBoxResult.Yes)
            {
                PublicMethods.DelAgnt(context, obj, SelectedPsn.FldCodPsn);

                if (AgntNodOfPsnCntOfTypPosPst.Contains(obj))
                {
                    AgntNodOfPsnCntOfTypPosPst.Remove(obj);
                }
                else
                {
                    AgntNodOfPsnCntOfTypRol.Remove(obj);
                }

                //RaisePropertyChanged("AgntNodOfPsnCntOfTypPosPst", "AgntNodOfPsnCntOfTypRol");
            }
        }


        private void ExecuteDelAgntAndUsrCommand()
        {
            if (SelectedOrg == null)
            {
                return;
            }
            if (!this.Acs_DelAgntOrg)
            {
                Util.ShowMessageBox(17, "حذف این نمایندگی شخص انتخاب شده");
                return;
            }
            //  در صورتی که سازمانی که قرار است حذف شود در سازمان های زیر مجموعه سازمان جاری نباشد
            if (PublicMethods.CurrentUser.TblOrg.GetSubOrgs().Where(m => m.FldCodOrg == this.SelectedOrg.FldCodOrg).Count() == 0)
            {
                Util.ShowMessageBox(22);
                return;
            }

            if (Util.ShowMessageBox(2, string.Format("نمایندگی {0}", SelectedOrg.Name)) == MessageBoxResult.Yes)
            {
                PublicMethods.DelAgnt(context, SelectedOrg.Agnt, SelectedPsn.FldCodPsn);

                AgntOrgs.Remove(SelectedOrg);
            }
        }

        private bool CanExecuteRemoveOrgFromPsn(Model.TblOrg arg)
        {
            return SelectedOrg != null;
        }


        ///// <summary>
        ///// افزودن نمایندگی  سازمان یا جایگاه و سمت
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void DefPsnInsideOrgViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    addAgnt();
        //}

        private void addAgnt()
        {
            if (this.LvlAcs.Count == 0)
            {
                Util.ShowMessageBox(20);
                return;
            }

            else
            {
                TblAgntNod addedAgnt = null;
                if (this.SlcNodVM.Mode == Popup.FormMode.SlcOrgMode)
                {
                    //اگر نمایندگی سازمان انتخاب شده پیش از این به شخص تخصیص داده شده باشد، امکان تخصیص مجدد آن وجود نداشته باشد
                    for (int i = 0; i < this.SlcNodVM.SelectedItems.Count; i++)
                    {
                        if((this.SlcNodVM.SelectedItems[i] as TblOrg)==null)
                        {
                            this.SlcNodVM.SelectedItems.RemoveAt(i);
                            i--;
                        }
                        
                    }

                        foreach (TblOrg item in this.SlcNodVM.SelectedItems)
                        {
                            if (this.SelectedPsn.TblAgntNods.Where(m => m.TblNod.EtyNod is TblOrg && (m.TblNod.EtyNod as TblOrg).FldCodOrg == item.FldCodOrg).Count() > 0)
                            {
                                Util.ShowMessageBox(19, "سازمان");
                                continue;
                            }

                            else
                            {
                                if (!Acs_AddAgntOrg)
                                {
                                    Util.ShowMessageBox(17, "این نماینده");
                                    continue;
                                }

                                #region ' 22157 افزودن نمایندگی سازمان برای یک شخص درون سازمانی '
                                // اگر نمایندگی سازمان جاری برای شخص جاری اضافه شود،  یوزر جدیدی افزوده نشود
                                //if (PublicMethods.CurrentUser.FldCodOrg != ((TblOrg)this.SlcNodVM.SelectedItm).FldCodOrg)
                                {

                                    addedAgnt = PublicMethods.AddAgntOfNodForPsn_22157(context, this.SelectedPsn, item.Nod);

                                    (addedAgnt.TblNod.EtyNod as TblOrg).Agnt = addedAgnt;

                                    //PublicMethods.AddAgntOfOrgForPsn_22157(context, this.SelectedPsn, (TblOrg)this.SlcNodVM.SelectedItm);
                                    //اگر شخص انتخاب شده در سازمان انتخاب شده دارای کاربری باشد، کاربری جدید افزوده نشود
                                    //if (!(this.SelectedPsn.TblUsrs.Any(m => m.TblOrg.FldCodOrg == ((TblOrg)this.SlcNodVM.SelectedItm).FldCodOrg)))
                                    //{
                                    //    TblUsr user = new TblUsr()
                                    //    {
                                    //        FldCodOrg = ((TblOrg)this.SlcNodVM.SelectedItm).FldCodOrg,
                                    //        FldNamUsr = this.SelectedPsn.FldNam2ndPsn + "#" + SelectedPsn.FldCodPsn.ToString() + "#" +
                                    //        ((TblOrg)this.SlcNodVM.SelectedItm).FldCodOrg,
                                    //        FldNamUsrActv = false,
                                    //        FldPassUsr = "123456"
                                    //    };

                                    //    this.SelectedPsn.TblUsrs.Add(user);
                                    //}
                                }


                                //TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod, FldCodNod = this.SlcNodVM.SelectedItm.Nod.FldCodNod };

                                //this.SelectedPsn.TblAgntNods.Add(agnt);


                                #endregion

                                AgntOrgs.Add(addedAgnt.TblNod.EtyNod as TblOrg);
                                this.SelectedOrg = (addedAgnt.TblNod.EtyNod as TblOrg);

                                //RaisePropertyChanged("AgntOrgs");
                            }
                        }
                    
                }

                else if (this.SlcNodVM.Mode == Popup.FormMode.SlcPosPstMode)
                {
                    for (int i = 0; i < this.SlcNodVM.SelectedItems.Count; i++)
                    {
                        if ((this.SlcNodVM.SelectedItems[i] as TblPosPstOrg) == null)
                        {
                            this.SlcNodVM.SelectedItems.RemoveAt(i);
                            i--;
                        }
                    }
                    foreach (TblPosPstOrg item in this.SlcNodVM.SelectedItems)
                    {
                        // اگر نماینده قبلا اضافه شده است دیگر اضافه نشود
                        if (this.SelectedPsn.TblAgntNods.Where(m => m.TblNod.FldCodNod == item.Nod.FldCodNod).Count() > 0)
                        {
                            Util.ShowMessageBox(19, "جایگاه/سمت");
                            continue;
                        }

                        else
                        {
                            if (!Acs_AddAgntPosPstOrg)
                            {
                                Util.ShowMessageBox(17, "این نماینده");
                                continue;

                            }
                            else
                            {
                                var agnt = PublicMethods.AddAgntOfNodForPsn_22157(context, this.SelectedPsn, item.Nod);

                                AgntNodOfPsnCntOfTypPosPst.Add(agnt);
                                //addUserInCurrentOrgIfNotExist();

                                //TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod, FldCodNod = this.SlcNodVM.SelectedItm.Nod.FldCodNod };

                                //this.SelectedPsn.TblAgntNods.Add(agnt);

                                //RaisePropertyChanged("AgntNodOfPsnCntOfTypPosPst", "AgntNodOfPsnCntOfTypRol");
                                this.SelectedPosPstOrg = (agnt);
                            }

                        }
                    }
                }

                else if (this.SlcNodVM.Mode == Popup.FormMode.SlcRolMode)
                {
                    for (int i = 0; i < this.SlcNodVM.SelectedItems.Count; i++)
                    {
                        if ((this.SlcNodVM.SelectedItems[i] as TblRol) == null)
                        {
                            this.SlcNodVM.SelectedItems.RemoveAt(i);
                            i--;

                        }
                    }
                    foreach (TblRol item in this.SlcNodVM.SelectedItems)
                    {
                        // اگر نماینده قبلا اضافه شده است دیگر اضافه نشود
                        if (this.SelectedPsn.TblAgntNods.Where(m => m.TblNod.FldCodNod == item.Nod.FldCodNod).Count() > 0)
                        {
                            Util.ShowMessageBox(19, "نقش");
                            continue;
                        }

                        else
                        {
                            if (!Acs_AddAgntRol)
                            {
                                Util.ShowMessageBox(17, "این نماینده");
                                continue;
                            }

                            var agnt = PublicMethods.AddAgntOfNodForPsn_22157(context, this.SelectedPsn, item.Nod);

                            AgntNodOfPsnCntOfTypRol.Add(agnt);
                            //addUserInCurrentOrgIfNotExist();

                            //TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod, FldCodNod = this.SlcNodVM.SelectedItm.Nod.FldCodNod };

                            //this.SelectedPsn.TblAgntNods.Add(agnt);

                            //RaisePropertyChanged("AgntNodOfPsnCntOfTypPosPst", "AgntNodOfPsnCntOfTypRol");
                            this.SelectedRol = (agnt);
                        }
                    }

                }
            }
        }

        private void addUserInCurrentOrgIfNotExist()
        {
            if (!this.SelectedPsn.TblUsrs.Any(u => u.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg))
            {
                TblUsr user = new TblUsr()
                {
                    FldCodOrg = PublicMethods.CurrentUser.FldCodOrg,
                    FldNamUsr = this.SelectedPsn.FldNam2ndPsn + "#" + SelectedPsn.FldCodPsn.ToString() + "#" +
                    PublicMethods.CurrentUser.FldCodOrg,
                    FldNamUsrActv = false,
                    FldPassUsr = TblUsr.CalculateMD5Hash("123456")
                };

                this.SelectedPsn.TblUsrs.Add(user);
            }
        }

        /// <summary>
        /// Deletes person confirm
        /// </summary>
        private void ExecuteDeletePsnCommand(Model.TblPsn obj)
        {
            //امکان حذف شخص جاری (کاربر جاری) در فرم اشخاص درون سازمانی نباید وجود داشته باشد.
            if (PublicMethods.CurrentUser.TblPsn.FldCodPsn == obj.FldCodPsn)
            {
                Util.ShowMessageBox(46);
                return;
            }

            if (Util.ShowMessageBox(2, "شخص درون سازمانی") == System.Windows.MessageBoxResult.Yes)
            {
                //برای یک شخص که دارای کاربری فعال است نباید امکان حذف وجود داشته باشد. در صورت حذف پیغام 58 نشان داده شود.
                //if (psn.TblUsrs.Any(m => m.FldNamUsrActv))
                //{
                //    Util.ShowMessageBox(33);
                //    return false;
                //}

                //در صورت حذف این شخص درون سازمانی، نام کاربری و رمز عبور ثبت شده برای آن نیز حذف می‏شود، آیا ادامه می‏دهید؟
                if (Util.ShowMessageBox(34) != MessageBoxResult.Yes)
                {
                    return;
                }

                try
                {
                    PublicMethods.DeletePsnIsd_22154(context, obj, true);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "33")
                    {
                        Util.ShowMessageBox(33);
                        return;
                    }
                }

                TblPsns.Remove(obj);
                TblPsnsCV.Refresh();
                RaisePropertyChanged("TblPsns", "TblPsnsCV");
            }
        }


        private bool canDeletePsn(TblPsn arg)
        {
            return true;
        }

        private bool isAdmin()
        {
            if (SelectedPsn != null)
            {
                return SelectedPsn.TblUsrs.Any(u => u.FldNamUsr.Trim() == "admin");
            }
            return false;
        }


        private bool isCurrOrgRoot(TblOrg currOrg = null)
        {
            if (currOrg == null)
            {
                return PublicMethods.CurrentUser.TblOrg.TblOrg2 == null && PublicMethods.CurrentUser.TblOrg.TblUsrs.Any();
            }
            else
            {
                return currOrg.TblOrg2 == null && PublicMethods.CurrentUser.TblOrg.TblUsrs.Any();
            }
        }

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);

            if (_agntChanged)
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


        public DelegateCommand AddNewAgntOfTypPosPstCommand { get; set; }
    }
}
