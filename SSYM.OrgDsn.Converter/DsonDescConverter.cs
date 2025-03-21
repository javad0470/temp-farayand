﻿using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class DsonDescConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return PublicMethods.GetDsonDesc(value as SSYM.OrgDsn.Model.Base.IWayAwrIfrm);
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
