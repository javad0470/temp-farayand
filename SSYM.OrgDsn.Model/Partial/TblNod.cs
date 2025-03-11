using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MindFusion.Diagramming.Wpf.Lanes;
using yWorks.yFiles.UI.Model;
using SSYM.OrgDsn.Model.Base;
using System.Data.Objects.DataClasses;

namespace SSYM.OrgDsn.Model
{
    class NodeComparer : IEqualityComparer<TblNod>
    {
        public bool Equals(TblNod x, TblNod y)
        {
            return x.FldCodNod == y.FldCodNod;
        }

        public int GetHashCode(TblNod obj)
        {
            return obj.GetHashCode();
        }
    }

    public partial class TblNod : IAllEty
    {
        public TblNod()
        {

        }

        IEtyNod etyNod;

        public IEtyNod EtyNod
        {
            get
            {
                if (etyNod == null)
                {
                    etyNod = DetectEtyOfNod_1082((BPMNDBEntities)this.GetContext(), this);
                    if (etyNod == null)
                    {

                    }

                    return etyNod;
                }

                return etyNod;
            }
        }

        string fldNamNod;

        public string FldNamNod
        {
            get
            {
                if (fldNamNod == string.Empty || fldNamNod == null)
                {
                    fldNamNod = Model.PublicMethods.PerformerName_950(this.FldCodEty, this.FldCodTypEty);
                    return fldNamNod;
                }

                return fldNamNod;
            }
        }

        string fldTtlNod;

        public string FldTtlNod
        {
            get
            {
                if (fldTtlNod == null)
                {
                    fldTtlNod = PublicMethods.PstPosNodTitle(this.FldCodEty, this.FldCodTypEty);
                }

                return fldTtlNod;
            }
        }

        bool isNodOsdOrg;

        /// <summary>
        /// آیا این گره یک گره برون سازمانی است؟
        /// </summary>
        public bool IsNodOsdOrg
        {
            get
            {
                if (isNodOsdOrg == null)
                {
                    return this.IsNodOsdOrg_1085((BPMNDBEntities)this.GetContext(), this, PublicMethods.CurrentUser.TblOrg);
                }

                return isNodOsdOrg;
            }
        }

        /// <summary>
        /// تمامی ورودی های فرستاده شده به گره
        /// </summary>
        public List<Tuple<TblObj, TblEvtSrt>> AllObjSentToNod
        {
            get
            {
                return DetectAllObjSentToNod();
            }
        }

        /// <summary>
        /// لیست تمام ورودی های ارسال شده برای گره جاری را نشان می دهد
        /// </summary>
        /// <returns></returns>
        private List<Tuple<TblObj, TblEvtSrt>> DetectAllObjSentToNod()
        {
            List<Tuple<TblObj, TblEvtSrt>> lst = new List<Tuple<TblObj, TblEvtSrt>>();

            foreach (TblAct item in this.TblActs)
            {
                foreach (Tuple<TblObj, TblEvtSrt> item1 in item.AllObjSentToAct)
                {
                    if (!lst.Contains(item1))
                    {
                        lst.Add(item1);
                    }
                }
            }

            return lst;
        }

        /// <summary>
        /// تمامی اشیای نتیجه فرستاده شده به گره
        /// </summary>
        public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentToNod
        {
            get
            {
                return DetectAllObjRstSentToNod();
            }
        }

        /// <summary>
        /// لیست تمام اشیای نتیجه ارسال شده برای گره جاری را نشان می دهد
        /// </summary>
        /// <returns></returns>
        private List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentToNod()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (TblAct item in this.TblActs)
            {
                var items = item.AllObjRstSentToAct;
                foreach (Tuple<IObjRst, TblEvtSrt> item1 in items)
                {
                    if (!lst.Contains(item1))
                    {
                        lst.Add(item1);
                    }
                }
            }

            return lst;
        }

        /// <summary>
        /// تمامی اشیای نتیجه ارسال شده از گره
        /// </summary>
        public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentFromNod
        {
            get
            {
                return DetectAllObjRstSentFromNod();
            }
        }

        private List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentFromNod()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (TblAct item in this.TblActs)
            {
                foreach (Tuple<IObjRst, TblEvtSrt> item1 in item.AllObjRstSentFromAct)
                {
                    if (!lst.Contains(item1))
                    {
                        lst.Add(item1);
                    }
                }
            }

