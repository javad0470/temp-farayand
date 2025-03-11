using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using System.Windows.Controls;
using System.Globalization;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.UserSetting
{
    public class UsernameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string newUsrNam = value as string;
            string error_context = null;
            bool is_valid = true;

            if (!string.IsNullOrWhiteSpace(newUsrNam))
            {
                if (MenuViewModel.MainContext.TblUsrs.Any(u => u.FldCodUsr != PublicMethods.CurrentUser.FldCodUsr
                    &&
                    u.FldNamUsr.Trim().ToLower() == newUsrNam.Trim().ToLower()))
                {
                    is_valid = false;
                    error_context = "نام کاربری نامعتبر است یا قبلا ثبت شده است.";
                }
            }

            return new ValidationResult(is_valid, error_context);
        }
    }

    public class UsrSettingViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities context;

        TblUsr usr;

        string repeatPass;

        bool _isChgNamUsrSelected;

        string passOld;

        string passNew;

        string namUsrNew;

        bool _validateRes = false;

        #endregion

        #region ' Initialaizer '

        public UsrSettingViewModel()
        {
            this.context = new BPMNDBEntities();

            this.Usr = this.context.TblUsrs.Single(u => u.FldCodUsr == PublicMethods.CurrentUser.FldCodUsr);

            OkCommand = new DelegateCommand(executeOkCommand, canExecuteOkCommand);

            this._isChgNamUsrSelected = true;

            this.NamUsrNew = Usr.FldNamUsr;

            if (IsAdmin)
            {
                Task t = new Task(new Action(showAlert));
                t.Start();
            }
        }

        private void showAlert()
        {
            System.Threading.Thread.Sleep(500);
            Util.ShowNotification(76);
            //System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
            //    {
            //    }));
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public TblUsr Usr
        {
            get { return usr; }
            set
            {
                usr = value;

                RaisePropertyChanged("Usr");
            }
        }

        public bool IsAdmin
        {
            get
            {
                if (this.Usr != null)
                {
                    return this.Usr.FldNamUsr.ToLower() == "admin";
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChgNamUsrSelected
        {
            get { return _isChgNamUsrSelected; }
            set
            {

                if (value) // تب اول
                {
                    if (canExecuteOkCommand())
                    {
                        if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                        {
                            executeOkCommand(false);
                        }
                        else
                        {
                            passOld = passNew = repeatPass = string.Empty;
                            RaisePropertyChanged("PassOld", "PassNew", "RepeatPass");
                        }
                    }
                    else
                    {
                        passOld = passNew = repeatPass = string.Empty;
                        RaisePropertyChanged("PassOld", "PassNew", "RepeatPass");
                    }
                }
                else // تب دوم
                {
                    if (NamUsrNew != Usr.FldNamUsr.Trim().ToLower())
                    {
                        if (canExecuteOkCommand())
                        {
                            if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                            {
                                executeOkCommand(false);
                            }
                            else
                            {
                                NamUsrNew = Usr.FldNamUsr.Trim().ToLower();
                            }
                        }
                        else
                        {
                            NamUsrNew = Usr.FldNamUsr.Trim().ToLower();
                        }
                    }
                }

                _isChgNamUsrSelected = value;

                RaisePropertyChanged("IsChgNamUsrSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OkCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PassOld
        {
            get { return passOld; }
            set
            {
                passOld = value;

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();


                if (!_validateRes && !string.IsNullOrEmpty(this.PassOld) && TblUsr.CalculateMD5Hash(this.PassOld) != this.Usr.FldPassUsr)
                {
                    throw new Exception("رمز عبور اشتباه است.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PassNew
        {
            get { return passNew; }
            set
            {
                passNew = value;

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();

                if (!_validateRes && this.PassNew == this.PassOld)
                {
                    throw new Exception("رمز عبور جدید نباید با رمز عبور قبلی برابر باشد.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RepeatPass
        {
            get { return repeatPass; }
            set
            {
                repeatPass = value;

                RaisePropertyChanged("RepeatPass");

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();

                if (!_validateRes && this.PassNew != this.RepeatPass)
                {
                    throw new Exception("تکرار رمز عبور مطابق رمز عبور جدید نیست.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NamUsrNew
        {
            get { return namUsrNew; }
            set
            {

                namUsrNew = value;

                Exception ex = null;
                if (!string.IsNullOrWhiteSpace(namUsrNew))
                {
                    if (this.context.TblUsrs.Any(u => u.FldCodUsr != PublicMethods.CurrentUser.FldCodUsr
                        &&
                        u.FldNamUsr.Trim().ToLower() == namUsrNew.Trim().ToLower()))
                    {
                        ex = new Exception("نام کاربری نامعتبر است یا قبلا ثبت شده است.");
                    }
                }


                if (!string.IsNullOrWhiteSpace(namUsrNew))
                {
                    namUsrNew = namUsrNew.Trim().ToLower();
                }

                (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged("NamUsrNew");

                if (ex != null)
                {
                    throw ex;
                }
            }
        }

        //bool _chgPassEnabled;
        //public bool ChgPassEnabled
        //{
        //    get { return _chgPassEnabled; }
        //    set
        //    {
        //        _chgPassEnabled = false;
        //        Util.ShowMessageBox(5);
        //        RaisePropertyChanged("ChgPassEnabled");
        //    }
        //}

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool canExecuteOkCommand()
        {
            _validateRes = true;

            if (IsChgNamUsrSelected)//تب اول
            {
                if (!string.IsNullOrWhiteSpace(this.NamUsrNew) && this.NamUsrNew.Trim().ToLower() != usr.FldNamUsr)
                {
                    if (context.TblUsrs.Any(u => u.FldCodUsr != PublicMethods.CurrentUser.FldCodUsr
                        &&
                        u.FldNamUsr.Trim().ToLower() == this.NamUsrNew.Trim().ToLower()))
                    {
                        _validateRes = false;
                        return _validateRes;
                    }

                    _validateRes = true;
                    return _validateRes;
                }

                _validateRes = false;
                return _validateRes;
            }
            else// تب دوم
            {
                if (!string.IsNullOrWhiteSpace(this.PassNew)
                    && !string.IsNullOrWhiteSpace(this.PassOld)
                    && TblUsr.CalculateMD5Hash(this.PassOld) == this.Usr.FldPassUsr
                    && this.PassNew == this.RepeatPass
                    && this.PassOld != this.PassNew)
                {
                    _validateRes = true;
                    return _validateRes;
                }

                _validateRes = false;
                return _validateRes;
            }
        }


        private void executeOkCommand()
        {
            executeOkCommand(true);
        }
        /// <summary>
        /// 
        /// </summary>
        private void executeOkCommand(bool showConfirm = true)
        {
            if (IsChgNamUsrSelected)
            {
                if (showConfirm)
                {
                    if (Util.ShowMessageBox(47) == MessageBoxResult.Yes)
                    {
                        this.Usr.FldNamUsr = this.NamUsrNew;
                    }
                }
                else
                {
                    this.Usr.FldNamUsr = this.NamUsrNew;
                    return;
                }
            }

            else
            {
                if (Util.ShowMessageBox(48) == MessageBoxResult.Yes)
                {
                    this.Usr.FldPassUsr = TblUsr.CalculateMD5Hash(this.PassNew);
                }
                else
                {
                    return;
                }
            }

            saveChanges();
        }

        private void saveChanges()
        {
            PublicMethods.SaveContext(this.context);
            this.ShowNotification("تغییرات با موفقیت ذخیره شد.", MessageBoxType.Question, true);
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
