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
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class VotForOwrPrpsPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        TblOwrPrpsPr owrPrpsPr;


        IEtyNod _selectedNod;
        bool isAgreeSelected;
        bool isDisAgreeSelected;
        bool isNuetralSelected;
        SlcPosPstRolViewModel _slcPosPst;


        #endregion

        #region ' Initialaizer '

        public VotForOwrPrpsPrsViewModel(BPMNDBEntities context)
            : base(context)
        {
            //this.PosPst = new ObservableCollection<TblPosPstOrg>(PublicMethods.DetectPosPstOrg_1607(context, UserManager.CurrentUser.TblOrg));
            IsAgreeSelected = true;
            _slcPosPst = new SlcPosPstRolViewModel();
            _slcPosPst.PosPstSelectVM = new SlcPstPosViewModel(bpmnEty);
            _slcPosPst.RolSlcVM = new SlcRolViewModel(bpmnEty);
            SelectNodCommand = new DelegateCommand(selectNodExecute);
        }

        #endregion

        #region ' Properties / Commands '

        public ICommand SelectNodCommand { get; set; }

        public TblOwrPrpsPr OwrPrpsPr
        {
            get { return owrPrpsPr; }
            set
            {
                owrPrpsPr = value;
                RaisePropertyChanged("OwrPrpsPr");
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
        /// جایگاه یا سمت در حالت انتخاب
        /// </summary>
        public IEtyNod SelectedNod
        {
            get { return _selectedNod; }
            set
            {
                _selectedNod = value;
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
                    int i = PublicMethods.ComputeTnoActOfNodInnPrs_1606(bpmnEty, OwrPrpsPr.TblPr, this.SelectedNod.Nod);
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
            if (SelectedNod != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region ' Private Methods '

        private void selectNodExecute()
        {
            Util.ShowPopup(_slcPosPst);

            if (_slcPosPst.Result == PopupResult.OK)
            {
                if (_slcPosPst.PosPostSelected)
                {
                    this.SelectedNod = _slcPosPst.PosPstSelectVM.SelectedPosPst;
                }
                else
                {
                    this.SelectedNod = _slcPosPst.RolSlcVM.SelectedRol;
                }
            }
        }

        /// <summary>
        /// P1791
        /// </summary>
        private void SlcCurrentPosPstVot()
        {
            if (this.SelectedNod != null)
            {
                //1792
                TblVotOwrPrp tbl = this.OwrPrpsPr.TblVotOwrPrps.SingleOrDefault(m => m.FldCodNodVotEer == SelectedNod.Nod.FldCodNod);

                //1794
                if (tbl != null)
                {
                    //1798
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
                //1793
                else
                {
                    //1799
                    IsAgreeSelected = true;
                }
            }
        }

        #endregion

        #region ' events '

        #endregion

    }
}
