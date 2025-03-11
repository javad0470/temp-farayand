using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public class RpotPsnInViewModel : BaseReportSearchViewModel<PsnInSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<PsnInSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblPsn> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblPsn>)prevQuery;
            }
            else
            {
                finalQuery = context.TblPsns;
            }

            if (cdn == null)
            {
                return finalQuery;
            }

            int intValue;
            string strValue = cdn.GetValue<string>();
            bool boolValue;

            switch (cdn.SelectedCdn)
            {
                case PsnInSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldNam1stPsn.Trim().ToLower().Contains(strValue));
                    break;
                case PsnInSrchTyp.Family:
                    finalQuery = finalQuery.Where(m => m.FldNam2ndPsn.Trim().ToLower().Contains(strValue));
                    break;
                case PsnInSrchTyp.Org:
                    finalQuery = finalQuery.Where(m => m.TblUsrs.Any(u => u.TblOrg.FldNamOrg.Trim().ToLower().Contains(strValue)));
                    break;
                case PsnInSrchTyp.PosPstRol:
                    finalQuery = finalQuery.Where(m => context.VwPosPstRolOfPsns.Any(x => x.FldCodPsn == m.FldCodPsn && x.NamEty.Trim().ToLower().Contains(strValue)));
                    //finalQuery = finalQuery.Where(m => m.TblUsrs.Any(u => u.TblOrg.TblPosPstOrgs.Any(p => p.FldNamPosPst.Trim().ToLower().Contains(strValue))));
                    break;
                default:
                    break;
            }
            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblPsn> finalQuery = (IQueryable<TblPsn>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblPsn>)item);
            }

            return finalQuery;
        }

        public override string ReportTitle
        {
            get { return "گزارش اشخاص درون سازمانی"; }
        }

    }
}
