using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.Properties;
using SSYM.OrgDsn.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();

            refreshList();

        }


        private void refreshList()
        {
            using (BPMNDBEntities context = new BPMNDBEntities())
            {
                List<TblVrsnSfw> result = context.TblVrsnSfws.ToList();
                grdersions.ItemsSource = result;
            }

        }

        private void btnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(this, "اتصال به سرور SSYM امکان پذیر نیست.");

            tblkAvailableVersion.Text = "اتصال به سرور SSYM امکان پذیر نیست.";
            tblkAvailableVersion.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            rdbGetFromServer.IsChecked = false;
            rdbGetFromLocal.IsChecked = rdbGetFromLocal.IsEnabled = true;
        }

        private void btnStartUpdate_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();

            if (!string.IsNullOrWhiteSpace(Settings.Default.LstLocUpdt))
            {
                dlg.SelectedPath = Settings.Default.LstLocUpdt;
            }


            var res = dlg.ShowDialog(this.GetIWin32Window());

            if (res == System.Windows.Forms.DialogResult.OK)
            {
                Settings.Default.LstLocUpdt = dlg.SelectedPath;
                Settings.Default.Save();


                try
                {
                    string currentLocation = Assembly.GetExecutingAssembly().Location;

                    currentLocation = Path.GetDirectoryName(currentLocation);

                    string path = Path.Combine(currentLocation, "OrgArc-Updates");

                    TblVrsnSfw version = getVersion(dlg.SelectedPath);

                    path = Path.Combine(path, version.FldNomVrsn);

                    Directory.CreateDirectory(path);

                    DirectoryCopy(dlg.SelectedPath, path, true);

                    string srvName = ServerInfo.ReadInfo().ServerName;

                    if (srvName.EndsWith("\\"))
                    {
                        srvName = srvName.Substring(0, srvName.Length - 1);
                    }

                    version.FldAdrsFileVrsn = string.Format(@"\\{0}\OrgArc-Updates\{1}", srvName, version.FldNomVrsn);

                    AddVrsn(version);

                    refreshList();

                    MessageBox.Show(this, "نسخه مورد نظر با موفقیت بارگزاری شد.");
                }
                catch (Exception ex)
                {
                    Util.ShowExeption(this, ex);
                }
            }

            dlg.Dispose();
        }

        private TblVrsnSfw getVersion(string folderPath)
        {
            string path = Path.Combine(folderPath, "SSYM.OrgDsn.UI.exe");
            //Assembly assembly = Assembly.LoadFrom(path);
            //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            //string version = fvi.FileVersion;

            string desc = File.ReadAllText(Path.Combine(folderPath, "DscChg.txt"));

            string ver = desc.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[0];

            return new TblVrsnSfw() { FldDscChg = desc, FldDteRlse = DateTime.Now, FldNomVrsn = ver };
        }

        private void AddVrsn(TblVrsnSfw newVer)
        {
            using (BPMNDBEntities context = new BPMNDBEntities())
            {
                context.TblVrsnSfws.AddObject(newVer);
                 PublicMethods.SaveContext(context);
            }

        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {

                if (Path.GetExtension(file.Name).ToLower().EndsWith("sql"))
                {
                    executeScript(dir, file);
                }


                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private static void executeScript(DirectoryInfo dir, FileInfo file)
        {
            string s1 = File.ReadAllText(Path.Combine(dir.FullName, file.Name));
            string s2 = CipherUtility.Decrypt<AesManaged>(s1, CipherUtility.pass, CipherUtility.salt);
            Util.ExecuteScriptWithCurrentConnection(s2);
        }

    }


    public static class MyWpfExtensions
    {
        public static System.Windows.Forms.IWin32Window GetIWin32Window(this System.Windows.Media.Visual visual)
        {
            var source = System.Windows.PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            private readonly System.IntPtr _handle;
            public OldWindow(System.IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members
            System.IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }
    }
}
