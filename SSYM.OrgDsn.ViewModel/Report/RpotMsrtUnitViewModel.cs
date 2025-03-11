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
    public class RpotMsrtUnitViewModel : BaseReportSearchViewModel<MsrtUnitSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<MsrtUnitSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblUntMsrt> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblUntMsrt>)prevQuery;
            }
            else
            {
                finalQuery = context.TblUntMsrts;
            }

            if (cdn == null)
            {
                return finalQuery;
            }

            int intValue;
            string strValue = cdn.GetValue<string>(); ;
            bool boolValue;

            strValue = cdn.GetValue<string>();

            switch (cdn.SelectedCdn)
            {
                case MsrtUnitSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldNamUntMsrt.Trim().ToLower().Contains(strValue));
                    break;
                case MsrtUnitSrchTyp.SubjMsrt:
                    finalQuery = finalQuery.Where(m => m.TblSbjMsrt.FldNamSbjMsrt.Trim().ToLower().Contains(strValue));

                    break;
                case MsrtUnitSrchTyp.NamAct:
                    finalQuery = finalQuery.Where(m => m.TblCdns.Any(x => (x.TblEvtSrt != null) && x.TblEvtSrt.TblAct.FldNamAct.Trim().ToLower().Contains(strValue)));
                    break;
                default:
                    break;
            }
            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblUntMsrt> finalQuery = (IQueryable<TblUntMsrt>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblUntMsrt>)item);
            }

            return finalQuery;
        }

        public List<TblUntMsrt> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش واحد های سنجش"; }
        }

    }
}
