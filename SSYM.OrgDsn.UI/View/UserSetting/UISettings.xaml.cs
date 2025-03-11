using SSYM.OrgDsn.UI.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.UserSetting
{
    /// <summary>
    /// Interaction logic for UISettings.xaml
    /// </summary>
    public partial class UISettings
    {
        public UISettings()
        {
            InitializeComponent();

            this.Loaded += UISettings_Loaded;
        }

        void UISettings_Loaded(object sender, RoutedEventArgs e)
        {
            refreshUI();
        }

        private void refreshUI()
        {

            switch (Settings.Default.Theme)
            {
                case "1":
                    Theme2.IsChecked = false;
                    Theme2.Tag = "0";
                    Theme3.IsChecked = false;
                    Theme3.Tag = "0";
                    Theme1.IsChecked = true;
                    Theme1.Tag = "1";
                    break;
                case "2":
                    Theme1.IsChecked = false;
                    Theme1.Tag = "0";
                    Theme3.IsChecked = false;
                    Theme3.Tag = "0";
                    Theme2.IsChecked = true;
                    Theme2.Tag = "1";
                    break;
                case "3":
                    Theme2.IsChecked = false;
                    Theme2.Tag = "0";
                    Theme1.IsChecked = false;
                    Theme1.Tag = "0";
                    Theme3.IsChecked = true;
                    Theme3.Tag = "1";
                    break;
            }

            //var clr1 = System.Drawing.ColorTranslator.FromHtml(Settings.Default.appClr1_c);
            //Color c1 = Color.FromRgb(clr1.R, clr1.G, clr1.B);

            //setR0(c1);


            //var clr2 = System.Drawing.ColorTranslator.FromHtml(Settings.Default.appClr2_c);
            //Color c2 = Color.FromRgb(clr2.R, clr2.G, clr2.B);

            //setR1(c2);

            //var clr3 = System.Drawing.ColorTranslator.FromHtml(Settings.Default.fontClr_c);
            //Color c3 = Color.FromRgb(clr3.R, clr3.G, clr3.B);

            //setR2(c3);
        }

        private void setR2(Color c1)
        {
            r21.Width = r22.Width = r23.Width = r24.Width = 50;
            r21.Height = r22.Height = r23.Height = r24.Height = 50;

            if ((r21.Fill as SolidColorBrush).Color == c1)
            {
                r21.Width = r21.Height = 70;
            }

            if ((r22.Fill as SolidColorBrush).Color == c1)
            {
                r22.Width = r22.Height = 70;
            }


            if ((r23.Fill as SolidColorBrush).Color == c1)
            {
                r23.Width = r23.Height = 70;
            }


            if ((r24.Fill as SolidColorBrush).Color == c1)
            {
                r24.Width = r24.Height = 70;
            }

        }

        private void setR1(Color c1)
        {
            r11.Width = r12.Width = r13.Width = r14.Width = 50;
            r11.Height = r12.Height = r13.Height = r14.Height = 50;

            if ((r11.Fill as SolidColorBrush).Color == c1)
            {
                r11.Width = r11.Height = 70;
            }

            if ((r12.Fill as SolidColorBrush).Color == c1)
            {
                r12.Width = r12.Height = 70;
            }


            if ((r13.Fill as SolidColorBrush).Color == c1)
            {
                r13.Width = r13.Height = 70;
            }


            if ((r14.Fill as SolidColorBrush).Color == c1)
            {
                r14.Width = r14.Height = 70;
            }
        }

        private void setR0(Color c1)
        {
            r01.Width = r02.Width = r03.Width = r04.Width = 50;
            r01.Height = r02.Height = r03.Height = r04.Height = 50;

            if ((r01.Fill as SolidColorBrush).Color == c1)
            {
                r01.Width = r01.Height = 70;
            }

            if ((r02.Fill as SolidColorBrush).Color == c1)
            {
                r02.Width = r02.Height = 70;
            }


            if ((r03.Fill as SolidColorBrush).Color == c1)
            {
                r03.Width = r03.Height = 70;
            }


            if ((r04.Fill as SolidColorBrush).Color == c1)
            {
                r04.Width = r04.Height = 70;
            }
        }

        private void Rectangle_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
            App.Current.Resources.Remove("appClr1");
            App.Current.Resources.Add("appClr1", new SolidColorBrush(b.Color));

            App.Current.Resources.Remove("appClr1_c");
            App.Current.Resources.Add("appClr1_c", b.Color);

            Color c = b.Color;

            tbkC1.Foreground = new SolidColorBrush(c);

            Settings.Default.appClr1_c = c.ToString();

            c = new Color() { A = (byte)(b.Color.A - 85), R = b.Color.R, B = b.Color.B, G = b.Color.G };

            App.Current.Resources.Remove("appClr1Opac_c");
            App.Current.Resources.Add("appClr1Opac_c", c);


            App.Current.Resources.Remove("appClr1Opac");
            App.Current.Resources.Add("appClr1Opac", new SolidColorBrush(c));

            Settings.Default.appClr1Opac_c = c.ToString();

            Settings.Default.Save();
            //appClr1Opac_c


            App.ApplyTheme(b);

            refreshUI();

        }

        private void Rectangle_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
            App.Current.Resources.Remove("appClr2");
            App.Current.Resources.Add("appClr2", new SolidColorBrush(b.Color));

            App.Current.Resources.Remove("appClr2_c");
            App.Current.Resources.Add("appClr2_c", b.Color);

            Settings.Default.appClr2_c = b.Color.ToString();

            Settings.Default.Save();


            var c = new Color() { A = (byte)(b.Color.A - 85), R = b.Color.R, B = b.Color.B, G = b.Color.G };

            App.Current.Resources.Remove("appClr2Opac_c");
            App.Current.Resources.Add("appClr2Opac_c", c);
            App.Current.Resources.Remove("appClr2Opac");
            App.Current.Resources.Add("appClr2Opac", new SolidColorBrush(c));


            tbkC2.Foreground = b;

            refreshUI();

        }

        private void Rectangle_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
            App.Current.Resources.Remove("fontClr");
            App.Current.Resources.Add("fontClr", new SolidColorBrush(b.Color));

            App.Current.Resources.Remove("fontClr_c");
            App.Current.Resources.Add("fontClr_c", b.Color);


            Settings.Default.fontClr_c = b.Color.ToString();

            Settings.Default.Save();

            refreshUI();


        }

        private void Theme1_Checked(object sender, RoutedEventArgs e)
        {
            Theme2.IsChecked = false;
            Theme2.Tag = "0";
            Theme3.IsChecked = false;
            Theme3.Tag = "0";
            Theme1.IsChecked = true;
            Theme1.Tag = "1";

            Settings.Default.Theme = "1";
            Settings.Default.Save();


            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 56, 56, 56));
            App.Current.Resources.Remove("appClr1");
            App.Current.Resources.Add("appClr1", b);
            Settings.Default.appClr1_c = b.Color.ToString();

            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 255, 90, 0));
            App.Current.Resources.Remove("appClr2");
            App.Current.Resources.Add("appClr2", b);
            Settings.Default.appClr2_c = b.Color.ToString();

            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 56, 56, 56));
            App.Current.Resources.Remove("appClr1_c");
            App.Current.Resources.Add("appClr1_c", b.Color);

            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 255, 90, 0));
            App.Current.Resources.Remove("appClr2_c");
            App.Current.Resources.Add("appClr2_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 56, 56, 56));
            App.Current.Resources.Remove("appClr1Opac");
            App.Current.Resources.Add("appClr1Opac", b);

            Settings.Default.appClr1Opac_c = b.Color.ToString();

            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 255, 90, 0));
            App.Current.Resources.Remove("appClr2Opac");
            App.Current.Resources.Add("appClr2Opac", b);

            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 56, 56, 56));
            App.Current.Resources.Remove("appClr1Opac_c");
            App.Current.Resources.Add("appClr1Opac_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 255, 90, 0));
            App.Current.Resources.Remove("appClr2Opac_c");
            App.Current.Resources.Add("appClr2Opac_c", b.Color);
            App.ApplyTheme(b);

            Settings.Default.Save();
        }

        private void Theme2_Checked(object sender, RoutedEventArgs e)
        {
            Theme1.IsChecked = false;
            Theme1.Tag = "0";
            Theme3.IsChecked = false;
            Theme3.Tag = "0";
            Theme2.IsChecked = true;
            Theme2.Tag = "1";

            Settings.Default.Theme = "2";
            Settings.Default.Save();


            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 37, 107, 127));
            App.Current.Resources.Remove("appClr1");
            App.Current.Resources.Add("appClr1", b);
            Settings.Default.appClr1_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 50, 200, 230));
            App.Current.Resources.Remove("appClr2");
            App.Current.Resources.Add("appClr2", b);
            Settings.Default.appClr2_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 37, 107, 127));
            App.Current.Resources.Remove("appClr1_c");
            App.Current.Resources.Add("appClr1_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 50, 200, 230));
            App.Current.Resources.Remove("appClr2_c");
            App.Current.Resources.Add("appClr2_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 37, 107, 127));
            App.Current.Resources.Remove("appClr1Opac");
            App.Current.Resources.Add("appClr1Opac", b);

            Settings.Default.appClr1Opac_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 50, 200, 230));
            App.Current.Resources.Remove("appClr2Opac");
            App.Current.Resources.Add("appClr2Opac", b);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 37, 107, 127));
            App.Current.Resources.Remove("appClr1Opac_c");
            App.Current.Resources.Add("appClr1Opac_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 50, 200, 230));
            App.Current.Resources.Remove("appClr2Opac_c");
            App.Current.Resources.Add("appClr2Opac_c", b.Color);
            App.ApplyTheme(b);

            Settings.Default.Save();
        }

        private void Theme3_Checked(object sender, RoutedEventArgs e)
        {
            Theme1.IsChecked = false;
            Theme1.Tag = "0";
            Theme2.IsChecked = false;
            Theme2.Tag = "0";
            Theme3.IsChecked = true;
            Theme3.Tag = "1";

            Settings.Default.Theme = "3";
            Settings.Default.Save();


            SolidColorBrush b = new SolidColorBrush(Color.FromArgb(255, 22, 22, 22));
            App.Current.Resources.Remove("appClr1");
            App.Current.Resources.Add("appClr1", b);
            Settings.Default.appClr1_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 206, 0, 0));
            App.Current.Resources.Remove("appClr2");
            App.Current.Resources.Add("appClr2", b);
            Settings.Default.appClr2_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 22, 22, 22));
            App.Current.Resources.Remove("appClr1_c");
            App.Current.Resources.Add("appClr1_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(255, 206, 0, 0));
            App.Current.Resources.Remove("appClr2_c");
            App.Current.Resources.Add("appClr2_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 22, 22, 22));
            App.Current.Resources.Remove("appClr1Opac");
            App.Current.Resources.Add("appClr1Opac", b);

            Settings.Default.appClr1Opac_c = b.Color.ToString();
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 206, 0, 0));
            App.Current.Resources.Remove("appClr2Opac");
            App.Current.Resources.Add("appClr2Opac", b);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 22, 22, 22));
            App.Current.Resources.Remove("appClr1Opac_c");
            App.Current.Resources.Add("appClr1Opac_c", b.Color);
            App.ApplyTheme(b);

            b = new SolidColorBrush(Color.FromArgb(100, 206, 0, 0));
            App.Current.Resources.Remove("appClr2Opac_c");
            App.Current.Resources.Add("appClr2Opac_c", b.Color);
            App.ApplyTheme(b);

            Settings.Default.Save();
        }

        //private void Rectangle_MouseUp_1(object sender, MouseButtonEventArgs e)
        //{
        //    SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
        //    App.Current.Resources.Remove("appClr1_c");
        //    App.Current.Resources.Add("appClr1_c", b.Color);
        //    App.ApplyTheme(b);
        //}

        //private void Rectangle_MouseUp_2(object sender, MouseButtonEventArgs e)
        //{
        //    SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
        //    App.Current.Resources.Remove("appClr2_c");
        //    App.Current.Resources.Add("appClr2", b.Color);
        //}

        //private void Rectangle_MouseUp_3(object sender, MouseButtonEventArgs e)
        //{
        //    SolidColorBrush b = (sender as Rectangle).Fill as SolidColorBrush;
        //    App.Current.Resources.Remove("fontClr_c");
        //    App.Current.Resources.Add("fontClr_c", b.Color);
        //}

    }
}
