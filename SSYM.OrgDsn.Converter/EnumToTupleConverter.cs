using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class EnumToTupleConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value.GetType().IsEnum)
            //{
            //    FieldInfo[] infosArr = value.GetType().GetFields();

            //    FieldInfo fi = infosArr.Single(m => m.GetValue(value) == value);

            //    Enum e = value as Enum;
            //    DisplayAttribute da = (DisplayAttribute)fi.GetCustomAttributes().First(m => m.GetType() == typeof(DisplayAttribute));

            //    return new Tuple<Enum, string>(e, da.Name);
            //}
            if (value == null)
            {
                return null;
            }
            Tuple<Enum, string> val = (Tuple<Enum, string>)value;

            return val.Item1;
        }

        #endregion
    }
}
