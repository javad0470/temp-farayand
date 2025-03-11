using SSYM.OrgDsn.Base;
using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Elysium;
using SSYM.OrgDsn.UI.Properties;
using SSYM.OrgDsn.UI.View.Admin;
using System.IO;

namespace SSYM.OrgDsn.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App currentApp;


        public App()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            currentApp = this;

            //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            applyUiSettings();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }

            Exception ex = e.ExceptionObject as Exception;
            handleException(e.ExceptionObject as Exception, ex);
        }


        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }

            Exception ex = UIUtil.GetInnerExceptionRecusive(e.Exception);

            e.Handled = true;

            var exObj = e.Exception;

            if (e.Exception.InnerException != null)
            {
                exObj = e.Exception.InnerException;
            }

            handleException(exObj, ex);

            //            MessageBox.Show(string.Format(@"
            //                            {0}\n
            //                            {1}\n
            //                            {2}\n
            //                            {3}\n
            //                            {4}"
            //                , ex.GetType().ToString() + Environment.NewLine + ex.Message,
            //                ex.InnerException,
            //                ex.Source,
            //                ex.StackTrace,
            //                ex.TargetSite));


        }

        private static void handleException(Exception ex, Exception innerEx)
        {
            TblLog.CreateLog(innerEx);

            if (ex is Common.ContextSaveException)
            {
                var obj = new ViewModel.ActivityDefinition.Popup.PopupDataObject(content: ex.Message, title: "خطا"
                    , messageBoxType: ViewModel.ActivityDefinition.Popup.MessageBoxType.Error);
                Menu.RaisePopupStatic(obj, null, null);
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                var obj = new ViewModel.ActivityDefinition.Popup.PopupDataObject(content: "خطایی پیش بینی نشده در نرم افزار بوجودآمده است.", title: "خطا"
    , messageBoxType: ViewModel.ActivityDefinition.Popup.MessageBoxType.Error);
                Menu.RaisePopupStatic(obj, null, null);

            }
        }

        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            this.Apply(Elysium.Theme.Light, FindResource("appClr1") as SolidColorBrush, Brushes.White);
        }

        public static void ApplyTheme(SolidColorBrush brush)
        {
            currentApp.Apply(Elysium.Theme.Light, brush, Brushes.White);
        }


        public static void applyUiSettings()
        {
            var c = parseColor(Settings.Default.appClr1_c);
            App.Current.Resources.Remove("appClr1_c");
            App.Current.Resources.Add("appClr1_c", c);

            App.Current.Resources.Remove("appClr1");
            App.Current.Resources.Add("appClr1", new SolidColorBrush(c));


            c = parseColor(Settings.Default.appClr2_c);
            App.Current.Resources.Remove("appClr2_c");
            App.Current.Resources.Add("appClr2_c", c);

            App.Current.Resources.Remove("appClr2");
            App.Current.Resources.Add("appClr2", new SolidColorBrush(c));

            c = parseColor(Settings.Default.fontClr_c);
            App.Current.Resources.Remove("fontClr_c");
            App.Current.Resources.Add("fontClr_c", c);

            App.Current.Resources.Remove("fontClr");
            App.Current.Resources.Add("fontClr", new SolidColorBrush(c));

            c = parseColor(Settings.Default.appClr1Opac_c);
            App.Current.Resources.Remove("appClr1Opac_c");
            App.Current.Resources.Add("appClr1Opac_c", c);

            App.Current.Resources.Remove("appClr1Opac");
            App.Current.Resources.Add("appClr1Opac", new SolidColorBrush(c));
        }


        static Color parseColor(string clrStr)
        {
            Color c = new Color();

            try
            {
                string clr = clrStr.Substring(1);

                byte alpha = Convert.ToByte(clr.Substring(0, 2), 16);

                byte r = Convert.ToByte(clr.Substring(2, 2), 16);

                byte g = Convert.ToByte(clr.Substring(4, 2), 16);

                byte b = Convert.ToByte(clr.Substring(6, 2), 16);

                c.A = alpha;

                c.R = r;

                c.G = g;

                c.B = b;

                return c;

            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException("color format invalid", ex);
            }
        }



    }

}
