using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SSYM.OrgDsn.UI.Extentions
{
    public class ScrollIntoViewBehavior : System.Windows.Interactivity.Behavior<Selector>
    {

        object lastSelectedItem = null;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);


        }
        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is Selector)
            {
                Selector grid = (sender as Selector);
                if (grid.SelectedItem != null && lastSelectedItem != grid.SelectedItem)
                {
                    lastSelectedItem = grid.SelectedItem;
                    grid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        grid.UpdateLayout();

                        if (grid.SelectedItem != null)
                        {
                            dynamic d = grid;
                            d.ScrollIntoView(grid.SelectedItem);
                        }
                    }));
                }
            }
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -=
                new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
        }
    }


    public class SetSelectedAfterAddNewItemBehavior : System.Windows.Interactivity.Behavior<Selector>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Selector)
            {
                Selector grid = (sender as Selector);

                if (grid.ItemsSource != null && grid.ItemsSource is INotifyCollectionChanged)
                {
                    (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged -= SetSelectedAfterAddNewItem_CollectionChanged;
                    (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged += SetSelectedAfterAddNewItem_CollectionChanged;
                }
                else
                {
                    var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(Selector));
                    if (dpd != null)
                    {
                        dpd.RemoveValueChanged(grid, ItemsSourceChanged);
                        dpd.AddValueChanged(grid, ItemsSourceChanged);
                    }
                }

            }
        }

        private void ItemsSourceChanged(object sender, EventArgs e)
        {
            Selector grid = (sender as Selector);

            if (grid.ItemsSource != null && grid.ItemsSource is INotifyCollectionChanged)
            {
                (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged -= SetSelectedAfterAddNewItem_CollectionChanged;
                (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged += SetSelectedAfterAddNewItem_CollectionChanged;
            }
            else if (grid.ItemsSource is INotifyCollectionChanged)
            {
                (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged -= SetSelectedAfterAddNewItem_CollectionChanged;
            }
        }

        void SetSelectedAfterAddNewItem_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.AssociatedObject.Dispatcher.BeginInvoke(new Action(() =>
{
    this.AssociatedObject.SelectedItem = e.NewItems[0];
}));

            }


        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }
    }

}
