using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class EnumToListConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            Type typ = (Type)value;

            if (!typ.IsEnum)
            {
                return null;
            }

            FieldInfo[] infosArr = typ.GetFields();


            List<FieldInfo> infos = new List<FieldInfo>(infosArr.Skip(1));

            var obj = Enum.GetValues(typ);

            List<Enum> values = new List<Enum>();

            foreach (var item in obj)
            {
                values.Add((Enum)item);
            }

            List<Tuple<Enum, string>> enumMembers = new List<Tuple<Enum, string>>();


            for (int i = 0; i < infos.Count(); i++)
            {
                DisplayAttribute da = (DisplayAttribute)infos[i].GetCustomAttributes(false).First(m => m.GetType() == typeof(DisplayAttribute));
                Tuple<Enum, string> t = new Tuple<Enum, string>(values[i], da.Name);

                enumMembers.Add(t);
            }

            return enumMembers;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
