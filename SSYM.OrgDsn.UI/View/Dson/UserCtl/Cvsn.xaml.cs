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

namespace SSYM.OrgDsn.UI.View.Dson.UserCtl
{
    /// <summary>
    /// Interaction logic for Cnvs.xaml
    /// </summary>
    public partial class Cvsn : UserControl
    {
        public Cvsn()
        {
            InitializeComponent();
            this.DataContextChanged += Cvsn_DataContextChanged;
        }

        void Cvsn_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
