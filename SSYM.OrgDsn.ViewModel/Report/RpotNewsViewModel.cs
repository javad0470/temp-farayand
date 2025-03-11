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
    public class RpotNewsViewModel : BaseReportSearchViewModel<NewsSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<NewsSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblNew> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblNew>)prevQuery;
            }
            else
            {
                finalQuery = context.TblNews;
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
                case NewsSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldTtlNews.Trim().ToLower().Contains(strValue));
                    break;
                case NewsSrchTyp.NamAct:

                    finalQuery = finalQuery.Where(m => m.TblEvtRst.TblAct.FldNamAct.Trim().ToLower().Contains(strValue));

                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblNew> finalQuery = (IQueryable<TblNew>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblNew>)item);
            }

            return finalQuery;
        }

        public List<TblNew> getnews()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش خبر ها"; }
        }

    }
}
