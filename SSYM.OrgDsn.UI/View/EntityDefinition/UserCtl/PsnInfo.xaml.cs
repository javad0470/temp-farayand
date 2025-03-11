using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for PsnInfo.xaml
    /// </summary>
    public partial class PsnInfo : UserControl
    {
        public PsnInfo()
        {
            InitializeComponent();

            //PersianCalendar hijri = new PersianCalendar();

            ////Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");

            //CultureInfo current = CultureInfo.CurrentCulture;

            //current.DateTimeFormat.Calendar = hijri;

            //Thread.CurrentThread.CurrentCulture = current;
            //Thread.CurrentThread.CurrentUICulture = current;
        }
    }
}
