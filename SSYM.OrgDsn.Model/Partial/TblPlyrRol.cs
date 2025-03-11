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
    public partial class TblPlyrRol
    {
        /// <summary>
        /// نام عضو مربوطه
        /// </summary>
        public string NameOfPlayer
        {
            get
            {
                return PublicMethods.PerformerName_950(this.TblNod.FldCodEty, this.TblNod.FldCodTypEty);
            }

        }


        public void DeleteFromRol()
        {
            TblRol.TblPlyrRols.Remove(this);
        }
    }
}
