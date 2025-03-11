using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefUntViewModel : PopupViewModel
    {
        #region ' Fields '

        //private string unitName;
        //private object parent;

        private Model.TblSbjMsrt selectedItem;

        private ObservableCollection<Model.TblSbjMsrt> tblSbjMsrt;

        string namUntMsrt;


        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parent"></param>
        public DefUntViewModel()
            : base(new BPMNDBEntities())
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

        }
        
        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public string NamUntMsrt
        {
            get { return namUntMsrt; }
            set
            {
                namUntMsrt = value;

                RaiseOKCanExecute();

                RaisePropertyChanged("NamUntMsrt");
            }
        }

        /// <summary>
        /// TblSbjMsrt
        /// </summary>
        public ObservableCollection<Model.TblSbjMsrt> TblSbjMsrt
        {
            get
            {
                //if (Parent.GetType() == typeof(DtlIntViewModel))
                //{
                //    DtlIntViewModel p = (DtlIntViewModel)Parent;
                //    return new ObservableCollection<Model.TblSbjMsrt>(p.bpmnEty.TblSbjMsrts);
                //}
                //if (Parent.GetType() == typeof(DtlOutViewModel))
                //{
                //    DtlOutViewModel p = (DtlOutViewModel)Parent;
                //    return new ObservableCollection<Model.TblSbjMsrt>(p.bpmnEty.TblSbjMsrts);
                //}
                //else
                //{
                //    return null;
                //}
                return new ObservableCollection<Model.TblSbjMsrt>(this.bpmnEty.TblSbjMsrts);

            }

            set { tblSbjMsrt = value; }
        }

        /// <summary>
        /// SelectedItem
        /// </summary>
        public Model.TblSbjMsrt SelectedItem
        {
            get { return selectedItem; }

            set
            {
                selectedItem = value;
                RaiseOKCanExecute();
            }
        }

        ///// <summary>
        ///// OkCommand
        ///// </summary>
        //public ICommand OkCommand { get; set; }

        ///// <summary>
        ///// CancelCommand
        ///// </summary>
        //public ICommand CancelCommand { get; set; }
        public Model.TblUntMsrt TblUntMsrt { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '
        
        protected override bool CanOKExecute()
        {
            if (this.SelectedItem != null && this.NamUntMsrt != null && this.NamUntMsrt != "")
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OKExecute()
        {
            base.OKExecute();

            this.TblUntMsrt = new TblUntMsrt();
            this.TblUntMsrt.FldNamUntMsrt = this.NamUntMsrt;
            this.TblUntMsrt.FldCodSbjMsrt = SelectedItem.FldCodSbjMsrt;
            this.bpmnEty.TblUntMsrts.AddObject(this.TblUntMsrt);
            PublicMethods.SaveContext(this.bpmnEty);
        }
        
        #endregion

        #region ' events '

        #endregion

    }
}
