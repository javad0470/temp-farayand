using SSYM.OrgDsn.ViewModel.EntityDefinition.Popup;
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
using SSYM.OrgDsn.Model;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcNod.xaml
    /// </summary>
    public partial class SlcNod : UserControl
    {
        public SlcNod()
        {
            InitializeComponent();
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (trvNod.DataContext as SlcNodViewModel).OKCommand.Execute(trvNod.DataContext);
                //Telerik.Windows.Controls.RadTreeViewItem r;
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (trvNod.DataContext as SlcNodViewModel).OKCommand.Execute(trvNod.DataContext);
                //Telerik.Windows.Controls.RadTreeViewItem r;
            }
        }

        private void trvNod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as Telerik.Windows.Controls.RadTreeView)!=null && (sender as Telerik.Windows.Controls.RadTreeView).Name == "trvNod")
            {
                foreach (TblOrg item in e.AddedItems)
                {
                    (this.DataContext as SlcNodViewModel).addToSelectedItems(item);
                }
                foreach (TblOrg item in e.RemovedItems)
                {
                    (this.DataContext as SlcNodViewModel).deleteFromSelectedItems(item);
                }
            }
            else if ((sender as Telerik.Windows.Controls.RadTreeView) != null && (sender as Telerik.Windows.Controls.RadTreeView).Name == "treeView")
            {
                foreach (TblPosPstOrg item in e.AddedItems)
                {
                    (this.DataContext as SlcNodViewModel).addToSelectedItems(item);
                }
                foreach (TblPosPstOrg item in e.RemovedItems)
                {
                    (this.DataContext as SlcNodViewModel).deleteFromSelectedItems(item);
                }
            }
            else if ((sender as SSYM.OrgDsn.UI.MyDataGrid)!=null )
            {
                foreach (TblRol item in e.AddedItems)
                {
                    (this.DataContext as SlcNodViewModel).addToSelectedItems(item);
                }
                foreach (TblRol item in e.RemovedItems)
                {
                    (this.DataContext as SlcNodViewModel).deleteFromSelectedItems(item);
                }
            }
        }

        private void RolTree_OnRowDoubleClick(object sender, DataGridRowEventArgs e)
        {
            (trvNod.DataContext as SlcNodViewModel).OKCommand.Execute(trvNod.DataContext);
        }
    }
}
