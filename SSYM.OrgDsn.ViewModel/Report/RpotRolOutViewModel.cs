using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.Report.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.Report
{
    class OrgComparer : IEqualityComparer<TblOrg>
    {

        public bool Equals(TblOrg x, TblOrg y)
        {
            return x.FldCodOrg == y.FldCodOrg;
        }

        public int GetHashCode(TblOrg obj)
        {
            return obj.FldCodOrg.GetHashCode();
        }
    }
    public class RpotRolOutViewModel : BaseReportSearchViewModel<RolOutSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<RolOutSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblRol> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblRol>)prevQuery;
            }
            else
            {
                finalQuery = context.TblRols.Where(m => !m.FldIsdOrg);
            }

            if (cdn == null)
            {
                return finalQuery;
            }

            int intValue;
            string strValue = cdn.GetValue<string>();
            bool boolValue;
            List<TblRol> rolList1 = null;
            List<TblRol> rolList2 = null;


            switch (cdn.SelectedCdn)
            {
                case RolOutSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldTtlRol.Trim().ToLower().Contains(strValue));
                    break;
                case RolOutSrchTyp.NamPsn:
                    List<TblPsn> lst = context.TblPsns.Where(m => !m.FldIsdOrg && (m.FldNam1stPsn + " " + m.FldNam2ndPsn).Trim().ToLower().Contains(strValue)).ToList();

                    List<TblNod> nods = new List<TblNod>();

                    foreach (var item in lst)
                    {
                        nods.Add(context.TblNods.Single(m => m.FldCodTypEty == (int)FldTypEty.Psn && m.FldCodEty == item.FldCodPsn));
                    }


                    rolList1 = finalQuery.ToList();

                    rolList2 = new List<TblRol>();

                    foreach (var rol in rolList1)
                    {
                        foreach (var nod in nods)
                        {
                            if (rol.TblPlyrRols.FirstOrDefault(m => m.FldCodNod == nod.FldCodNod) != null)
                            {
                                rolList2.Add(rol);
                                break;
                            }
                        }
                    }

                    finalQuery = rolList2.AsQueryable();

                    //finalQuery = finalQuery.Where(m => m.TblPlyrRols.Any(x => nods.FirstOrDefault(y => y.FldCodNod == x.FldCodNod) != null));

                    break;
                case RolOutSrchTyp.OrgOuter:

                    List<TblOrg> inOrg = PublicMethods.CurrentUser.TblOrg.GetSubOrgs();

                    List<TblOrg> outOrg = context.TblOrgs.Where(m => inOrg.FirstOrDefault(x => x.FldCodOrg == m.FldCodOrg) == null).ToList();

                    List<TblNod> nods1 = new List<TblNod>();

                    foreach (var item in outOrg)
                    {
                        nods1.Add(context.TblNods.Single(m => m.FldCodTypEty == (int)FldTypEty.Org && m.FldCodEty == item.FldCodOrg));
                    }



                    rolList1 = finalQuery.ToList();

                    rolList2 = new List<TblRol>();

                    foreach (var rol in rolList1)
                    {
                        foreach (var nod in nods1)
                        {
                            if (rol.TblPlyrRols.FirstOrDefault(m => m.FldCodNod == nod.FldCodNod) != null)
                            {
                                rolList2.Add(rol);
                                break;
                            }
                        }
                    }

                    finalQuery = rolList2.AsQueryable();

                    //finalQuery = finalQuery.Where(m => m.TblPlyrRols.Any(x => nods1.FirstOrDefault(y => y.FldCodNod == x.FldCodNod) != null));

                    break;
                default:
                    break;
            }

            return finalQuery;
        }

        protected override IQueryable UnionAllGroups(List<IQueryable> cdnGroups)
        {
            IQueryable<TblRol> finalQuery = (IQueryable<TblRol>)cdnGroups.First();
            foreach (var item in cdnGroups.Skip(1))
            {
                finalQuery = finalQuery.Union((IQueryable<TblRol>)item);
            }

            return finalQuery;
        }


        public override string ReportTitle
        {
            get { return "گزارش نقش های برون سازمانی"; }
        }

    }
}
