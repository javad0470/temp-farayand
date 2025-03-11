using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class TblAct_Sfw
    {
        public string FldNamSfw
        {
            get
            {
                if (this.TblSfw != null)
                {
                    return this.TblSfw.FldNamSfw;
                }
                return string.Empty;
            }
        }
    }
}
