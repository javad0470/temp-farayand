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
    public class RpotErrViewModel : BaseReportSearchViewModel<ErrSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<ErrSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblEror> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblEror>)prevQuery;
            }
            else
            {
                finalQuery = context.TblErors;//.Where(m => m.TblEvtRsts.Any(x => x.TblAct.tblno == PublicMethods.CurrentUser.FldCodOrg));
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
                case ErrSrchTyp.Nam:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNamEror.Trim().ToLower().Contains(strValue));
                    break;
                case ErrSrchTyp.NamAct:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblAct.FldNamAct.Trim().ToLower().Contains(strValue)));
                    break;
                case ErrSrchTyp.TypErr:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblTypEror.FldTtlTypEror.Trim().ToLower().Contains(strValue));
                    break;
                default:
                    break;
            }


            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblEror> finalQuery = (IQueryable<TblEror>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblEror>)item);
            }

            return finalQuery;
        }

        public List<TblEror> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش خطا ها"; }
        }

    }
}
