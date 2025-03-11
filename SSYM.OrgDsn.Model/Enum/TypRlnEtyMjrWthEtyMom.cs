using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Enum
{
    public enum TypRlnEtyMjrWthEtyMom
    {
        /// <summary>
        /// مالک فرآیند
        /// </summary>
        OwnerProcess,
        /// <summary>
        /// مشارکت کننده در فرآیند
        /// </summary>
        ContributerInProcess,
        /// <summary>
        /// مجری فعالیت
        /// </summary>
        PerformerOfActivity,
        /// <summary>
        /// سازمان جاری به همراه تمام جایگاه ها و سمت های  آن
        /// </summary>
        PosPstOfOrgAndCurrentOrg,
        /// <summary>
        /// سازمان جاری به همراه سازمان های تابعه
        /// </summary>
        OrgCntAndOrgSub,

        /// <summary>
        /// خود گره
        /// </summary>
        NodSelf

    }
}
