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
    /// Interaction logic for DtlInt.xaml
    /// </summary>
    public partial class DtlInt : UserControl
    {
        public DtlInt()
        {
            InitializeComponent();
        }

        private void txtIntPerRecv_TextInput_1(object sender, TextCompositionEventArgs e)
        {
            bool sw = false;

            foreach (char it in txtIntPerRecv.Text)
                if (it == '.')
                    sw = true;

            if (e.Text[e.Text.Length - 1] == '.')
            {
                if (sw == true)
                {
                    e.Handled = true;
                    return;
                }
                else

                    return;
            }


            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;

            }

        }
    }
}
