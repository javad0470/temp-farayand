using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class VwAllUsedOut
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        public string FldNamNod
        {
            get
            {
                return Model.PublicMethods.PerformerName_950(this.FldCodEty, this.FldCodTypEty);
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
