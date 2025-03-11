﻿using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel
{
	public class Link : LinkViewModelBase<Node>
	{
		public Link(Node source, Node target)
			: base(source, target)
		{
		}
	}
}