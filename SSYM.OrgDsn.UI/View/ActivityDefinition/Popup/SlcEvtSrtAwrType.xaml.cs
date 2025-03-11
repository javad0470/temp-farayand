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

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for SlcEvtSrtAwrType.xaml
    /// </summary>
    public partial class SlcEvtSrtAwrType : UserControl
    {
        public SlcEvtSrtAwrType()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Selected = (sender as Button).DataContext;
            if (OnSelected != null)
            {
                OnSelected(this, e);
            }
            //if (SelectCommand != null)
            //{
            //    SelectCommand.Execute(CommandParameter = (sender as Button).DataContext);
            //}
        }

        //public static readonly DependencyProperty SelectCommandProperty =
        //    DependencyProperty.Register(
        //        "SelectCommand",
        //        typeof(ICommand),
        //        typeof(UIEntity)
        //    );

        //public ICommand SelectCommand
        //{
        //    get { return (ICommand)GetValue(SelectCommandProperty); }
        //    set { SetValue(SelectCommandProperty, value); }
        //}



        public event EventHandler OnSelected;

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register(
                "Selected",
                typeof(object),
                typeof(SlcEvtSrtAwrType)
        );

        public object Selected
        {
            get { return (object)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

    }
}
