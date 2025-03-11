/****************************************************************************
 **
 ** This file is part of yFiles WPF 2.4.
 ** 
 ** yWorks proprietary/confidential. Use is subject to license terms.
 **
 ** Redistribution of this file or of an unauthorized byte-code version
 ** of this file is strictly forbidden.
 **
 ** Copyright (c) 2003-2013 by yWorks GmbH, Vor dem Kreuzberg 28, 
 ** 72070 Tuebingen, Germany. All rights reserved.
 **
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry.Structs;
using yWorks.Canvas.Input;
using yWorks.Support;
using yWorks.Support.Annotations;
using yWorks.Support.Extensions;
using yWorks.Support.Windows;
using yWorks.yFiles.Layout;
using yWorks.yFiles.Layout.Hierarchic;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Input;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Model;
using System.Linq;
using yWorks.yFiles.UI.PortLocationModels;
using SSYM.OrgDsn.Model.BPMNDgm.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
    /// <summary>
    /// An abstract NodeStyle for BPDM nodes.
    /// </summary>
    /// <remarks>
    /// This style acts as loopup for a <see cref="IPortCandidateProvider"/>
    /// and a BPDM <see cref="Element"/>.
    /// 
    /// It loads an appropriate XAML resource based on the specified element key,
    /// knows how to create a default label configuration and what <see cref="PortCandidate"/>s
    /// are applicable to this node type.
    /// </remarks>
    public abstract class BPDMNodeStyleBase : SimpleAbstractNodeStyle<FrameworkElement>
    {
        /// <summary>
        /// Gets the resource key for the XAML lookup.
        /// </summary>
        protected abstract object ResourceKey { get; }

        /// <summary>
        /// Gets the BPDM element.
        /// </summary>
        protected abstract Element Element { get; }


        /// <summary>
        /// Lookups the specified type for the given node.
        /// </summary>
        /// <remarks>
        /// Can handle BPMN <see cref="Element"/> and <see cref="IPortCandidateProvider"/>
        /// in addition to all lookups provided by its parent hierarchy.
        /// </remarks>
        /// <param name="node">The node.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected override object Lookup(INode node, Type type)
        {
            if (type == typeof(Element))
            {
                return Element;
            }
            if (type == typeof(IPortCandidateProvider))
            {
                return new MyPortCandidateProvider(this, node);
            }
            return base.Lookup(node, type);
        }

        /// <summary>
        /// Creates the default label configuration.
        /// </summary>
        /// <remarks>
        /// The default implementation can be overriden by child classes. It places the label at the center of the node.
        /// </remarks>
        /// <param name="labeledItem">The labeled item.</param>
        /// <param name="param">The param.</param>
        /// <param name="style">The style.</param>
        /// <param name="preferredSize">Size of the preferred.</param>
        public virtual void CreateDefaultLabelConfiguration(ILabeledItem labeledItem, out ILabelModelParameter param,
                                                               out ILabelStyle style, out SizeD? preferredSize)
        {
            var model = new InteriorLabelModel { Insets = new InsetsD(10, 10, 10, 10) };
            param = model.CreateParameter(InteriorLabelModel.Position.Center);
            style = new SimpleLabelStyle
                      {
                          TypefaceFormat = { TextAlignment = TextAlignment.Center }
                      };
            preferredSize = null;
        }

        #region IPortCandidateProvider

        /// <summary>
        /// A implementation of <see cref="IPortCandidateProvider"/> that filters the port candidates based on the BPMN rules.
        /// </summary>
        /// <remarks>
        /// It will provide either all port candidates that the node style specifies or none if the rules forbid edges between
        /// the specified nodes.
        /// </remarks>
        private sealed class MyPortCandidateProvider : IPortCandidateProvider
        {
            private readonly BPDMNodeStyleBase outer;
            private readonly INode node;

            public MyPortCandidateProvider(BPDMNodeStyleBase outer, INode node)
            {
                this.outer = outer;
                this.node = node;
            }

            public IEnumerable<IPortCandidate> GetSourcePortCandidates(IInputModeContext context)
            {
                var graph = context.GetGraph();
                var edgeStyle = graph.EdgeDefaults.Style as RelationEdgeStyle;
                return GetSourcePortCandidates(context, null, edgeStyle);
            }

            public IEnumerable<IPortCandidate> GetTargetPortCandidates(IInputModeContext context)
            {
                var graph = context.GetGraph();
                var edgeStyle = graph.EdgeDefaults.Style as RelationEdgeStyle;
                return GetTargetPortCandidates(context, null, edgeStyle);
            }

            public IEnumerable<IPortCandidate> GetSourcePortCandidates(IInputModeContext context, IEdge edge)
            {
                var targetNode = edge.GetTargetNode();
                var edgeStyle = edge.Style as RelationEdgeStyle;

                return GetSourcePortCandidates(context, targetNode, edgeStyle);
            }

            public IEnumerable<IPortCandidate> GetTargetPortCandidates(IInputModeContext context, IEdge edge)
            {
                var sourceNode = edge.GetSourceNode();
                var edgeStyle = edge.Style as RelationEdgeStyle;

                return GetTargetPortCandidates(context, sourceNode, edgeStyle);
            }

            public IEnumerable<IPortCandidate> GetTargetPortCandidates(IInputModeContext context, IPortCandidate source)
            {
                var sourceNode = source.Owner as INode;
                var graph = context.GetGraph();
                var edgeStyle = graph.EdgeDefaults.Style as RelationEdgeStyle;

                return GetTargetPortCandidates(context, sourceNode, edgeStyle);
            }

            private IEnumerable<IPortCandidate> GetSourcePortCandidates([NotNull] IInputModeContext context, [CanBeNull] INode targetNode, [NotNull] RelationEdgeStyle edgeStyle)
            {
                var graph = context.GetGraph();

                // forbid self loops and edges from parent to child and child to parent
                var selfloop = node == targetNode;
                if (IsParent(node, targetNode, graph) == false && !selfloop)
                {
                    // check if the nodes are in the same pools
                    bool? inSamePool = targetNode != null ? AreNodesInTheSamePool(node, targetNode, graph) : (bool?)null;
                    var relation = edgeStyle.Relation;
                    var element = outer.Element;
                    var otherElement = targetNode != null ? targetNode.Get<Element>() : null;
                    // if the rules allow edges under the given conditions, return the port candidates
                    if (element.AcceptOutgoingEdge(inSamePool, relation, otherElement))
                    {
                        return GetPortCandidates(context, PortCandidateValidity.Valid);
                    }
                    else
                    {
                        // otherwise return invalid port candidates
                        return GetPortCandidates(context, PortCandidateValidity.Invalid);
                    }
                }
                // otherwise return an empty list (forbid all ports)
                return EmptyList<IPortCandidate>.Instance;
            }

            private IEnumerable<IPortCandidate> GetTargetPortCandidates([NotNull] IInputModeContext context, [CanBeNull] INode sourceNode, [NotNull] RelationEdgeStyle edgeStyle)
            {
                var graph = context.GetGraph();

                // forbid self loops and edges from parent to child and child to parent
                var selfloop = node == sourceNode;
                if (sourceNode != null && IsParent(node, sourceNode, graph) == false && !selfloop)
                {
                    // check if the nodes are in the same pools
                    var inSamePool = AreNodesInTheSamePool(node, sourceNode, graph);
                    var relation = edgeStyle.Relation;
                    var element = outer.Element;
                    var otherElement = sourceNode.Get<Element>();
                    // if the rules allow edges under the given conditions, return the port candidates
                    if (otherElement == null || otherElement.AcceptOutgoingEdge(inSamePool, relation, element))
                    {
                        return GetPortCandidates(context, PortCandidateValidity.Valid);
                    }
                    else
                    {
                        // otherwise return invalid port candidates
                        return GetPortCandidates(context, PortCandidateValidity.Invalid);
                    }
                }
                // otherwise return an empty list (forbid all ports)
                return EmptyList<IPortCandidate>.Instance;
            }

            public IEnumerable<IPortCandidate> GetSourcePortCandidates(IInputModeContext context, IPortCandidate target)
            {
                var targetNode = target.Owner as INode;
                var graph = context.GetGraph();
                var edgeStyle = graph.EdgeDefaults.Style as RelationEdgeStyle;

                return GetSourcePortCandidates(context, targetNode, edgeStyle);
            }

            /// <summary>
            /// Returns the port candidates as specified by the node style.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="portCandidateValidity"></param>
            /// <returns></returns>
            private IEnumerable<IPortCandidate> GetPortCandidates(IInputModeContext context, PortCandidateValidity portCandidateValidity)
            {
                // translate the PortLocationModel to an actual PortCandidate
                return outer.GetPortCandidateParameters().Select(parameter => (IPortCandidate)new DefaultPortCandidate(node, parameter)
                                                                                                {
                                                                                                    Validity = portCandidateValidity
                                                                                                });
            }

            /// <summary>
            /// Determines whether the specified source is a parent of target.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="target">The target.</param>
            /// <param name="graph">The graph.</param>
            /// <returns>
            ///   <c>true</c> if the specified source is a parent of target; otherwise, <c>false</c>.
            /// </returns>
            private bool IsParent(INode source, INode target, IGraph graph)
            {
                // get the grouped graph
                var groupedGraph = graph.GetGroupedGraph();
                if (groupedGraph != null)
                {
                    // check if target is a descendant of source
                    return groupedGraph.Hierarchy.IsDescendant(target, source);
                }
                return false;
            }

            /// <summary>
            /// Checks if the nodes are in the same pool.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <param name="targetNode">The target node.</param>
            /// <param name="graph">The graph.</param>
            /// <returns>
            ///   <c>true</c> if the nodes are in the same pool; otherwise, <c>false</c>.
            /// </returns>
            private bool AreNodesInTheSamePool(INode node, INode targetNode, IGraph graph)
            {
                var groupedGraph = graph.GetGroupedGraph();
                if (groupedGraph != null)
                {
                    var parent = GetParentTableNode(groupedGraph, node);
                    var otherParent = GetParentTableNode(groupedGraph, targetNode);
                    return parent == otherParent;
                }
                return true;
            }

            /// <summary>
            /// Gets the parent table node or <c>null</c> if the node is not within a table.
            /// </summary>
            /// <param name="groupedGraph">The grouped graph.</param>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            [CanBeNull]
            private INode GetParentTableNode(IGroupedGraph groupedGraph, INode node)
            {
                INode parent = node;
                // iterate over the nodes hierarchy
                while (parent != groupedGraph.Hierarchy.Root)
                {
                    parent = groupedGraph.Hierarchy.GetParent(parent);
                    // check if we've found the parent
                    if (parent == null || parent.Get<ITable>() != null) return parent;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the port candidate parameters used by the <see cref="IPortCandidateProvider"/>.
        /// </summary>
        /// <remarks>
        /// The default implementation will return a centered port.
        /// </remarks>
        /// <returns></returns>
        protected virtual IEnumerable<IPortLocationModelParameter> GetPortCandidateParameters()
        {
            return new[] { NodeScaledPortLocationModel.NodeCenterAnchored };
        }

        /// <summary>
        /// Gets the port candidate set used by the <see cref="IncrementalHierarchicLayouter"/>.
        /// </summary>
        /// <remarks>
        /// The default implementation will return a centered port candidate which allows unlimited connections.
        /// </remarks>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual PortCandidateSet GetPortCandidateSet(INode node)
        {
            var ports = new PortCandidateSet();
            ports.Add(PortCandidate.CreateCandidate(PortDirection.Any), int.MaxValue);
            return ports;
        }

        /// <summary>
        /// Creates a <see cref="PortCandidate"/> at the given (relative) offset of the node with the specified direction.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="node">The node.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        protected PortCandidate CreateCandidate(PointD offset, INode node, PortDirection direction)
        {
            RectD layout = node.Layout.ToRectD();
            double x = layout.Width * offset.X;
            double y = layout.Height * offset.Y;
            return PortCandidate.CreateCandidate(x, y, direction);
        }

        #endregion

        #region UI implementation

        protected override FrameworkElement CreateVisual(INode node, IRenderContext renderContext)
        {
            // try to find the data template
            var dataTemplate = renderContext.Canvas.TryFindResource(ResourceKey) as DataTemplate;
            // create the element
            var element = (FrameworkElement)dataTemplate.LoadContent();

            // cache it
            element.SetRenderDataCache(ResourceKey);
            // do the layout
            element.SetCanvasArrangeRect(node.Layout.ToRectD().ToRect());

            // return the element
            return element;

        }

        protected override FrameworkElement UpdateVisual(INode node, IRenderContext renderContext, FrameworkElement oldVisual)
        {
            // update the visual if the type of the element changed
            if (!Equals(ResourceKey, oldVisual.GetRenderDataCache<object>()))
            {
                return CreateVisual(node, renderContext);
            }
            // otherwise just update the layout
            oldVisual.SetCanvasArrangeRect(node.Layout.ToRectD().ToRect());
            return oldVisual;
        }

        #endregion
    }
}