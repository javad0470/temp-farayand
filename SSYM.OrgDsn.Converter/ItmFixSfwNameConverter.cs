using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class ItmFixSfwNameConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int val = (int)value;
            return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == int.Parse(parameter.ToString()) & m.FldCodItm == val).FldNamItm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
