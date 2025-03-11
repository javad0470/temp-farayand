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
    public class RpotSoftViewModel : BaseReportSearchViewModel<SoftSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<SoftSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblSfw> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblSfw>)prevQuery;
            }
            else
            {
                finalQuery = context.TblSfws;
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
                case SoftSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldNamSfw.Trim().ToLower().Contains(strValue));
                    break;
                case SoftSrchTyp.NamAct:
                    finalQuery = finalQuery.Where(m => m.TblAct_Sfw.Any(s => s.TblAct.FldNamAct.Trim().ToLower().Contains(strValue)));

                    break;
                default:
                    break;
            }


            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblSfw> finalQuery = (IQueryable<TblSfw>)cdnGroups.First();

            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblSfw>)item);
            }

            return finalQuery;
        }

        public List<TblSfw> getlist()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش نرم‏افزار ها"; }
        }

    }
}
