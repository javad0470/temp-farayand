using System;
using System.Windows.Data;
using System.Windows.Media;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel.Converter
{
	public class ShapeBackgroundSelector : IValueConverter
	{
        public Brush OrganizationBrush
		{
			get;
			set;
		}

        public Brush OrganizationalPositionBrush
		{
			get;
			set;
		}

        public Brush OrganizationalPostBrush
		{
			get;
			set;
		}

        public Brush SubOrganizationBrush
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Branch branch = (Branch)value;
			switch (branch)
			{
				case Branch.Organization:
                    return this.OrganizationBrush;
				case Branch.OrganizationalPosition:
                    return this.OrganizationalPositionBrush;
				case Branch.OrganizationalPost:
                    return this.OrganizationalPostBrush;
				case Branch.SubOrganization:
                    return this.SubOrganizationBrush;
				default:
					return null;
			}	
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
