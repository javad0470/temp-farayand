using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using SSYM.OrgDsn.Model.Infra;
using System.ComponentModel;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Enum;


namespace SSYM.OrgDsn.Model
{
    public interface ITblOrg
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(500, ErrorMessageResourceName = "MaxLength500", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNamOrg { get; set; }
    }

    public partial class TblOrg : Base.IOrgChart, IDataErrorInfo, INotifyDataErrorInfo, IEtyNod, IAllEty, ISearchableTree
    {
        public TblOrg()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        List<Base.IOrgChart> Base.IOrgChart.SubNodes
        {
            get
            {
                return new List<Base.IOrgChart>(this.TblOrg1.ToList());
            }
        }

        public Base.TypeOfElementInOrgChart Type
        {
            get
            {
                return Base.TypeOfElementInOrgChart.Organization;
            }
            //set;
            //{
            //    if (value == Base.TypeOfElementInOrgChart.OrganizationalPosition)
            //    {
            //        this.FldCodTyp = 1;
            //    }
            //    if (value == Base.TypeOfElementInOrgChart.OrganizationalPost)
            //    {
            //        this.FldCodTyp = 2;
            //    }

            //}
        }

        public int OrganizationID
        {
            get
            {
                return this.FldCodOrg;
            }
            set
            {
                this.FldCodOrg = value;
            }
        }

        public void AddSubNode(Base.IOrgChart subNode)
        {
            this.TblOrg1.Add((TblOrg)subNode);
        }

        public string Name
        {
            get { return this.FldNamOrg; }
            set
            {
                this.FldNamOrg = value;
                OnPropertyChanged("Name");
            }
        }

        public string NamParent
        {
            get
            {
                if (TblOrg2 != null)
                {
                    return TblOrg2.FldNamOrg;
                }
                else
                {
                    return null;
                }
            }
        }

        public string MssnOrg
        {
            get
            {
                return this.FldMssnOrg;
            }
            set
            {
                this.FldMssnOrg = value;

            }
        }

        public Enum.AllTypEty TypEty
        {
            get { return Enum.AllTypEty.Org; }
        }

        /// <summary>
        /// آیا این سازمان یک سازمان تابعه است؟
        /// </summary>
        public bool IsDepOrg { get; set; }

        public TblOrg Org
        {
            get { return this; }
        }

        TblAgntNod agnt;

        /// <summary>
        /// این پراپرتی برای مورد خاص افزوده شده است و از آن استفاده نکنید
        /// </summary>
        public TblAgntNod Agnt
        {
            get { return agnt; }
            set
            {
                agnt = value;
                OnPropertyChanged("Agnt");
            }
        }
        /// <summary>
        /// تمامی اشیای نتیجه فرستاده شده به گره های سازمان از گره های مربوط به سازمان جاری
        /// </summary>
        public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentToOrg
        {
            get
            {
                return DetectAllObjRstSentToOrg();
            }
        }

        /// <summary>
        /// لیست تمامی اشیای نتیجه فرستاده شده به گره های سازمان از گره های مربوط به سازمان جاری را نشان می دهد
        /// </summary>
        /// <returns></returns>
        private List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentToOrg()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (TblNod item in ((BPMNDBEntities)this.GetContext()).TblNods)
            {
                if (item.EtyNod.Org != null && item.EtyNod.Org.FldCodOrg == this.FldCodOrg)
                {
                    foreach (var item1 in item.AllObjRstSentToNod)
                    {
                        lst.Add(item1);
                    }
                }
            }



            return lst;
        }

        ///// <summary>
        ///// تمامی اشیای نتیجه فرستاده شده از گره های مربوط به سازمان جاری به سایر گره های مربوط به سازمان جاری
        ///// </summary>
        //public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentFromOrg
        //{
        //    get
        //    {
        //        return DetectAllObjRstSentFromOrg();
        //    }
        //}

