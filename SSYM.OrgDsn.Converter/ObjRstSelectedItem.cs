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
    public class ObjRstSelectedItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return ((value as System.Windows.Data.CollectionViewGroup).Items[0] as Tuple<SSYM.OrgDsn.Model.Base.IObjRst, SSYM.OrgDsn.Model.TblEvtSrt>);
        }
    }
}
