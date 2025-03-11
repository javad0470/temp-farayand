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
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using yWorks.Support.Extensions;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model.BPMNDgm.Styles
{
  /// <summary>
  /// Helper class that can be used as StyleTag to bundle common visualization parameters for stripes
  /// </summary>
  public class StripeDescriptor
  {
    private Brush backgroundBrush = Brushes.Transparent;

    /// <summary>
    /// The background brush for a stripe
    /// </summary>
    [DefaultValue(typeof (Brush), "Transparent")]
    public Brush BackgroundBrush {
      get { return backgroundBrush; }
      set { backgroundBrush = value; }
    }

    private Brush insetBrush = Brushes.Transparent;

    /// <summary>
    /// The inset brush for a stripe
    /// </summary>
    [DefaultValue(typeof (Brush), "Transparent")]
    public Brush InsetBrush {
      get { return insetBrush; }
      set { insetBrush = value; }
    }

    private Brush borderBrush = Brushes.Black;

    /// <summary>
    /// The border brush for a stripe
    /// </summary>
    [DefaultValue(typeof (Brush), "Black")]
    public Brush BorderBrush {
      get { return borderBrush; }
      set { borderBrush = value; }
    }

    private Thickness borderThickness = new Thickness(1);

    /// <summary>
    /// The border thickness for a stripe
    /// </summary>
    [DefaultValue(typeof (Thickness), "1")]
    public Thickness BorderThickness {
      get { return borderThickness; }
      set { borderThickness = value; }
    }

    public static bool operator ==(StripeDescriptor p1, StripeDescriptor p2) {
      return p1.InsetBrush == p2.InsetBrush && p1.BorderBrush == p2.BorderBrush && p1.BackgroundBrush == p2.BackgroundBrush && p1.BorderThickness == p2.BorderThickness;
    }

    public static bool operator !=(StripeDescriptor p1, StripeDescriptor p2) {
      return !(p1 == p2);
    }


    public bool Equals(StripeDescriptor other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return Equals(other.backgroundBrush, backgroundBrush) && Equals(other.insetBrush, insetBrush) && Equals(other.borderBrush, borderBrush) && other.borderThickness.Equals(borderThickness);
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj.GetType() != typeof (StripeDescriptor)) {
        return false;
      }
      return Equals((StripeDescriptor) obj);
    }

    public override int GetHashCode() {
      unchecked {
        int result = (backgroundBrush != null ? backgroundBrush.GetHashCode() : 0);
        result = (result * 397) ^ (insetBrush != null ? insetBrush.GetHashCode() : 0);
        result = (result * 397) ^ (borderBrush != null ? borderBrush.GetHashCode() : 0);
        result = (result * 397) ^ borderThickness.GetHashCode();
        return result;
      }
    }
  }

  /// <summary>
  /// Convert a row to its header insets
  /// </summary>
  public class RowInsetConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var stripe = ((INode)value).Get<IStripe>();
      if(stripe != null) {
        return new Thickness(stripe.GetActualInsets().Left, 0, stripe.GetActualInsets().Right, 0);
      }
      return new Thickness();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Convert a column to its header insets
  /// </summary>
  public class ColumnInsetConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      var stripe = ((INode)value).Get<IStripe>();
      if (stripe != null) {
        return new Thickness(0, stripe.GetActualInsets().Top, 0, stripe.GetActualInsets().Bottom);
      }
      return new Thickness();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}