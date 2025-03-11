using SSYM.OrgDsn.UI.View.Admin;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SSYM.OrgDsn.UI.View.Base
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {

        System.Timers.Timer tmr;

        public PopupWindow()
        {
            InitializeComponent();
            //this.ShowCloseButton = false;
            //this.ShowMaxRestoreButton = false;
            //this.ShowMinButton = false;
            this.Loaded += PopupWindow_Loaded;
            //this.clos
        }

        void PopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tmr = new System.Timers.Timer(1);

            tmr.Elapsed += tmr_Elapsed;

            tmr.Start();
        }

        void tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmr.Stop();
            Application.Current.Dispatcher.BeginInvoke(
  DispatcherPriority.Background,
  new Action(() =>
  {
      this.InvalidateArrange();
      this.InvalidateMeasure();
      this.InvalidateVisual();
  }));

            //Dispatcher.Invoke(new EventHandler(TimerWorkItem));
            //        tmr.Stop();
        }

        private void TimerWorkItem(object sender, EventArgs e)
        {

        }

        public PopupViewModel PopupContext
        {
            get
            {
                if (this.basePopup.PopupContent != null)
                {
                    return this.basePopup.PopupContent.DataContext as PopupViewModel;
                }

                return null;
            }
        }

        public UserControl PopupContent
        {
            get
            {
                return this.basePopup.PopupContent;
            }
            set
            {
                this.basePopup.PopupContent = value;
                if (this.basePopup != null)
                {
                    PopupContext.ResultChanged -= PopupContext_ResultChanged;
                    PopupContext.ResultChanged += PopupContext_ResultChanged;
                    this.DataContext = this.basePopup.PopupContent.DataContext;
                }
            }
        }

        void PopupContext_ResultChanged(PopupViewModel sender, PopupResult newResult)
        {
            //if (newResult == PopupResult.OK)
            //{

            //}

            this.Close();
        }

        //void basePopup_OnOK(object sender, EventArgs e)
        //{
        //    this.DialogResult = true;
        //    this.Close();
        //    //throw new NotImplementedException();
        //}

        //void basePopup_OnCancel(object sender, EventArgs e)
        //{
        //    this.DialogResult = false;
        //    this.Close();
        //    //throw new NotImplementedException();
        //}

        private void UIElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
