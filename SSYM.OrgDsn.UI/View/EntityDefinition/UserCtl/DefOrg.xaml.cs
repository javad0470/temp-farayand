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
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for DefOrg.xaml
    /// </summary>
    public partial class DefOrg : UserControl
    {
        public DefOrg()
        {
            InitializeComponent();
        }

        private void tbCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var vm = this.DataContext as SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.DefOrg;

                if (tbCtrl.SelectedIndex == 0)
                {
                    dtlOrg.AgntVisibility = System.Windows.Visibility.Visible;
                }

                if (tbCtrl.SelectedIndex == 1)
                {
                    dtlOrg.AgntVisibility = System.Windows.Visibility.Collapsed;
                    vm.SelectedSharedProfitOrg = dg1.SelectedItem as SSYM.OrgDsn.Model.TblOrg;
                }

                if (tbCtrl.SelectedIndex == 2)
                {
                    dtlOrg.AgntVisibility = System.Windows.Visibility.Collapsed;
                    vm.SelectedNonDepOrg = dg2.SelectedItem as SSYM.OrgDsn.Model.TblOrg;
                }

            }
            catch (Exception)
            {
            }
        }

        
    }
}
