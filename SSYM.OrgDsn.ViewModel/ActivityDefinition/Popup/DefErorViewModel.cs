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
    public class DefErorViewModel : PopupViewModel
    {
        #region ' Fields '

        private ObservableCollection<Model.TblTypEror> tblTypEror;
        //private UserCtl.EvtRstErorViewModel parent;
        //private string errorName;
        private Model.TblTypEror selectedItem;

        #endregion

        #region ' Initialaizer '

        public DefErorViewModel()
            : base(new BPMNDBEntities())
        {
            this.TblEror = new TblEror();
            this.TblEror.PropertyChanged += TblEror_PropertyChanged;

            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;
        }

        void TblEror_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldNamEror")
            {
                RaiseOKCanExecute();
            }
        }

        //public DefErorViewModel()
        //{
        //OkCommand = new DelegateCommand(ExecuteOkCommand);
        //CancelCommand = new DelegateCommand(ExecuteCancelCommand);
        //}


        #endregion

        #region ' Properties / Commands '

        ///// <summary>
        ///// parent of this user control
        ///// </summary>
        //public UserCtl.EvtRstErorViewModel Parent
        //{
        //    get { return parent; }
        //    set
        //    {
        //        parent = value;
        //        RaisePropertyChanged("TblTypEror");
        //    }
        //}

        /// <summary>
        /// type of error
        /// </summary>
        public ObservableCollection<Model.TblTypEror> TblTypEror
        {
            get
            {
                //if (Parent != null)
                //{
                //    return new ObservableCollection<Model.TblTypEror>(Parent.bpmnEty.TblTypErors);
                //}
                //else
                //{
                //    return null;
                //}
                return new ObservableCollection<Model.TblTypEror>(this.bpmnEty.TblTypErors);
            }
            set { tblTypEror = value; }
        }

        ///// <summary>
        ///// ErrorName
        ///// </summary>
        //public string ErrorName
        //{
        //    get { return errorName; }
        //    set { errorName = value; }
        //}

        ///// <summary>
        ///// ok command
        ///// </summary>
        //public ICommand OkCommand { get; set; }

        ///// <summary>
        ///// CancelCommand
        ///// </summary>
        //public ICommand CancelCommand { get; set; }

        /// <summary>
        /// type of error
        /// </summary>
        public Model.TblTypEror SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// TblEror
        /// </summary>
        public TblEror TblEror { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        ///// <summary>
        ///// ExecuteOkCommand
        ///// </summary>
        //private void ExecuteOkCommand()
        //{
        //    Model.TblEror tbl=new Model.TblEror() { FldNamEror = this.ErrorName, FldCodTypEror=SelectedItem.FldCodTypEror };
        //    Parent.bpmnEty.TblErors.AddObject(tbl);
        //    Parent.TblEvtRst.TblErors.Add(tbl);
        //    Parent.ErrorDoesntExist = false;
        //}

        ///// <summary>
        ///// ExecuteCancelCommand
        ///// </summary>
        //private void ExecuteCancelCommand()
        //{
        //    Parent.ErrorDoesntExist = false;
        //}

        /// <summary>
        /// CanOKExecute
        /// </summary>
        /// <returns></returns>
        protected override bool CanOKExecute()
        {
            if (this.SelectedItem == null)
            {
                return false;
            }
            else
            {
                return !this.TblEror.HasErrors;
            }
        }

        #endregion

        #region ' events '

        #endregion

    }
}
