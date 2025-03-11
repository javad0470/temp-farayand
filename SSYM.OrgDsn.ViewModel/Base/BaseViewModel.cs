using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public class BaseViewModel : NotificationObject
    {
        internal IViewModelBase Parent { get; set; }

        protected void ShowNotification(string message, MessageBoxType status, bool autoHide = false, int hideAfter = 4000)
        {
            if (Parent != null)
            {
                Parent.View.ShowNotification(message, status, autoHide, hideAfter);
            }
        }

        //void Hide(int hideAfter = 0)
        //{
        //    if (Parent != null)
        //    {
        //        Parent.View.(status, autoHide, hideAfter);
        //    }
        //}

    }
}
