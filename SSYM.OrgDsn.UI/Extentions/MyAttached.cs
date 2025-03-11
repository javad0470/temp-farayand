using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SSYM.OrgDsn.UI.Extentions
{
    public class MyAttached
    {
        #region ' ScrollIntoView '

        public static readonly DependencyProperty ScrollIntoViewProperty =
DependencyProperty.RegisterAttached("ScrollIntoView",
typeof(bool), typeof(DataGrid), new PropertyMetadata
(true, null));

        private static void ScrollIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as DataGrid;
            grid.SelectionChanged -= grid_SelectionChanged;
            grid.SelectionChanged += grid_SelectionChanged;
        }

        static void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid grid = (sender as DataGrid);
                if (grid.SelectedItem != null)
                {
                    grid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        grid.UpdateLayout();

                        if (grid.SelectedItem != null)
                        {
                            grid.ScrollIntoView(grid.SelectedItem, null);
                        }
                    }));
                }
            }
        }

        public static void SetScrollIntoView(DataGrid element, bool value)
        {
            if (value)
            {
                element.SelectionChanged -= grid_SelectionChanged;
                element.SelectionChanged += grid_SelectionChanged;
            }
            else
            {
                element.SelectionChanged -= grid_SelectionChanged;
            }
            element.SetValue(ScrollIntoViewProperty, value);
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static bool GetScrollIntoView(DataGrid element)
        {
            return (bool)element.GetValue(ScrollIntoViewProperty);
        }

        #endregion

        #region ' SelectNewlyAdded '

        static DataGrid _grid;
        public static readonly DependencyProperty SelectNewlyAddedProperty =
DependencyProperty.RegisterAttached("SelectNewlyAdded",
typeof(bool), typeof(DataGrid), new PropertyMetadata
(true, null));

        static void grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid grid = (sender as DataGrid);

                if (grid.ItemsSource != null && grid.ItemsSource is INotifyCollectionChanged)
                {
                    (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged -= SetSelectedAfterAddNewItem_CollectionChanged;
                    (grid.ItemsSource as INotifyCollectionChanged).CollectionChanged += SetSelectedAfterAddNewItem_CollectionChanged;
                }
            }

        }

        static void SetSelectedAfterAddNewItem_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_grid != null)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    _grid.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        _grid.SelectedItem = e.NewItems[0];
                    }));

                }

            }
        }

        public static void SetSelectNewlyAdded(DataGrid element, bool value)
        {
            if (value)
            {
                element.Loaded -= grid_Loaded;
                element.Loaded += grid_Loaded;
            }
            element.SetValue(SelectNewlyAddedProperty, value);
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static bool GetSelectNewlyAdded(DataGrid element)
        {
            return (bool)element.GetValue(SelectNewlyAddedProperty);
        }

        #endregion
    }
}
