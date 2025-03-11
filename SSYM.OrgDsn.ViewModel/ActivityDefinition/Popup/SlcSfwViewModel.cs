using System.Windows.Data;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcSfwViewModel : PopupViewModel
    {
        #region ' Fields '

        private ObservableCollection<TblSfw> _tblSfw;

        private bool isSelectionModeSingle;
        private Model.TblSfw selectedItem;
        private ObservableCollection<TblSfw> selectedItems;
        string _txtSrch;
        private ListCollectionView _sfwCV;

        #endregion

        #region ' Initialaizer '

        public SlcSfwViewModel()
            : base(new BPMNDBEntities())
        {
            SoftwareDoesnExistCommand = new DelegateCommand(ExecuteSoftwareDoesnExistCommand);
            SelectedItems = new ObservableCollection<TblSfw>();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// gets all softwares define in the current organization
        /// </summary>
        public ObservableCollection<TblSfw> TblSfw
        {
            get { return _tblSfw; }
            set { _tblSfw = value; }
        }


        /// <summary>
        /// data grid selection mode
        /// </summary>
        public bool IsSelectionModeSingle
        {
            get { return isSelectionModeSingle; }
            set
            {
                isSelectionModeSingle = value;
                RaisePropertyChanged("IsSelectionModeSingle");
            }
        }

        /// <summary>
        /// selected item in software data grid
        /// </summary>
        public Model.TblSfw SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }

        /// <summary>
        /// SelectedItems
        /// </summary>
        public ObservableCollection<TblSfw> SelectedItems
        {
            get
            {
                if (selectedItems == null)
                {
                    selectedItems = new ObservableCollection<TblSfw>();
                }
                return selectedItems;
            }
            set
            {
                selectedItems = value;

                RaisePropertyChanged("SelectedItems");
            }
        }

        /// <summary>
        /// SoftwareDoesnExistCommand
        /// </summary>
        public ICommand SoftwareDoesnExistCommand { get; set; }

        public ListCollectionView SfwCv
        {
            get
            {
                if (TblSfw == null)
                {
                    return null;
                }
                if (_sfwCV == null)
                {
                    _sfwCV = new ListCollectionView(TblSfw);
                    _sfwCV.Filter = new Predicate<object>(filterSfws);
                }
                return _sfwCV;
            }
            set { _sfwCV = value; }
        }

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                if (_txtSrch != value)
                {
                    _txtSrch = value;
                    SfwCv.Refresh();
                }
            }
        }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی نرم افزارها
        /// </summary>
        public void DetectAllSfws(List<TblSfw> excludeRange = null)
        {
            var data = this.bpmnEty.TblSfws.Where(m => m.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList();

            data.ForEach(s =>
            {
                s.PropertyChanged -= swf_PropChanged;
                s.PropertyChanged += swf_PropChanged;
                s.IsSelected = false;
            });

            if (excludeRange != null && excludeRange.Count > 0)
            {
                SelectedItems.Clear();

                foreach (var item in excludeRange)
                {
                    var sfw = data.SingleOrDefault(s => s.FldCodSfw == item.FldCodSfw);
                    if (sfw != null)
                    {
                        sfw.IsSelected = true;
                    }
                }
            }

            this.TblSfw = new ObservableCollection<TblSfw>(data);

            RaisePropertyChanged("TblSfw");
        }


        private void swf_PropChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                var sft = (sender as TblSfw);
                if (sft.IsSelected.Value)
                {
                    if (!SelectedItems.Any(s => s.FldCodSfw == sft.FldCodSfw))
                    {
                        SelectedItems.Add(sft);
                    }
                }
                else
                {
                    if (SelectedItems.Any(s => s.FldCodSfw == sft.FldCodSfw))
                    {
                        SelectedItems.Remove(sft);
                    }
                }
            }
        }

        #endregion

        #region ' Private Methods '
        private bool filterSfws(object obj)
        {
            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            var sfw = obj as TblSfw;

            if (sfw == null)
            {
                return true;
            }


            return sfw.Name.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
        }

        /// <summary>
        /// execute ok command
        /// </summary>
        protected override void OKExecute()
        {
            base.OKExecute();
        }

        /// <summary>
        /// execute cancel command
        /// </summary>
        protected override void CancelExecute()
        {
            base.CancelExecute();
        }

        /// <summary>
        /// ExecuteSoftwareDoesnExistCommand
        /// </summary>
        private void ExecuteSoftwareDoesnExistCommand()
        {
            this.Result = Base.PopupResult.Yes;
        }


        #endregion

    }
}
