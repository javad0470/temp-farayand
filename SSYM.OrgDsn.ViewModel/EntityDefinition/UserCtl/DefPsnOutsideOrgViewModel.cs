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
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{


    public class DefPsnOutsideOrgViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        FormMode mode;
        BPMNDBEntities context;
        private Model.TblPsn selectedPsn;
        TblUsr user;
        string searchFilter;
        bool gridEnabled = true;
        ObservableCollection<Model.TblOrg> tblOrg;

        private ObservableCollection<TblSbjActPsn> _actSbjs;

        //TblPsn person;
        private Model.TblOrg selectedItem;


        #endregion

        #region ' Initialaizer '

        public DefPsnOutsideOrgViewModel()
        {
            context = new BPMNDBEntities();

            this._actSbjs = new ObservableCollection<TblSbjActPsn>(context.TblSbjActPsns);

            user = SSYM.OrgDsn.ViewModel.Base.UserManager.CurrentUser;
            TblOrg org = user.TblOrg;

            List<TblPsn> psns = PublicMethods.GetPsnOutsideOrg_22244(this.context, PublicMethods.CurrentUser.FldCodOrg);

            TblPsns = new ObservableCollection<TblPsn>(psns);

            TblPsnsCV = new ListCollectionView(TblPsns);
            TblPsnsCV.Filter = new Predicate<object>(TblPsnsCV_Filter);


            this.AddNewPsnCommand = new DelegateCommand(ExecuteAddNewPsnCommand, CanAddNewPsn);
            this.DeletePsnCommand = new DelegateCommand<Model.TblPsn>(ExecuteDeletePsnCommand);
            this.SelectPsnCommand = new DelegateCommand<Model.TblPsn>(ExecuteSelectPersonCommand);
            this.EditPersonCommand = new DelegateCommand(ExecuteEditPerson, CanExecuteEditPerson);
            this.OKAddEditPsnCommand = new DelegateCommand(ExecuteOKAddEditPsn, CanOKAddEditPsn);
            this.RejectChangesCommand = new DelegateCommand(ExecuteRejectChanges, CanRejectChanges);
            this.SelectedPerson = new PsnInfoViewModel();

            Mode = FormMode.View;
            if (TblPsns.Count > 0)
                SelectedPsn = TblPsns.FirstOrDefault();

            this.Other = this.Peymankar = this.Producing = this.Servicing = this.Commercial = false;
        }

        #endregion

        #region ' Properties / Commands '

        #region Access

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddPsnOsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["AddPsnOsdOrg"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditPsnOsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["EditPsnOsdOrg"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelPsnOsdOrg
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["DelPsnOsdOrg"];

            }
        }

        #endregion

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

            }
        }

        /// <summary>
        /// لیست موضوعات فعالیت اشخاص
        /// </summary>
        public ObservableCollection<TblSbjActPsn> ActSbjs
        {
            get { return _actSbjs; }
            set { _actSbjs = value; }
        }


        /// <summary>
        /// موضوع فعالیت انتخاب شده
        /// </summary>
        public TblSbjActPsn SelectedSbj { get; set; }

        public PsnInfoViewModel SelectedPerson { get; set; }


        /// <summary>
        /// اشخاص
        /// </summary>
        public ObservableCollection<TblPsn> TblPsns
        {
            get;
            private set;
        }

        public ListCollectionView TblPsnsCV { get; set; }


        public object SelectedObj
        {
            get
            {
                if (SelectedPsn != null)
                {
                    return new { SelectedPsn.FldCodPsn, SelectedPsn.FldNam1stPsn, SelectedPsn.FldNam2ndPsn };
                }
                return null;
            }
            set
            {
                dynamic d = value;
                int code = (int)d.FldCodPsn;
                SelectedPsn = context.TblPsns.Single(m => m.FldCodPsn == code);
                RaisePropertyChanged("SelectedPsn");
            }
        }
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

                if (selectedPsn == null)
                {
                    return;
                }

                LoadTypeAct();

                value.PropertyChanged += value_PropertyChanged;


            }
        }

        public DelegateCommand RejectChangesCommand { get; set; }

        public DelegateCommand AddNewPsnCommand { get; set; }

        public DelegateCommand EditPersonCommand { get; set; }

        public DelegateCommand OKAddEditPsnCommand { get; set; }

        public DelegateCommand<TblPsn> DeletePsnCommand { get; set; }

        public DelegateCommand<TblPsn> SelectPsnCommand { get; set; }

        public bool? Producing { get; set; }

        public bool? Commercial { get; set; }

        public bool? Servicing { get; set; }

        public bool? Peymankar { get; set; }

        public bool? Other { get; set; }

        public string SelectedSbjText { get; set; }


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



        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool CanAddNewPsn()
        {
            return Mode == FormMode.View && Acs_AddPsnOsdOrg;
        }

        /// <summary>
        /// Adds new user
        /// </summary>
        private void ExecuteAddNewPsnCommand()
        {
            TblPsn psn = new TblPsn();

            this.Other = this.Peymankar = this.Producing = this.Servicing = this.Commercial = false;

            psn.PropertyChanged -= value_PropertyChanged;
            psn.PropertyChanged += value_PropertyChanged;

            SelectedPsn = psn;

            Mode = FormMode.New;
        }

        /// <summary>
        /// Deletes person confirm
        /// </summary>
        private void ExecuteDeletePsnCommand(Model.TblPsn obj)
        {
            if (Util.ShowMessageBox(2, "شخص برون سازمانی") == System.Windows.MessageBoxResult.Yes)
            {
                if (PublicMethods.TryDeleteNod_2076(context, obj.Nod))
                {
                    PublicMethods.DeletePsnIsd_22154(context, obj, false);

                    TblPsns.Remove(obj);
                    TblPsnsCV.Refresh();
                }
                else
                {
                    Util.ShowMessageBox(27, "شخص برون سازمانی");
                }
            }
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
            return Mode == FormMode.View && Acs_EditPsnOsdOrg;
        }

        private void ExecuteEditPerson()
        {
            SelectedPsn.PropertyChanged -= value_PropertyChanged;
            SelectedPsn.PropertyChanged += value_PropertyChanged;
            Mode = FormMode.Edit;
        }

        private bool CanOKAddEditPsn()
        {
            //validation check goes here
            if (SelectedPsn == null)
            {
                return false;
            }
            return (Mode == FormMode.Edit || Mode == FormMode.New) && !SelectedPsn.HasErrors && ValidateNamePsn(SelectedPsn.FldNam1stPsn) && ValidateNamePsn(SelectedPsn.FldNam2ndPsn);
        }

        private void ExecuteOKAddEditPsn()
        {
            TblSbjActPsn newSbj = null;
            if (!string.IsNullOrWhiteSpace(SelectedSbjText))
            {
                if (SelectedSbj != null)
                {
                    this.SelectedPerson.Person.TblSbjActPsn = SelectedSbj;
                }
                else
                {
                    newSbj = new TblSbjActPsn() { FldNamSbjAct = SelectedSbjText };
                    this.SelectedPerson.Person.TblSbjActPsn = newSbj;

                }
            }

            if (Mode == FormMode.New)
            {
                #region ' 22171 افزودن شخص برون سازمانی به سازمان جاری '

                SaveTypeAct();

                TblOrg org = SSYM.OrgDsn.ViewModel.Base.UserManager.CurrentUser.TblOrg;

                TblUsr user = PublicMethods.CreateNewUser_1550(this.context, this.SelectedPerson.Person.FldNam2ndPsn, org);

                user.TblPsn = this.SelectedPerson.Person;

                user.TblOrg = context.TblOrgs.Single(m => m.FldCodOrg == org.FldCodOrg);

                context.TblPsns.AddObject(this.SelectedPerson.Person);

                TblPsns.Add(this.SelectedPerson.Person);

                TblPsnsCV.Refresh();

                PublicMethods.SaveContext(context);

                TblNod nod = new TblNod() { FldCodTypEty = (int)FldTypEty.Psn, FldCodEty = this.SelectedPerson.Person.FldCodPsn };

                context.TblNods.AddObject(nod);

                TblAct newAct = PublicMethods.CreateUnspsifiedAct();

                newAct.FldCodNod = nod.FldCodNod;

                context.AddToTblActs(newAct);

                PublicMethods.SaveContext(context);

                #endregion

            }

            if (Mode == FormMode.Edit)
            {
                SaveTypeAct();
                PublicMethods.SaveContext(context);
            }

            Mode = FormMode.View;
            if (newSbj != null)
            {
                this.ActSbjs.Add(newSbj);
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

                    RaisePropertyChanged("SelectedPsn");

                    break;

                case FormMode.New:

                    TblPsns.Remove(SelectedPsn);
                    TblPsnsCV.Refresh();

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



        void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OKAddEditPsnCommand.RaiseCanExecuteChanged();
        }

        private void RejectChanges()
        {
            try
            {
                PublicMethods.RollBackContext(this.context);

            }
            catch (Exception)
            {
            }
        }

        private void SaveTypeAct()
        {
            selectedPsn.FldTypAct = null;
            if (Producing.HasValue && Producing.Value)
            {
                selectedPsn.FldTypAct = 1;
            }
            if (Commercial.HasValue && Commercial.Value)
            {
                selectedPsn.FldTypAct = 2;

            }
            if (Peymankar.HasValue && Peymankar.Value)
            {
                selectedPsn.FldTypAct = 3;

            }

            if (Servicing.HasValue && Servicing.Value)
            {
                selectedPsn.FldTypAct = 4;

            }
            if (Other.HasValue && Other.Value)
            {
                selectedPsn.FldTypAct = 5;
            }

        }

        private void LoadTypeAct()
        {
            Producing = Commercial = Servicing = Peymankar = Other = false;
            if (selectedPsn.FldTypAct.HasValue)
            {
                switch (selectedPsn.FldTypAct.Value)
                {
                    case 1:
                        Producing = true;
                        break;
                    case 2:
                        Commercial = true;
                        break;
                    case 3:
                        Peymankar = true;
                        break;
                    case 4:
                        Servicing = true;
                        break;
                    case 5:
                        Other = true;
                        break;
                    default:
                        break;
                }

            }

            RaisePropertyChanged("Producing", "Commercial", "Peymankar", "Servicing", "Other");
        }

        private bool ValidateNamePsn(string input)
        {
            return !string.IsNullOrWhiteSpace(input);
            //if (input == null || input.Length == 0 || input[0] == ' ')
            //{
            //    return false;
            //}
            //for (int i = 0; i < input.Length; i++)
            //{
            //    if (input[i] <= '9' && input[i] >= '0')
            //        return false;
            //}

            //return true;
        }


        private bool TblPsnsCV_Filter(object obj)
        {
            if (!string.IsNullOrEmpty(SearchFilter))
            {
                return (obj as TblPsn).FldNam2ndPsn.Contains(SearchFilter) || (obj as TblPsn).FldNam1stPsn.Contains(SearchFilter);
            }
            return true;
        }


        #endregion

        public void SaveContext()
        {
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
