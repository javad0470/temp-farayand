using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class PosPstRolViewModel : UserControlViewModel
    {
        #region ' Fields '

        TblAct selectedAct;
        string pos;
        TblNod selectedNode;

        bool _isSpec;
        bool _isPbc;
        TblNod _nodSlcEed;
        string _lblPosRol;
        string _lblPst;
        bool _isChkCmbVisible;
        bool _cmbVisible;
        private TblRol _selectedRol;
        List<TblRol> _otherRols;
        private bool _publicVisible;
        TblPosPstOrg _posPstSlcEed;
        bool lastCanceled = false;

        #endregion

        #region ' Initialaizer '

        public PosPstRolViewModel(BPMNDBEntities context)
            : base(context)
        {
            PosPstSelectVM = new SlcPstPosViewModel(context);

            PosPstSelectVM.PropertyChanged += OrgSelectVM_PropertyChanged;

            RolSlcVM = new SlcRolViewModel(context, PublicMethods.CurrentUser.TblPsn);

            RolSlcVM.PropertyChanged += RolSlcVM_PropertyChanged;


            PosPstRolVM = new SlcPosPstRolViewModel() { PosPstSelectVM = PosPstSelectVM, RolSlcVM = RolSlcVM };

            //OtherRolVisibility = false;
        }

        #endregion

        #region ' Properties / Commands '

        public SlcPosPstRolViewModel PosPstRolVM { get; set; }
        //public bool CanChangeCurrentItem()
        //{
        //    if (SelectedActChanging != null)
        //    {
        //        SelectedActChanging(this, null);

        //        return CanChangeAct;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public void OpenPopup()
        {
            if (PosPstRolVM.PosPstSelectVM.TblPosPstOrgs.Count > 0  &&  PosPstRolVM.PosPstSelectVM.SelectedOrgTemp==null)
            {
                PosPstRolVM.PosPstSelectVM.SelectedOrgTemp = PosPstRolVM.PosPstSelectVM.TblPosPstOrgs[0];
            }
            if (PosPstRolVM.RolSlcVM.RolsCV.Count > 0  &&  PosPstRolVM.RolSlcVM.SelectedRolTmp==null)
            {
                PosPstRolVM.RolSlcVM.SelectedRolTmp = PosPstRolVM.RolSlcVM.RolsCV.GetItemAt(0) as TblRol;
            }
            Util.ShowPopup(PosPstRolVM);
            
        }

        // 23272
        public bool IsSpec
        {
            get { return _isSpec; }
            set
            {
                _isSpec = value;

                if (!_isSpec)
                {
                    //23274
                    PosPstSlcEed = NodSlcEed.EtyNod as TblPosPstOrg;
                    Rols = TblPosPstOrg.GetRolsOfPosPst_23262(bpmnEty, PosPstSelectVM.SelectedPosPst);

                    CmbVisible = true;

                    if (Rols.Count > 0)
                    {
                        NodSlcEed = Rols.First().Nod;
                    }
                }
            }
        }


        //23276
        public bool IsPbc
        {
            get { return _isPbc; }
            set
            {
                _isPbc = value;
                if (!_isPbc)
                {
                    NodSlcEed = PosPstSlcEed.Nod;
                    CmbVisible = false;
                }

                RaisePropertyChanged("IsPbc");
            }
        }


        //23276
        //public bool IsPbc
        //{
        //    get { return _isPbc; }
        //    set
        //    {
        //        _isPbc = value;
        //        if (!_isPbc)
        //        {
        //            NodSlcEed = PosPstSlcEed.Nod;
        //            CmbVisible = false;
        //        }

        //        RaisePropertyChanged("IsPbc");
        //    }
        //}


        //23280
        public TblRol SelectedRol
        {
            get { return _selectedRol; }
            set
            {
                _selectedRol = value;
                NodSlcEed = _selectedRol.Nod;
                RaisePropertyChanged("SelectedRol");
            }
        }


        public TblNod NodSlcEed
        {
            get { return _nodSlcEed; }
            set
            {
                _nodSlcEed = value;
                RaisePropertyChanged("NodSlcEed");
            }
        }




        public string LblPosRol
        {
            get { return _lblPosRol; }
            set
            {
                _lblPosRol = value;
                RaisePropertyChanged("LblPosRol");
                RaisePropertyChanged("CallOutVisible");
            }
        }


        public string LblPst
        {
            get { return _lblPst; }
            set
            {
                _lblPst = value;
                RaisePropertyChanged("LblPst");
                RaisePropertyChanged("CallOutVisible");
            }
        }



        public TblPosPstOrg PosPstSlcEed
        {
            get { return _posPstSlcEed; }
            set
            {
                _posPstSlcEed = value;
                RaisePropertyChanged("PosPstSlcEed");
            }
        }


        public List<TblRol> Rols
        {
            get { return _otherRols; }
            set
            {
                _otherRols = value;
                RaisePropertyChanged("Rols");
            }
        }

        public SlcRolViewModel RolSlcVM { get; set; }

        public SlcPstPosViewModel PosPstSelectVM { get; set; }

        public bool IsChkCmbVisible
        {
            get { return _isChkCmbVisible; }
            set
            {
                _isChkCmbVisible = value;
                RaisePropertyChanged("IsChkCmbVisible");
            }
        }

        public bool CmbVisible
        {
            get { return _cmbVisible; }
            set
            {
                _cmbVisible = value;

                RaisePropertyChanged("CmbVisible");
            }
        }


        public bool PublicVisible
        {
            get { return _publicVisible; }
            set
            {
                _publicVisible = value;
                RaisePropertyChanged("PublicVisible");
            }
        }

        private bool _specVisible;

        public bool SpecVisible
        {
            get { return _specVisible; }
            set
            {
                _specVisible = value;
                RaisePropertyChanged("SpecVisible");
            }
        }

        public TblAct SelectedAct
        {
            get
            {
                return selectedAct;
            }
            set
            {

                selectedAct = value;
                RaisePropertyChanged("SelectedAct");
            }
        }


        AllTypEty _typeEty;

        public AllTypEty TypeEty
        {
            get { return _typeEty; }
            set
            {
                _typeEty = value;

                RaisePropertyChanged("TypeEty");
            }
        }


        private bool _pstVisible;

        public bool PstVisible
        {
            get { return _pstVisible; }
            set
            {
                _pstVisible = value;
                RaisePropertyChanged("PstVisible");
               
            }
        }
        #endregion
        public bool CallOutVisible
        {
            get
            {
                return _lblPosRol==null && _lblPst==null;
            }
        }
        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool can()
        {
            return true;
        }


        //23271
        void OrgSelectVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedPosPst")
            {
                if (PosPstSelectVM.SelectedPosPst != null)
                {
                    //23263
                    NodSlcEed = PosPstSelectVM.SelectedPosPst.Nod;

                    //23265
                    if (NodSlcEed.CodTypEty != AllTypEty.Pst)
                    {
                        LblPosRol = NodSlcEed.Name;
                        TypeEty = AllTypEty.Pos;
                        PstVisible = false;
                    }
                    //23266
                    else
                    {
                        LblPosRol = PosPstSelectVM.SelectedPosPst.TblPosPstOrg2.Name;
                        LblPst = NodSlcEed.Name;
                        TypeEty = PosPstSelectVM.SelectedPosPst.TblPosPstOrg2.TypEty;
                        PstVisible = true;
                    }


                    CmbVisible = false;
                    PublicVisible = true;
                    SpecVisible = true;


                    //23269
                    _isSpec = true;

                    //23288
                    PosPstSlcEed = NodSlcEed.EtyNod as TblPosPstOrg;

                    RaisePropertyChanged("IsSpec");

                }
            }
        }

        void RolSlcVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedRol")
            {
                NodSlcEed = RolSlcVM.SelectedRol.Nod;

                //23283
                if (NodSlcEed.CodTypEty == AllTypEty.Rol)
                {
                    //23284
                    LblPosRol = NodSlcEed.Name;
                    TypeEty = AllTypEty.Rol;
                    CmbVisible = false;
                    PublicVisible = false;
                    SpecVisible = false;
                    PstVisible = false;
                }

            }
        }

        //private void CurrentItemChanging(object sender, System.ComponentModel.CurrentChangingEventArgs e)
        //{
        //    if (e.IsCancelable)
        //    {
        //        if (SelectedActChanging != null)
        //        {
        //            SelectedActChanging(this, null);

        //            lastCanceled = e.Cancel = !CanChangeAct;

        //            CanChangeAct = true;
        //        }
        //        else
        //        {
        //            lastCanceled = false;
        //        }
        //    }
        //    else
        //    {
        //        lastCanceled = false;
        //    }
        //}


        #endregion

        #region ' Events '


        #endregion

    }

}
