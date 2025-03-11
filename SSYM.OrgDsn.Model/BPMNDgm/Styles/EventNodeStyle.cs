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
using yWorks.Canvas.Geometry;
using yWorks.Canvas.Geometry.Structs;
using yWorks.Canvas.Input;
using yWorks.Support.Extensions;
using yWorks.yFiles.Layout;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Model;
using yWorks.yFiles.UI.PortLocationModels;
using SSYM.OrgDsn.Model.BPMNDgm.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  /// <summary>
  /// Concrete implementation of <see cref="BPDMNodeStyleBase"/> for event nodes.
  /// </summary>
  public class EventNodeStyle : BPDMNodeStyleBase, IStyle
  {
    public Event Event { get; set; }

    protected override object ResourceKey {
      get { return Event.Type; }
    }

    protected override Element Element {
      get { return Event; }
    }

    public override void CreateDefaultLabelConfiguration(ILabeledItem labeledItem, out ILabelModelParameter param,
                                                           out ILabelStyle style, out SizeD? preferredSize) {
      // the label should always be placed at the outside of the node
      var model = new ExteriorLabelModel { Insets = new InsetsD(10) };
      param = model.CreateParameter(ExteriorLabelModel.Position.South);
      style = new SimpleLabelStyle
      {
        TypefaceFormat = { TextAlignment = TextAlignment.Center }
      };
      preferredSize = null;
    }

    protected override object Lookup(INode node, Type type) {
      // turn off resizing of Event nodes
      if (type == typeof(IReshapeHandleProvider)) {
        return new ReshapeableHandles(null) {HandlePositions = HandlePositions.None};
      }
      return base.Lookup(node, type);
    }

    protected override GeneralPath GetOutline(INode node) {
      var path = new GeneralPath();
      var layout = node.Layout;
      path.AppendEllipse(ImmutableRectangle.Create(layout.GetTopLeft(), layout.GetSize()), false);
      return path;
    }

    protected override IEnumerable<IPortLocationModelParameter> GetPortCandidateParameters() {
      // return positions relative to the center of the node
      return new[]
               {
                  NodeScaledPortLocationModel.Instance.CreateScaledParameter(new PointD(0, -0.49)), // top
                  NodeScaledPortLocationModel.Instance.CreateScaledParameter(new PointD(-0.49, 0)), // left
                  NodeScaledPortLocationModel.Instance.CreateScaledParameter(new PointD(0, 0.49)),  // bottom
                  NodeScaledPortLocationModel.Instance.CreateScaledParameter(new PointD(0.49, 0))   // right
               };
    }

    public override PortCandidateSet GetPortCandidateSet(INode node) {
      var ports = new PortCandidateSet();
      // return positions relative to the center of the node
      ports.Add(CreateCandidate(new PointD(0, -0.49), node, PortDirection.West), int.MaxValue);  // top
      ports.Add(CreateCandidate(new PointD(-0.49, 0), node, PortDirection.North), int.MaxValue); // left
      ports.Add(CreateCandidate(new PointD(0, 0.49), node, PortDirection.East), int.MaxValue);   // bottom
      ports.Add(CreateCandidate(new PointD(0.49, 0), node, PortDirection.South), int.MaxValue);  // right
      return ports;
    }

    public IRow Col
    {
        get
        {
            return this.Event.Col;
        }
        set
        {
            this.Event.Col = value;
        }
    }
  }
}