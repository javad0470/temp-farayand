using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class TblWayIfrm_SndOut : IWayIfrm
    {
        #region ' Fields '

        private int fldCodActDst;
        private string fldNamActDst;
        private int fldCodNodDst;
        private string fldNamNodDst;

        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// شناسه فعالیت مقصد این نحوه آگاه سازی
        /// </summary>
        public int FldCodActDst
        {
            get
            {
                if (this.TblWayAwr_RecvInt != null)
                    return this.TblWayAwr_RecvInt.TblEvtSrt.FldCodAct;
                else
                    return 0;
            }
            //set { fldCodActDst = value; }
        }

        /// <summary>
        /// نام فعالیت مقصد این نحوه آگاه سازی
        /// </summary>
        public string FldNamActDst
        {
            get
            {
                if (this.TblWayAwr_RecvInt != null)
                    return this.TblWayAwr_RecvInt.TblEvtSrt.TblAct.FldNamAct;
                else
                    return null;
            }
            //set { fldNamActDst = value; }
        }

        /// <summary>
        /// شناسه مجری مقصد این نحوه آگاه سازی
        /// </summary>
        public int FldCodNodDst
        {
            get
            {
                if (this.TblWayAwr_RecvInt != null)
                    return this.TblWayAwr_RecvInt.TblEvtSrt.TblAct.TblNod.FldCodNod;
                else
                    return 0;
            }
            //set { fldCodNodDst = value; }
        }

        /// <summary>
        /// نام مجری مقصد این نحوه آگاه سازی
        /// </summary>
        public string FldNamNodDst
        {
            get
            {
                if (this.TblWayAwr_RecvInt != null)
                    return Model.PublicMethods.PerformerName_950(this.TblWayAwr_RecvInt.TblEvtSrt.TblAct.TblNod.FldCodEty, this.TblWayAwr_RecvInt.TblEvtSrt.TblAct.TblNod.FldCodTypEty);
                else
                    return null;
            }

            //set { fldNamNodDst = value; }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' events '

        #endregion


        public string Title
        {
            get { return this.TblObj.FldNamObj; }
        }

        public TypDson DsonType
        {
            get { return (TypDson)this.FldStsDsonWayIfrm; }
        }



        public int FldCod
        {
            get { return this.FldCodWayIfrm; }
        }


        public TypWayAwrIfrm FldTypEtyCnvs
        {
            get { return TypWayAwrIfrm.Obj; }
        }


        public TblAct ActSrc
        {
            get
            {
                return this.TblObj.TblEvtRst.TblAct;
            }

        }

        public TblAct ActDst
        {
            get
            {
                if (this.TblWayAwr_RecvInt != null && this.TblWayAwr_RecvInt.TblEvtSrt != null)
                {
                    return this.TblWayAwr_RecvInt.TblEvtSrt.TblAct;
                }

                return null;
            }

        }

        public bool IsDson
        {
            get;
            set;
        }

        public bool IsAdded
        {
            get;
            set;
        }

        public IObjRst ObjRst
        {
            get { return this.TblObj; }
        }

        public IWayAwr WayAwr
        {
            get { return this.TblWayAwr_RecvInt; }
        }

        public TypEtyForCvsn EtyForCvsnTyp
        {
            get { return TypEtyForCvsn.TblWayAwr_RecvInt; }
        }
    }
}
