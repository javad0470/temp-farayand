using SSYM.OrgDsn.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class IntToSfwConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value != null)
            //{
            //    Type t = value.GetType();
            //    PropertyInfo p = t.GetProperty("FldCodSfw");
            //    int i = (int)p.GetValue(value);
            //    return MenuViewModel.MainContext.TblSfws.SingleOrDefault(m => m.FldCodSfw == i);
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value != null)
            //{
            //    Type t = value.GetType();
            //    PropertyInfo p = t.GetProperty("FldCodSfw");
            //    int i = (int)p.GetValue(value);
            //    return MenuViewModel.MainContext.TblSfws.SingleOrDefault(m => m.FldCodSfw == i);
            //}
            return null;
        }
    }
}
