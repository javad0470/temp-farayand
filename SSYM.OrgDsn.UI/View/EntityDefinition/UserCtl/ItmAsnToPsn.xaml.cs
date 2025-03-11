using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl;
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
using Telerik.Windows.DragDrop;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for ItmAsnToPsn.xaml
    /// </summary>
    public partial class ItmAsnToPsn : UserControl
    {
        public ItmAsnToPsn(object datacontext)
        {
            this.DataContext = datacontext;
            InitializeComponent();
            lstbxAsnedItms.Drop += lstbxAsnedItms_Drop;
        }

        void lstbxAsnedItms_Drop(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                ContentPresenter data = (ContentPresenter)e.Data.GetData(typeof(ContentPresenter));
                TblRol rol = (TblRol)data.Content;
                (this.DataContext as ItmAsnToPsnViewModel).AssignRolToPsn(rol);
            }
            catch (Exception)
            {
                Node node = (Node)(e.Data.GetData(typeof(Telerik.Windows.Controls.RadDiagramShape)) as Telerik.Windows.Controls.RadDiagramShape).Content;
                (this.DataContext as ItmAsnToPsnViewModel).AssignPosPstToPsn(node.CurrentNode as TblPosPstOrg);
            }
            //_Drop
        }

    }
}
