using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefOralViewModel : PopupViewModel
    {
        #region ' Fields '

        private Model.TblSbjOral tblOral;
        private TblAct _previousActivity;
        private int _codSelectedNod;
        TblEvtSrt _srt;
        private bool _isSelectedSourceEnable = true;
        #endregion

        #region ' Initialaizer '

        //public DefIntViewModel(DtlIntViewModel parent)
        //{
        //}

        public DefOralViewModel(BPMNDBEntities ctx, int codSelectedNod, TblEvtSrt srt)
            : base(ctx)
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            IsSelectActEnable = false;

            this._codSelectedNod = codSelectedNod;
            SlcSrcAndDstCommand = new DelegateCommand(ExecuteSlcSrcAndDstCommand, CanExecuteSlcSrcAndDstCommand);
            ActOfNodCommand = new DelegateCommand(ActOfNodExecute, CanSelectAct);

            SlcSrcAndDst = new SlcSrcAndDstViewModel();
            SlcSrcAndDst.IsDepOrgVisible = SlcSrcAndDst.IsOutOrgVisible = false;
            this._srt = srt;
        }


        #endregion

        #region ' Properties / Commands '

        public TblEvtSrt EvtSrt { get; set; }

        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

        /// <summary>
        /// SlcSrcAndDstCommand
        /// </summary>
        public ICommand SlcSrcAndDstCommand
        {
            get;
            set;
        }

        public ICommand ActOfNodCommand { get; set; }

        public TblAct SelectedAct { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SlcActOfNodViewModel SlcActOfNodVM { get; set; }

        public bool IsSelectActEnable { get; set; }

        public bool IsSelectResourceEnable
        {
            get
            {
                return _isSelectedSourceEnable;
            }
            set
            {
                _isSelectedSourceEnable = value;
                RaisePropertyChanged("IsSelectResourceEnable");
            }
        }

        public string PerformerName { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcSrcAndDst.Dispose();
        }

        #endregion

        #region ' Private Methods '


        /// <summary>
        /// ExecuteSlcSrcAndDstCommand
        /// </summary>
        private void ExecuteSlcSrcAndDstCommand()
        {
            SlcSrcAndDst.IsSelectionModeSingle = true;

            if (this.EvtSrt != null)
            {
                this.SlcSrcAndDst.IsDepOrgVisible = this.SlcSrcAndDst.IsOutOrgVisible = this.EvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt;
            }

            Util.ShowPopup(SlcSrcAndDst);

            if (SlcSrcAndDst.Result == PopupResult.OK && SlcSrcAndDst.SelectedItem != null)
            {
                if (this._srt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt)
                {
                    if (this._srt.TblAct.TblNod.FldCodNod == SlcSrcAndDst.SelectedItem.Nod.FldCodNod)
                    {
                        //امکان انتخاب فعالیت نامشخص وجود ندارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, this._srt.FldCodAct);
                    }
                    else
                    {
                        //امکان انتخاب فعالیت نامشخص وجود دارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, SlcSrcAndDst.SelectedItem.Nod.FldCodNod != _codSelectedNod, this._srt.FldCodAct);
                    }
                }
                else
                {
                    //امکان انتخاب فعالیت نامشخص وجود ندارد
                    SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, this._srt.FldCodAct);
                }

                PerformerName = SlcSrcAndDst.SelectedItem.Nod.Name;

                IsSelectActEnable = true;
                IsSelectResourceEnable = false;
                this.SelectedAct = null; //SlcActOfNodVM.SelectedAct;
                RaisePropertyChanged("SelectedAct");
                RaiseOKCanExecute();

                (ActOfNodCommand as DelegateCommand).RaiseCanExecuteChanged();

                RaisePropertyChanged("PerformerName", "IsSelectActEnable");
            }

        }

        /// <summary>
        /// can execute OK
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {
            return this.SelectedAct != null;
        }

        /// <summary>
        /// CanExecuteSlcSrcAndDstCommand
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteSlcSrcAndDstCommand()
        {
            return IsSelectResourceEnable;
        }


        private bool CanSelectAct()
        {
            return SlcSrcAndDst != null && SlcSrcAndDst.SelectedItem != null;
        }

        private void ActOfNodExecute()
        {
            Util.ShowPopup(SlcActOfNodVM);
            if (SlcActOfNodVM.Result == PopupResult.OK)
            {
                this.SelectedAct = SlcActOfNodVM.SelectedAct;
                RaisePropertyChanged("SelectedAct");
                RaiseOKCanExecute();
            }
        }


        #endregion
    }
}
