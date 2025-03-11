using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcRol.xaml
    /// </summary>
    public partial class SlcRol : Base.BasePopup
    {
        public SlcRol()
        {
            InitializeComponent();
            this.DataContextChanged += SlcRol_DataContextChanged;
        }

        void SlcRol_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                (this.DataContext as SlcRolViewModel).OK();
            }
        }

        private void MyDataGrid_RowDoubleClick(object sender, DataGridRowEventArgs e)
        {
            (this.DataContext as SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcRolViewModel).OKCommand.Execute(e.Row.DataContext);
        }

    }
}
