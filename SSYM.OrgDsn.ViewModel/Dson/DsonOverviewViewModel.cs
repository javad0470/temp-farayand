using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using System.Windows.Data;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class DsonOverviewViewModel : BaseViewModel
    {
        #region ' Fields '

        IWayAwrIfrm _currAwrIfrm;


        #endregion

        #region ' Initialaizer '

        public DsonOverviewViewModel()
        {
        }

        #endregion

        #region ' Properties / Commands '


        public string Desc
        {
            get
            {
                if (CurrAwrIfrm != null)
                {
                    return PublicMethods.GetDsonDesc(CurrAwrIfrm);
                }
                else
                {
                    return null;
                }
            }
        }

        public IWayAwrIfrm CurrAwrIfrm
        {
            get { return _currAwrIfrm; }
            set
            {
                _currAwrIfrm = value;
                RaisePropertyChanged("CurrAwrIfrm", "CurrAwr", "CurrIfrm", "Desc");
            }
        }

        public IWayAwr CurrAwr
        {
            get
            {
                if (CurrAwrIfrm != null)
                {
                    return CurrAwrIfrm as IWayAwr;
                }
                else
                {
                    return null;
                }
            }
        }

        public IWayIfrm CurrIfrm
        {
            get
            {
                if (CurrAwrIfrm != null)
                {
                    return CurrAwrIfrm as IWayIfrm;
                }
                else
                {
                    return null;
                }
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
