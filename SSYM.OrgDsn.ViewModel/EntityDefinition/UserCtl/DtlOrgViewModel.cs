using System.Data;
using System.Data.Objects;
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
    public class DtlOrgViewModel : BaseViewModel
    {
        #region ' Fields '

        TblOrg _selectedOrg;

        BPMNDBEntities context;

        TblAgntNod _selectedAgnt;

        TblPsn selectedPsnIsdOrg;

        TblOrg currentOrg = null;

        bool isSlcPsnOpen;

        ObservableCollection<TblAgntNod> _agntsOfOrg;

        #endregion

        #region ' Initialaizer '

        public DtlOrgViewModel(BPMNDBEntities context)
        {
            this.context = context;

            DeleteAgntNodCommand = new DelegateCommand<TblAgntNod>(ExecuteDeleteAgntNodCommand);

            OpenAddAgntNodCommand = new DelegateCommand(ExecuteOpenAddAgntNodCommand);

            PsnIsdOrgVM = new PsnIsdOrgViewModel();

            //PsnIsdOrgVM.OnOKExecute += PsnIsdOrgVM_OnOKExecute;

            //PsnIsdOrgVM.PropertyChanged += PsnIsdOrgVM_PropertyChanged;

            currentOrg = context.TblOrgs.Single(o => o.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg);

            AgntChanged = false;
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// نمایش اشخاص درون سازمانی
        /// </summary>
        public PsnIsdOrgViewModel PsnIsdOrgVM { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public ICommand DeleteAgntNodCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TblOrg SelectedOrg
        {
            get { return _selectedOrg; }
            set
            {
                _selectedOrg = value;
                _agntsOfOrg = null;
                RaisePropertyChanged("SelectedOrg", "AgntsOfOrg");

                //, "Acs_ViewAgntOrg2"
                //    , "Acs_AddAgntOrg2", "Acs_DelAgntOrg2", "Acs_EditAgntOrg2");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblAgntNod> AgntsOfOrg
        {
            get
            {
                if (SelectedOrg == null)
                {
                    return null;
                }
                if (!SelectedOrg.IsDepOrg)
                {
                    return null;
                }
                if (_agntsOfOrg == null)
                {
                    _agntsOfOrg = new ObservableCollection<TblAgntNod>(SelectedOrg.Nod.TblAgntNods);
                    _agntsOfOrg.ToList().ForEach(a => a.PropertyChanged += agnt_ProperyChanged);
                    _agntsOfOrg.CollectionChanged += _agntsOfOrg_CollectionChanged;
                }
                return _agntsOfOrg;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblLvlAc> LvlAcs
        {
            get { return new ObservableCollection<TblLvlAc>(this.context.TblLvlAcs); }

        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenAddAgntNodCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSlcPsnOpen
        {
            get { return isSlcPsnOpen; }
            set
            {
                isSlcPsnOpen = value;

                RaisePropertyChanged("IsSlcPsnOpen");
            }
        }

        /// <summary>
        /// نشان میدهد که آیا از ابتدای لود شدن این فرم نمایندگان تغییر کرده اند یا خیر؟
        /// </summary>
        public bool AgntChanged { get; set; }

        #region ' Access '

        /// <summary>
        /// 
        /// </summary>
        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }


        bool? _canEdit;

        public bool? CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                RaisePropertyChanged("CanEdit", "Acs_EditOrgOut", "Acs_EditOrgSub", "Acs_EditOrg");
            }
        }

        public TblAgntNod SelectedAgnt
        {
            get { return _selectedAgnt; }
            set
            {
                _selectedAgnt = value;
                RaisePropertyChanged("SelectedAgnt");
                //            , "Acs_ViewAgntOrg2"
                //, "Acs_AddAgntOrg2", "Acs_DelAgntOrg2", "Acs_EditAgntOrg2");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditOrgSub
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["EditOrgSub"];
            }
        }


        bool? _acs_EditOrgOut;
        public bool Acs_EditOrgOut
        {
            get
            {
                if (!_acs_EditOrgOut.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Edit", namTypEtyMjr: "OrgOut", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_EditOrgOut = this.Usr.AcsUsr["EditOrgOut"] && CanEdit.HasValue && CanEdit.Value;
                }

                return _acs_EditOrgOut.Value;
            }
        }


        public bool Acs_EditOrg
        {
            get
            {
                if (CanEdit.HasValue)
                {
                    return Acs_EditOrgOut;
                }
                else
                {
                    return Acs_EditOrgSub;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_ViewAgntOrg2
        //{
        //    get
        //    {
        //        if (SelectedOrg == null)
        //        {
        //            return false;
        //        }
        //        if (!SelectedOrg.IsDepOrg)
        //        {
        //            return false;
        //        }
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["ViewAgntOrg2"] &&
        //            (TblOrg.IsOrgAnsestorOfThisOrg(currentOrg, SelectedOrg) || currentOrg.FldCodOrg == SelectedOrg.FldCodOrg);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        //public bool Acs_AddAgntOrg2
        //{
        //    get
        //    {
        //        if (!SelectedOrg.IsDepOrg)
        //        {
        //            return false;
        //        }
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["AddAgntOrg2"];
        //    }
        //}

        //bool? acs_DelAgntOrg2;
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool Acs_DelAgntOrg2
        //{
        //    get
        //    {
        //        if (!SelectedOrg.IsDepOrg)
        //        {
        //            return false;
        //        }
        //        if (!acs_DelAgntOrg2.HasValue)
        //        {
        //            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //            acs_DelAgntOrg2 = this.Usr.AcsUsr["DelAgntOrg2"];
        //        }

        //        return acs_DelAgntOrg2.Value;
        //    }
        //}

        //public bool Acs_EditAgntOrg2
        //{
        //    get
        //    {
        //        if (!SelectedOrg.IsDepOrg)
        //        {
        //            return false;
        //        }
        //        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

        //        return this.Usr.AcsUsr["EditAgntOrg2"] && !isAdmin();
        //    }
        //}

        #endregion

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شخص جاری را نماینده سازمان داده شده با بالاترین سطح دسترسی میکند
        /// </summary>
        /// <param name="org"></param>
        internal void AddCurrentPsnAsAgntOfOrg(TblOrg org)
        {
            PublicMethods.AddAgntOfNodForPsn_22157(context, context.TblPsns.Single(p => p.FldCodPsn == PublicMethods.CurrentUser.FldCodPsn), org.Nod);

            SelectedAgnt = this.SelectedOrg.Nod.TblAgntNods.LastOrDefault();

            RaisePropertyChanged("AgntsOfOrg");
        }
        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDeleteAgntNodCommand(TblAgntNod obj)
        {
            if (this.SelectedOrg != null && obj != null)
            {
                if (SelectedAgnt != null && SelectedAgnt.IsAdmin)
                {
                    Util.ShowNotification(17, "حذف این شخص از لیست نمایندگان سازمان انتخاب شده");
                    return;
                }

                if (Util.ShowMessageBox(2, "این نماینده") == System.Windows.MessageBoxResult.Yes)
                {
                    PublicMethods.DelAgnt(context, obj, obj.TblPsn.FldCodPsn);

                    AgntsOfOrg.Remove(obj);

                    //RaisePropertyChanged("AgntsOfOrg");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenAddAgntNodCommand()
        {
            if (this.SelectedOrg != null)
            {
                DetectPsnIsdOrg();

                foreach (TblAgntNod item in this.SelectedOrg.Nod.TblAgntNods)
                {
                    PsnIsdOrgVM.PsnIsdOrg.Remove(item.TblPsn);
                }

                PsnIsdOrgVM.SelectedPsnIsdOrg = PsnIsdOrgVM.PsnIsdOrg.FirstOrDefault();

                Util.ShowPopup(this.PsnIsdOrgVM);

                while (this.PsnIsdOrgVM.Result == PopupResult.OK && this.PsnIsdOrgVM.SelectedPsnIsdOrg != null && this.SelectedOrg.Nod.TblAgntNods.Where(m => m.FldCodPsn == this.PsnIsdOrgVM.SelectedPsnIsdOrg.FldCodPsn).Count() == 0)
                {
                    var addedAgnt = PublicMethods.AddAgntOfNodForPsn_22157(context, this.PsnIsdOrgVM.SelectedPsnIsdOrg, SelectedOrg.Nod);
                    //SelectedAgnt = addedAgnt;// this.SelectedOrg.Nod.TblAgntNods.LastOrDefault();
                    AgntsOfOrg.Add(addedAgnt);
                    SelectedAgnt = addedAgnt;
                    this.PsnIsdOrgVM.RemoveSelectedPsnIsdOrg(this.PsnIsdOrgVM.SelectedPsnIsdOrg);
                }
                //SelectedAgnt=AgntsOfOrg.LastOrDefault();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void DetectPsnIsdOrg()
        {
            this.PsnIsdOrgVM.PsnIsdOrg = new ObservableCollection<TblPsn>(PublicMethods.GetPsnInOrg(this.context));
        }

        //private bool isAdmin()
        //{

        //    if (SelectedAgnt != null)
        //    {
        //        return SelectedAgnt.TblPsn.TblUsrs.Any(u => u.FldNamUsr.Trim() == "admin");
        //    }
        //    return false;
        //}

        private void agnt_ProperyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AgntChanged = true;
        }

        void _agntsOfOrg_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AgntChanged = true;
        }

        #endregion

        #region ' Events '

        #endregion
    }
}
