using System.IO;
using System.Threading;
using Microsoft.Win32;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.View.Process.Windows;
using SSYM.OrgDsn.ViewModel.Process.UserCtl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.Model.Base;
using System.Data.Objects.DataClasses;
using SSYM.OrgDsn.Model.BPMNShapes;
using SSYM.OrgDsn.ViewModel.Utility;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Model;
using yWorks.Canvas;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry.Structs;
using yWorks.Canvas.Input;
using yWorks.Canvas.Model;
using yWorks.Support;
using yWorks.Support.Annotations;
using yWorks.Support.Extensions;
using yWorks.yFiles.Layout;
using yWorks.yFiles.Layout.Hierarchic;
using yWorks.yFiles.Layout.Hierarchic.Incremental;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Input;
using yWorks.yFiles.UI.Drawing.Common;
using yWorks.Canvas.Geometry;
using yWorks.yFiles.UI;
using SSYM.OrgDsn.Model.BPMNDgm.Styles;
using SSYM.OrgDsn.Model.BPMNDgm.Model;
using yWorks.yFiles.Layout.Partial;
using yWorks.yFiles.GraphML;
using SSYM.OrgDsn.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using Demo.yFiles.Option.Handler;
using System.Resources;
using System.Printing;
using Demo.yFiles.Option.DataBinding;
using System.Globalization;


namespace SSYM.OrgDsn.UI.View.Process.UserCtl
{
    /// <summary>
    /// Interaction logic for DisPrc.xaml
    /// </summary>
    public partial class DisPrs : UserControl
    {
        DisPrsViewModel disPrsVM;

        BPMNDBEntities context;

        private bool runningLayout;

        Brush graphBackground = null;

        private static readonly Color ColorPartialNode = Color.FromRgb(255, 151, 0);

        private static readonly CollapsibleNodeStyleDecorator PartialGroupNodeStyle =
      new CollapsibleNodeStyleDecorator(new PanelNodeStyle
      {
          Color = Color.FromArgb(255, 202, 220, 255),
          LabelInsetsColor = ColorPartialNode,
          Insets = new InsetsD(0, 0, 0, 0)
      });

        List<Tuple<INode, INode, IEdge>> lstLnk = null;

        INode groupNode = null;
        bool isLayouted = false;

