using System;
using System.Windows.Data;
using System.Windows.Media;

namespace SSYM.OrgDsn.Converter
{
    public class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var br = value as SolidColorBrush;
            return br.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
