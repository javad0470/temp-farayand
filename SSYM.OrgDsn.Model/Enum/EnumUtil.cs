using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.Enum
{
    class EnumUtil
    {
        public static string NamNod(SSYM.OrgDsn.Model.Enum.AllTypEty typ)
        {
            switch (typ)
            {
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Org:
                    return "سازمان";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Pos:
                    return "جایگاه";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Pst:
                    return "سمت";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Rol:
                    return "نقش";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Psn:
                    return "شخص";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Act:
                    return "فعالیت";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Prs:
                    return "فرایند";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Agnt:
                    return "نماینده";
                case SSYM.OrgDsn.Model.Enum.AllTypEty.Dson:
                    return "ناهمسانی";
                default:
                    return "";
            }
        }

    }
}
