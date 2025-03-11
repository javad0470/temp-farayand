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
    public class RpotOrgViewModel : BaseReportSearchViewModel<OrgSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<OrgSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblOrg> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblOrg>)prevQuery;
            }
            else
            {
                finalQuery = context.TblOrgs;
            }


            if (cdn == null)
            {
                return finalQuery;
            }


            int intValue;
            string strValue;
            bool boolValue;

            switch (cdn.SelectedCdn)
            {
                case OrgSrchTyp.Nam:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNamOrg.Trim().ToLower().Contains(strValue));
                    break;
                case OrgSrchTyp.IsInner:
                    boolValue = cdn.GetValue<bool>();
                    if (boolValue)
                    {
                        finalQuery = finalQuery.Where(m => m.TblOrg2 != null);
                    }
                    else
                    {
                        finalQuery = finalQuery.Where(m => m.TblOrg2 == null);
                    }
                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblOrg> finalQuery = (IQueryable<TblOrg>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblOrg>)item);
            }

            return finalQuery;

        }


        public List<TblOrg> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش سازمان های وابسته"; }
        }

    }
}
