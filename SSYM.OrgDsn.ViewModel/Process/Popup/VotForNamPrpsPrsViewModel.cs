using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class VotForNamPrpsPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        TblNamPrpsPr namPrpsPrs;
        IEtyNod selectedNod;
        bool isAgreeSelected;
        bool isDisAgreeSelected;
        bool isNuetralSelected;
        SlcPosPstRolViewModel _slcPosPstRolVM;


        #endregion

        #region ' Initialaizer '

        public VotForNamPrpsPrsViewModel(BPMNDBEntities context)
            : base(context)
        {

            SlcPstPosViewModel posPst = new SlcPstPosViewModel(this.bpmnEty);
            SlcRolViewModel slcRol = new SlcRolViewModel(this.bpmnEty);
            _slcPosPstRolVM = new SlcPosPstRolViewModel() { PosPstSelectVM = posPst, RolSlcVM = slcRol };

            SelectPosPstCommand = new DelegateCommand(selectPosPstExecute);
            IsAgreeSelected = true;
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// نام پیشنهادی فرآیند
        /// </summary>
        public TblNamPrpsPr NamPrpsPrs
        {
            get { return namPrpsPrs; }
            set
            {
                namPrpsPrs = value;
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
        /// گره در حالت انتخاب
        /// </summary>
        public IEtyNod SelectedNod
        {
            get { return selectedNod; }
            set
            {
                selectedNod = value;
                SlcCurrentPosPstVot();
                RaiseOKCanExecute();
                RaisePropertyChanged("SelectedNod", "TnoActVotEer");
            }
        }

        /// <summary>
        /// رأی مثبت در حالت انتخاب قرار دارد
        /// </summary>
        public bool IsAgreeSelected
        {
            get { return isAgreeSelected; }
            set
            {
                isAgreeSelected = value;
                RaisePropertyChanged("IsAgreeSelected");
            }
        }

        /// <summary>
        /// رأی منفی در حالت انتخاب قرار دارد
        /// </summary>
        public bool IsDisAgreeSelected
        {
            get { return isDisAgreeSelected; }
            set
            {
                isDisAgreeSelected = value;
                RaisePropertyChanged("IsDisAgreeSelected");
            }
        }

        /// <summary>
        /// بدون رأی در حالت انتخاب قرار دارد
        /// </summary>
        public bool IsNuetralSelected
        {
            get { return isNuetralSelected; }
            set
            {
                isNuetralSelected = value;
                RaisePropertyChanged("IsNuetralSelected");
            }
        }

        public int TnoActVotEer
        {
            get
            {
                if (this.SelectedNod != null)
                {
                    TblNod nod = this.SelectedNod.Nod;
                    int i = PublicMethods.ComputeTnoActOfNodInnPrs_1606(bpmnEty, NamPrpsPrs.TblPr, nod);
                    return i;
                }

                return 0;
            }
        }

        public ICommand SelectPosPstCommand { get; set; }


        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {
            if (SelectedNod != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ' Private Methods '


        private void selectPosPstExecute()
        {
            Util.ShowPopup(_slcPosPstRolVM);

            if (_slcPosPstRolVM.Result == PopupResult.OK)
            {
                if (_slcPosPstRolVM.PosPostSelected && _slcPosPstRolVM.PosPstSelectVM.SelectedPosPst != null)
                {
                    this.SelectedNod = _slcPosPstRolVM.PosPstSelectVM.SelectedPosPst;
                }
                else if (_slcPosPstRolVM.RolSlcVM.SelectedRol != null)
                {
                    this.SelectedNod = _slcPosPstRolVM.RolSlcVM.SelectedRol;
                }
            }
        }


        /// <summary>
        /// P1621
        /// </summary>
        private void SlcCurrentPosPstVot()
        {
            if (this.SelectedNod != null)
            {
                //1622
                TblVotNamPrpsPr tbl = this.NamPrpsPrs.TblVotNamPrpsPrs.SingleOrDefault(m => m.FldCodNodVotEer == SelectedNod.Nod.FldCodNod);

                //1788
                if (tbl != null)
                {
                    //1623
                    if (tbl.FldVot == (int)Model.Enum.TypVot.Agree)
                    {
                        IsAgreeSelected = true;
                    }
                    else if (tbl.FldVot == (int)Model.Enum.TypVot.DisAgree)
                    {
                        IsDisAgreeSelected = true;
                    }
                    else if (tbl.FldVot == (int)Model.Enum.TypVot.Neutral)
                    {
                        IsNuetralSelected = true;
                    }
                }
                //1789
                else
                {
                    //1790
                    IsAgreeSelected = true;
                }
            }
        }

        #endregion

        #region ' events '

        #endregion

    }
}
