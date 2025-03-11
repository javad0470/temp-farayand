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
using SSYM.OrgDsn.ViewModel.UserSetting;
using SSYM.OrgDsn.UI.Utility;

namespace SSYM.OrgDsn.UI.View.UserSetting
{
    /// <summary>
    /// Interaction logic for UsrSetting.xaml
    /// </summary>
    public partial class UsrSetting : UserControl
    {
        public UsrSetting()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            //(this.DataContext as UsrSettingViewModel).PassOld = (sender as PasswordBox).Password;
            //var pass = sender as PasswordBox;
            //BindingExpression bePassword = pass.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            //if (bePassword != null) bePassword.UpdateSource();
        }

        private void PasswordBox_PasswordChanged_2(object sender, RoutedEventArgs e)
        {
            //(this.DataContext as UsrSettingViewModel).PassNew = (sender as PasswordBox).Password;
            //var pass = sender as PasswordBox;
            //BindingExpression bePassword = pass.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            //if (bePassword != null) bePassword.UpdateSource();

        }

        private void PasswordBox_PasswordChanged_3(object sender, RoutedEventArgs e)
        {
            //var pass = sender as PasswordBox;
            //BindingExpression bePassword = pass.GetBindingExpression(PasswordBoxAssistant.BoundPassword);
            //if (bePassword != null) bePassword.UpdateSource();
            //(this.DataContext as UsrSettingViewModel).RepeatNamUsr = pass.Password;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtUsername.Text == null)
            {
                (this.DataContext as UsrSettingViewModel).NamUsrNew = string.Empty;
            }
            else
            {
                (this.DataContext as UsrSettingViewModel).NamUsrNew = txtUsername.Text.Trim().ToLower();
            }
        }

        //private void TabControl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}
    }
}
