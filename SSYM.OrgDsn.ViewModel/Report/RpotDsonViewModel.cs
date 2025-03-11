using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.Report
{
    class DsonComparer : IEqualityComparer<Tuple<IWayAwrIfrm, TblNod>>
    {

        public bool Equals(Tuple<IWayAwrIfrm, TblNod> x, Tuple<IWayAwrIfrm, TblNod> y)
        {
            return (x.Item1.GetHashCode() == y.Item1.GetHashCode());
        }

        public int GetHashCode(Tuple<IWayAwrIfrm, TblNod> obj)
        {
            return obj.Item1.GetHashCode();
        }
    }

    public class RpotDsonViewModel : BaseReportSearchViewModel<DsonSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<DsonSrchTyp> cdn, IQueryable prevQuery)
        {
            if (cdn == null)
            {
                return null;
            }
            IQueryable finalQuery = null;

            int intValue;
            string strValue = cdn.GetValue<string>();
            bool boolValue;

            switch (cdn.SelectedCdn)
            {
                case DsonSrchTyp.PosPstRol:

                    List<TblPosPstOrg> posPsts = context.TblPosPstOrgs.Where(m => m.FldNamPosPst.Trim().ToLower().Contains(strValue)).ToList();

                    List<TblRol> rols = context.TblRols.Where(m => m.FldTtlRol.Trim().ToLower().Contains(strValue)).ToList();

                    List<TblNod> allNods = new List<TblNod>();

                    foreach (var item in posPsts)
                    {
                        TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodEty == item.FldCodPosPst && m.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst);

                        if (nod != null)
                        {
                            allNods.Add(nod);
                        }
                    }

                    foreach (var item in rols)
                    {
                        TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodEty == item.FldCodRol && m.FldCodTypEty == (int)Model.Enum.FldTypEty.Rol);

                        if (nod != null)
                        {
                            allNods.Add(nod);
                        }
                    }

                    //TblNod nod = context.TblNods.Where(m=>m.FldCodTypEty ==  0 && 
                    //context.TblPosPstOrgs.Where(m => m.FldNamPosPst == "");

                    if (allNods.Count > 0)
                    {
                        foreach (var item in allNods)
                        {
                            if (finalQuery != null)
                            {
                                List<Tuple<IWayAwrIfrm, TblNod>> lst = new List<Tuple<IWayAwrIfrm, TblNod>>();
                                var query = PublicMethods.DetectDsonsClaimedByNod_19020(context, item);
                                foreach (var q in query)
                                {
                                    lst.Add(new Tuple<IWayAwrIfrm, TblNod>(q.Item1, item));
                                }
                                finalQuery = (finalQuery as IQueryable<Tuple<IWayAwrIfrm, TblNod>>).Union(lst.AsQueryable(), new DsonComparer());
                            }
                            else
                            {
                                List<Tuple<IWayAwrIfrm, TblNod>> lst = new List<Tuple<IWayAwrIfrm, TblNod>>();
                                var query = PublicMethods.DetectDsonsClaimedByNod_19020(context, item);
                                foreach (var q in query)
                                {
                                    lst.Add(new Tuple<IWayAwrIfrm, TblNod>(q.Item1, item));
                                }

                                finalQuery = lst.AsQueryable();
                            }
                        }

                    }
                    else
                    {
                        return prevQuery;
                    }

                    if (prevQuery != null)
                    {
                        finalQuery = (finalQuery as IQueryable<Tuple<IWayAwrIfrm, TblNod>>).Intersect(prevQuery as IQueryable<Tuple<IWayAwrIfrm, TblNod>>, new DsonComparer());
                    }

                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<Tuple<IWayAwrIfrm, TblNod>> finalQuery = (IQueryable<Tuple<IWayAwrIfrm, TblNod>>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<Tuple<IWayAwrIfrm, TblNod>>)item, new DsonComparer());
            }

            return finalQuery;
        }

        public override string ReportTitle
        {
            get { return "گزارش ناهمسانی ها"; }
        }


        public IQueryable<Tuple<IWayAwrIfrm, TblNod>> getQuery()
        {
            return null;
        }

    }
}
