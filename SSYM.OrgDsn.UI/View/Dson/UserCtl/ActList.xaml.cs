using SSYM.OrgDsn.Model;
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
    /// Interaction logic for ActList.xaml
    /// </summary>
    public partial class ActList : UserControl
    {
        public ActList()
        {
            InitializeComponent();
        }

        public TblAct SelectedAct
        {
            get { return (TblAct)GetValue(SelectedActProperty); }
            set
            {
                SetValue(SelectedActProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedActProperty =
        DependencyProperty.Register(
                "SelectedAct",
                typeof(TblAct),
                typeof(ActList)
        );

        private void rtvActList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAct = (TblAct)rtvActList.SelectedItem;
        }

    }
}