        /// <summary>
        /// 
        /// </summary>
        public DisPrs()
        {
            DataGridRow r;
            
            //r.Item
            InitializeComponent();

            context = new BPMNDBEntities();

            InitializeGraph(grphCtl);

            InitializeGrphForPartialLayout(grphCtl);

            this.Loaded += DisPrs_Loaded;

            graphBackground = FindResource("GraphControlBackgroundColor") as SolidColorBrush;
            //GreenYellow
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DisPrs_Loaded(object sender, RoutedEventArgs e)
        {
            disPrsVM = UsrCtl.DataContext as DisPrsViewModel;

            // پنهان سازی ستون وضعیت در لیست فعالیت ها
            //this.dgrdPrs.Columns[3].Visibility = disPrsVM.FlagVisible ? Visibility.Visible : System.Windows.Visibility.Collapsed;

            grphCtl.LayoutUpdated += grphCtl_LayoutUpdated;


            OnLoaded();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void grphCtl_LayoutUpdated(object sender, EventArgs e)
        {
            //AddAttachedNodes(groupNode);

            //layout2(groupNode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDiagram(object sender, RoutedEventArgs e)
        {

            var prvwPrs = new PrvwPrs(grphCtl, grphCtl.Width, grphCtl.Height, disPrsVM.SelectedPrs.FldNamPrs);
            prvwPrs.ShowDialog();
            //SaveFileDialog save = new SaveFileDialog();
            //save.OverwritePrompt = false;
            //save.Filter = "XML (*.xml)|*.xml";
            //save.RestoreDirectory = true;
            //save.Title = "ذخیره دیاگرام";
            //save.CheckPathExists = false;
            //if (save.ShowDialog() == true)
            //{
            //    disPrsVM.CurrentDgm.GrphCtl.SaveToXml(save.FileName);

            //    PdfExporter pdf = new PdfExporter();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomInButton_Click(object sender, RoutedEventArgs e)
        {
            //disPrsVM.CurrentDgm.GrphCtl.ZoomFactor = Math.Min(1000, disPrsVM.CurrentDgm.GrphCtl.ZoomFactor + 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            //disPrsVM.CurrentDgm.GrphCtl.ZoomFactor = Math.Max(20, disPrsVM.CurrentDgm.GrphCtl.ZoomFactor - 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fitButton_Click(object sender, RoutedEventArgs e)
        {
            //Rect union = Rect.Empty;
            //foreach (DiagramNode node in disPrsVM.CurrentDgm.GrphCtl.Nodes)
            //{
            //    if (union.IsEmpty)
            //        union = node.Bounds;
            //    else
            //        union.Union(node.Bounds);
            //}

            //union.Inflate(50, 50);

            //disPrsVM.CurrentDgm.GrphCtl.ZoomToRect(union);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noZoomButton_Click(object sender, RoutedEventArgs e)
        {
            //disPrsVM.CurrentDgm.GrphCtl.ZoomFactor = 100;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayPrs(object sender, SelectionChangedEventArgs e)
        {
            
            if (disPrsVM.SelectedPrs == null || this.DataContext == null)
            {
                return;
            }
            disPrsVM.DisplayingPrs = true;
            
                if (!(this.DataContext as DisPrsViewModel).Acs_ViewPrs)
                {
                    UserManager.ShowAccessMessage(17, "مشاهده این فرآیند");

                    return;
                }

                List<INode> lst1 = grphCtl.Graph.Nodes.ToList();

                foreach (INode item in lst1)
                {
                    grphCtl.Graph.Remove(item);
                }

                List<IEdge> lst2 = grphCtl.Graph.Edges.ToList();

                foreach (IEdge item in lst2)
                {
                    grphCtl.Graph.Remove(item);
                }

                grphCtl.Graph.Clear();

                yWorks.yFiles.UI.Model.Table tbl = new yWorks.yFiles.UI.Model.Table();

                tbl.CreateColumn(300);

                var groupedGraph = grphCtl.Graph.GetGroupedGraph();
                // create the actual table node at the root level
                groupNode = groupedGraph.CreateGroupNode(groupedGraph.Hierarchy.Root, new RectD(new PointD(10, 10), tbl.Layout.GetSize()),
                                                             new TableNodeStyle(tbl)
                                                             {
                                                                 TableRenderingOrder = TableRenderingOrder.RowsFirst,
                                                                 BackgroundStyle = new ShapeNodeStyle() { Brush = graphBackground }
                                                             });

                PublicMethods.DisPrs_3015(context, grphCtl, groupNode, disPrsVM.SelectedPrs);

                //string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                //grphCtl.ExportToGraphML(str + @"\sss.xsd");

                ClearAllPropertiesOfPrs(disPrsVM.SelectedPrs);

                disPrsVM.CurrentDgm = new DisPrsViewModel.DgmOfPrs();

                disPrsVM.CurrentDgm.GrphCtl = grphCtl;

                if (grphCtl.Graph.Nodes.Count == 1)
                {
                    grphCtl.Graph.Clear();
                }

                else
                {
                    layout1(groupNode);
                }
                disPrsVM.DisplayingPrs = false;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prs"></param>
        private void ClearAllPropertiesOfPrs(TblPr prs)
        {
            if (prs.TblNod != null)
            {
                prs.TblNod.Tbl = null;

                prs.TblNod.GrpNod = null;

                prs.TblNod.Col = null;
            }

            List<TblAct> act = PublicMethods.DetectActsOfPrs_946(context, prs);

            ClearAllPropertiesOfActs(act);

            List<TblAct> actUspf = PublicMethods.DetectActUspfPrs_21839(context, prs);

            ClearAllPropertiesOfActs(actUspf);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        private void ClearAllPropertiesOfActs(List<TblAct> act)
        {
            foreach (TblAct act_i in act)
            {
                act_i.Shp = null;

                if (act_i.TblNod != null)
                {
                    act_i.TblNod.Tbl = null;

                    act_i.TblNod.GrpNod = null;

                    act_i.TblNod.Col = null;
                }

                foreach (TblEvtRst evtRst in act_i.TblEvtRsts)
                {
                    evtRst.Shp = null;

                    foreach (TblNew objRst in evtRst.TblNews)
                    {
                        objRst.Shp = null;

                        objRst.Shp1 = null;
                    }

                    foreach (TblObj objRst in evtRst.TblObjs)
                    {
                        objRst.Shp = null;

                        objRst.Shp1 = null;
                    }

                    foreach (TblSbjOral objRst in evtRst.TblSbjOrals)
                    {
                        objRst.Shp = null;

                        objRst.Shp1 = null;
                    }
                }

                foreach (TblEvtSrt evtSrt in act_i.TblEvtSrts)
                {
                    evtSrt.Shp = null;

                    foreach (TblWayAwr_Oral wayAwr in evtSrt.TblWayAwr_Oral)
                    {
                        wayAwr.ObjRst.Shp = null;

                        wayAwr.ObjRst.Shp1 = null;
                    }

                    foreach (TblWayAwr_News wayAwr in evtSrt.TblWayAwr_News)
                    {
                        wayAwr.ObjRst.Shp = null;

                        wayAwr.ObjRst.Shp1 = null;
                    }

                    foreach (TblWayAwr_RecvInt wayAwr in evtSrt.TblWayAwr_RecvInt)
                    {
                        wayAwr.ObjRst.Shp = null;

                        wayAwr.ObjRst.Shp1 = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgmOfPrs"></param>
        private void InitializeGraph(GraphControl grphCtl1)
        {
            InitializeInputModes(grphCtl);

            // set the edge label model
            grphCtl.Graph.EdgeDefaults.Labels.LabelModelParameter = new SideSliderEdgeLabelModel().CreateDefaultParameter();

            // generate style nodes
            //GenerateDefaultStyles(graphControl.Graph);

            var defaultGraph = grphCtl.Graph.SafeGet<DefaultGraph>();

            // enable grouping
            defaultGraph.GroupingSupported = true;

            //Configure Undo...
            //Enable general undo support
            defaultGraph.UndoEngineEnabled = true;
            //Use the undo support from the graph also for all future table instances
            yWorks.yFiles.UI.Model.Table.RegisterStaticUndoSupport(defaultGraph, defaultGraph.Get<IUndoSupport>());

            // create a default style for group nodes
            var dashDotPen = new Pen { Brush = Brushes.Black, DashStyle = DashStyles.DashDot, Thickness = 1 };
            grphCtl.Graph.GetGroupedGraph().GroupNodeDefaults.Style = new ShapeNodeStyle(ShapeNodeShape.RoundRectangle, dashDotPen, null);

            // make sure the graph fits
            grphCtl.FitGraphBounds();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grphCtl1"></param>
        private void InitializeGrphForPartialLayout(GraphControl grphCtl1)
        {
            var fm = new FoldingManager();
            grphCtl1.Graph = fm.CreateManagedView().Graph;

            IGraph graph = grphCtl1.Graph;
            grphCtl1.NavigationCommandsEnabled = true;

            //Create graph
            //graph.NodeDefaults.Size = new SizeD(60, 30);

            // set the style as the default for all new nodes
            //graph.NodeDefaults.Style = PartialNodeStyle;

            // set the style as the default for all new edges
            //graph.EdgeDefaults.Style = PartialEdgeStyle;

            IGroupedGraph grouped = graph.GetGroupedGraph();
            if (grouped != null)
            {
                CollapsibleNodeStyleDecorator groupNodeStyle = PartialGroupNodeStyle;
                grouped.GroupNodeDefaults.Labels.LabelModelParameter = InteriorStretchLabelModel.North;
                grouped.GroupNodeDefaults.Labels.Style = new SimpleLabelStyle { TypefaceFormat = { TextAlignment = TextAlignment.Right } };
                grouped.GroupNodeDefaults.Style = groupNodeStyle;
            }


            //Create and register mappers that specifiy partial graph elements
            partialNodesMapper = new DictionaryMapper<INode, bool> { DefaultValue = true };
            partialEdgesMapper = new DictionaryMapper<IEdge, bool> { DefaultValue = true };
            grphCtl1.Graph.GetFoldedGraph().Manager.MasterGraph.MapperRegistry.AddMapper(
              PartialLayouter.PartialNodesDpKey, partialNodesMapper);
            grphCtl1.Graph.GetFoldedGraph().Manager.MasterGraph.MapperRegistry.AddMapper(
              PartialLayouter.PartialEdgesDpKey, partialEdgesMapper);

            var graphMlioHandler = new GraphMLIOHandler();
            graphMlioHandler.AddInputMapper((string)PartialLayouter.PartialNodesDpKey, partialNodesMapper);
            graphMlioHandler.AddInputMapper((string)PartialLayouter.PartialEdgesDpKey, partialEdgesMapper);

            var lookupDecorator = grphCtl1.Get<ILookupDecorator>();
            if (lookupDecorator != null)
            {
                lookupDecorator.AddConstant<GraphControl, GraphMLIOHandler>(graphMlioHandler);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgmOfPrs"></param>
        protected virtual void InitializeInputModes(GraphControl grphCtl)
        {
            grphCtl.InputMode = new GraphViewerInputMode();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prs"></param>
        /// <returns></returns>
        private DisPrsViewModel.DgmOfPrs IsPrsOpen(TblPr prs)
        {
            foreach (DisPrsViewModel.DgmOfPrs item in disPrsVM.OpenedDgms)
            {
                if (item.Prs == prs)
                {
                    return item;
                }
            }

            return null;
        }

        #region Layout

        /// <summary>
        /// Event callback to handle the automatic layout.
        /// </summary>
        private void layout1(INode groupNode)
        {
            RemoveAttachedNodes();

            if (!runningLayout)
            {
                runningLayout = true;

                var graph = disPrsVM.CurrentDgm.GrphCtl.Graph;

                // Automatically determine port constraints for source and target
                // store the mapping in the mapper registry
                var portCandidateMap = graph.MapperRegistry.AddMapper<INode, PortCandidateSet>(PortCandidateSet.NodeDpKey);

                // iterate through all nodes
                foreach (var node in graph.Nodes)
                {
                    // if it isn't a BPDM node, do nothing
                    var style = node.Style as BPDMNodeStyleBase;
                    if (style != null)
                    {
                        // create a mapping from the specified node to the calculated candidate set
                        portCandidateMap[node] = style.GetPortCandidateSet(node);
                    }
                }

                // incoming edges must be at the west (left) side of the node
                graph.MapperRegistry.AddConstantMapper<IEdge, PortConstraint>(PortConstraintKeys.TargetPortConstraintDpKey, PortConstraint.Create(PortSide.West, false));

                // create the layouter
                var ihl = new IncrementalHierarchicLayouter()
                {
                    ComponentLayouterEnabled = false,

                    LayoutOrientation = yWorks.yFiles.Layout.LayoutOrientation.LeftToRight,
                    OrthogonalRouting = true,
                    RecursiveGroupLayering = false,
                    EdgeLayoutDescriptor =
                    {
                        MinimumFirstSegmentLength = 15,
                        MinimumLastSegmentLength = 15,
                        OrthogonallyRouted = true,
                        MinimumDistance = 10
                    },
                    NodeLayoutDescriptor =
                    {
                        LayerAlignment = 0.5
                    },
                    ConsiderNodeLabels = true,
                    IntegratedEdgeLabeling = true,
                };
                // add custom port optimizer
                ihl.HierarchicLayouter.PortConstraintOptimizer = new BPMNPortOptimizer();
                ((SimplexNodePlacer)ihl.NodePlacer).BaryCenterMode = true;

                //We use Layout executor convenience method that already sets up the whole layout pipeline correctly
                new LayoutExecutor(disPrsVM.CurrentDgm.GrphCtl, ihl)
                {
                    //Table layout is enabled by default already...
                    ConfigureTableNodeLayout = true,
                    Duration = TimeSpan.FromMilliseconds(500),
                    AnimateViewport = true,
                    UpdateContentRect = true,
                    TableLayoutConfigurator =
                    {
                        //Table cells may only grow by an automatic layout.
                        CompactionEnabled = false,
                        //Keep the order of pool nodes as in the sketch
                        FromSketch = true
                    },
                    RunInThread = false,
                    FinishHandler = (o, args) =>
                    {
                        graph.MapperRegistry.RemoveMapper(PortCandidateSet.NodeDpKey);
                        graph.MapperRegistry.RemoveMapper(PortConstraintKeys.TargetPortConstraintDpKey);
                        runningLayout = false;
                    }
                }.Start();
            }
        }

        /// <summary>
        /// Event callback to handle the automatic layout.
        /// </summary>
        private void layout2(INode groupNode)
        {
            //RemoveAttachedNodes();

            if (!runningLayout)
            {
                runningLayout = true;

                var graph = disPrsVM.CurrentDgm.GrphCtl.Graph;

                // Automatically determine port constraints for source and target
                // store the mapping in the mapper registry
                var portCandidateMap = graph.MapperRegistry.AddMapper<INode, PortCandidateSet>(PortCandidateSet.NodeDpKey);

                // iterate through all nodes
                foreach (var node in graph.Nodes)
                {
                    // if it isn't a BPDM node, do nothing
                    var style = node.Style as BPDMNodeStyleBase;
                    if (style != null)
                    {
                        // create a mapping from the specified node to the calculated candidate set
                        portCandidateMap[node] = style.GetPortCandidateSet(node);
                    }
                }

                // incoming edges must be at the west (left) side of the node
                graph.MapperRegistry.AddConstantMapper<IEdge, PortConstraint>(PortConstraintKeys.TargetPortConstraintDpKey, PortConstraint.Create(PortSide.West, false));

                var executor = new LayoutExecutor(
        disPrsVM.CurrentDgm.GrphCtl, CreateConfiguredPartialLayouter())
        {
            Duration = TimeSpan.FromMilliseconds(500),
            TargetBoundsInsets = new InsetsD(0),
            AnimateViewport = true,


            //Table layout is enabled by default already...
            ConfigureTableNodeLayout = false,
            UpdateContentRect = false,
            //TableLayoutConfigurator =
            //{
            //    //Table cells may only grow by an automatic layout.
            //    CompactionEnabled = false,
            //    //Keep the order of pool nodes as in the sketch
            //    FromSketch = true
            //},
            UseWaitInputMode = true,
            RunInThread = false,
            FinishHandler = (o, args) =>
            {
                graph.MapperRegistry.RemoveMapper(PortCandidateSet.NodeDpKey);
                graph.MapperRegistry.RemoveMapper(PortConstraintKeys.TargetPortConstraintDpKey);
                runningLayout = false;
            }

        };
                executor.Start();
            }
        }

        // the mapper storing if a node/edge is fixed or shall be moved by the partial layouter

        private static readonly IDictionary<string, ILayouter> SubGraphLayouters = new Dictionary<string, ILayouter>(4);
        private DictionaryMapper<IEdge, bool> partialEdgesMapper;
        private DictionaryMapper<INode, bool> partialNodesMapper;

        ///<summary>Create a <c>PartialLayouter</c> using the selected demo settings</summary>
        private ILayouter CreateConfiguredPartialLayouter()
        {
            var subGraphLayouter = new IncrementalHierarchicLayouter()
                {
                    LayoutMode = yWorks.yFiles.Layout.Hierarchic.LayoutMode.Incremental,
                    OrthogonalRouting = true,
                    ComponentLayouterEnabled = false,
                    AutomaticEdgeGrouping = true,
                    LayoutOrientation = yWorks.yFiles.Layout.LayoutOrientation.LeftToRight,
                    RecursiveGroupLayering = false,
                    EdgeLayoutDescriptor =
                    {
                        MinimumFirstSegmentLength = 15,
                        MinimumLastSegmentLength = 15,
                        OrthogonallyRouted = true,
                        MinimumDistance = 10
                    },
                    NodeLayoutDescriptor =
                    {
                        LayerAlignment = 0.5
                    },
                    ConsiderNodeLabels = true,
                    IntegratedEdgeLabeling = true
                };

            subGraphLayouter.HierarchicLayouter.PortConstraintOptimizer = new BPMNPortOptimizer();


            var partialLayouter = new PartialLayouter
            {
                ComponentAssignmentStrategy = yWorks.yFiles.Layout.Partial.ComponentAssignmentStrategy.Single,
                CoreLayouter = subGraphLayouter,
                PositioningStrategy = SubgraphPositioningStrategy.Barycenter,
                EdgeRoutingStrategy = yWorks.yFiles.Layout.Partial.EdgeRoutingStrategy.Orthogonal,
                LayoutOrientation = yWorks.yFiles.Layout.Partial.LayoutOrientation.None,
                MinimalNodeDistance = 100,
                AllowMirroring = false,
            };



            return partialLayouter;
        }

        /// <summary>
        /// This class optimizes the port allocation during the layout of business process diagrams
        /// </summary>
        private class BPMNPortOptimizer : IPortConstraintOptimizer
        {
            public void OptimizeAfterLayering(LayoutGraph graph, ILayers layers, ILayoutDataProvider ldp, yWorks.yFiles.Layout.Hierarchic.Incremental.IItemFactory itemFactory)
            {
                // nothing is changed after layering.
            }

            /// <summary>
            /// /// Redistributes some edge ports of a node according to simple rules.
            /// </summary>
            /// <remarks>
            /// <list type="bullet">
            /// <item>
            /// <description>If neither the in degree nor the out degree of the node is bigger than 1, nothing is done.</description>
            /// </item>
            /// <item>
            /// <description>If out degree is 2 and in degree &lt; 2, the leftmost out edge is assigned to a port on the west side.</description>
            /// </item>
            /// <item>
            /// <description>If out degree &gt; 2 and in degree &lt; out degree, the leftmost out edge is assigned to a port on the west side
            ///  and the rightmost out edge is assigned to a port on the est side.</description>
            /// </item>
            /// <item>
            /// <description>If in degree is 2 and out degree is 2, the rightmost in edge is assigned to a port on the east side
            /// and the leftmost out edge is assigned to a port on the west side.</description>
            /// </item>
            /// <item>
            /// <description>If in degree is 2 and out degree &lt; 2, the rightmost in edge is assigned to a port on the east side.</description>
            /// </item>
            /// <item>
            /// <description>If in degree &gt; 2 and out degree &lt; in degree, the rightmost in edge is assigned to a port on the east side
            /// and the leftmost in edge is assigned to a port on the west side.</description>
            /// </item>
            /// </list>
            /// </remarks>
            /// <param name="graph"></param>
            /// <param name="layers"></param>
            /// <param name="layoutData"></param>
            /// <param name="itemFactory"></param>
            public void OptimizeAfterSequencing(LayoutGraph graph, ILayers layers, ILayoutDataProvider layoutData, yWorks.yFiles.Layout.Hierarchic.Incremental.IItemFactory itemFactory)
            {
                foreach (var node in graph.Nodes)
                {
                    if (layoutData.GetNodeData(node).Type == NodeDataType.Normal && (node.OutDegree > 1 || node.InDegree > 1))
                    {
                        if (node.OutDegree > node.InDegree)
                        {
                            itemFactory.SetTemporaryPortConstraint(node.FirstOutEdge, true, PortConstraint.Create(PortSide.West));
                            if (node.OutDegree > 2)
                            {
                                itemFactory.SetTemporaryPortConstraint(node.LastOutEdge, true, PortConstraint.Create(PortSide.East));
                            }
                        }
                        else
                        {
                            itemFactory.SetTemporaryPortConstraint(node.LastInEdge, false, PortConstraint.Create(PortSide.East));
                            if (node.InDegree > 2)
                            {
                                itemFactory.SetTemporaryPortConstraint(node.FirstInEdge, false, PortConstraint.Create(PortSide.West));
                            }
                            else if (node.OutDegree > 1)
                            {
                                itemFactory.SetTemporaryPortConstraint(node.FirstOutEdge, true, PortConstraint.Create(PortSide.West));
                            }
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        private void SetProperties(ref DisPrsViewModel.DgmOfPrs dgm)
        {
            //dgm.GrphCtl.LinkHeadShapeSize = 12;

            //dgm.GrphCtl.LinkHeadShape = MindFusion.Diagramming.Wpf.Shapes.Arrow1;

            //dgm.GrphCtl.LinkCascadeOrientation = MindFusion.Diagramming.Wpf.Orientation.Horizontal;

            //dgm.GrphCtl.EnableLanes = true;

            //dgm.GrphCtl.LinkCascadeOrientation = MindFusion.Diagramming.Wpf.Orientation.Horizontal;

            //dgm.GrphCtl.LinkCrossings = LinkCrossings.Arcs;

            //dgm.GrphCtl.LinkHitDistance = 5;

            //dgm.GrphCtl.LinksRetainForm = false;

            //dgm.GrphCtl.NodesExpandable = true;

            //dgm.GrphCtl.RoundedLinks = false;

            //dgm.GrphCtl.AllowLinksRepeat = false;

            //dgm.GrphCtl.AllowSelfLoops = true;

            //dgm.GrphCtl.AllowInplaceEdit = true;

            //dgm.GrphCtl.AllowUnanchoredLinks = false;

            //dgm.GrphCtl.AutoAlignDistance = 10;

            //dgm.GrphCtl.AutoAlignNodes = true;

            //dgm.GrphCtl.AutoHighlightRows = true;

            //dgm.GrphCtl.AutoResize = AutoResize.AllDirections;

            //dgm.GrphCtl.AutoScroll = true;

            //dgm.GrphCtl.AutoSnapDistance = 10;

            //dgm.GrphCtl.LinkSegments = 3;

            //dgm.GrphCtl.LinkShape = LinkShape.Cascading;

            //dgm.GrphCtl.AutoSnapLinks = true;

            //var router = dgm.Dgm.LinkRouter as QuickRouter;
            //if (router != null)
            //    router.UBendMaxLen = 10;
            //dgm.Dgm.RoutingOptions.TriggerRerouting |= RerouteLinks.WhileCreating;
            //dgm.Dgm.RoutingOptions.Anchoring = Anchoring.Keep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewPrs(object sender, RoutedEventArgs e)
        {
            if (disPrsVM.CurrentDgm != null)
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                //disPrsVM.CurrentDgm.Dgm.SaveToXml(doc);

                //string str = disPrsVM.CurrentDgm.Dgm.SaveToString();

                //Process.Windows.PrvwPrs prvwPrs = new Windows.PrvwPrs(disPrsVM.CurrentDgm.GrphCtl);

                //prvwPrs.ShowDialog();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveAttachedNodes()
        {
            lstLnk = new List<Tuple<yWorks.yFiles.UI.Model.INode, yWorks.yFiles.UI.Model.INode, yWorks.yFiles.UI.Model.IEdge>>();

            List<INode> lstNod = disPrsVM.CurrentDgm.GrphCtl.Graph.Nodes.ToList();

            foreach (INode nod in lstNod)
            {
                if (nod.Style.GetType() == typeof(ActivityNodeStyle))
                {
                    INode n = null;

                    //if ((nod.Style as ActivityNodeStyle).Activity.AttachedNodes.Count == 1)
                    //{
                    //    ShapeNodeStyle style = new ShapeNodeStyle() { Brush = Brushes.Transparent, Pen = new Pen(Brushes.Transparent, 0), Shape = ShapeNodeShape.Ellipse };

                    //    RectD rec = new RectD(new PointD(nod.Layout.ToRectD().X, nod.Layout.ToRectD().Y), new SizeD(25,25));

                    //    INode subNode = disPrsVM.CurrentDgm.GrphCtl.Graph.GetGroupedGraph().CreateNode(nod, rec, style);

                    //    (nod.Style as ActivityNodeStyle).Activity.AttachedNodes.Add(subNode);
                    //}

                    foreach (INode nod1 in (nod.Style as ActivityNodeStyle).Activity.AttachedNodes)
                    {
                        //if (n != null)
                        //{
                        //    disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge(nod1, n, new ArcEdgeStyle() { Pen = new Pen(Brushes.Transparent, 0) });
                        //}

                        //n = nod1;

                        //l.AddPlaceNodeBelowConstraint(nod, nod1);

                        List<IEdge> lnk1 = disPrsVM.CurrentDgm.GrphCtl.Graph.InEdgesAt(nod1).ToList();

                        foreach (IEdge lnk in lnk1)
                        {
                            IEdge lnkNew = disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge((INode)lnk.SourcePort.Owner, nod, lnk.Style);

                            lstLnk.Add(new Tuple<INode, INode, IEdge>((INode)lnk.SourcePort.Owner, (INode)lnk.TargetPort.Owner, lnkNew));

                            disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(lnk);
                        }

                        List<IEdge> lnk2 = disPrsVM.CurrentDgm.GrphCtl.Graph.OutEdgesAt(nod1).ToList();

                        foreach (IEdge lnk in lnk2)
                        {
                            IEdge lnkNew = disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge(nod, (INode)lnk.TargetPort.Owner, lnk.Style);

                            INode n1 = (INode)lnk.SourcePort.Owner;

                            INode n2 = (INode)lnk.TargetPort.Owner;

                            lstLnk.Add(new Tuple<INode, INode, IEdge>(n1, n2, lnkNew));

                            disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(lnk);
                        }

                        disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(nod1);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupNode"></param>
        private void AddAttachedNodes(INode groupNode)
        {
            List<INode> lstNod = disPrsVM.CurrentDgm.GrphCtl.Graph.Nodes.ToList();

            foreach (INode nod in lstNod)
            {
                if (nod.Style.GetType() == typeof(ActivityNodeStyle))
                {
                    double d = 0;

                    List<INode> lst = (nod.Style as ActivityNodeStyle).Activity.AttachedNodes.ToList();

                    foreach (INode nod1 in lst)
                    {
                        INode nod2 = disPrsVM.CurrentDgm.GrphCtl.Graph.GetGroupedGraph().CreateNode(groupNode,
                            new RectD(nod.Layout.ToRectD().X + d, nod.Layout.ToRectD().Y + nod.Layout.ToRectD().Height - nod1.Layout.ToRectD().Height / 2, nod1.Layout.ToRectD().Width, nod1.Layout.ToRectD().Height), nod1.Style);
                        d += nod1.Layout.ToRectD().Width + 5;

                        ReplaceItemsOfLstLnk(nod1, nod2);

                        //CopyLinks(nod1, nod2);

                        //disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(nod1);

                        (nod.Style as ActivityNodeStyle).Activity.AttachedNodes.Remove(nod1);

                        (nod.Style as ActivityNodeStyle).Activity.AttachedNodes.Add(nod2);

                        SetFixed(nod2, true);

                        if (disPrsVM.CurrentDgm.GrphCtl.Graph.Contains(nod1))
                        {
                            disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(nod1);
                        }
                    }

                    SetFixed(nod, true);
                }

                SetFixed(nod, true);
            }

            foreach (IEdge lnk in disPrsVM.CurrentDgm.GrphCtl.Graph.Edges)
            {
                if (((INode)lnk.SourcePort.Owner).Style.GetType() != typeof(TableNodeStyle) &&
                    ((INode)lnk.TargetPort.Owner).Style.GetType() != typeof(TableNodeStyle))
                {
                    SetFixed(lnk, true);
                }
            }

            List<Tuple<INode, INode, IEdge>> lstLnk2 = lstLnk.ToList();

            foreach (var item in lstLnk2)
            {
                IEdge lnk = disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge((INode)item.Item1, (INode)item.Item2, item.Item3.Style);

                if (disPrsVM.CurrentDgm.GrphCtl.Graph.Contains(item.Item3))
                {
                    disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(item.Item3);
                }

                //lstLnk.Add(new Tuple<INode, INode, IEdge>(item.Item1, item.Item2, lnk));

                //lstLnk.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nod1"></param>
        /// <param name="nod2"></param>
        private void ReplaceItemsOfLstLnk(INode nod1, INode nod2)
        {
            List<Tuple<INode, INode, IEdge>> lstLnk2 = lstLnk.Where(m => m.Item1 == nod1).ToList();

            foreach (var item in lstLnk2)
            {
                lstLnk.Add(new Tuple<INode, INode, IEdge>(nod2, item.Item2, item.Item3));

                lstLnk.Remove(item);
            }

            List<Tuple<INode, INode, IEdge>> lstLnk3 = lstLnk.Where(m => m.Item2 == nod1).ToList();

            foreach (var item in lstLnk3)
            {
                lstLnk.Add(new Tuple<INode, INode, IEdge>(item.Item1, nod2, item.Item3));

                lstLnk.Remove(item);
            }
        }

        /// <summary>
        /// Sets the given node as fixed or movable and changes its color to indicate its new state.
        /// </summary>
        private void SetFixed(INode node, bool isFixed)
        {
            var mapper = disPrsVM.CurrentDgm.GrphCtl.Graph.MapperRegistry.GetMapper<INode, bool>(PartialLayouter.PartialNodesDpKey);
            mapper[node] = !isFixed;
        }

        /// <summary>
        /// Returns if a given edge is considered fixed or shall be rerouted by the layouter.
        /// Note that an edge always gets rerouted if any of its end nodes may be moved.
        /// </summary>
        private void SetFixed(IEdge edge, bool isFixed)
        {
            var mapper = disPrsVM.CurrentDgm.GrphCtl.Graph.MapperRegistry.GetMapper<IEdge, bool>(PartialLayouter.PartialEdgesDpKey);
            mapper[edge] = !isFixed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nod1"></param>
        /// <param name="nod2"></param>
        private void CopyLinks(INode nod1, INode nod2)
        {
            List<IEdge> lnk1 = disPrsVM.CurrentDgm.GrphCtl.Graph.InEdgesAt(nod1).ToList();

            foreach (IEdge lnk in lnk1)
            {
                IEdge lnkNew = disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge((INode)lnk.SourcePort.Owner, nod2, lnk.Style);

                disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(lnk);
            }

            List<IEdge> lnk2 = disPrsVM.CurrentDgm.GrphCtl.Graph.OutEdgesAt(nod1).ToList();

            foreach (IEdge lnk in lnk2)
            {
                IEdge lnkNew = disPrsVM.CurrentDgm.GrphCtl.Graph.CreateEdge(nod2, (INode)lnk.TargetPort.Owner, lnk.Style);

                disPrsVM.CurrentDgm.GrphCtl.Graph.Remove(lnk);
            }
        }

        #region '  Classes  '

        /// <summary>
        /// Overrides the behavior for the creation of the default label configuration (depends on the type of the node)
        /// </summary>
        public class BPMNGraphEditorInputMode : GraphEditorInputMode
        {
            /// <summary>
            /// Delegates the label configuration to the style if the item is a node and the style knows how to do it.
            /// </summary>
            /// <param name="labeledItem"></param>
            /// <param name="param"></param>
            /// <param name="style"></param>
            /// <param name="preferredSize"></param>
            protected override void CreateDefaultLabelConfiguration(ILabeledItem labeledItem, out ILabelModelParameter param, out ILabelStyle style, out SizeD? preferredSize)
            {
                var node = labeledItem as INode;
                var nodeStyle = node != null ? (node.Style as BPDMNodeStyleBase) : null;
                // if it is a node with the BPDMNodeStyleBase style, use the style to get the parameters
                if (node != null && nodeStyle != null)
                {
                    nodeStyle.CreateDefaultLabelConfiguration(labeledItem, out param, out style, out preferredSize);
                }
                else
                {
                    base.CreateDefaultLabelConfiguration(labeledItem, out param, out style, out preferredSize);
                }
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layOut_Click_1(object sender, RoutedEventArgs e)
        {
            AddAttachedNodes(groupNode);

            layout2(groupNode);
        }

        private void getXPDL()
        {
            
        }
        private void XPDLExport(object sender, RoutedEventArgs e)
        {
            var stream=new MemoryStream();
            grphCtl.ExportToGraphML(stream);
            var aray= stream.ToArray();
            string str1 = Encoding.UTF8.GetString(aray);
            var save=new SaveFileDialog();
            save.FileName = "Untitled";
            save.DefaultExt = ".XPDL";
            save.Filter = "Process Definition Language (.XPDL)|*.XPDL";
            bool? result=save.ShowDialog();
            if (result==true)
            {
                var generator = new XPDLGenerator(str1, save.FileName, disPrsVM.SelectedPrs.FldNamPrs);
            }
        }

        public int i { get; set; }

        private void DisplayPrs2(object sender, SelectionChangedEventArgs e)
        {
            //if (IsPrsOpen(disPrsVM.SelectedPrs) != null)
            //{
            //    disPrsVM.CurrentDgm = IsPrsOpen(disPrsVM.SelectedPrs);
            //}

            //else
            //{
            //DisPrsViewModel.DgmOfPrs dgmOfPrs = new DisPrsViewModel.DgmOfPrs();

            //dgmOfPrs.GrphCtl = new GraphControl();

            //dgmOfPrs.Prs = disPrsVM.SelectedPrs;

            //dgmOfPrs.NamDgm = disPrsVM.SelectedPrs.FldNamPrs;

            //dgmOfPrs.CodPrs = disPrsVM.SelectedPrs.FldCodPrs;

            //disPrsVM.OpenedDgms.Add(dgmOfPrs);

            //InitializeGraph(dgmOfPrs);

            //yWorks.yFiles.UI.Model.Table tbl = new yWorks.yFiles.UI.Model.Table();

            //tbl.CreateColumn(300);

            //var groupedGraph = dgmOfPrs.GrphCtl.Graph.GetGroupedGraph();
            //// create the actual table node at the root level
            //INode groupNode = groupedGraph.CreateGroupNode(groupedGraph.Hierarchy.Root, new RectD(new PointD(10, 10), tbl.Layout.GetSize()),
            //                                             new TableNodeStyle(tbl)
            //                                             {
            //                                                 TableRenderingOrder = TableRenderingOrder.RowsFirst,
            //                                                 BackgroundStyle = new ShapeNodeStyle() { Brush = new SolidColorBrush(Colors.Aqua) }
            //                                             });

            //PublicMethods.DisPrs_3015(context, dgmOfPrs.GrphCtl,groupNode, disPrsVM.SelectedPrs);

            //disPrsVM.CurrentDgm = dgmOfPrs;

            //}

            List<INode> lst1 = grphCtl.Graph.Nodes.ToList();

            foreach (INode item in lst1)
            {
                grphCtl.Graph.Remove(item);
            }

            List<IEdge> lst2 = grphCtl.Graph.Edges.ToList();

            foreach (IEdge item in lst2)
            {
                grphCtl.Graph.Remove(item);
            }

            grphCtl.Graph.Clear();

            yWorks.yFiles.UI.Model.Table tbl = new yWorks.yFiles.UI.Model.Table();

            tbl.CreateColumn(300);

            var groupedGraph = grphCtl.Graph.GetGroupedGraph();
            // create the actual table node at the root level
            groupNode = groupedGraph.CreateGroupNode(groupedGraph.Hierarchy.Root, new RectD(new PointD(10, 10), tbl.Layout.GetSize()),
                                                         new TableNodeStyle(tbl)
                                                         {
                                                             TableRenderingOrder = TableRenderingOrder.RowsFirst,
                                                             BackgroundStyle = new ShapeNodeStyle() { Brush = graphBackground }
                                                         });

            PublicMethods.DisPrs_3015(context, grphCtl, groupNode, disPrsVM.SelectedPrs);

            ClearAllPropertiesOfPrs(disPrsVM.SelectedPrs);

            disPrsVM.CurrentDgm = new DisPrsViewModel.DgmOfPrs();

            disPrsVM.CurrentDgm.GrphCtl = grphCtl;

            if (grphCtl.Graph.Nodes.Count == 1)
            {
                grphCtl.Graph.Clear();
            }

            else
            {
                layout1(groupNode);
            }
        }



        #region ' Print '

        #region private fields

        // Optionhandler for print options
        private OptionHandler handler;


        // region that gets printed
        private yWorks.Canvas.Geometry.Rectangle exportRect;
        // printable representation of the graph control
        private CanvasPrintDocument printDocument;
        // dialog that holds printer and page settings used for printing
        private PrintDialog printDialog;
        private ShapePaintable visualExportRect;

        #endregion

        #region Constructors

        private void OnLoaded()
        {
            InitializeInputModes();
            InitializeGraph();

            SetupOptions();
            InitializePrinting();
        }

        #endregion

        #region Properties

        private OptionHandler Handler
        {
            get { return handler; }
        }

        #endregion

        #region Initialization

        private void InitializeInputModes()
        {
            // Create a GraphEditorInputMode instance
            var editMode = new GraphEditorInputMode();

            // and install the edit mode into the canvas.
            //grphCtl.InputMode = editMode;

            // create the model for the export rectangle 
            exportRect = new yWorks.Canvas.Geometry.Rectangle(0, 0, 100, 100);
            // visualize it
            visualExportRect = ShapePaintable.CreateViewRectangle(exportRect);
            visualExportRect.Pen = new Pen(HatchBrushes.Hatch50, 5);
            grphCtl.Add(visualExportRect, grphCtl.BackgroundGroup);

            AddExportRectInputModes(editMode);
        }

        /// <summary>
        /// Adds the view modes that handle the resizing and movement of the export rectangle.
        /// </summary>
        /// <param name="inputMode"></param>
        private void AddExportRectInputModes(MultiplexingInputMode inputMode)
        {
            // create handles for interactivel resizing the export rectangle
            var rectangleHandles = new ReshapeableHandles(exportRect) { MinimumSize = new SizeD(1, 1) };

            // create a mode that deals with the handles
            var exportHandleInputMode = new HandleInputMode();

            // add it to the graph editor mode
            inputMode.AddConcurrent(exportHandleInputMode, 1);

            // now the handles
            var inputModeContext = SimpleInputModeContext.Create(exportHandleInputMode);
            exportHandleInputMode.Handles = new DefaultCollectionModel<IHandle>
                                        {
                                          rectangleHandles.GetHandle(inputModeContext, HandlePositions.NorthEast),
                                          rectangleHandles.GetHandle(inputModeContext, HandlePositions.NorthWest),
                                          rectangleHandles.GetHandle(inputModeContext, HandlePositions.SouthEast),
                                          rectangleHandles.GetHandle(inputModeContext, HandlePositions.SouthWest),
                                        };

            // create a mode that allows for dragging the export rectangle at the sides
            var moveInputMode = new MoveInputMode
                                  {
                                      Movable = exportRect,
                                      HitTestable = HitTestable.Create(
                                        (location, context) => visualExportRect.IsHit(location, context)
                                                               &&
                                                               !exportRect.ToRectD().GetEnlarged(-2 * context.HitTestRadius).
                                                                  Contains(location))
                                  };

            // add it to the edit mode
            inputMode.AddConcurrent(moveInputMode, 41);
        }

        private void InitializeGraph()
        {

            // initially set the export rect to enclose part of the graph's contents
            exportRect.Set(grphCtl.ContentRect);

            //IGraph graph = grphCtl.Graph;
            //// initialize defaults
            //graph.NodeDefaults.Style = new ShinyPlateNodeStyle(Colors.DarkOrange);
            //graph.EdgeDefaults.Style = new PolylineEdgeStyle { TargetArrow = DefaultArrow.Default };

            //// create sample graph
            //graph.AddLabel(graph.CreateNode(new PointD(30, 30)), "Node");
            //INode node = graph.CreateNode(new PointD(90, 30));
            //graph.CreateEdge(node, graph.CreateNode(new PointD(90, 90)));

            //grphCtl.FitGraphBounds();

            //graph.CreateEdge(node, graph.CreateNode(new PointD(200, 30)));

            //grphCtl.FitGraphBounds();

        }

        private void InitializePrinting()
        {
            printDialog = new PrintDialog() { PageRangeSelection = PageRangeSelection.AllPages, UserPageRangeEnabled = false };

            // create new canvas print document
            printDocument = new CanvasPrintDocument() { AddOverlay = true };
        }

        #endregion

        #region export option handler

        private void SetupOptions()
        {
            // create the options
            SetupHandler();
            // create the control to visualize them
            AddEditorControlToForm();
        }

        private void AddEditorControlToForm()
        {
            // add a new editor control for the option handler
            //editorControl.OptionHandler = Handler;
        }


        /// <summary>
        /// Initializes the option handler for the export
        /// </summary>
        private void SetupHandler()
        {
            handler = new OptionHandler(PRINTING);

            OptionGroup currentGroup = handler.AddGroup(OUTPUT);
            currentGroup.AddBool(HIDE_DECORATIONS, true);
            currentGroup.AddBool(PRINT_RECTANGLE, true);

            currentGroup = handler.AddGroup(DOCUMENT_SETTINGS);

            var item = currentGroup.AddDouble(SCALE, 1.0);
            currentGroup.AddBool(CENTER_CONTENT, false);
            currentGroup.AddBool(PAGE_MARK_PRINTING, false);
            currentGroup.AddBool(SCALE_DOWN_TO_FIT_PAGE, false);
            currentGroup.AddBool(SCALE_UP_TO_FIT_PAGE, false);

            // localization
            //var rm =
            //  new ResourceManager("Demo.yFiles.Printing.Printing",
            //                      Assembly.GetExecutingAssembly());
            //var rmf = new ResourceManagerI18NFactory();
            //rmf.AddResourceManager(Handler.Name, rm);
            //Handler.I18nFactory = rmf;
        }

        #endregion

        #region eventhandlers

        private void printPreviewButton_Click(object sender, EventArgs e)
        {
            PreparePrinting();

            // show new PrintPreviewDialog
            printDocument.Print(printDialog, true, true, InsetsD.Empty, PrintDialogTitle);
        }

        private void PreparePrinting()
        {
            GraphControl control = grphCtl;
            // check if the rectangular region or the whole viewport should be printed
            bool useRect = (bool)handler.GetValue(OUTPUT, PRINT_RECTANGLE);
            //RectD bounds = useRect ? exportRect.ToRectD() : grphCtl.ContentRect;
            RectD bounds = grphCtl.ContentRect;
            // check whether decorations (selection, handles, ...) should be hidden
            bool hide = (bool)handler.GetValue(OUTPUT, HIDE_DECORATIONS);
            if (hide)
            {
                // if so, create a new grphCtl whith the same graph
                control = new GraphControl { Graph = grphCtl.Graph, FlowDirection = grphCtl.FlowDirection };
            }

            // read CanvasPrintDocument options
            printDocument.Scale = (double)Handler.GetValue(DOCUMENT_SETTINGS, SCALE);
            printDocument.CenterContent = (bool)Handler.GetValue(DOCUMENT_SETTINGS, CENTER_CONTENT);
            printDocument.PageMarkPrinting = (bool)Handler.GetValue(DOCUMENT_SETTINGS, PAGE_MARK_PRINTING);
            printDocument.ScaleDownToFitPage = (bool)Handler.GetValue(DOCUMENT_SETTINGS, SCALE_DOWN_TO_FIT_PAGE);
            printDocument.ScaleUpToFitPage = (bool)Handler.GetValue(DOCUMENT_SETTINGS, SCALE_UP_TO_FIT_PAGE);
            // set grphCtl
            printDocument.Canvas = control;
            // set print area
            printDocument.PrintRectangle = bounds;
        }

        private void printerSetupButton_Click(object sender, EventArgs e)
        {
            // show new PrintDialog
            var showDialog = printDialog.ShowDialog();
            if (showDialog.HasValue && showDialog.Value)
            {
                PreparePrinting();
                printDocument.Print(printDialog, true, false, InsetsD.Empty, PrintDialogTitle);
            }
        }

        private void pageSetupButton_Click(object sender, EventArgs e)
        {            
            //This is a optionhandler
            OptionHandler optionHandler = new OptionHandler("PageSettings");

            //We use only a subset of the available settings, since we want to filter some values 
            //("Unknown" can be returned by some properties, but may not be set on a PrintTicket)
            var selectionProvider = new DefaultSelectionProvider<PrintTicketOptionsHelper>(new[] { new PrintTicketOptionsHelper(printDialog.PrintTicket) });
            selectionProvider.ContextLookup = Lookups.CreateContextLookupChainLink(OptionHandlerContextLookup);
            selectionProvider.UpdatePropertyViewsNow();
            //We populate the OptionHandler
            optionHandler.BuildFromSelection(selectionProvider, Lookups.CreateContextLookupChainLink(OptionHandlerContextLookup));

            //EditorForm form = new EditorForm() { OptionHandler = optionHandler, IsAutoAdopt = true, IsAutoCommit = false, Title = "Page Setup" };
            //form.ShowDialog();
        }

        private object OptionHandlerContextLookup(object subject, Type type)
        {
            if (type == typeof(IOptionBuilder) && (subject is PrintTicketOptionsHelper || subject is PrintTicket))
            {
                return new AttributeBasedOptionBuilder();
            }
            if (type == typeof(IPropertyMapBuilder) && (subject is PrintTicketOptionsHelper || subject is PrintTicket))
            {
                return new AttributeBasedPropertyMapBuilderAttribute().CreateBuilder(subject.GetType());
            }
            return null;
        }

        /// <summary>
        /// Helper class that serves as proxy for the most important settings of a print ticket
        /// </summary>
        public class PrintTicketOptionsHelper
        {
            private readonly PrintTicket ticket;
            public PrintTicketOptionsHelper(PrintTicket ticket)
            {
                this.ticket = ticket;
            }

            public int? CopyCount
            {
                get { return ticket.CopyCount; }
                set { ticket.CopyCount = value; }
            }

            [OptionItemAttribute(Name = OptionItem.CustomDialogitemEditor, Value = "ConverterBased.OptionItemPresenter")]
            [OptionItemAttribute(Name = OptionItem.CustomValueConverterAttribute, Value = typeof(PageMediaSizeConverter))]
            public PageMediaSize PageMediaSize
            {
                get { return ticket.PageMediaSize; }
                set { ticket.PageMediaSize = value; }
            }

            public class PageMediaSizeConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    var sz = (PageMediaSize)value;
                    var pageMediaSizeName = sz.PageMediaSizeName;
                    if (pageMediaSizeName.HasValue)
                    {
                        return pageMediaSizeName.Value;
                    }
                    return sz.Height.GetValueOrDefault().ToString(CultureInfo.InvariantCulture) + "x" +
                           sz.Width.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    string strValue = value as string;
                    PageMediaSize retval = null;
                    if (strValue != null)
                    {
                        try
                        {
                            var pageMediaSizeName = (PageMediaSizeName)Enum.Parse(typeof(PageMediaSizeName), strValue);
                            retval = new PageMediaSize(pageMediaSizeName);
                        }
                        catch (Exception) { }
                        if (retval == null)
                        {
                            var tokens = strValue.Split('x');
                            if (tokens.Length == 2)
                            {
                                try
                                {
                                    double height = Double.Parse(tokens[0], CultureInfo.InvariantCulture);
                                    double width = Double.Parse(tokens[1], CultureInfo.InvariantCulture);
                                    retval = new PageMediaSize(width, height);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                    return retval ?? DependencyProperty.UnsetValue;
                }
            }

            public MyPageOrder PageOrder
            {
                get
                {
                    var pageOrder = ticket.PageOrder;
                    if (pageOrder.HasValue && pageOrder != System.Printing.PageOrder.Unknown)
                    {
                        return (MyPageOrder)pageOrder;
                    }
                    return MyPageOrder.Unspecified;
                }
                set
                {
                    if (value == MyPageOrder.Unspecified)
                    {
                        ticket.PageOrder = null;
                    }
                    else
                    {
                        ticket.PageOrder = (PageOrder)value;
                    }
                }
            }

            public MyPageOrientation PageOrientation
            {
                get
                {
                    var pageOrientation = ticket.PageOrientation;
                    if (pageOrientation.HasValue && pageOrientation != System.Printing.PageOrientation.Unknown)
                    {
                        return (MyPageOrientation)pageOrientation;
                    }
                    return MyPageOrientation.Unspecified;
                }
                set
                {
                    if (value == MyPageOrientation.Unspecified)
                    {
                        ticket.PageOrientation = null;
                    }
                    else
                    {
                        ticket.PageOrientation = (PageOrientation)value;
                    }
                }
            }

            public int? PagesPerSheet
            {
                get { return ticket.PagesPerSheet; }
                set { ticket.PagesPerSheet = value; }
            }

            /// <summary>
            /// Helper enum that filters out the "Unknown" value which may not be set interactively, and filters the NullableValues from the original enum...
            /// </summary>
            public enum MyPageOrientation
            {
                Unspecified,
                Landscape = System.Printing.PageOrientation.Landscape,
                Portrait = System.Printing.PageOrientation.Portrait,
                ReverseLandscape = System.Printing.PageOrientation.ReverseLandscape,
                ReversePortrait = System.Printing.PageOrientation.ReversePortrait
            }

            /// <summary>
            /// Helper enum that filters out the "Unknown" value which may not be set interactively, and filters the NullableValues from the original enum...
            /// </summary>
            public enum MyPageOrder
            {
                Unspecified,
                Standard = System.Printing.PageOrder.Standard,
                Reverse = System.Printing.PageOrder.Reverse
            }
        }

        #endregion

        #region static members

        private const string PRINTING = "PRINTING";

        private const string OUTPUT = "OUTPUT";
        private const string HIDE_DECORATIONS = "HIDE_DECORATIONS";
        private const string PRINT_RECTANGLE = "PRINT_RECTANGLE";

        private const string DOCUMENT_SETTINGS = "DOCUMENT_SETTINGS";
        private const string SCALE = "SCALE";
        private const string CENTER_CONTENT = "CENTER_CONTENT";
        private const string PAGE_MARK_PRINTING = "PAGE_MARK_PRINTING";
        private const string SCALE_DOWN_TO_FIT_PAGE = "SCALE_DOWN_TO_FIT_PAGE";
        private const string SCALE_UP_TO_FIT_PAGE = "SCALE_UP_TO_FIT_PAGE";
        private const string PrintDialogTitle = "Printing yFiles Example";

        #endregion

        /// <summary>
        /// Helper method that allows for reusing this window in other applications.
        /// </summary>
        /// <param name="grphCtl">The graph control.</param>
        public void ShowGraph(GraphControl grphCtl)
        {
            this.grphCtl.Graph = grphCtl.Graph;
            this.grphCtl.Selection.Clear(); // or possibly: = grphCtl.Selection;
            this.grphCtl.ContentRect = grphCtl.ContentRect.GetEnlarged(20);

            // show all of the contents
            this.grphCtl.FitContent();

            // or possibly the same viewport
            //      this.grphCtl.Zoom = grphCtl.Zoom;
            //      this.grphCtl.ViewPoint = grphCtl.ViewPoint;

            var inputMode = new MultiplexingInputMode();

            // set the whole content rect as the export rectangle
            exportRect.Set(grphCtl.ContentRect);
            // or possibly just the visible viewport
            //exportRect.Set(grphCtl.Viewport);

            AddExportRectInputModes(inputMode);
            this.grphCtl.InputMode = inputMode;
        }


        #endregion

        
    }


}
