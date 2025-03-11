using System.Runtime.CompilerServices;
using MahApps.Metro.Controls;
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
using System.Configuration;
using System.Xml;
using SSYM.OrgDsn.ViewModel.Base;
using System.Windows.Media.Animation;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.View.Process.UserCtl;
using SSYM.OrgDsn.UI.View.UserSetting;
using SSYM.OrgDsn.UI.View.Admin;
using System.IO;
using SSYM.OrgDsn.UI.Properties;
using System.Diagnostics;
using System.Reflection;
using SSYM.OrgDsn.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System.Windows.Controls.Primitives;
using SSYM.OrgDsn.ViewModel.UserSetting;
using Telerik.Windows.Controls;
using Xceed.Wpf.DataGrid.Settings;

namespace SSYM.OrgDsn.UI
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : IView
    {
        //string connectionStringTest = @"metadata=res://*/DbModel.csdl|res://*/DbModel.ssdl|res://*/DbModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=group1-pc;Initial Catalog=DBOrgDsnTest;User id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework&quot;";

        //string connectionStringDemo = @"metadata=res://*/DbModel.csdl|res://*/DbModel.ssdl|res://*/DbModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=group1-pc;Initial Catalog=DBOrgDsnDemo;User id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework&quot;";


        public Menu()
        {
            InitializeComponent();
            //this.WindowState = System.Windows.WindowState.
            this.DataContextChanged += Menu_DataContextChanged;

            this.Loaded += Menu_Loaded;
            //IsTestDBSelected = false;

        }

        private void WindowBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void bClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private ToggleButton lastChecked = null;
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SettingContent.Content = null;
            SettingContent.IsEnabled = true;
            if (lastChecked != null) lastChecked.IsChecked = false;
            SettingTitle.Text = (sender as ToggleButton).ToolTip.ToString();
            lastChecked = sender as ToggleButton;

            switch ((sender as ToggleButton).ToolTip.ToString())
            {
                case "اطلاعات کاربری":
                    (this.DataContext as MenuViewModel).SelectedObj = new ViewModel.UserSetting.UsrSettingViewModel();
                    SettingContent.Content = new UsrSetting() { DataContext = (this.DataContext as MenuViewModel).SelectedObj };
                    
                    //if ((vm.SelectedObj as UsrSettingViewModel).IsAdmin) SettingContent.IsEnabled = false;
                    
                    break;
                case "ویژگی های نمایش":
                    SettingContent.Content = new UISettings();
                    break;
                case "بروز رسانی":
                    if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
                    {
                        return;
                    }

                    TblVrsnSfw ver = null;
                    using (BPMNDBEntities context = new BPMNDBEntities())
                    {
                        ver = context.TblVrsnSfws.OrderByDescending(v => v.FldCodVrsnSfw).FirstOrDefault();
                    }

                    if (ver == null)
                    {
                        if (sender != null)
                        {
                            ViewModel.Util.ShowMessageBox(66);
                        }
                        return;
                    }

                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

                    Version curr = new Version(fvi.FileVersion);

                    if (curr.CompareTo(new Version(ver.FldNomVrsn)) >= 0)
                    {
                        if (sender != null)
                        {
                            ViewModel.Util.ShowMessageBox(66);
                        }
                        return;
                    }

                    if (ViewModel.Util.ShowMessageBox(69, ver.FldDscChg) == MessageBoxResult.Yes)
                    {
                        Process.Start("SSYM.OrgDsn.Updater.exe", string.Format("{0} {1}", string.Format("\"{0}\"", ver.FldAdrsFileVrsn), string.Format("\"{0}\"", UIUtil.getCurrentLocation())));

                        App.Current.Shutdown();
                    }
                    break;
                case "نمایش محدودیت های تجاری":
                    SettingContent.Content = new ShowRemnLcs();
                    break;
                case "فعال کردن کاربران":
                    (this.DataContext as MenuViewModel).SelectedObj = new ViewModel.Admin.ActiveUsrViewModel();
                    SettingContent.Content = new ActiveUsr() { DataContext=(this.DataContext as MenuViewModel).SelectedObj };
                    break;
                case "سطح دسترسی":
                    (this.DataContext as MenuViewModel).SelectedObj = new ViewModel.Admin.DefLvlAcsViewModel();
                    SettingContent.Content = new DefLvlAcs() { DataContext = (this.DataContext as MenuViewModel).SelectedObj };
                    break;
                case "ارتقاء به نسخه کامل":
                    if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
                    {
                        return;
                    }

                    if (!CipherUtility.ValidateInstallState())
                    {
                        ViewModel.Util.ShowMessageBox(67);
                        return;
                    }

                    UpgrdToVrsnCmplt wnd = new UpgrdToVrsnCmplt();
                    wnd.ShowDialog();
                    (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();
                    updateVersion();
                    break;

                case "برگشت به نسخه راهبری":
                    if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
                    {
                        return;
                    }

                    string hid = Util.GenerateSerialNumber();

                    string path = CipherUtility.SetAsUnused(hid);

                    //Settings.Default.TypVrsn = "SERVER";

                    //Settings.Default.Save();


                    CipherUtility.updateMyConfig("SERVER", UIUtil.getCurrentLocation());


                    ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.SERVER;

                    CipherUtility.UpdateInstallState();

                    ViewModel.Util.ShowMessageBox(65);

                    (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();

                    updateVersion();
                    break;
                case "نصب نسخه کاربری":
                    if (!System.Diagnostics.Debugger.IsAttached || !Settings.Default.UndrDvlp)
                    {
                        if (!CipherUtility.ValidateInstallState())
                        {
                            ViewModel.Util.ShowMessageBox(67);
                            return;
                        }
                    }


                    InslCstmrVrsn wnd1 = new InslCstmrVrsn();
                    wnd1.ShowDialog();

                    if (wnd1.InstallSuccess)
                    {
                        ViewModel.Util.IsInslVrsnClntOnnSysCnt = Util.calculateIsInslVrsnClntOnnSysCnt();

                        (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();
                    }
                    break;
                case "حذف نسخه کاربری":
                    try
                    {
                        if (ViewModel.Util.ShowMessageBox(61) != MessageBoxResult.Yes)
                        {
                            return;
                        }

                        string hid1 = Util.GenerateSerialNumber();

                        string path1 = CipherUtility.SetAsUnused(hid1);

                        if (!string.IsNullOrEmpty(path1))
                        {
                            if (Directory.Exists(path1))
                            {
                                Directory.Delete(path1, true);
                            }
                        }
                        else
                        {
                            MessageBox.Show("کد سخت افزاری پیدا نشد");
                            return;
                        }


                        ViewModel.Util.IsInslVrsnClntOnnSysCnt = Util.calculateIsInslVrsnClntOnnSysCnt();

                        (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();

                        CipherUtility.UpdateInstallState();

                        ViewModel.Util.ShowMessageBox(62);

                        deleteShortcut();

                        CipherUtility.unInstallCertificate();
                    }
                    catch (Exception ex)
                    {
                        Util.ShowExeption(this, ex);
                    }
                    break;
                case "بارگزاری نسخه جدید":
                    if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
                    {
                        return;
                    }

                    View.Admin.Update updt = new Update();
                    updt.ShowDialog();
                    break;
                case "مدیریت داده":
                    SettingContent.Content = new DataMgr();
                    break;
            }

            if (SettingContent.Content != null) (this.FindResource("SettingPages_Enter") as Storyboard).Begin();
        }

        private void backSetting_Click(object sender, RoutedEventArgs e)
        {
            if (lastChecked != null) lastChecked.IsChecked = false;
            SettingContent.Content = null;
        }

        private void bMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void bNormal_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void bMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            App.applyUiSettings();

            updateVersion();

            update_Click(null, null);

            this.Closing += Menu_Closing;
        }

        void Menu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = this.DataContext as MenuViewModel;

            if (vm.SelectedObj != null)
            {
                (vm.SelectedObj as IViewModel).ConfirmAndClose();
            }
            if (Util.ShowMessageBox(79) == MessageBoxResult.No)
                e.Cancel = true;

        }

        void Menu_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                (this.DataContext as IViewModelBase).View = this;

                this.DataContextChanged -= Menu_DataContextChanged;
            }
        }

        private void ShowReport(object sender, RoutedEventArgs e)
        {
            //SSYM.OrgDsn.UI.View.Report.RpotPrs rp = new View.Report.RpotPrs();
            //Window w = new Window();
            //w.Content = rp;
            //w.ShowDialog();
        }

        //private void clearContent(object sender, RoutedEventArgs e)
        //{
        //mainCnt.Content = null;
        //GC.Collect();
        //}

        //bool isTestDBSelected = true;

        //public bool IsTestDBSelected
        //{
        //    get
        //    {
        //        return isTestDBSelected;
        //    }
        //    set
        //    {
        //        isTestDBSelected = value;

        //        if (value)
        //        {
        //            updateConfigFile(connectionStringTest);
        //            //to refresh connection string each time else it will use previous connection string
        //            ConfigurationManager.RefreshSection("connectionStrings");
        //        }

        //        else
        //        {
        //            updateConfigFile(connectionStringDemo);
        //            //to refresh connection string each time else it will use previous connection string
        //            ConfigurationManager.RefreshSection("connectionStrings");
        //        }

        //    }
        //}

        //public void updateConfigFile(string con)
        //{
        //    //updating config file
        //    XmlDocument XmlDoc = new XmlDocument();
        //    //Loading the Config file
        //    XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //    foreach (var xElement in XmlDoc.DocumentElement)
        //    {
        //        if (xElement.GetType() == typeof(XmlElement))
        //        {
        //            XmlElement x = (XmlElement)xElement;
        //            if (x.Name == "connectionStrings")
        //            {
        //                //setting the coonection string
        //                x.FirstChild.Value = con;
        //            }
        //        }



        //    }
        //    //writing the connection string in config file
        //    XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //}

        public static MessageBoxResult RaisePopupStatic(ViewModel.ActivityDefinition.Popup.PopupDataObject entity, Action<ViewModel.ActivityDefinition.Popup.PopupDataObject> callback, Action cancelCallback)
        {
            MessageBoxResult result = MessageBoxResult.Cancel;
            switch (entity.MessageBoxType)
            {
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Information:
                    result = MessageBox.Show(entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Error:
                    result = MessageBox.Show(entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Warning:
                    result = MessageBox.Show(entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Confirmation:
                    result = MessageBox.Show(entity.Content, entity.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Question:
                    result = MessageBox.Show(entity.Content, entity.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    break;
                default:
                    break;

            }

            return result;
        }

        public MessageBoxResult RaisePopup(ViewModel.ActivityDefinition.Popup.PopupDataObject entity, Action<ViewModel.ActivityDefinition.Popup.PopupDataObject> callback, Action cancelCallback)
        {
            Style messageBoxStyle = this.FindResource("XceedMessageBox") as Style;
            MessageBoxResult result = MessageBoxResult.Cancel;
            switch (entity.MessageBoxType)
            {

                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Information:

                    result = Xceed.Wpf.Toolkit.MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, messageBoxStyle);//, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Error:
                    result = Xceed.Wpf.Toolkit.MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, messageBoxStyle);//, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    //result = MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Warning:
                    result = Xceed.Wpf.Toolkit.MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, messageBoxStyle);//, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    //result = MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Confirmation:
                    result = Xceed.Wpf.Toolkit.MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, messageBoxStyle);// MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Question:
                    result = Xceed.Wpf.Toolkit.MessageBox.Show(this, entity.Content, entity.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.OK, messageBoxStyle);//MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    break;
                default:
                    break;
            }

            if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                if (callback != null)
                {
                    callback(entity);
                }
            }

            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                if (cancelCallback != null)
                {
                    cancelCallback();
                }
            }

            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public MessageBoxResult ShowPopupWindow(PopupViewModel dataContext)
        {
            dataContext.ResetResult();

            Type typ = dataContext.GetType();

            UserControl uc = null;

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcIntViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcInt() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefIntViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefInt() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcSrcAndDstViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcSrcAndDst() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcActOfNodViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcActsOfNod() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcActSrcViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcActSrc() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcNewsRecvViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcNewsRecv() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefNewsViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefNews() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcSfwViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcSfw() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefSfwViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefSfw() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcUntViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcUnt() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefUntViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefUnt() { DataContext = dataContext };
            }


            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcIdxViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcIdx() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefIdxViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefIdx() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcErorViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcEror() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefErorViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefEror() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefOralViewModel))
            {
                uc = new View.ActivityDefinition.Popup.DefOral() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcNodAndActViewModel))
            {
                uc = new View.ActivityDefinition.Popup.SlcNodAndAct() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.VotForNamPrpsPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.VotForNamPrpsPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.VotForNamPrpsPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.VotForNamPrpsPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.DtlVotNamPrpsPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.DtlVotNamPrpsPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.VotForOwrPrpsPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.VotForOwrPrpsPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.PrpsNamForPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.PrpsNamForPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.PrpsOwrForPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.PrpsOwrForPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.SttPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.SttPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.FndPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.FndPrs() { DataContext = dataContext };
            }


            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcOutViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcOut() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.DefOutViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.DefOut() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcDstForOutViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcDstForOut() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcActDstViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcActDst() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcNewsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcNews() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.PsnIsdOrgViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.EntityDefinition.Popup.SlcPsnIsdOrg() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.SlcPosPstRolViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcPosPstRol() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.EntityDefinition.Popup.SlcNodViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.EntityDefinition.Popup.SlcNod() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Process.Popup.DtlVotOwrPrpsPrsViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Process.Popup.DtlVotOwrPrpsPrs() { DataContext = dataContext };
            }

            if (typ == typeof(SlcPosPstOrgViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcPosPstOrg() { DataContext = dataContext };
            }

            if (typ == typeof(SlcPsnAndOrgOsdViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.ActivityDefinition.Popup.SlcPsnAndOrgOsd() { DataContext = dataContext };
            }

            if (dataContext is SSYM.OrgDsn.ViewModel.Report.IReport)
            {
                uc = new SSYM.OrgDsn.UI.View.Report.ucRpotSrch() { DataContext = dataContext };
            }

            if (typ == typeof(SSYM.OrgDsn.ViewModel.Admin.AdminInfViewModel))
            {
                uc = new SSYM.OrgDsn.UI.View.Admin.AdminInf() { DataContext = dataContext };
            }

            if (uc != null)
            {
                SSYM.OrgDsn.UI.View.Base.PopupWindow wnd = new View.Base.PopupWindow() { PopupContent = uc };
                wnd.Owner = this;
                wnd.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                wnd.ResizeMode = System.Windows.ResizeMode.NoResize;
                //wnd.Width = uc.Width;
                //wnd.Height = uc.Height;
                return wnd.ShowDialog() == true ? MessageBoxResult.OK : MessageBoxResult.Cancel;
            }

            return MessageBoxResult.OK;
        }

        public void HideCurrentView()
        {
            //(this.FindResource("ShowMnuStoryBoard") as Storyboard).Begin();
            //(this.FindResource("HideDtlStoryBoard") as Storyboard).Begin();
        }

        private void closeApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //App.Current.Shutdown();
        }

        private void showHelp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try { ucNotification.Text = PublicMethods.TblHelpDynm[int.Parse((sender as FrameworkElement).Tag.ToString())]; }
            catch (Exception) { }
            ucNotification.Show(MessageBoxType.Information);
        }

        private void hideHelp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //this.BeginStoryboard(FindResource("hideStatusBar") as Storyboard);
            ucNotification.Hide();
        }

        private void showSettingForm_Click(object sender, RoutedEventArgs e)
        {
            /*UISettings wnd = new UISettings();
            wnd.ShowDialog();*/
        }

        private void showInstallWizard_Click(object sender, RoutedEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached || !Settings.Default.UndrDvlp)
            {
                if (!CipherUtility.ValidateInstallState())
                {
                    ViewModel.Util.ShowMessageBox(67);
                    return;
                }
            }


            InslCstmrVrsn wnd = new InslCstmrVrsn();
            wnd.ShowDialog();

            if (wnd.InstallSuccess)
            {
                ViewModel.Util.IsInslVrsnClntOnnSysCnt = Util.calculateIsInslVrsnClntOnnSysCnt();

                (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();
            }
        }

        private void uninstall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ViewModel.Util.ShowMessageBox(61) != MessageBoxResult.Yes)
                {
                    return;
                }

                string hid = Util.GenerateSerialNumber();

                string path = CipherUtility.SetAsUnused(hid);

                if (!string.IsNullOrEmpty(path))
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
                else
                {
                    MessageBox.Show("کد سخت افزاری پیدانشد");
                    return;
                }


                ViewModel.Util.IsInslVrsnClntOnnSysCnt = Util.calculateIsInslVrsnClntOnnSysCnt();

                (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();

                CipherUtility.UpdateInstallState();

                ViewModel.Util.ShowMessageBox(62);

                deleteShortcut();

                CipherUtility.unInstallCertificate();
            }
            catch (Exception ex)
            {
                Util.ShowExeption(this, ex);
            }
        }

        private void deleteShortcut()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.File.Delete(Path.Combine(desktopPath, "OrgArc.lnk"));
        }

        private void upgrade_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
            {
                return;
            }

            if (!CipherUtility.ValidateInstallState())
            {
                ViewModel.Util.ShowMessageBox(67);
                return;
            }

            UpgrdToVrsnCmplt wnd = new UpgrdToVrsnCmplt();
            wnd.ShowDialog();
            (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();
            updateVersion();


        }

        private void downGradeToSrvVrsn_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
            {
                return;
            }

            string hid = Util.GenerateSerialNumber();

            string path = CipherUtility.SetAsUnused(hid);

            //Settings.Default.TypVrsn = "SERVER";

            //Settings.Default.Save();


            CipherUtility.updateMyConfig("SERVER", UIUtil.getCurrentLocation());


            ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.SERVER;

            CipherUtility.UpdateInstallState();

            ViewModel.Util.ShowMessageBox(65);

            (this.DataContext as SSYM.OrgDsn.ViewModel.MenuViewModel).RaiseProperties();

            updateVersion();

        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
            {
                return;
            }

            TblVrsnSfw ver = null;
            using (BPMNDBEntities context = new BPMNDBEntities())
            {
                ver = context.TblVrsnSfws.OrderByDescending(v => v.FldCodVrsnSfw).FirstOrDefault();
            }

            if (ver == null)
            {
                if (sender != null)
                {
                    ViewModel.Util.ShowMessageBox(66);
                }
                return;
            }

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            Version curr = new Version(fvi.FileVersion);

            if (curr.CompareTo(new Version(ver.FldNomVrsn)) >= 0)
            {
                if (sender != null)
                {
                    ViewModel.Util.ShowMessageBox(66);
                }
                return;
            }

            if (ViewModel.Util.ShowMessageBox(69, ver.FldDscChg) == MessageBoxResult.Yes)
            {
                Process.Start("SSYM.OrgDsn.Updater.exe", string.Format("{0} {1}", string.Format("\"{0}\"", ver.FldAdrsFileVrsn), string.Format("\"{0}\"", UIUtil.getCurrentLocation())));

                App.Current.Shutdown();
            }

        }

        private void updateVersion()
        {

            switch (ViewModel.Util.VrsnTyp)
            {
                case SSYM.OrgDsn.ViewModel.TypVrsn.SERVER:
                    tbkVersion.Text = "نسخه راهبری";
                    break;
                case SSYM.OrgDsn.ViewModel.TypVrsn.CLIENT:
                    tbkVersion.Text = "نسخه کاربری";
                    break;
                case SSYM.OrgDsn.ViewModel.TypVrsn.COMPELETE:
                    tbkVersion.Text = "نسخه کامل";
                    break;
                default:
                    break;
            }
        }

        private void loadUpdatedVer_Click(object sender, RoutedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
            {
                return;
            }

            View.Admin.Update updt = new Update();
            updt.ShowDialog();
        }

        private void hlpClnt_Click(object sender, RoutedEventArgs e)
        {
            string address = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            //address = Path.Combine(address, "ClientResources");

            address = Path.Combine(address, "hlpClnt.pdf");

            Process.Start(address);
        }

        private void hlpSrv_Click(object sender, RoutedEventArgs e)
        {
            string address = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            //address = Path.Combine(address, "ClientResources");

            address = Path.Combine(address, "hlpSrv.pdf");

            Process.Start(address);
        }

        private void support_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TeamViewerQS.exe"));
        }

        private void dataMgr_Click(object sender, RoutedEventArgs e)
        {
            DataMgr mgr = new DataMgr();
            mgr.ParentOfType<Window>().ShowDialog();
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {
            var about = new SSYM.OrgDsn.UI.View.Support.About();
            about.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            about.ShowDialog();
        }


        public void ShowNotification(string message, MessageBoxType status, bool autoHide = false, int hideAfter = 4000)
        {
            this.Dispatcher.Invoke(new Action(() =>
                {
                    ucNotification.Text = message;
                    ucNotification.Show(status, autoHide, hideAfter);
                }));
        }


        public void HideNotification()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                ucNotification.Hide();
            }));

        }

        bool IsColumnDrag = false;

        private void VSplitter_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsColumnDrag) Side.Width = ColumnsGrid.ActualWidth - e.GetPosition(ColumnsGrid).X;
        }

        private void VSplitter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsColumnDrag = true;
        }

        private void VSplitter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsColumnDrag = false;
        }

        private void tbnStructure_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("Structure_Expand") as Storyboard).Begin();
        }

        private void tbnStructure_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("Structure_Collapse") as Storyboard).Begin();
        }

        private void tbnOfficeStructure_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("OfficeStructure_Expand") as Storyboard).Begin();
        }

        private void tbnOfficeStructure_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("OfficeStructure_Collapse") as Storyboard).Begin();
        }

        private void tbnProcess_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("Process_Expand") as Storyboard).Begin();
        }

        private void tbnProcess_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.FindResource("Process_Collapse") as Storyboard).Begin();
        }

        private void TbnHome_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                tbnHome.IsEnabled = false;

                tbnReport.IsChecked = false;
                tbnReport.IsEnabled = true;

                tbnSetting.IsChecked = false;
                tbnSetting.IsEnabled = true;

                (this.FindResource("Home_Enter") as Storyboard).Begin();
            }
            catch { }
        }

        private void TbnReport_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                tbnReport.IsEnabled = false;

                tbnHome.IsChecked = false;
                tbnHome.IsEnabled = true;

                tbnSetting.IsChecked = false;
                tbnSetting.IsEnabled = true;

                (this.FindResource("Report_Enter") as Storyboard).Begin();
            }
            catch { }
        }

        private void TbnSetting_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                tbnSetting.IsEnabled = false;

                tbnHome.IsChecked = false;
                tbnHome.IsEnabled = true;

                tbnReport.IsChecked = false;
                tbnReport.IsEnabled = true;

                (this.FindResource("Setting_Enter") as Storyboard).Begin();
            }
            catch { }
        }

        private void Buttons_Click(object sender, RoutedEventArgs e)
        {
            (this.FindResource("Main_Leave") as Storyboard).Begin();
            (this.FindResource("Pages_Enter") as Storyboard).Begin();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            (this.FindResource("Main_Enter") as Storyboard).Begin();
            (this.FindResource("Pages_Leave") as Storyboard).Begin();
        }
    }
}
