using SSYM.OrgDsn.ViewModel.Admin;
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

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for ActiveUsr.xaml
    /// </summary>
    public partial class ActiveUsr : UserControl
    {
        public ActiveUsr()
        {
            InitializeComponent();
        }
        private void pswrd2_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            //(this.DataContext as ActiveUsrViewModel).RepeatPassUsr = (sender as PasswordBox).Password;

        }

        private void pswrd_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            //(this.DataContext as ActiveUsrViewModel).OriginalPass = (sender as PasswordBox).Password;

        }
    }
}
