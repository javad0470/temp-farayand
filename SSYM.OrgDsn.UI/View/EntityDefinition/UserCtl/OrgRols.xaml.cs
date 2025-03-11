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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.DragDrop;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for OrgRols.xaml
    /// </summary>
    public partial class OrgRols : UserControl
    {
        public OrgRols()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(itmCtrlRols, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(itmCtrlRols, OnGiveFeedback);
            //DragDropManager.AddDragDropCompletedHandler(itmCtrlRols, OnDragCompleted);

        }



        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.All;
            //var payload = Telerik.Windows.DragDrop.DragDropPayloadManager.GeneratePayload(null);
            //payload.SetData("DragData", ((FrameworkElement)args.OriginalSource).DataContext);
            //args.Data = payload;
            //args.DragVisual = new ContentControl { Content = args.Data, ContentTemplate = LayoutRoot.Resources["ApplicationTemplate"] as DataTemplate };
            //args.DragVisual = new ContentControl() {ContentTemplate=shp.ContentTemplate, Content=shp.Content};
            args.DragVisual = new ContentControl() { ContentTemplate = ((ItemsControl)sender).ItemTemplate, Content = ((ItemsControl)sender).Template.LoadContent() };
        }


        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;

        }

        public void OnDragCompleted(object sender, Telerik.Windows.DragDrop.DragDropCompletedEventArgs args)
        {
            //((IList)(sender as ListBox).ItemsSource).Remove(args.Data);
        }

    }
}
