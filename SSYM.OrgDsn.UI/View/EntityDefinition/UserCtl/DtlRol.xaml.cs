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

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for DtlRol.xaml
    /// </summary>
    public partial class DtlRol : UserControl
    {
        public DtlRol()
        {
            InitializeComponent();
        }

        static DtlRol()
        {
            AgntVisibilityProperty = DependencyProperty.Register("AgntVisibility", typeof(Visibility), typeof(DtlRol), new FrameworkPropertyMetadata(Visibility.Collapsed));
        }


        public static readonly DependencyProperty AgntVisibilityProperty;

        public Visibility AgntVisibility
        {
            set { SetValue(AgntVisibilityProperty, value); }
            get { return (Visibility)GetValue(AgntVisibilityProperty); }
        }

    }
}
