using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.View.ActivityDefinition.Main;
using SSYM.OrgDsn.UI.View.Dson;
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
using Telerik.Windows.Controls;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for OrgPos.xaml
    /// </summary>
    public partial class OrgPos : UserControl
    {
        public OrgPos()
        {
            InitializeComponent();
        }



        private void hideCallout(object sender, RoutedEventArgs e)
        {
            callOut.Visibility = System.Windows.Visibility.Hidden;
        }
        
        private void setWidthBinding(ToggleButton btn, string param)
        {
            Binding b = new Binding("Width");
            b.ElementName = "grdBtns";
            b.Path = new PropertyPath("ActualWidth");
            b.Converter = new SSYM.OrgDsn.Converter.SizeConverter();
            b.ConverterParameter = param;
            btn.SetBinding(ToggleButton.WidthProperty, b);
        }


        public Visibility RadiosVisible
        {
            get
            {
                return grdBtns.Visibility;
            }
            set
            {
                grdBtns.Visibility = value;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if(this.ParentOfType<DefAct>() != null) this.ParentOfType<DefAct>().actLst.dataGrid1.Columns[1].Header = (sender as RadioButton).Content;
            //else if (this.ParentOfType<DsonList>() != null) this.ParentOfType<DsonList>()..dataGrid1.Columns[1].Header = (sender as RadioButton).Content;
        }
    }
}
