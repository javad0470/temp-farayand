using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Media;
using yWorks.yFiles.UI.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.Model
{
    public partial class TblCvsn
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="srcNod"></param>
        /// <param name="dstNod"></param>
        /// <param name="codEty"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static TblCvsn GetCvsn(BPMNDBEntities context, TblNod srcNod, TblNod dstNod, int codEty, TypEtyForCvsn typ)
        {
            return context.TblCvsns.SingleOrDefault(m => m.FldCodNodSrc == srcNod.FldCodNod && m.FldCodNodDst == dstNod.FldCodNod && m.FldTypEty == (int)typ && m.FldCodEty == codEty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="srcNod"></param>
        /// <param name="dstNod"></param>
        /// <param name="codEty"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public static TblCvsn CreateCvsn(BPMNDBEntities context, TblNod srcNod, TblNod dstNod, int codEty, TypEtyForCvsn typ)
        {
            TblCvsn cvsn = new TblCvsn()
                {
                    FldCodNodSrc = srcNod.FldCodNod,
                    FldCodNodDst = dstNod.FldCodNod,
                    FldCodEty = codEty,
                    FldTypEty = (int)typ
                };

            context.TblCvsns.AddObject(cvsn);

            return cvsn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cvsn"></param>
        /// <returns></returns>
        public static List<TblCvsnDtl> GetDtls(BPMNDBEntities context, TblCvsn cvsn)
        {
            return context.TblCvsnDtls.Where(m => m.FldCodCvsn == cvsn.FldCodCvsn).ToList();
        }

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion
    }
}
