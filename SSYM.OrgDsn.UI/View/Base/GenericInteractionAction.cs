using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Controls;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.UI.View.Base
{
    public class GenericInteractionAction<T> : TriggerAction<Grid>
    {
        public static readonly DependencyProperty DialogProperty =
            DependencyProperty.Register("Dialog", typeof(GenericInteractionDialogBase<T>), typeof(GenericInteractionAction<T>), new PropertyMetadata(null));

        public GenericInteractionDialogBase<T> Dialog
        {
            get { return (GenericInteractionDialogBase<T>)GetValue(DialogProperty); }
            set { SetValue(DialogProperty, value); }
        }


        protected override void Invoke(object parameter)
        {
            var args = parameter as GenericInteractionRequestEventArgs<T>;
            this.SetDialog(args.Entity, args.Callback, args.CancelCallback, null);
        }

        private void SetDialog(T entity, Action<T> callback, Action cancelCallback, UIElement element)
        {
            PopupDataObject obj = entity as PopupDataObject;
            MessageBoxResult result = MessageBoxResult.Cancel;
            switch (obj.MessageBoxType)
            {
                case MessageBoxType.Information:
                    result = MessageBox.Show(obj.Content, obj.Title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case MessageBoxType.Error:
                    result = MessageBox.Show(obj.Content, obj.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case MessageBoxType.Warning:
                    result = MessageBox.Show(obj.Content, obj.Title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case MessageBoxType.Confirmation:
                    result = MessageBox.Show(obj.Content, obj.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                    break;
                case MessageBoxType.Question:
                    result = MessageBox.Show(obj.Content, obj.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

                    break;
                default:
                    break;
            }

            if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
            {
                callback(entity);
            }

            if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
            {
                cancelCallback();
            }

            return;


            //if (this.Dialog is IGenericInteractionView<T>)
            //{
            //    IGenericInteractionView<T> view = this.Dialog as IGenericInteractionView<T>;
            //    view.SetEntity(entity);

            //    EventHandler handler = null;
            //    handler = (s, e) =>
            //    {

            //        this.Dialog.ConfirmEventHandler -= handler;
            //        this.Dialog.CancelEventHandler -= handler;
            //        this.AssociatedObject.Children.Remove(this.Dialog);

            //        if (e.ToString() == InteractionType.OK.ToString())
            //            callback(view.GetEntity());
            //        else
            //        {
            //            if (cancelCallback != null)
            //            {
            //                cancelCallback();
            //            }
            //        }
            //    };

            //    this.Dialog.ConfirmEventHandler += handler;
            //    this.Dialog.CancelEventHandler += handler;
            //    this.Dialog.SetValue(Grid.RowSpanProperty, this.AssociatedObject.RowDefinitions.Count == 0 ? 1 : this.AssociatedObject.RowDefinitions.Count);
            //    this.Dialog.SetValue(Grid.ColumnSpanProperty, this.AssociatedObject.ColumnDefinitions.Count == 0 ? 1 : this.AssociatedObject.ColumnDefinitions.Count);
            //    this.AssociatedObject.Children.Add(this.Dialog);
            //}
        }
    }
}

