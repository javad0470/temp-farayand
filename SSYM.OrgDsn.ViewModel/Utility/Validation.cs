using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel
{
    public class Validation
    {
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        public static extern bool StrongNameSignatureVerificationEx(
            string wszFilePath, bool fForceVerification, ref bool pfWasVerified);

        public static bool SigCheck()
        {
            var assembly = Assembly.GetExecutingAssembly();
            bool pfWasVerified = false;
            if (!StrongNameSignatureVerificationEx(assembly.Location, true, ref pfWasVerified))
            {
                //خاتمه برنامه در صورت عدم وجود امضای دیجیتال معتبر
                return false;
            }

            return true;
        }
    }
}
