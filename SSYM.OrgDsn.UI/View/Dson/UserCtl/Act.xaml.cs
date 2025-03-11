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
    /// Interaction logic for Act.xaml
    /// </summary>
    public partial class Act : UserControl
    {
        public Act()
        {
            InitializeComponent();
            this.DataContextChanged += Act_DataContextChanged;
        }

        void Act_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        public void ShowActName()
        {
            tblkActNam.Visibility = System.Windows.Visibility.Visible;
        }

        public void HideActName()
        {
            tblkActNam.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}
