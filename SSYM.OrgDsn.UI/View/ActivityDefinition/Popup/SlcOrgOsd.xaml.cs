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
    /// Interaction logic for SlcOrgOut.xaml
    /// </summary>
    public partial class SlcOrgOsd : UserControl
    {
        public SlcOrgOsd()
        {
            InitializeComponent();
        }

        private void OutsideOrgs_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as SlcOrgOsdViewModel).OKCommand.Execute(this.DataContext);
        }
    }
}
