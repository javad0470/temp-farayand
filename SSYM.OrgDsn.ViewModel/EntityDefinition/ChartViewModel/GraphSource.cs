using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel
{
	public class GraphSource :  ObservableGraphSourceBase<Node, Link>
	{
		public void PopulateGraphSource(Node node)
		{
			this.AddNode(node);
			foreach (Node subNode in node.Children)
			{
				Link link = new Link(node, subNode);
				this.AddLink(link);
				this.PopulateGraphSource(subNode);
			}
		}

        public void PopulateGraphSource2(Node node)
        {
            this.AddNode(node);
            foreach (Node subNode in node.Children)
            {
                this.PopulateGraphSource2(subNode);
                Link link = new Link(node, subNode);
                this.AddLink(link);
            }
        }
	}
}
