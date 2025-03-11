using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefOutViewModel : PopupViewModel
    {
        #region ' Fields '

        //private DtlOutViewModel parent;
        //private bool isSelectSourceEnabel;
        //private string outputName;
        private bool isSlcSrcAndDstOpen;
        private string performerName;
        private int destinationActivityId;



        #endregion

        #region ' Initialaizer '

        public DefOutViewModel()
            : base(new BPMNDBEntities())
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            //this.Parent = parent;
            //InsertOutputCommand = new DelegateCommand(ExecuteInsertOutputCommand);
            //CancelCommand = new DelegateCommand(ExecuteCancelCommand);
            SlcSrcAndDstCommand = new DelegateCommand(ExecuteSlcSrcAndDstCommand);
            SlcSrcAndDst = new SlcSrcAndDstViewModel();
            this.TblObj = new TblObj();
            this.TblObj.PropertyChanged += TblObj_PropertyChanged;
        }

        void TblObj_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldNamObj")
            {
                RaiseOKCanExecute();
            }
        }



        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// TblObj
        /// </summary>
        public Model.TblObj TblObj { get; set; }

        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

       

        /// <summary>
        /// SlcSrcAndDstCommand
        /// </summary>
        public ICommand SlcSrcAndDstCommand
        {
            get;
            set;
        }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcSrcAndDst.Dispose();
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// ExecuteSlcSrcAndDstCommand
        /// </summary>
        private void ExecuteSlcSrcAndDstCommand()
        {
            SlcSrcAndDst.IsSelectionModeSingle = true;
            //IsSlcSrcAndDstOpen = true;
        }

        /// <summary>
        /// CanOKExecute
        /// </summary>
        /// <returns>bool</returns>
        protected override bool CanOKExecute()
        {
            return !this.TblObj.HasErrors;
        }


        #endregion
    }
}
