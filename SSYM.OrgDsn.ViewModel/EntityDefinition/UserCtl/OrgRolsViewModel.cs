using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class OrgRolsViewModel : BaseViewModel
    {
        public OrgRolsViewModel(BPMNDBEntities context, TblOrg org)
        {
            Rols = org.TblRols.ToList();
        }

        public List<TblRol> Rols { get; set; }
    }
}
