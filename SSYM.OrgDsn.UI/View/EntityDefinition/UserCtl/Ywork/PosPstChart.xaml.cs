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
using System.Threading.Tasks;
using System.Threading;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl.Ywork
{
    /// <summary>
    /// Interaction logic for OrgChart.xaml
    /// </summary>
    public partial class PosPstChart : UserControl
    {
        public PosPstChart()
        {
            InitializeComponent();

            hiddenNodesSet = new yWorks.Support.HashSet<INode>();

            

            DragDropManager.AddDragInitializeHandler(treeView, OnDragInitialize);

            DragDropManager.AddGiveFeedbackHandler(treeView, OnGiveFeedback);


        }

        void PosPstChart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="PosPstCV")
            {
                RebindChart();
                ddb.IsOpen = false;
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

            BackgroundedImage img = new BackgroundedImage() { Source = ((BackgroundedImage)((Grid)args.OriginalSource).FindName("imgPosPst")).Source };

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
            tree.NodesSource = null;

            tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel).PosPst;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblPosPstOrg>).Count() > 0)
            {
                RefreshLayout(-1, graphControl.Graph.Nodes.First());
            }
            if (graphControl.Graph.Nodes.Count>0)
            {
                graphControl.CurrentItem = graphControl.Graph.Nodes.ElementAt(0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RebindChartAfterAddFirstPosPst()
        {
            using (var ctx = new BPMNDBEntities())
            {
                if (Util.LcsSfw == null)
                {
                    MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                    Application.Current.Shutdown();
                }
                if (Util.LcsSfw.MaxTnoPosPst != -1 &&
                    ctx.TblPosPstOrgs.LongCount() >= Util.LcsSfw.MaxTnoPosPst)
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 81);
                    MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                        msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                        (r) => { },
                        null);
                    return;
                }
            }
            var vm = this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel;

            vm.ExecuteAddFirstNodeCommand();

            tree.NodesSource = null;

            tree.NodesSource = vm.PosPst;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblPosPstOrg>).Count() > 0)
            {
                RefreshLayout(-1, graphControl.Graph.Nodes.First());
            }

            vm.RefreshTree();

        }

        /// <summary>
        /// امکان افزودن یک جایگاه به عنوان زیر شاخه یک سمت نباید داده شود.
        /// </summary>
        public void RebindChartAfterAddPos()
        {
            using (var ctx = new BPMNDBEntities())
            {
                if (Util.LcsSfw == null)
                {
                    MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                    Application.Current.Shutdown();
                }
                if (Util.LcsSfw.MaxTnoPosPst != -1 &&
                    ctx.TblPosPstOrgs.LongCount() >= Util.LcsSfw.MaxTnoPosPst)
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 81);
                    MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                        msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                        (r) => { },
                        null);
                    return;
                }
            }
            var vm = this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel;

            var addedItem = vm.ExecuteAddNewPosPstCommand(Model.Enum.PosPst.Pos);

            if (addedItem == null)
            {
                return;
            }

            tree.NodesSource = null;

            tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel).PosPst;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblPosPstOrg>).Count() > 0)
            {
                if (addedItem != null)
                {
                    List<INode> res = new List<INode>(graphControl.Graph.Predecessors(graphControl.Graph.Nodes.SingleOrDefault(n => n.Tag == addedItem.TblPosPstOrg2)));
                    RefreshLayout(-1, res.FirstOrDefault());
                }
            }
            BringIntoView(addedItem);
            int str = (addedItem as TblPosPstOrg).FldCodPosPst;
            INode selectedNode = graphControl.Graph.Nodes.SingleOrDefault(m => (m.Tag as TblPosPstOrg).FldCodPosPst == str);
            INode nod = UIUtil.findNodByTag(graphControl, addedItem, out selectedNode);
            graphControl.CurrentItem = selectedNode;
            FldCodePost = str;
        }

        int FldCodePost = 0;
        /// <summary>
        /// 
        /// </summary>
        public void RebindChartAfterAddPst()
        {
            using (var ctx = new BPMNDBEntities())
            {
                if (Util.LcsSfw == null)
                {
                    MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                    Application.Current.Shutdown();
                }
                if (Util.LcsSfw.MaxTnoPosPst != -1 &&
                    ctx.TblPosPstOrgs.LongCount() >= Util.LcsSfw.MaxTnoPosPst)
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 81);
                    MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                        msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                        (r) => { },
                        null);
                    return;
                }
            }
            var addedItem = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel).ExecuteAddNewPosPstCommand(Model.Enum.PosPst.Pst);

            if (addedItem == null)
            {
                return;
            }

            tree.NodesSource = null;
            
            tree.NodesSource = (this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel).PosPst;

            if (tree.NodesSource != null && (tree.NodesSource as IEnumerable<TblPosPstOrg>).Count() > 0)
            {
                if (addedItem != null)
                {
                    List<INode> res = new List<INode>(graphControl.Graph.Predecessors(graphControl.Graph.Nodes.SingleOrDefault(n => n.Tag == addedItem.TblPosPstOrg2)));
                    RefreshLayout(-1, res.FirstOrDefault());
                    
                }
            }
            BringIntoView(addedItem);

            int str = (addedItem as TblPosPstOrg).FldCodPosPst;
            INode selectedNode = graphControl.Graph.Nodes.SingleOrDefault(m => (m.Tag as TblPosPstOrg).FldCodPosPst == str);
            INode nod = UIUtil.findNodByTag(graphControl, addedItem,out selectedNode);
            graphControl.CurrentItem = selectedNode;
            FldCodePost = str;
        }

        
        

        /// <summary>
        /// Brings a selected item into TreeView ViewPort
        /// </summary>
        /// <param name="obj"></param>
        public void BringIntoView(TblPosPstOrg obj)
        {
            int repeat = treeView.ContainerFromItemRecursive(treeView.SelectedItem).Items.Count;
            double d;
            if (!treeView.ContainerFromItemRecursive(treeView.SelectedItem).IsExpanded)
            {
                d = treeView.ScrollViewer.ContentVerticalOffset + treeView.ContainerFromItemRecursive(treeView.SelectedItem).ActualHeight * (repeat );
                (treeView.SelectedItem as TblPosPstOrg).IsExpanded = true;
            }
            else
            {
                 d = treeView.ScrollViewer.ContentVerticalOffset + treeView.ContainerFromItemRecursive(treeView.SelectedItem).ActualHeight;
            }
            treeView.ScrollViewer.ScrollToTop();
            treeView.ScrollViewer.ScrollToVerticalOffset(d);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RebindChartAfterDeletePosPst()
        {
            var vm = this.DataContext as ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel;

            if (vm.SelectedOrg != null)
            {
                TblPosPstOrg posPst = vm.SelectedPosPst;

                if (!posPst.FldCodUpl.HasValue)
                {
                    Util.ShowMessageBox(16, "جایگاه سرشاخه");
                    return;
                }

                TblPosPstOrg parent = posPst.TblPosPstOrg2;

                string posOrPst = (Model.Enum.PosPst)posPst.FldCodTyp == Model.Enum.PosPst.Pos ? "این جایگاه" : "این سمت";

                var codMsg = 0;
                if (posPst.TblPosPstOrg1.Count > 0)
                {
                    codMsg = 30;
                }
                else
                {
                    codMsg = 2;
                }

                var result = Util.ShowMessageBox(codMsg, posOrPst);

                if (result == MessageBoxResult.OK
                    || result == MessageBoxResult.Yes)
                {
                    INode parentNod;
                    INode nod = UIUtil.findNodByTag(graphControl, posPst, out parentNod);

                    if (!PublicMethods.DeletePosPst_2221(vm.bpmnEty, posPst))
                    {
                        Util.ShowMessageBox(29, posOrPst);
                    }
                    else
                    {
                        //var successors = graphControl.Graph.Successors(nod).ToList();

                        graphControl.CurrentItem = graphControl.Graph.Predecessors(nod).FirstOrDefault();

                        var childs = UIUtil.findAllChilds(graphControl.Graph, nod);
                        for (int i = 0; i < childs.Count; i++)
                        {
                            graphControl.Graph.Remove(childs[i]);
                        }

                        if (parent != null)
                        {
                            if (parent.SubPosPst.Contains(posPst))
                            {
                                parent.SubPosPst.Remove(posPst);
                            }
                            //this.Dispatcher.Invoke(new Action(() =>
                            //    {
                            //        parent.ChildsCV.Remove(posPst);
                            //    }));
                            //parent.SubPosPst.Remove(posPst);
                            //parent.ChildsCV.Remove(posPst);
                            //parent.ChildsCV.Refresh();
                        }
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
                                                                                typeof(PosPstChart));

        /// <summary>
        /// The command that can be used by the buttons to hide the parent node.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand HideParentCommand = new RoutedUICommand("Hide Parent", "HideParent",
                                                                                typeof(PosPstChart));

        /// <summary>
        /// The command that can be used by the buttons to show the child nodes.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand ShowChildrenCommand = new RoutedUICommand("Show Children", "ShowChildren",
                                                                                typeof(PosPstChart));

        /// <summary>
        /// The command that can be used by the buttons to hide the child nodes.
        /// </summary>
        /// <remarks>
        /// This command requires the corresponding <see cref="INode"/> as the <see cref="ExecutedRoutedEventArgs.Parameter"/>.
        /// </remarks>
        public static readonly RoutedUICommand HideChildrenCommand = new RoutedUICommand("Hide Children", "HideChildren",
                                                                                typeof(PosPstChart));

        /// <summary>
        /// The command that can be used by the buttons to expand all collapsed nodes.
        /// </summary>
        public static readonly RoutedUICommand ShowAllCommand = new RoutedUICommand("Show All", "ShowAll",
                                                                                typeof(PosPstChart));

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
            //var nodeStyle = new NodeControlNodeStyle("PosPsteNodeControlStyle");

            //graphControl.CurrentItemChanged += graphControl_CurrentItemChanged;

            //graphControl.Graph.NodeDefaults.Style = nodeStyle;
            graphControl.Graph.NodeDefaults.Size = new SizeD(250, 100);

            //graphControl.Graph.EdgeDefaults.Style = new PolylineEdgeStyle { Smoothing = 10, };

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
                    Duration = TimeSpan.FromMilliseconds(0),
                    
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
            INode selectedNode = (sender as GraphControl).Graph.Nodes.SingleOrDefault(m => (m.Tag as TblPosPstOrg).FldCodPosPst == FldCodePost);
            graphControl.CurrentItem = selectedNode;
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
                int str = (treeView.SelectedItem as TblPosPstOrg).FldCodPosPst;
                INode selectedNode = graphControl.Graph.Nodes.SingleOrDefault(m => (m.Tag as TblPosPstOrg).FldCodPosPst == str);
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
                var selected = graphControl.CurrentItem.Tag as TblPosPstOrg;

                int count = 0;
                if (treeView.SelectedItem != selected)
                {
                    treeView.CollapseAll();
                    var parent = selected.SearchParent;
                    var child=selected;
                    while (parent != null)
                    {
                        IList<Model.Base.ISearchableTree> temp=parent.SearchChilds;
                        int i = 0;
                        count++;
                        while ((temp[i] as TblPosPstOrg) != child)
                        {
                            i++; count++;
                        }
                        parent.IsExpanded = true;
                        child = parent as TblPosPstOrg;
                        parent = parent.SearchParent;
                    }
                    treeView.SelectedItem = selected;
                    treeView.ScrollViewer.ScrollToTop();
                    treeView.ScrollViewer.ScrollToVerticalOffset(count * 36);
                    
                }
                
            }


            //foreach (var item in treeView.Items)
            //{
            //    SelectCurrentItemInTreeRec(treeView, item);
            //}
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
            //Task setFirstSelected = new Task(new Action(() =>
            //{
            //    Thread.Sleep(2000);


            //    Dispatcher.Invoke(new Action(() =>
            //    {
            //        ZoomToTreeItem();
            //    }));
            //}));

            //setFirstSelected.Start();
            /*if(filteredGraphWrapper!=null)
                ZoomToCurrentItem();*/
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
            (this.DataContext as SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork.PosPstChartViewModel).PropertyChanged += PosPstChart_PropertyChanged;

        }

        private void treeView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ZoomToTreeItem();
            //انتخاب جایگاه سرشاخه جدید
            
            /*if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                
                treeView.BringItemIntoView(e.AddedItems[0]);
            }*/
        }

    

        
    }


}
