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
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl;


namespace SSYM.OrgDsn.UI.View.EntityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcPsnIsdOrg.xaml
    /// </summary>
    public partial class SlcPsnIsdOrg : UserControl
    {
        public SlcPsnIsdOrg()
        {
            InitializeComponent();
        }

        private void MyDataGrid_RowDoubleClick(object sender, DataGridRowEventArgs e)
        {
            (this.DataContext as SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.PsnIsdOrgViewModel).OKCommand.Execute(e.Row.DataContext);
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((this.DataContext as PsnIsdOrgViewModel) != null)
            {
                foreach (TblPsn i in e.AddedItems)
                {
                    (this.DataContext as PsnIsdOrgViewModel).AddSelectedPsnIsdOrg(i);

                }
                foreach (TblPsn i in e.RemovedItems)
                {
                    (this.DataContext as PsnIsdOrgViewModel).RemoveSelectedPsnIsdOrg(i);
                }
            }
            
        }
    }
}
