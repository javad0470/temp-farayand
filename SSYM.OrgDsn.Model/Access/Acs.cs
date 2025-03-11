using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model.Base;
using System.Data.Entity;

namespace SSYM.OrgDsn.Model.Access
{
    public class Acs
    {
        public Acs()
        {
            HasAcs = new Dictionary<string, bool>();
        }

        /// <summary>
        /// مشخص میکند که ایا به دلیل داشتن نمایندگی سازمان امکان ویرایش نقش وجود دارد یا خیر 
        /// </summary>
        public bool EditRolAllowedByOrg { get; set; }

        /// <summary>
        /// مشخص میکند که ایا به دلیل داشتن نمایندگی سازمان امکان ویرایش جایگاه و سمت وجود دارد یا خیر 
        /// </summary>
        public bool EditPosPstAllowedByOrg { get; set; }

        public void ExeAcsWotEtyMom_25266()
        {
            HasAcs.Clear();

            var psn = PublicMethods.CurrentUser.TblPsn;

            // شناسایی نماینده از نوع سازمان شخص جاری در سازمان جاری
            var agnt = psn.TblAgntNods.SingleOrDefault(a => a.TblNod.EtyNod is TblOrg && a.TblNod.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg);

            // اگر شخص جاری نماینده سازمان جاری نیست؟ 
            // تمامی آیتم های دسترسی ره مقدار دهی میکند false بده
            if (agnt == null)
            {
                using (BPMNDBEntities ctx = new BPMNDBEntities())
                {
                    //.Where(i => i.FldCodTypEtyMom == 1)
                    var itmsAcs = ctx.TblItmAcs.ToList();
                    foreach (var item in itmsAcs)
                    {
                        HasAcs[item.FldNamActn + item.FldNamTypEtyMjr] = false;
                    }
                }
            }
            else
            {
                // اگر شخص جاری نماینده سازمان جاری هست
                // آیتمهای سطح دسترسی سازمان جاری را برابر با ایتم موجود در سطح دسترسی قرار میدهد
                foreach (var item in agnt.TblLvlAc.TblItmAcs)
                {
                    if (item.FldCodTypEtyMom == 1)
                    {
                        HasAcs[item.FldNamActn + item.FldNamTypEtyMjr] = true;
                    }
                }

                //و بقیه را false میدهد.
                using (BPMNDBEntities ctx = new BPMNDBEntities())
                {
                    //.Where(i => i.FldCodTypEtyMom == 1)
                    var itmsAcs = ctx.TblItmAcs.ToList();
                    foreach (var item in itmsAcs)
                    {
                        if (!HasAcs.ContainsKey(item.FldNamActn + item.FldNamTypEtyMjr))
                        {
                            HasAcs[item.FldNamActn + item.FldNamTypEtyMjr] = false;
                        }
                    }
                }
            }

            EditRolAllowedByOrg = HasAcs["EditRolIsdOrg"];
            EditPosPstAllowedByOrg = HasAcs["EditPosPst"];
        }

        public void ExeAcsWotEtyMom_22061()
        {
            ExeAcsWotEtyMom_25266();
            return;

            //BPMNDBEntities context
            //using (BPMNDBEntities context = new BPMNDBEntities())
            {
                //PublicMethods.DetectAllItmAcs(context);
                //PublicMethods.AllItmAcs = 
                if (PublicMethods.AllItmAcs != null)
                {
                    //22070
                    foreach (var item in PublicMethods.AllItmAcs)
                    {
                        //context.LoadProperty(item, "TblLvlAcs");
                        List<TblLvlAc> lvlAcs = item.TblLvlAcs.ToList();

                        List<TblNod> nodWthAcs = new List<TblNod>();

                        //22071
                        foreach (TblLvlAc lvlAcs_i in lvlAcs)
                        {
                            //22085
                            foreach (TblAgntNod agntNod_i in lvlAcs_i.TblAgntNods.Where(m => m.TblNod.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.TblOrg.FldCodOrg))
                            {
                                //22075
                                if (agntNod_i.TblPsn.FldCodPsn == PublicMethods.CurrentUser.TblPsn.FldCodPsn)
                                {
                                    nodWthAcs.Add(agntNod_i.TblNod);
                                }
                            }
                        }

                        //22078, 22081
                        if (!HasAcs.Keys.Contains(item.FldNamActn + item.FldNamTypEtyMjr))
                        {
                            HasAcs.Add(item.FldNamActn + item.FldNamTypEtyMjr, nodWthAcs.Count > 0);
                        }

                        else
                        {
                            //if (!HasAcs[item.FldNamActn + item.FldNamTypEtyMjr])
                            {
                                HasAcs[item.FldNamActn + item.FldNamTypEtyMjr] = nodWthAcs.Count > 0;
                            }
                        }
                    }

                }
            }
        }


