using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class SlcEvtSrtVisibilityConverter : IMultiValueConverter
    {
        /// <summary>
        /// Pdr9010
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue || values[2] == DependencyProperty.UnsetValue)
                return Visibility.Visible;
            int code = (int)values[0];
            int wayAwrCount = (int)values[1];
            bool isInChangeMode = (bool)values[2];
            int current = (int)values[3];

            if (code == current)
            {
                return Visibility.Collapsed;
            }

            if (isInChangeMode)
            {
                if (code == 1 || code == 2)
                {
                    if (wayAwrCount > 0)
                        return Visibility.Collapsed;
                    else
                        return Visibility.Visible;
                }
                if (code == 3)
                {
                    if (wayAwrCount > 1)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as object[];
        }
    }
}