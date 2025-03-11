using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class SlcEvtRstVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EvtRstType fldTypEvtRst = (EvtRstType)values[0];

            bool shouldAnyCdnVisible = (bool)values[1];

            bool shouldCancelVisible = (bool)values[2];

            bool shouldErrVisible = (bool)values[3];

            int currentEvtRstType = (int)values[4];

            if (currentEvtRstType == (int)fldTypEvtRst)
            {
                return Visibility.Collapsed;
            }

            if (!shouldAnyCdnVisible && fldTypEvtRst == EvtRstType.anyCdnEvtRst)
            {
                return Visibility.Collapsed;
            }

            if (!shouldCancelVisible && fldTypEvtRst == EvtRstType.cancelEvtRst)
            {
                return Visibility.Collapsed;
            }

            if (!shouldErrVisible && fldTypEvtRst == EvtRstType.errAccurEvtRst)
            {
                return Visibility.Collapsed;
            }


            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as object[];
        }
    }
}