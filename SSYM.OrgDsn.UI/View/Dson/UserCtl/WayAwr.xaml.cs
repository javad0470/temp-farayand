using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Dson;
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

namespace SSYM.OrgDsn.UI.View.Dson.UserCtl
{
    /// <summary>
    /// Interaction logic for WayAwr.xaml
    /// </summary>
    public partial class WayAwr : UserControl
    {
        public WayAwr()
        {
            InitializeComponent();
        }

        private void InputDrop(object sender, DragEventArgs e)
        {
            TblEvtSrt evtSrt = (sender as UIEntity).DataContext as TblEvtSrt;
            IWayAwr wayAwr = (IWayAwr)e.Data.GetData(typeof(SSYM.OrgDsn.Model.Base.IWayAwrIfrm));

            (this.DataContext as WayAwrViewModel).AddWayAwrToEvtSrt(evtSrt, wayAwr);
        }

        private void DeleteCommand_Click(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as UIEntity).DeleteCommand.Execute(sender);
            //(this.DataContext as WayAwrViewModel).DeleteWayAwr((sender as UIEntity).DataContext as IWayAwr);
        }
    }
}
