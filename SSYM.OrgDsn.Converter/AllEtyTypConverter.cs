using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class AllEtyTypConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (string)((Model.Enum.AllTypEty)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
