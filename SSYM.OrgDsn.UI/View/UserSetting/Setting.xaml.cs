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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.UserSetting
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        public Setting()
        {
            InitializeComponent();
        }

        private ToggleButton lastChecked = null;
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (lastChecked != null) lastChecked.IsChecked = false;
            SettingTitle.Text = (sender as ToggleButton).ToolTip.ToString();
            (this.FindResource("SettingPages_Enter") as Storyboard).Begin();
            lastChecked = sender as ToggleButton;
        }
    }
}
