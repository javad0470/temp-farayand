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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for DtlAct.xaml
    /// </summary>
    public partial class DtlAct : UserControl
    {
        public DtlAct()
        {
            InitializeComponent();
            this.Loaded += DtlAct_Loaded;
        }

        void DtlAct_Loaded(object sender, RoutedEventArgs e)
        {
            //App.ApplyTheme(new SolidColorBrush(Color.FromRgb(255,0,0)));
        }
    }
}
