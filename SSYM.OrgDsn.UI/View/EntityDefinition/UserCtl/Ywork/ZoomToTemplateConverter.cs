/****************************************************************************
 **
 ** This file is part of yFiles WPF 2.4.
 ** 
 ** yWorks proprietary/confidential. Use is subject to license terms.
 **
 ** Redistribution of this file or of an unauthorized byte-code version
 ** of this file is strictly forbidden.
 **
 ** Copyright (c) 2003-2013 by yWorks GmbH, Vor dem Kreuzberg 28, 
 ** 72070 Tuebingen, Germany. All rights reserved.
 **
 ***************************************************************************/
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl.Ywork
{
    /// <summary>
    /// symbolic zoom levels
    /// </summary>
    public enum ZoomLevel
    {
        Detail, Intermediate, Overview
    }

    /// <summary>
    /// Converter which converts from a double zoom value to a symbolic <see cref="ZoomLevel"/>.
    /// </summary>
    /// <remarks>
    /// Two thresholds can be set (<see cref="DetailThreshold"/> and <see cref="OverviewThreshold"/>),
    /// which define the intervals of the incoming zoom value which are mapped to one of the three
    /// symbolic zoom levels.
    /// </remarks>
    [ValueConversion(typeof(double), typeof(ZoomLevel))]
    public class ZoomToZoomLevelConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the detail threshold. A zoom value greater than or equal to this
        /// threshold is mapped to the <see cref="ZoomLevel.Detail"/> zoom level.
        /// </summary>
        public double DetailThreshold { get; set; }

        /// <summary>
        /// Gets or sets the overview threshold. A zoom value less than or equal to this
        /// threshold is mapped to the <see cref="ZoomLevel.Overview"/> zoom level.
        /// </summary>
        public double OverviewThreshold { get; set; }

        public ZoomToZoomLevelConverter()
        {
            DetailThreshold = 0.7;
            OverviewThreshold = 0.2;
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double zoom = (double)value;
                if (zoom >= DetailThreshold)
                {
                    return ZoomLevel.Detail;
                }
                if (zoom <= OverviewThreshold)
                {
                    return ZoomLevel.Overview;
                }

            }
            return ZoomLevel.Intermediate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    /// <summary>
    /// This converter maps the icon attribute of the business data for an employee to an image source.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class NameToImageSourceConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "pack://application:,,,/Resources/" + value + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// This converter maps a boolean value to a Visibility
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// A converter that converts between zoom levels and DataTemplates
    /// </summary>
    public class ZoomToTemplateConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the detail node style template.
        /// </summary>
        /// <value>The detail node style template.</value>
        public DataTemplate DetailTemplate { get; set; }

        /// <summary>
        /// Gets or sets the overview node style template.
        /// </summary>
        /// <value>The overview node style template.</value>
        public DataTemplate OverviewTemplate { get; set; }

        /// <summary>
        /// Gets or sets the normal node style template.
        /// </summary>
        /// <value>The normal node style template.</value>
        public DataTemplate NormalTemplate { get; set; }

        /// <summary>
        /// Gets or sets the detail threshold. A zoom value greater than or equal to this
        /// threshold is mapped to the <see cref="DetailTemplate"/> zoom level.
        /// </summary>
        public double DetailThreshold { get; set; }

        /// <summary>
        /// Gets or sets the overview threshold. A zoom value less than or equal to this
        /// threshold is mapped to the <see cref="OverviewTemplate"/> zoom level.
        /// </summary>
        public double OverviewThreshold { get; set; }

        public ZoomToTemplateConverter()
        {
            DetailThreshold = 0.7;
            OverviewThreshold = 0.2;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double zoom = (double)value;
                if (zoom >= DetailThreshold)
                {
                    return DetailTemplate;
                }
                if (zoom <= OverviewThreshold)
                {
                    return OverviewTemplate;
                }
            }
            return NormalTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedItemConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
