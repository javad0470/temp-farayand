using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public partial class TblAgntNod
    {
        public Enum.DmnAcs DmnAcs
        {
            get
            {
                return (Enum.DmnAcs)this.FldDmnAgnt;
            }

            set
            {
                this.FldDmnAgnt = (int)value;

                OnPropertyChanged("DmnAcs");
            }
        }


        public string NamNod
        {
            get
            {
                return this.TblNod.EtyNod.Name;
            }
        }

        bool? _isAdmin;
        /// <summary>
        /// آیا این نمایندگی نمایندگی ادمین ست؟
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                if (!_isAdmin.HasValue)
                {
                    _isAdmin= this.TblPsn.TblUsrs.Any(u => u.FldNamUsr.Trim() == "admin");
                }
                return _isAdmin.Value;
            }
        }
    }
}
