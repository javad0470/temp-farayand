using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class AndVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                foreach (object value in values)
                {
                    if ((Visibility)value == Visibility.Collapsed)
                    {
                        return Visibility.Collapsed;
                    }
                }
                return Visibility.Visible;

            }
            catch (Exception)
            {
                return Visibility.Collapsed;
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
