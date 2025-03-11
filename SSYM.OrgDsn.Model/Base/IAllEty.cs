using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public interface IAllEty
    {
        int CodEty { get; }

        Enum.AllTypEty CodTypEty { get; }

        string Name { get; }
    }
}
