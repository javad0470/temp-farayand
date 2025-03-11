using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public class RpotActViewModel : BaseReportSearchViewModel<ActSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<ActSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblAct> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblAct>)prevQuery;
            }
            else
            {
                finalQuery = context.TblActs.Where(m => !m.FldActUspf);
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
                case ActSrchTyp.CodAct:
                    intValue = cdn.GetValue<int>();
                    finalQuery = finalQuery.Where(m => m.FldCodAct == intValue);
                    break;

                case ActSrchTyp.NamAct:
                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.FldNamAct.Trim().ToLower().Contains(strValue));
                    break;

                case ActSrchTyp.SubActStatus:

                    intValue = (int)cdn.GetValue<HasOrDoesntHave>();
                    finalQuery = finalQuery.Where(m => m.FldActSubHav.HasValue && m.FldActSubHav.Value == intValue);

                    break;
                case ActSrchTyp.TypeAct:

                    intValue = (int)cdn.GetValue<Model.Enum.ActivityTypes>();
                    finalQuery = finalQuery.Where(m => m.FldTypAct == intValue);

                    break;
                case ActSrchTyp.PfrAct:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => context.Nod_View.Where(x => x.NamEty.Trim().ToLower().Contains(strValue)).Any(n => m.TblNod.FldCodNod == n.FldCodNod));

                    break;
                case ActSrchTyp.EvtSrtType:

                    intValue = (int)cdn.GetValue<Model.Enum.EvtSrtType>();
                    finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.FldTypEvtSrt == intValue));

                    break;
                case ActSrchTyp.EvtRstType:

                    intValue = (int)cdn.GetValue<Model.Enum.EvtRstType>();
                    finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.FldTypEvtRst == intValue));

                    break;
                case ActSrchTyp.WayAwrType:
                    Model.Enum.TypWayAwr wayAwr = cdn.GetValue<Model.Enum.TypWayAwr>();
                    switch (wayAwr)
                    {
                        case SSYM.OrgDsn.Model.Enum.TypWayAwr.News:
                            finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.TblWayAwr_News.Count > 0));
                            break;
                        case SSYM.OrgDsn.Model.Enum.TypWayAwr.SbjOral:
                            finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.TblWayAwr_Oral.Count > 0));
                            break;
                        case SSYM.OrgDsn.Model.Enum.TypWayAwr.Obj:
                            finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.TblWayAwr_RecvInt.Count > 0));
                            break;
                        default:
                            break;
                    }

                    break;
                case ActSrchTyp.WayIfrmType:

                    Model.Enum.TypWayIfrm wayIfrm = cdn.GetValue<Model.Enum.TypWayIfrm>();
                    switch (wayIfrm)
                    {
                        case SSYM.OrgDsn.Model.Enum.TypWayIfrm.News:
                            finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblNews.Count > 0));
                            break;
                        case SSYM.OrgDsn.Model.Enum.TypWayIfrm.SbjOral:
                            finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblSbjOrals.Count > 0));
                            break;
                        case SSYM.OrgDsn.Model.Enum.TypWayIfrm.Obj:
                            finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblObjs.Count > 0));
                            break;
                        default:
                            break;
                    }
                    break;
                case ActSrchTyp.Input:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.TblWayAwr_RecvInt.Any(y => y.TblWayIfrm_SndOut.TblObj.FldNamObj.Trim().ToLower().Contains(strValue))));

                    break;
                case ActSrchTyp.Output:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblObjs.Any(y => y.FldNamObj.Trim().ToLower().Contains(strValue))));

                    break;

                case ActSrchTyp.RcevNews:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblEvtSrts.Any(x => x.TblWayAwr_News.Any(y => y.TblWayIfrm_News.TblNew.FldTtlNews.Trim().ToLower().Contains(strValue))));

                    break;

                case ActSrchTyp.SentNews:

                    strValue = cdn.GetValue<string>();
                    finalQuery = finalQuery.Where(m => m.TblEvtRsts.Any(x => x.TblNews.Any(y => y.FldTtlNews.Trim().ToLower().Contains(strValue))));

                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblAct> finalQuery = (IQueryable<TblAct>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblAct>)item);
            }

            return finalQuery;
        }

        public List<TblAct> GetActs()
        {
            return null;
        }

        public override string ReportTitle
        {
            get { return "گزارش فعالیت ها"; }
        }
    }
}
