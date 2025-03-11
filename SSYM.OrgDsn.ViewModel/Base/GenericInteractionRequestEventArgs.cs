using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public class GenericInteractionRequestEventArgs<T> : EventArgs
    {
        public GenericInteractionRequestEventArgs(T _entity, Action<T> _callback, Action _cancelCallback)
        {
            this.CancelCallback = _cancelCallback;
            this.Callback = _callback;
            this.Entity = _entity;
        }

        public Action CancelCallback { get; private set; }
        public Action<T> Callback { get; private set; }
        public T Entity { get; private set; }
    }
}
