using SSYM.OrgDsn.Model.Base;
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

namespace SSYM.OrgDsn.UI.View.Dson.UserCtl
{
    /// <summary>
    /// Interaction logic for InOutIcn.xaml
    /// </summary>
    public partial class InOutIcn : UserControl
    {
        public InOutIcn()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(_imgIcn, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(_imgIcn, OnGiveFeedback);

        }

        public ImageSource ImgIcn
        {
            get
            {
                return _imgIcn.Source;
            }
            set
            {
                _imgIcn.Source = value;
            }
        }

        public string LblName
        {
            get
            {
                return _txtblockLbl.Text;
            }
            set
            {
                _txtblockLbl.Text = value;
            }
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
            BackgroundedImage imgCursur = new BackgroundedImage() { Source = img.Source, Stretch = Stretch.Fill, Width = 48, Height = 48 };
            args.DragVisual = new ContentControl() { Content = imgCursur };
            DataObject data = new DataObject(typeof(IWayAwrIfrm), this.DataContext);
            args.Data = data;
        }

    }
}
