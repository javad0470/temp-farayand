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
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for SlcActSrc.xaml
    /// </summary>
    public partial class SlcActsOfNod : Base.BasePopup
    {
        public SlcActsOfNod()
        {
            InitializeComponent();
        }

        private void BasePopup_Loaded(object sender, RoutedEventArgs e)
        {
            UIUtil.SelectFirstDataItem(dgrd1);
        }

        private void Dgrd1_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as SlcActOfNodViewModel).OKCommand.Execute(this.DataContext);
        }
    }
}
