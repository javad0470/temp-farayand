using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DtlRolIsdOrgViewModel : BaseViewModel
    {
        #region ' Fields '

        TblPsn selectedPsnIsdOrg;
        //Admin.DefLvlAcsViewModel defLvlAcs;
        ObservableCollection<TblPsn> psnIsdOrg;
        bool isSlcPsnOpen;
        BPMNDBEntities bpmnEty;
        TblRol selectedRol;
        ObservableCollection<TblItmFixSfw> _dmnAcs;
        TblAgntNod _selectedAgnt;
        bool _isIsd;


        #endregion

        #region ' Initialaizer '

        public DtlRolIsdOrgViewModel(BPMNDBEntities context)
        {
            bpmnEty = context;
            //this.DefLvlAcs = new Admin.DefLvlAcsViewModel();
            OpenAddAgntNodCommand = new DelegateCommand(ExecuteOpenAddAgntNodCommand);
            DeleteAgntNodCommand = new DelegateCommand<TblAgntNod>(ExecuteDeleteAgntNodCommand);

            PsnIsdOrgVM = new PsnIsdOrgViewModel();
            //PsnIsdOrgVM.OnOKExecute += PsnIsdOrgVM_OnOKExecute;
            //PsnIsdOrgVM.PropertyChanged += PsnIsdOrgVM_PropertyChanged;
            AgntChanged = false;
        }


        #endregion

        #region ' Properties / Commands '

        public bool AgntChanged { get; set; }

        /// <summary>
        /// نمایش اشخاص درون سازمانی
        /// </summary>
        public PsnIsdOrgViewModel PsnIsdOrgVM { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenAddAgntNodCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DeleteAgntNodCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TblRol SelectedRol
        {
            get { return selectedRol; }
            set
            {
                if (value == null)
                {
                    return;
                }
                selectedRol = value;
                selectedRol.Nod.TblAgntNods.ToList().ForEach(a =>
                    {
                        a.PropertyChanged -= a_PropertyChanged;
                        a.PropertyChanged += a_PropertyChanged;
                    });
                RaisePropertyChanged("SelectedRol", "Acs_EditRol");
            }
        }


        public TblAgntNod SelectedAgnt
        {
            get { return _selectedAgnt; }
            set
            {
                if (_selectedAgnt != value)
                {
                    _selectedAgnt = value;
                    RaisePropertyChanged("SelectedAgnt");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblLvlAc> LvlAcs
        {
            get
            {
                return new ObservableCollection<TblLvlAc>(this.bpmnEty.TblLvlAcs);
            }
        }

        /// <summary>
        /// دامنه دسترسی
        /// </summary>
        public ObservableCollection<TblItmFixSfw> DmnAcs
        {
            get
            {
                if (_dmnAcs == null)
                {
                    return _dmnAcs = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == 36));
                }

                return _dmnAcs;
            }
        }


        #region ' Access '

        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }

        //public bool Acs_EditRol
        //{
        //    get
        //    {
        //        if (SelectedRol == null)
        //        {
        //            return false;
        //        }
        //        if (PublicMethods.CurrentUser.AcsUsr.EditRolAllowedByOrg)
        //        {
        //            return true;
        //        }

        //        if (IsIsd)
        //        {
        //            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(SelectedRol.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
        //            return PublicMethods.CurrentUser.AcsUsr["EditRolIsdOrg"] || SelectedRol.NewlyAdded;
        //        }
        //        else
        //        {
        //            return Acs_EditRolOsd;
        //        }

        //    }
        //}


        bool? _acs_EditRolOsd;
        public bool Acs_EditRolOsd
        {
            get
            {
                if (!_acs_EditRolOsd.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "RolOsdOrg", AllTypEty.Rol, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_EditRolOsd = PublicMethods.CurrentUser.AcsUsr["EditRolOsdOrg"];
                }
                return _acs_EditRolOsd.Value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_ViewAgntRol2
        //{
        //    get
        //    {
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["ViewAgntRol2"] || SelectedRol.NewlyAdded;
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_AddAgntRol2
        //{
        //    get
        //    {
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["AddAgntRol2"] || SelectedRol.NewlyAdded;
        //    }
        //}


        //bool? acs_DelAgntRol2;
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool Acs_DelAgntRol2
        //{
        //    get
        //    {
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["DelAgntRol2"] || SelectedRol.NewlyAdded;
        //    }
        //}

        //public bool Acs_EditAgntRol2
        //{
        //    get
        //    {
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["EditAgntRol2"] || SelectedRol.NewlyAdded;
        //    }
        //}

        /// <summary>
        /// آیا جزئیات نقش درون سازمان است یا خیر
        /// </summary>
        public bool IsIsd
        {
            get { return _isIsd; }
            set { _isIsd = value; }
        }

        #endregion

        #endregion

        #region ' Public Methods '

        internal TblAgntNod AddCurrentPsnAsAgntOfRol(TblRol rol)
        {
            TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod };

            rol.Nod.TblAgntNods.Add(agnt);

            agnt.TblPsn = bpmnEty.TblPsns.Single(p => p.FldCodPsn == PublicMethods.CurrentUser.FldCodPsn);

            return agnt;
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenAddAgntNodCommand()
        {
            if (this.SelectedRol != null)
            {
                DetectPsnIsdOrg();

                foreach (TblAgntNod item in this.SelectedRol.Nod.TblAgntNods)
                {
                    this.PsnIsdOrgVM.PsnIsdOrg.Remove(item.TblPsn);
                }

                PsnIsdOrgVM.SelectedPsnIsdOrg = PsnIsdOrgVM.PsnIsdOrg.FirstOrDefault();

                Util.ShowPopup(this.PsnIsdOrgVM);
                var temp = SelectedAgnt;

                while (this.PsnIsdOrgVM.Result == PopupResult.OK && this.PsnIsdOrgVM.SelectedPsnIsdOrg != null && this.SelectedRol.Nod.TblAgntNods.Where(m => m.FldCodPsn == this.PsnIsdOrgVM.SelectedPsnIsdOrg.FldCodPsn).Count() == 0)
                {
                    TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod };

                    this.SelectedRol.Nod.TblAgntNods.Add(agnt);

                    agnt.TblPsn = this.PsnIsdOrgVM.SelectedPsnIsdOrg;

                    temp = agnt;

                    AgntChanged = true;

                    this.PsnIsdOrgVM.RemoveSelectedPsnIsdOrg(this.PsnIsdOrgVM.SelectedPsnIsdOrg);
                }
                SelectedAgnt = temp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void DetectPsnIsdOrg()
        {
            TblOrg org = this.bpmnEty.TblOrgs.Single(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);
            this.PsnIsdOrgVM.PsnIsdOrg = new ObservableCollection<TblPsn>(PublicMethods.GetPsnInOrg(this.bpmnEty));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDeleteAgntNodCommand(TblAgntNod obj)
        {
            if (this.SelectedRol != null && obj != null)
            {
                if (Util.ShowMessageBox(2, "این نماینده") == System.Windows.MessageBoxResult.Yes)
                {
                    PublicMethods.DelAgnt(bpmnEty, obj, obj.TblPsn.FldCodPsn);
                    AgntChanged = true;
                }
            }
        }

        void a_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AgntChanged = true;
        }

        #endregion

        #region ' Events '


        #endregion

    }
}
