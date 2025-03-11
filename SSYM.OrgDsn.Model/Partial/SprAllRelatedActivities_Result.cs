using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.Model
{
     public partial class SprAllRelatedActivities_Result
    {

         public string FldNamNodSrc
         {
             get
             {
                 return Model.PublicMethods.ActivityPerformerName_951(this.FldCodActSrc ?? 0);
             }
         }
         public string FldNamNodDst
         {
             get
             {
                 return Model.PublicMethods.ActivityPerformerName_951(this.FldCodActDst ?? 0);
             }
         }
    }
}
