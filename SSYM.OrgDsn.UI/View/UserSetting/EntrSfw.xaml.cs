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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using SSYM.OrgDsn.ViewModel.Utility;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.Layout.Hierarchic;
using yWorks.yFiles.UI.Model;
using yWorks.yFiles.Layout;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.Properties;
using System.Threading;
using SSYM.OrgDsn.ViewModel;
using System.Windows.Threading;

namespace SSYM.OrgDsn.UI.View.UserSetting
{
    /// <summary>
    /// Interaction logic for EntrSfw.xaml
    /// </summary>
    public partial class EntrSfw : Window
    {
        public EntrSfw()
        {
            InitializeComponent();

            this.Loaded += EntrSfw_Loaded;
        }

        void EntrSfw_Loaded(object sender, RoutedEventArgs e)
        {
            App.applyUiSettings();

            Util.LcsSfw = null;

            if (System.Diagnostics.Debugger.IsAttached || Settings.Default.UndrDvlp)
            {
                ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.COMPELETE;
                Util.LcsSfw = new License() { MaxTnoPosPst = -1, NamOrg = "", TnoAct = -1, TnoOrgSub = -1, TnoPrs = -1,TnoUsr = -1,TnoNod = -1};
            }
            else
            {
                initialChecks();
            }

            initForm();

            this.Loaded -= EntrSfw_Loaded;
        }

        private void initForm()
        {
            txtUsrNam.Text = Settings.Default.userName;

            if (Settings.Default.remember)
            {
                chkRemember.IsChecked = true;
                pwr.Password = Settings.Default.password;
            }
            else
            {
                chkRemember.IsChecked = false;
            }
        }

        private static void initialChecks()
        {
            ViewModel.Util.IsLocalPathExeVrsnSrv = !Util.IsRunningFromNetworkDrive();

            ViewModel.Util.IsInslVrsnClntOnnSysCnt = Util.calculateIsInslVrsnClntOnnSysCnt();

            if (!ViewModel.Util.chkSfwLcs())
            {
                MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                App.Current.Shutdown();
            }
            if (!Util.chkTypVrsn(UIUtil.getCurrentLocation()))
            {
                MessageBox.Show("بنا به دلایل امنیتی، استفاده از نرم افزار مقدور نیست. Version Not avalable ", "", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                App.Current.Shutdown();
                
            }

            if (!Util.chkHidClntSrv())
            {
                MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                App.Current.Shutdown();
            }

            Util.chgTypVrsnCmpltIffNotLocal();
        }

        private void EnterSfw(object sender, RoutedEventArgs e)
        {
            ViewModel.UserSetting.EntrSfwViewModel vm = this.DataContext as ViewModel.UserSetting.EntrSfwViewModel;

            vm.PrepareContext();

            TblUsr usr = vm.ValidateUserPass(txtUsrNam.Text);

            if (usr != null)
            {

                if (usr.FldPassUsr == TblUsr.CalculateMD5Hash(pwr.Password))
                {

                    if (!usr.FldNamUsrActv)
                    {
                        txtUsrActv.Text = "این نام کاربری غیر فعال است.";
                        txtUsrActv.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        return;
                    }


                    //Model.TblUsr usr = (this.DataContext as ViewModel.UserSetting.EntrSfwViewModel).Usr;

                    //usr.AcsUsr.HasAcs.Clear();

                    //(this.DataContext as ViewModel.UserSetting.EntrSfwViewModel).Usr = null;

                    Model.PublicMethods.DetectAllItmAcs(SSYM.OrgDsn.ViewModel.MenuViewModel.MainContext);

                    //(this.DataContext as ViewModel.UserSetting.EntrSfwViewModel).Usr = usr;

                    if (Model.PublicMethods.CurrentUser != null)
                    {
                        Model.PublicMethods.CurrentUser.AcsUsr.ExeAcsWotEtyMom_22061();
                    }

                    Menu m = new Menu();

                    m.DataContext = (this.DataContext as ViewModel.UserSetting.EntrSfwViewModel).MenuVM;

                    PublicMethods.DeleteAllPrsWotAct_19381(SSYM.OrgDsn.ViewModel.MenuViewModel.MainContext);

                    Settings.Default.userName = txtUsrNam.Text;

                    if (chkRemember.IsChecked.HasValue && chkRemember.IsChecked.Value)
                    {
                        Settings.Default.password = pwr.Password;
                        Settings.Default.remember = true;
                    }
                    else
                    {
                        Settings.Default.remember = false;
                        Settings.Default.password = null;
                    }

                    Settings.Default.Save();

                    this.Close();

                    m.ShowDialog();
                }
                else
                {
                    showWrongUserPass();
                }
            }
            else
            {
                showWrongUserPass();
            }

        }

        private void showWrongUserPass()
        {
            txtUsrActv.Text = "نام کاربری یا کلمه عبور نامعتبر است.";
            txtUsrActv.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            imgOrg.Source = FindResource("errorLogin") as ImageSource;
            imgOrg.Visibility = System.Windows.Visibility.Visible;
        }

        private void txtUsrNam_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            ViewModel.UserSetting.EntrSfwViewModel vm = this.DataContext as ViewModel.UserSetting.EntrSfwViewModel;

            TblUsr usr = vm.ValidateUserPass(txtUsrNam.Text);

            if (usr != null)
            {
                showOrg(usr);
            }
            else
            {
                hideOrg();
            }
        }

