using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public partial class TblItmAc
    {

        public List<TblAgntNod> AgntNod
        {
            get
            {
                List<TblAgntNod> lst = new List<TblAgntNod>();

                foreach (var item in this.TblLvlAcs)
                {
                    lst.AddRange(item.TblAgntNods);
                }

                return lst;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblLvlAc LvlAcsCnt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsChecked
        {
            get
            {
                if (LvlAcsCnt == null)
                {
                    return null;
                }
                return LvlAcsCnt.TblItmAcs.Where(m => m.FldCod == this.FldCod).Count() > 0;

                //return DetectSttOfCheck(this);
            }

            set
            {
                if (value != null && value.Value)
                {
                    CheckItmAcsChild(this);

                    CheckItmAcsParent(this);
                }

                else
                {
                    UnCheckItmAcsChild(this);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itmAcs"></param>
        /// <returns></returns>
        private bool? DetectSttOfCheck(TblItmAc itmAcs)
        {
            bool b = LvlAcsCnt.TblItmAcs.Where(m => m.FldCod == this.FldCod).Count() > 0;

            if (b)
            {
                foreach (TblItmAc item in itmAcs.TblItmAcs1)
                {
                    if (!DetectSttOfCheck(item).Value || DetectSttOfCheck(item) == null)
                    {
                        return null;
                    }
                }
            }

            return b;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="itmAcs"></param>
        private void CheckItmAcsChild(TblItmAc itmAcs)
        {
            this.LvlAcsCnt.TblItmAcs.Add(itmAcs);

            itmAcs.RaiseIsCheck();

            foreach (TblItmAc item in itmAcs.TblItmAcs1)
            {
                CheckItmAcsChild(item);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="itmAcs"></param>
        private void CheckItmAcsParent(TblItmAc itmAcs)
        {
            this.LvlAcsCnt.TblItmAcs.Add(itmAcs);

            itmAcs.RaiseIsCheck();

            if (itmAcs.TblItmAc1 != null)
            {
                CheckItmAcsParent(itmAcs.TblItmAc1);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="itmAcs"></param>
        private void UnCheckItmAcsChild(TblItmAc itmAcs)
        {
            //using (BPMNDBEntities context = new BPMNDBEntities())
            //{
            //    context.ExecuteStoreCommand("Delete From TblLvlAcs_ItmAcs where FldCodLvlAcs = {0} and FldCodItmAcs = {1}", this.LvlAcsCnt.FldCod, itmAcs.FldCod);
            //}

            this.LvlAcsCnt.TblItmAcs.Remove(itmAcs);

            itmAcs.RaiseIsCheck();

            foreach (TblItmAc item in itmAcs.TblItmAcs1)
            {
                UnCheckItmAcsChild(item);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="itmAcs"></param>
        //public void RaiseIsCheckForParents(TblItmAc itmAcs)
        //{
        //    itmAcs.RaiseIsCheck();

        //    if (itmAcs.TblItmAc1 != null)
        //    {
        //        RaiseIsCheckForParents(itmAcs.TblItmAc1);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        public void RaiseIsCheck()
        {
            OnPropertyChanged("IsChecked");
        }
    }
}
