using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class ListItemInGroupIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IEnumerable list = (IEnumerable)values[1];
            int index = 1;

            foreach (var item in list)
            {
                if (values[2] == item)
                {
                    return (index++).ToString();
                }
                index++;
            }

            return "";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
