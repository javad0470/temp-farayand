using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SSYM.OrgDsn.Model;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcErorViewModel : PopupViewModel
    {
        #region ' Fields '

        //private EvtRstErorViewModel parent;
        private TblEror selectedItem;
        private ObservableCollection<TblEror> selectedItems;
        ObservableCollection<TblEror> allErors;


        #endregion

        #region ' Initialaizer '

        public SlcErorViewModel()
            : base(new BPMNDBEntities())
        {
            ErrorDoesntExistCommand = new DelegateCommand(ExecuteErrorDoesntExistCommand);

            DetectAllErors();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// all errors
        /// </summary>
        public ObservableCollection<TblEror> AllErors
        {
            get { return allErors; }
            set
            {
                allErors = value;

                RaisePropertyChanged("AllErors");
            }
        }

        /// <summary>
        /// IsSelectionModeSingle
        /// </summary>
        public bool IsSelectionModeSingle { get; set; }

        /// <summary>
        /// SelectedItems
        /// </summary>
        public ObservableCollection<TblEror> SelectedItems
        {
            get
            {
                if (selectedItems == null)
                {
                    selectedItems = new ObservableCollection<TblEror>();
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
        /// selected item
        /// </summary>
        public TblEror SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }

        /// <summary>
        /// ErrorDoesntExistCommand
        /// </summary>
        public ICommand ErrorDoesntExistCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی خطاهای ثبت شده تا کنون
        /// </summary>
        public void DetectAllErors(List<TblEror> excludeRange = null)
        {
            var data = this.bpmnEty.TblErors.ToList();

            data.ForEach(s =>
            {
                s.PropertyChanged -= err_PropChanged;
                s.PropertyChanged += err_PropChanged;
                s.IsSelected = false;
            });

            if (excludeRange != null && excludeRange.Count > 0)
            {
                SelectedItems.Clear();

                foreach (var item in excludeRange)
                {
                    var err = data.SingleOrDefault(s => s.FldCodEror == item.FldCodEror);
                    if (err != null)
                    {
                        err.IsSelected = true;
                    }
                }
            }

            this.AllErors = new ObservableCollection<TblEror>(data);
        }


        #endregion

        #region ' Private Methods '

        ///// <summary>
        ///// ExecuteOkCommand
        ///// </summary>
        //private void ExecuteOkCommand()
        //{
        //    if (Parent.TblEvtRst.TblErors.Where(E => E.FldCodEror == SelectedItem.FldCodEror).Count() == 0)
        //    {
        //        Parent.TblEvtRst.TblErors.Add(SelectedItem);
        //    }
        //    Parent.IsSelectErorPopupOpen = false;
        //}

        ///// <summary>
        ///// ExecuteCancelCommand
        ///// </summary>
        //private void ExecuteCancelCommand()
        //{
        //    Parent.IsSelectErorPopupOpen = false;
        //}

        /// <summary>
        /// ExecuteErrorDoesntExistCommand
        /// </summary>
        private void ExecuteErrorDoesntExistCommand()
        {
            //Parent.IsSelectErorPopupOpen = false;
            //Parent.ErrorDoesntExist = true;

            this.Result = PopupResult.Yes;
        }

        private void err_PropChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                var err = (sender as TblEror);
                if (err.IsSelected)
                {
                    if (!SelectedItems.Any(s => s.FldCodEror == err.FldCodEror))
                    {
                        SelectedItems.Add(err);
                    }
                }
                else
                {
                    if (SelectedItems.Any(s => s.FldCodEror == err.FldCodEror))
                    {
                        SelectedItems.Remove(err);
                    }
                }
            }
        }

        #endregion

        #region ' events '

        #endregion

    }
}
