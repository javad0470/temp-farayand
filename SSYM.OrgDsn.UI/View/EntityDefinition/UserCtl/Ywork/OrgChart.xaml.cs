using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using yWorks.Canvas.Geometry.Structs;
using yWorks.Canvas.Input;
using yWorks.Canvas.Model;
using yWorks.Support;
using yWorks.yFiles.Algorithms;
using yWorks.yFiles.Algorithms.Geometry;
using yWorks.yFiles.Layout;
using yWorks.yFiles.Layout.Tree;
using yWorks.yFiles.UI;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Model;
using yWorks.Support.Extensions;
using Application = System.Windows.Application;
using LineSegment = yWorks.yFiles.Algorithms.Geometry.LineSegment;
using yWorks.yFiles.UI.DataBinding;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using Telerik.Windows.DragDrop;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl.Ywork
{
    /// <summary>
    /// Interaction logic for OrgChart.xaml
    /// </summary>
    public partial class OrgChart : UserControl
    {
        public OrgChart()
        {
            InitializeComponent();

            hiddenNodesSet = new yWorks.Support.HashSet<INode>();

            DragDropManager.AddDragInitializeHandler(treeView, OnDragInitialize);

            DragDropManager.AddGiveFeedbackHandler(treeView, OnGiveFeedback);

            this.Loaded += OrgChart_Loaded;
        }

        void OrgChart_Loaded(object sender, RoutedEventArgs e)
        {
            if (treeView.Items.Count > 0)
            {
                treeView.SelectedItem = treeView.Items[0];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.Copy;

            Image img = new Image() { Source = ((BackgroundedImage)((Grid)args.OriginalSource).FindName("imgOrg")).Source };

            img.Height = 30;

            img.Width = 30;

            Border bd = new Border();

            bd.ApplyTemplate();

            bd.Child = img;

            bd.Background = Brushes.Black;

            args.DragVisual = new ContentControl() { Content = bd };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;

        }

        /// <summary>
        /// 
        /// </summary>
        public void RebindChart()
        {
            tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).Org;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblOrg>).Count() > 0)
            {
                RefreshLayout(-1, graphControl.Graph.Nodes.First());
            }
            if (graphControl.Graph.Nodes.Count > 0)
            {
                graphControl.CurrentItem = graphControl.Graph.Nodes.ElementAt(0);
            }
        }

        int FldCodePost = 0;
        bool ShouldIZoom = false;
        /// <summary>
        /// 
        /// </summary>
        public void RebindChartAterAddOrg()
        {
            using (var ctx = new BPMNDBEntities())
            {
                if (Util.LcsSfw == null)
                {
                    MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                    Application.Current.Shutdown();
                }
                if (Util.LcsSfw.TnoOrgSub != -1 &&
                    ctx.TblOrgs.LongCount(a => a.FldCodUpl != null && a.FldCodOrg != 1) >= Util.LcsSfw.TnoOrgSub)
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 81);
                    MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                        msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                        (r) => { },
                        null);
                    return;
                }
            }
            var addedOrg = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).ExecuteAddNewOrgCommand();

            if (addedOrg == null)
            {
                return;
            }
            //addedOrg.SearchParent.IsExpanded = true;
            //addedOrg.IsSelected = true;

            tree.NodesSource = null;

            tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).Org;

            BringIntoView1();

            var selectedChartNode = graphControl.Graph.Nodes.SingleOrDefault(n => n.Tag == addedOrg);

            var selectedParentChartNode = graphControl.Graph.Nodes.SingleOrDefault(n => n.Tag == addedOrg.TblOrg2);

            graphControl.CurrentItem = selectedChartNode;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblOrg>).Count() > 0)
            {
                RefreshLayout(-1, selectedParentChartNode);
            }

            int str = (addedOrg as TblOrg).FldCodOrg;
            INode selectedNode = graphControl.Graph.Nodes.SingleOrDefault(m => (m.Tag as TblOrg).FldCodOrg == str);
            INode nod = UIUtil.findNodByTag(graphControl, addedOrg, out selectedNode);
            graphControl.CurrentItem = selectedNode;
            FldCodePost = str;
            ShouldIZoom = true;

        }
        public void BringIntoView1()
        {

            var parent = (treeView.SelectedItem as TblOrg).SearchParent;
            if (parent == null)
            {
                return;
            }

            var obj = treeView.ContainerFromItemRecursive(parent);

            if (obj == null)
            {
                return;
            }

            int repeat = obj.Items.Count;

            double d;
            if (!treeView.ContainerFromItemRecursive((treeView.SelectedItem as TblOrg).SearchParent).IsExpanded)
            {
                d = treeView.ScrollViewer.ContentVerticalOffset + treeView.ContainerFromItemRecursive((treeView.SelectedItem as TblOrg).SearchParent).ActualHeight * (repeat);
                ((treeView.SelectedItem as TblOrg).SearchParent).IsExpanded = true;
            }
            else
            {
                d = treeView.ScrollViewer.ContentVerticalOffset + treeView.ContainerFromItemRecursive((treeView.SelectedItem as TblOrg).SearchParent).ActualHeight;
            }
            treeView.ScrollViewer.ScrollToVerticalOffset(d);
        }
        /// <summary>
        /// 
        /// </summary>
        public void RebindChartAfterDeleteOrg()
        {
            var vm = this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel;
            TblOrg org = vm.SelectedOrg;

            TblOrg parent = org.TblOrg2;

            if (org != null)
            {
                int codMsg = 2;

                if (org.TblOrg1.Count > 0)
                {
                    codMsg = 31;
                }

                MessageBoxResult res = Util.ShowMessageBox(codMsg, org.FldNamOrg);

                if (res == MessageBoxResult.Yes | res == MessageBoxResult.OK)
                {
                    INode parentNod;
                    INode nod = UIUtil.findNodByTag(graphControl, org, out parentNod);

                    if (!PublicMethods.DeleteOrg_2261((this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).bpmnEty, org))
                    {
                        Util.ShowMessageBox(32, org.FldNamOrg);
                    }
                    else
                    {

                        graphControl.CurrentItem = graphControl.Graph.Predecessors(nod).FirstOrDefault();

                        var childs = UIUtil.findAllChilds(graphControl.Graph, nod);
                        for (int i = 0; i < childs.Count; i++)
                        {
                            graphControl.Graph.Remove(childs[i]);
                        }
                        graphControl.CurrentItem = parentNod;

                        if (parent != null)
                        {
                            parent.ChildsCV.Remove(org);
                            parent.ChildsCV.Refresh();
                        }

                        //tree.NodesSource = null;

                        //tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).Org;

                        //if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblOrg>).Count() > 0)
                        //{
                        //    if (parentNod != null)
                        //    {
                        //        RefreshLayout(-1, parentNod);
                        //    }
                        //    else
                        //    {
                        //        RefreshLayout(-1, graphControl.Graph.Nodes.First());
                        //    }

                        //    graphControl.CurrentItem = parentNod;
                        //}
                    }

                }
            }
        }

        /// <summary>
        /// The command that can be used by the buttons to show the parent node.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="System.Windows.Input.ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand ShowParentCommand = new RoutedUICommand("Show Parent", "ShowParent",
                                                                                typeof(OrgChart));

        /// <summary>
        /// The command that can be used by the buttons to hide the parent node.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand HideParentCommand = new RoutedUICommand("Hide Parent", "HideParent",
                                                                                typeof(OrgChart));

        /// <summary>
        /// The command that can be used by the buttons to show the child nodes.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand ShowChildrenCommand = new RoutedUICommand("Show Children", "ShowChildren",
                                                                                typeof(OrgChart));

        /// <summary>
        /// The command that can be used by the buttons to hide the child nodes.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand HideChildrenCommand = new RoutedUICommand("Hide Children", "HideChildren",
                                                                                typeof(OrgChart));

        /// <summary>
        /// The command that can be used by the buttons to expand all collapsed nodes.
        /// </summary>
        public static readonly RoutedUICommand ShowAllCommand = new RoutedUICommand("Show All", "ShowAll",
                                                                                typeof(OrgChart));

        /// <summary>
        /// Used by the predicate function to determine which nodes should not be shown.
        /// </summary>
        private readonly yWorks.Support.HashSet<INode> hiddenNodesSet;

        /// <summary>
        /// The filtered graph instance that hides nodes from the to create smaller graphs for easier navigation.
        /// </summary>
        private FilteredGraphWrapper filteredGraphWrapper;

        private void OnLoaded(object src, EventArgs eventArgs)
        {

        }

        /// <summary>
        /// Called when an item has been double clicked.
        /// </summary>
        /// <param name="o">The source of the event.</param>
        /// <param name="itemInputEventArgs">The event argument instance containing the event data.</param>
        private void OnItemDoubleClicked(object o, ItemInputEventArgs<IModelItem> itemInputEventArgs)
        {
            graphControl.CurrentItem = itemInputEventArgs.Item;
            ZoomToCurrentItem();
        }

        private void InitializeGraph()
        {
            // create new nodestyle that delegates to other 
            // styles for different zoom ranges
            //var nodeStyle = new NodeControlNodeStyle("EmployeeNodeControlStyle");

            graphControl.CurrentItemChanged += graphControl_CurrentItemChanged;

            //graphControl.Graph.NodeDefaults.Style = nodeStyle;
            //graphControl.Graph.NodeDefaults.Size = new SizeD(250, 100);

            //graphControl.Graph.EdgeDefaults.Style = new PolylineEdgeStyle { Smoothing = 10 };
        }

        /// <summary>
        /// The predicate used for the FilterGraphWrapper
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool ShouldShowNode(INode obj)
        {
            return !hiddenNodesSet.Contains(obj);
        }

        /// <summary>
        /// Gets the GraphControl instance used in the form.
        /// </summary>
        public GraphControl GraphControl
        {
            get { return graphControl; }
        }

        #region Tree Layout Configuration and initial execution

        /// <summary>
        /// Does a tree layout of the Graph provided by the <see cref="TreeBuilder"/>.
        /// The layout and assistant attributes from the business data of the employees are used to
        /// guide the the layout.
        /// </summary>
        public void DoLayout()
        {
            IGraph tree = graphControl.Graph;

            ConfigureLayout(tree);
            new BendDuplicatorStage(new GenericTreeLayouter()).DoLayout(tree);
            CleanUp(tree);
        }

        private void ConfigureLayout(IGraph tree)
        {
            IMapperRegistry registry = tree.MapperRegistry;

            var nodePlacerMapper = registry.AddDictionaryMapper<INode, INodePlacer>(GenericTreeLayouter.NodePlacerDpKey);
            var assistantMapper = registry.AddDictionaryMapper<INode, bool>(AssistantPlacer.AssistantDpKey);

            foreach (var node in tree.Nodes)
            {
                if (tree.InDegree(node) == 0)
                {
                    SetNodePlacers(node, nodePlacerMapper, assistantMapper, tree);
                }
            }
        }

        private void SetNodePlacers(INode rootNode,
                                    IMapper<INode, INodePlacer> nodePlacerMapper, IMapper<INode, bool> assistantMapper,
                                    IGraph tree)
        {
            var employee = rootNode.Tag as XmlElement;
            if (employee != null)
            {
                var layout = employee.GetAttribute("layout");
                switch (layout)
                {
                    case "rightHanging":
                        nodePlacerMapper[rootNode] = new DefaultNodePlacer(ChildPlacement.VerticalToRight,
                                                                           RootAlignment.LeadingOnBus, 30, 30) { RoutingStyle = RoutingStyle.ForkAtRoot };
                        break;
                    case "leftHanging":
                        nodePlacerMapper[rootNode] = new DefaultNodePlacer(ChildPlacement.VerticalToLeft,
                                                                           RootAlignment.LeadingOnBus, 30, 30) { RoutingStyle = RoutingStyle.ForkAtRoot };
                        break;
                    case "bothHanging":
                        nodePlacerMapper[rootNode] = new LeftRightPlacer { PlaceLastOnBottom = false };
                        break;
                    default:
                        nodePlacerMapper[rootNode] = new DefaultNodePlacer(ChildPlacement.HorizontalDownward,
                                                                           RootAlignment.Median, 30, 30);
                        break;
                }

                var assistant = employee.Attributes["assistant"];
                if (assistant != null && assistant.Value == "true" && tree.InDegree(rootNode) > 0)
                {
                    IEdge inEdge = tree.InEdgesAt(rootNode)[0];
                    INode parent = inEdge.GetSourceNode();
                    INodePlacer oldParentPlacer = nodePlacerMapper[parent];
                    AssistantPlacer assistantPlacer = new AssistantPlacer();
                    assistantPlacer.ChildNodePlacer = oldParentPlacer;
                    nodePlacerMapper[parent] = assistantPlacer;
                    assistantMapper[rootNode] = true;
                }
            }

            foreach (IEdge outEdge in tree.OutEdgesAt(rootNode))
            {
                INode child = (INode)outEdge.TargetPort.Owner;
                SetNodePlacers(child, nodePlacerMapper, assistantMapper, tree);
            }
        }

        private void CleanUp(IGraph graph)
        {
            IMapperRegistry registry = graph.MapperRegistry;
            registry.RemoveMapper(AssistantPlacer.AssistantDpKey);
            registry.RemoveMapper(GenericTreeLayouter.NodePlacerDpKey);
        }

        #endregion

        #region Command Binding Helper methods

        /// <summary>
        /// Helper method that determines whether the <see cref="ShowParentCommand"/> can be executed.
        /// </summary>
        public void CanExecuteShowChildren(object sender, CanExecuteRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout && filteredGraphWrapper != null)
            {
                e.CanExecute = filteredGraphWrapper.OutDegree(node) != filteredGraphWrapper.FullGraph.OutDegree(node);
            }
            else
            {
                e.CanExecute = false;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handler for the <see cref="ShowChildrenCommand"/>
        /// </summary>
        public void ShowChildrenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout)
            {
                int count = hiddenNodesSet.Count;
                foreach (var childEdge in filteredGraphWrapper.FullGraph.OutEdgesAt(node))
                {
                    var child = childEdge.GetTargetNode();
                    if (hiddenNodesSet.Remove(child))
                    {
                        filteredGraphWrapper.FullGraph.SetCenter(child, node.Layout.GetCenter());
                        filteredGraphWrapper.FullGraph.ClearBends(childEdge);
                    }
                }
                RefreshLayout(count, node);
            }
        }

        /// <summary>
        /// Helper method that determines whether the <see cref="ShowParentCommand"/> can be executed.
        /// </summary>
        private void CanExecuteShowParent(object sender, CanExecuteRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout && filteredGraphWrapper != null)
            {
                e.CanExecute = filteredGraphWrapper.InDegree(node) == 0 && filteredGraphWrapper.FullGraph.InDegree(node) > 0;
            }
            else
            {
                e.CanExecute = false;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handler for the <see cref="ShowParentCommand"/>
        /// </summary>
        private void ShowParentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout)
            {
                int count = hiddenNodesSet.Count;
                foreach (var parentEdge in filteredGraphWrapper.FullGraph.InEdgesAt(node))
                {
                    var parent = parentEdge.GetSourceNode();
                    if (hiddenNodesSet.Remove(parent))
                    {
                        filteredGraphWrapper.FullGraph.SetCenter(parent, node.Layout.GetCenter());
                        filteredGraphWrapper.FullGraph.ClearBends(parentEdge);
                    }
                }
                RefreshLayout(count, node);
            }
        }

        /// <summary>
        /// Helper method that determines whether the <see cref="HideParentCommand"/> can be executed.
        /// </summary>
        private void CanExecuteHideParent(object sender, CanExecuteRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout && filteredGraphWrapper != null)
            {
                e.CanExecute = filteredGraphWrapper.InDegree(node) > 0;
            }
            else
            {
                e.CanExecute = false;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handler for the <see cref="HideParentCommand"/>
        /// </summary>
        private void HideParentExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout)
            {
                int count = hiddenNodesSet.Count;

                foreach (var testNode in filteredGraphWrapper.FullGraph.Nodes)
                {
                    if (testNode != node && filteredGraphWrapper.Contains(testNode) &&
                        filteredGraphWrapper.InDegree(testNode) == 0)
                    {
                        // this is a root node - remove it and all children unless 
                        HideAllExcept(testNode, node);
                    }
                }
                RefreshLayout(count, node);
            }
        }

        /// <summary>
        /// Helper method that determines whether the <see cref="HideChildrenCommand"/> can be executed.
        /// </summary>
        private void CanExecuteHideChildren(object sender, CanExecuteRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout && filteredGraphWrapper != null)
            {
                e.CanExecute = filteredGraphWrapper.OutDegree(node) > 0;
            }
            else
            {
                e.CanExecute = false;
            }
            e.Handled = true;
        }

        /// <summary>
        /// Handler for the <see cref="HideChildrenCommand"/>
        /// </summary>
        private void HideChildrenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var node = (e.Parameter ?? graphControl.CurrentItem) as INode;
            if (node != null && !doingLayout)
            {
                int count = hiddenNodesSet.Count;
                foreach (var child in filteredGraphWrapper.Successors(node))
                {
                    HideAllExcept(child, node);
                }
                RefreshLayout(count, node);
            }
        }

        /// <summary>
        /// Helper method that determines whether the <see cref="ShowParentCommand"/> can be executed.
        /// </summary>
        private void CanExecuteShowAll(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = filteredGraphWrapper != null && hiddenNodesSet.Count != 0 && !doingLayout;
            e.Handled = true;
        }

        /// <summary>
        /// Handler for the <see cref="ShowAllCommand"/>
        /// </summary>
        private void ShowAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!doingLayout)
            {
                hiddenNodesSet.Clear();
                RefreshLayout(-1, graphControl.CurrentItem as INode);
            }
        }

        #endregion

        /// <summary>
        /// Help method that hides all nodes and its descendants except for a given node
        /// </summary>
        private void HideAllExcept(INode nodeToHide, INode exceptNode)
        {
            hiddenNodesSet.Add(nodeToHide);
            foreach (var child in filteredGraphWrapper.FullGraph.Successors(nodeToHide))
            {
                if (exceptNode != child)
                {
                    HideAllExcept(child, exceptNode);
                }
            }
        }

        // indicates whether a layout is calculated at the moment
        private bool doingLayout;

        /// <summary>
        /// Helper method that refreshes the layout after children or parent nodes have been added
        /// or removed.
        /// </summary>
        private void RefreshLayout(int count, INode centerNode)
        {
            if (doingLayout)
            {
                return;
            }
            doingLayout = true;
            if (count != hiddenNodesSet.Count)
            {
                // tell our filter to refresh the graph
                try
                {
                    filteredGraphWrapper.NodePredicateChanged();
                }
                catch (Exception)
                {


                }

                // the commands CanExecute state might have changed - suggest a requery.
                CommandManager.InvalidateRequerySuggested();

                // now layout the graph in animated fashion
                IGraph tree = graphControl.Graph;

                // we mark a node as the center node
                graphControl.Graph.MapperRegistry.AddMapper<INode, bool>("CenterNode", node => node == centerNode);

                // configure the tree layout
                ConfigureLayout(tree);

                // create the layouter (with a stage that fixes the center node in the coordinate system
                var layouter = new BendDuplicatorStage(new FixNodeLocationStage(new GenericTreeLayouter()));

                // configure a LayoutExecutor
                var executor = new LayoutExecutor(graphControl, layouter)
                {
                    AnimateViewport = centerNode == null,
                    EasedAnimation = true,
                    RunInThread = true,
                    UpdateContentRect = true,
                    Duration = TimeSpan.FromMilliseconds(500)
                };
                executor.FinishHandler += new EventHandler(GraphControlFinishAnimation);
                executor.Start();
                // add hook for cleanup
                executor.FinishHandler += delegate
                {
                    graphControl.Graph.MapperRegistry.RemoveMapper("CenterNode");
                    CleanUp(tree);
                    doingLayout = false;
                };
            }
        }
        void GraphControlFinishAnimation(object sender, EventArgs e)
        {
            INode selectedNode = (sender as GraphControl).Graph.Nodes.SingleOrDefault(m => (m.Tag as TblOrg).FldCodOrg == FldCodePost);
            graphControl.CurrentItem = selectedNode;
            ShouldIZoom = false;
            if (selectedNode != null)
            {
                graphControl.ZoomTo(selectedNode.Layout.GetTopLeft(), 0.95);
            }
        }

        private void ZoomToCurrentItem()
        {
            var currentItem = GraphControl.CurrentItem as INode;
            // visible current item
            if (GraphControl.Graph.Contains(currentItem))
            {
                GraphControl.ZoomToCurrentItemCommand.Execute(null, GraphControl);
            }
            else
            {
                // see if it can be made visible
                if (filteredGraphWrapper.FullGraph.Nodes.Contains(currentItem))
                {
                    // uhide all nodes...
                    hiddenNodesSet.Clear();
                    // except the node to be displayed and all its descendants
                    foreach (var testNode in filteredGraphWrapper.FullGraph.Nodes)
                    {
                        if (testNode != currentItem && filteredGraphWrapper.FullGraph.InDegree(testNode) == 0)
                        {
                            HideAllExcept(testNode, currentItem);
                        }
                    }
                    // reset the layout to make the animation nicer
                    foreach (var n in filteredGraphWrapper.Nodes)
                    {
                        filteredGraphWrapper.SetCenter(n, PointD.Origin);
                    }
                    foreach (var edge in filteredGraphWrapper.Edges)
                    {
                        filteredGraphWrapper.ClearBends(edge);
                    }
                    RefreshLayout(-1, null);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region TreeView related

        private void TreeMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            XmlLinkedNode clickedItem = ((TreeView)e.Source).SelectedItem as XmlLinkedNode;
            INode selectedNode = filteredGraphWrapper.FullGraph.Nodes.FirstOrDefault(node => node.Tag == clickedItem);
            graphControl.CurrentItem = selectedNode;
            ZoomToCurrentItem();
        }

        private void TreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ZoomToTreeItem();
        }

        private void ZoomToTreeItem()
        {

            //XmlLinkedNode clickedItem = treeView.SelectedItem as XmlLinkedNode;
            // get the correspondent node in the graph
            if (treeView.SelectedItem != null)
            {
                int str = (treeView.SelectedItem as TblOrg).FldCodOrg;
                INode selectedNode = graphControl.Graph.Nodes.SingleOrDefault(m => (m.Tag as TblOrg).FldCodOrg == str); // filteredGraphWrapper.FullGraph.Nodes.FirstOrDefault(node => node.Tag == clickedItem);
                if (selectedNode != null && graphControl.Graph.Contains(selectedNode))
                {
                    // select the node in the GraphControl
                    GraphControl.CurrentItem = selectedNode;
                    graphControl.EnsureVisible(selectedNode.Layout.ToRectD());
                }
            }

        }

        void graphControl_CurrentItemChanged(object sender, RoutedPropertyChangedEventArgs<IModelItem> e)
        {
            if (e.OldValue != null && e.NewValue == null)
                graphControl.CurrentItem = e.OldValue;
            if (graphControl.CurrentItem != null)
            {
                var selected = graphControl.CurrentItem.Tag as TblOrg;
                int count = 0;
                if (treeView.SelectedItem != selected)
                {
                    treeView.CollapseAll();
                    var parent = selected.SearchParent;
                    var child = selected;
                    /*if (selected.SearchParent != null)
                    {
                        selected.SearchParent.IsExpanded = true;
                    }*/
                    while (parent != null)
                    {
                        IList<Model.Base.ISearchableTree> temp = parent.SearchChilds;
                        int i = 0;
                        count++;
                        while ((temp[i] as TblOrg) != child)
                        {
                            i++; count++;
                        }
                        parent.IsExpanded = true;
                        child = parent as TblOrg;
                        parent = parent.SearchParent;
                    }
                    treeView.SelectedItem = selected;
                    treeView.ScrollViewer.ScrollToTop();
                    treeView.ScrollViewer.ScrollToVerticalOffset(count * 36);
                }
            }
        }

        private void SelectCurrentItemInTreeRec(ItemsControl parent, object item)
        {
            var currentItem = graphControl.CurrentItem;
            if (currentItem == null)
            {
                return;
            }
            XmlLinkedNode employee = currentItem.Tag as XmlLinkedNode;

            if (item == null || parent == null || employee == null)
            {
                return;
            }

            var treeViewItem = parent.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
            if (treeViewItem != null)
            {
                if (item == employee)
                {
                    treeViewItem.IsSelected = true;
                    return;
                }
                foreach (var child in treeViewItem.Items)
                {
                    SelectCurrentItemInTreeRec(treeViewItem, child);
                }
            }
        }

        private void TreeSource_GraphRebuilt(object sender, EventArgs e)
        {
        }

        private void TreeViewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                ZoomToCurrentItem();
            }
        }

        #endregion

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            InitializeGraph();

            // register command bindings
            graphControl.CommandBindings.Add(new CommandBinding(HideChildrenCommand, HideChildrenExecuted, CanExecuteHideChildren));
            graphControl.CommandBindings.Add(new CommandBinding(ShowChildrenCommand, ShowChildrenExecuted, CanExecuteShowChildren));
            graphControl.CommandBindings.Add(new CommandBinding(HideParentCommand, HideParentExecuted, CanExecuteHideParent));
            graphControl.CommandBindings.Add(new CommandBinding(ShowParentCommand, ShowParentExecuted, CanExecuteShowParent));
            graphControl.CommandBindings.Add(new CommandBinding(ShowAllCommand, ShowAllExecuted, CanExecuteShowAll));

            // disable selection, focus and highlight painting
            GraphControl.SelectionPaintManager.Enabled = false;
            GraphControl.FocusPaintManager.Enabled = false;
            GraphControl.HighlightPaintManager.Enabled = false;

            // we wrap the graph instance by a filtered graph wrapper
            filteredGraphWrapper = new FilteredGraphWrapper(GraphControl.Graph, ShouldShowNode, edge => true);
            GraphControl.Graph = filteredGraphWrapper;

            // now calculate the initial layout
            //DoLayout();
            GraphControl.FitGraphBounds();

            RebindChart();
        }

        private void treeView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ZoomToTreeItem();
        }

        private void UsrCtl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                this.treeView.SelectedItem = (this.DataContext as SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).SelectedOrg;
                (this.DataContext as SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork.OrgChartViewModel).SelectedOrg = this.treeView.SelectedItem as TblOrg;
            }
        }
    }

    /// <summary>
    /// An <see cref="AbstractLayoutStage"/> that uses a <see cref="IDataProvider"/>/<see cref="IMapper{K,V}"/>
    /// to determine a node whose location should not be changed during the layout.
    /// </summary>
    internal sealed class FixNodeLocationStage : AbstractLayoutStage
    {
        public FixNodeLocationStage(ILayouter layouter) : base(layouter) { }

        public override bool CanLayout(LayoutGraph graph)
        {
            return graph != null;
        }

        public override void DoLayout(LayoutGraph graph)
        {
            // determine the single node to keep at the center.
            var provider = graph.GetDataProvider("CenterNode");
            Node centerNode = null;
            if (provider != null)
            {
                centerNode = graph.Nodes.FirstOrDefault(provider.GetBool);
            }
            if (CoreLayouter != null)
            {
                if (centerNode != null)
                {
                    // remember old center
                    var center = graph.GetCenter(centerNode);
                    // run layout
                    CoreLayouter.DoLayout(graph);
                    // obtain new center
                    var newCenter = graph.GetCenter(centerNode);
                    // and adjust the layout
                    LayoutTool.MoveSubgraph(graph, graph.GetNodeCursor(), center.X - newCenter.X, center.Y - newCenter.Y);
                }
                else
                {
                    CoreLayouter.DoLayout(graph);
                }
            }
        }
    }

    /// <summary>
    /// LayoutStage that duplicates bends that share a common bus.
    /// </summary>
    class BendDuplicatorStage : AbstractLayoutStage
    {

        public BendDuplicatorStage()
            : base()
        {
        }

        public BendDuplicatorStage(ILayouter coreLayouter)
            : base(coreLayouter)
        {
        }

        public override bool CanLayout(LayoutGraph graph)
        {
            return true;
        }

        public override void DoLayout(LayoutGraph graph)
        {

            DoLayoutCore(graph);

            foreach (Node n in graph.Nodes)
            {
                foreach (Edge e in n.OutEdges)
                {
                    bool lastSegmentOverlap = false;
                    IEdgeLayout er = graph.GetEdgeLayout(e);
                    if (er.PointCount() > 0)
                    {
                        // last bend point
                        YPoint bendPoint = er.GetPoint(er.PointCount() - 1);

                        IEnumerator<Edge> ecc = n.OutEdges.GetEnumerator();
                    loop: while (ecc.MoveNext())
                        {
                            Edge eccEdge = ecc.Current;
                            if (eccEdge != e)
                            {
                                YPointPath path = graph.GetPath(eccEdge);
                                for (ILineSegmentCursor lc = path.LineSegments(); lc.Ok; lc.Next())
                                {
                                    LineSegment seg = lc.LineSegment;
                                    if (seg.Contains(bendPoint))
                                    {
                                        lastSegmentOverlap = true;
                                        goto loop;
                                    }
                                }
                            }
                        }
                    }


                    YList points = graph.GetPointList(e);
                    for (ListCell c = points.FirstCell; c != null; c = c.Succ())
                    {
                        YPoint p = (YPoint)c.Info;
                        if (c.Succ() == null && !lastSegmentOverlap)
                        {
                            break;
                        }

                        YPoint p0 = (YPoint)(c.Pred() == null ? graph.GetSourcePointAbs(e) : c.Pred().Info);
                        YPoint p2;
                        if (Math.Abs(p0.X - p.X) < 0.01)
                        {
                            p2 = new YPoint(p.X, p.Y - 0.001);
                        }
                        else
                        {
                            p2 = new YPoint(p.X - 0.001, p.Y);
                        }

                        points.InsertBefore(p2, c);
                    }
                    graph.SetPoints(e, points);
                }
            }
        }
    }

    public class NameToShortNameConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String name = value.ToString();
            String[] names = name.Split(' ');
            String shortName;
            if (names.Length > 1)
            {
                shortName = names[0].Substring(0, 1) + ". " + names[names.Length - 1];
            }
            else
            {
                shortName = names[0];
            }
            return shortName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    public class StatusToSolidColorBrushConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color c = Colors.White;
            if (value != null)
            {
                string s = (string)value;
                if (s.Equals("present"))
                {
                    c = Colors.Green;
                }
                else if (s.Equals("unavailable"))
                {
                    c = Colors.Red;
                }
                else if (s.Equals("travel"))
                {
                    c = Colors.Purple;
                }
            }
            return new SolidColorBrush(c);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }

    public class NodeToEmployeeConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            INode node = value as INode;
            if (node != null)
            {
                return node.Tag;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }


}
