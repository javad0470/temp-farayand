using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SSYM.OrgDsn.Converter
{
    public class ScaleYArrowHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int rowIndex = (int)values[0];
                int totalCount = (int)values[1];

                rowIndex++;

                if (totalCount % 2 == 0)
                {
                    if (rowIndex <= totalCount / 2)//up
                    {
                        return 1D;
                    }
                    else//down
                    {
                        return -1D;
                    }
                }
                else
                {
                    if (rowIndex == totalCount / 2 + 1) //center
                    {
                        return 1D;
                    }
                    else if (rowIndex <= totalCount / 2)//up
                    {
                        return 1D;
                    }
                    else if (rowIndex > totalCount / 2 + 1)//down
                    {
                        return -1D;
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