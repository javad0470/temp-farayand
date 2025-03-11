using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class SizeConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double val = (double)value;
                if (parameter != null)
                {
                    string str = parameter.ToString();

                    char sign = str[0];

                    if (sign == '+')
                    {
                        return val + double.Parse(str.Substring(1), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    }
                    else if (sign == '-')
                    {
                        return val - double.Parse(str.Substring(1), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    }
                    else if (sign == '/')
                    {
                        return val / double.Parse(str.Substring(1), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    }

                    else if (sign == '*')
                    {
                        return val * double.Parse(str.Substring(1), System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    }

                    return val + double.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                }
                return val;

            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
