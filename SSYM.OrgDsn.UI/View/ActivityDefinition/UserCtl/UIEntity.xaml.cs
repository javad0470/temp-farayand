using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Reflection;
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
using Telerik.Windows.DragDrop;
using ThicknessConverter = Xceed.Wpf.DataGrid.Converters.ThicknessConverter;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for UIEntity.xaml
    /// </summary>
    public partial class UIEntity : UserControl
    {

        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        public UIEntity()
        {
            InitializeComponent();
            //main.Background = Background;
            this.DataContextChanged += UIEntity_DataContextChanged;
            
            DragDropManager.AddDragInitializeHandler(main, OnDragInitialize);

            DragDropManager.AddGiveFeedbackHandler(main, OnGiveFeedback);

        }

        void UIEntity_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (this.DataContext as EntityObject).PropertyChanged -= UIEntity_PropertyChanged;
            (this.DataContext as EntityObject).PropertyChanged += UIEntity_PropertyChanged;
        }

        void UIEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                dynamic d = sender;
                //main.Background = Background;
                if (d.IsSelected) main.Foreground = FindResource("appClr2") as SolidColorBrush;
                else main.Foreground = FindResource("appClr1") as SolidColorBrush;

            }
        }

        static UIEntity()
        {
            //MainImageProperty = DependencyProperty.Register("MainImage",
            //    typeof(ImageSource), typeof(UIEntity), new FrameworkPropertyMetadata(null, callback)
            //   );
            
            LeftImageProperty = DependencyProperty.Register("LeftImage",
                typeof(ImageSource), typeof(UIEntity), new FrameworkPropertyMetadata(null, LeftImageCallback)
               );

            MainImageBackgroundProperty = DependencyProperty.Register("MainImageBackground",
    typeof(Brush), typeof(UIEntity), new FrameworkPropertyMetadata(null, MainImageCallback)
   );

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.Copy;

            var originalbtn = main;

            Button btn = new Button() { BorderBrush = originalbtn.BorderBrush };

            btn.Height = originalbtn.Height;

            btn.Width = originalbtn.Width;

            btn.Foreground = originalbtn.Foreground;

            //Border bd = new Border();

            //bd.ApplyTemplate();

            //bd.Child = img;

            //bd.Background = Brushes.Black;

            args.Data = this.DataContext;

            args.DragVisual = new ContentControl() { Content = btn };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;

        }

        #endregion

        #region ' Properties / Commands '

        public object ToolTip
        {
            get
            {
                return main.ToolTip;
            }
            set
            {
                main.ToolTip = value;
            }
        }

        public ImageSource TopImage
        {
            get { return (top.Content as BackgroundedImage).Source; }
            set
            {
                (top.Content as BackgroundedImage).Source = value;
                if (value == null)
                {
                    top.Visibility = Visibility.Collapsed;
                }
                else
                {
                    top.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public ImageSource BottomImage
        {
            get { return (bottom.Content as BackgroundedImage).Source; }
            set
            {
                (bottom.Content as BackgroundedImage).Source = value;

                if (value == null)
                {
                    bottom.Visibility = Visibility.Collapsed;
                }
                else
                {
                    bottom.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        //public ImageSource LeftImage
        //{
        //    get { return (left.Content as BackgroundedImage).Source; }
        //    set
        //    {
        //        (left.Content as BackgroundedImage).Source = value;
        //        if (value == null)
        //        {
        //            left.Visibility = Visibility.Hidden;
        //        }
        //        else
        //        {
        //            left.Visibility = System.Windows.Visibility.Visible;
        //        }
        //    }
        //}

        public ImageSource RightImage
        {
            get { return (right.Content as BackgroundedImage).Source; }
            set
            {
                (right.Content as BackgroundedImage).Source = value;
                if (value == null)
                {
                    right.Visibility = Visibility.Hidden;
                }
                else
                {
                    right.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }


        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(
                "DeleteCommand",
                typeof(ICommand),
                typeof(UIEntity)
            );

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(UIEntity)
        );

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        //public static readonly DependencyProperty MainImageProperty;

        //public ImageSource MainImage
        //{
        //    set { SetValue(MainImageProperty, value); }
        //    get { return (ImageSource)GetValue(MainImageProperty); }
        //}

        public static readonly DependencyProperty LeftImageProperty;

        public ImageSource LeftImage
        {
            set { SetValue(LeftImageProperty, value); }
            get { return (ImageSource)GetValue(LeftImageProperty); }
        }


        public static readonly DependencyProperty MainImageBackgroundProperty;

        public Brush MainImageBackground
        {
            set { SetValue(LeftImageProperty, value); }
            get { return (Brush)GetValue(LeftImageProperty); }
        }


        //public bool IsCircle
        //{
        //    get { return true;} //(main.Content as Button).IsCircle;
            
        //    set
        //    {

        //        if (!value)
        //        {
        //            (main.Content as Button).Style = null;
        //            (left.Content as Button).Style = null;
        //            main.Width = 30;
        //            main.Height = 30;
        //            left.Width = 30;
        //            left.Height = 30;
        //        }
        //        //(main.Content as Button).IsCircle = value;
        //        //(left.Content as BackgroundedImage).IsCircle = value;
        //    }
        //}

        #endregion

        #region ' Public Methods '

        public void Focus()
        {
            main.Focus();
        }

        public void OnSelectionChanged()
        {
            if (Deleted != null)
            {
                Deleted(this, new EventArgs());
            }
        }

        #endregion

        #region ' Private Methods '
        //private void OnDragEnter(object sender, DragEventArgs e)
        //{

        //}

        private static void LeftImageCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((d as UIEntity).left.Content as BackgroundedImage).Source = e.NewValue as ImageSource;
        }

        private static void callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //
            //(d as UIEntity).main.Background = e.NewValue as ImageBrush;
        }


        private static void MainImageCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ety = d as UIEntity;
            ety.main.Foreground = e.NewValue as Brush;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (DeleteCommand != null)
            {
                DeleteCommand.Execute(this.CommandParameter);
            }
        }

        #endregion

        #region ' Event '

        public event EventHandler Deleted;

        public event RoutedEventHandler TopClicked
        {
            add
            {
                this.top.Click += value;
            }
            remove
            {
                this.top.Click -= value;
            }
        }

        public event RoutedEventHandler LeftClicked
        {
            add
            {
                this.left.Click += value;
            }
            remove
            {
                this.left.Click -= value;
            }
        }

        public event RoutedEventHandler BottomClicked
        {
            add
            {
                this.bottom.Click += value;
            }
            remove
            {
                this.bottom.Click -= value;
            }
        }

        public event RoutedEventHandler RightClicked
        {
            add
            {
                this.right.Click += value;
            }
            remove
            {
                this.right.Click -= value;
            }
        }


        #endregion

        //#region ' Drag Drop '


        //Point _startPoint;
        //bool IsDragging;

        //private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    _startPoint = e.GetPosition(null);
        //}

        //private void Image_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
        //    {
        //        Point position = e.GetPosition(null);

        //        if (Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //            Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
        //        {
        //            StartDrag(e);
        //        }
        //    }
        //}

        //private void StartDrag(MouseEventArgs e)
        //{
        //    IsDragging = true;
        //    DataObject data = new DataObject(this.DataContext);
        //    DragDropEffects de = DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
        //    IsDragging = false;
        //}

        //#endregion

        private void grdMain_Drop_1(object sender, System.Windows.DragEventArgs e)
        {
            try
            {
                TblEvtSrt s = (TblEvtSrt)e.Data.GetData(typeof(TblEvtSrt));

                if (this.DataContext is TblEvtSrt)
                {
                    (this.Tag as ActDgrmViewModel).ChangeGroup(s, (this.DataContext as TblEvtSrt));
                    //s.FldGrpEvt = (this.DataContext as TblEvtSrt).FldGrpEvt;
                }

            }
            catch (Exception)
            {
            }
        }

        private void UserControl_GotFocus_1(object sender, RoutedEventArgs e)
        {
            //(sender as BackgroundedImage).Background = FindResource("appClr2") as SolidColorBrush;
        }

        private void main_Click_1(object sender, RoutedEventArgs e)
        {
            ((sender as Button).Content as Button).Focus();
        }

        private void BackgroundedImage_LostFocus_1(object sender, RoutedEventArgs e)
        {
            //(sender as Button).Foreground = FindResource("appClr1") as SolidColorBrush;
        }


        //public ImageSource MainImage
        //{
        //    get { return (main.Content as BackgroundedImage).Source; }
        //    set
        //    {
        //        (main.Content as BackgroundedImage).Source = value;
        //    }
        //}
    }
}