        /// <summary>
        /// برای مشخص کردن سازمانی که دسترسی در آن چک می شود     
        /// </summary>
        /// <param name="fldCodOrg"></param>
        /// <param name="etyCnt"></param>
        /// <param name="namActn"></param>
        /// <param name="typRlnEtyMjrWthEtyMom"></param>
        public void DetectSttAcsWthEtyMom_22090(
            int fldCodOrg,
    IAllEty etyCnt,
    string namActn,
    Enum.TypRlnEtyMjrWthEtyMom typRlnEtyMjrWthEtyMom)
        {
            DetectSttAcsWthEtyMom_22090(fldCodOrg, etyCnt, namActn, typRlnEtyMjrWthEtyMom);
        }

        /// <summary>
        /// برای مشخص کردن سازمانی که دسترسی در آن چک می شود
        /// </summary>
        /// <param name="fldCodOrg"></param>
        /// <param name="namActn"></param>
        /// <param name="namTypEtyMjr"></param>
        /// <param name="typEtyCnt"></param>
        /// <param name="nodMomEty"></param>
        public void DetectSttAcsWthEtyMom_22090(
            int fldCodOrg,
            string namActn,
            string namTypEtyMjr,
            Enum.AllTypEty typEtyCnt,
        params TblNod[] nodMomEty)
        {
            DetectSttAcsWthEtyMom_22090(fldCodOrg, null, namActn, null, typEtyCnt, namTypEtyMjr, nodMomEty);
        }


        /// <summary>
        /// برای مشخص کردن سازمانی که دسترسی در آن چک می شود     
        /// </summary>
        /// <param name="fldCodOrg">سازمانی که ناهمسانی ها در محیط آن سازمان بررسی می شود</param>
        /// <param name="etyCnt">موجودیت جاری</param>
        /// <param name="namActn">نام عمل مورد نظر</param>
        /// <param name="typRlnEtyMjrWthEtyMom">نوع رابطه موجودیت اصلی با موجودیت مادر</param>
        /// <param name="typEtyCnt">نوع موجودیت جاری</param>
        /// <param name="namTypEtyMjr">عنوان نوع موجودیت اصلی</param>
        /// <param name="nodMomEty">لیست گره های مادر موجودیت جاری</param>
        public void DetectSttAcsWthEtyMom_22090(
            int fldCodOrg,
            IAllEty etyCnt,
            string namActn,
            Enum.TypRlnEtyMjrWthEtyMom? typRlnEtyMjrWthEtyMom = null,
            Enum.AllTypEty? typEtyCnt = null,
            string namTypEtyMjr = null,
            params TblNod[] nodMomEty)
        {
            checkAcs(fldCodOrg, etyCnt, namActn, typRlnEtyMjrWthEtyMom, typEtyCnt, namTypEtyMjr, nodMomEty);

        }





        public void DetectSttAcsWthEtyMom_22088(
            IAllEty etyCnt,
            string namActn,
            Enum.TypRlnEtyMjrWthEtyMom typRlnEtyMjrWthEtyMom)
        {
            DetectSttAcsWthEtyMom_22088(etyCnt, namActn, typRlnEtyMjrWthEtyMom);
        }



