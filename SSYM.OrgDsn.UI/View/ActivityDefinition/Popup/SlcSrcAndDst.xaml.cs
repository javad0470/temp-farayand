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
    /// Interaction logic for SlcSrcAndDst.xaml
    /// </summary>
    public partial class SlcSrcAndDst : Base.BasePopup
    {
        public SlcSrcAndDst()
        {
            InitializeComponent();
        }

        private void tabIsdSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl)
            {
                isd1.SelectedItem = null;
                isd2.SelectedItem = null;
                isd3.SelectedItem = null;

                osd1.SelectedItem = null;
                osd2.SelectedItem = null;
                osd3.SelectedItem = null;
                /*
                var tab = sender as TabControl;

                if (tab.SelectedIndex == 0)
                {
                    UIUtil.SelectFirstDataItem(isd1);
                }
                else if (tab.SelectedIndex == 1)
                {
                    UIUtil.SelectFirstDataItem(isd2);
                }
                else if (tab.SelectedIndex == 2)
                {
                    UIUtil.SelectFirstDataItem(isd3);
                }
                */
            }


        }

        private void tabOsdSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is TabControl)
            {
                isd1.SelectedItem = null;
                isd2.SelectedItem = null;
                isd3.SelectedItem = null;

                osd1.SelectedItem = null;
                osd2.SelectedItem = null;
                osd3.SelectedItem = null;
                /*var tab = sender as TabControl;

                if (tab.SelectedIndex == 0)
                {
                    UIUtil.SelectFirstDataItem(osd1);
                }
                else if (tab.SelectedIndex == 1)
                {
                    UIUtil.SelectFirstDataItem(osd2);
                }
                else if (tab.SelectedIndex == 2)
                {
                    UIUtil.SelectFirstDataItem(osd3);
                }
                */
            }

        }

        private void MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (this.DataContext as SlcSrcAndDstViewModel).OKCommand.Execute(this.DataContext);
            }
        }

        private void Isd2_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as SlcSrcAndDstViewModel).OKCommand.Execute(this.DataContext);
        }
    }
}
