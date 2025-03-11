using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model.CustomCollections
{
    public class RelatedActivities
    {
        #region ' Fields '

        private int fldCodActSrc;
        private int fldsttActSrc;
        private string fldNamActSrc;
        private int fldTypActSrc;
        private int fldActSubHavSrc;

        private int fldCodActDst;
        private int fldsttActDst;
        private string fldNamActDst;
        private int fldTypActDst;
        private int fldActSubHavDst;
        private int fldCodNodDst;
        private int fldCodEtyDst;
        private int fldCodTypEty;

        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        public int FldCodTypEty
        {
            get { return fldCodTypEty; }
            set { fldCodTypEty = value; }
        }

        public int FldCodNodDst
        {
            get { return fldCodNodDst; }
            set { fldCodNodDst = value; }
        }

        public int FldCodEtyDst
        {
            get { return fldCodEtyDst; }
            set { fldCodEtyDst = value; }
        }

        public int FldCodActDst
        {
            get { return fldCodActDst; }
            set { fldCodActDst = value; }
        }

        public int FldsttActDst
        {
            get { return fldsttActDst; }
            set { fldsttActDst = value; }
        }

        public string FldNamActDst
        {
            get { return fldNamActDst; }
            set { fldNamActDst = value; }
        }

        public int FldTypActDst
        {
            get { return fldTypActDst; }
            set { fldTypActDst = value; }
        }

        public int FldActSubHavDst
        {
            get { return fldActSubHavDst; }
            set { fldActSubHavDst = value; }
        }

        public int FldActSubHavSrc
        {
            get { return fldActSubHavSrc; }
            set { fldActSubHavSrc = value; }
        }

        public int FldTypActSrc
        {
            get { return fldTypActSrc; }
            set { fldTypActSrc = value; }
        }

        public string FldNamActSrc
        {
            get { return fldNamActSrc; }
            set { fldNamActSrc = value; }
        }

        public int FldSttActSrc
        {
            get { return fldsttActSrc; }
            set { fldsttActSrc = value; }
        }

        public int FldCodActSrc
        {
            get { return fldCodActSrc; }
            set { fldCodActSrc = value; }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' events '

        #endregion

    }
}
