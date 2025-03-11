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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using yWorks.Canvas;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Geometry;
using yWorks.Support.Extensions;
using yWorks.Support.Windows;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  /// <summary>
  /// Custom stripe style that alternates the visualizations for the leaf nodes and uses a different style for all parent stripes.
  /// </summary>
  public class AlternatingLeafStripeStyle : SimpleAbstractNodeStyle<CanvasContainer>
  {
    /// <summary>
    /// Visualization for all leaf stripes that have an even index
    /// </summary>
    public StripeDescriptor EvenLeafDescriptor { get; set; }

    /// <summary>
    /// Visualization for all stripes that are not leafs
    /// </summary>
    public StripeDescriptor ParentDescriptor { get; set; }

    /// <summary>
    /// Visualization for all leaf stripes that have an odd index
    /// </summary>
    public StripeDescriptor OddLeafDescriptor { get; set; }

    protected override CanvasContainer CreateVisual(INode node, IRenderContext ctx) {
      var stripe = node.Get<IStripe>();
      IRectangle layout = node.Layout;
      if (stripe != null) {
        var cc = new CanvasContainer() { AllowDrop = true};
        Thickness stripeInsets;

        StripeDescriptor descriptor;
        //Depending on the stripe type, we need to consider horizontal or vertical insets
        if (stripe is IColumn) {
          var col = (IColumn)stripe;
          stripeInsets = new Thickness(0, col.GetActualInsets().Top, 0, col.GetActualInsets().Bottom);
        } else {
          var row = (IRow)stripe;
          stripeInsets = new Thickness(row.GetActualInsets().Left, 0, row.GetActualInsets().Right, 0);
        }

        Thickness actualBorderThickness;

        if (stripe.GetChildren().Count() > 0) {
          //Parent stripe - use the parent descriptor
          descriptor = ParentDescriptor;
          actualBorderThickness = descriptor.BorderThickness;
        } else {
          int index;
          if (stripe is IColumn) {
            var col = (IColumn) stripe;
            //Get all leaf columns
            var leafs = col.Table.RootColumn.GetLeaves().ToList();
            //Determine the index
            index = leafs.FindIndex(curr => col == curr);
            //Use the correct descriptor
            descriptor = index % 2 == 0 ? EvenLeafDescriptor : OddLeafDescriptor;
            actualBorderThickness = descriptor.BorderThickness;
          } else {
            var row = (IRow) stripe;
            var leafs = row.Table.RootRow.GetLeaves().ToList();
            index = leafs.FindIndex((curr) => row == curr);
            descriptor = index % 2 == 0 ? EvenLeafDescriptor : OddLeafDescriptor;
            actualBorderThickness = descriptor.BorderThickness;
          }
        }

        {
          cc.Add(new Border
                   {
                     Background = descriptor.BackgroundBrush,
                     BorderBrush = descriptor.InsetBrush,
                     BorderThickness = stripeInsets,
                     Width = layout.Width,
                     Height = layout.Height,
                     AllowDrop = true
                   });
          cc.Add(new Border
                   {
                     Background = Brushes.Transparent,
                     BorderBrush = descriptor.BorderBrush,
                     BorderThickness = actualBorderThickness,
                     Width = layout.Width,
                     Height = layout.Height,
                     AllowDrop = true
                   });
        }
        cc.SetCanvasArrangeRect(layout.ToRectD().ToRect());
        var renderData = CreateRenderDataCache(ctx, descriptor, stripe, stripeInsets);
        cc.SetRenderDataCache(renderData);
        return cc;
      }
      return new CanvasContainer();
    }    
    
    protected override CanvasContainer UpdateVisual(INode node, IRenderContext ctx, CanvasContainer oldVisual) {
      var stripe = node.Get<IStripe>();
      IRectangle layout = node.Layout;
      if (stripe != null) {
        Thickness stripeInsets;
        //Check if values have changed - then update everything
        StripeDescriptor descriptor;
        if (stripe is IColumn) {
          var col = (IColumn)stripe;
          stripeInsets = new Thickness(0, col.GetActualInsets().Top, 0, col.GetActualInsets().Bottom);
        } else {
          var row = (IRow)stripe;
          stripeInsets = new Thickness(row.GetActualInsets().Left, 0, row.GetActualInsets().Right, 0);
        }

        Thickness actualBorderThickness;

        if (stripe.GetChildren().Count() > 0) {
          descriptor = ParentDescriptor;
          actualBorderThickness = descriptor.BorderThickness;
        } else {
          int index;
          if (stripe is IColumn) {
            var col = (IColumn) stripe;
            var leafs = col.Table.RootColumn.GetLeaves().ToList();
            index = leafs.FindIndex((curr) => col == curr);
            descriptor = index % 2 == 0 ? EvenLeafDescriptor : OddLeafDescriptor;
            actualBorderThickness = descriptor.BorderThickness;
          } else {
            var row = (IRow) stripe;
            var leafs = row.Table.RootRow.GetLeaves().ToList();
            index = leafs.FindIndex((curr) => row == curr);
            descriptor = index % 2 == 0 ? EvenLeafDescriptor : OddLeafDescriptor;
            actualBorderThickness = descriptor.BorderThickness;
          }
        }

        // get the data with wich the oldvisual was created
        var oldCache = oldVisual.GetRenderDataCache<RenderDataCache>();
        // get the data for the new visual
        RenderDataCache newCache = CreateRenderDataCache(ctx, descriptor, stripe, stripeInsets);

        // check if something changed except for the location of the node
        if (!newCache.Equals(oldCache)) {
          // something changed - just re-render the visual
          return CreateVisual(node, ctx);
        }

        Border borderVisual = (Border) oldVisual.Children[0];
        borderVisual.Width = layout.Width;
        borderVisual.Height = layout.Height;
        borderVisual.BorderThickness = stripeInsets;

        Border stripeVisual = (Border)oldVisual.Children[1];
        stripeVisual.Width = layout.Width;
        stripeVisual.Height = layout.Height;
        stripeVisual.BorderThickness = actualBorderThickness;
        oldVisual.SetCanvasArrangeRect(layout.ToRectD().ToRect());
        return oldVisual;
      }
      return new CanvasContainer();
    }

#pragma warning disable 659
    /// <summary>
    /// Helper class to cache rendering related data
    /// </summary>
    private struct RenderDataCache
    {
      public StripeDescriptor Descriptor { get; set; }

      public Thickness Insets { get; set; }

      private IStripe stripe;

      public IStripe Stripe {
        get { return stripe; }
        set { stripe = value; }
      }

      public bool Equals(RenderDataCache other) {
        return other.Descriptor == Descriptor && other.Insets == Insets && other.Stripe == Stripe;
      }


      public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
          return false;
        }
        if (obj.GetType() != typeof(RenderDataCache)) {
          return false;
        }
        return Equals((RenderDataCache)obj);
      }
    }
#pragma warning restore 659

    private static RenderDataCache CreateRenderDataCache(IRenderContext renderContext, StripeDescriptor descriptor, IStripe stripe, Thickness insets) {
      return new RenderDataCache { Descriptor = descriptor, Stripe = stripe, Insets = insets };
    }
  }
}