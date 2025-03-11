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
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using yWorks.Canvas;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry.Structs;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  public class DefaultFlowSourceArrowStyle : IArrow, IVisualCreator, IBoundsProvider
  {
    // these variables hold the state for the flyweight pattern
    // they are populated in GetPaintable and used in the implementations of the IVisualCreator interface.
    private PointD anchor;
    private PointD direction;
    private PathFigure arrowFigure;

    #region Constructor

    #endregion

    #region Properties

    /// <summary>
    /// Returns the length of the arrow, i.e. the distance from the arrow's tip to
    /// the position where the visual representation of the edge's path should begin.
    /// </summary>
    /// <value>Always returns 0</value>
    public double Length {
      get { return 0; }
    }

    /// <summary>
    /// Gets the cropping length associated with this instance.
    /// </summary>
    /// <value>Always returns 1</value>
    /// <remarks>
    /// This value is used by <see cref="IEdgeStyle"/>s to let the
    /// edge appear to end shortly before its actual target.
    /// </remarks>
    public double CropLength {
      get { return 1; }
    }

    #endregion

    #region IArrow Members

    /// <summary>
    /// Gets an <see cref="IVisualCreator"/> implementation that will create
    /// the <see cref="FrameworkElement"/> for this arrow
    /// at the given location using the given direction for the given edge.
    /// </summary>
    /// <param name="edge">the edge this arrow belongs to</param>
    /// <param name="atSource">whether this will be the source arrow</param>
    /// <param name="anchor">the anchor point for the tip of the arrow</param>
    /// <param name="direction">the direction the arrow is pointing in</param>
    /// <returns>
    /// Itself as a flyweight.
    /// </returns>
    public IVisualCreator GetPaintable(IEdge edge, bool atSource, PointD anchor, PointD direction) {
      this.anchor = anchor;
      this.direction = direction;
      return this;
    }

    /// <summary>
    /// Gets an <see cref="IBoundsProvider"/> implementation that can yield
    /// this arrow's bounds if painted at the given location using the
    /// given direction for the given edge.
    /// </summary>
    /// <param name="edge">the edge this arrow belongs to</param>
    /// <param name="atSource">whether this will be the source arrow</param>
    /// <param name="anchor">the anchor point for the tip of the arrow</param>
    /// <param name="direction">the direction the arrow is pointing in</param>
    /// <returns>
    /// an implementation of the <see cref="IBoundsProvider"/> interface that can
    /// subsequently be used to query the bounds. Clients will always call
    /// this method before using the implementation and may not cache the instance returned.
    /// This allows for applying the flyweight design pattern to implementations.
    /// </returns>
    public IBoundsProvider GetBoundsProvider(IEdge edge, bool atSource, PointD anchor, PointD direction) {
      this.anchor = anchor;
      this.direction = direction;
      return this;
    }

    #endregion

    #region Rendering

    /// <summary>
    /// This method is called by the framework to create a <see cref="FrameworkElement"/>
    /// that will be included into the <see cref="IRenderContext"/>.
    /// </summary>
    /// <param name="ctx">The context that describes where the visual will be used.</param>
    /// <returns>
    /// The arrow visual to include in the canvas object visual tree./>.
    /// </returns>
    /// <seealso cref="UpdateVisual"/>
    Visual IVisualCreator.CreateVisual(IRenderContext ctx) {

      // Create a new Path to draw the arrow
      if (arrowFigure == null) {
        arrowFigure = new PathFigure
        {
          IsClosed = false,
          StartPoint = new Point(-10, -5),
          Segments =
                            {
                              new LineSegment {Point = new Point(0, 5), IsSmoothJoin = true}
                            }
        };
        arrowFigure.Freeze();
      }

      Path p = new Path
                 {
                   // set Stretch, MinWidth and MinHeight so Path gets drawn in negative coordinate range
                   Stretch = Stretch.None,
                   MinWidth = 1,
                   MinHeight = 1,
                   Stroke = Brushes.Black,
                   StrokeThickness = 1,
                   // Draw arrow outline
                   Data = new PathGeometry
                            {
                              Figures = {arrowFigure}
                            },
                   RenderTransform = new MatrixTransform
                                       {
                                         Matrix =
                                           new Matrix(direction.X, direction.Y, -direction.Y, direction.X, anchor.X,
                                                      anchor.Y)
                                       }
                 };

      // Rotate arrow and move it to correct position
      return p;
    }


    /// <summary>
    /// This method updates or replaces a previously created <see cref="FrameworkElement"/> for inclusion
    /// in the <see cref="IRenderContext"/>.
    /// </summary>
    /// <param name="ctx">The context that describes where the visual will be used in.</param>
    /// <param name="oldVisual">The visual instance that had been returned the last time the <see cref="IVisualCreator.CreateVisual"/>
    /// method was called on this instance.</param>
    /// <returns>
    /// 	<paramref name="oldVisual"/>, if this instance modified the visual, or a new visual that should replace the
    /// existing one in the canvas object visual tree.
    /// </returns>
    /// <remarks>
    /// The <see cref="CanvasControl"/> uses this method to give implementations a chance to
    /// update an existing Visual that has previously been created by the same instance during a call
    /// to <see cref="IVisualCreator.CreateVisual"/>. Implementation may update the <paramref name="oldVisual"/>
    /// and return that same reference, or create a new visual and return the new instance or <see langword="null"/>.
    /// </remarks>
    /// <seealso cref="IVisualCreator.CreateVisual"/>
    /// <seealso cref="ICanvasObjectDescriptor"/>
    /// <seealso cref="CanvasControl"/>
    public Visual UpdateVisual(IRenderContext ctx, Visual oldVisual) {
      Path p = oldVisual as Path;
      if (p != null) {
        ((MatrixTransform)p.RenderTransform).Matrix = new Matrix(direction.X, direction.Y, -direction.Y, direction.X,
                                                                  anchor.X, anchor.Y);
        return p;
      }
      return ((IVisualCreator)this).CreateVisual(ctx);
    }

    #endregion

    #region Rendering Helper Methods

    /// <summary>
    /// Returns the bounds of the arrow for the current flyweight configuration.
    /// </summary>
    RectD IBoundsProvider.GetBounds(ICanvasContext ctx) {

      return new RectD(anchor.X - 10, anchor.Y - 10, 10, 10);
    }

    #endregion
  }
}
