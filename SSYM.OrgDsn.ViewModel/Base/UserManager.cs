using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public class UserManager
    {


        //static UserManager()
        //{
        //    CurrentUser = context.TblUsrs.Single(E => E.FldCodUsr == 3);
        //}
        static TblUsr currentUser;

        public static TblUsr CurrentUser
        {
            get { return UserManager.currentUser; }
            set
            {
                UserManager.currentUser = value;
                PublicMethods.CurrentUser = value;
            }
        }


        public static void ShowAccessMessage(int codMsg, params string[] txt)
        {
            MenuViewModel.MainMenu.RaisePopup(new PopupDataObject() { Content = string.Format(PublicMethods.TblMsgs.Single(E => E.FldCodMsg == codMsg).FldTxtMsg, txt), MessageBoxType = (MessageBoxType)PublicMethods.TblMsgs.Single(E => E.FldCodMsg == codMsg).FldTypMsg, Title = PublicMethods.TblMsgs.Single(E => E.FldCodMsg == codMsg).FldTtlMsg }, (s) =>
            {
            }, () => { });
        }

        //public static TblUsr CurrentUser { get; set; }

    }
}
