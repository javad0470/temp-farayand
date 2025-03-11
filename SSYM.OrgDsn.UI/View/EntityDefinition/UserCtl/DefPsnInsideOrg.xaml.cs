using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for DefPsnInsideOrg.xaml
    /// </summary>
    public partial class DefPsnInsideOrg : UserControl
    {

        public DefPsnInsideOrg()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(UserControl1_Loaded);
        }


        void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(this.ActualHeight.ToString());
        }

        //public void ShowWindow()
        //{
        //    ItmAsnToPsnViewModel vm = (this.DataContext as DefPsnInsideOrgViewModel).ItmAsnToPsnVM;
        //    ItmAsnToPsn view = new ItmAsnToPsn(vm);
        //    Window wnd = new Window();
        //    wnd.Content = view;
        //    wnd.ShowDialog();
        //}

    }
}
