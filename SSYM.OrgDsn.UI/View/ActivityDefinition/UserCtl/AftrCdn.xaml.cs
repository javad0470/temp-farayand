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
    /// Interaction logic for AftrCdn.xaml
    /// </summary>
    public partial class AftrCdn : UserControl
    {
        public AftrCdn()
        {
            InitializeComponent();

            Button b = new Button();

            //b.Click += b_Click;

            //SSYM.OrgDsn.UI.View.CustomControl.SchCbo sc = new CustomControl.SchCbo();

            //this.FindName();

            //sc.OnSearch += sc_OnSearch;

        }

        void sc_OnSearch(object sender, RoutedEventArgs e)
        {
            
        }

        public void b_Click(object sender, RoutedEventArgs e)
        {
        }

        public void test()
        {
        }

        private void srchCombo_OnSearch_1(object sender, RoutedEventArgs e)
        {

        }

        private void srchCombo_DataContextChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
        
    }
}
