using SSYM.OrgDsn.Model.Enum;
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
    /// Interaction logic for SlctEvtRstType.xaml
    /// </summary>
    public partial class SlctEvtRstType : UserControl
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        public SlctEvtRstType()
        {
            InitializeComponent();
            //this.IsInChangeMode = true;
            //this.WayAwrCount = 1;
        }

        static SlctEvtRstType()
        {

            IsInChangeModeProperty =
            DependencyProperty.Register(
                "IsInChangeMode",
                typeof(bool),
                typeof(SlctEvtRstType),
                new FrameworkPropertyMetadata(false, IsInChangeModeCallback)
            );

            CurrentEvtRstTypeProperty =
            DependencyProperty.Register(
                "CurrentEvtRstType",
                typeof(int),
                typeof(SlctEvtRstType),
                new FrameworkPropertyMetadata(0)
            );

            ShouldAnyCdnVisibleProperty =
                DependencyProperty.Register(
                    "ShouldAnyCdnVisible",
                    typeof(bool),
                    typeof(SlctEvtRstType),
                    new FrameworkPropertyMetadata(false, ShouldAnyCdnVisibleCallback)
                );

            ShouldCancelVisibleProperty =
                DependencyProperty.Register(
                    "ShouldCancelVisible",
                    typeof(bool),
                    typeof(SlctEvtRstType),
                    new FrameworkPropertyMetadata(false, ShouldCancelVisibleCallback)
                );

            ShouldErrCdnVisibleProperty =
                DependencyProperty.Register(
                    "ShouldErrCdnVisible",
                    typeof(bool),
                    typeof(SlctEvtRstType),
                    new FrameworkPropertyMetadata(false, ShouldErrCdnVisibleCallback)
                );


            
        }

        private static void ShouldErrCdnVisibleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtRstType).ShouldErrCdnVisible = (bool)e.NewValue;

        }

        private static void ShouldCancelVisibleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtRstType).ShouldCancelVisible = (bool)e.NewValue;
        }

        private static void ShouldAnyCdnVisibleCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtRstType).ShouldAnyCdnVisible = (bool)e.NewValue;
        }

        private static void IsInChangeModeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtRstType).IsInChangeMode = (bool)e.NewValue;
        }

        #endregion

        #region ' Properties / Commands '

        public static DependencyProperty IsInChangeModeProperty;

        public bool IsInChangeMode
        {
            get
            {
                return (bool)GetValue(IsInChangeModeProperty);
            }
            set
            {
                SetValue(IsInChangeModeProperty, value);
            }
        }

        public static DependencyProperty ShouldCancelVisibleProperty;

        public bool ShouldCancelVisible
        {
            get
            {
                return (bool)GetValue(ShouldCancelVisibleProperty);
            }
            set
            {
                SetValue(ShouldCancelVisibleProperty, value);
            }
        }


        public static DependencyProperty ShouldAnyCdnVisibleProperty;

        public bool ShouldAnyCdnVisible
        {
            get
            {
                return (bool)GetValue(ShouldAnyCdnVisibleProperty);
            }
            set
            {
                SetValue(ShouldAnyCdnVisibleProperty, value);
            }
        }


        public static DependencyProperty ShouldErrCdnVisibleProperty;

        public bool ShouldErrCdnVisible
        {
            get
            {
                return (bool)GetValue(ShouldErrCdnVisibleProperty);
            }
            set
            {
                SetValue(ShouldErrCdnVisibleProperty, value);
            }
        }



        public static DependencyProperty CurrentEvtRstTypeProperty;

        public int CurrentEvtRstType
        {
            get
            {
                return (int)GetValue(CurrentEvtRstTypeProperty);
            }
            set
            {
                SetValue(CurrentEvtRstTypeProperty, value);
            }
        }


        #endregion

        #region ' Public Methods '

        public object SelectedType { get; set; }

        public void OnSelectionChanged()
        {
            if (SelectedTypeChanged != null)
            {
                SelectedTypeChanged(this, new EventArgs());
            }
        }

        #endregion

        #region ' Private Methods '

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectedType = (sender as Button).DataContext;

            OnSelectionChanged();
        }

        #endregion

        #region ' Events '

        public event EventHandler SelectedTypeChanged;

        #endregion
    }
}
