using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class BPMNDBEntities
    {
        public override int SaveChanges(System.Data.Objects.SaveOptions options)
        {
            return base.SaveChanges(options);
        }

    }
}
