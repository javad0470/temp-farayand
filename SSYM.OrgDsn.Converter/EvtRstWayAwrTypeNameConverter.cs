using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class EvtRstWayAwrTypeNameConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return null;
            }
            string wayAwrType = parameter.ToString();
            switch (wayAwrType)
            {
                case "TblNew":
                    return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 11 && m.FldCodItm == 3).FldNamItm;
                case "TblSbjOral":
                    return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 11 && m.FldCodItm == 2).FldNamItm;
                case "TblObj":
                    return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 11 && m.FldCodItm == 1).FldNamItm;

                default:
                    return null;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
