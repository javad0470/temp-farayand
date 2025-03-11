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
    public class RpotPosPstViewModel : BaseReportSearchViewModel<PosPstSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<PosPstSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblPosPstOrg> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblPosPstOrg>)prevQuery;
            }
            else
            {
                finalQuery = context.TblPosPstOrgs;
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
                case PosPstSrchTyp.PosPst:
                    finalQuery = finalQuery.Where(m => m.FldNamPosPst.Trim().ToLower().Contains(strValue));
                    break;
                default:
                    break;
            }
            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblPosPstOrg> finalQuery = (IQueryable<TblPosPstOrg>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblPosPstOrg>)item);
            }

            return finalQuery;
        }

        public List<TblPosPstOrg> GetOrgs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش جایگاه و سمت ها"; }
        }

    }
}
