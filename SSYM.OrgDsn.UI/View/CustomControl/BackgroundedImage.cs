using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
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
    ///     xmlns:MyNamespace="clr-namespace:SSYM.OrgDsn.UI"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SSYM.OrgDsn.UI;assembly=SSYM.OrgDsn.UI"
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
    ///     <MyNamespace:BackgroundedImage/>
    ///
    /// </summary>
    public class BackgroundedImage : Control
    {

        public ImageSource src = null;

        public BackgroundedImage()
        {
            //this.IsEnabledChanged += BackgroundedImage_IsEnabledChanged;
        }

        //void BackgroundedImage_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (this.IsEnabled)
        //    {
        //        var rect = this.Template.FindName("rect1", this) as Rectangle;
        //        rect.Fill = FindResource("appClr1") as SolidColorBrush;
        //    }
        //    else
        //    {
        //        var rect = this.Template.FindName("rect1", this) as Rectangle;
        //        rect.Fill = FindResource("appClr1Opac") as SolidColorBrush;
        //    }
        //}

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);


            if (e.Property.Name == "ActualHeight")
            {
                if (this.ActualHeight > 0)
                {
                    if (IsCircle)
                    {
                        if (this.Template != null)
                        {
                            if (!sizeChanged)
                            {
                                Image img = this.Template.FindName("img1", this) as Image;

                                img.Height = this.ActualHeight * 0.8;
                                img.Width = this.ActualHeight * 0.8;
                                img.InvalidateArrange();
                                img.InvalidateVisual();

                                sizeChanged = true;
                            }
                        }
                    }
                }
            }
        }

        bool sizeChanged = false;

        static BackgroundedImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BackgroundedImage), new FrameworkPropertyMetadata(typeof(BackgroundedImage)));

            BackgroundProperty = DependencyProperty.Register("Background",
typeof(Brush), typeof(BackgroundedImage), new FrameworkPropertyMetadata(null)
);

            HoverEnabledProperty = DependencyProperty.Register("HoverEnabled", typeof(bool), typeof(BackgroundedImage), new FrameworkPropertyMetadata(true));

            StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(BackgroundedImage), new FrameworkPropertyMetadata(Stretch.Fill));



        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }

        public static readonly DependencyProperty BackgroundProperty;

        public Brush Background
        {
            set { SetValue(BackgroundProperty, value); }
            get { return (Brush)GetValue(BackgroundProperty); }
        }

        public static readonly DependencyProperty HoverEnabledProperty;

        public bool HoverEnabled
        {
            set { SetValue(TemplateProperty, value); }
            get { return (bool)GetValue(TemplateProperty); }
        }


        public Stretch Stretch
        {
            get
            {
                return (Stretch)base.GetValue(StretchProperty);
            }
            set
            {
                base.SetValue(StretchProperty, value);
            }
        }
        public static readonly DependencyProperty StretchProperty;


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


        public bool IsCircle
        {
            set { SetValue(IsCircleProperty, value); }
            get
            {
                return (bool)GetValue(IsCircleProperty);
            }
        }


        public static readonly DependencyProperty IsCircleProperty = DependencyProperty.Register("IsCircle", typeof(bool), typeof(BackgroundedImage), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, isCircleChanged), null);

        private static void isCircleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //bool value = (bool)e.NewValue;
            //if (value)
            //{
            //    BackgroundedImage bi = d as BackgroundedImage;

            //    if (bi.Template != null)
            //    {
            //        Image img = bi.Template.FindName("img1", bi) as Image;

            //        img.Margin = new Thickness(bi.Height / 3);

            //    }

            //}
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(BackgroundedImage), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null), null);

    }
}