        /// <summary>
        /// لیست تمامی اشیای نتیجه فرستاده شده به گره های سازمان از گره های مربوط به سازمان جاری را نشان می دهد
        /// </summary>
        /// <returns></returns>
        public List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentFromOrg(BPMNDBEntities context)
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (TblNod item in (context).TblNods)
            {
                // این مورد باگ است و نباید نال باشد/
                // باید جایی که یک گره بدون جایگاه، سمت،یا نقشی ایجاد میشود، شناسایی و اصلاح شود.
                if (item.EtyNod == null)
                {
                    continue;
                }
                if (item.EtyNod.Org != null && item.EtyNod.Org.FldCodOrg == this.FldCodOrg)
                {
                    foreach (var item1 in item.AllObjRstSentFromNod)
                    {
                        lst.Add(item1);
                    }
                }
            }



            return lst;
        }


        /// <summary>
        /// node of current org
        /// </summary>
        public TblNod Nod
        {
            get { return PublicMethods.DetectNodOfOrg_1738(this.GetContext<BPMNDBEntities>(), this); }
        }

        /// <summary>
        /// لیستی از فرایند های سازمان جاری را برمیگرداند
        /// </summary>
        /// <returns></returns>
        public List<TblPr> LstOfPrs_22043(BPMNDBEntities ctx)
        {
            if (ctx == null)
            {
                ctx = this.GetContext<BPMNDBEntities>();
            }

            List<TblPr> prses = ctx.TblPrs.ToList();

            prses = prses.Where(m => m.Org != null && m.Org.FldCodOrg == this.FldCodOrg).ToList();

            return prses;
        }

        /// <summary>
        /// لیستی از مجری های سازمان جاری را برمیگرداند
        /// </summary>
        /// <returns></returns>
        public List<TblNod> LstOfNods_22044(BPMNDBEntities ctx)
        {
            if (ctx == null)
            {
                ctx = this.GetContext<BPMNDBEntities>();
            }

            TblNod[] nods = ctx.TblNods.ToArray();

            return nods.Where(m => m.EtyNod != null && (m.EtyNod is TblPosPstOrg || m.EtyNod is TblRol) && m.EtyNod.Org.FldCodOrg == this.FldCodOrg).ToList();
        }

        /// <summary>
        /// لیست تمامی سازمان های زیرمجموعه به ترتیب اول سطح. اولین آیتم لیست سازمان جاری است
        /// </summary>
        /// <returns></returns>
        public List<TblOrg> GetSubOrgs()
        {
            List<TblOrg> subOrgList = new List<TblOrg>();
            Queue<TblOrg> subOrgs = new Queue<TblOrg>();

            subOrgs.Enqueue(this);

            while (subOrgs.Count > 0)
            {
                TblOrg org = subOrgs.Dequeue();
                subOrgList.Add(org);

                foreach (var subOrg in org.TblOrg1)
                {
                    subOrgs.Enqueue(subOrg);
                }
            }

            return subOrgList;
        }

        /// <summary>
        /// لیست تمامی گره های سازمان جاری
        /// </summary>
        public List<TblNod> AllNodesOfCurrentOrg
        {
            get
            {
                return DetectAllNodesOfOrg();
            }
        }

        /// <summary>
        /// شناسایی لیست تمام گره های سازمان جاری
        /// </summary>
        /// <returns></returns>
        private List<TblNod> DetectAllNodesOfOrg()
        {
            List<TblNod> nod = new List<TblNod>();

            using (BPMNDBEntities context = new BPMNDBEntities())
            {
                nod = context.TblNods.ToList();

                return nod.Where(m => m.EtyNod != null && m.EtyNod.Org != null && m.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg).ToList();
            }
        }

        #region شناسایی گره های زیر مجموعه تا یک سطح معین


        /// <summary>
        /// گره های زیر مجموعه گره جاری را تا یک سطح معین تعیین می کند
        /// </summary>
        /// <param name="tnoLvlSubNod">تعداد سطوح</param>
        /// <returns></returns>
        public List<TblNod> DetectSubNod_22116(int tnoLvlSubNod)
        {
            List<TblNod> nod = new List<TblNod>();

            int i = 0;

            if (tnoLvlSubNod != 100)
            {
                SubNodes_01(nod, this, tnoLvlSubNod, i);
            }

            else
            {
                SubNodes_02(nod, this);
            }

            return nod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="org"></param>
        /// <param name="tnoLvlSubNod"></param>
        /// <param name="lvlCnt"></param>
        private void SubNodes_01(List<TblNod> lst, TblOrg org, int tnoLvlSubNod, int lvlCnt)
        {
            lst.Add(org.Nod);

            lvlCnt++;

            if (tnoLvlSubNod >= lvlCnt)
            {
                foreach (TblOrg item in org.TblOrg1)
                {
                    SubNodes_01(lst, item, tnoLvlSubNod, lvlCnt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="org"></param>
        /// <param name="tnoLvlSubNod"></param>
        /// <param name="lvlCnt"></param>
        private void SubNodes_02(List<TblNod> lst, TblOrg org)
        {
            lst.Add(org.Nod);

            foreach (TblOrg item in org.TblOrg1)
            {
                SubNodes_02(lst, item);
            }
        }

        #endregion

        #region شناسایی اشخاص درون و برون سازمانی

        /// <summary>
        /// 
        /// </summary>
        public List<TblPsn> PsnIsdOrg
        {
            get
            {
                return DetectAllPsnIsdOrg();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TblPsn> PsnOsdOrg
        {
            get
            {
                return DetectAllPsnOsdOrg();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<TblPsn> DetectAllPsnIsdOrg()
        {
            List<TblPsn> psn = new List<TblPsn>();

            foreach (TblUsr item in this.TblUsrs)
            {
                if (item.TblPsn.FldIsdOrg)
                {
                    psn.Add(item.TblPsn);
                }
            }

            return psn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<TblPsn> DetectAllPsnOsdOrg()
        {
            List<TblPsn> psn = new List<TblPsn>();

            foreach (TblUsr item in this.TblUsrs)
            {
                if (item.TblPsn.FldIsdOrg)
                {
                    psn.Add(item.TblPsn);
                }
            }

            return psn;
        }

        #endregion

        #region IAllEty


        public int CodEty
        {
            get { return this.FldCodOrg; }
        }

        #endregion


        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamOrg")
                {
                    return true;
                }
            }
            return false;
        }

        [NonSerialized]
        private DataErrorInfoSupport dataErrorInfoSupport;


        public string Error
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return null;
                }

                return dataErrorInfoSupport.Error;
            }
        }

        public string this[string memberName]
        {
            get
            {
                if (!shouldCheckErrors(memberName))
                {
                    return null;
                }

                return dataErrorInfoSupport[memberName];
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return;
            }

            dataErrorInfoSupport.RaiseErrorsChanged(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged += value;
                }
            }
            remove
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged -= value;
                }
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return null;
            }

            return dataErrorInfoSupport.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return false;
                }

                return dataErrorInfoSupport.HasErrors;
            }
        }

        #endregion


        #region MyRegion

        public static bool IsOrgAnsestorOfThisOrg(TblOrg parent, TblOrg child)
        {
            while (true)
            {
                if (child.TblOrg2 == null)
                {
                    return false;
                }
                if (child.TblOrg2.FldCodOrg == parent.FldCodOrg)
                {
                    return true;
                }
                else
                {
                    child = child.TblOrg2;
                }
            }
        }

        #endregion





        public Enum.AllTypEty CodTypEty
        {
            get { return Enum.AllTypEty.Org; }
        }

        #region ' Tree Search '


        #region IsExpanded

        bool _isExpanded;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && this.SearchParent != null)
                    this.SearchParent.IsExpanded = true;
            }
        }

        #endregion // IsExpanded

        #region IsSelected

        bool _isSelected;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected

        ListCollectionView _childsCV;
        public ListCollectionView ChildsCV
        {
            get
            {
                if (this._childsCV == null)
                {
                    this._childsCV = new ListCollectionView(this.TblOrg1.ToList());
                }

                return this._childsCV;
            }
        }

        public void UnselectRecurcive()
        {
            this.IsExpanded = this.IsSelected = false;

            this.HighlightPhrase = null;

            foreach (var item in this.SearchChilds)
            {
                item.UnselectRecurcive();
            }
        }

        public void SetFilterMethodRec(Predicate<object> method)
        {
            foreach (var item in this.SearchChilds)
            {
                item.SetFilterMethodRec(method);
            }

            this.ChildsCV.Filter = method;
        }

        public void RefreshRec()
        {
            foreach (var item in this.SearchChilds)
            {
                item.RefreshRec();
            }
            this.ChildsCV.Refresh();
        }

        public string HighlightPhrase
        {
            get;

            set;
        }

        public string SearchableString
        {
            get { return this.Name; }
        }

        public ISearchableTree SearchParent
        {
            get { return this.TblOrg2; }
        }

        public IList<ISearchableTree> SearchChilds
        {
            get { return new List<ISearchableTree>(this.TblOrg1); }
        }

        #endregion


        public string NamTypEty
        {
            get
            {
                return SSYM.OrgDsn.Model.Enum.EnumUtil.NamNod(this.TypEty);
            }
        }

        bool _isSelectedInTree;
        public bool IsSelectedInTree
        {
            get { return _isSelectedInTree; }
            set
            {
                _isSelectedInTree = value;
                OnPropertyChanged("IsSelectedInTree");
            }
        }

        bool _isListSelected;

        public bool IsListSelected
        {
            get { return _isListSelected; }
            set
            {
                _isListSelected = value;
                OnPropertyChanged("IsListSelected");
            }
        }


        #region ' Access '

        bool? _acs_ViewAgntOrg2;
        public bool Acs_ViewAgntOrg2
        {
            get
            {
                if (!_acs_ViewAgntOrg2.HasValue)
                {
                    if (!this.IsDepOrg)
                    {
                        _acs_ViewAgntOrg2 = false;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                        _acs_ViewAgntOrg2 = PublicMethods.CurrentUser.AcsUsr["ViewAgntOrg2"] &&
                            (TblOrg.IsOrgAnsestorOfThisOrg(PublicMethods.CurrentUser.TblOrg, this) || PublicMethods.CurrentUser.TblOrg.FldCodOrg == this.FldCodOrg);
                    }
                }

                return _acs_ViewAgntOrg2.Value;
            }
        }


        bool? _acs_AddAgntOrg2;
        public bool Acs_AddAgntOrg2
        {
            get
            {
                if (!_acs_AddAgntOrg2.HasValue)
                {
                    if (!this.IsDepOrg)
                    {
                        _acs_AddAgntOrg2 = false;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                        _acs_AddAgntOrg2 = PublicMethods.CurrentUser.AcsUsr["AddAgntOrg2"];
                    }
                }
                return _acs_AddAgntOrg2.Value;
            }
        }


        bool? _acs_DelAgntOrg2;
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAgntOrg2
        {
            get
            {
                if (!_acs_DelAgntOrg2.HasValue)
                {
                    if (!this.IsDepOrg)
                    {
                        _acs_DelAgntOrg2 = false;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                        _acs_DelAgntOrg2 = PublicMethods.CurrentUser.AcsUsr["DelAgntOrg2"];
                    }
                }

                return _acs_DelAgntOrg2.Value;
            }
        }


        bool? _acs_EditAgntOrg2;
        public bool Acs_EditAgntOrg2
        {
            get
            {
                if (!_acs_EditAgntOrg2.HasValue)
                {
                    if (!this.IsDepOrg)
                    {
                        _acs_EditAgntOrg2 =  false;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                        _acs_EditAgntOrg2 = PublicMethods.CurrentUser.AcsUsr["EditAgntOrg2"];// && !isAdmin();
                    }
                }
                return _acs_EditAgntOrg2.Value;
            }
        }


        #endregion
    }
}
