﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel.Converter
{
	public class ItemDisplayModeToFluidStateConverter: IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var contentState = (ItemDisplayMode)value;
			switch (contentState)
			{
				case ItemDisplayMode.Small:
					return FluidContentControlState.Small;
				case ItemDisplayMode.Standard:
					return FluidContentControlState.Normal;
				case ItemDisplayMode.Detailed:
					return FluidContentControlState.Large;
				default:
					return FluidContentControlState.Normal;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
