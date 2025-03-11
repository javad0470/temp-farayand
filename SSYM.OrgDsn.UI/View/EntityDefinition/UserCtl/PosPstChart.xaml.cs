using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Controls.Diagrams.Extensions;
using Telerik.Windows.Diagrams.Core;
using System.Windows.Threading;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using SSYM.OrgDsn.ViewModel.EntityDefinition.OrgChart;
using System.Windows.Controls;
using Telerik.Windows.DragDrop;


namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    public partial class PosPstChart : UserControl
    {
        private PosPstChartViewModel viewModel;
        private TreeLayout treeLayout;
        private Storyboard collapse, show;
        private static double minZoom, maxZoom, zoomFactor;

        public PosPstChart()
        {
            InitializeComponent();
            DragDropManager.AddDragInitializeHandler(diagram, OnDragInitialize);
            DragDropManager.AddGiveFeedbackHandler(diagram, OnGiveFeedback);
            this.Loaded += this.OnOrgChartExampleLoaded;

        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.All;
            Telerik.Windows.Controls.RadDiagramShape shp = (Telerik.Windows.Controls.RadDiagramShape)args.OriginalSource;
            args.DragVisual = new ContentControl() { ContentTemplate = shp.ContentTemplate, Content = shp.Content };
        }
        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;

        }

        private void GetViewModelAndBindForEvents()
        {
            if (this.viewModel != null)
            {
                if (this.viewModel.ChildTreeLayoutViewModel == null)
                {
                    return;
                }
                //this.viewModel = this.RootGrid.Resources["ViewModel"] as OrgChartViewModel;
                this.viewModel.ChildTreeLayoutViewModel.LayoutSettingsChanged += (_, __) => this.LayoutOrgChart(false);
                this.viewModel.ChildrenExpandedOrCollapsed += this.OnViewModelChildrenExpandedOrCollapsed;
            }
        }

        private void OnViewModelChildrenExpandedOrCollapsed(object sender, EventArgs e)
        {
            if (this.viewModel.ShouldLayoutAfterExpandCollapse == true)
            {
                this.LayoutOrgChart(false);
            }
        }

        private void OnOrgChartExampleLoaded(object sender, RoutedEventArgs e)
        {
            this.viewModel = this.RootGrid.DataContext as PosPstChartViewModel;
            this.GetViewModelAndBindForEvents();
            this.Unloaded += this.OnOrgChartExampleUnloaded;
            this.diagram.RoutingService.Router = this.viewModel.Router;
            this.treeLayout = new TreeLayout();


            minZoom = DiagramConstants.MinimumZoom;
            maxZoom = DiagramConstants.MaximumZoom;
            zoomFactor = DiagramConstants.ZoomFactor;

            DiagramConstants.MinimumZoom = 0.2d;
            DiagramConstants.MaximumZoom = 20d;
            DiagramConstants.ZoomFactor = 0.1d;

            this.BindSelectionChangedEvents();
            this.SetLayoutRoots();
            this.LayoutOrgChart(false);

            this.Loaded -= OnOrgChartExampleLoaded;
        }

        private void OnOrgChartExampleUnloaded(object sender, RoutedEventArgs e)
        {
            DiagramConstants.MinimumZoom = minZoom;
            DiagramConstants.MaximumZoom = maxZoom;
            DiagramConstants.ZoomFactor = zoomFactor;
        }

        private void BindSelectionChangedEvents()
        {
            this.viewModel.CurrentLayoutTypeChanged += (_, __) => this.LayoutOrgChart(false);
        }

        private void InvokeDispatchedLayout(bool shouldAautofit)
        {
            Action action = new Action(() => this.LayoutOrgChart(shouldAautofit));

            this.Dispatcher.BeginInvoke(action, DispatcherPriority.Background);
        }

        private void TreeLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.LayoutOrgChart(false);
        }

        private void SetLayoutRoots()
        {
            if (this.viewModel == null || this.viewModel.HierarchicalDataSource == null)
            {
                return;
            }
            foreach (var item in this.viewModel.HierarchicalDataSource)
            {
                RadDiagramShape shape = this.diagram.ContainerGenerator.ContainerFromItem(item) as RadDiagramShape;
                this.viewModel.ChildTreeLayoutViewModel.CurrentLayoutSettings.Roots.Add(shape);
            }
        }

        private void LayoutOrgChart(bool shouldAutoFit)
        {
            if (!IsLoaded || this.viewModel.ChildTreeLayoutViewModel == null)
            {
                return;
            }
            // suspend auto update for all connections:
            this.diagram.Connections.ForEach(x => RadDiagramConnection.SetIsAutoUpdateSuppressed((RadDiagramConnection)x, true));
            this.EnsureConnectors();
            this.treeLayout.Layout(this.diagram, this.viewModel.ChildTreeLayoutViewModel.CurrentLayoutSettings);

            // unsuspend auto update for all connections & update:
            this.diagram.Connections.ForEach(x => RadDiagramConnection.SetIsAutoUpdateSuppressed((RadDiagramConnection)x, false));
            this.diagram.Connections.ForEach(x => x.Update());

            if (shouldAutoFit)
            {
                this.diagram.AutoFit(new Thickness(10), false);
            }
        }

        private void EnsureConnectors()
        {
            //if (this.viewModel.CurrentTreeLayoutType == TreeLayoutType.TipOverTree)
            //{
            //    var shapesWithIncomingLinks = this.diagram.Shapes.Where(x => x.IncomingLinks.Any()).ToList();
            //    shapesWithIncomingLinks.ForEach(y =>
            //    {
            //        if (y.Connectors.Count == 5)
            //        {
            //            var customConnector = new RadDiagramConnector { Offset = new Point(0.15, 1) };
            //            customConnector.Name = CustomConnectorPosition.TreeLeftBottom;
            //            y.Connectors.Add(customConnector);
            //        }
            //    });
            //}
        }

        //private void DiagramExpandNodeByPath(string path)
        //{
        //    string[] pathParts = path.Split('|');
        //    if (pathParts.Length < 2) return;
        //    for (int i = 0; i < pathParts.Length - 1; i++)
        //    {
        //        var allLinks = this.viewModel.GraphSource.Links.ToList();
        //        var currLink = allLinks.FirstOrDefault(x => (x.Source as Node).FullName == pathParts[i] && (x.Target as Node).FullName == pathParts[i + 1]) as Link;
        //        if (currLink != null)
        //        {
        //            currLink.Visibility = Visibility.Visible;
        //            currLink.Source.Visibility = Visibility.Visible;
        //            currLink.Source.AreChildrenCollapsed = false;

        //            //A) Making children visible.
        //            foreach (var item in currLink.Source.Children)
        //            {
        //                var link = this.viewModel.GraphSource.Links.FirstOrDefault(x => x.Source == currLink.Source && x.Target == item) as Link;
        //                link.Visibility = Visibility.Visible;
        //                item.Visibility = Visibility.Visible;

        //                //B) Presumption till not changed in A.
        //                (item as Node).AreChildrenCollapsed = true;
        //            }
        //        }
        //    }
        //}

        private void LayoutButtonClicked(object sender, RoutedEventArgs e)
        {
            this.SetLayoutRoots();
            this.LayoutOrgChart(true);
        }

        private void diagram_ItemsChanged_1(object sender, DiagramItemsChangedEventArgs e)
        {
            this.SetLayoutRoots();
            this.LayoutOrgChart(true);
        }


        private void txtNam_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
    }
}
