using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public interface ITblEror
    {

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNamEror { get; set; }
    }
    [MetadataType(typeof(ITblEror))]
    public partial class TblEror : INamedItm, ITblEror, IDataErrorInfo, INotifyDataErrorInfo
    {
        bool _isSelected;

        public TblEror()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }


        /// <summary>
        /// فعالیت هایی مر بوط به این خطا
        /// </summary>
        public List<string> ActNames
        {
            get
            {
                return this.TblEvtRsts.Select(m => m.TblAct.FldNamAct).ToList();
            }
        }



        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamEror")
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
            get { return this.FldNamEror; }
        }

        public string Type
        {
            get { return this.TblTypEror.FldTtlTypEror; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

    }
}