        public void DetectSttAcsWthEtyMom_22088(
            string namActn,
            string namTypEtyMjr,
            Enum.AllTypEty typEtyCnt,
        params TblNod[] nodMomEty)
        {
            DetectSttAcsWthEtyMom_22088(null, namActn, null, typEtyCnt, namTypEtyMjr, nodMomEty);
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="etyCnt">موجودیت جاری</param>
        /// <param name="namActn">نام عمل مورد نظر</param>
        /// <param name="typRlnEtyMjrWthEtyMom">نوع رابطه موجودیت اصلی با موجودیت مادر</param>
        /// <param name="typEtyCnt">نوع موجودیت جاری</param>
        /// <param name="namTypEtyMjr">عنوان نوع موجودیت اصلی</param>
        /// <param name="nodMomEty">لیست گره های مادر موجودیت جاری</param>
        public void DetectSttAcsWthEtyMom_22088(
            IAllEty etyCnt,
            string namActn,
            Enum.TypRlnEtyMjrWthEtyMom? typRlnEtyMjrWthEtyMom = null,
            Enum.AllTypEty? typEtyCnt = null,
            string namTypEtyMjr = null,
            params TblNod[] nodMomEty)
        {
            checkAcs(PublicMethods.CurrentUser.FldCodOrg, etyCnt, namActn, typRlnEtyMjrWthEtyMom, typEtyCnt, namTypEtyMjr, nodMomEty);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fldCodOrg">سازمانی که ناهمسانی ها در محیط آن سازمان بررسی می شود</param>
        /// <param name="etyCnt">موجودیت جاری</param>
        /// <param name="namActn">نام عمل مورد نظر</param>
        /// <param name="typRlnEtyMjrWthEtyMom">نوع رابطه موجودیت اصلی با موجودیت مادر</param>
        /// <param name="typEtyCnt">نوع موجودیت جاری</param>
        /// <param name="namTypEtyMjr">عنوان نوع موجودیت اصلی</param>
        /// <param name="nodMomEty">لیست گره های مادر موجودیت جاری</param>
        /// <returns></returns>
        private Enum.AllTypEty? checkAcs(int fldCodOrg, IAllEty etyCnt, string namActn, Enum.TypRlnEtyMjrWthEtyMom? typRlnEtyMjrWthEtyMom, Enum.AllTypEty? typEtyCnt, string namTypEtyMjr, TblNod[] nodMomEty)
        {

            if (PublicMethods.AllItmAcs != null)
            {
                List<TblNod> nodMom;

                if (etyCnt != null)
                {
                    nodMom = FindEtyMom_22100(etyCnt, typRlnEtyMjrWthEtyMom);

                    using (BPMNDBEntities context = new BPMNDBEntities())
                    {
                        typEtyCnt = DetectTypeOfEty_22105(etyCnt);
                    }
                }

                else
                {
                    nodMom = new List<TblNod>();

                    nodMom.AddRange(nodMomEty);
                }

                List<TblNod> nodWthAcs = new List<TblNod>();

                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    //22111
                    string str = namTypEtyMjr != null ? namTypEtyMjr : typEtyCnt.ToString();
                    List<TblItmAc> itmAcs = PublicMethods.AllItmAcs.Where(m => m.FldNamActn == namActn && m.FldNamTypEtyMjr == str).ToList();

                    //22115
                    foreach (var itmAcs_i in itmAcs)
                    {
                        List<TblLvlAc> lvlAcs = itmAcs_i.TblLvlAcs.ToList();

                        //22091
                        foreach (TblLvlAc lvlAcs_i in lvlAcs)
                        {
                            List<TblAgntNod> agntNod = lvlAcs_i.TblAgntNods.Where(m => m.TblNod.EtyNod.Org.FldCodOrg == fldCodOrg).ToList();

                            //22092
                            foreach (TblAgntNod agntNod_i in agntNod)
                            {
                                //22097
                                if (agntNod_i.TblPsn.FldCodPsn == PublicMethods.CurrentUser.TblPsn.FldCodPsn)
                                {
                                    nodWthAcs.AddRange(agntNod_i.TblNod.EtyNod.DetectSubNod_22116(agntNod_i.FldDmnAgnt).Where(m => m != null && m.FldCodTypEty == itmAcs_i.FldCodTypEtyMom).ToList());

                                    //nodWthAcs = nodWthAcs.Where(m => nodMom.FirstOrDefault(x => x.FldCodTypEty == m.FldCodTypEty) != null).ToList();
                                }
                            }
                        }
                    }

                    //22095, 22093
                    List<TblNod> nod = new List<TblNod>();
                    nodMom.ForEach((m) =>
                    {
                        if (m != null)
                        {
                            nod.AddRange(nodWthAcs.Where(n => n.FldCodNod == m.FldCodNod));
                        }
                        //m != null && 
                    });
                    HasAcs[itmAcs.First().FldNamActn + itmAcs.First().FldNamTypEtyMjr] = nod.Count() > 0;

                    if (itmAcs.First().FldNamActn == "View")
                    {
                        TblLog log = new TblLog()
                        {
                            FldCodUsr = PublicMethods.CurrentUser.FldCodUsr,
                            FldDteLog = DateTime.Now,
                            FldTypLog = (int)Enum.TypLog.Access,
                            FldActnImpEed = itmAcs.First().FldNamActn + itmAcs.First().FldNamTypEtyMjr + (nod.Count() > 0 ? "-Ok" : "-NotOk"),
                            FLdCodEty = etyCnt != null ? etyCnt.CodEty : nodMomEty.First().FldCodNod,
                            FldCodTypEty = etyCnt != null ? (int)DetectTypeOfEty_22105(etyCnt) : typEtyCnt == null ? default(int?) : (int)typEtyCnt
                        };

                        context.TblLogs.AddObject(log);

                        PublicMethods.SaveContext(context);
                    }
                }
            }
            return typEtyCnt;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<TblNod> FindEtyMom_22100(IAllEty ety, Enum.TypRlnEtyMjrWthEtyMom? typRlnEtyMjrWthEtyMom)
        {
            List<TblNod> nod = new List<TblNod>();

            if (typRlnEtyMjrWthEtyMom != null)
            {
                if (ety is TblAct)
                {
                    if (typRlnEtyMjrWthEtyMom == Enum.TypRlnEtyMjrWthEtyMom.PerformerOfActivity)
                    {
                        nod.Add(((TblAct)ety).TblNod);
                    }
                }
                else if (ety is TblNod)
                {
                    nod.Add(ety as TblNod);
                }

                //else if (ety is TblPosPstOrg)
                //{
                //    nod.Add(((TblPosPstOrg)ety).Nod);
                //}

                else if (ety is TblPr)
                {
                    if (typRlnEtyMjrWthEtyMom == Enum.TypRlnEtyMjrWthEtyMom.OwnerProcess)
                    {
                        nod.Add(((TblPr)ety).TblNod);
                    }

                    else if (typRlnEtyMjrWthEtyMom == Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess)
                    {
                        ((TblPr)ety).Acts.ForEach(m => nod.Add(m.TblNod));
                    }
                }

                else if (ety is TblOrg)
                {
                    if (typRlnEtyMjrWthEtyMom == Enum.TypRlnEtyMjrWthEtyMom.PosPstOfOrgAndCurrentOrg)
                    {
                        nod.Add(((TblOrg)ety).Nod);

                        ((TblOrg)ety).TblPosPstOrgs.ToList().ForEach(m => nod.Add(m.Nod));
                    }

                    else if (typRlnEtyMjrWthEtyMom == Enum.TypRlnEtyMjrWthEtyMom.OrgCntAndOrgSub)
                    {
                        nod.Add(((TblOrg)ety).Nod);

                        nod.AddRange(((TblOrg)ety).DetectSubNod_22116(100));
                    }
                }
            }

            return nod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lst"></param>
        public static void ExeAcsView(BPMNDBEntities context, List<dynamic> lst)
        {
            List<TblItmAc> itmAcs = context.TblItmAcs.Where(m => m.FldNamTypEtyMjr == DetectTypeOfEty_22105(lst).ToString() && m.FldNamActn == "View").ToList();

            List<TblLvlAc> lvlAcs = itmAcs.First().TblLvlAcs.ToList();

            List<TblNod> nod = new List<TblNod>();
            lvlAcs.ForEach(m => m.TblAgntNods.Where(n => n.TblPsn == PublicMethods.CurrentUser.TblPsn).ToList().ForEach(k => nod.Add(k.TblNod)));

            List<int> i = new List<int>();
            nod.ForEach(m => i.Add(m.FldCodNod));
            Filter(lst, DetectTypeOfFilter(lst), i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private static Enum.AllTypEty DetectTypeOfEty_22105(List<dynamic> lst)
        {
            if (lst.GetType() == typeof(List<TblOrg>))
            {
                return Enum.AllTypEty.Org;
            }

            else if (lst.GetType() == typeof(List<TblPosPstOrg>))
            {
                return Enum.AllTypEty.Pos;
            }

            else if (lst.GetType() == typeof(List<TblRol>))
            {
                return Enum.AllTypEty.Rol;
            }

            else if (lst.GetType() == typeof(List<TblPsn>))
            {
                return Enum.AllTypEty.Psn;
            }

            else if (lst.GetType() == typeof(List<TblAct>))
            {
                return Enum.AllTypEty.Act;
            }

            else if (lst.GetType() == typeof(List<TblPr>))
            {
                return Enum.AllTypEty.Prs;
            }

            return Enum.AllTypEty.Org;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private static Enum.AllTypEty DetectTypeOfEty_22105(IAllEty ety)
        {

            if (ety is TblNod)
            {
                if ((ety as TblNod).FldCodTypEty == (int)Enum.FldTypEty.PosPst)
                {
                    return Enum.AllTypEty.Pos;
                }
                if ((ety as TblNod).FldCodTypEty == (int)Enum.FldTypEty.Rol)
                {
                    return Enum.AllTypEty.Rol;
                }
            }

            if (ety is TblOrg)
            {
                return Enum.AllTypEty.Org;
            }

            else if (ety is TblPosPstOrg)
            {
                return Enum.AllTypEty.Pos;
            }

            else if (ety is TblRol)
            {
                return Enum.AllTypEty.Rol;
            }

            else if (ety is TblPsn)
            {
                return Enum.AllTypEty.Psn;
            }

            else if (ety is TblAct)
            {
                return Enum.AllTypEty.Act;
            }

            else if (ety is TblPr)
            {
                return Enum.AllTypEty.Prs;
            }

            return Enum.AllTypEty.Org;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private static Enum.TypFlt DetectTypeOfFilter(List<dynamic> lst)
        {
            if (lst.GetType() == typeof(List<TblOrg>))
            {

            }

            else if (lst.GetType() == typeof(List<TblPosPstOrg>))
            {

            }

            else if (lst.GetType() == typeof(List<TblRol>))
            {

            }

            else if (lst.GetType() == typeof(List<TblPsn>))
            {

            }

            else if (lst.GetType() == typeof(List<TblAct>))
            {
                return Enum.TypFlt.ActBsoPfr;
            }

            else if (lst.GetType() == typeof(List<TblPr>))
            {
                return Enum.TypFlt.PrsBsoOwr;
            }

            return Enum.TypFlt.ActBsoPfr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="codEtyBse">شناسه موجودیت مبنا که بر اساس آن فیلتر انجام می شود</param>
        /// <param name="typFlt"></param>
        public static void Filter(List<dynamic> lst, Enum.TypFlt typFlt, params object[] codNodMom)
        {
            switch (typFlt)
            {
                case SSYM.OrgDsn.Model.Enum.TypFlt.ActBsoPfr:

                    List<dynamic> act = new List<dynamic>();

                    foreach (var item in codNodMom)
                    {
                        act.AddRange(lst.Where(m => m.FldCodNod == (int)item));
                    }

                    lst = act;

                    break;

                case SSYM.OrgDsn.Model.Enum.TypFlt.PrsBsoOwr:

                    List<dynamic> prs = new List<dynamic>();

                    foreach (var item in codNodMom)
                    {
                        prs.AddRange(lst.Where(m => m.FldCodOwrPrs == (int)item));
                    }

                    lst = prs;

                    break;

                default:
                    break;
            }
        }

        Dictionary<string, bool> hasAcs;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, bool> HasAcs
        {
            get
            {
                return hasAcs;
            }
            set { hasAcs = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionKey"></param>
        /// <returns></returns>
        public bool this[string permissionKey]
        {
            get
            {
                return HasAcs[permissionKey];
            }
            set
            {
                HasAcs[permissionKey] = value;
            }
        }
    }
}
