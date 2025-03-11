using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using SSYM.OrgDsn.Model;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace SSYM.OrgDsn.ViewModel
{

    public class CipherUtility
    {

        public static string pass = "&^%fngdfgn$%^^%^&##@fkdgbfg@#$%$^$&*&^(*)*(";
        public static string salt = "%^^&N^*&IBDFgr5756853GNR#$%IK<d23W#^%^&IKM<E&^^Y";



        public static string Encrypt<T>(string value, string password, string salt)
             where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateEncryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream())
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        writer.Write(value);
                    }
                }

                return Convert.ToBase64String(buffer.ToArray());
            }
        }

        public static string Decrypt<T>(string text, string password, string salt)
           where T : SymmetricAlgorithm, new()
        {
            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));

            SymmetricAlgorithm algorithm = new T();

            byte[] rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            byte[] rgbIV = rgb.GetBytes(algorithm.BlockSize >> 3);

            ICryptoTransform transform = algorithm.CreateDecryptor(rgbKey, rgbIV);

            using (MemoryStream buffer = new MemoryStream(Convert.FromBase64String(text)))
            {
                using (CryptoStream stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Unicode))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }


        //bool IsSigValid(TblInsOnnClnt clnt)
        //{
        //    try
        //    {
        //        string decrypted = CipherUtility.Decrypt<AesManaged>(clnt.FldSig, pass, salt);

        //        if (decrypted == string.Format("{0}{1}{2}", clnt.FldSeriInsl, clnt.FldSttInsl, clnt.FldAcvnInsl))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }


        /// <summary>
        /// اعتبار سنجی رشته وضعیت های نصب
        /// </summary>
        public static bool ValidateInstallState()
        {

            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                var clnts = ctx.TblInsOnnClnts.ToList();

                string clntsStr = "";

                foreach (var item in clnts)
                {
                    clntsStr += item.FldSttInsl == false ? "0" : "1";
                }

                return CipherUtility.CalculateMD5Hash(clntsStr) == ctx.TblInsOnnSrvs.First().FldCmnSttClnt;
            }
        }


        /// <summary>
        /// به روز رسانی رشته وضعیت های نصب
        /// </summary>
        public static void UpdateInstallState()
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                var clnts = ctx.TblInsOnnClnts.ToList();

                string clntsStr = "";

                foreach (var item in clnts)
                {
                    clntsStr += item.FldSttInsl == false ? "0" : "1";
                }

                clntsStr = CipherUtility.CalculateMD5Hash(clntsStr);

                var srv = ctx.TblInsOnnSrvs.First();

                srv.FldCmnSttClnt = clntsStr;

                PublicMethods.SaveContext(ctx);
            }
        }



        public static void unInstallCertificate()
        {
            // Use other store locations if your certificate is not in the current user store.
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite | OpenFlags.IncludeArchived);

            // You could also use a more specific find type such as X509FindType.FindByThumbprint
            X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindBySubjectName, "mys", false);

            foreach (var cert in col)
            {
                //Console.Out.WriteLine(cert.SubjectName.Name);

                // Remove the certificate
                store.Remove(cert);
            }
            store.Close();
        }

        public static void updateMyConfig(string val, string location)
        {
            val = string.Format("{0}@$$@{1}", val, Util.GenerateSerialNumber());

            val = CipherUtility.Encrypt<AesManaged>(val, CipherUtility.pass, CipherUtility.salt);

            System.IO.File.WriteAllText(Path.Combine(location, "myConfig.txt"), val);
        }

        public static string[] getMyConfig(string location)
        {
            string fileContent = System.IO.File.ReadAllText(Path.Combine(location, "myConfig.txt"));
            fileContent = CipherUtility.Decrypt<AesManaged>(fileContent, CipherUtility.pass, CipherUtility.salt);

            string[] file = fileContent.Split(new string[] { "@$$@" }, StringSplitOptions.RemoveEmptyEntries);

            //if (Util.isHidValid(file[1]))
            //{
            //    return file[0];
            //}
            return file;
        }

        public static bool IsSigValid(TblInsOnnClnt clnt)
        {
            try
            {
                string decrypted = CipherUtility.Decrypt<AesManaged>(clnt.FldSig, CipherUtility.pass, CipherUtility.salt);

                if (decrypted == string.Format("{0}{1}{2}", clnt.FldSeriInsl, clnt.FldSttInsl, clnt.FldAcvnInsl))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void SetAsUsed(string fldSeriInstall, string installPath)
        {
            string HId = Util.GenerateSerialNumber();

            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                TblInsOnnClnt clnt = ctx.TblInsOnnClnts.Single(c => c.FldSeriInsl == fldSeriInstall);
                clnt.FldInslPth = installPath;
                clnt.FldHID = Util.GenerateSerialNumber();
                clnt.FldSttInsl = true;
                string encrypted = CipherUtility.Encrypt<AesManaged>(string.Format("{0}{1}{2}", clnt.FldSeriInsl, clnt.FldSttInsl, clnt.FldAcvnInsl), CipherUtility.pass, CipherUtility.salt);
                clnt.FldSig = encrypted;
                PublicMethods.SaveContext(ctx);
            }
        }


        public static string SetAsUnused(string HID)
        {

            using (SSYM.OrgDsn.Model.BPMNDBEntities ctx = new Model.BPMNDBEntities())
            {
                var clnts = ctx.TblInsOnnClnts.ToList();

                SSYM.OrgDsn.Model.TblInsOnnClnt clnt = clnts.SingleOrDefault(c => c.FldHID != null && Util.isHidValid(c.FldHID));

                if (clnt == null)
                {
                    return null;
                }

                string encrypted = CipherUtility.Encrypt<AesManaged>(string.Format("{0}{1}{2}", clnt.FldSeriInsl, false, clnt.FldAcvnInsl), CipherUtility.pass, CipherUtility.salt);

                string path = clnt.FldInslPth;

                clnt.FldSig = encrypted;

                clnt.FldHID = null;

                clnt.FldInslPth = null;

                clnt.FldSttInsl = false;

                PublicMethods.SaveContext(ctx);
                

                return path;
            }

        }

        public static TblInsOnnClnt GetByHID(string HID)
        {
            using (SSYM.OrgDsn.Model.BPMNDBEntities ctx = new Model.BPMNDBEntities())
            {
                return ctx.TblInsOnnClnts.FirstOrDefault(c => c.FldHID == HID);
            }

            //    using (SqlConnection con = new SqlConnection(ServerInfo.ReadInfo().ConStr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            con.Open();

            //            cmd.Connection = con;

            //            cmd.CommandText = string.Format("select * from [dbo].[TblInsOnnClnt] ");//where [FldHID] = '{0}'  , HID

            //            var reader = cmd.ExecuteReader();

            //            TblInsOnnClnt clnt = null;

            //            while (reader.Read())
            //            {
            //                clnt = TblInsOnnClnt.CreateFromReader(reader);

            //                if (clnt != null)
            //                {
            //                    if (clnt.FldHID == null)
            //                    {
            //                        continue;
            //                    }

            //                    if (Util.isHidValid(clnt.FldHID))
            //                    {
            //                        reader.Close();
            //                        return clnt;
            //                    }
            //                }
            //            }

            //            if (clnt == null)
            //            {
            //                reader.Close();
            //                return null;
            //            }

            //            return null;
            //        }
            //    }


        }
    }
}
