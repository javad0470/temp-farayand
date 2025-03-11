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
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcSfw.xaml
    /// </summary>
    public partial class SlcSfw : Base.BasePopup
    {
        public SlcSfw()
        {
            InitializeComponent();
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            var chk = (sender as CheckBox);
            (chk.DataContext as SSYM.OrgDsn.Model.TblSfw).IsSelected = chk.IsChecked;
        }

        private void dgr_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as SlcSfwViewModel).OKCommand.Execute(this.DataContext);
        }
    }
}
