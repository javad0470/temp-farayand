using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using yWorks.yFiles.UI.DataBinding;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model.Enum;
using System.Windows.Data;
using System.Collections;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork
{
    public class PosPstChartViewModel : UserControlViewModel
    {
        #region ' Fields '

        ObservableCollection<TblOrg> org;

        TblOrg selectedOrg;

        ObservableCollection<TblPosPstOrg> posPst;
        List<TblPosPstOrg> _allItems;

        TblPosPstOrg selectedPosPst;

        bool canUsrEditPosPst = true;




        #endregion

        #region ' Initialaizer '

        public PosPstChartViewModel(BPMNDBEntities context)
            : base(context)
        {
            SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = null;

            DetectAllSubOrg();

        }

        #endregion

        #region ' Properties / Commands '

        #region Access

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewPosPst
        {
            get
            {
                if (this.SelectedOrg != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, this.SelectedOrg, "View", Model.Enum.TypRlnEtyMjrWthEtyMom.PosPstOfOrgAndCurrentOrg, namTypEtyMjr: "PosPst");

                    bool b = PublicMethods.CurrentUser.AcsUsr["ViewPosPst"];

                    if (!b)
                    {
                        Util.ShowMessageBox(17, "مشاهده جایگاه ها و سمت های این سازمان");
                    }

                    return b;
                }

                return false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddPosPst
        {
            get
            {
                if (this.SelectedOrg != null)
                {
                    if (this.SelectedPosPst != null)
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Add", "PosPst", AllTypEty.Pos, new TblNod[] { this.SelectedOrg.Nod, this.SelectedPosPst.Nod });
                    }

                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Add", "PosPst", AllTypEty.Pos, new TblNod[] { this.SelectedOrg.Nod });
                    }

                    return PublicMethods.CurrentUser.AcsUsr["AddPosPst"] || (this.SelectedPosPst != null && this.SelectedPosPst.NewlyAdded);
                }

                return false;

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelPosPst
        {
            get
            {
                if (this.SelectedOrg != null)
                {
                    if (this.SelectedPosPst != null)
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Del", "PosPst", AllTypEty.Pos, new TblNod[] { this.SelectedOrg.Nod, this.SelectedPosPst.Nod });
                    }

                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.SelectedOrg.FldCodOrg, "Del", "PosPst", AllTypEty.Pos, new TblNod[] { this.SelectedOrg.Nod });
                    }

                    return PublicMethods.CurrentUser.AcsUsr["DelPosPst"] || (this.SelectedPosPst != null && this.SelectedPosPst.NewlyAdded);
                }

                return false;

            }
        }

        #endregion

        public ListCollectionView PosPstCV { get; set; }

        string txtSch;

        public string TxtSch
        {
            get { return txtSch; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = txtSch = value;

                RaisePropertyChanged("TxtSch");

                RefreshTree();
            }
        }

        public void RefreshTree()
        {
            _allItems.ForEach(p => p.RefreshRec());

            PosPstCV.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanUsrEditPosPst
        {
            get { return canUsrEditPosPst; }
            set
            {
                canUsrEditPosPst = value;

                RaisePropertyChanged("CanUsrEditPosPst");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanUsrAddFirstPosPst
        {
            get
            {
                if (this.SelectedOrg != null && this.PosPst.Count == 0 && Acs_AddPosPst)
                {
                    return true;
                }

                return false;
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
                bool b = false;

                if (selectedOrg != value)
                {
                    b = true;
                }

                selectedOrg = value;

                DetectAllSubPosPst();

                RaisePropertyChanged("SelectedOrg", "CanUsrAddFirstPosPst");

                if (b)
                {
                    RaisePropertyChanged("Acs_ViewPosPst", "Acs_AddPosPst", "Acs_DelPosPst");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblOrg> Org
        {
            get { return org; }
            set
            {
                org = value;

                RaisePropertyChanged("Org");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblPosPstOrg> PosPst
        {
            get { return posPst; }

            set
            {
                posPst = value;
                posPst.CollectionChanged -= posPst_CollectionChanged;
                posPst.CollectionChanged += posPst_CollectionChanged;
                RaisePropertyChanged("PosPst", "CanUsrAddFirstPosPst");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblPosPstOrg SelectedPosPst
        {
            get
            {
                if (selectedPosPst == null && this.PosPst != null && this.PosPst.Count > 0)
                {
                    return this.PosPst.First();
                }
                return selectedPosPst;
            }

            set
            {
                bool b = false;

                if (selectedPosPst != value)
                {
                    b = true;
                }

                selectedPosPst = value;

                if (b)
                {
                    RaisePropertyChanged("SelectedPosPst", "Acs_AddPosPst", "Acs_DelPosPst");
                }
            }
        }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public TblPosPstOrg ExecuteAddNewPosPstCommand(PosPst CodTypPosPst)
        {
            if (this.SelectedPosPst != null)
            {
                //یک جایگاه سازمانی نمی تواند زیر مجموعه یک سمت سازمانی قرار گیرد.
                if (SelectedPosPst.FldCodTyp == (int)Model.Enum.PosPst.Pst)
                {
                    if (CodTypPosPst == Model.Enum.PosPst.Pos)
                    {
                        Util.ShowMessageBox(28);
                        return null;
                    }
                }

                TblPosPstOrg tbl = new TblPosPstOrg() { FldCodOrg = this.SelectedOrg.FldCodOrg, FldCodTyp = (int)CodTypPosPst, FldNamPosPst = CodTypPosPst == Model.Enum.PosPst.Pos ? "جایگاه سازمانی جدید" : "سمت سازمانی جدید", FldLvl = 0 };

                tbl.NewlyAdded = true;

                //Nourbaksh 
                //must be checked
                if (this.SelectedPosPst.NewlyAdded && this.SelectedPosPst.SubPosPst==null)
                {
                    this.SelectedPosPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                }

                this.SelectedPosPst.SubPosPst.Add(tbl);

                this.SelectedPosPst.TblPosPstOrg1.Add(tbl);

                PublicMethods.SaveContext(this.bpmnEty);

                Model.TblNod tbl2 = new TblNod() { FldCodEty = tbl.FldCodPosPst, FldCodTypEty = 2 };

                this.bpmnEty.TblNods.AddObject(tbl2);

                Model.TblAct tbl3 = new TblAct() { FldNamAct = "فعالیت نامشخص", FldTypAct = (int)Model.Enum.ActivityTypes.Manual, FldActUspf = true };

                tbl2.TblActs.Add(tbl3);

                PublicMethods.SaveContext(this.bpmnEty);

                onPosPstAdded(tbl);

                this.SelectedPosPst.ResetChildCV();

                return tbl;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecuteAddFirstNodeCommand()
        {
            if (this.SelectedOrg != null && this.SelectedOrg.TblPosPstOrgs.Count == 0)
            {
                Model.TblPosPstOrg tblPosPst = new Model.TblPosPstOrg() { FldCodTyp = 1, FldNamPosPst = "جایگاه سازمانی جدید" };

                tblPosPst.NewlyAdded = true;

                this.SelectedOrg.TblPosPstOrgs.Add(tblPosPst);

                this.PosPst.Add(tblPosPst);

                PublicMethods.SaveContext(bpmnEty);

                TblNod nod = new TblNod() { FldCodEty = tblPosPst.FldCodPosPst, FldCodTypEty = (int)Model.Enum.FldTypEty.PosPst };

                this.bpmnEty.TblNods.AddObject(nod);

                TblAct act = new TblAct() { FldNamAct = "فعالیت نامشخص", FldActUspf = true, FldTypAct = (int)Model.Enum.ActivityTypes.Manual };

                nod.TblActs.Add(act);

                PublicMethods.SaveContext(bpmnEty);

                RaisePropertyChanged("CanUsrAddFirstPosPst");

                onPosPstAdded(tblPosPst);

                RefreshTree();
            }
        }


        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void DetectAllSubOrg()
        {
            var currPsn = this.bpmnEty.TblPsns.Single(p => p.FldCodPsn == PublicMethods.CurrentUser.FldCodPsn);

            PublicMethods.ReloadEntity(this.bpmnEty, PublicMethods.CurrentUser.TblOrg, PublicMethods.CurrentUser.TblOrg.TblOrg1, "TblOrg1");

            this.Org = new ObservableCollection<TblOrg>(PublicMethods.GetOrgForPsnAgntOfThemInCurrOrgAndSubOrg(currPsn));

            //this.Org = new ObservableCollection<TblOrg>(this.bpmnEty.TblOrgs.Where(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg));
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که شخص جاری نماینده آنهاست
        /// </summary>
        private void DetectAllSubPosPst()
        {

            if (this.SelectedOrg != null)
            {

                if (
                    //اگر نماینده سازمان است سطوح نمایندگی اصلا مطرح نیست!!!
                    PublicMethods.GetAgntOfPsnIsdOrg_22230(this.bpmnEty, PublicMethods.CurrentUser.TblPsn.FldCodPsn, this.SelectedOrg.FldCodOrg).Any(a => a.TblNod.FldCodTypEty == (int)FldTypEty.Org)
                    )
                {
                    _allItems = this.SelectedOrg.TblPosPstOrgs.Where(p => !p.FldCodUpl.HasValue).ToList();
                    this.PosPst = new ObservableCollection<TblPosPstOrg>(_allItems);

                    foreach (var item in PosPst)
                    {
                        PublicMethods.DetectSubOrgLevel(item, 100);
                    }

                }
                else
                {
                    _allItems = PublicMethods.GetPosPstOfPsnAgntOfThem(bpmnEty, PublicMethods.CurrentUser.TblPsn, this.SelectedOrg);
                    _allItems = TblPosPstOrg.normalizePosPstTree(_allItems);
                    List<List<int>> temp = null;
                    temp = PublicMethods.GetPosPstOfPsnAgntOfThem(bpmnEty, PublicMethods.CurrentUser.TblPsn, this.SelectedOrg, true);
                    _allItems = TblPosPstOrg.normalizePosPstTree(temp, bpmnEty, this.SelectedOrg);
                    this.PosPst = new ObservableCollection<TblPosPstOrg>(_allItems);
                }

                _allItems.ForEach(p => p.SetFilterMethodRec(SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch));

                PosPstCV = new ListCollectionView(this.PosPst);

                PosPstCV.Filter = SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch;

                SelectedPosPst = this.PosPst.FirstOrDefault();

                RaisePropertyChanged("PosPstCV");

            }
        }

        /// <summary>
        /// برای ریفرش شدن دکمه افزودن گره سرشاخه
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void posPst_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("CanUsrAddFirstPosPst");
        }

        private void onPosPstAdded(TblPosPstOrg posPst)
        {
            if (PosPstAdded != null)
            {
                PosPstAdded(posPst);
            }
        }
        #endregion

        #region ' Events '

        internal event Action<TblPosPstOrg> PosPstAdded;

        #endregion

    }
}
