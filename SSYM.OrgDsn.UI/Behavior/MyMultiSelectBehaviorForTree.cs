using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using System.Collections.Specialized;
using System.Windows;
using System.Collections;

namespace SSYM.OrgDsn.UI.Behavior
{
    public class MyMultiSelectBehaviorForTree : Behavior<RadTreeView>
    {
        private RadTreeView Tree
        {
            get
            {
                return AssociatedObject as RadTreeView;
            }
        }

        public INotifyCollectionChanged SelectedItems
        {
            get { return (INotifyCollectionChanged)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemsProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(INotifyCollectionChanged), typeof(MyMultiSelectBehaviorForTree), new PropertyMetadata(OnSelectedItemsPropertyChanged));


        private static void OnSelectedItemsPropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var collection = args.NewValue as INotifyCollectionChanged;
            if (collection != null)
            {
                collection.CollectionChanged += ((MyMultiSelectBehaviorForTree)target).ContextSelectedItems_CollectionChanged;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            Tree.SelectedItems.CollectionChanged += TreeSelectedItems_CollectionChanged;
        }

        void ContextSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(SelectedItems as IList, Tree.SelectedItems);

            SubscribeToEvents();
        }

        void TreeSelectedItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents();

            Transfer(Tree.SelectedItems, SelectedItems as IList);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            Tree.SelectedItems.CollectionChanged += TreeSelectedItems_CollectionChanged;

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged += ContextSelectedItems_CollectionChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            Tree.SelectedItems.CollectionChanged -= TreeSelectedItems_CollectionChanged;

            if (SelectedItems != null)
            {
                SelectedItems.CollectionChanged -= ContextSelectedItems_CollectionChanged;
            }
        }

        public static void Transfer(IList source, IList target)
        {
            if (source == null || target == null)
                return;

            target.Clear();

            foreach (var o in source)
            {
                target.Add(o);
            }
        }
    }
}
