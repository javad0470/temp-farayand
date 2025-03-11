using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SSYM.OrgDsn.ViewModel.Utility
{
    public class License
    {
        [XmlAttribute]
        public string NamOrg { set; get; }

        [XmlAttribute]
        public int TnoOrgSub { set; get; }

        [XmlAttribute]
        public int MaxTnoPosPst { set; get; }

        [XmlAttribute]
        public int TnoAct { set; get; }

        [XmlAttribute]
        public int TnoPrs { set; get; }

        [XmlAttribute]
        public int TnoUsr { set; get; }

        [XmlAttribute]
        public int TnoNod { set; get; }
    }

    //public class CodPrvOrg
    //{
    //    [XmlAttribute]
    //    public int FldCodPrvOrg { set; get; }

    //    [XmlAttribute]
    //    public string NamOrg { set; get; }

    //    [XmlAttribute]
    //    public int TnoOrgSub { set; get; }

    //    [XmlAttribute]
    //    public int MaxTnoPosPst { set; get; }

    //    [XmlAttribute]
    //    public int TnoAct { set; get; }

    //    [XmlAttribute]
    //    public int TnoPrs { set; get; }
    //}
}
