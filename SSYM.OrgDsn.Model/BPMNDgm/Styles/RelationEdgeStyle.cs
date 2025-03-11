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
using System;
using System.Windows.Media;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Model;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  /// <summary>
  /// Concrete implementation of <see cref="IPolylineEdgeStyle"/> that respects BPMN <see cref="Relation"/>s.
  /// </summary>
  /// <remarks>
  /// This style will change the source and target arrows as well as the dash style based on the type
  /// of the relation to reflect the BPDM specification.
  /// </remarks>
  public class RelationEdgeStyle : IPolylineEdgeStyle
  {
    /// <summary>
    /// Backing field for the <see cref="Relation"/> property.
    /// </summary>
    private Relation relation;

    /// <summary>
    /// Gets or sets the relation.
    /// </summary>
    /// <value>
    /// The relation.
    /// </value>
    public Relation Relation {
      get { return relation; }
      set {
        if(relation != value) {
          // modify arrows and pen depending on the specified relation
          switch (value.Type) {
            default:
            case RelationType.SequenceFlow:
              pen = Pens.Black;
              sourceArrow = DefaultArrow.None;
              targetArrow = DefaultArrow.Triangle;
              break;
            case RelationType.DefaultFlow:
              pen = Pens.Black;
              sourceArrow = new DefaultFlowSourceArrowStyle();
              targetArrow = DefaultArrow.Triangle;
              break;
            case RelationType.ConditionalFlow:
              pen = Pens.Black;
              // diamond arrow without fill
              sourceArrow = DefaultArrow.Create(ArrowType.Diamond, Pens.Black, null, 1);
              targetArrow = DefaultArrow.Triangle;
              break;
            case RelationType.Association:
              pen = new Pen {Brush = Brushes.Black, DashStyle = DashStyles.Dot};
              sourceArrow = DefaultArrow.None;
              targetArrow = DefaultArrow.None;
              break;
            case RelationType.DirectedAssociation:
              pen = new Pen {Brush = Brushes.Black, DashStyle = DashStyles.Dot};
              sourceArrow = DefaultArrow.None;
              targetArrow = DefaultArrow.Simple;
              break;
            case RelationType.BiDirectedAssociation:
              pen = new Pen {Brush = Brushes.Black, DashStyle = DashStyles.Dot};
              sourceArrow = DefaultArrow.Simple;
              targetArrow = DefaultArrow.Simple;
              break;
            case RelationType.MessageFlow:
              pen = new Pen {Brush = Brushes.Black, DashStyle = DashStyles.Dash};
              sourceArrow = DefaultArrow.Create(ArrowType.Circle, Pens.Black, null, 1); // no fill
              targetArrow = DefaultArrow.Create(ArrowType.Triangle, Pens.Black, null, 1); // no fill
              break;
            case RelationType.NonDirectedMessageFlow:
              pen = new Pen {Brush = Brushes.Black, DashStyle = DashStyles.Dash};
              sourceArrow = DefaultArrow.Create(ArrowType.Circle, Pens.Black, null, 1); // no fill
              targetArrow = DefaultArrow.Create(ArrowType.Circle, Pens.Black, null, 1); // no fill
              break;
          }
          relation = value;
        }
      }
    }

    #region implementation of IPolylineEdgeStyle interface
    private readonly PolylineEdgeStyleRenderer polylineEdgeStyleRenderer = new PolylineEdgeStyleRenderer();

    public void Install(IInstallerContext context, IEdge item) {
      polylineEdgeStyleRenderer.Install(context, item);
    }

    public object Clone() {
      var clone = (RelationEdgeStyle)base.MemberwiseClone();
      if (sourceArrow is ICloneable) {
        clone.sourceArrow = (IArrow)((ICloneable)sourceArrow).Clone();
      }
      if (targetArrow is ICloneable) {
        clone.targetArrow = (IArrow)((ICloneable)targetArrow).Clone();
      }
      return clone;
    }

    public IEdgeStyleRenderer Renderer {
      get { return polylineEdgeStyleRenderer; }
    }

    private IArrow sourceArrow;
    public IArrow SourceArrow {
      get { return sourceArrow; }
    }

    private IArrow targetArrow;
    public IArrow TargetArrow {
      get { return targetArrow; }
    }

    private Pen pen;
    public Pen Pen {
      get { return pen; }
    }

    public double Smoothing {
      get { return 15; }
    }

    #endregion
  }
}
