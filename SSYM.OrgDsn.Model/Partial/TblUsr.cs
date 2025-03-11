using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public partial class TblUsr : IUser
    {
        BPMNDBEntities _ctx;

        public TblUsr()
        {
            ChangedUsername = string.Empty;
            this.AcsUsr = new Access.Acs();
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        public BPMNDBEntities Ctx
        {
            get
            {
                if (_ctx == null)
                {
                    _ctx = new BPMNDBEntities();
                }

                return _ctx;
            }
        }

        public int ID
        {
            get
            {
                return this.FldCodUsr;
            }
            set
            {
                this.FldCodUsr = value;
            }
        }

        /// <summary>
        /// لیست گره هایی که کاربر جاری نماینده آن ها است
        /// </summary>
        public List<TblNod> NodAgntEedByUsr
        {
            get
            {
                List<TblNod> nod = new List<TblNod>();

                foreach (var item in this.TblPsn.TblAgntNods)
                {
                    nod.Add(item.TblNod);
                }

                return nod;
            }
        }

        public string ChangedUsername { get; set; }
        public string Username
        {
            get
            {
                return this.FldNamUsr;
            }
            set
            {
                ChangedUsername = value;

                OnPropertyChanged("ChangedUsername");

                if (Ctx.TblUsrs.Any(u => u.FldCodUsr != this.FldCodUsr && u.FldNamUsr.Trim().ToLower() == value.Trim().ToLower()))
                {
                    throw new Exception("نام کاربری تکراری است.");
                }
                else
                {
                    this.FldNamUsr = value.Trim().ToLower();
                }
                OnPropertyChanged("Username");
            }
        }

        public Access.Acs AcsUsr { get; set; }

        /// <summary>
        /// معادل هر شخص نسبت داده شده به یک سازمان (کاربر) یک نماینده وجود دارد
        /// </summary>
        public TblAgntNod AgntNodEqualToUsr
        {
            get
            {
                return this.TblOrg.Nod.TblAgntNods.SingleOrDefault(m => m.FldCodPsn == this.FldCodPsn);
            }
        }


        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamOrg")
                {
                    return true;
                }
            }
            return false;
        }

        [NonSerialized]
        private DataErrorInfoSupport dataErrorInfoSupport;


        public string Error
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return null;
                }

                return dataErrorInfoSupport.Error;
            }
        }

        public string this[string memberName]
        {
            get
            {
                if (!shouldCheckErrors(memberName))
                {
                    return null;
                }

                return dataErrorInfoSupport[memberName];
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return;
            }

            dataErrorInfoSupport.RaiseErrorsChanged(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged += value;
                }
            }
            remove
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged -= value;
                }
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return null;
            }

            return dataErrorInfoSupport.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return false;
                }

                return dataErrorInfoSupport.HasErrors;
            }
        }

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

        #endregion

    }
}
