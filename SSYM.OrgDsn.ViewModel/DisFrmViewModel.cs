using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel
{
    public class DisFrmViewModel : NotificationObject
    {
        #region ' Fields '

        object selectedObj;


        #endregion

        #region ' Initialaizer '

        public DisFrmViewModel()
        {
             
        }  

        #endregion

        #region ' Properties / Commands '

        public ViewModel.ActivityDefinition.Main.ActDefViewModel DefAct { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefOrg DefOrg { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefPosPstViewModel DefPosPst { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefPsnInsideOrgViewModel DefPsnInsideOrg { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefPsnOutsideOrgViewModel DefPsnOutsideOrg { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefRolIsdOrgViewModel DefRolIsdOrg { get; set; }

        public ViewModel.EntityDefinition.UserCtl.DefRolOsdViewModel DefRolOstOrg { get; set; }

        public ViewModel.Process.UserCtl.DisPrsViewModel DisPrs { get; set; }

        public object SelectedObj
        {
            get { return selectedObj; }
            set
            {
                selectedObj = value;
                RaisePropertyChanged("SelectedObj");
            }
        }


        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion

    }
}
