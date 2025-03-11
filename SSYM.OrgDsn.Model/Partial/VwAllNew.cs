using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class VwAllNew
    {

        public string FldNamNod
        {
            get { return PublicMethods.ActivityPerformerName_951(this.FldCodAct); }
        }
        
    }
}
