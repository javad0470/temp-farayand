using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class PsnInfoViewModel : BaseViewModel
    {
        #region ' Fields '

        bool isEnabled;
        TblPsn person;

        #endregion

        #region ' Initialaizer '

        public PsnInfoViewModel()
        {
        }

        #endregion

        #region ' Properties / Commands '

        public TblPsn Person
        {
            get
            {
                return person;
            }
            set
            {
                person = value;
                RaisePropertyChanged("Person");
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }

        #endregion

        #region ' Public Methods '
        #endregion

        #region ' Private Methods '
        #endregion

        #region ' events '
        #endregion

    }
}
