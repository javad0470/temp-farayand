using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public enum TypeOfElementInOrgChart
    {
        Organization=1,
        OrganizationalPosition=2,
        OrganizationalPost=3
    }

    public interface IOrgChart
    {
        List<IOrgChart> SubNodes { get; }

        /// <summary>
        ///برای چارت سازمانی
        /// </summary>
        string Name { get; set; }


        /// <summary>
        ///برای چارت سازمانی
        /// </summary>
        Base.TypeOfElementInOrgChart Type { get; }


        /// <summary>
        ///برای چارت سازمانی
        /// </summary>
        int OrganizationID { get; set; }

        void AddSubNode(IOrgChart subNode);


    }
}
