using SSYM.OrgDsn.Model.BPMNDgm.Model;
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
using System.Collections.Generic;
using System.Windows;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry;
using yWorks.Canvas.Geometry.Structs;
using yWorks.Support.Extensions;
using yWorks.yFiles.Layout;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Model;
using yWorks.yFiles.UI.PortLocationModels;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  /// <summary>
  /// This style represents an Artifact node.
  /// </summary>
  public class ArtifactNodeStyle : BPDMNodeStyleBase
  {
    public Artifact Artifact { get; set; }

    protected override object ResourceKey {
      get { return Artifact.Type; }
    }

    protected override Element Element {
      get { return Artifact; }
    }

    protected override IEnumerable<IPortLocationModelParameter> GetPortCandidateParameters() {
      // text annotation types need a different port candidate
        if (Artifact.Type == Enum.TypShp.TextAnnotation)
        {
        // return a port candidate at the left of the node
        return new[] { NodeScaledPortLocationModel.NodeLeftAnchored };
      }
      // otherwise use a centered port candidate
      return new[] { NodeScaledPortLocationModel.NodeCenterAnchored };
    }

    public override PortCandidateSet GetPortCandidateSet(INode node) {
      // text annotation types need a different port candidate
        if (Artifact.Type == Enum.TypShp.TextAnnotation)
        {
        // return a port candidate at the left of the node
        var ports = new PortCandidateSet();
        ports.Add(PortCandidate.CreateCandidate(PortDirection.Any), int.MaxValue);
        return ports;
      }
      // otherwise use a centered port candidate
      return base.GetPortCandidateSet(node);
    }

    public override void CreateDefaultLabelConfiguration(ILabeledItem labeledItem, out ILabelModelParameter param,
                                                           out ILabelStyle style, out SizeD? preferredSize) {
      // for data object types, the label is placed below the node
      if(Artifact.Type == Enum.TypShp.DO1) {
        var model = new ExteriorLabelModel { Insets = new InsetsD(10) };
        param = model.CreateParameter(ExteriorLabelModel.Position.South);
        style = new SimpleLabelStyle
        {
          TypefaceFormat = { TextAlignment = TextAlignment.Center }
        };
        preferredSize = null;
      } else {
        base.CreateDefaultLabelConfiguration(labeledItem, out param, out style, out preferredSize);
      }
    }

    protected override GeneralPath GetOutline(INode node) {
      // get useful variables
      var layout = node.Layout;
      var path = new GeneralPath();
      var w = layout.Width;
      var h = layout.Height;
      var x = layout.X;
      var y = layout.Y;
      // create the outline depending on the type of the node
      switch (Artifact.Type) {
          default:
          case Enum.TypShp.TextAnnotation:
            path.AppendRectangle(ImmutableRectangle.Create(layout.GetTopLeft(), layout.GetSize()), false);
            break;
          case Enum.TypShp.DO1:
            path.MoveTo(x, y);
            path.LineTo(x, y+h);
            path.LineTo(x+w, y+h);
            path.LineTo(x+w, y+15);
            path.LineTo(x+25, y);
            path.LineTo(x, y);
          break;
      }
      return path;
    }
  }
}
