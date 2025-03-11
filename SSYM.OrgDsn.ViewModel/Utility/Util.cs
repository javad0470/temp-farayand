using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Reflection;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Utility;

namespace SSYM.OrgDsn.ViewModel
{
    public class Util
    {
        public static MessageBoxResult ShowMessageBox(int fldCodMsg)
        {
            TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == fldCodMsg);
            var typeMsg = (MessageBoxType)msg.FldTypMsg;


            if (typeMsg == MessageBoxType.Error
                || typeMsg == MessageBoxType.Information
                || typeMsg == MessageBoxType.Warning)
            {
                ShowNotification(fldCodMsg);
                return MessageBoxResult.OK;
            }


            return MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                (r) => { },
                null);

        }

        public static MessageBoxResult ShowMessageBox(int fldCodMsg, string placeHolder)
        {
            TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == fldCodMsg);

            var typeMsg = (MessageBoxType)msg.FldTypMsg;

            if (typeMsg == MessageBoxType.Error
                || typeMsg == MessageBoxType.Information
                || typeMsg == MessageBoxType.Warning)
            {
                ShowNotification(fldCodMsg, placeHolder);
                return MessageBoxResult.OK;
            }

            return MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
               string.Format(msg.FldTxtMsg, placeHolder), msg.FldTtlMsg, (MessageBoxType)msg.FldTypMsg, null),
                (r) => { },
                null);
        }

        public static void ShowNotification(int fldCodMsg, string placeHolder = null, bool autoHide = true, int hideAfter = 4000)
        {
            TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == fldCodMsg);
            string msgText = msg.FldTxtMsg;
            if (!string.IsNullOrWhiteSpace(placeHolder))
            {
                msgText = string.Format(msg.FldTxtMsg, placeHolder);
            }

            MenuViewModel.MainMenu.View.ShowNotification(msgText, (MessageBoxType)msg.FldTypMsg, autoHide, hideAfter);
        }

        public static void HideNotification()
        {
            MenuViewModel.MainMenu.View.HideNotification();
        }
        public static MessageBoxResult ShowPopup(Base.PopupViewModel dataContext)
        {
            return MenuViewModel.MainMenu.ShowPopup(dataContext);
        }

        public static bool HasContextChanges(BPMNDBEntities context)
        {
            return context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified).Count() > 0;
        }

        public static string GetNodTypeString(Model.Enum.FldTypEty typ)
        {
            switch (typ)
            {
                case SSYM.OrgDsn.Model.Enum.FldTypEty.Org:
                    return "سازمان";
                case SSYM.OrgDsn.Model.Enum.FldTypEty.PosPst:
                    return "جایگاه و سمت";
                case SSYM.OrgDsn.Model.Enum.FldTypEty.Psn:
                    return "شخص";
                case SSYM.OrgDsn.Model.Enum.FldTypEty.Rol:
                    return "نقش";
                default:
                    break;
            }
            return "";
        }

        public static TypVrsn VrsnTyp { get; set; }

        /// <summary>
        /// مسیر فایل اجرایی در صورتی که نسخه سرور باشد
        /// </summary>
        public static bool IsLocalPathExeVrsnSrv { get; set; }

        /// <summary>
        /// لایسنس ارائه شده در صورت معتبر بودن
        /// </summary>
        public static License LcsSfw { get; set; }

        /// <summary>
        /// آیا نسخه کلاینت روی سیستم جاری نصب شده است؟
        /// </summary>
        public static bool IsInslVrsnClntOnnSysCnt { get; set; }


        #region ' UI '

        public static Server connectLocalDefaultInstanceWA()
        {
            //Connect to the local, default instance of SQL Server. 
            Server srv;
            srv = new Server();
            //The connection is established when a property is requested. 
            return srv;
            //The connection is automatically disconnected when the Server variable goes out of scope.
        }


        public static Server connectRemoteInstanceWA(string strServer)
        {
            //Connect to a remote instance of SQL Server. 
            Server srv;
            //The strServer string variable contains the name of a remote instance of SQL Server. 
            srv = new Server(strServer);
            //The actual connection is made when a property is retrieved. 

            return srv;
            //The connection is automatically disconnected when the Server variable goes out of scope.
        }


        public static Server connectToInstanceOfSqlSA(
                String sqlServerLogin = "user_id",
                String password = "pwd",
                String instanceName = "instance_name",
                String remoteSvrName = "remote_server_name"
            )
        {


            //// Connecting to an instance of SQL Server using SQL Server Authentication
            //Server srv1 = new Server();   // connects to default instance
            //srv1.ConnectionContext.LoginSecure = false;   // set to true for Windows Authentication
            //srv1.ConnectionContext.Login = sqlServerLogin;
            //srv1.ConnectionContext.Password = password;
            //Console.WriteLine(srv1.Information.Version);   // connection is established

            //// Connecting to a named instance of SQL Server with SQL Server Authentication using ServerConnection
            //ServerConnection srvConn = new ServerConnection();
            //srvConn.ServerInstance = @".\" + instanceName;   // connects to named instance
            //srvConn.LoginSecure = false;   // set to true for Windows Authentication
            //srvConn.Login = sqlServerLogin;
            //srvConn.Password = password;
            //Server srv2 = new Server(srvConn);
            //Console.WriteLine(srv2.Information.Version);   // connection is established


            // For remote connection, remote server name / ServerInstance needs to be specified
            ServerConnection srvConn2 = new ServerConnection(string.Format("{0}\\{1}", remoteSvrName, instanceName));
            srvConn2.LoginSecure = false;
            srvConn2.Login = sqlServerLogin;
            srvConn2.Password = password;
            Server srv3 = new Server(srvConn2);
            return srv3;  // connection is established


        }

        public static void CreateDB(string dbName, Server srv)
        {
            var db = new Database(srv, dbName);
            db.Create();
        }

        public static void ExecuteScriptFile(string fileName, Server srv)
        {
            string script = File.ReadAllText(fileName);
            srv.ConnectionContext.ExecuteNonQuery(script);
        }

        public static void ExecuteScriptWithCurrentConnection(string script)
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                ctx.ExecuteStoreCommand(script);
            }
            //srv.ConnectionContext.ExecuteNonQuery(script);
        }




        public static string GenerateSerialNumber()
        {
            string result = string.Empty;
            System.Management.ManagementObjectSearcher managementObjectSearcher;

            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                result += GetPropertyDataValue(item.Properties["Model"]);
                result += "@*$$*@";
                break;
            }

            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                result += GetPropertyDataValue(item.Properties["ProcessorId"]);
                result += "@*$$*@";
                break;
            }

            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                result += GetPropertyDataValue(item.Properties["SerialNumber"]);
                result += "@*$$*@";
                break;
            }

            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                result += GetPropertyDataValue(item.Properties["Name"]);
                result += "@*$$*@";
                break;
            }

            return result;
        }

        public static string GetPropertyDataValue(System.Management.PropertyData propertyData)
        {
            if (propertyData.Value == null)
                return null;

            if (string.IsNullOrEmpty(propertyData.Value.ToString().Trim()))
                return null;

            return propertyData.Value.ToString();
        }

        public static void ShowExeption(System.Windows.Window parent, Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            MessageBox.Show(parent, string.Format("{0}\n{1}\n{2}", "خطایی رخ داده است:", ex.Message, ex.StackTrace), "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        public static bool isHidValid(string str)
        {
            string part1 = string.Empty;
            System.Management.ManagementObjectSearcher managementObjectSearcher;

            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                part1 += GetPropertyDataValue(item.Properties["Model"]);
                break;
            }

            string part2 = string.Empty;
            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                part2 += GetPropertyDataValue(item.Properties["ProcessorId"]);
                break;
            }

            string part3 = string.Empty;
            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                part3 += GetPropertyDataValue(item.Properties["SerialNumber"]);
                break;
            }


            string part4 = string.Empty;
            managementObjectSearcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (System.Management.ManagementObject item in managementObjectSearcher.Get())
            {
                part4 += GetPropertyDataValue(item.Properties["Name"]);
                break;
            }


            string[] arr = str.Split(new string[] { "@*$$*@" }, StringSplitOptions.RemoveEmptyEntries);

            bool b = false;
            int res = 0;

            if (arr[0] == part1)
            {
                res++;
            }

            if (arr[1] == part2)
            {
                res++;
            }

            if (arr[2] == part3)
            {
                res++;
            }

            if (arr[3] == part4)
            {
                res++;
            }

            if (res >= 2)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        public static bool calculateIsInslVrsnClntOnnSysCnt()
        {
            string hid = Util.GenerateSerialNumber();

            var res = CipherUtility.GetByHID(hid);

            if (res != null)
            {
                return true;
                //ViewModel.Util.IsInslVrsnClntOnnSysCnt = true;
            }
            else
            {
                return false;
                //ViewModel.Util.IsInslVrsnClntOnnSysCnt = false;
            }
        }

        public static bool IsRunningFromNetworkDrive()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var driveLetter = dir.First();
            if (!Char.IsLetter(driveLetter))
                return true;
            if (new DriveInfo(driveLetter.ToString()).DriveType == DriveType.Network)
                return true;
            return false;
        }


        public static bool chkTypVrsn(string loc)
        {


            var config = CipherUtility.getMyConfig(loc);

            switch (config[0])
            {
                case "SERVER":
                    ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.SERVER;
                    break;

                case "CLIENT":
                    if (!Util.isHidValid(config[1]))
                    {
                        return false;
                    }
                    ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.CLIENT;

                    break;

                case "COMPELETE":
                    ViewModel.Util.VrsnTyp = ViewModel.TypVrsn.COMPELETE;
                    break;
                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// بررسی می کند که نسخه جاری حتما بر روی یکی از سیستم های سازمان نصب شده باشد
        /// </summary>
        public static bool chkHidClntSrv()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return true;
            }

            using (SSYM.OrgDsn.Model.BPMNDBEntities ctx = new SSYM.OrgDsn.Model.BPMNDBEntities())
            {

                if (!(ViewModel.Util.VrsnTyp == SSYM.OrgDsn.ViewModel.TypVrsn.COMPELETE || ViewModel.Util.VrsnTyp == SSYM.OrgDsn.ViewModel.TypVrsn.SERVER)) //not server version
                {
                    var serial = Util.GenerateSerialNumber();
                    if (!ctx.TblInsOnnClnts.Any(c => c.FldHID == serial && c.FldSttInsl))
                    {

                        return false;
                    }
                }
                else// is server version
                {
                    if (ViewModel.Util.IsLocalPathExeVrsnSrv)
                    {
                        if (ctx.TblInsOnnSrvs.First().FldHidSrv != Util.GenerateSerialNumber())
                        {

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static void chgTypVrsnCmpltIffNotLocal()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                return;
            }

            if (ViewModel.Util.VrsnTyp == SSYM.OrgDsn.ViewModel.TypVrsn.COMPELETE
                && !ViewModel.Util.IsLocalPathExeVrsnSrv)
            {
                ViewModel.Util.VrsnTyp = SSYM.OrgDsn.ViewModel.TypVrsn.SERVER;
            }
        }


        public static TblInsOnnClnt getNextSerial()
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                var clnts = ctx.TblInsOnnClnts.ToList();
                return clnts.FirstOrDefault(c => !c.FldSttInsl && CipherUtility.IsSigValid(c));
            }

            //            using (SqlConnection con = new SqlConnection(_srv.ConnectionContext.ConnectionString))
            //            {
            //                using (SqlCommand cmd = new SqlCommand(
            //@"
            //SELECT *
            //  FROM [dbo].[TblInsOnnClnt]
            //
            //  where [FldSttInsl] = 0
            //"))
            //                {
            //                    cmd.Connection = con;

            //                    con.Open();

            //                    SqlDataReader reader = cmd.ExecuteReader();

            //                    while (reader.Read())
            //                    {
            //                        TblInsOnnClnt clnt = TblInsOnnClnt.CreateFromReader(reader);
            //                        if (clnt.IsSigValid())
            //                        {
            //                            return clnt;
            //                        }
            //                    }

            //                    return null;

            //                }
            //            }

            
        }

        public static int getCountSerialsLeft()
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                var clnts = ctx.TblInsOnnClnts.ToList();
                return clnts.Count(c => !c.FldSttInsl && CipherUtility.IsSigValid(c));
            }
        }


        public static void ConfirmAndRestartApp()
        {
            Util.ShowMessageBox(78);
            //if (Util.ShowMessageBox(78) == MessageBoxResult.Yes)
            //{
            //    System.Windows.Forms.Application.Restart();
            //    System.Windows.Application.Current.Shutdown();
            //}
        }

        public static bool chkSfwLcs()
        {
            License lcs = null;
            using (var ctx = new BPMNDBEntities())
            {
                var xmlHashed = ctx.TblInsOnnSrvs.FirstOrDefault().FlcCmnEncrpEed;
                var prvOrg = ctx.TblInsOnnSrvs.FirstOrDefault().FldCodPrvOrg.Value;
                var xml = ctx.TblInsOnnSrvs.FirstOrDefault().Fldlcs;
                const string publicKey = @"
                        
                        <RSAKeyValue>
            <Modulus>
                0kJjxj6vTgmHvJWp2A7DjOK6uJIldeEnzI3rDkpDCdOvi2WuZkb4NQ/xlb/sJJMr9OXLCXqhKmJCXsH+W/To3ncgvmpWeBLe8nJms7wla7gCYZe8d/4lJ9QXBIFbmEf594KiPjnzeSdCqIhw0sZm9BxDmHLquAwOLDdr8HiLs+U=
            </Modulus>	
            <Exponent>
                AQAB
            </Exponent>
            </RSAKeyValue>";
                lcs = LicenseGenerator.ReadLicense<License>(publicKey, xml);
                //var result = LicenseGenerator.ReadLicense<CodPrvOrg>(publicKey, xmlHashed);
                ;
                if (CipherUtility.CalculateMD5Hash(prvOrg + xml)!=xmlHashed)
                {
                    LcsSfw = null;
                    return false;

                }
            }
            LcsSfw = lcs;
            return true;
        }
        #endregion



    }

    public enum TypVrsn
    {
        SERVER,
        CLIENT,
        COMPELETE
    }
}
