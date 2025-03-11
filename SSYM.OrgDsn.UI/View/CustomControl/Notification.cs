using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SSYM.OrgDsn.UI.View.CustomControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SSYM.OrgDsn.UI.View.CustomControl;assembly=SSYM.OrgDsn.UI.View.CustomControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Notification/>
    ///
    /// </summary>
    public class Notification : Control
    {

        MessageBoxType _status;

        public Notification()
        {
        }
        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Notification), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null), null);

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Notification), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null), null);

        public void Show(MessageBoxType status, bool autoHide = false, int hideAfter = 4000)
        {
            if (this.Status != status)
            {
                this.Status = status;
            }

            var storyBoard = (this.Template.FindName("mainBrd", this) as Border).FindResource("showNotification") as Storyboard;
            storyBoard.Begin();

            if (autoHide)
            {
                Task hideTask = new Task(hideStoryBoard, hideAfter);
                hideTask.Start();
            }
        }

        private void hideStoryBoard(object obj)
        {
            int waitTime = (int)obj;
            Thread.Sleep(waitTime);

            this.Dispatcher.Invoke(new Action(() =>
                 {
                     var storyBoard = (this.Template.FindName("mainBrd", this) as Border).FindResource("hideNotification") as Storyboard;
                     storyBoard.Begin();

                 }));
        }

        public void Hide(int hideAfter = 0)
        {

            Task hideTask = new Task(hideStoryBoard, hideAfter);
            hideTask.Start();
        }

        public ImageSource Source
        {
            get
            {
                return (ImageSource)base.GetValue(SourceProperty);
            }
            set
            {
                base.SetValue(SourceProperty, value);
            }
        }

        public string Text
        {
            get
            {

                return (string)base.GetValue(TextProperty);
            }
            set
            {
                base.SetValue(TextProperty, value);
            }
        }

        public MessageBoxType Status
        {
            get { return _status; }
            set
            {
                _status = value;

                switch (_status)
                {
                    case MessageBoxType.Error:
                        this.Foreground = App.Current.FindResource("MenuStatusBar_Error") as SolidColorBrush;
                        this.Background = App.Current.FindResource("Error") as SolidColorBrush;

                        //this.Source = App.Current.FindResource("error_WE") as BitmapImage;
                        break;
                    case MessageBoxType.Information:
                        this.Foreground = App.Current.FindResource("MenuStatusBar_Notification") as SolidColorBrush;
                        this.Background = App.Current.FindResource("Information") as SolidColorBrush;

                        //this.Background = App.Current.FindResource("MenuStatusBar_Notification") as SolidColorBrush;
                        //this.Source = App.Current.FindResource("info_WE") as BitmapImage;
                        break;
                    case MessageBoxType.Warning:
                        this.Foreground = App.Current.FindResource("MenuStatusBar_Warning") as SolidColorBrush;
                        this.Background = App.Current.FindResource("Warning") as SolidColorBrush;

                        //this.Background = App.Current.FindResource("MenuStatusBar_Warning") as SolidColorBrush;
                        //this.Source = App.Current.FindResource("warning_WE") as BitmapImage;
                        break;
                    case MessageBoxType.Question:
                        this.Foreground = App.Current.FindResource("MenuStatusBar_Help") as SolidColorBrush;
                        this.Background = App.Current.FindResource("SmallHelp") as SolidColorBrush;

                        //this.Background = App.Current.FindResource("MenuStatusBar_Help") as SolidColorBrush;
                        //this.Source = App.Current.FindResource("help_WE") as BitmapImage;
                        break;
                    default:
                        break;
                }

            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var brd = this.Template.FindName("mainBrd", this);
        }

    }
}
