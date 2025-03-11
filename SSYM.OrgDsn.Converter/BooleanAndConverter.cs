using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "bool")
            {
                try
                {
                    foreach (object value in values)
                    {
                        if (!(bool)value)
                        {
                            return false;
                        }
                    }
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }

            try
            {
                foreach (object value in values)
                {
                    if (!(bool)value)
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
