using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using System.Xml.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;
using Telerik.Windows.Diagrams.Core;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel
{
    public class BaseOrgChartViewModel : ViewModelBase
    {
        private GraphSource graphSource;
        private TreeLayoutType currentTreeLayoutType;
        private ItemDisplayMode currentItemDisplayMode;
        private bool shouldLayoutAfterExpandCollapse;
        private TreeLayoutViewModel childTreeLayoutViewModel;
        private readonly OrgTreeRouter router = new OrgTreeRouter();
        private double zoomFactor;
        private static double SmallToNormalTemplateThreshHold = 0.75d;
        private static double NormallToLargeTemplateThreshHold = 1.4d;
        public Model.BPMNDBEntities bpmnEty;
        //protected IOrgChart root;
        public BaseOrgChartViewModel(Model.BPMNDBEntities context, IOrgChart root)
        {
            this.bpmnEty = context;
            //this.root = root;
            this.HierarchicalDataSource = new ObservableCollection<Node>();
            this.GraphSource = new GraphSource();
            this.ChildRouterViewModel = new OrgRouterViewModel(this.router);
            this.ChildTreeLayoutViewModel = new TreeLayoutViewModel();
            AddNodeCommand = new Microsoft.Practices.Prism.Commands.DelegateCommand<Node>(ExecuteAddNodeCommand);

            if (root == null)
            {
                return;
            }            

            this.PopulateWithData(root);
            this.PopulateGraphSources();
            
            this.CurrentTreeLayoutType = TreeLayoutType.TreeDown;
            this.CurrentItemDisplayMode = ItemDisplayMode.Standard;
            this.BindCommands();
            this.ShouldLayoutAfterExpandCollapse = false;
            this.ZoomFactor = 1.0d;
            
        }

        public void ReBuildGraph(IOrgChart root)
        {
            //this.root = root;
            this.HierarchicalDataSource.Clear();// = new ObservableCollection<Node>();
            this.GraphSource.Clear();// = new GraphSource();            
            this.PopulateWithData(root);
            this.GraphSource.PopulateGraphSource2(HierarchicalDataSource[0]);

        }



        public event EventHandler<VisibilityChangedEventArgs> NodeVisibilityChanged;
        public event EventHandler CurrentLayoutTypeChanged;
        public event EventHandler CurrentLayoutTypeSettingsChanged;
        public event EventHandler ChildrenExpandedOrCollapsed;


        public double ZoomFactor
        {
            get { return this.zoomFactor; }
            set
            {
                if (this.zoomFactor != value)
                {
                    this.zoomFactor = value;
                    this.OnZoomChanged();
                    this.OnPropertyChanged("ZoomFactor");
                }
            }
        }

        public ItemDisplayMode CurrentItemDisplayMode
        {
            get { return this.currentItemDisplayMode; }
            set
            {
                if (this.currentItemDisplayMode != value)
                {
                    this.currentItemDisplayMode = value;
                    this.OnPropertyChanged("CurrentItemDisplayMode");
                }
            }
        }

        public TreeLayoutType CurrentTreeLayoutType
        {
            get { return currentTreeLayoutType; }
            set
            {
                this.currentTreeLayoutType = value;
                this.ChildTreeLayoutViewModel.CurrentTreeLayoutType = value;
                this.ChildRouterViewModel.CurrentTreeLayoutType = value;
                this.OnPropertyChanged("CurrentTreeLayoutType");
                this.OnCurrentLayoutTypeChanged();
            }
        }

        public GraphSource GraphSource
        {
            get
            {
                return this.graphSource;
            }
            set
            {
                if (this.graphSource != value)
                {
                    this.graphSource = value;
                    this.OnPropertyChanged("GraphSource");
                }
            }
        }

        public ObservableCollection<Node> HierarchicalDataSource { get; set; }

        public OrgTreeRouter Router { get { return this.router; } }

        public DelegateCommand ToggleVisibilityCommand { get; set; }

        public TreeLayoutViewModel ChildTreeLayoutViewModel
        {
            get { return this.childTreeLayoutViewModel; }
            protected set
            {
                if (this.childTreeLayoutViewModel != value)
                {
                    this.childTreeLayoutViewModel = value;
                    this.OnPropertyChanged("ChildTreeLayoutViewModel");
                }
            }
        }

        public OrgRouterViewModel ChildRouterViewModel { get; protected set; }

        public bool ShouldLayoutAfterExpandCollapse
        {
            get { return this.shouldLayoutAfterExpandCollapse; }
            set
            {
                if (this.shouldLayoutAfterExpandCollapse != value)
                {
                    this.shouldLayoutAfterExpandCollapse = value;
                    this.OnPropertyChanged("ShouldLayoutAfterExpandCollapse");
                }
            }
        }

        public void PopulateGraphSources()
        {
            foreach (var item in this.HierarchicalDataSource)
            {
                this.GraphSource.PopulateGraphSource(item);
            }
        }

        protected bool CanExecuteToggleVisibilityCommand(object o)
        {
            return o != null && (o as Node).Children.Count > 0;
        }

        protected void ExecuteToggleVisibilityCommand(object o)
        {
            var node = o as Node;
            var areChildrenCollapsed = node != null && node.AreChildrenCollapsed;

            this.ToggleChildrenVisibility(node, areChildrenCollapsed);
            node.AreChildrenCollapsed = !node.AreChildrenCollapsed;
        }

        private void BindCommands()
        {
            this.ToggleVisibilityCommand = new DelegateCommand(this.ExecuteToggleVisibilityCommand, this.CanExecuteToggleVisibilityCommand);
        }

        //private void PopulateWithData()
        //{
        //    var stream = Application.GetResourceStream(new Uri("/Diagrams;component/OrgChart/XmlSource/Organization.xml", UriKind.RelativeOrAbsolute));
        //    XElement dataXml = XElement.Load(stream.Stream);

        //    foreach (XElement element in dataXml.Elements("Node"))
        //    {
        //        Node node = this.CreateNode(element, null);
        //        node.Children.AddRange(this.GetSubNodes(element, node));
        //        this.HierarchicalDataSource.Add(node);
        //    }
        //}

        protected void PopulateWithData(IOrgChart tbl)
        {
            Node node = this.CreateNode(tbl);
            node.Children.AddRange(this.GetSubNodes(tbl));
            this.HierarchicalDataSource.Add(node);
        }

        private ObservableCollection<HierarchicalNodeViewModel> GetSubNodes(IOrgChart tbl)
        {
            var nodes = new ObservableCollection<HierarchicalNodeViewModel>();
            foreach (IOrgChart item in tbl.SubNodes)
            {
                var node = this.CreateNode(item);
                node.Children.AddRange(this.GetSubNodes(item));
                nodes.Add(node);
            }
            return nodes;
        }

        //private ObservableCollection<HierarchicalNodeViewModel> GetSubNodes(XContainer element, Node parent)
        //{
        //    var nodes = new ObservableCollection<HierarchicalNodeViewModel>();
        //    foreach (var subElement in element.Elements("Node"))
        //    {
        //        Node node = this.CreateNode(subElement, parent);
        //        node.Children.AddRange(this.GetSubNodes(subElement, node));
        //        nodes.Add(node);
        //    }
        //    return nodes;
        //}

        //private Node CreateNode(XElement element, Node parentNode)
        //{
        //    Node node = new Node();
        //    node.PropertyChanged += this.OnNodePropertyChanged;
        //    node.FirstName = element.Attribute("FirstName").Value;
        //    node.LastName = element.Attribute("LastName").Value;
        //    node.Phone = element.Attribute("Phone").Value;
        //    node.Email = element.Attribute("Email").Value;
        //    node.Address = element.Attribute("Address").Value;
        //    node.Path = parentNode == null ? node.FullName : parentNode.Path + "|" + node.FullName;
        //    //string imagePath = "../Images/Diagrams/OrgChart/{0}{1}.jpg";
        //    //node.ImagePath = string.Format(imagePath, node.FirstName, node.LastName);
        //    node.JobPosition = element.Attribute("Position").Value;
        //    node.Branch = (Branch)Enum.Parse(typeof(Branch), element.Attribute("Branch").Value, true);
        //    return node;
        //}

        protected virtual Node CreateNode(IOrgChart tbl)
        {
            return null;
        }

        protected void OnNodePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visibility")
            {
                if (this.NodeVisibilityChanged != null)
                {
                    this.NodeVisibilityChanged(sender, new VisibilityChangedEventArgs((sender as Node).Visibility == Visibility.Visible));
                }
            }
        }

        private void ToggleChildrenVisibility(HierarchicalNodeViewModel node, bool areChildrenVisible)
        {
            foreach (Node subNode in node.Children)
            {
                var visibility = areChildrenVisible ? Visibility.Visible : Visibility.Collapsed;
                subNode.Visibility = visibility;
                subNode.IsSelected = false;
                this.GraphSource.InternalLinks.Where(link => link.Source == node).ToList()
                    .ForEach(link =>
                    {
                        link.Visibility = visibility;
                        link.IsSelected = false;
                    });

                if (subNode.AreChildrenCollapsed) continue;

                this.ToggleChildrenVisibility(subNode, areChildrenVisible);
            }
            this.OnChildrenExpandedOrCollapsed();
        }

        private void OnCurrentLayoutTypeChanged()
        {
            if (this.CurrentLayoutTypeChanged != null)
            {
                this.CurrentLayoutTypeChanged(this, EventArgs.Empty);
            }
        }

        private void OnChildrenExpandedOrCollapsed()
        {
            if (this.ChildrenExpandedOrCollapsed != null)
            {
                this.ChildrenExpandedOrCollapsed(this, EventArgs.Empty);
            }
        }

        private void OnZoomChanged()
        {
            ItemDisplayMode newMode;
            if (SmallToNormalTemplateThreshHold < this.ZoomFactor && this.ZoomFactor <= NormallToLargeTemplateThreshHold)
                newMode = ItemDisplayMode.Standard;
            else if (this.ZoomFactor <= SmallToNormalTemplateThreshHold)
                newMode = ItemDisplayMode.Small;
            else
                newMode = ItemDisplayMode.Detailed;

            if (this.CurrentItemDisplayMode != newMode)
                this.ChangeAllShapesDisplayMode(newMode);
        }

        private void ChangeAllShapesDisplayMode(ItemDisplayMode newMode)
        {
            //this.CurrentItemDisplayMode = newMode;
            //foreach (var node in this.GraphSource.InternalItems)
            //{
            //    node.CurrentDisplayMode = newMode;
            //}
        }

        public System.Windows.Input.ICommand AddNodeCommand { get; set; }

        protected virtual void ExecuteAddNodeCommand(Node obj)
        {

        }

        private Node selectedNode;
        public Node SelectedNode
        {
            get { return selectedNode; }
            set
            {
                selectedNode = value;
                this.OnPropertyChanged("SelectedNode");
            }
        }


    }

    public class VisibilityChangedEventArgs : EventArgs
    {
        public VisibilityChangedEventArgs(bool isVisible)
        {
            this.IsVisible = isVisible;
        }
        public bool IsVisible { get; private set; }
    }
}
