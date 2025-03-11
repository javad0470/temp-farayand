using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace SSYM.OrgDsn.Converter
{
    public class EvtSrtWayAwrTypeNameConverter : IValueConverter
    {

        #region ' Public Methods '

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string wayAwrType = parameter.ToString();
                switch (wayAwrType)
                {
                    case "TblWayAwr_News":
                        return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 5 && m.FldCodItm == 3).FldNamItm;
                    case "TblWayAwr_Oral":
                        return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 5 && m.FldCodItm == 2).FldNamItm;
                    case "TblWayAwr_RecvInt":
                        return PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == 5 && m.FldCodItm == 1).FldNamItm;

                    default:
                        return null;
                }
            }
            catch (Exception)
            {
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
