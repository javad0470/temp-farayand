using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Common
{
    public class ContextSaveException : Exception
    {
        public ContextSaveException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
