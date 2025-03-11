using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for ActLst.xaml
    /// </summary>
    public partial class ActLst : UserControl
    {
        public ActLst()
        {
            InitializeComponent();
        }

        private void DataGridCell_LostFocus(object sender, RoutedEventArgs e)
        {
            //_FocusedCell = sender as DataGridCell;
        }

        Object _SelectedItem; //_SelectedItem is used to avoid the repeated loops
        DataGridCell _FocusedCell; //_FocusedCell is used to restore focus


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext == null)
            {
                return;
            }
            if (_SelectedItem == dataGrid1.SelectedItem)
            {
                return;
            }

            _SelectedItem = dataGrid1.SelectedItem;


            if (!(this.DataContext as ActLstViewModel).CanChangeCurrentItem())
            {
                if (e.RemovedItems.Count > 0 && e.RemovedItems[0] != null)
                {
                    _SelectedItem = e.RemovedItems[0];
                    Dispatcher.BeginInvoke(
                      new Action(() =>
                      {
                          dataGrid1.SelectedItem = e.RemovedItems[0];
                          if (_FocusedCell != null)
                          {
                              _FocusedCell.Focus();
                          }
                      }), System.Windows.Threading.DispatcherPriority.Send); //hope a high priority can avoid flicking.
                }
            }
            else
            {
                (this.DataContext as ActLstViewModel).SelectedAct = dataGrid1.SelectedItem as TblAct;
            }
        }



    }
}
