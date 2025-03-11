using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.Admin
{
    public class ActiveUsrViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        ObservableCollection<TblPsn> psn;

        TblPsn selectedPsn;

        TblOrg selectedOrg;

        string repeatPassUsr;
        private ListCollectionView _psnCV;
        string _originalPass;

        string _txtSrch;

        BPMNDBEntities context;

        bool canUsrChgPass;
        bool _editingUser;


        #endregion

        #region ' Initialaizer '

        public ActiveUsrViewModel()
        {
            this.context = new BPMNDBEntities();

            DetectPsn();

            this.OkCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand);

        }


        #endregion

        #region ' Properties / Commands '

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                if (_txtSrch != value)
                {
                    _txtSrch = value;
                    PsnCV.Refresh();
                }
            }
        }

        /// <summary>
        /// سازمان هایی که شخص جاری در آنها عضو است
        /// </summary>
        public List<TblOrg> SelectedPsnOrgs
        {
            get
            {
                if (SelectedPsn == null)
                {
                    return null;
                }
                List<TblOrg> orgs = TblPsn.GetOrgsOfPsn_22160(SelectedPsn);

                //سازمان جاری و سازمان های زیرمجموعه که شخص داده شده در آنها عضو است
                List<TblOrg> orgs2 = new List<TblOrg>();

                foreach (var org in orgs)
                {
                    if (PublicMethods.CurrentUser.TblOrg.FldCodOrg == org.FldCodOrg || TblOrg.IsOrgAnsestorOfThisOrg(PublicMethods.CurrentUser.TblOrg, org))
                    {
                        orgs2.Add(org);
                    }
                }

                SelectedOrg = orgs2.FirstOrDefault();

                return orgs2;
            }
        }

        /// <summary>
        /// امکان فعال کردن کاربری که نماینده هیچ سازمان/جایگاه/سمت/نقش نیست، نباید وجود داشته باشد
        /// </summary>
        public bool CanEnableUser
        {
            get
            {
                return true;
                //if (SelectedUsr != null && SelectedOrg != null)
                //{
                //    return PublicMethods.GetAgntOfPsnIsdOrg_22230(context, SelectedUsr.TblPsn.FldCodPsn, SelectedOrg.FldCodOrg).Count > 0;
                //}
                //else
                //{
                //    return true;
                //}
            }
        }

        /// <summary>
        ///امکان غیر فعال کردن کاربر شخص جاری در سازمان جاری نباید وجود داشته باشد
        /// </summary>
        public bool CanDisableUser
        {
            get
            {
                if (SelectedUsr == null)
                {
                    return false;
                }

                return !(SelectedUsr.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg && SelectedUsr.TblPsn.FldCodPsn == PublicMethods.CurrentUser.TblPsn.FldCodPsn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblPsn> Psn
        {
            get { return psn; }
            set
            {
                psn = value;

                RaisePropertyChanged("Psn");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblPsn SelectedPsn
        {
            get
            {
                return selectedPsn;
            }
            set
            {
                selectedPsn = value;

                RaisePropertyChanged("SelectedPsn", "SelectedOrg", "SelectedUsr", "DtlVisible", "CanDisableUser", "CanEnableUser", "SelectedPsnOrgs");

                RaiseCanOk();

                this.CanUsrChgPass = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblOrg SelectedOrg
        {
            get
            {
                return selectedOrg;
                ////if (selectedOrg == null && this.SelectedPsn != null && this.SelectedPsn.OrgsOfPsn.Count > 0)
                ////{
                ////    return this.SelectedPsn.OrgsOfPsn.First();
                ////}

                ////return selectedOrg;

                //return PublicMethods.CurrentUser.TblOrg;
            }

            set
            {
                selectedOrg = value;

                RaisePropertyChanged("SelectedOrg", "SelectedUsr", "DtlVisible", "CanDisableUser", "CanEnableUser", "EditingUser");

                RaiseCanOk();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanUsrChgPass
        {
            get { return canUsrChgPass; }
            set
            {
                canUsrChgPass = value;
                RepeatPassUsr = OriginalPass = string.Empty;
                RaisePropertyChanged("CanUsrChgPass");
            }
        }

        public bool DtlVisible
        {
            get
            {
                return SelectedUsr != null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblUsr SelectedUsr
        {
            get
            {
                if (this.SelectedOrg != null && this.SelectedPsn != null)
                {
                    var usr = this.SelectedPsn.TblUsrs.Where(m => m.FldCodOrg == this.SelectedOrg.FldCodOrg).First();
                    //usr.PropertyChanged -= usr_PropertyChanged;
                    //usr.PropertyChanged += usr_PropertyChanged;
                    return usr;
                }

                return null;
            }
        }

        //void usr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Username")
        //    {
        //        RaiseCanOk();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public string RepeatPassUsr
        {
            get { return repeatPassUsr; }
            set
            {
                repeatPassUsr = value;

                if (value != OriginalPass)
                {
                    (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
                    throw new Exception("تکرار رمز عبور با رمز عبور  اصلی برابر نیست.");
                }

                RaisePropertyChanged("RepeatPassUsr");

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        public string OriginalPass
        {
            get { return _originalPass; }
            set
            {
                _originalPass = value;
                RaisePropertyChanged("OriginalPass");

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// امکان ویرایش اطلاعات یوزر
        /// </summary>
        public bool EditingUser
        {
            get { return _editingUser && !isAdmin(); }
            set
            {
                _editingUser = value;
                RaisePropertyChanged("EditingUser");
            }
        }

        public ListCollectionView PsnCV
        {
            get
            {
                if (psn == null)
                {
                    return null;
                }
                if (_psnCV == null)
                {
                    _psnCV = new ListCollectionView(psn);
                    _psnCV.Filter = new Predicate<object>(filterPsns);
                }
                return _psnCV;
                
            }
            set { _psnCV = value; }
        }

        private bool filterPsns(object obj)
        {
            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            var ps = obj as TblPsn;

            if (ps == null)
            {
                return true;
            }


            return ps.FldNam1stPsn.Trim().ToLower().Contains(TxtSrch.Trim().ToLower()) || ps.FldNam2ndPsn.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
        }

        #endregion

        #region ' Public Methods '


        /// <summary>
        /// زمانی که کاربر از ویرایش اطلاعات کاربر انصراف می دهد
        /// </summary>
        public void CancelEdit()
        {
            PublicMethods.RollBackContext(this.context);
            SelectedUsr.Username = SelectedUsr.FldNamUsr;
            this.EditingUser = false;
            CanUsrChgPass = false;
        }

        /// <summary>
        /// وقتی که کاربر شروع به ویرایش کاربر میکند
        /// </summary>
        public void StartEditing()
        {
            if (isAdmin())
            {
                Util.ShowNotification(76);
                return;
            }
            //else
            //{
            //    Util.HideNotification();
            //}

            this.EditingUser = true;
        }




        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void DetectPsn()
        {
            //PublicMethods.ReloadEntity(context, context.TblPsns);

            this.Psn = new ObservableCollection<TblPsn>(PublicMethods.GetPsnInOrg(this.context));

            //TblOrg currentOrg = context.TblOrgs.Single(o => o.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);

            //foreach (TblUsr item in currentOrg.TblUsrs)
            //{
            //    if (!this.Psn.Contains(item.TblPsn) && item.TblPsn.FldIsdOrg)
            //    {
            //        this.Psn.Add(item.TblPsn);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOkCommand()
        {

            //در فرم فعال کردن کاربران، امکان فعال کردن کاربری که نماینده هیچ سازمان/جایگاه/سمت/نقش نیست، نباید وجود داشته باشد.
            if (SelectedUsr != null && SelectedOrg != null)
            {
                if (PublicMethods.GetAgntOfPsnIsdOrg_22230(context, SelectedUsr.TblPsn.FldCodPsn, SelectedOrg.FldCodOrg).Count == 0)
                {
                    if (SelectedUsr.FldNamUsrActv)
                    {
                        Util.ShowMessageBox(58, SelectedOrg.FldNamOrg);
                        PublicMethods.RollBackContext(this.context);
                        SelectedUsr.FldNamUsrActv = false;
                        return;

                    }
                }
            }

            if (CanUsrChgPass)
            {
                SelectedUsr.FldPassUsr = TblUsr.CalculateMD5Hash(OriginalPass);
            }
            PublicMethods.SaveContext(this.context);
            this.EditingUser = false;
            CanUsrChgPass = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteOkCommand()
        {
            if (this.SelectedUsr != null)
            {
                return validateUsername()
                &&
                validatePassword();
            }

            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RaiseCanOk()
        {
            this.SelectedUsr.PropertyChanged -= SelectedUsr_PropertyChanged;

            this.SelectedUsr.PropertyChanged += SelectedUsr_PropertyChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SelectedUsr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            (this.OkCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private bool validateUsername()
        {
            //نام کاربر تکراری نباشد
            return !this.context.TblUsrs.Any(m => m.FldCodUsr != SelectedUsr.FldCodUsr && m.FldNamUsr.Trim().ToLower() == SelectedUsr.ChangedUsername.Trim().ToLower());
        }

        private bool validatePassword()
        {
            if (CanUsrChgPass)
            {
                if (!string.IsNullOrWhiteSpace(OriginalPass))
                {
                    return this.OriginalPass == this.RepeatPassUsr;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool isAdmin()
        {
            if (SelectedPsn != null && SelectedOrg != null)
            {
                if (SelectedPsn.TblUsrs.Any(u => u.FldCodOrg == SelectedOrg.FldCodOrg && u.FldNamUsr.Trim() == "admin"))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region ' Events '

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
