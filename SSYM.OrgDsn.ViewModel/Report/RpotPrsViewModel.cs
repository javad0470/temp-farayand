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
    public class RpotPrsViewModel : BaseReportSearchViewModel<PrsSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<PrsSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblPr> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblPr>)prevQuery;
            }
            else
            {
                finalQuery = context.TblPrs;
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
                case PrsSrchTyp.CodPrs:

                    intValue = cdn.GetValue<int>();
                    finalQuery = finalQuery.Where(m => m.FldCodPrs == intValue);

                    break;
                case PrsSrchTyp.NamPrs:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNamPrs.Trim().ToLower().Contains(strValue));

                    break;
                case PrsSrchTyp.StatusPrs:
                    SttPrs val = cdn.GetValue<SttPrs>();
                    finalQuery = finalQuery.Where(m => m.FldSttPrs.HasValue && m.FldSttPrs.Value == (int)val);

                    break;
                case PrsSrchTyp.NodPrs:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblNod.FldNamNod.Trim().ToLower().Contains(strValue));

                    break;
                default:
                    break;


            } return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblPr> finalQuery = (IQueryable<TblPr>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblPr>)item);
            }

            return finalQuery;
        }

        public List<TblPr> Prs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش فرایند ها"; }
        }


    }
}
