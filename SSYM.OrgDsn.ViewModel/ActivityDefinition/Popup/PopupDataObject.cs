using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public enum MessageBoxType
    {
        Information = 1,
        Error = 2,
        Warning = 3,
        Confirmation = 4,
        Question = 5
    }

    public class PopupDataObject : NotificationObject
    {
        public PopupDataObject(string content = "content", string title = "title", MessageBoxType messageBoxType = SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup.MessageBoxType.Question, object UserState = null)
        {
            this.MessageBoxType = messageBoxType;
            this.Content = content;
            this.Title = title;
            RaisePropertyChanged("MessageBoxType", "Content", "Title");
        }

        public string Content { get; set; }

        public string Title { get; set; }

        public string OkText
        {
            get
            {
                return "تأیید";
            }
        }


        public string CancelText
        {
            get
            {
                return "انصراف";
            }
        }

        public string YesText
        {
            get
            {
                return "بلی";
            }
        }

        public string NoText
        {
            get
            {
                return "خیر";
            }
        }

        public object UserState { get; set; }

        public MessageBoxType MessageBoxType
        {
            get;
            set;
        }


        public string OKContent
        {
            get
            {
                switch (this.MessageBoxType)
                {
                    case MessageBoxType.Information:
                        return OkText;
                    case MessageBoxType.Error:
                        return OkText;
                    case MessageBoxType.Warning:
                        return OkText;
                    case MessageBoxType.Confirmation:
                        return OkText;
                    case MessageBoxType.Question:
                        return YesText;
                    default:
                        break;
                }

                return null;
            }
        }


        public string CancelContent
        {
            get
            {
                switch (this.MessageBoxType)
                {
                    case MessageBoxType.Information:
                        return null;
                    case MessageBoxType.Error:
                        return null;
                    case MessageBoxType.Warning:
                        return null;
                    case MessageBoxType.Confirmation:
                        return CancelText;
                    case MessageBoxType.Question:
                        return NoText;
                    default:
                        break;
                }

                return null;
            }
        }

        public Visibility CancelVisible
        {
            get
            {
                if (MessageBoxType == Popup.MessageBoxType.Confirmation || MessageBoxType == Popup.MessageBoxType.Question)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

    }
}
