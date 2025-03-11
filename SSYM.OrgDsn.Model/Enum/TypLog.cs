using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    /// <summary>
    /// بر اساس 
    /// entity state
    /// </summary>
    public enum TypLog
    {
        Error = 1,
        Add = 4,
        Delete = 8,
        Edit = 16,
        Access = 5,
        Info = 6
    }
}
