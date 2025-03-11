using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class PrpsOwrForPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        //ObservableCollection<TblPosPstOrg> posPst;
        //ObservableCollection<TblPosPstOrg> _posPstInPrs;

        IEtyNod selectedNode;
        IEtyNod selectedOwr;

        TblPr currentPrs;


        #endregion

        #region ' Initialaizer '

        public PrpsOwrForPrsViewModel(BPMNDBEntities context)
            : base(context)
        {

            RolSlcVM = new SlcRolViewModel(context, UserManager.CurrentUser.TblPsn);
            SlcPstPosVM = new SlcPstPosViewModel(context);

            SlcPosPstRolVM = new SlcPosPstRolViewModel() { RolSlcVM = RolSlcVM, PosPstSelectVM = SlcPstPosVM };

            SelectOwrCommand = new DelegateCommand(selectOwrExecute);
            SelectNodCommand = new DelegateCommand(selectNodCommand);
        }

        #endregion

        #region ' Properties / Commands '

        public ICommand SelectOwrCommand { get; set; }

        public ICommand SelectNodCommand { get; set; }

        /// <summary>
        /// نقش ها
        /// </summary>
        public SlcRolViewModel RolSlcVM { get; set; }

        public SlcPstPosViewModel SlcPstPosVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SlcPosPstRolViewModel SlcPosPstRolVM { get; set; }


        /// <summary>
        /// نقش هایی که در فرایند شرکت کرده اند و میتوانند مالک فرایند باشند
        /// </summary>
        public SlcRolViewModel RolSlcInPrsVM { get; set; }

        /// <summary>
        /// جایگاه و سمت هایی که در فرایند شرکت کرده اند و میتوانند مالک فرایند باشند
        /// </summary>
        public SlcPstPosViewModel SlcPstPosInPrsVM { get; set; }


        public SlcPosPstRolViewModel SlcPosPstRolInPrsVM { get; set; }

        /// <summary>
        /// مالک انتخاب شده
        /// </summary>
        public IEtyNod SelectedOwr
        {
            get { return selectedOwr; }
            set
            {
                selectedOwr = value;
                RaisePropertyChanged("SelectedOwr");
            }
        }


        /// <summary>
        /// جایگاه یا سمت در حالت انتخاب
        /// </summary>
        public IEtyNod SelectedNod
        {
            get { return selectedNode; }
            set
            {
                selectedNode = value;
                RaiseOKCanExecute();
                RaisePropertyChanged("SelectedNod", "TnoActPrpsEer");
            }
        }

        /// <summary>
        /// فرآیند جاری
        /// </summary>
        public TblPr CurrentPrs
        {
            get
            {
                return currentPrs;
            }
            set
            {
                currentPrs = value;

                SlcPstPosInPrsVM = new SlcPstPosViewModel(this.bpmnEty, currentPrs.FldCodPrs);

                RolSlcInPrsVM = new SlcRolViewModel(this.bpmnEty, currentPrs);

                SlcPosPstRolInPrsVM = new SlcPosPstRolViewModel() { PosPstSelectVM = SlcPstPosInPrsVM, RolSlcVM = RolSlcInPrsVM };

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
            if (SelectedNod != null && this.SelectedOwr != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ' Private Methods '

        private void selectNodCommand()
        {
            Util.ShowPopup(SlcPosPstRolVM);

            if (SlcPosPstRolVM.Result == PopupResult.OK)
            {
                if (SlcPosPstRolVM.PosPostSelected)
                {
                    SelectedNod = SlcPosPstRolVM.PosPstSelectVM.SelectedPosPst;
                }
                else
                {
                    SelectedNod = SlcPosPstRolVM.RolSlcVM.SelectedRol;
                }
            }
        }

        private void selectOwrExecute()
        {
            Util.ShowPopup(SlcPosPstRolInPrsVM);

            if (SlcPosPstRolInPrsVM.Result == PopupResult.OK)
            {
                if (SlcPosPstRolInPrsVM.PosPostSelected)
                {
                    SelectedOwr = SlcPosPstRolInPrsVM.PosPstSelectVM.SelectedPosPst;
                }
                else
                {
                    SelectedOwr = SlcPosPstRolInPrsVM.RolSlcVM.SelectedRol;
                }
            }
        }


        #endregion

        #region ' events '

        #endregion

    }
}
