using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.Model.Base;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class PrpsNamForPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        string namPrpsPrs;
        IEtyNod selectedNod;
        TblPr currentPrs;
        SlcPosPstRolViewModel _slcPosPstRolVM;


        #endregion

        #region ' Initialaizer '

        public PrpsNamForPrsViewModel(BPMNDBEntities context)
            : base(context)
        {
            SlcRolViewModel RolSlcVM = new SlcRolViewModel(context, UserManager.CurrentUser.TblPsn);
            SlcPstPosViewModel posPst = new SlcPstPosViewModel(this.bpmnEty);
            SlcPosPstRolVM = new SlcPosPstRolViewModel() { PosPstSelectVM = posPst, RolSlcVM = RolSlcVM };
            SelectNodCommand = new DelegateCommand(SelectNodExecute);
        }

        #endregion

        #region ' Properties / Commands '
        public SlcPosPstRolViewModel SlcPosPstRolVM
        {
            get { return _slcPosPstRolVM; }
            set { _slcPosPstRolVM = value; }
        }

        public ICommand SelectNodCommand { get; set; }

        public SlcRolViewModel RolSlcVM { get; set; }

        /// <summary>
        /// نام پیشنهادی فرآیند
        /// </summary>
        public string NamPrpsPrs
        {
            get { return namPrpsPrs; }
            set
            {
                namPrpsPrs = value;
                RaiseOKCanExecute();
                RaisePropertyChanged("NamPrpsPrs");
            }
        }

        ///// <summary>
        ///// لیست جایگاه ها و سمت های سازمانی سازمان جاری
        ///// </summary>
        //public ObservableCollection<TblPosPstOrg> PosPst
        //{
        //    get { return posPst; }
        //    set
        //    {
        //        posPst = value;
        //        RaisePropertyChanged("PosPst");
        //    }
        //}

        /// <summary>
        /// نود در حالت انتخاب
        /// </summary>
        public IEtyNod SelectedNod
        {
            get { return selectedNod; }
            set
            {
                selectedNod = value;
                RaiseOKCanExecute();
                RaisePropertyChanged("SelectedNod", "TnoActPrpsEer");
            }
        }

        /// <summary>
        /// فرآیند جاری
        /// </summary>
        public TblPr CurrentPrs
        {
            get { return currentPrs; }
            set
            {
                currentPrs = value;
                RaisePropertyChanged("TnoActPrpsEer");
            }
        }

        /// <summary>
        /// تعداد فعالیتهای پیشنهاد دهنده در فرآیند
        /// </summary>
        public int TnoActPrpsEer
        {
            get
            {
                if (this.SelectedNod != null && this.CurrentPrs != null)
                {
                    //TblNod nod = PublicMethods.DetectNodOfPosPst_753(bpmnEty, this.SelectedNod);
                    TblNod nod = this.SelectedNod.Nod;
                    int i = PublicMethods.ComputeTnoActOfNodInnPrs_1606(bpmnEty, CurrentPrs, nod);
                    return i;
                }

                return 0;
            }
        }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {
            if (SelectedNod != null && this.NamPrpsPrs != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ' Private Methods '
        private void SelectNodExecute(object obj)
        {
            Util.ShowPopup(SlcPosPstRolVM);

            if (SlcPosPstRolVM.Result == PopupResult.OK)
            {
                if (SlcPosPstRolVM.PosPostSelected)
                {
                    this.SelectedNod = SlcPosPstRolVM.PosPstSelectVM.SelectedPosPst;
                }
                else
                {
                    this.SelectedNod = SlcPosPstRolVM.RolSlcVM.SelectedRol;
                }
            }
        }



        //void RolSlcVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "SelectedRol")
        //    {
        //        if (RolSlcVM.SelectedRol != null)
        //        {
        //            SelectedNod = RolSlcVM.SelectedRol;
        //        }
        //    }
        //}

        #endregion

        #region ' events '

        #endregion

    }
}
