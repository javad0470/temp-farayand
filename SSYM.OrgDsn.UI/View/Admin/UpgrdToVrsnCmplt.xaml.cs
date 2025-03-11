using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.Properties;
using SSYM.OrgDsn.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for UpgrdToVrsnCmplt.xaml
    /// </summary>
    public partial class UpgrdToVrsnCmplt : Window
    {

        TblInsOnnClnt _serial;

        public UpgrdToVrsnCmplt()
        {
            InitializeComponent();
            this.Loaded += UpgrdToVrsnCmplt_Loaded;
        }

        void UpgrdToVrsnCmplt_Loaded(object sender, RoutedEventArgs e)
        {
            _serial = Util.getNextSerial();

            tbkSerialNotFound.Visibility = System.Windows.Visibility.Collapsed;
            grdCheckSerialsInner.Visibility = System.Windows.Visibility.Collapsed;

            if (_serial != null)
            {
                txtSerial.Text = _serial.FldSeriInsl;
                txtActvnCod.Text = _serial.FldAcvnInsl;
                grdCheckSerialsInner.Visibility = System.Windows.Visibility.Visible;
                btncheckSerial.IsEnabled = true;
            }
            else
            {
                tbkSerialNotFound.Visibility = System.Windows.Visibility.Visible;
                btncheckSerial.IsEnabled = false;
            }
        }

        private void checkSerials_Click(object sender, RoutedEventArgs e)
        {
            if (_serial != null)
            {

                string loc = UIUtil.getCurrentLocation();

                ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.COMPELETE;

                CipherUtility.updateMyConfig("COMPELETE",loc);

                ViewModel.Util.ShowMessageBox(63);

                CipherUtility.SetAsUsed(_serial.FldSeriInsl,loc);

                CipherUtility.UpdateInstallState();

                this.DialogResult = true;

            }
            else
            {
                ViewModel.Util.ShowMessageBox(64);

                this.DialogResult = false;
            }


            this.Close();
        }
    }
}
