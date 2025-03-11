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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for AddEvtSrtPnl.xaml
    /// </summary>
    public partial class AddEvtRstPnl : UserControl
    {
        public AddEvtRstPnl()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(evtRstAftrAnyCdnAftrAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstAftrAnyCdnAftrAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstCnlInTimImpAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstCnlInTimImpAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstErorInTimImpAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstErorInTimImpAct, OnGiveFeedback);

            //DragDropManager.AddDragInitializeHandler(evtRstGainAwrNewAftrAct, OnDragInitialize);
            //DragDropManager.AddGiveFeedbackHandler(evtRstGainAwrNewAftrAct, OnGiveFeedback);

            //DragDropManager.AddDragInitializeHandler(evtRstGainAwrNewInTimImpAct, OnDragInitialize);
            //DragDropManager.AddGiveFeedbackHandler(evtRstGainAwrNewInTimImpAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstSpcCdnAftrAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstSpcCdnAftrAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstSpcCdnInTimImpAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstSpcCdnInTimImpAct, OnGiveFeedback);
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
    }
}
