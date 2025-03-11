using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public abstract class BaseReportSearchViewModel<T> : PopupViewModel, IReport
    {
        #region ' Fields '

        protected Model.BPMNDBEntities context = new Model.BPMNDBEntities();

        #endregion

        #region ' Initialaizer '

        public BaseReportSearchViewModel()
        {
            InitialaizeCommands();

            SearchItems = new ObservableCollection<SrchCdn<T>>();
        }

        private void InitialaizeCommands()
        {
            AddCdnToGrp = new DelegateCommand<object>(AddCdnToGrpExecute, CanAddCdnToGrp);
            AddNewGrp = new DelegateCommand(AddNewGrpExecute, CanAddNewGrp);
            DeleteCdn = new DelegateCommand<object>(DeleteSearchObj);
            OKCommand = new DelegateCommand(OKExecute, CanOK);
        }

        ~BaseReportSearchViewModel()
        {
            context.Dispose();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// چه نوعی جستجو میشود
        /// یکی از انواع 16 گانه جستجو
        /// </summary>
        public Type SearchType { get { return typeof(T); } }

        public ObservableCollection<SrchCdn<T>> SearchItems { get; set; }

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

        public ICommand OKCommand { get; set; }

        //public ICommand CancelCommand { get; set; }

        public DelegateCommand<object> AddCdnToGrp { get; set; }

        public DelegateCommand AddNewGrp { get; set; }

        public DelegateCommand<object> DeleteCdn { get; set; }

        public List<object> FilterResult { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private bool CanOK()
        {
            return true;
            //return SearchItems != null && SearchItems.Count > 0;
        }

        private void DeleteSearchObj(object obj)
        {
            SearchItems.Remove(obj as SrchCdn<T>);
        }

        private bool CanAddNewGrp()
        {
            return true;
        }

        private void AddNewGrpExecute()
        {
            int newGroupNum = GetNewGrpNum();

            SearchItems.Add(new SrchCdn<T>() { GrpCdn = newGroupNum });
        }

        private bool CanAddCdnToGrp(object arg)
        {
            return true;
        }

        private void AddCdnToGrpExecute(object obj)
        {
            CollectionViewGroup grp = obj as CollectionViewGroup;
            SearchItems.Add(new SrchCdn<T>() { GrpCdn = (grp.Items[0] as SrchCdn<T>).GrpCdn });
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
                maxNum = Math.Max((item as SrchCdn<T>).GrpCdn, maxNum);
            }

            return maxNum + 1;
        }

        private void OKExecute()
        {
            List<IQueryable> cdnGoups = new List<IQueryable>();

            if (SearchItems == null)
            {
                return;
            }


            List<SrchCdn<T>> sItems = SearchItems.ToList();

            int c = sItems.RemoveAll(m => (m.SelectedValueType == typeof(string) && string.IsNullOrEmpty(m.GetValue<string>())) ||
                (m.SelectedValueType == typeof(int) && m.GetValue<int>() == 0));

            if (sItems.Count == 0)
            {
                ReportResultCreated(this, new ReportEventArgs() { Data = ApplySearchCdn(null, null) });
                return;
            }


            var groups = sItems.GroupBy(m => m.GrpCdn);
            foreach (var grp in groups)
            {
                IQueryable grpResult = null;

                foreach (var item in grp)
                {
                    grpResult = ApplySearchCdn(item, grpResult);
                }

                // گروههای مختلف با هم اور
                cdnGoups.Add(grpResult);
            }

            ReportResultCreated(this, new ReportEventArgs() { Data = UnionAllGroups(cdnGoups) });
        }

        private void callback(IAsyncResult ar)
        {
            // throw new NotImplementedException();
        }

        protected virtual IQueryable ApplySearchCdn(SrchCdn<T> cdn, IQueryable prevQuery)
        {
            return null;
        }


        protected virtual IQueryable UnionAllGroups(List<IQueryable> cdnGoups)
        {
            return null;
        }

        #endregion

        #region ' Events '

        public event EventHandler<ReportEventArgs> ReportResultCreated;

        #endregion


        public abstract string ReportTitle
        {
            get;
        }
    }
}

