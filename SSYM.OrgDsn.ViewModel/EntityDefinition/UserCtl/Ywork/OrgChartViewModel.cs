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
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork
{
    public class OrgChartViewModel : UserControlViewModel
    {
        #region ' Fields '

        ObservableCollection<TblOrg> org;

        List<TblOrg> _allOrgs;

        ListCollectionView _orgsCV;

        TblOrg selectedOrg;

        //public BPMNDBEntities context;

        bool canUsrEditChart = true;

        string _srchTxt;

        #endregion

        #region ' Initialaizer '

        public OrgChartViewModel(BPMNDBEntities context)
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
        public bool Acs_ViewOrgSub
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["ViewOrgSub"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddOrgSub
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["AddOrgSub"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelOrgSub
        {
            get
            {
                return PublicMethods.CurrentUser.AcsUsr["DelOrgSub"];
            }
        }


        #endregion


        public ListCollectionView OrgsCV
        {
            get
            {
                if (_orgsCV == null)
                {
                    _orgsCV = new ListCollectionView(Org);
                }

                return _orgsCV;
            }
        }

        public string SrchTxt
        {
            get { return _srchTxt; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = _srchTxt = value;
                RaisePropertyChanged("SrchTxt");

                _allOrgs.ForEach(p => p.RefreshRec());

                OrgsCV.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanUsrEditChart
        {
            get { return canUsrEditChart; }
            set
            {
                canUsrEditChart = value;

                RaisePropertyChanged("CanUsrEditChart");
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
        public TblOrg SelectedOrg
        {
            get
            {
                if (selectedOrg == null && this.Org.Count > 0)
                {
                    return this.Org.First();
                }
                return selectedOrg;
            }
            set
            {
                selectedOrg = value;
                RaisePropertyChanged("SelectedOrg");
                //if (value != selectedOrg)
                //{
                   
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DeleteOrgCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public TblOrg ExecuteAddNewOrgCommand()
        {
            if (this.SelectedOrg != null)
            {
                TblOrg org = new TblOrg() { FldNamOrg = "سازمان جدید" };

                PublicMethods.AddOrg_2272(this.bpmnEty, org);

                this.SelectedOrg.TblOrg1.Add(org);

                this.SelectedOrg.ChildsCV.AddNewItem(org);

                this.SelectedOrg.ChildsCV.CommitNew();

                PublicMethods.SaveContext(this.bpmnEty);

                this.SelectedOrg = org;

                onOrgAdded(org);

                return org;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ExecuteDeleteOrgCommand()
        {
            if (this.SelectedOrg != null)
            {
                if (Util.ShowMessageBox(2, "این سازمان") == System.Windows.MessageBoxResult.Yes)
                {
                    if (!PublicMethods.DeleteOrg_2261(this.bpmnEty, this.SelectedOrg))
                    {
                        Util.ShowMessageBox(2, "سازمان");
                    }
                }
            }
        }

        #endregion

        #region ' Private Methods '

        private void DetectAllSubOrg()
        {
            PublicMethods.ReloadEntity(this.bpmnEty, PublicMethods.CurrentUser.TblOrg, PublicMethods.CurrentUser.TblOrg.TblOrg1, "TblOrg1");

            _allOrgs = this.bpmnEty.TblOrgs.Where(m => m.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg).ToList();

            _allOrgs.ForEach(o => o.SetFilterMethodRec(SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch));

            this.Org = new ObservableCollection<TblOrg>(_allOrgs);

            this.OrgsCV.Filter = Utility.SearchAgnt.TreeSearch;

            //this.SelectedOrg = _allOrgs.FirstOrDefault();
        }

        private void onOrgAdded(TblOrg org)
        {
            if (OrgAdded != null)
            {
                OrgAdded(org);
            }
        }

        #endregion

        #region ' Events '

        internal event Action<TblOrg> OrgAdded;

        #endregion

    }
}
