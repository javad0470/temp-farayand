using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class PersianDateConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PersianLibrary.PersianDate dte = PersianLibrary.PersianDate.Convert((DateTime)value);
            return string.Format("{0} {1} ساعت {2}", dte.DayOfWeek, dte.DateString, ((DateTime)value).ToShortTimeString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
