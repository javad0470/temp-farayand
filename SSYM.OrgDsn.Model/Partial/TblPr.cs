using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model.Base;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model
{
    public partial class TblPr : IAllEty
    {
        //BPMNShapes.ShpBase shp;

        //public BPMNShapes.ShpBase Shp
        //{
        //    get { return shp; }
        //    set
        //    {
        //        shp = value;
        //        shp.Id = this.FldCodPrs;
        //        shp.Text = this.FldNamPrs;
        //    }
        //}


        INode shp;

        public INode Shp
        {
            get { return shp; }
            set
            {
                shp = value;
            }
        }

        private List<TblAct> _acts = null;
        public List<TblAct> Acts
        {
            get
            {
                if (_acts == null)
                    _acts = DetectActsOfPrs_946((BPMNDBEntities) this.GetContext(), this);
                return _acts;
            }
        }

        TblOrg org = null;

        /// <summary>
        /// سازمانی که فرآیند جاری در آن تعریف شده است
        /// </summary>
        public TblOrg Org
        {
            get
            {
                if (org == null)
                {
                    List<TblNod> nod = new List<TblNod>();
                    this.Acts.ForEach(m => nod.Add(m.TblNod));

                    List<TblOrg> orgWthMaxRepeat = new List<TblOrg>();

                    List<TblNod> lst = nod.Distinct().ToList();

                   // List<Tuple<long, object>> lst1 = new List<Tuple<long, object>>();

                    long l = 0;
                    Model.TblNod tempNod = null;
                    foreach (var item in lst)
                    {
                        long l1 = nod.LongCount(m => m == item);

                        //lst1.Add(new Tuple<long, object>(l1, item));

                        if (l1 > l)
                        {
                            l = l1;
                            tempNod = item;
                        }
                    }


                    //List<object> lst = new List<object>();
                    //nod.ForEach(m => lst.Add(m));
                   
                    //PublicMethods.DetectCptWthMaxRepeatInArray_21973((BPMNDBEntities)this.GetContext(), lst).ForEach(m => orgWthMaxRepeat.Add(((TblNod)m).EtyNod.Org));
                    //if (orgWthMaxRepeat.Count == 0)
                    //{
                        
                    //}
                    if(tempNod!=null)
                        org = tempNod.EtyNod.Org; //orgWthMaxRepeat.FirstOrDefault();
                }

                return org;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        private List<TblAct> DetectActsOfPrs_946(BPMNDBEntities context, TblPr prs)
        {
            PublicMethods.ReloadEntity(context, prs, prs.TblActPrs, "TblActPrs");

            List<TblAct> lst = new List<TblAct>();

            foreach (TblActPr item in prs.TblActPrs)
            {
                lst.Add(item.TblAct);
            }

            return lst;
        }



        public int CodEty
        {
            get { return this.FldCodPrs; }
        }


        public Enum.AllTypEty CodTypEty
        {
            get { return Enum.AllTypEty.Prs; }
        }

        public string Name
        {
            get
            {
                return this.FldNamPrs;
            }
           
        }
    }
}
