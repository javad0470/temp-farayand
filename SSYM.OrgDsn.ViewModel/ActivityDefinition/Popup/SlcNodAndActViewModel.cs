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
    public class SlcNodAndActViewModel : PopupViewModel
    {
        #region ' Fields '

        private int? _codSelectedNod;
        private int? _codAct;
        string _lblNod;
        string _lblAct;
        string _lblObj;
        bool _actUspfEnabled;
        #endregion

        #region ' Initialaizer '

        //public DefIntViewModel(DtlIntViewModel parent)
        //{
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="codSelectedNod">نودی که میخواهیم چک کنیم اگر آن نود انتخاب شده است، نتواند فعالیت نامشخص آن را انتخاب کند</param>
        /// <param name="codAct">فعالیتی که میخواهیم در لیست فعالیت های آن نود نمایش داده نشود</param>
        /// <param name="actUspfEnabled">آیا اجازه انتخاب فعالیت نامشخص داده شده است؟</param>
        public SlcNodAndActViewModel(BPMNDBEntities ctx, int? codSelectedNod = null, int? codAct = null, bool actUspfEnabled = true)
            : base(ctx)
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            _actUspfEnabled = actUspfEnabled;

            this._codAct = codAct;

            IsSelectActEnable = false;

            this._codSelectedNod = codSelectedNod;

            SlcSrcAndDstCommand = new DelegateCommand(ExecuteSlcSrcAndDstCommand, CanExecuteSlcSrcAndDstCommand);

            ActOfNodCommand = new DelegateCommand(ActOfNodExecute, CanSelectAct);

            SlcSrcAndDst = new SlcSrcAndDstViewModel();


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

        public string PerformerName { get; set; }


        public string LblNod
        {
            get { return _lblNod; }
            set
            {
                _lblNod = value;
                RaisePropertyChanged("LblNod");
            }
        }

        public string LblAct
        {
            get { return _lblAct; }
            set
            {
                _lblAct = value;
                RaisePropertyChanged("LblAct");
            }
        }

        public string LblObj
        {
            get { return _lblObj; }
            set
            {
                _lblObj = value;
                RaisePropertyChanged("LblObj");
            }
        }

        public bool IsDepOrgVisible
        {
            get
            {
                return SlcSrcAndDst.IsDepOrgVisible;
            }
            set
            {
                SlcSrcAndDst.IsDepOrgVisible = value;
            }
        }

        public bool IsOutOrgVisible
        {
            get
            {
                return SlcSrcAndDst.IsOutOrgVisible;

            }
            set
            {
                SlcSrcAndDst.IsOutOrgVisible = value;
            }
        }

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

            Util.ShowPopup(SlcSrcAndDst);

            if (SlcSrcAndDst.Result == PopupResult.OK && SlcSrcAndDst.SelectedItem != null)
            {
                //امکان انتخاب فعالیت نامشخص در صورتی که جایگاه انتخب شده جایگاه جاری باشد وجود ندارد
                SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, _codSelectedNod != SlcSrcAndDst.SelectedItem.Nod.FldCodNod
                    && _actUspfEnabled // آیا اجازه انتخاب فعالیت نامشخص از بیرون داده شده است؟
                    , _codAct);

                //this.SelectedAct = SlcActOfNodVM.SelectedAct;
                //RaisePropertyChanged("SelectedAct");
                RaiseOKCanExecute();

                PerformerName = SlcSrcAndDst.SelectedItem.Nod.Name;

                IsSelectActEnable = true;

                (ActOfNodCommand as DelegateCommand).RaiseCanExecuteChanged();

                RaisePropertyChanged("PerformerName", "IsSelectActEnable", "SelectedAct");
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
            return true;
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
