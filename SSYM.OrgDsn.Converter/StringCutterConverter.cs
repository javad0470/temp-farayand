using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class StringCutterConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value.ToString();

            int count = 15;

            if (parameter != null)
            {
                count = int.Parse(parameter.ToString());
            }

            str = str.Substring(0, count);

            str += " ...";

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
