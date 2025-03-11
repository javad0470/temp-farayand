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
    public class RpotIdxViewModel : BaseReportSearchViewModel<IdxSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<IdxSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblIdx> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblIdx>)prevQuery;
            }
            else
            {
                finalQuery = context.TblIdxes.Where(m => m.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg);
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
                case IdxSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldNamIdx.Trim().ToLower().Contains(strValue));
                    break;
                case IdxSrchTyp.SubjMsrt:
                    finalQuery = finalQuery.Where(m => m.TblSbjMsrt.FldNamSbjMsrt.Trim().ToLower().Contains(strValue));
                    break;
                case IdxSrchTyp.NamAct:
                    finalQuery = finalQuery.Where(m => m.TblCdns.Any(x => (x.TblEvtSrt != null) && x.TblEvtSrt.TblAct.FldNamAct.Trim().ToLower().Contains(strValue)));
                    break;
                default:
                    break;
            }


            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblIdx> finalQuery = (IQueryable<TblIdx>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblIdx>)item);
            }

            return finalQuery;
        }

        public List<TblIdx> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش شاخص ها"; }
        }
    }
}
