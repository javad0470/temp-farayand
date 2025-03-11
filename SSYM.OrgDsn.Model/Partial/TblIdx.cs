using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public partial class TblIdx : INamedItm
    {

        public List<string> ActNames
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    TblIdx idx = context.TblIdxes.Single(m => m.FldCodIdx == this.FldCodIdx);
                    return idx.TblCdns.Where(m => m.TblEvtSrt != null).Select(m => m.TblEvtSrt.TblAct.FldNamAct).ToList();
                }
            }
        }

        public string Name
        {
            get { return this.FldNamIdx; }
        }


        public string Type
        {
            get { return this.TblSbjMsrt.FldNamSbjMsrt; }
        }
    }
}
