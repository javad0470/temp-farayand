using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.Report
{

    public delegate void ReportChangedHandler(IReport selectedReport, System.Collections.IEnumerable reportResult);

    public class MainReportMenuViewModel : BaseViewModel, IViewModel
    {

        #region ' Fields '

        IReport _baseReportSearchVM;

        bool _isSearchFilterPopupOpen;

        #endregion

        #region ' Initialaizer '

        public MainReportMenuViewModel()
        {
            initCommands();
        }

        #endregion

        #region ' Properties / Commands '

        public bool IsSearchFilterPopupOpen
        {
            get { return _isSearchFilterPopupOpen; }
            set
            {
                _isSearchFilterPopupOpen = value;
                RaisePropertyChanged("IsSearchFilterPopupOpen");
            }
        }


        public IReport BaseReportSearchVM
        {
            get { return _baseReportSearchVM; }
            set
            {
                if (_baseReportSearchVM == null)
                {
                    _baseReportSearchVM = value;
                    RaisePropertyChanged("BaseReportSearchVM");
                }
                else
                {
                    if (_baseReportSearchVM.GetType().Name != value.GetType().Name)
                    {
                        _baseReportSearchVM = value;
                        RaisePropertyChanged("BaseReportSearchVM");
                    }
                }

                //IsSearchFilterPopupOpen = true;

                object o = BaseReportSearchVM;

                _baseReportSearchVM.ReportResultCreated -= _baseReportSearchVM_ReportResultCreated;
                _baseReportSearchVM.ReportResultCreated += _baseReportSearchVM_ReportResultCreated;

                Util.ShowPopup(o as PopupViewModel);

                //if (BaseReportSearchVM.res)
                //{
                    
                //}


            }
        }

        void _baseReportSearchVM_ReportResultCreated(object sender, ReportEventArgs e)
        {
            if (ReportChanged != null)
            {
                ReportChanged(_baseReportSearchVM, e.Data);
            }
        }

        /// <summary>
        /// گزارش فعالیت ها
        /// </summary>
        public ICommand ActReportCommand { get; set; }

        /// <summary>
        /// گزارش فرایند ها
        /// </summary>
        public ICommand PrsReportCommand { get; set; }

        /// <summary>
        /// گزارش ناهمسانی ها
        /// </summary>
        public ICommand DsonReportCommand { get; set; }

        #region ' Org Report '

        /// <summary>
        /// گزارش جایگاه و سمت
        /// </summary>
        public ICommand PosPstReportCommand { get; set; }


        /// <summary>
        /// گزارش سازمان های تابعه
        /// </summary>
        public ICommand DepReportCommand { get; set; }


        /// <summary>
        /// گزارش افراد برون سازمانی
        /// </summary>
        public ICommand PsnOutsideOrgReportCommand { get; set; }


        /// <summary>
        /// گزارش افراد درون سازمانی
        /// </summary>
        public ICommand PsnInsideOrgReportCommand { get; set; }


        /// <summary>
        /// گزارش سازمان های بیرونی
        /// </summary>
        public ICommand OuterOrgReportCommand { get; set; }


        /// <summary>
        /// گزارش نقش های درون سازمانی
        /// </summary>
        public ICommand InsideRoleReportCommand { get; set; }


        /// <summary>
        /// گزارش نقش های برون سازمانی
        /// </summary>
        public ICommand OutsideRoleReportCommand { get; set; }

        #endregion

        #region ' Other Reports '


        /// <summary>
        /// گزارش واحد سنجش
        /// </summary>
        public ICommand UnitOfMesureReportCommand { get; set; }


        /// <summary>
        /// گزارش شاخص
        /// </summary>
        public ICommand IndexReportCommand { get; set; }


        /// <summary>
        /// گزارش خطا
        /// </summary>
        public ICommand ErrorReportCommand { get; set; }


        /// <summary>
        /// گزارش نرمافزار
        /// </summary>
        public ICommand SoftwareReportCommand { get; set; }

        /// <summary>
        /// گزارش خبر
        /// </summary>
        public ICommand NewsReportCommand { get; set; }


        /// <summary>
        /// گزارش ورودی خروجی
        /// </summary>
        public ICommand InOutReportCommand { get; set; }


        #endregion

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private void initCommands()
        {
            ActReportCommand = new DelegateCommand(actReportExecute);

            PrsReportCommand = new DelegateCommand(prsReportExecute);

            DsonReportCommand = new DelegateCommand(dsonReportExecute);

            PosPstReportCommand = new DelegateCommand(posPstReportExecute);

            DepReportCommand = new DelegateCommand(depOrgReportExecute);

            PsnOutsideOrgReportCommand = new DelegateCommand(psnOutsideOrgReportExecute);

            PsnInsideOrgReportCommand = new DelegateCommand(psnInsideOrgReportExecute);

            OuterOrgReportCommand = new DelegateCommand(outerOrgReportExecute);

            InsideRoleReportCommand = new DelegateCommand(insideRoleReportExecute);

            OutsideRoleReportCommand = new DelegateCommand(outsideRoleReportExecute);

            UnitOfMesureReportCommand = new DelegateCommand(unitOfMesureReportExecute);

            IndexReportCommand = new DelegateCommand(indexReportExecute);

            ErrorReportCommand = new DelegateCommand(errorReportExecute);

            SoftwareReportCommand = new DelegateCommand(softwareReportExecute);

            NewsReportCommand = new DelegateCommand(newsReportExecute);

            InOutReportCommand = new DelegateCommand(inOutReportExecute);
        }

        private void inOutReportExecute()
        {
            BaseReportSearchVM = new RpotInOutViewModel();
        }

        private void newsReportExecute()
        {
            BaseReportSearchVM = new RpotNewsViewModel();
        }

        private void softwareReportExecute()
        {
            BaseReportSearchVM = new RpotSoftViewModel();
        }

        private void errorReportExecute()
        {
            BaseReportSearchVM = new RpotErrViewModel();
        }

        private void indexReportExecute()
        {
            BaseReportSearchVM = new RpotIdxViewModel();
        }

        private void unitOfMesureReportExecute()
        {
            BaseReportSearchVM = new RpotMsrtUnitViewModel();
        }

        private void outsideRoleReportExecute()
        {
            BaseReportSearchVM = new RpotRolOutViewModel();
        }

        private void insideRoleReportExecute()
        {
            BaseReportSearchVM = new RpotRolInViewModel();
        }

        private void outerOrgReportExecute()
        {
            BaseReportSearchVM = new RpotOutOrgViewModel();
        }

        private void psnInsideOrgReportExecute()
        {
            BaseReportSearchVM = new RpotPsnInViewModel();
        }

        private void psnOutsideOrgReportExecute()
        {
            BaseReportSearchVM = new RpotPsnOutViewModel();
        }

        private void depOrgReportExecute()
        {
            BaseReportSearchVM = new RpotOrgViewModel();
        }

        private void posPstReportExecute()
        {
            BaseReportSearchVM = new RpotPosPstViewModel();
        }

        private void dsonReportExecute()
        {
            BaseReportSearchVM = new RpotDsonViewModel();
        }

        private void prsReportExecute()
        {
            BaseReportSearchVM = new RpotPrsViewModel();
        }

        private void actReportExecute()
        {
            BaseReportSearchVM = new RpotActViewModel();
        }

        #endregion

        #region ' Events '

        public event ReportChangedHandler ReportChanged;

        #endregion


        public void SaveContext()
        {
            
        }

        public bool ConfirmAndClose()
        {
            return true;
        }

    }
}
