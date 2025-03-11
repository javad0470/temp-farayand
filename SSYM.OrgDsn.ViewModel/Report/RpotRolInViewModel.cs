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
    public class RpotRolInViewModel : BaseReportSearchViewModel<RolInSrchTyp>
    {
        protected override IQueryable ApplySearchCdn(SrchCdn<RolInSrchTyp> cdn, IQueryable prevQuery)
        {
            IQueryable<TblRol> finalQuery = null;

            if (prevQuery != null)
            {
                finalQuery = (IQueryable<TblRol>)prevQuery;
            }
            else
            {
                finalQuery = context.TblRols.Where(m => m.FldIsdOrg);
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
                case RolInSrchTyp.Nam:
                    finalQuery = finalQuery.Where(m => m.FldTtlRol.Trim().ToLower().Contains(strValue));
                    break;
                case RolInSrchTyp.PosPst:

                    List<TblPosPstOrg> lst = context.TblPosPstOrgs.Where(m => m.FldNamPosPst.Trim().ToLower().Contains(strValue)).ToList();

                    List<TblNod> nods = new List<TblNod>();

                    foreach (var item in lst)
                    {
                        nods.Add(context.TblNods.Single(m => m.FldCodTypEty == (int)FldTypEty.PosPst && m.FldCodEty == item.FldCodPosPst));
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

                    break;
                case RolInSrchTyp.DepOrg:

                    List<TblOrg> lst1 = PublicMethods.CurrentUser.TblOrg.GetSubOrgs();

                    lst1 = lst1.Where(m => m.FldNamOrg.Trim().ToLower().Contains(strValue)).ToList();
                    List<TblNod> nods1 = new List<TblNod>();

                    foreach (var item in lst1)
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


        public List<TblRol> getrols()
        {
            return null;
        }


        public override string ReportTitle
        {
            get { return "گزارش نقش های درون سازمانی"; }
        }

    }
}
