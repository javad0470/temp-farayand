using IWshRuntimeLibrary;
using Microsoft.SqlServer.Management.Smo;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SSYM.OrgDsn.UI.View.Admin
{

    enum SetupStep
    {
        Wellcome,
        SetConnection,
        Auhenticate,
        CheckSerials,
        SelectFolder,
        Installing
    }


    public partial class InslCstmrVrsn : Window
    {
        public InslCstmrVrsn()
        {

            InitializeComponent();

            InstallSuccess = false;

            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            txtDBName.Text = "BPMNDB";
            CurrentStep = SetupStep.Wellcome;

            ServerInfo info = ServerInfo.ReadInfo();


            if (info != null)
            {

                txtServerName.Text = info.ServerName;
                txtDBName.Text = info.DBName;

                if (!string.IsNullOrEmpty(info.UserName))
                {
                    cmbMode.SelectedIndex = 1;
                    txtUsername.Text = info.UserName;
                    txtPassword.Password = info.Password;
                }
            }

            //_defaultInstallDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "SSYM\\OrgDsn");
            _dlg = new System.Windows.Forms.FolderBrowserDialog();

            _dlg.SelectedPath = installPath = txtInstallPath.Text = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles), "SSYM\\OrgArc");
        }


        SetupStep _currentStep;
        Server _srv;
        string connectionString = "";
        string installPath = "";
        TblInsOnnClnt _serial;
        System.Windows.Forms.FolderBrowserDialog _dlg;
        string shortcutName = "OrgArc";

        #region Wellcome

        private void btnStart_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentStep = SetupStep.SetConnection;
        }

        #endregion

        #region SetConnection

        private void cmbMode_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMode.SelectedIndex == 0)
            {
                if (sqlMode != null)
                {
                    sqlMode.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                sqlMode.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void connectToDB_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMode.SelectedIndex == 0) // windows Authentication
            {
                try
                {
                    _srv = Util.connectRemoteInstanceWA(txtServerName.Text);

                    _srv.ConnectionContext.DatabaseName = txtDBName.Text;
                    _srv.ConnectionContext.MultipleActiveResultSets = true;
                    _srv.ConnectionContext.ApplicationName = "EntityFramework";



                    MessageBox.Show(string.Format("اتصال به دیتابیس از طریق SQL Server ورژن {0} باموفقیت انجام شد.", _srv.Information.Version.ToString()), "پیام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                }
                catch (Exception ex)
                {
                    ShowExeption(ex);
                    return;
                }
            }
            else // sql server Authentication
            {
                try
                {
                    string[] str = txtServerName.Text.Split('\\');

                    string instanse = "";
                    if (str.Length > 1)
                    {
                        instanse = str[1];
                    }

                    _srv = Util.connectToInstanceOfSqlSA(txtUsername.Text, txtPassword.Password, instanse, str[0]);

                    _srv.ConnectionContext.DatabaseName = txtDBName.Text;
                    _srv.ConnectionContext.MultipleActiveResultSets = true;
                    _srv.ConnectionContext.ApplicationName = "EntityFramework";

                    MessageBox.Show(string.Format("اتصال به دیتابیس از طریق SQL Server ورژن {0} باموفقیت انجام شد.", _srv.Information.Version.ToString()), "پیام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
                catch (Exception ex)
                {
                    ShowExeption(ex);
                    return;
                }
            }

            this.connectionString = createConnectionString(_srv);


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

            CurrentStep = SetupStep.CheckSerials;


        }


        #endregion

        #region authenticate

        private void authenticate_Click(object sender, RoutedEventArgs e)
        {
            string hashedPassword = CipherUtility.CalculateMD5Hash(txtPass.Password);

            using (SqlConnection con = new SqlConnection(_srv.ConnectionContext.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(
                    string.Format(@"
                        SELECT * FROM [dbo].[TblUsr]
                          where [FldNamUsr] = '{0}'
                          and [FldPassUsr] = '{1}'"
                    , txtUsr.Text, hashedPassword)
                    , con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
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

                        CurrentStep = SetupStep.CheckSerials;
                    }
                    else
                    {
                        MessageBox.Show(this, "نام کاربری یا کلمه عبور نامعتبر است", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        return;
                    }
                }
            }
        }

        #endregion

        #region CheckSerials

        private void checkSerials_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentStep = SetupStep.SelectFolder;
        }



        #endregion

        #region SelectFolder

        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult result = _dlg.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            installPath = txtInstallPath.Text = _dlg.SelectedPath;

            if (!_dlg.SelectedPath.Trim().ToLower().EndsWith("ssym\\orgarc"))
            {
                installPath = txtInstallPath.Text = Path.Combine(_dlg.SelectedPath, "SSYM\\OrgArc");
            }


        }

        private void goToInstall_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtInstallPath.Text))
            {

                if (!txtInstallPath.Text.Trim().ToLower().EndsWith("ssym\\orgarc"))
                {
                    installPath = Path.Combine(txtInstallPath.Text, "SSYM\\OrgArc");
                }
                else
                {
                    installPath = txtInstallPath.Text;
                }

                Directory.CreateDirectory(installPath);

                if (!Directory.Exists(installPath))
                {
                    MessageBox.Show(this, "آدرس مورد نظر وجود ندارد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
                else
                {
                    CurrentStep = SetupStep.Installing;
                }
            }
            else
            {
                MessageBox.Show(this, "آدرس مورد نظر نا معتبر است.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
        }

        #endregion

        #region Installing

        private void startInstall_Click(object sender, RoutedEventArgs e)
        {
            btnBack.Visibility = System.Windows.Visibility.Collapsed;

            btnStartInstall2.Visibility = System.Windows.Visibility.Collapsed;

            stkInstallProgress.Visibility = System.Windows.Visibility.Visible;

            Thread t = new Thread(install);

            t.Start(null);

        }

        private void install(object obj)
        {
            try
            {
                startCopy();

                setAppConfig();

                setMyConfig();

                installCertificate();

                createShortcut();

                CipherUtility.SetAsUsed(_serial.FldSeriInsl, installPath);

                CipherUtility.UpdateInstallState();

            }
            catch (Exception ex)
            {
                ShowExeption(ex);
                return;
            }


            Dispatcher.Invoke(new Action(() =>
            {
                stkInstallProgress.Visibility = System.Windows.Visibility.Collapsed;

                grdFinish.Visibility = System.Windows.Visibility.Visible;
            }
), null);

        }

        void installCertificate()
        {
            X509Certificate2 certificate = new X509Certificate2(UIUtil.ReadFileContentFromResource("SSYM.OrgDsn.UI.ClientResources.keyHlp.pfx"), "09159230532", X509KeyStorageFlags.PersistKeySet);
            X509Store store = new X509Store(StoreName.My);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);
            store.Close();
        }





        void startCopy()
        {
            DirectoryCopy(new Tuple<string, string, bool>(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, installPath, false));

            //Tuple<Stream, string>[] files = GetAppFiles();

            //foreach (var file in files)
            //{
            //    byte[] content = new byte[file.Item1.Length];
            //    file.Item1.Read(content, 0, (int)file.Item1.Length);
            //    FileStream fs = new FileStream(installPath + "\\" + file.Item2, FileMode.Create);
            //    fs.Write(content, 0, (int)file.Item1.Length);

            //    file.Item1.Flush();
            //    file.Item1.Close();

            //    fs.Flush();
            //    fs.Close();
            //}
        }


        private void DirectoryCopy(object paramObj)
        {
            Tuple<string, string, bool> param = paramObj as Tuple<string, string, bool>;
            string sourceDirName = param.Item1;
            string destDirName = param.Item2;
            bool copySubDirs = param.Item3;

            //Thread.Sleep(5000);

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
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(new Tuple<string, string, bool>(subdir.FullName, temppath, copySubDirs));
                }
            }
        }



        #endregion

        public bool InstallSuccess { get; set; }

        #region Utils

        private void return_Click(object sender, RoutedEventArgs e)
        {
            switch (CurrentStep)
            {
                case SetupStep.Wellcome:
                    break;
                case SetupStep.SetConnection:
                    CurrentStep = SetupStep.Wellcome;
                    break;

                case SetupStep.Auhenticate:
                    CurrentStep = SetupStep.SetConnection;
                    break;

                case SetupStep.CheckSerials:
                    CurrentStep = SetupStep.SetConnection;
                    break;

                case SetupStep.SelectFolder:
                    CurrentStep = SetupStep.CheckSerials;
                    break;

                case SetupStep.Installing:
                    CurrentStep = SetupStep.SelectFolder;
                    break;
                default:
                    break;
            }
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            InstallSuccess = true;
            this.Close();
        }

        private string createConnectionString(Server srv)
        {
            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = srv.ConnectionContext.ServerInstance;
            string databaseName = srv.ConnectionContext.DatabaseName;

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
            new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;

            if (string.IsNullOrEmpty(srv.ConnectionContext.Login))
            {
                sqlBuilder.IntegratedSecurity = true;
            }

            else
            {
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = srv.ConnectionContext.Login;
                sqlBuilder.Password = srv.ConnectionContext.Password;
            }

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
            new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/DbModel.csdl|
res://*/DbModel.ssdl|
res://*/DbModel.msl";


            return entityBuilder.ConnectionString.Replace('\"', '\'');
        }

        private void setAppConfig()
        {
            string configContent = System.IO.File.ReadAllText(Path.Combine(installPath, "SSYM.OrgDsn.UI.exe.config"));

            configContent = string.Format(configContent, createConnectionString(_srv));

            //configContent = configContent.Replace("<value>SERVER</value>", "<value>CLIENT</value>");

            System.IO.File.WriteAllText(Path.Combine(installPath, "SSYM.OrgDsn.UI.exe.config"), configContent);
        }

        ///
        /// ایجاد فایل myconfig
        /// </summary>
        private void setMyConfig()
        {
            CipherUtility.updateMyConfig("CLIENT", installPath);
        }


        private void ShowExeption(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            MessageBox.Show(this, string.Format("{0}\n{1}\n{2}", "خطایی رخ داده است:", ex.Message, ex.StackTrace), "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private SetupStep CurrentStep
        {
            get { return _currentStep; }
            set
            {
                grdWelcome.Visibility = System.Windows.Visibility.Collapsed;
                grdSetConnection.Visibility = System.Windows.Visibility.Collapsed;
                grdCheckSerials.Visibility = System.Windows.Visibility.Collapsed;
                grdInstalling.Visibility = System.Windows.Visibility.Collapsed;
                grdSlcDirectory.Visibility = System.Windows.Visibility.Collapsed;
                grdUserPass.Visibility = System.Windows.Visibility.Collapsed;
                _currentStep = value;

                switch (_currentStep)
                {
                    case SetupStep.Wellcome:
                        grdWelcome.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case SetupStep.SetConnection:
                        grdSetConnection.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case SetupStep.Auhenticate:
                        grdUserPass.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case SetupStep.CheckSerials:
                        grdCheckSerials.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case SetupStep.SelectFolder:
                        grdSlcDirectory.Visibility = System.Windows.Visibility.Visible;
                        break;

                    case SetupStep.Installing:
                        grdInstalling.Visibility = System.Windows.Visibility.Visible;
                        break;

                    default:
                        break;
                }
            }
        }

        private static Tuple<Stream, string>[] GetAppFiles()
        {
            string prefix = "SSYM.OrgDsn.UI.ClientResources.AppFiles";
            Assembly asm = Assembly.GetExecutingAssembly();
            var resourceNames =
                asm.GetManifestResourceNames()
                .Where(name => name.StartsWith(prefix));


            List<Tuple<Stream, string>> streams = new List<Tuple<Stream, string>>();

            foreach (var file in resourceNames)
            {
                Stream stream = asm.GetManifestResourceStream(file);
                streams.Add(new Tuple<Stream, string>(stream as Stream, file.Replace("SSYM.OrgDsn.UI.ClientResources.AppFiles.", "")));
            }

            return streams.ToArray();
        }

        private void createShortcut()
        {
            //string argument = string.Format("/movetime 01\\08\\2013 \"{0}\"", installPath + "\\" + "SSYM.OrgDsn.UI.exe");

            //string desktop = string.Format("{0}", installPath + "\\" + "RunAsDate.exe");


            //if (Environment.Is64BitOperatingSystem)
            //{
            //    desktop = string.Format("{0}", installPath + "\\" + "RunAsDate64.exe");
            //}

            //createShortcut(shortcutName, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), desktop, argument);
            string argument = string.Format("/path \"{0}\"", installPath);

            string desktop = string.Format("{0}", installPath + "\\" + "OrgArcMaster.exe");


            //if (Environment.Is64BitOperatingSystem)
            //{
            //    desktop = string.Format("{0}", _defaultInstallDirectory + "\\" + "RunAsDate64.exe");
            //}

            createShortcut("OrgArc", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), desktop, argument);
        }


        private void createShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string arg)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Arguments = arg;
            shortcut.Description = "SYYM Group";   // The description of the shortcut
            shortcut.IconLocation = Path.Combine(installPath, "Logo.ico");// The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ShowExeption(e.Exception);

            App.Current.Shutdown();
        }

        #endregion
        private void Btn_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("آیا میخواهید از برنامه نصب خارج شوید؟", "خروج", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading) == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
            }
        }

        private void Btn_forward_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentStep == SetupStep.Wellcome)
            {
                CurrentStep = SetupStep.SetConnection;
            }
            else if (CurrentStep == SetupStep.SetConnection)
            {
                if (cmbMode.SelectedIndex == 0) // windows Authentication
                {
                    try
                    {
                        _srv = Util.connectRemoteInstanceWA(txtServerName.Text);

                        _srv.ConnectionContext.DatabaseName = txtDBName.Text;
                        _srv.ConnectionContext.MultipleActiveResultSets = true;
                        _srv.ConnectionContext.ApplicationName = "EntityFramework";



                        MessageBox.Show(string.Format("اتصال به دیتابیس از طریق SQL Server ورژن {0} باموفقیت انجام شد.", _srv.Information.Version.ToString()), "پیام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    }
                    catch (Exception ex)
                    {
                        ShowExeption(ex);
                        return;
                    }
                }
                else // sql server Authentication
                {
                    try
                    {
                        string[] str = txtServerName.Text.Split('\\');

                        string instanse = "";
                        if (str.Length > 1)
                        {
                            instanse = str[1];
                        }

                        _srv = Util.connectToInstanceOfSqlSA(txtUsername.Text, txtPassword.Password, instanse, str[0]);

                        _srv.ConnectionContext.DatabaseName = txtDBName.Text;
                        _srv.ConnectionContext.MultipleActiveResultSets = true;
                        _srv.ConnectionContext.ApplicationName = "EntityFramework";

                        MessageBox.Show(string.Format("اتصال به دیتابیس از طریق SQL Server ورژن {0} باموفقیت انجام شد.", _srv.Information.Version.ToString()), "پیام", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    }
                    catch (Exception ex)
                    {
                        ShowExeption(ex);
                        return;
                    }
                }

                this.connectionString = createConnectionString(_srv);


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

                CurrentStep = SetupStep.CheckSerials;

            }
            else if (CurrentStep == SetupStep.CheckSerials)
            {
                this.CurrentStep = SetupStep.SelectFolder;
            }
            else if (CurrentStep == SetupStep.SelectFolder)
            {
                if (!string.IsNullOrWhiteSpace(txtInstallPath.Text))
                {

                    if (!txtInstallPath.Text.Trim().ToLower().EndsWith("ssym\\orgarc"))
                    {
                        installPath = Path.Combine(txtInstallPath.Text, "SSYM\\OrgArc");
                    }
                    else
                    {
                        installPath = txtInstallPath.Text;
                    }

                    Directory.CreateDirectory(installPath);

                    if (!Directory.Exists(installPath))
                    {
                        MessageBox.Show(this, "آدرس مورد نظر وجود ندارد.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    }
                    else
                    {
                        CurrentStep = SetupStep.Installing;
                    }
                }
                else
                {
                    MessageBox.Show(this, "آدرس مورد نظر نا معتبر است.", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                }
            }
            else if (CurrentStep == SetupStep.Installing)
            {
                btnBack.Visibility = System.Windows.Visibility.Collapsed;

                btnStartInstall2.Visibility = System.Windows.Visibility.Collapsed;

                stkInstallProgress.Visibility = System.Windows.Visibility.Visible;

                Thread t = new Thread(install);

                t.Start(null);
            }
        }

    }


}
