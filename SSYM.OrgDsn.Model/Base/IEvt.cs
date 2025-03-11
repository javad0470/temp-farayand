using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IEvt
    {
        EntityCollection<TblCdn> TblCdns { get; set; }
    }
}
