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
    /// Interaction logic for SlcPosPstRolPopup.xaml
    /// </summary>
    public partial class SlcOrgDep : Base.BasePopup
    {
        public SlcOrgDep()
        {
            InitializeComponent();
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (this.DataContext as SlcOrgDepViewModel).OKCommand.Execute(this.DataContext);
            }
        }
    }
}
