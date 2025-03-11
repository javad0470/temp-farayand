using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public enum SearchObjType
    {
        /// <summary>
        /// جستجو در فعالیت ها
        /// </summary>
        Act = 1,

        /// <summary>
        /// جستجو در مجریان
        /// </summary>
        Nod = 2,

        /// <summary>
        /// جستجو در روند ها
        /// </summary>
        Prs = 3,

        /// <summary>
        /// جستجو در رخدادهای آغازگر
        /// </summary>
        EvtSrt = 4,

        /// <summary>
        /// جستجو در رخدادهای نتیجه
        /// </summary>
        EvtRst = 5,

        /// <summary>
        /// جستجو در ورودی/خروجی ها
        /// </summary>
        InOut = 6,

        /// <summary>
        /// جستجو در اخبار
        /// </summary>
        News = 7
    }

    public class PrsSearchObj : NotificationObject
    {
        public Visibility EvtSrtVisibility { get; set; }

        public Visibility EvtRstVisibility { get; set; }

        public Visibility OtherVisibility { get; set; }

        SearchObjType searchType;

        /// <summary>
        /// نوع جستجو
        /// </summary>
        public SearchObjType SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                searchType = value;

                EvtSrtVisibility = Visibility.Collapsed;
                EvtRstVisibility = Visibility.Collapsed;
                OtherVisibility = Visibility.Visible;

                switch (searchType)
                {
                    case SearchObjType.Act:
                        break;
                    case SearchObjType.Nod:
                        break;
                    case SearchObjType.Prs:
                        break;
                    case SearchObjType.EvtSrt:
                        EvtSrtVisibility = Visibility.Visible;
                        EvtRstVisibility = Visibility.Collapsed;
                        OtherVisibility = Visibility.Collapsed;
                        break;
                    case SearchObjType.EvtRst:
                        EvtSrtVisibility = Visibility.Collapsed;
                        EvtRstVisibility = Visibility.Visible;
                        OtherVisibility = Visibility.Collapsed;

                        break;
                    case SearchObjType.InOut:
                        break;
                    case SearchObjType.News:
                        break;
                    default:
                        break;
                }
                RaisePropertyChanged("SearchType", "EvtSrtVisibility", "EvtRstVisibility", "OtherVisibility");
            }
        }

        /// <summary>
        /// مبنای جستجو
        /// </summary>
        public object SearchString { get; set; }


        KeyValuePair<int, string> selectedCdn;
        /// <summary>
        /// شرط انتخاب شده
        /// </summary>
        public KeyValuePair<int, string> SelectedCdn
        {
            get
            {
                return selectedCdn;
            }
            set
            {
                selectedCdn = value;
                SearchType = (SearchObjType)value.Key;
                SearchString = null;
                RaisePropertyChanged("SelectedCdn", "SearchString");
            }
        }

        /// <summary>
        /// گروه شرط
        /// شرطهای با گروه یکسان با هم اند و خود گروه ها با هم اور میشوند
        /// </summary>
        public int GrpCdn { get; set; }
    }

    public class FndPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        List<KeyValuePair<int, string>> comboItems;
        BPMNDBEntities context = new BPMNDBEntities();

        #endregion

        #region ' Initialaizer '

        public FndPrsViewModel()
        {
            InitialaizeCommands();
            SearchItems = new ObservableCollection<PrsSearchObj>();
            RaisePropertyChanged();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// لیست موارد مورد شرط
        /// </summary>
        public List<KeyValuePair<int, string>> ComboItems
        {
            get
            {
                if (comboItems == null)
                {
                    List<KeyValuePair<int, string>> dropDownItems = new List<KeyValuePair<int, string>>();
                    foreach (var item in Enum.GetValues(typeof(SearchObjType)))
                    {
                        SearchObjType itm = (SearchObjType)item;

                        dropDownItems.Add(new KeyValuePair<int, string>((int)itm, TranslateSearchObjType(itm)));

                        //if (SearchItems.FirstOrDefault(m => m.SearchType == itm) == null)// serach item not exist in collection
                        //{
                        //    dropDownItems.Add(new KeyValuePair<int, string>((int)itm, ""));
                        //}
                    }
                    comboItems = dropDownItems;
                }
                return comboItems;
            }
        }

        public ObservableCollection<PrsSearchObj> SearchItems { get; set; }

        public ListCollectionView SearchItemsCV
        {
            get
            {
                ListCollectionView cv = (ListCollectionView)CollectionViewSource.GetDefaultView(SearchItems);

                PropertyGroupDescription groupDescription
                        = new PropertyGroupDescription("GrpCdn");
                cv.GroupDescriptions.Clear();
                cv.GroupDescriptions.Add(groupDescription);
                return cv;

            }
        }

        public DelegateCommand<object> AddCdnToGrp { get; set; }

        public DelegateCommand AddNewGrp { get; set; }

        public DelegateCommand<PrsSearchObj> DeleteCdn { get; set; }

        public List<TblPr> FilterResult { get; set; }

        List<KeyValuePair<EvtSrtType, string>> evtSrtTypes;
        public List<KeyValuePair<EvtSrtType, string>> EvtSrtTypes
        {
            get
            {
                if (evtSrtTypes == null)
                {
                    evtSrtTypes = new List<KeyValuePair<EvtSrtType, string>>();
                    List<TblItmFixSfw> evtSrts = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == (int)ItmFixSfw.TypEvtSrt).ToList();

                    foreach (var item in evtSrts)
                    {
                        evtSrtTypes.Add(new KeyValuePair<EvtSrtType, string>((EvtSrtType)item.FldCodItm, item.FldNamItm));
                    }
                }

                return evtSrtTypes;
            }
        }


        List<KeyValuePair<EvtRstType, string>> evtRstTypes;
        public List<KeyValuePair<EvtRstType, string>> EvtRstTypes
        {
            get
            {
                if (evtRstTypes == null)
                {
                    evtRstTypes = new List<KeyValuePair<EvtRstType, string>>();
                    List<TblItmFixSfw> evtRsts = PublicMethods.TblItmFixSfws.Where(m => m.FldCodSbj == (int)ItmFixSfw.TypeEvtRst).ToList();

                    foreach (var item in evtRsts)
                    {
                        evtRstTypes.Add(new KeyValuePair<EvtRstType, string>((EvtRstType)item.FldCodItm, item.FldNamItm));
                    }
                }

                return evtRstTypes;
            }
        }


        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private List<KeyValuePair<int, string>> FillDropDown()
        {
            List<KeyValuePair<int, string>> dropDownItems = new List<KeyValuePair<int, string>>();
            foreach (var item in Enum.GetValues(typeof(SearchObjType)))
            {
                SearchObjType itm = (SearchObjType)item;

                dropDownItems.Add(new KeyValuePair<int, string>((int)itm, ""));

                //if (SearchItems.FirstOrDefault(m => m.SearchType == itm) == null)// serach item not exist in collection
                //{
                //    dropDownItems.Add(new KeyValuePair<int, string>((int)itm, ""));
                //}
            }

            return dropDownItems;
        }

        private string TranslateSearchObjType(SearchObjType type)
        {
            switch (type)
            {
                case SearchObjType.Act:
                    return "فعالیت";

                case SearchObjType.Nod:
                    return "گره";

                case SearchObjType.Prs:
                    return "فرایند";

                case SearchObjType.EvtSrt:
                    return "رخداد آغازگر";

                case SearchObjType.EvtRst:
                    return "رخداد نتیجه";

                case SearchObjType.InOut:
                    return "ورودی/خروجی";

                case SearchObjType.News:
                    return "خبر";
                default:
                    return null;
            }
        }

        private void InitialaizeCommands()
        {
            AddCdnToGrp = new DelegateCommand<object>(AddCdnToGrpExecute, CanAddCdnToGrp);
            AddNewGrp = new DelegateCommand(AddNewGrpExecute, CanAddNewGrp);
            DeleteCdn = new DelegateCommand<PrsSearchObj>(DeleteSearchObj);
        }

        private void DeleteSearchObj(PrsSearchObj obj)
        {
            SearchItems.Remove(obj);
        }

        private bool CanAddNewGrp()
        {
            return true;
        }

        private void AddNewGrpExecute()
        {
            int newGroupNum = GetNewGrpNum();

            SearchItems.Add(new PrsSearchObj() { GrpCdn = newGroupNum });
        }

        private bool CanAddCdnToGrp(object arg)
        {
            return true;
        }

        private void AddCdnToGrpExecute(object obj)
        {
            CollectionViewGroup grp = obj as CollectionViewGroup;
            SearchItems.Add(new PrsSearchObj() { GrpCdn = (grp.Items[0] as PrsSearchObj).GrpCdn });
        }

        /// <summary>
        /// این تابع شماره گروه جدید شرط را پیدا میکند
        /// </summary>
        /// <returns></returns>
        private int GetNewGrpNum()
        {
            int maxNum = 0;

            foreach (var item in SearchItems)
            {
                maxNum = Math.Max(item.GrpCdn, maxNum);
            }

            return maxNum + 1;
        }

        protected override void OKExecute()
        {
            base.OKExecute();
            FilterResult = new List<TblPr>();

            if (SearchItemsCV.Groups.Count == 0)
            {
                FilterResult = null;
            }
            foreach (var item in SearchItemsCV.Groups)
            {
                CollectionViewGroup grp = item as System.Windows.Data.CollectionViewGroup;
                List<TblPr> grpResult = new List<TblPr>();
                int count = 0;
                foreach (var g in grp.Items)
                {
                    PrsSearchObj so = g as PrsSearchObj;
                    if (count == 0)
                    {
                        grpResult.AddRange(ApplySearchObj(so));
                    }
                    else
                    {
                        grpResult = new List<TblPr>(grpResult.Intersect(ApplySearchObj(so)));
                        if (grpResult.Count == 0)
                        {
                            break;
                        }
                    }

                    count++;
                }

                FilterResult = new List<TblPr>(FilterResult.Union(grpResult));
            }

            RaisePropertyChanged("FilterResult");
        }

        private List<TblPr> ApplySearchObj(PrsSearchObj so)
        {
            List<TblPr> prs = new List<TblPr>();


            if (so.SearchType.GetType() == typeof(string) && string.IsNullOrEmpty(so.SearchType.ToString()))
            {
                return prs;
            }
            switch (so.SearchType)
            {
                case SearchObjType.Act:
                    prs = PublicMethods.DetectPrsByActs_1774(context, PublicMethods.DetectActByName_1786(context, so.SearchString.ToString()));
                    break;
                case SearchObjType.Nod:
                    prs = PublicMethods.DetectPrsByNod_1736(context, so.SearchString.ToString());
                    break;
                case SearchObjType.Prs:
                    prs = PublicMethods.DetectPrsByName_1777(context, so.SearchString.ToString());
                    break;
                case SearchObjType.EvtSrt:
                    prs = PublicMethods.DetectPrsByTypEvtSrt_1885(context, ((KeyValuePair<EvtSrtType, string>)so.SearchString).Key);
                    break;
                case SearchObjType.EvtRst:
                    prs = PublicMethods.DetectPrsByTypEvtRst_1886(context, ((KeyValuePair<EvtRstType, string>)so.SearchString).Key);
                    break;
                case SearchObjType.InOut:
                    prs = PublicMethods.DetectPrsByObjName_1778(context, so.SearchString.ToString());
                    break;
                case SearchObjType.News:
                    prs = PublicMethods.DetectPrsByNewsName_1782(context, so.SearchString.ToString());
                    break;
                default:
                    break;
            }

            return prs;
        }

        #endregion
    }
}
