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
    public class DefIntViewModel : PopupViewModel
    {
        #region ' Fields '

        private bool isSelectSourceEnabel;
        private Model.TblObj tblObj;
        private TblAct _previousActivity;
        private int _codSelectedNod;
        TblEvtSrt _srt;

        #endregion

        #region ' Initialaizer '

        //public DefIntViewModel(DtlIntViewModel parent)
        //{
        //}

        public DefIntViewModel(BPMNDBEntities ctx, int codSelectedNod, TblEvtSrt srt)
            : base(ctx)
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            IsSelectActEnable = false;
            //this.Parent = parent;
            //InsertInputCommand = new DelegateCommand(ExecuteInsertInputCommand);
            //CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            this._codSelectedNod = codSelectedNod;
            SlcSrcAndDstCommand = new DelegateCommand(ExecuteSlcSrcAndDstCommand, CanExecuteSlcSrcAndDstCommand);
            ActOfNodCommand = new DelegateCommand(ActOfNodExecute);

            SlcSrcAndDst = new SlcSrcAndDstViewModel();
            this.TblObj = new Model.TblObj();
            TblObj.PropertyChanged += TblObj_PropertyChanged;
            this._srt = srt;
        }

        void TblObj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldNamObj")
            {
                RaiseOKCanExecute();
            }
        }

        #endregion

        #region ' Properties / Commands '

        public TblEvtSrt EvtSrt { get; set; }

        /// <summary>
        /// فعالیت تولید کننده ورودی
        /// </summary>
        public TblAct PreviousActivity
        {
            get { return _previousActivity; }
            set
            {
                _previousActivity = value;
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// can user select the source for the input
        /// </summary>
        public bool IsSelectSourceEnabel
        {
            get
            {
                return isSelectSourceEnabel;
            }

            set
            {
                isSelectSourceEnabel = value;
                RaisePropertyChanged("IsSelectSourceEnabel");
                RaisePropertyChanged("PerformerName");
                RaiseOKCanExecute();
                (this.SlcSrcAndDstCommand as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// performer name
        /// </summary>
        public string PerformerName
        {
            get
            {
                if (SlcActOfNodVM != null && SlcActOfNodVM.Nod != null)
                    return SlcActOfNodVM.Nod.FldNamNod;
                else
                    return string.Empty;
                /*if (this.PreviousActivity != null)
                {
                    return Model.PublicMethods.ActivityPerformerName_951(this.PreviousActivity.FldCodAct);
                }
                return string.Empty;*/
            }
        }

        /// <summary>
        /// TblObj
        /// </summary>
        public Model.TblObj TblObj
        {
            get { return tblObj; }
            set
            {
                tblObj = value;
            }
        }


        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

        //bool _showSlcAct;

        //public bool ShowSlcAct
        //{
        //    get { return _showSlcAct; }
        //    set
        //    {
        //        _showSlcAct = value;

        //        if (!_showSlcAct && SlcActOfNodVM != null && PreviousActivity != SlcActOfNodVM.SelectedAct)
        //        {
        //            PreviousActivity = SlcActOfNodVM.SelectedAct;
        //            RaisePropertyChanged("PreviousActivity");
        //            RaiseOKCanExecute();
        //        }
        //        RaisePropertyChanged("ShowSlcAct");
        //    }
        //}


        /// <summary>
        /// SlcSrcAndDstCommand
        /// </summary>
        public ICommand SlcSrcAndDstCommand
        {
            get;
            set;
        }

        public ICommand ActOfNodCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SlcActOfNodViewModel SlcActOfNodVM { get; set; }

        public bool IsSelectActEnable { get; set; }

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
        /// inserts input
        /// </summary>
        private void ExecuteInsertInputCommand()
        {
        }

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

            //IsSlcSrcAndDstOpen = true;

            //RaisePropertyChanged("IsSlcSrcAndDstOpen");

            Util.ShowPopup(SlcSrcAndDst);

            if (SlcSrcAndDst.Result == PopupResult.OK && SlcSrcAndDst.SelectedItem != null)
            {
                if (this._srt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt)
                {
                    if (this._srt.TblAct.TblNod.FldCodNod == SlcSrcAndDst.SelectedItem.Nod.FldCodNod)
                    {
                        //امکان انتخاب فعالیت نامشخص وجود ندارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, EvtSrt.FldCodAct, true);
                        PreviousActivity = SlcActOfNodVM.SelectedAct;
                        //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(a => !a.FldActUspf);
                    }
                    else
                    {
                        //امکان انتخاب فعالیت نامشخص وجود دارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, SlcSrcAndDst.SelectedItem.Nod.FldCodNod != _codSelectedNod, EvtSrt.FldCodAct, true);
                        PreviousActivity = SlcActOfNodVM.SelectedAct;
                        //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.Single(a => a.FldActUspf);
                    }
                }
                else
                {
                    //امکان انتخاب فعالیت نامشخص وجود ندارد
                    SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, EvtSrt.FldCodAct, true);
                    PreviousActivity = SlcActOfNodVM.SelectedAct;
                    //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(a => !a.FldActUspf);

                }
                /*
                if (PreviousActivity == null)
                {
                    Util.ShowMessageBox(71, SlcSrcAndDst.SelectedItem.NamTypEty);
                }
                */
                IsSelectActEnable = true;
                IsSelectSourceEnabel = false;
                RaisePropertyChanged("SlcActOfNodVM", "IsSelectActEnable", "PreviousActivity", "PerformerName");
            }

        }

        /// <summary>
        /// can execute OK
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {

            if (this.TblObj.HasErrors || string.IsNullOrWhiteSpace(PerformerName)
                || this.PreviousActivity == null)
            {
                return false;
            }
            return true;

            //return !this.TblObj.HasErrors;
            //return true;
        }

        /// <summary>
        /// CanExecuteSlcSrcAndDstCommand
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteSlcSrcAndDstCommand()
        {
            if (IsSelectSourceEnabel)
            {
                return true;
            }
            return false;
        }


        private void ActOfNodExecute()
        {

            Util.ShowPopup(SlcActOfNodVM);

            if (SlcActOfNodVM.Result == PopupResult.OK && PreviousActivity != SlcActOfNodVM.SelectedAct)
            {
                PreviousActivity = SlcActOfNodVM.SelectedAct;
                RaisePropertyChanged("PreviousActivity");
                RaiseOKCanExecute();
            }
        }


        #endregion


    }
}
