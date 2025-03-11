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
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using System.Windows.Data;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    //public class OrgComparer : IEqualityComparer<TblOrg>
    //{

    //    public bool Equals(TblOrg x, TblOrg y)
    //    {
    //        return x.FldCodOrg == y.FldCodOrg;
    //    }

    //    public int GetHashCode(TblOrg obj)
    //    {
    //        return obj.GetHashCode();
    //    }
    //}

    public class DefOrg : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities context;
        string txtForSchOsdOrg;
        TblOrg _selectedNonDepOrg;
        string _txtForSchNonDepOrg;
        TblOrg _selectedOsdOrg;


        #endregion

        #region ' Initialaizer '

        public DefOrg()
        {
            this.context = new BPMNDBEntities();
            //this.SaveChangesCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(ExecuteSaveChangesCommand);
            Model.TblOrg tbl = context.TblOrgs.Single(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg);
            this.OrgChartVM = new OrgChartViewModel(context);
            this.OrgChartVM.OrgAdded += OrgChartVM_OrgAdded;
            OrgChartVM.PropertyChanged += OrgChartVM_PropertyChanged;
            this.DtlOrgVM = new DtlOrgViewModel(context);
            this.DtlOrgVMOutside = new DtlOrgViewModel(context);



            this.OrgChartVM.CanUsrEditChart = true;

            List<TblOrg> orgs = PublicMethods.DetectOrgNotSubOrgOfOrg_2073(context, this.context.TblOrgs.SingleOrDefault(E => E.FldCodOrg == Base.UserManager.CurrentUser.FldCodOrg));

            // سازمانهای بیرونی
            this.NonDepOrgLst = new ListCollectionView(context.TblOrgs.Where(o =>o.FldCodOrg!=1 && o.TblOrg1.Count == 0 && o.TblOrg2 == null).ToList());
            NonDepOrgLst.Filter = srchNonDepOrgs;

            // هم سود
            // سازمان هایی که به سازمان جاری وابسته نیستند و بیرونی نیز نیستند
            OsdOrgLst = new ListCollectionView(orgs.Where(o => o.TblOrg1.Count > 0 || o.TblOrg2 != null).ToList());
            OsdOrgLst.Filter = srchOsdOrgs;


            AddNonDepOrgCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(ExecuteAddNonDepOrgCommand);
            DeleteNonDepOrgCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<TblOrg>(ExecuteDeleteNonDepOrgCommand);

            //AddOsdOrgCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand(ExecuteAddOsdOrgCommand);
            //DeleteOsdOrgCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<TblOrg>(ExecuteDeleteOsdOrgCommand);
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// سازمان بیرونی انتخاب شده
        /// </summary>
        public TblOrg SelectedNonDepOrg
        {
            get { return _selectedNonDepOrg; }
            set
            {
                _selectedNonDepOrg = value;
                if (_selectedNonDepOrg != null)
                {
                    _selectedNonDepOrg.IsDepOrg = false;
                }
                DtlOrgVMOutside.SelectedOrg = _selectedNonDepOrg;
                DtlOrgVMOutside.CanEdit = true;
                SelectedDtl = DtlOrgVMOutside;
                RaisePropertyChanged("SelectedDtl");
            }
        }

        /// <summary>
        /// سازمان هم سود انتخاب شده
        /// </summary>
        public TblOrg SelectedSharedProfitOrg
        {
            get { return _selectedSharedProfitOrg; }
            set
            {
                _selectedSharedProfitOrg = value;
                if (_selectedSharedProfitOrg != null)
                {
                    _selectedSharedProfitOrg.IsDepOrg = false;
                }

                DtlOrgVMOutside.SelectedOrg = _selectedSharedProfitOrg;
                DtlOrgVMOutside.CanEdit = false;
                SelectedDtl = DtlOrgVMOutside;
                RaisePropertyChanged("SelectedDtl");
            }
        }

        /// <summary>
        /// جزئیات سازمان تابعه انتخاب شده
        /// </summary>
        public DtlOrgViewModel DtlOrgVM { get; set; }

        /// <summary>
        /// جزئیات سازمان های بیرونی یا سازمان هم سود انتخاب شده
        /// </summary>
        public DtlOrgViewModel DtlOrgVMOutside { get; set; }

        public DtlOrgViewModel SelectedDtl { get; set; }

        /// <summary>
        /// OrgChartViewModel
        /// </summary>
        public OrgChartViewModel OrgChartVM { get; set; }

        /// <summary>
        /// SaveChangesCommand
        /// </summary>
        public System.Windows.Input.ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// سازمان های بیرونی
        /// </summary>
        public ListCollectionView NonDepOrgLst { get; set; }


        /// <summary>
        /// سازمان های غیر تابعه
        /// </summary>
        public ListCollectionView OsdOrgLst { get; set; }



        /// <summary>
        /// افزودن سازمان جدید
        /// </summary>
        public System.Windows.Input.ICommand AddNonDepOrgCommand { get; set; }

        /// <summary>
        /// حذف یک سازمان
        /// </summary>
        public System.Windows.Input.ICommand DeleteNonDepOrgCommand { get; set; }

        /// <summary>
        /// متن جستجوی سازمان های خارجی
        /// </summary>
        public string TxtForSchOsdOrg
        {
            get { return txtForSchOsdOrg; }
            set
            {
                txtForSchOsdOrg = value;
                RaisePropertyChanged("TxtForSchOsdOrg");

                OsdOrgLst.CancelNew();
                OsdOrgLst.Refresh();
            }
        }

        public void SearchOsdOrg()
        {
            if (string.IsNullOrWhiteSpace(TxtForSchOsdOrg))
            {
                OsdOrgLst.CancelNew();
                OsdOrgLst.Refresh();
            }
        }

        /// <summary>
        /// جستجوی سازمان های غیر وابسته
        /// </summary>
        public void SearchNonDepOrg()
        {
            if (!string.IsNullOrWhiteSpace(TxtForSchOsdOrg))
            {
                NonDepOrgLst.CancelNew();
                NonDepOrgLst.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TxtForSchNonDepOrg
        {
            get { return _txtForSchNonDepOrg; }
            set
            {
                _txtForSchNonDepOrg = value;

                NonDepOrgLst.CancelNew();
                NonDepOrgLst.Refresh();
            }
        }

        private bool srchOsdOrgs(object obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(TxtForSchOsdOrg))
            {
                return (obj as TblOrg).FldNamOrg.Trim().ToLower().Contains(TxtForSchOsdOrg.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }

        private bool srchNonDepOrgs(object obj)
        {
            if (obj != null && !string.IsNullOrWhiteSpace(TxtForSchNonDepOrg))
            {
                return (obj as TblOrg).FldNamOrg.Trim().ToLower().Contains(TxtForSchNonDepOrg.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }

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

        bool? _acs_ViewOrgOut;

        public bool Acs_ViewOrgOut
        {
            get
            {
                if (!_acs_ViewOrgOut.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "View", namTypEtyMjr: "OrgOut", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_ViewOrgOut = this.Usr.AcsUsr["ViewOrgOut"];
                }

                return _acs_ViewOrgOut.Value;
            }
        }



        bool? _acs_AddOrgOut;
        public bool Acs_AddOrgOut
        {
            get
            {
                if (!_acs_AddOrgOut.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Add", namTypEtyMjr: "OrgOut", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_AddOrgOut = this.Usr.AcsUsr["AddOrgOut"];
                }
                return _acs_AddOrgOut.Value;
            }
        }

        bool? _acs_DelOrgOut;

        public bool Acs_DelOrgOut
        {
            get
            {
                if (!_acs_DelOrgOut.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Del", namTypEtyMjr: "OrgOut", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_DelOrgOut = this.Usr.AcsUsr["DelOrgOut"];
                }
                return _acs_DelOrgOut.Value;
            }
        }


        #endregion

        #endregion

        #region ' Public Methods '

        public void SaveChanges()
        {
            PublicMethods.SaveContext(context);





            //    foreach (TblPosPstOrg item in context.TblPosPstOrgs)
            //    {
            //        if (context.TblNods.Where(E => E.FldCodTypEty == 2 && E.FldCodEty == item.FldCodPosPst).Count() == 0)
            //        {
            //            Model.TblNod tbl = new TblNod() { FldCodEty = item.FldCodPosPst, FldCodTypEty = 2 };
            //            context.TblNods.AddObject(tbl);
            //        }
            //    }

            //    foreach (TblPsn item in context.TblPsns)
            //    {
            //        if (context.TblNods.Where(E => E.FldCodTypEty == 3 && E.FldCodEty == item.FldCodPsn).Count() == 0)
            //        {
            //            Model.TblNod tbl = new TblNod() { FldCodEty = item.FldCodPsn, FldCodTypEty = 3 };
            //            context.TblNods.AddObject(tbl);
            //        }
            //    }

            //    PublicMethods.AddActUspfToOrgs_2075(context);

            //    this.PublicMethods.SaveContext(context);






            //    List<TblNod> nods = context.TblNods.ToList();

            //    foreach (TblNod item in nods)
            //    {
            //        if (item.FldCodTypEty == 2)
            //        {
            //            if (context.TblPosPstOrgs.SingleOrDefault(m => m.FldCodPosPst == item.FldCodEty) == null)
            //            {
            //                PublicMethods.DeleteNod_2194(context, item);
            //            }
            //        }
            //        if (item.FldCodTypEty == 1)
            //        {
            //            if (context.TblOrgs.SingleOrDefault(m => m.FldCodOrg == item.FldCodEty) == null)
            //            {
            //                PublicMethods.DeleteNod_2194(context, item);
            //            }
            //        }
            //    }

            //    this.PublicMethods.SaveContext(context);
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// یک سازمان خارجی اضافه می کند
        /// </summary>
        private void ExecuteAddNonDepOrgCommand()
        {
            int MaxNo = 0;
            for (int i = 0; i < this.NonDepOrgLst.Count; i++)
            {
                if ((this.NonDepOrgLst.GetItemAt(i) as TblOrg) != null)
                {
                    string FldName = (this.NonDepOrgLst.GetItemAt(i) as TblOrg).FldNamOrg;
                    if (FldName.Contains("سازمان جدید"))
                    {
                        int temp = 0;
                        if (int.TryParse(FldName.Substring(11 <= FldName.Length ? 11 : FldName.Length), out temp))
                        {
                            if (temp > MaxNo)
                            {
                                MaxNo = temp;
                            }
                        }
                    }
                }
            }
            MaxNo++;
            string name = "سازمان جدید " + MaxNo;
            TblOrg tbl = new TblOrg() { FldNamOrg = name };
            PublicMethods.AddOrg_2272(context, tbl);
            this.NonDepOrgLst.AddNewItem(tbl);
        }

        /// <summary>
        /// deletes organization
        /// </summary>
        private void ExecuteDeleteNonDepOrgCommand(TblOrg obj)
        {
            if (obj == null)
            {
                return;
            }
            if (Util.ShowMessageBox(2, obj.FldNamOrg) == MessageBoxResult.Yes)
            {
                if (
                    TblOrg.IsOrgAnsestorOfThisOrg(obj, context.TblOrgs.Single(o => o.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg))
                    )
                {
                    Util.ShowMessageBox(22, obj.FldNamOrg);
                    return;
                }

                if (!PublicMethods.DeleteOrg_2261(context, obj))
                {
                    Util.ShowMessageBox(32, obj.FldNamOrg);
                }
                else
                {
                    this.NonDepOrgLst.CancelNew();
                    this.NonDepOrgLst.Remove(obj);
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OrgChartVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedOrg")
            {
                OrgChartVM.SelectedOrg.IsDepOrg = true;
                DtlOrgVM.SelectedOrg = OrgChartVM.SelectedOrg;
                DtlOrgVM.CanEdit = null;
                SelectedDtl = DtlOrgVM;
                RaisePropertyChanged("SelectedDtl");
            }
        }

        /// <summary>
        /// وقتی یک سازمان اضافه میشود شخص جاری نماینده آن میشود
        /// </summary>
        /// <param name="obj"></param>
        void OrgChartVM_OrgAdded(TblOrg obj)
        {
            this.DtlOrgVM.AddCurrentPsnAsAgntOfOrg(obj);
        }


        #endregion

        #region ' events '

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);

            if (DtlOrgVM.AgntChanged)
            {
                Util.ConfirmAndRestartApp();
            }
        }

        public bool ConfirmAndClose()
        {
            if (Util.HasContextChanges(this.context))
            {
                if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                {
                    this.SaveContext();
                    return true;
                }
                else
                {
                    PublicMethods.RollBackContext(this.context);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }


        public TblOrg _selectedSharedProfitOrg { get; set; }
    }
}
