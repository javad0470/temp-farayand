using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.UserSetting
{
    public class EntrSfwViewModel : NotificationObject
    {
        #region ' Fields '

        BPMNDBEntities context;

        MenuViewModel menuVM;

        TblUsr usr;

        string namUsr;

        string pass;

        TblOrg org;

        #endregion

        #region ' Initialaizer '

        public EntrSfwViewModel()
        {
            context = new BPMNDBEntities();

            MenuVM = new MenuViewModel();

            ClearUsrPassCommand = new DelegateCommand(ExecuteClearUsrPassCommand);
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public bool IsOkEnabled
        {
            get
            {
                return this.Usr != null ? (TblUsr.CalculateMD5Hash(this.Pass) == this.Usr.FldPassUsr && this.Usr.FldNamUsrActv) : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pass
        {
            get { return pass; }
            set
            {
                pass = value;

                //RaisePropertyChanged("Pass", "IsOkEnabled");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NamUsr
        {
            get { return namUsr; }
            set
            {
                namUsr = value;

                this.Usr = context.TblUsrs.SingleOrDefault(m => m.FldNamUsr == value);

                //RaisePropertyChanged("NamUsr", "IsOkEnabled");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MenuViewModel MenuVM
        {
            get { return menuVM; }
            set { menuVM = value; }
        }

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

        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearUsrPassCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        public void PrepareContext()
        {
            MenuViewModel.MainContext = new BPMNDBEntities();

            PublicMethods.TblItmFixSfws = new List<TblItmFixSfw>(MenuViewModel.MainContext.TblItmFixSfws.ToList());

            PublicMethods.TblMsgs = new List<TblMsg>(MenuViewModel.MainContext.TblMsgs.ToList());

            UserManager.CurrentUser = MenuViewModel.MainContext.TblUsrs.SingleOrDefault(m => m.FldNamUsr == NamUsr);

            fillHelp();
        }

        public TblUsr ValidateUserPass(string userName/*, string pass*/)
        {
            return context.TblUsrs.SingleOrDefault(m => m.FldNamUsr == userName);

            // && m.FldPassUsr == TblUsr.CalculateMD5Hash(pass)
        }

        #endregion

        #region ' Private Methods '

        private void ExecuteClearUsrPassCommand()
        {
            this.NamUsr = string.Empty;

            this.Pass = string.Empty;
        }


        private void fillHelp()
        {
            PublicMethods.TblHelpDynm = new Dictionary<int, string>();

            foreach (var item in MenuViewModel.MainContext.TblHelpDynms)
            {
                PublicMethods.TblHelpDynm.Add(item.FldCod, item.FldTxtHelp);
            }
        }
        #endregion

        #region ' Events '

        #endregion

    }
}
