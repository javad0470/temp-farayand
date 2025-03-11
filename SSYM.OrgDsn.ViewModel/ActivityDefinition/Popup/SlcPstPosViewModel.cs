using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcPstPosViewModel : PopupViewModel
    {

        #region ' Fields '

        BPMNDBEntities context;
        List<TblPosPstOrg> _allItems;
        bool _showPosPstInsidePrs = false;
        bool _isMultiSelect = false;
        TblPosPstOrg selectedPosPst;
        string txtSch;
        List<TblPosPstOrg> tblPosPstOrgs;

        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="codPrs">کد فرایندی که جایگاه و سمت های آن نمایش داده میشود</param>
        public SlcPstPosViewModel(BPMNDBEntities context, int? codPrs = null, bool isMultiSelect = false)
            : base(context)
        {

            SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = null;

            this.context = context;

            ShowPosPstInsidePrs = codPrs.HasValue;

            this.IsMultiSelect = isMultiSelect;

            if (ShowPosPstInsidePrs)
            {
                _allItems = PublicMethods.DetectPosPstInPrs_22181(context, context.TblPrs.Single(p => p.FldCodPrs == codPrs.Value));
            }
            else
            {
                _allItems = DetectAllPosPst(true);
            }

            foreach (var item in _allItems)
            {
                item.SetFilterMethodRec(SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch);
            }

            List<TblPosPstOrg> res = new List<TblPosPstOrg>();
            foreach (var item in _allItems)
            {
                PublicMethods.DetectSubPosPst_2191(item, res);
                res.ForEach(p =>
                           {
                               p.PropertyChanged -= p_PropertyChanged;
                               p.PropertyChanged += p_PropertyChanged;
                           });

            }


            //_allItems.First().get

            PosPstCV = new ListCollectionView(_allItems);

            PosPstCV.Filter = SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.TreeSearch;
            /*
            if(_allItems.Count>0 && SelectedOrgTemp==null)
                SelectedOrgTemp = _allItems[0];
            */
        }


        #endregion

        #region ' Properties / Commands '

        public bool IsMultiSelect
        {
            get { return _isMultiSelect; }
            set
            {
                _isMultiSelect = value;
                RaisePropertyChanged("IsMultiSelect");
            }
        }

        public string TxtSch
        {
            get { return txtSch; }
            set
            {
                SSYM.OrgDsn.ViewModel.Utility.SearchAgnt.SerachTerm = txtSch = value;

                RaisePropertyChanged("TxtSch");

                _allItems.ForEach(p => p.RefreshRec());

                PosPstCV.Refresh();
            }
        }

        /// <summary>
        /// جایگاه و سمت های داخل یک فرایند خاص را نشان بده
        /// </summary>
        public bool ShowPosPstInsidePrs
        {
            get { return _showPosPstInsidePrs; }
            set { _showPosPstInsidePrs = value; }
        }

        public ListCollectionView PosPstCV { get; set; }

        public TblPosPstOrg SelectedPosPst
        {
            get
            {
                return selectedPosPst;
            }
            set
            {
                selectedPosPst = value;
                RaisePropertyChanged("SelectedPosPst");
            }
        }

        public TblPosPstOrg SelectedOrgTemp
        {
            get;
            set;
        }

        public List<TblPosPstOrg> TblPosPstOrgs
        {
            get { return tblPosPstOrgs; }
            set
            {
                tblPosPstOrgs = value;

                RaisePropertyChanged("TblPosPstOrgs");
            }
        }

        #endregion

        #region ' Public Methods '

        public void SelectItems(List<TblPosPstOrg> selectedPoses)
        {
            foreach (var item in _allItems)
            {
                var allPoses = new List<TblPosPstOrg>();

                PublicMethods.DetectSubPosPst_2191(item, allPoses);
                allPoses.ForEach(p =>
                {
                    p.PropertyChanged -= p_PropertyChanged;
                    p.IsSelectedInTree = false;
                    p.PropertyChanged += p_PropertyChanged;
                });

                allPoses.Intersect(selectedPoses).ToList().ForEach(p =>
                {
                    //p.PropertyChanged -= p_PropertyChanged;
                    p.IsSelectedInTree = true;
                    //p.PropertyChanged += p_PropertyChanged;
                });
            }
        }

        public void OK()
        {
            OKExecute();
        }

        #endregion

        #region ' Private Methods '

        protected override void OKExecute()
        {
            base.OKExecute();
            SelectedPosPst = SelectedOrgTemp;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<TblPosPstOrg> DetectAllPosPst()
        {
            if (IsMultiSelect) // اگر این فرم در قسمت انتخاب بازیگر نقش باز می شود به سطوح دسترسی توجه نکن
            {
                var org = context.TblOrgs.Single(o => o.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);

                TblPosPstOrgs = org.TblPosPstOrgs.Where(p => !p.FldCodUpl.HasValue).ToList();

                foreach (var item in TblPosPstOrgs)
                {
                    PublicMethods.DetectSubOrgLevel(item, int.MaxValue);
                }
            }
            else
            {
                TblPosPstOrgs = PublicMethods.GetPosPstOfPsnAgntOfThem(context, PublicMethods.CurrentUser.TblPsn, PublicMethods.CurrentUser.TblOrg);
            }
            
            return TblPosPstOrg.normalizePosPstTree(TblPosPstOrgs);
            //PublicMethods.DetectPosPstWithAgntOfPsn_22179(context, PublicMethods.CurrentUser.TblPsn).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        private List<TblPosPstOrg> DetectAllPosPst(bool UseFldCode)
        {
            if (IsMultiSelect) // اگر این فرم در قسمت انتخاب بازیگر نقش باز می شود به سطوح دسترسی توجه نکن
            {
                var org = context.TblOrgs.Single(o => o.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);

                TblPosPstOrgs = org.TblPosPstOrgs.Where(p => !p.FldCodUpl.HasValue).ToList();

                foreach (var item in TblPosPstOrgs)
                {
                    PublicMethods.DetectSubOrgLevel(item, int.MaxValue);
                }

                return TblPosPstOrg.normalizePosPstTree(TblPosPstOrgs);
            }
            else
            {
                List<List<int>> temp = null;
                temp = PublicMethods.GetPosPstOfPsnAgntOfThem(context, PublicMethods.CurrentUser.TblPsn, PublicMethods.CurrentUser.TblOrg,true);
                TblPosPstOrgs = PublicMethods.GetPosPstOfPsnAgntOfThem(context, PublicMethods.CurrentUser.TblPsn, PublicMethods.CurrentUser.TblOrg);
                return TblPosPstOrg.normalizePosPstTree(temp, context);

            }
            
            //PublicMethods.DetectPosPstWithAgntOfPsn_22179(context, PublicMethods.CurrentUser.TblPsn).ToList();
        }

        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelectedInTree")
            {
                if (TblPosPstSelectionChanged != null)
                {
                    TblPosPstSelectionChanged(sender as TblPosPstOrg);
                }
            }
        }

        #endregion

        #region ' Events '
        public event Action<TblPosPstOrg> TblPosPstSelectionChanged;

        #endregion

    }
}
