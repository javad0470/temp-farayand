using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace SSYM.OrgDsn.ViewModel.Base
{

    public delegate void PopupResultChangedHandler(PopupViewModel sender, PopupResult newResult);

    public enum PopupResult
    {
        OK,
        Cancel,
        Yes,
        No
    }

    public class PopupViewModel : BaseViewModel, IDisposable
    {

        #region ' Fields '

        protected BPMNDBEntities bpmnEty;

        bool _commandBarVisible = true;
        bool _cancelVisible = true;  

        public static double SmallWidth = 400;
        public static double SmallHeight = 150;


        public static double MidWidth = 400;
        public static double MidHeight = 250;

        public static double LargeWidth = 400;
        public static double LargeHeight = 450;

        #endregion

        #region ' Initialaizer '

        public PopupViewModel()
        {
            Width = LargeWidth;
            Height = LargeHeight;
            this._result = PopupResult.Cancel;
            OKCommand = new DelegateCommand(OKExecute, CanOKExecute);
            (OKCommand as DelegateCommand).CanExecuteChanged += PopupViewModel_CanExecuteChanged;
            CancelCommand = new DelegateCommand(CancelExecute);
        }

        public PopupViewModel(BPMNDBEntities context)
        {
            Width = LargeWidth;
            Height = LargeHeight;

            this._result = PopupResult.Cancel;
            bpmnEty = context;
            OKCommand = new DelegateCommand(OKExecute, CanOKExecute);
            (OKCommand as DelegateCommand).CanExecuteChanged += PopupViewModel_CanExecuteChanged;
            CancelCommand = new DelegateCommand(CancelExecute);
        }

        void PopupViewModel_CanExecuteChanged(object sender, EventArgs e)
        {
        }

        #endregion

        #region ' Properties / Commands '

        public bool CommandBarVisible
        {
            get { return _commandBarVisible; }
            set
            {
                _commandBarVisible = value;
                RaisePropertyChanged("CommandBarVisible");
            }
        }

        /// <summary>
        /// confirm changes
        /// </summary>
        public ICommand OKCommand { get; set; }

        /// <summary>
        /// cancel changes
        /// </summary>
        public ICommand CancelCommand { get; set; }

        //public PopupResult Result { get; set; }

        PopupResult _result;

        public PopupResult Result
        {
            get { return _result; }
            set
            {
                _result = value;
                if (ResultChanged != null)
                {
                    ResultChanged(this, _result);
                }
            }
        }

        public bool OKEnabled
        {
            get
            {
                return this.CanOKExecute();
            }
        }


        public double Width { get; set; }

        public double Height { get; set; }
        #endregion

        #region ' Public Methods '

        public void ResetResult()
        {
            this._result = PopupResult.Cancel;
        }

        public bool CancelVisible
        {
            get { return _cancelVisible; }
            set
            {
                _cancelVisible = value;
                RaisePropertyChanged("CancelVisible");
            }
        }


        #endregion

        #region ' Private Methods '

        protected virtual bool CanOKExecute()
        {
            return true;
        }

        protected virtual void OKExecute()
        {
            Result = PopupResult.OK;
        }

        protected virtual void CancelExecute()
        {
            Result = PopupResult.Cancel;
        }

        protected void RaiseOKCanExecute()
        {
            RaisePropertyChanged("OKEnabled");
            (this.OKCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        #endregion

        #region ' events '

        public event PopupResultChangedHandler ResultChanged;

        #endregion


        public virtual void Dispose()
        {
            this.bpmnEty.Dispose();
        }

        List<TblPosPstOrg> _allItems;
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
        void p_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelectedInTree")
            {
                //if (tbl != null)
                //{
                //    TblPosPstSelectionChanged(sender as TblPosPstOrg);
                //}
            }
        }
    }

    public static class DispatcherTestHelper
    {
        private static DispatcherOperationCallback exitFrameCallback = ExitFrame;

        /// <summary>
        /// Synchronously processes all work items in the current dispatcher queue.
        /// </summary>
        /// <param name="minimumPriority">
        /// The minimum priority. 
        /// All work items of equal or higher priority will be processed.
        /// </param>
        public static void ProcessWorkItems(DispatcherPriority minimumPriority)
        {
            var frame = new DispatcherFrame();

            // Queue a work item.
            Dispatcher.CurrentDispatcher.BeginInvoke(
                minimumPriority, exitFrameCallback, frame);

            // Force the work item to run.
            // All queued work items of equal or higher priority will be run first. 
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object state)
        {
            var frame = (DispatcherFrame)state;

            // Stops processing of work items, causing PushFrame to return.
            frame.Continue = false;
            return null;
        }
    }
}
