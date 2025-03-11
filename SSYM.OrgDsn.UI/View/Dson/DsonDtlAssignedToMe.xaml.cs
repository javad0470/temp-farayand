using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.UI.View.Dson.UserCtl;
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
using SSYM.OrgDsn.ViewModel.Dson;

namespace SSYM.OrgDsn.UI.View.Dson
{
    /// <summary>
    /// Interaction logic for ShwDsonDtl.xaml
    /// </summary>
    public partial class DsonDtlAssignedToMe : UserControl
    {
        public DsonDtlAssignedToMe()
        {
            InitializeComponent();
            this.Loaded += ShwDsonDtl_Loaded;
        }


        void ShwDsonDtl_Loaded(object sender, RoutedEventArgs e)
        {
            rdbFrst.IsChecked = true;
        }

        //private void dGrdActList_Loaded_1(object sender, RoutedEventArgs e)
        //{
        //    if (dGrdActList.Items.Count == 0)
        //    {
        //        dGrdActList.Width = 0;
        //    }
        //}

        private void rdbTrd_Checked(object sender, RoutedEventArgs e)
        {
            cmbActs.Visibility = System.Windows.Visibility.Visible;
            actCenter.HideActName();
            cmbActs.IsDropDownOpen = true;
        }

        private void rdbTrd_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbActs.Visibility = System.Windows.Visibility.Collapsed;
            actCenter.ShowActName();
            cmbActs.IsDropDownOpen = false;
        }
    }
}
