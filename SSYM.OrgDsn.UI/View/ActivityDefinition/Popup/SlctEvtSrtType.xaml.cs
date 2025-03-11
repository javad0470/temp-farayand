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
    /// Interaction logic for SlctEvtSrtType.xaml
    /// </summary>
    public partial class SlctEvtSrtType : UserControl
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        public SlctEvtSrtType()
        {
            InitializeComponent();
            //this.IsInChangeMode = true;
            //this.WayAwrCount = 1;
        }

        static SlctEvtSrtType()
        {
            WayAwrCountProperty =
            DependencyProperty.Register(
                "WayAwrCount",
                typeof(int),
                typeof(SlctEvtSrtType),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure
                      | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure
                      | FrameworkPropertyMetadataOptions.AffectsRender, callback2, coerce)
            );

            IsInChangeModeProperty =
            DependencyProperty.Register(
                "IsInChangeMode",
                typeof(bool),
                typeof(SlctEvtSrtType),
                new FrameworkPropertyMetadata(false, callback1)
            );

            CurrentEvtSrtTypeProperty =
            DependencyProperty.Register(
                "CurrentEvtSrtType",
                typeof(int),
                typeof(SlctEvtSrtType),
                new FrameworkPropertyMetadata(0)
            );


        }

        private static object coerce(DependencyObject d, object baseValue)
        {
            return (int)baseValue;
        }

        private static void callback2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtSrtType).WayAwrCount = (int)e.NewValue;
        }

        private static void callback1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SlctEvtSrtType).IsInChangeMode = (bool)e.NewValue;
        }

        #endregion

        #region ' Properties / Commands '

        //public static DependencyProperty WayAwrCount = 

        public static DependencyProperty WayAwrCountProperty;

        public int WayAwrCount
        {
            get
            {
                return (int)GetValue(WayAwrCountProperty);
            }
            set
            {
                SetValue(WayAwrCountProperty, value);
            }
        }

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

        public static DependencyProperty CurrentEvtSrtTypeProperty;

        public int CurrentEvtSrtType
        {
            get
            {
                return (int)GetValue(CurrentEvtSrtTypeProperty);
            }
            set
            {
                SetValue(CurrentEvtSrtTypeProperty, value);
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
