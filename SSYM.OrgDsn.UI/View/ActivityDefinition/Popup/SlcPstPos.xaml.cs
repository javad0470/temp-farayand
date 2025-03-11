using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcPstPos.xaml
    /// </summary>
    public partial class SlcPstPos : Base.BasePopup
    {
        public SlcPstPos()
        {
            InitializeComponent();
        }

        private void MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (trvPosPstOrgs.DataContext as SlcPstPosViewModel).OK();
                //Telerik.Windows.Controls.RadTreeViewItem r;
            }
        }

    }
}
