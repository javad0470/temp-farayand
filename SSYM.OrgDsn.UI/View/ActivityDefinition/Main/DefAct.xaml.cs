using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSYM.OrgDsn.Model;
using Telerik.Windows.DragDrop;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Main
{
    /// <summary>
    /// Interaction logic for DefAct.xaml
    /// </summary>
    public partial class DefAct : UserControl
    {
        public DefAct()
        {
            InitializeComponent();

            //Start Events
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

            //Result Events
            DragDropManager.AddDragInitializeHandler(evtRstAftrAnyCdnAftrAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstAftrAnyCdnAftrAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstCnlInTimImpAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstCnlInTimImpAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(evtRstErorInTimImpAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(evtRstErorInTimImpAct, OnGiveFeedback);

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
            Button btn = (Button)args.OriginalSource;
            Button btnCursor = new Button() { Style = btn.Style, Background = btn.Background, Foreground = btn.Foreground };
            args.DragVisual = new ContentControl() { Content = btnCursor };
            DataObject data = new DataObject(System.Windows.DataFormats.Text.ToString(), (args.OriginalSource as Button).Tag.ToString());
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
