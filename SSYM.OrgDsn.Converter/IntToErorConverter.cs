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
    public class IntToErorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value != null)
            //{
            //    Type t = value.GetType();
            //    PropertyInfo p = t.GetProperty("FldCodEror");
            //    int i = (int)p.GetValue(value);
            //    return MainWindowViewModel.MainContext.TblErors.SingleOrDefault(m => m.FldCodEror == i);
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value != null)
            //{
            //    Type t = value.GetType();
            //    PropertyInfo p = t.GetProperty("FldCodEror");
            //    int i = (int)p.GetValue(value);
            //    return MainWindowViewModel.MainContext.TblErors.SingleOrDefault(m => m.FldCodEror == i);
            //}
            return null;
        }
    }
}
