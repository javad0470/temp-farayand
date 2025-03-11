using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class TranslateYArrowConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = 40;
            try
            {
                int rowIndex = (int)values[0];
                int totalCount = (int)values[1];

                rowIndex++;

                if (totalCount % 2 == 0)
                {
                    if (rowIndex <= totalCount / 2)//up
                    {
                        d = ((totalCount / 2) - rowIndex) * 80 + 40;
                        double d2 = (d / 2);
                        return -d2;

                    }
                    else//down
                    {
                        d = (rowIndex - 1 - (totalCount / 2)) * 80 + 40;
                        return (d / 2);
                    }
                }
                else
                {
                    if (rowIndex == totalCount / 2 + 1) //center
                    {
                        return 0D;
                    }
                    else if (rowIndex <= totalCount / 2)//up
                    {
                        d = Math.Abs(((totalCount / 2) - rowIndex + 1) * 80);
                        return -(d / 2);
                    }
                    else if (rowIndex > totalCount / 2 + 1)//down
                    {
                        d = Math.Abs((totalCount / 2 + 1 - rowIndex) * 80);
                        return (d / 2);
                    }

                }
                return values;

            }
            catch (Exception)
            {
                return 0D;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return value as object[];
        }
    }
}