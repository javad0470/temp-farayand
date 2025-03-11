using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSYM.OrgDsn.Model
{
    public interface ITblPsn
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNam1stPsn { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNam2ndPsn { get; set; }
    }


    [MetadataType(typeof(ITblPsn))]
    public partial class TblPsn : ITblPsn, IDataErrorInfo, INotifyDataErrorInfo, IEtyNod, IAllEty
    {
        public TblPsn()
        {
            this.FldSbjAct = 1;
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }


        public TblNod Nod
        {
            get { return PublicMethods.DetectNodOfPsn_1740(this.GetContext<BPMNDBEntities>(), this); }
        }


        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNam1stPsn" || property == "FldNam2ndPsn")
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

        #endregion

        public string Name
        {
            get
            {
                return this.FldNam2ndPsn;
            }

            set
            {
                throw new NotImplementedException();
            }
        }


        public Enum.AllTypEty TypEty
        {
            get { return Enum.AllTypEty.Psn; }
        }


        public TblOrg Org
        {
            get
            {
                if (this.TblUsrs.Count > 0)
                {
                    return this.TblUsrs.FirstOrDefault().TblOrg;
                }

                else
                {
                    return null;
                }

            }
        }

        public string TypAct
        {
            get
            {
                if (!this.FldTypAct.HasValue)
                {
                    return null;
                }

                TypActPsn actTyp = (TypActPsn)this.FldTypAct.Value;

                switch (actTyp)
                {
                    case TypActPsn.Producing:
                        return "تولیدی";
                    case TypActPsn.Commercial:
                        return "تجاری";
                    case TypActPsn.Peymankar:
                        return "پیمانکاری";
                    case TypActPsn.Servicing:
                        return "خدماتی";
                    case TypActPsn.Other:
                        return "سایر";
                    default:
                        break;
                }

                return null;
            }

        }

        List<TblOrg> orgsOfPsn;

        public List<TblOrg> OrgsOfPsn
        {
            get
            {
                if (orgsOfPsn == null)
                {
                    orgsOfPsn = new List<TblOrg>();

                    foreach (TblUsr item in this.TblUsrs)
                    {
                        orgsOfPsn.Add(item.TblOrg);
                    }
                }

                return orgsOfPsn;
            }

            set
            {
                orgsOfPsn = value;
            }
        }


        /// <summary>
        /// جایگاه،سمت و نقش هایی که این شخص نماینده آنهاست.
        /// </summary>
        public List<string> PosPstRol
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    return context.VwPosPstRolOfPsns.Where(m => m.FldCodPsn == this.FldCodPsn).Select(m => m.NamEty).ToList();
                }
            }
        }

        #region شناسایی گره های زیر مجموعه تا یک سطح معیین

        /// <summary>
        /// گره های زیر مجموعه گره جاری را تا یک سطح معین تعیین می کند
        /// </summary>
        /// <param name="tnoLvlSubNod">تعداد سطوح</param>
        /// <returns></returns>
        public List<TblNod> DetectSubNod_22116(int tnoLvlSubNod)
        {
            List<TblNod> nod = new List<TblNod>();

            nod.Add(this.Nod);

            return nod;
        }

        #endregion

        public int CodEty
        {
            get { return this.FldCodPsn; }
        }



        #region ' Static Methods '

        /// <summary>
        /// سازمان هایی شخص داده شده در آن ها عضو است
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        public static List<TblOrg> GetOrgsOfPsn_22160(TblPsn psn)
        {
            List<TblOrg> result = new List<TblOrg>();
            foreach (var usr in psn.TblUsrs)
            {
                result.Add(usr.TblOrg);
            }

            return result;
        }

        #endregion


        public AllTypEty CodTypEty
        {
            get { return AllTypEty.Psn; }
        }



        #region IsSelected

        bool _isSelected;

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected


        public string NamTypEty
        {
            get
            {
                return SSYM.OrgDsn.Model.Enum.EnumUtil.NamNod(this.TypEty);
            }
        }

    }
}
