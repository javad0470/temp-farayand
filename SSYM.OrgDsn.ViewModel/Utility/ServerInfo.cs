using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSYM.OrgDsn.ViewModel
{
    public class ServerInfo
    {
        public string ServerName { get; set; }

        public string DBName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConStr { get; set; }

        public static ServerInfo ReadInfo()
        {
            try
            {
                BPMNDBEntities context = new BPMNDBEntities();


                string str = ConfigurationManager.ConnectionStrings["BPMNDBEntities"].ConnectionString;

                EntityConnectionStringBuilder e = new EntityConnectionStringBuilder(str);

                

                //str = str.Substring(str.IndexOf('"') + 1);

                //str = str.Substring(0, str.IndexOf('"'));

                var csb = new SqlConnectionStringBuilder(e.ProviderConnectionString);

                context.Dispose();
                //conStr.ElementInformation.

                ServerInfo result = new ServerInfo();

                result.ServerName = csb.DataSource;
                result.DBName = csb.InitialCatalog;
                result.UserName = csb.UserID;
                result.Password = csb.Password;

                result.ConStr = csb.ConnectionString;

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