        private void hideOrg()
        {
            txtUsrActv.Text = null;
            imgOrg.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void showOrg(TblUsr usr)
        {
            txtUsrActv.Text = usr.TblOrg.FldNamOrg;
            //txtUsrActv.Foreground = FindResource("publicGreenColor") as SolidColorBrush;
            imgOrg.Source = FindResource("org_E") as ImageSource;
            imgOrg.Visibility = System.Windows.Visibility.Visible;
        }

        //private void grph_Loaded_1(object sender, RoutedEventArgs e)
        //{
        //    grph.Graph.CreateNode(new yWorks.Canvas.Geometry.Structs.RectD(2, 2, 2, 2), new ShapeNodeStyle(), 1);

        //    // create the layouter
        //    var ihl = new IncrementalHierarchicLayouter()
        //    {
        //        ComponentLayouterEnabled = false,

        //        LayoutOrientation = yWorks.yFiles.Layout.LayoutOrientation.LeftToRight,
        //        OrthogonalRouting = true,
        //        RecursiveGroupLayering = false,
        //        EdgeLayoutDescriptor =
        //        {
        //            MinimumFirstSegmentLength = 15,
        //            MinimumLastSegmentLength = 15,
        //            OrthogonallyRouted = true,
        //            MinimumDistance = 10
        //        },
        //        NodeLayoutDescriptor =
        //        {
        //            LayerAlignment = 0.5
        //        },
        //        ConsiderNodeLabels = true,
        //        IntegratedEdgeLabeling = true,
        //    };

        //    //We use Layout executor convenience method that already sets up the whole layout pipeline correctly
        //    new LayoutExecutor(grph, ihl)
        //    {
        //        //Table layout is enabled by default already...
        //        ConfigureTableNodeLayout = true,
        //        Duration = TimeSpan.FromMilliseconds(500),
        //        AnimateViewport = true,
        //        UpdateContentRect = true,
        //        TableLayoutConfigurator =
        //        {
        //            //Table cells may only grow by an automatic layout.
        //            CompactionEnabled = false,
        //            //Keep the order of pool nodes as in the sketch
        //            FromSketch = true
        //        },
        //        RunInThread = false,
        //        FinishHandler = (o, args) =>
        //        {
        //            grph.Graph.MapperRegistry.RemoveMapper(PortCandidateSet.NodeDpKey);
        //            grph.Graph.MapperRegistry.RemoveMapper(PortConstraintKeys.TargetPortConstraintDpKey);
        //        }
        //    }.Start();

        //}

        /// <summary>
        /// 
        /// </summary>



        void a()
        {
            //// The Work to perform on another thread 
            //ThreadStart start = delegate() { // ... // Sets the Text on a TextBlock Control. // This will work as its using the dispatcher 
            //    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<string>(SetStatus), "From Other Thread"); };
            //// Create the thread and kick it started! new Thread(start).Start();




            //// The Work to perform on another thread 
            
            //ThreadStart start = delegate() { // ... // This will work as its using the dispatcher
            //    DispatcherOperation op = Dispatcher.BeginInvoke( DispatcherPriority.Normal, new Action<string>(SetStatus), "From Other Thread (Async)"); 
            //    DispatcherOperationStatus status = op.Status; while (status != DispatcherOperationStatus.Completed) 
            //    { status = op.Wait(TimeSpan.FromMilliseconds(1000)); if (status == DispatcherOperationStatus.Aborted) 
            //    { // Alert Someone } } }; // Create the thread and kick it started! 
            //        new Thread(start).Start();
        }

        private void WindowBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void bClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void bMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
