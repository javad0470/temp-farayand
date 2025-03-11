using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IView
    {
        MessageBoxResult RaisePopup(PopupDataObject entity, Action<PopupDataObject> callback, Action cancelCallback);

        void HideCurrentView();

        MessageBoxResult ShowPopupWindow(PopupViewModel dataContext);

        void ShowNotification(string message, MessageBoxType status, bool autoHide = false, int hideAfter = 4000);

        //void HideNotification(int hideAfter = 0);


        void HideNotification();
    }
}
