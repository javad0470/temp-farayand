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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for BasePopup.xaml
    /// </summary>
    public partial class BasePopup : UserControl
    {
        public BasePopup()
        {
            InitializeComponent();
        }

        private System.Windows.Controls.Primitives.Popup FindFirstPopup()
        {
            FrameworkElement current = this;

            if (current.Parent == null)
            {
                return null;
            }

            while (current.Parent != null && current.Parent.GetType() != typeof(System.Windows.Controls.Primitives.Popup))
            {
                current = current.Parent as FrameworkElement;
            }

            if (current.Parent == null)
            {
                return null;
            }
            return current.Parent as System.Windows.Controls.Primitives.Popup;
        }

        public void CloseParentPopup()
        {
            var p = FindFirstPopup();

            if (p != null)
            {
                p.IsOpen = false;
            }
        }


        public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register("PopupContent",
                typeof(UserControl), typeof(BasePopup), new FrameworkPropertyMetadata(null, callback)
               );

        private static void callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        
        public UserControl PopupContent
        {
            set { SetValue(PopupContentProperty, value); }
            get { return (UserControl)GetValue(PopupContentProperty); }
        }

    }
}
