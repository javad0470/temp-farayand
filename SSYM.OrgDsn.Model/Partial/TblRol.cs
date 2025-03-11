using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.Model
{
    public partial class TblRol : Base.IOrgChart, IEtyNod, IAllEty
    {
        public TblRol()
        {
            NewlyAdded = false;
        }

        public bool NewlyAdded { get; set; }

        List<Base.IOrgChart> Base.IOrgChart.SubNodes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Base.TypeOfElementInOrgChart Type
        {
            get
            {
                throw new NotImplementedException();
            }
            //set;
            //{
            //    if (value == Base.TypeOfElementInOrgChart.OrganizationalPosition)
            //    {
            //        this.FldCodTyp = 1;
            //    }
            //    if (value == Base.TypeOfElementInOrgChart.OrganizationalPost)
            //    {
            //        this.FldCodTyp = 2;
            //    }

            //}
        }

        public int OrganizationID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void AddSubNode(Base.IOrgChart subNode)
        {
            throw new NotImplementedException();
        }


        public string Name
        {
            get { return this.FldTtlRol; }
            set
            {
                this.FldTtlRol = value;
                OnPropertyChanged("Name");
            }
        }


        public string MssnOrg
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public Enum.AllTypEty TypEty
        {
            get { return Enum.AllTypEty.Rol; }
        }


        public TblOrg Org
        {
            get { return this.TblOrg; }
        }


        public TblNod Nod
        {
            get { return PublicMethods.DetectNodOfRol_1742(this.GetContext<BPMNDBEntities>(), this); }
        }

        #region شناسایی گره های زیر مجموعه تا یک سطح معیین

        /// <summary>
        /// گره های زیر مجموعه گره جاری را تا یک سطح معین تعیین می کند
        /// </summary>
        /// <param name="tnoLvlSubNod">تعداد سطوح</param>
        /// <returns></returns>
        public List<TblNod> DetectSubNod_22116(int tnoLvlSubNod)
        {
            List<TblNod> nod = new List<TblNod>();

            nod.Add(this.Nod);

            return nod;
        }

        #endregion


        public int CodEty
        {
            get { return this.FldCodRol; }
        }


        /// <summary>
        /// نود های نقش جاری را بر میگرداند
        /// </summary>
        /// <returns></returns>
        public List<TblNod> GetSubNods(BPMNDBEntities context)
        {
            List<TblNod> nodes = new List<TblNod>();
            TblRol thisRol = context.TblRols.Single(m => m.FldCodRol == this.FldCodRol);
            foreach (var item in thisRol.TblPlyrRols)
            {
                nodes.Add(item.TblNod);
            }

            return nodes;
        }


        /// <summary>
        /// سازمان های وابسته نقش جاری را بر میگراند
        /// </summary>
        public List<TblOrg> DepOrg
        {
            get
            {
                TblOrg orgCurr = PublicMethods.CurrentUser.TblOrg;

                List<TblOrg> orgs = new List<TblOrg>();
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    List<TblNod> nods = GetSubNods(context);

                    foreach (var nod in nods)
                    {
                        if (nod.FldCodTypEty == (int)FldTypEty.Org)
                        {
                            TblOrg org = context.TblOrgs.Single(m => m.FldCodOrg == nod.FldCodEty);

                            if (orgCurr.GetSubOrgs().SingleOrDefault(m => m.FldCodOrg == org.FldCodOrg) != null)
                            {
                                orgs.Add(org);
                            }
                        }
                    }

                    return orgs;
                }
            }
        }



        /// <summary>
        /// جایگاه وسمت های نقش جاری را بر میگراند
        /// </summary>
        public List<TblPosPstOrg> PosPsts
        {
            get
            {
                List<TblPosPstOrg> posPsts = new List<TblPosPstOrg>();
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    List<TblNod> nods = GetSubNods(context);

                    foreach (var nod in nods)
                    {
                        if (nod.FldCodTypEty == (int)FldTypEty.PosPst)
                        {
                            TblPosPstOrg posPst = context.TblPosPstOrgs.Single(m => m.FldCodPosPst == nod.FldCodEty);
                            posPsts.Add(posPst);
                        }
                    }

                    return posPsts;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TblPsn> PsnOuters
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    List<TblPsn> psns = new List<TblPsn>();

                    TblRol thisRol = context.TblRols.Single(m => m.FldCodRol == this.FldCodRol);

                    List<TblNod> nods = GetSubNods(context);

                    foreach (var item in nods)
                    {
                        if (item.FldCodTypEty == (int)FldTypEty.Psn)
                        {
                            TblPsn psn = context.TblPsns.SingleOrDefault(m => m.FldCodPsn == item.FldCodEty && !m.FldIsdOrg);
                            if (psn != null)
                            {
                                psns.Add(psn);
                            }
                        }
                    }
                    return psns;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TblOrg> OrgOuters
        {
            get
            {
                TblOrg orgCurr = PublicMethods.CurrentUser.TblOrg;

                List<TblOrg> orgs = new List<TblOrg>();
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    List<TblNod> nods = GetSubNods(context);

                    foreach (var nod in nods)
                    {
                        if (nod.FldCodTypEty == (int)FldTypEty.Org)
                        {
                            TblOrg org = context.TblOrgs.Single(m => m.FldCodOrg == nod.FldCodEty);

                            if (orgCurr.GetSubOrgs().SingleOrDefault(m => m.FldCodOrg == org.FldCodOrg) == null)
                            {
                                orgs.Add(org);
                            }
                        }
                    }

                    return orgs;
                }
            }
        }


        bool _isSelected;

        /// <summary>
        /// از این پراپرتی در قسمت نقش های درون سازمانی استفاده شده است
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }


        public AllTypEty CodTypEty
        {
            get { return AllTypEty.Rol; }
        }

        public string NamTypEty
        {
            get
            {
                return SSYM.OrgDsn.Model.Enum.EnumUtil.NamNod(this.TypEty);
            }
        }


        #region ' Access '


        bool? _acs_EditRol;

        public bool Acs_EditRol
        {
            get
            {
                if (!_acs_EditRol.HasValue)
                {
                    if (PublicMethods.CurrentUser.AcsUsr.EditRolAllowedByOrg)
                    {
                        _acs_EditRol = true;
                    }
                    else
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
                        _acs_EditRol = PublicMethods.CurrentUser.AcsUsr["EditRolIsdOrg"] || this.NewlyAdded;
                    }
                }
                return _acs_EditRol.Value;
            }
        }


        bool? _acs_ViewAgntRol2;
        public bool Acs_ViewAgntRol2
        {
            get
            {
                if (!_acs_ViewAgntRol2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_ViewAgntRol2 = PublicMethods.CurrentUser.AcsUsr["ViewAgntRol2"] || this.NewlyAdded;
                }
                return _acs_ViewAgntRol2.Value;
            }
        }


        bool? _acs_AddAgntRol2;
        public bool Acs_AddAgntRol2
        {
            get
            {
                if (!_acs_AddAgntRol2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_AddAgntRol2 = PublicMethods.CurrentUser.AcsUsr["AddAgntRol2"] || this.NewlyAdded;
                }
                return _acs_AddAgntRol2.Value;
            }
        }


        bool? _acs_DelAgntRol2;
        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAgntRol2
        {
            get
            {
                if (!_acs_DelAgntRol2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Del", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_DelAgntRol2 = PublicMethods.CurrentUser.AcsUsr["DelAgntRol2"] || this.NewlyAdded;
                }
                return _acs_DelAgntRol2.Value;
            }
        }


        bool? _acs_EditAgntRol2;
        public bool Acs_EditAgntRol2
        {
            get
            {
                if (!_acs_EditAgntRol2.HasValue)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Edit", "AgntRol2", AllTypEty.Agnt, PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                    _acs_EditAgntRol2 = PublicMethods.CurrentUser.AcsUsr["EditAgntRol2"] || this.NewlyAdded;
                }
                return _acs_EditAgntRol2.Value;
            }
        }

        #endregion
    }
}
