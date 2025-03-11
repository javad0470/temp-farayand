using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class NotConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            if (value.GetType() == typeof(int))
            {
                return (int)value == 0;
            }
            else
            {
                if (value != null)
                {
                    bool val = (bool)value;
                    return !val;
                }
                return false;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool val = (bool)value;
                return !val;
            }
            return false;
        }

        #endregion
    }
}
