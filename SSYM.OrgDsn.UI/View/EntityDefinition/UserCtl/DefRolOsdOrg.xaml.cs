using SSYM.OrgDsn.Model;
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
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for DefRol.xaml
    /// </summary>
    public partial class DefRolOsdOrg : UserControl
    {
        public DefRolOsdOrg()
        {
            InitializeComponent();

            //DragDropManager.AddDragInitializeHandler(OutsideOrgs, OnDragInitialize);

            //DragDropManager.AddGiveFeedbackHandler(OutsideOrgs, OnGiveFeedback);

            //DragDropManager.AddDragInitializeHandler(OutsidePsns, OnDragInitialize);

            //DragDropManager.AddGiveFeedbackHandler(OutsidePsns, OnGiveFeedback);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.Copy;

            var border = ((ItemsControl)sender).Template.LoadContent() as Border;

            border.DataContext = (args.OriginalSource as Grid).DataContext;

            args.DragVisual = new ContentControl() { ContentTemplate = ((ItemsControl)sender).ItemTemplate, Content = border };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;
        }

        //private void selectOrg(object sender, MouseButtonEventArgs e)
        //{
        //    foreach (var item in OutsideOrgs.Items)
        //    {
        //        (item as TblOrg).IsSelected = false;
        //    }
        //    var org = (sender as FrameworkElement).DataContext as TblOrg;

        //    org.IsSelected = true;

        //    (this.DataContext as DefRolOsdViewModel).SelectedOrg = org;
        //}

        //private void selectPsn(object sender, MouseButtonEventArgs e)
        //{
        //    foreach (var item in OutsidePsns.Items)
        //    {
        //        (item as TblPsn).IsSelected = false;
        //    }
        //    var psn = (sender as FrameworkElement).DataContext as TblPsn;

        //    psn.IsSelected = true;

        //    (this.DataContext as DefRolOsdViewModel).SelectedPsn = psn;
        //}
       
    }
}
