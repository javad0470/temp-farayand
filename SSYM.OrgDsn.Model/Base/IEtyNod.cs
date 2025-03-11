using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IEtyNod
    {
        string Name { get; set; }

        Model.Enum.AllTypEty TypEty { get; }

        string NamTypEty { get; }

        /// <summary>
        /// سازمان گره جاری
        /// </summary>
        TblOrg Org { get; }

        TblNod Nod { get; }

        List<TblNod> DetectSubNod_22116(int tnoLvlSubNod);
        
    }
}
