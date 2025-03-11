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
    public class RpotInOutViewModel : BaseReportSearchViewModel<InOutSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<InOutSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblObj> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblObj>)prevQuery;
            }
            else
            {
                finalQuery = context.TblObjs;
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
                case InOutSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldNamObj.Trim().ToLower().Contains(strValue));
                    break;
                case InOutSrchTyp.NamAct:
                    finalQuery = finalQuery.Where(m => m.TblEvtRst.TblAct.FldNamAct.Trim().ToLower().Contains(strValue));
                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblObj> finalQuery = (IQueryable<TblObj>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblObj>)item);
            }

            return finalQuery;
        }

        public List<TblObj> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش ورودی/خروجی ها"; }
        }
    }
}
