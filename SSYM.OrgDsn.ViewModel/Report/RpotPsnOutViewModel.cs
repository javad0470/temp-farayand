using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
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
    public class RpotPsnOutViewModel : BaseReportSearchViewModel<PsnOutSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<PsnOutSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblPsn> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblPsn>)prevQuery;
            }
            else
            {
                finalQuery = context.TblPsns.Where(m => !m.FldIsdOrg);
            }


            if (cdn == null)
            {
                return finalQuery;
            }

            int intValue;
            string strValue = null;
            bool boolValue;

            switch (cdn.SelectedCdn)
            {
                case PsnOutSrchTyp.Nam:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNam1stPsn.Trim().ToLower().Contains(strValue));
                    break;
                case PsnOutSrchTyp.Family:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNam2ndPsn.Trim().ToLower().Contains(strValue));
                    break;
                case PsnOutSrchTyp.ActType:

                    TypActPsn typ = cdn.GetValue<TypActPsn>();
                    finalQuery = finalQuery.Where(m => m.FldTypAct == (int)typ);

                    break;
                case PsnOutSrchTyp.SubjAct:
                    throw new NotImplementedException();
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

        public List<TblPsn> gets()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش اشخاص برون سازمانی"; }
        }

    }
}
