using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for DefLvlAcs.xaml
    /// </summary>
    public partial class DefLvlAcs : UserControl
    {
        public DefLvlAcs()
        {
            InitializeComponent();
        }


        void DefLvlAcs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                dgrdLvlAcs.SelectedItem = e.NewItems[0];
                dgrdLvlAcs.CurrentColumn = dgrdLvlAcs.Columns[0];
                dgrdLvlAcs.Focus();
                dgrdLvlAcs.BeginEdit();
                //(dgrdLvlAcs.CurrentCell.Item as DataGridCell).IsEditing = true;


                //dg.ItemsSource.Add(data);
                //dg.SelectedItem = data;                  //set SelectedItem to the new object
                //dg.ScrollIntoView(data, dg.Columns[0]);  //scroll row into view, for long lists, setting it to start with the first column
                //dg.Focus();                              //required in my case because contextmenu click was not setting focus back to datagrid
                //dg.BeginEdit();   
            }
        }

        private void RadToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void RadToggleButton_Checked_2(object sender, RoutedEventArgs e)
        {

        }

        private void orgToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (trvOrgAcs == null)
            {
                return;
            }
            RadToggleButton tb = sender as RadToggleButton;
            if (tb.IsChecked.Value)
            {
                trvOrgAcs.ExpandAll();
                tb.Content = "بستن همه";
            }
            else
            {
                trvOrgAcs.CollapseAll();
                tb.Content = "باز کردن همه";
            }
        }

        private void posToggleButton_Checked(object sender, RoutedEventArgs e)
        {

            if (trvPosPst == null)
            {
                return;
            }
            RadToggleButton tb = sender as RadToggleButton;
            if (tb.IsChecked.Value)
            {
                trvPosPst.ExpandAll();
                tb.Content = "بستن همه";
            }
            else
            {
                trvPosPst.CollapseAll();
                tb.Content = "باز کردن همه";
            }
        }

        private void rolToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (trvRolAcs == null)
            {
                return;
            }
            RadToggleButton tb = sender as RadToggleButton;
            if (tb.IsChecked.Value)
            {
                trvRolAcs.ExpandAll();
                tb.Content = "بستن همه";
            }
            else
            {
                trvRolAcs.CollapseAll();
                tb.Content = "باز کردن همه";
            }

        }

        private void dgrdLvlAcs_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.dgrdLvlAcs.ItemsSource != null)
            {
                (this.dgrdLvlAcs.ItemsSource as ObservableCollection<TblLvlAc>).CollectionChanged += DefLvlAcs_CollectionChanged;
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            var txt = sender as TextBox;

            txt.Text = (txt.DataContext as TblLvlAc).FldNam;

            //var cell = UIUtil.FindParent<DataGridCell>(txt);

            //Validation.ClearInvalid(txt.GetBindingExpression(TextBox.TextProperty));

            //cell.IsEditing = false;
        }
    }
}