            return lst;
        }



        #region For yWork

        /// <summary>
        /// 
        /// </summary>
        public INode GrpNod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Table Tbl { get; set; }

        IRow col;

        public IRow Col
        {
            get { return col; }
            set
            {
                col = value;

                if (col != null)
                {
                    col.Table.AddLabel(col, this.FldNamNod);
                }
            }
        }

        #endregion

        #region forMindfusionDiagraming

        //public Header HdrNod { get; set; }

        //BPMNShapes.ShpBase shp;

        //public BPMNShapes.ShpBase Shp
        //{
        //    get { return shp; }
        //    set
        //    {
        //        shp = value;
        //        shp.Id = this.FldCodNod;
        //        shp.Text = this.FldNamNod;
        //    }
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static IEtyNod DetectEtyOfNod_1082(BPMNDBEntities context, TblNod nod)
        {
            if (nod.FldCodTypEty == (int)Enum.FldTypEty.Org)
            {
                return context.TblOrgs.SingleOrDefault(m => m.FldCodOrg == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.PosPst)
            {
                return context.TblPosPstOrgs.SingleOrDefault(m => m.FldCodPosPst == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.Psn)
            {
                return context.TblPsns.SingleOrDefault(m => m.FldCodPsn == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.Rol)
            {
                return context.TblRols.SingleOrDefault(m => m.FldCodRol == nod.FldCodEty);
            }

            return null;
        }

        /// <summary>
        /// Transfered
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <param name="orgCnt"></param>
        /// <returns></returns>
        private bool IsNodOsdOrg_1085(BPMNDBEntities context, TblNod nod, TblOrg orgCnt)
        {
            EntityObject obj = PublicMethods.DetectEtyOfNod_1082(context, nod);

            if (obj.GetType() == typeof(TblOrg))
            {
                return ((TblOrg)obj).FldCodOrg != orgCnt.FldCodOrg;
            }

            else if (obj.GetType() == typeof(TblPosPstOrg))
            {
                return ((TblPosPstOrg)obj).FldCodOrg != orgCnt.FldCodOrg;
            }

            else if (obj.GetType() == typeof(TblPsn))
            {
                TblPsn psn = (TblPsn)obj;

                foreach (TblUsr item in psn.TblUsrs)
                {
                    if (item.TblOrg.FldCodOrg == orgCnt.FldCodOrg && psn.FldIsdOrg)
                    {
                        return false;
                    }
                }

                return true;
            }

            else if (obj.GetType() == typeof(TblRol))
            {
                TblRol rol = (TblRol)obj;

                if (rol.FldCodOrg == orgCnt.FldCodOrg && rol.FldIsdOrg)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public int CodEty
        {
            get { return this.FldCodNod; }
        }



        /// <summary>
        /// شناسایی گره های دارای نمایندگی یک شخص در یک سازمان
        /// </summary>
        /// <returns></returns>
        public static List<TblNod> GetNodOfPsnIsdOrg_22192(BPMNDBEntities context, int codPsn, int fldCodOrg)
        {
            TblPsn psn = context.TblPsns.Single(p => p.FldCodPsn == codPsn);

            //شناسایی تمام نمایندگی های شخص
            List<TblNod> agntOfPsn = new List<TblNod>();

            foreach (var agnt in psn.TblAgntNods)
            {
                if (agnt.TblNod.EtyNod.Org.FldCodOrg == fldCodOrg)
                {
                    agntOfPsn.Add(agnt.TblNod);
                }
            }

            return agntOfPsn;

            ////agntOfPsn.Where(a=>a.EtyNod.Org == 

            ////شناسایی تمام نمایندگی های یک سازمان اعم از نقش یا جایگاه و سمت و سازمان
            //List<TblNod> agntOfOrg = new List<TblNod>();

            //foreach (var rol in org.TblRols)
            //{
            //    agntOfOrg.Add(rol.Nod);
            //}

            //foreach (var posPst in org.TblPosPstOrgs)
            //{
            //    agntOfOrg.Add(posPst.Nod);
            //}


            //agntOfOrg.Add(org.Nod);

            //NodeComparer cmp = new NodeComparer();
            //List<TblNod> sharedNods = agntOfOrg.Intersect(agntOfPsn, cmp).ToList();

            //return sharedNods;
        }




        public Enum.AllTypEty CodTypEty
        {
            get
            {
                try
                {
                    return (this.EtyNod as IEtyNod).TypEty;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public string Name
        {
            get { return this.FldNamNod; }
        }
    }
}
