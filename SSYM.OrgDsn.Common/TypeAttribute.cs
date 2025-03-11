using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Common
{
    public class TypeAttribute : Attribute
    {
        public Type ValueType { get; set; }

        public TypeAttribute(Type type)
        {
            ValueType = type;
        }
    }
}
