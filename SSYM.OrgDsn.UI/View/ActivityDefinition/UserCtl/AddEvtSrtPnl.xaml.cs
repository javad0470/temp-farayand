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
using SSYM.OrgDsn.Model;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for AddEvtSrtPnl.xaml
    /// </summary>
    public partial class AddEvtSrtPnl : UserControl
    {
        public AddEvtSrtPnl()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(InSgmtTim, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(InSgmtTim, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(AftrCdn, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(AftrCdn, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(AftrAwr, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(AftrAwr, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(AnyCdnAftrActPvs, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(AnyCdnAftrActPvs, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(CdnSpcAftrActPvs, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(CdnSpcAftrActPvs, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(ErorInActPvs, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(ErorInActPvs, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(CnlActPvs, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(CnlActPvs, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(CdnSpcInTimImpActPvs, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(CdnSpcInTimImpActPvs, OnGiveFeedback);
        }

        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.All;
            BackgroundedImage img = (BackgroundedImage)args.OriginalSource;
            BackgroundedImage imgCursur = new BackgroundedImage() { Source = img.Source, Stretch = Stretch.Fill, Width = 48, Height = 48, IsCircle = true };
            args.DragVisual = new ContentControl() { Content = imgCursur };
            DataObject data = new DataObject(System.Windows.DataFormats.Text.ToString(), (args.OriginalSource as BackgroundedImage).Tag.ToString());
            args.Data = data;
        }

        private void InSgmtTim_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PublicMethods.IsDraging = true;
        }

        private void InSgmtTim_MouseUp(object sender, MouseButtonEventArgs e)
        {
            PublicMethods.IsDraging = false;
        }
    }
}
