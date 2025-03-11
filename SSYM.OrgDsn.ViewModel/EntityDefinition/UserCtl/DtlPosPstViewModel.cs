using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
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
using Microsoft.Practices.Prism.Commands;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{

    public class DtlPosPstViewModel : BaseViewModel
    {
        #region ' Fields '

        Admin.DefLvlAcsViewModel defLvlAcs;
        BPMNDBEntities bpmnEty;
        TblPosPstOrg selectedPosPst;
        TblOrg selectedOrg;
        TblAgntNod _selectedAgnt;
        ObservableCollection<TblItmFixSfw> _dmnAcs;

        #endregion

        #region ' Initialaizer '

        public DtlPosPstViewModel(BPMNDBEntities context)
        {
            bpmnEty = context;
            this.DefLvlAcs = new Admin.DefLvlAcsViewModel();
            PsnIsdOrgVM = new PsnIsdOrgViewModel();
            //PsnIsdOrgVM.OnOKExecute += PsnIsdOrgVM_OnOKExecute;
            //PsnIsdOrgVM.PropertyChanged += PsnIsdOrgVM_PropertyChanged;
            OpenAddAgntNodCommand = new DelegateCommand(ExecuteOpenAddAgntNodCommand);
            DeleteAgntNodCommand = new DelegateCommand<TblAgntNod>(ExecuteDeleteAgntNodCommand);
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
        public TblPosPstOrg SelectedPosPst
        {
            get { return selectedPosPst; }
            set
            {
                if (value == null)
                {
                    return;
                }
                selectedPosPst = value;
                selectedPosPst.Nod.TblAgntNods.ToList().ForEach(a =>
                    {
                        a.PropertyChanged -= a_PropertyChanged;
                        a.PropertyChanged += a_PropertyChanged;
                    });
                RaisePropertyChanged("SelectedPosPst");
                //,
                //                        "Acs_EditPosPst",
                //                        "Acs_ViewAgntPosPstOrg2",
                //                        "Acs_AddAgntPosPstOrg2",
                //                        "Acs_DelAgntPosPstOrg2",
                //                        "Acs_EditAgntPosPst2");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TblOrg SelectedOrg
        {
            get { return selectedOrg; }
            set
            {
                selectedOrg = value;
                RaisePropertyChanged("SelectedOrg", "Acs_EditPosPst");
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //public TblPsn SelectedPsnIsdOrg
        //{
        //    get { return selectedPsnIsdOrg; }
        //    set
        //    {
        //        selectedPsnIsdOrg = value;

        //        RaisePropertyChanged("SelectedPsnIsdOrg", "Acs_EditPosPst");
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public bool IsSlcPsnOpen
        //{
        //    get { return isSlcPsnOpen; }
        //    set
        //    {
        //        isSlcPsnOpen = value;

        //        RaisePropertyChanged("IsSlcPsnOpen");
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public Admin.DefLvlAcsViewModel DefLvlAcs
        {
            get { return defLvlAcs; }
            set { defLvlAcs = value; }
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

        public TblAgntNod SelectedAgnt
        {
            get { return _selectedAgnt; }
            set
            {
                if (_selectedAgnt != value)
                {
                    _selectedAgnt = value;
                    RaisePropertyChanged("SelectedAgnt",
                        "Acs_EditPosPst",
                        "Acs_ViewAgntPosPstOrg2",
                        "Acs_AddAgntPosPstOrg2",
                        "Acs_DelAgntPosPstOrg2",
                        "Acs_EditAgntPosPst2");
                }

                //if (isAdmin())
                //{
                //    Util.ShowNotification(77, SelectedAgnt.TblPsn.Name);
                //}
            }
        }

        #region ' Access '

        /// <summary>
        /// به منظور بهینه سازی سرعت دسترسی ها به کلاس 
        /// TblAgntNod 
        /// و 
        /// TblPosPstOrg
        /// منتقل شد.
        /// </summary>

        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_EditPosPst
        //{
        //    get
        //    {
        //        if (this.SelectedOrg != null)
        //        {
        //            if (this.SelectedPosPst != null)
        //            {
        //                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, this.SelectedPosPst.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.PosPstOfOrgAndCurrentOrg, null, "PosPst");
        //                return PublicMethods.CurrentUser.AcsUsr["EditPosPst"] || this.SelectedPosPst.NewlyAdded;

        //                //|| PublicMethods.IsPsnAgntOfOrg(PublicMethods.CurrentUser.TblPsn, PublicMethods.CurrentUser.FldCodOrg);
        //            }

        //            //else
        //            //{
        //            //    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "PosPst", AllTypEty.Pos, new TblNod[] { this.SelectedOrg.Nod });
        //            //}

        //            //return PublicMethods.CurrentUser.AcsUsr["EditPosPst"];
        //        }

        //        return false;

        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_ViewAgntPosPstOrg2
        //{
        //    get
        //    {
        //        if (this.SelectedPosPst != null)
        //        {
        //            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "View", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //            return this.Usr.AcsUsr["ViewAgntPosPstOrg2"];
        //        }

        //        return false;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_AddAgntPosPstOrg2
        //{
        //    get
        //    {
        //        if (this.SelectedPosPst != null)
        //        {
        //            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Add", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //            return this.Usr.AcsUsr["AddAgntPosPstOrg2"];
        //        }

        //        return false;
        //    }
        //}



        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_DelAgntPosPstOrg2
        //{
        //    get
        //    {
        //        if (this.SelectedPosPst != null)
        //        {
        //            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Del", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //            acs_DelAgntPosPstOrg2 = this.Usr.AcsUsr["DelAgntPosPstOrg2"];

        //            return acs_DelAgntPosPstOrg2.Value;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //}

        //public bool Acs_EditAgntPosPst2
        //{
        //    get
        //    {
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Edit", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["EditAgntPosPstOrg2"] && !isAdmin();
        //    }
        //}


        //Acs_DelAgntPosPstOrg2

        #endregion

        #endregion

        #region ' Public Methods '

        internal void AddCurrentPsnAsAgntOfPosPst(TblPosPstOrg pos)
        {
            SelectedAgnt = PublicMethods.AddAgntOfNodForPsn_22157(bpmnEty, bpmnEty.TblPsns.Single(p => p.FldCodPsn == PublicMethods.CurrentUser.FldCodPsn), pos.Nod);
        }
        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenAddAgntNodCommand()
        {
            if (this.SelectedPosPst != null)
            {
                DetectPsnIsdOrg();

                foreach (TblAgntNod item in this.SelectedPosPst.Nod.TblAgntNods)
                {
                    PsnIsdOrgVM.PsnIsdOrg.Remove(item.TblPsn);
                }
                PsnIsdOrgVM.SelectedPsnIsdOrg = PsnIsdOrgVM.PsnIsdOrg.FirstOrDefault();

                Util.ShowPopup(this.PsnIsdOrgVM);
                var temp = SelectedAgnt;
                while (this.PsnIsdOrgVM.Result == PopupResult.OK && this.PsnIsdOrgVM.SelectedPsnIsdOrg != null && this.SelectedPosPst.Nod.TblAgntNods.Where(m => m.FldCodPsn == this.PsnIsdOrgVM.SelectedPsnIsdOrg.FldCodPsn).Count() == 0)
                {
                    temp = PublicMethods.AddAgntOfNodForPsn_22157(bpmnEty, this.PsnIsdOrgVM.SelectedPsnIsdOrg, this.SelectedPosPst.Nod);
                    AgntChanged = true;
                    this.PsnIsdOrgVM.RemoveSelectedPsnIsdOrg(this.PsnIsdOrgVM.SelectedPsnIsdOrg);
                }
                SelectedAgnt = temp;
                
                //this.IsSlcPsnOpen = true;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void DetectPsnIsdOrg()
        {
            TblOrg org = this.bpmnEty.TblOrgs.Single(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);
            this.PsnIsdOrgVM.PsnIsdOrg = new ObservableCollection<TblPsn>(org.PsnIsdOrg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDeleteAgntNodCommand(TblAgntNod obj)
        {
            if (obj == null)
            {
                return;
            }

            if (this.SelectedPosPst != null)
            {

                if (SelectedAgnt != null && SelectedAgnt.IsAdmin)
                {
                    Util.ShowNotification(17, "حذف این شخص از لیست نمایندگان جایگاه سازمانی انتخاب شده");
                    return;
                }

                if (Util.ShowMessageBox(2, "این نماینده") == System.Windows.MessageBoxResult.Yes)
                {
                    PublicMethods.DelAgnt(bpmnEty, obj, obj.TblPsn.FldCodPsn);
                    AgntChanged = true;
                }
            }
        }

        ///// <summary>
        ///// وقتی که یک شخص انتخاب میشود
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void PsnIsdOrgVM_OnOKExecute(object sender, EventArgs e)
        //{
        //    if (this.SelectedPsnIsdOrg != null && this.SelectedPosPst.Nod.TblAgntNods.Where(m => m.FldCodPsn == this.SelectedPsnIsdOrg.FldCodPsn).Count() == 0)
        //    {
        //        //TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = this.LvlAcs.First().FldCod };

        //        //this.SelectedPosPst.Nod.TblAgntNods.Add(agnt);

        //        //agnt.TblPsn = SelectedPsnIsdOrg;

        //        PublicMethods.AddAgntOfNodForPsn_22157(bpmnEty, this.SelectedPsnIsdOrg, this.SelectedPosPst.Nod);

        //        //PublicMethods.AddAgntOfOrgForPsn_22157(bpmnEty, this.SelectedPsnIsdOrg, this.SelectedOrg);
        //    }
        //}

        //void PsnIsdOrgVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "SelectedPsnIsdOrg")
        //    {
        //        this.SelectedPsnIsdOrg = PsnIsdOrgVM.SelectedPsnIsdOrg;
        //    }
        //}

        //private bool isAdmin()
        //{

        //    if (SelectedAgnt != null)
        //    {
        //        return SelectedAgnt.TblPsn.TblUsrs.Any(u => u.FldNamUsr.Trim() == "admin");
        //    }
        //    return false;
        //}

        void a_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AgntChanged = true;
        }


        #endregion

        #region ' Events '

        #endregion

    }
}
