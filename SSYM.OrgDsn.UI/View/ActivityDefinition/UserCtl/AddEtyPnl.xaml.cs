using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for AddEtyPnl.xaml
    /// </summary>
    public partial class AddEtyPnl : UserControl
    {
        public AddEtyPnl()
        {
            InitializeComponent();
            DragDropManager.AddDragInitializeHandler(imgAct, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(imgAct, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(imgEvtSrt, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(imgEvtSrt, OnGiveFeedback);

            DragDropManager.AddDragInitializeHandler(imgEvtRst, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(imgEvtRst, OnGiveFeedback);


        }

        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.All;
            Image img = (Image)args.OriginalSource;
            Image imgCursur = new Image() { Source = img.Source, Stretch = Stretch.None };
            args.DragVisual = new ContentControl() { Content = imgCursur };
            DataObject data = new DataObject(System.Windows.DataFormats.Text.ToString(), (args.OriginalSource as Image).Tag.ToString());
            args.Data = data;
        }

        Point _startPoint;
        bool IsDragging;

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void Image_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);

                }
            }
        }


        private void StartDrag(MouseEventArgs e)
        {
            IsDragging = true;
            DataObject data = new DataObject(System.Windows.DataFormats.Text.ToString(), (e.OriginalSource as Image).Tag.ToString());
            DragDropEffects de = DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            IsDragging = false;
        }
    }
}
