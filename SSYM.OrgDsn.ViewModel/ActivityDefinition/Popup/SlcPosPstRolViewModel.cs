using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcPosPstRolViewModel : PopupViewModel
    {
        #region ' Fields '
        bool _isRolVisible;

        #endregion

        #region ' Initialaizer '

        public SlcPosPstRolViewModel()
        {
            PosPostSelected = true;
            this.IsRolVisible = true;
        }

        #endregion

        #region ' Properties / Commands '

        SlcPstPosViewModel _posPstSelectVM;
        public SlcPstPosViewModel PosPstSelectVM
        {
            get { return _posPstSelectVM; }
            set
            {
                _posPstSelectVM = value;
                _posPstSelectVM.ResultChanged += _posPstSelectVM_ResultChanged;
                RaisePropertyChanged("PosPstSelectVM");
            }
        }


        SlcRolViewModel _rolSlcVM;
        public SlcRolViewModel RolSlcVM
        {
            get { return _rolSlcVM; }
            set
            {
                _rolSlcVM = value;
                _rolSlcVM.ResultChanged += _rolSlcVM_ResultChanged;
                RaisePropertyChanged("RolSlcVM");
            }
        }

        public bool IsRolVisible
        {
            get { return _isRolVisible; }
            set
            {
                _isRolVisible = value;
                RaisePropertyChanged("IsRolVisible");
            }
        }

        public bool PosPostSelected { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        protected override void OKExecute()
        {
            base.OKExecute();
            if (PosPostSelected)
            {
                PosPstSelectVM.OK();
            }
            else
            {
                RolSlcVM.OK();
            }

        }

        void _posPstSelectVM_ResultChanged(PopupViewModel sender, PopupResult newResult)
        {
            this.Result = PosPstSelectVM.Result;
        }

        void _rolSlcVM_ResultChanged(PopupViewModel sender, PopupResult newResult)
        {
            this.Result = RolSlcVM.Result;
        }

        #endregion

        #region ' Events '

        #endregion

    }
}
