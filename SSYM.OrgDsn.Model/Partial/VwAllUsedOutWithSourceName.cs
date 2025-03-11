using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class VwAllUsedOutWithSourceName
    {


        public string FldNamNodSrc
        {
            get
            {
                string str = PublicMethods.ActivityPerformerName_951(this.FldCodActSrc);
                return str;
            }
        }


    }
}
