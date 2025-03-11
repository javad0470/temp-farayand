using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for DataMgr.xaml
    /// </summary>
    public partial class DataMgr
    {
        public DataMgr()
        {
            InitializeComponent();

            this.Loaded += DataMgr_Loaded;
        }

        BPMNDBEntities ctx;

        List<TblBakAndRsr> _bakRstrs;

        void DataMgr_Loaded(object sender, RoutedEventArgs e)
        {
            ctx = new BPMNDBEntities();

            dtgrd1.ItemsSource = _bakRstrs = new List<TblBakAndRsr>(ctx.TblBakAndRsrs.ToList().Where(b => !b.FldTyp));

            cmbAction.SelectedIndex = 0;

            this.Loaded -= DataMgr_Loaded;
        }

        private void cmbAction_Selected_1(object sender, RoutedEventArgs e)
        {
            if (cmbAction.SelectedIndex == 0) // backup
            {
                tbk1.Text = "در آدرس";
                stkDest.Visibility = System.Windows.Visibility.Visible;
            }
            else // restore
            {
                if (dtgrd1.SelectedItem == null)
                {
                    UIUtil.ShowMessageBox("لطفا یک پشتیبان گیری انتخاب کنید.", "", MessageBoxButton.OK, MessageBoxImage.Error, this.ParentOfType<Window>());

                    cmbAction.SelectionChanged -= cmbAction_Selected_1;

                    cmbAction.SelectedIndex = 0;

                    cmbAction.SelectionChanged += cmbAction_Selected_1;
                }

                stkDest.Visibility = System.Windows.Visibility.Visible;
            }

            btnStart.Visibility = System.Windows.Visibility.Visible;
        }

        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            var lstBak = _bakRstrs.OrderByDescending(b => b.FldCod).Where(b => !b.FldTyp).FirstOrDefault();

            if (lstBak != null)
            {
                dlg.SelectedPath = Directory.GetParent(lstBak.FldAdrs).FullName;
            }

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtDest.Text = dlg.SelectedPath;
            }

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAction.SelectedIndex == 0) // backup
            {
                var si = ServerInfo.ReadInfo();

                SqlServerBackupRestore.BackupHelper bh = new SqlServerBackupRestore.BackupHelper();

                var address = Path.Combine(txtDest.Text, string.Format("{0}.bak", si.DBName));

                bh.BackupDatabase(si.DBName, si.UserName, si.Password, si.ServerName, address);

                TblBakAndRsr newBak = new TblBakAndRsr() { FldAdrs = address, FldTyp = false, FldDate = DateTime.Now };

                ctx.TblBakAndRsrs.AddObject(newBak);

                PublicMethods.SaveContext(ctx);
                
                _bakRstrs.Add(newBak);

                dtgrd1.ItemsSource = _bakRstrs;

                UIUtil.ShowMessageBox("پشتیبان گیری با موفقیت انجام شد.", "پیام", MessageBoxButton.OK, MessageBoxImage.Information, this.ParentOfType<Window>());
            }
            else
            {
                UIUtil.ShowMessageBox("این قسمت از نرم افزار هنوز پیاده سازی نشده است.", "پیام", MessageBoxButton.OK, MessageBoxImage.Information, this.ParentOfType<Window>());
                return;

                //TblBakAndRsr selectedBk = dtgrd1.SelectedItem as TblBakAndRsr;


                //var si = ServerInfo.ReadInfo();


                ////                ctx.ExecuteStoreCommand(string.Format(
                ////@"
                ////ALTER DATABASE [{0}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE;
                ////USE [{0}];
                ////ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                ////", si.DBName));

                ////                USE tempdb;
                ////DROP DATABASE [{0}];

                ////ctx.Dispose();

                //SqlServerBackupRestore.RestoreHelper rh = new SqlServerBackupRestore.RestoreHelper();

                //rh.RestoreDatabase(si.DBName, selectedBk.FldAdrs, si.ServerName, si.UserName, si.Password);

                //ctx = new BPMNDBEntities();

                ////ctx.ExecuteStoreCommand(string.Format("ALTER DATABASE {0} SET Multi_User", si.DBName));

                //TblBakAndRsr newRstr = new TblBakAndRsr() { FldTyp = true, FldDate = DateTime.Now, FldBakRlt = selectedBk.FldCod };

                //ctx.TblBakAndRsrs.AddObject(newRstr);

                //ctx.SaveChanges();

                //dtgrd1.ItemsSource = _bakRstrs;
            }
        }
    }
}
