using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public partial class TblLvlAc
    {
        BPMNDBEntities _ctx;


        public TblLvlAc()
        {

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

        /// <summary>
        /// نام آیتم دسترسی
        /// </summary>
        public string Name
        {
            get { return this.FldNam; }
            set
            {
                if (FldNam == "سطح دسترسی کامل")
                {
                    throw new Exception("این سطح دسترسی قابل ویرایش نیست.");
                }

                //_name = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    //this.Name = this.FldNam;
                    //OnPropertyChanged("Name");
                    throw new Exception("نام سطح دسترسی الزامی است.");
                }

                var nam = value.Trim();

                //if (nam == "سطح دسترسی جدید")
                //{
                //    throw new Exception("نام سطح دسترسی نامعتبر است.");
                //}

                if (Ctx.TblLvlAcs.Any(l => l.FldCod != this.FldCod && l.FldNam == nam))
                {
                    //this.Name = this.FldNam;
                    //OnPropertyChanged("Name");
                    throw new Exception("نام سطح دسترسی تکراری است.");
                }

                this.FldNam = nam;

                OnPropertyChanged("Name");
            }
        }

        public static string GenerateUniqueName(BPMNDBEntities context)
        {
            var names = context.TblLvlAcs.Select(l => l.FldNam).ToList();

            string baseName = "سطح دسترسی ";
            int i = 1;

            while (names.Contains(baseName + i.ToString()))
            {
                i++;
            }

            return baseName + i.ToString();
        }

        ~TblLvlAc()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
            }
        }
    }
}
