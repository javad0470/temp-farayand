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
    /// Interaction logic for SlcActDst.xaml
    /// </summary>
    public partial class SlcActDst : Base.BasePopup
    {
        public SlcActDst()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tab = sender as TabControl;

            if (tab.SelectedIndex == 0)
            {
                UIUtil.SelectFirstDataItem(dgrd1);
            }
            else if (tab.SelectedIndex == 1)
            {
                UIUtil.SelectFirstDataItem(dgrd2);
            }

        }

        private void Dgrd1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as SlcActDstViewModel).OKCommand.Execute(this.DataContext);
        }
    }
}
