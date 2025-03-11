using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.Dson.UserCtl
{
    /// <summary>
    /// Interaction logic for DsonOverview.xaml
    /// </summary>
    public partial class DsonOverview : UserControl
    {
        public DsonOverview()
        {
            InitializeComponent();

            this.DataContextChanged += DsonOverview_DataContextChanged;

            this.Loaded += DsonOverview_Loaded;

            this.SizeChanged += DsonOverview_SizeChanged;
        }

        void DsonOverview_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                var vm = this.DataContext as SSYM.OrgDsn.ViewModel.Dson.DsonOverviewViewModel;
                if (vm.CurrAwr != null)
                {
                    callOutDest = tbkRight; 
                }
                else
                {
                    callOutDest = tbkLeft; 
                }

                AdjustCalloutAnchor(callOutDest);
            }
        }

        FrameworkElement callOutDest = null;

        void DsonOverview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustCalloutAnchor(callOutDest);
        }

        void DsonOverview_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustCalloutAnchor(callOutDest);
        }

        private void AdjustCalloutAnchor(FrameworkElement el)
        {
            if (el == null)
            {
                return;
            }
            // locate the positions of the callout and the element to point to
            Point calloutPoint = callOutDson.TransformToAncestor(grdDson).Transform(new Point(0, 0));
            Point elementPoint = el.TransformToAncestor(grdDson).Transform(new Point(0, 0));

            double vertOffset = calloutPoint.Y + callOutDson.ActualHeight;
            double horizOffset = elementPoint.X;
            if (calloutPoint.X > elementPoint.X)
            {
                // increase the horizontal offset if the callout 
                // is to the right of the element
                horizOffset += el.ActualWidth;
            }

            double vertDistance = (elementPoint.Y + el.ActualHeight) - vertOffset;
            double horizDistance = calloutPoint.X - horizOffset;

            // add some cushion between the element and the new AnchorPoint
            int pad = 5;

            // the AnchorPoint is a relative distance based on the actual height
            // and width of the callout.
            double x = (horizDistance / (callOutDson.ActualWidth + pad)) * -1;
            double y = (vertDistance / (callOutDson.ActualHeight + pad)) + 1;

            // set the new AnchorPoint location
            callOutDson.AnchorPoint = new Point(x, y);

            //callOutDson.Content = string.Format("I'm pointing at {0}", el.Name);
        }
    }
}
