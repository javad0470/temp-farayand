using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SSYM.OrgDsn.Model
{
    public partial class TblPosPstOrg : Base.IOrgChart, IEtyNod, IAllEty, ISearchableTree
    {

        public TblPosPstOrg()
        {
            //this.SubPosPst = new ObservableCollection<TblPosPstOrg>(this.TblPosPstOrg1);

            NewlyAdded = false;

        }

        public void RaisePropertyChanged(string propName)
        {
            OnPropertyChanged(propName);
        }

        public void ResetChildCV()
        {
            this._childsCV = null;
            OnPropertyChanged("ChildsCV");
        }

        public bool NewlyAdded { get; set; }

        List<Base.IOrgChart> Base.IOrgChart.SubNodes
        {
            get
            {
                var subPos = this.TblPosPstOrg1.ToList();
                //SubPosPst = new ObservableCollection<TblPosPstOrg>(subPos);
                return new List<Base.IOrgChart>(subPos);
            }
        }

        public Base.TypeOfElementInOrgChart Type
        {
            get
            {
                if (this.FldCodTyp == 1)
                {
                    return Base.TypeOfElementInOrgChart.OrganizationalPosition;
                }
                else
                {
                    return Base.TypeOfElementInOrgChart.OrganizationalPost;
                }
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
            //int i= (subNode.Type == Base.TypeOfElementInOrgChart.OrganizationalPosition) ? 1 : 2;
            //this.TblPosPstOrg1.Add(new TblPosPstOrg() { FldCodOrg = subNode.OrganizationID, FldCodTyp = i, FldNamPosPst=subNode.Name });

            this.TblPosPstOrg1.Add((TblPosPstOrg)subNode);
        }


        public string NamParent
        {
            get
            {
                if (this.TblPosPstOrg2 != null)
                {
                    return this.TblPosPstOrg2.FldNamPosPst;
                }
                else
                {
                    return null;
                }
            }
        }

        public string Name
        {
            get { return this.FldNamPosPst; }
            set { this.FldNamPosPst = value; }
        }

        public Enum.AllTypEty TypEty
        {
            get
            {
                if (this.FldCodTyp == 1)
                {
                    return Enum.AllTypEty.Pos;
                }

                return Enum.AllTypEty.Pst;
            }
        }

        public TblOrg Org
        {
            get { return this.TblOrg; }
        }

        /// <summary>
        /// node of pospst
        /// </summary>
        public TblNod Nod
        {
            get { return PublicMethods.DetectNodOfPosPst_753(this.GetContext<BPMNDBEntities>(), this); }
        }

        #region شناسایی گره های زیر مجموعه تا یک سطح معیین


        ObservableCollection<TblPosPstOrg> _subPosPst;

        public ObservableCollection<TblPosPstOrg> SubPosPst
        {
            get
            {
                if (_subPosPst == null)
                {
                    return new ObservableCollection<TblPosPstOrg>(this.TblPosPstOrg1);
                }

                return _subPosPst;
            }
            set
            {
                _subPosPst = value;
                //if (_subPosPst != null)
                //{
                //    _subPosPst.CollectionChanged -= _subPosPst_CollectionChanged;
                //    _subPosPst.CollectionChanged += _subPosPst_CollectionChanged;
                //}
            }
        }

        //void _subPosPst_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    switch (e.Action)
        //    {
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
        //            break;
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
        //            break;
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:

        //            break;
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
        //            break;
        //        case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
        //            break;
        //        default:
        //            break;
        //    }
        //}

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
        /// <param name="posPst"></param>
        /// <param name="tnoLvlSubNod"></param>
        /// <param name="lvlCnt"></param>
        private void SubNodes_01(List<TblNod> lst, TblPosPstOrg posPst, int tnoLvlSubNod, int lvlCnt)
        {
            lst.Add(posPst.Nod);

            lvlCnt++;

            if (tnoLvlSubNod >= lvlCnt)
            {
                foreach (TblPosPstOrg item in posPst.TblPosPstOrg1)
                {
                    SubNodes_01(lst, item, tnoLvlSubNod, lvlCnt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="posPst"></param>
        /// <param name="tnoLvlSubNod"></param>
        /// <param name="lvlCnt"></param>
        private void SubNodes_02(List<TblNod> lst, TblPosPstOrg posPst)
        {
            lst.Add(posPst.Nod);

            foreach (TblPosPstOrg item in posPst.TblPosPstOrg1)
            {
                SubNodes_02(lst, item);
            }

        }

        #endregion


        #region tree normalization

        /// <summary>
        /// لیست جایگاه و سمت هایی که نماینده شناخته شده اند
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> normalizePosPstTree(List<TblPosPstOrg> lst)
        {
            // لیست همه مجموعه هایی که تاکنون ایجاد شده اند
            List<List<TblPosPstOrg>> allRoots = new List<List<TblPosPstOrg>>();

            if (lst.Count > 1)
            {
                // مرتب سازی لیست جایگاه ها بر اساس بالاترین ریشه
                lst.Sort(new Comparison<TblPosPstOrg>(compareDistanceFromRoot));
                int currentRootIndex = 0;
                allRoots.Add(new List<TblPosPstOrg>());

                // تمام زیرمجموعه های نماینده را به مجموعه بعدی اضافه میکند
                allRoots[currentRootIndex].AddRange(getAllPosPst(lst[0]));

                // به ازای نمایندگان بعدی
                for (int i = 1; i < lst.Count; i++)
                {
                    // به ازای تمام مجموعه های ساخته شده تا حالا
                    TblPosPstOrg foundItem = null;
                    for (int j = 0; j < allRoots.Count; j++)
                    {
                        // آیا این نماینده در این مجموعه هست؟
                        foundItem = allRoots[j].FirstOrDefault(p => p == lst[i]);
                        if (foundItem == null)
                        {
                            if (lst[i].TblPosPstOrg2 != null)
                            {
                                foundItem = allRoots[j].FirstOrDefault(p => p == lst[i].TblPosPstOrg2);

                                //اگر این نماینده در مجموعه نیست ولی پدرش هست،
                                if (foundItem != null)
                                {
                                    if (foundItem.SubPosPst == null)
                                    {
                                        foundItem.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                                    }

                                    // نماینده جاری را به مجموعه جاری اضافه کن
                                    allRoots[j].Add(lst[i]);

                                    // نماینده جاری را به پدرش اضافه کن
                                    if (!foundItem.SubPosPst.Contains(lst[i]))
                                    {
                                        foundItem.SubPosPst.Add(lst[i]);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    // اگر این نماینده در مجموعه های قبلی نبود
                    if (foundItem == null)
                    {
                        //این نماینده را به همراه تمام زیرمجموعه هایش به عنوان یک مجموعه جدید معرفی کن
                        allRoots.Add(new List<TblPosPstOrg>());
                        currentRootIndex++;
                        allRoots[currentRootIndex].AddRange(getAllPosPst(lst[i]));// Add(lst[i]);
                    }
                }
                return allRoots.Select(l => l.First()).ToList();
            }
            else
            {
                return lst;
            }
        }

        /// <summary>
        /// لیست جایگاه و سمت هایی که نماینده شناخته شده اند
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> normalizePosPstTree(List<List<int>> lst, BPMNDBEntities context, TblOrg org = null)
        {
            // لیست همه مجموعه هایی که تاکنون ایجاد شده اند
            List<List<TblPosPstOrg>> allRoots = new List<List<TblPosPstOrg>>();
            if (org == null)
                org = PublicMethods.CurrentUser.TblOrg;
            if (lst.Count > 1)
            {
                // مرتب سازی لیست جایگاه ها بر اساس بالاترین ریشه
                int i, j;
                for(i=0;i<lst.Count;i++)
                {
                    for (j = i; j < lst.Count; j++)
                    {
                        int iP1 = lst[i][0];
                        int iP2 = lst[j][0];
                        TblPosPstOrg p1 = context.TblPosPstOrgs.FirstOrDefault(p => p.FldCodPosPst == iP1);
                        TblPosPstOrg p2 = context.TblPosPstOrgs.FirstOrDefault(p => p.FldCodPosPst == iP2);
                        int p1Dist = getDistanceFromRoot(p1);
                        int p2Dist = getDistanceFromRoot(p2);
                        if(p2Dist<p1Dist)
                        {
                            List< int> t = lst[i];
                            lst[i] = lst[j];
                            lst[j]= t;
                        }
                    }
                }
                //lst.Sort(new Comparison<List<int>>(compareDistanceFromRoot));
                int currentRootIndex = 0;
                allRoots.Add(new List<TblPosPstOrg>());

                // تمام زیرمجموعه های نماینده را به مجموعه بعدی اضافه میکند
                allRoots[currentRootIndex].AddRange(context.TblPosPstOrgs.Where(a=> a.FldCodOrg==org.FldCodOrg).ToList().Where(a=> lst[0].Contains(a.FldCodPosPst)));
                foreach(TblPosPstOrg iii in allRoots[currentRootIndex])
                {
                    iii.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                    foreach (TblPosPstOrg ttt in iii.TblPosPstOrg1)
                    {
                        if (lst[0].Contains(ttt.FldCodPosPst))
                        {
                            iii.SubPosPst.Add(allRoots[currentRootIndex].FirstOrDefault(a => a.FldCodPosPst == ttt.FldCodPosPst));
                        }
                    }
                }
                // به ازای نمایندگان بعدی
                for ( i = 1; i < lst.Count; i++)
                {
                    // به ازای تمام مجموعه های ساخته شده تا حالا
                    TblPosPstOrg foundItem = null;
                    for ( j = 0; j < allRoots.Count; j++)
                    {
                        // آیا این نماینده در این مجموعه هست؟
                        int ii=lst[i][0];
                        foundItem = allRoots[j].FirstOrDefault(p => p.FldCodPosPst ==ii );
                        if (foundItem == null)
                        {
                            TblPosPstOrg itm = context.TblPosPstOrgs.FirstOrDefault(p => p.FldCodPosPst == ii);
                            if (itm.TblPosPstOrg2 != null)
                            {
                                foundItem = allRoots[j].FirstOrDefault(p => p.FldCodPosPst == itm.TblPosPstOrg2.FldCodPosPst);

                                //اگر این نماینده در مجموعه نیست ولی پدرش هست،
                                if (foundItem != null)
                                {
                                    if (foundItem.SubPosPst == null)
                                    {
                                        foundItem.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                                    }

                                    // نماینده جاری را به مجموعه جاری اضافه کن
                                    allRoots[j].Add(itm);

                                    // نماینده جاری را به پدرش اضافه کن
                                    if (!foundItem.SubPosPst.Contains(itm))
                                    {
                                        foundItem.SubPosPst.Add(itm);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //foundItem.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                            foreach (TblPosPstOrg iii in allRoots[j])
                            {
                                if (foundItem.FldCodPosPst == iii.FldCodPosPst)
                                {
                                    if (foundItem.SubPosPst == null)
                                        foundItem.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                                   
                                    foreach (TblPosPstOrg ttt in foundItem.TblPosPstOrg1)
                                    {
                                        if (lst[i].Contains(ttt.FldCodPosPst))
                                        {
                                            foundItem.SubPosPst.Add(context.TblPosPstOrgs.Where(a => a.FldCodOrg == org.FldCodOrg).ToList().FirstOrDefault(a => a.FldCodPosPst == ttt.FldCodPosPst));
                                            foundItem.SubPosPst[foundItem.SubPosPst.Count - 1].SubPosPst = new ObservableCollection<TblPosPstOrg>();
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }

                    // اگر این نماینده در مجموعه های قبلی نبود
                    if (foundItem == null)
                    {
                        //این نماینده را به همراه تمام زیرمجموعه هایش به عنوان یک مجموعه جدید معرفی کن
                        allRoots.Add(new List<TblPosPstOrg>());
                        currentRootIndex++;
                        allRoots[currentRootIndex].AddRange(context.TblPosPstOrgs.Where(a => a.FldCodOrg == org.FldCodOrg).ToList().Where(a => lst[i].Contains(a.FldCodPosPst)));
                        foreach (TblPosPstOrg iii in allRoots[currentRootIndex])
                        {
                            iii.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                            foreach(TblPosPstOrg ttt in iii.TblPosPstOrg1)
                            {
                                if(lst[i].Contains( ttt.FldCodPosPst))
                                {
                                    iii.SubPosPst.Add(allRoots[currentRootIndex].FirstOrDefault(a => a.FldCodPosPst == ttt.FldCodPosPst));
                                }
                            }
                        }
                    }
                }
                return allRoots.Select(l => l.First()).ToList();
            }
            else
            {
                List<TblPosPstOrg> rtnList = new List<TblPosPstOrg>();
                if (lst.Count > 0)
                {
                    int iP1 = lst[0][0];
                    TblPosPstOrg p1 = context.TblPosPstOrgs.FirstOrDefault(p => p.FldCodPosPst == iP1);
                    rtnList.Add(p1);
                }
                return rtnList;
            }
        }

        public static List<TblPosPstOrg> getAllPosPst(TblPosPstOrg p)
        {
            List<TblPosPstOrg> result = new List<TblPosPstOrg>();

            Queue<TblPosPstOrg> q = new Queue<TblPosPstOrg>();
            int i=0;
            q.Enqueue(p);

            while (q.Count > 0)
            {
                var p2 = q.Dequeue();

                if (p2.Name.Contains("123") || p2.Name.Contains("a1") || p2.FldCodPosPst == 17877)
                {
                }
                result.Add(p2);
                i++;
                foreach (var item in p2.SubPosPst)
                {
                    q.Enqueue(item);
                }
            }

            return result;
        }

        public static int compareDistanceFromRoot(TblPosPstOrg p1, TblPosPstOrg p2)
        {
            int p1Dist = getDistanceFromRoot(p1);
            int p2Dist = getDistanceFromRoot(p2);
            if (p1Dist > p2Dist)
            {
                return 1;
            }
            if (p2Dist > p1Dist)
            {
                return -1;
            }
            return 0;
        }

        

        public static int getDistanceFromRoot(TblPosPstOrg curr)
        {
            int i = 0;
            while (curr.TblPosPstOrg2 != null)
            {
                curr = curr.TblPosPstOrg2;
                i++;
            }

            return i;
        }



        #endregion

        public int CodEty
        {
            get { return this.FldCodPosPst; }
        }


        public Enum.AllTypEty CodTypEty
        {
            get { return Enum.AllTypEty.Pos; }
        }

        public static List<TblRol> GetRolsOfPosPst_23262(BPMNDBEntities ctx, TblPosPstOrg posPst)
        {
            return posPst.Nod.TblPlyrRols.Select(p => p.TblRol).ToList();
        }

        public static void RemoveFromTreeRec(TblPosPstOrg pos, TblPosPstOrg removingItem)
        {
            if (pos.SubPosPst == null)
            {
                return;
            }

            if (pos.SubPosPst.Contains(pos))
            {
                pos.SubPosPst.Remove(pos);
                pos.ChildsCV.Refresh();
                return;
            }

            foreach (var item in pos.SubPosPst)
            {
                RemoveFromTreeRec(item as TblPosPstOrg, removingItem);
            }
        }

        #region ' TreeSearch '

        ListCollectionView _childsCV;
        public ListCollectionView ChildsCV
        {
            get
            {
                if (this._childsCV == null)
                {
                    this._childsCV = new ListCollectionView(this.SubPosPst);
                }

                return this._childsCV;
            }
        }


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
                if (_isExpanded && this.TblPosPstOrg2 != null)
                    this.TblPosPstOrg2.IsExpanded = true;
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

        public void UnselectRecurcive()
        {
            this.IsExpanded = this.IsSelected = false;

            this.HighlightPhrase = null;

            if (this.SubPosPst != null)
            {
                foreach (var item in this.SubPosPst)
                {
                    item.UnselectRecurcive();
                }

                return;
            }
        }

        public void SetFilterMethodRec(Predicate<object> method)
        {
            if (this.SubPosPst != null)
            {
                foreach (var item in this.SubPosPst)
                {
                    item.SetFilterMethodRec(method);
                }
            }
            this.ChildsCV.Filter = method;
        }

        public void RefreshRec()
        {
            if (this.SubPosPst != null)
            {
                foreach (var item in this.SubPosPst)
                {
                    item.RefreshRec();
                }
            }
            this.ChildsCV.Refresh();
        }

        string _highlightPhrase;

        public string HighlightPhrase
        {
            get { return _highlightPhrase; }
            set
            {
                _highlightPhrase = value;
                OnPropertyChanged("HighlightPhrase");
            }
        }

        public string SearchableString
        {
            get { return this.FldNamPosPst; }
        }

        public ISearchableTree SearchParent
        {
            get { return this.TblPosPstOrg2; }
        }

        public IList<ISearchableTree> SearchChilds
        {
            get { return new List<ISearchableTree>(this.TblPosPstOrg1); }
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

        #region  ' Access '

        bool? _acs_EditPosPst = null;
        public bool Acs_EditPosPst
        {
            get
            {
                if (!_acs_EditPosPst.HasValue)
                {
                    if (PublicMethods.CurrentUser.AcsUsr.EditPosPstAllowedByOrg)
                    {
                        _acs_EditPosPst = true;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.FldCodOrg, this.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.PosPstOfOrgAndCurrentOrg, null, "PosPst");
                        _acs_EditPosPst = PublicMethods.CurrentUser.AcsUsr["EditPosPst"] || this.NewlyAdded;
                    }
                }

                return _acs_EditPosPst.Value;
            }
        }

        bool? _acs_ViewAgntPosPstOrg2;
        public bool Acs_ViewAgntPosPstOrg2
        {
            get
            {
                if (!_acs_ViewAgntPosPstOrg2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.FldCodOrg, "View", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_ViewAgntPosPstOrg2 = PublicMethods.CurrentUser.AcsUsr["ViewAgntPosPstOrg2"];
                }

                return _acs_ViewAgntPosPstOrg2.Value;
            }
        }

        bool? _acs_AddAgntPosPstOrg2 = null;

        public bool Acs_AddAgntPosPstOrg2
        {
            get
            {
                if (!_acs_AddAgntPosPstOrg2.HasValue)
                {

                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.FldCodOrg, "Add", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_AddAgntPosPstOrg2 = PublicMethods.CurrentUser.AcsUsr["AddAgntPosPstOrg2"];
                }

                return _acs_AddAgntPosPstOrg2.Value;
            }
        }


        bool? _acs_DelAgntPosPstOrg2;

        public bool Acs_DelAgntPosPstOrg2
        {
            get
            {
                if (!_acs_DelAgntPosPstOrg2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.FldCodOrg, "Del", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                    _acs_DelAgntPosPstOrg2 = PublicMethods.CurrentUser.AcsUsr["DelAgntPosPstOrg2"];

                }
                return _acs_DelAgntPosPstOrg2.Value;
            }
        }

        bool? _acs_EditAgntPosPst2;
        public bool Acs_EditAgntPosPst2
        {
            get
            {
                if (!_acs_EditAgntPosPst2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(this.FldCodOrg, "Edit", "AgntPosPstOrg2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                    _acs_EditAgntPosPst2 = PublicMethods.CurrentUser.AcsUsr["EditAgntPosPstOrg2"];
                    //&& !isAdmin();

                }

                return _acs_EditAgntPosPst2.Value;
            }
        }

        #endregion
    }
}
