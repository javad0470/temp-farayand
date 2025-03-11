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
    public interface ITblUntMsrt
    {

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNamUntMsrt { get; set; }
    }
    [MetadataType(typeof(ITblUntMsrt))]
    public partial class TblUntMsrt : INamedItm, ITblUntMsrt, IDataErrorInfo, INotifyDataErrorInfo
    {
        public TblUntMsrt()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        public List<string> ActNames
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    TblUntMsrt unt = context.TblUntMsrts.Single(m => m.FldCodUntMsrt == this.FldCodUntMsrt);
                    var obj = unt.TblCdns.Where(m => m.TblEvtSrt != null);
                    return unt.TblCdns.Where(m => m.TblEvtSrt != null).Select(m => m.TblEvtSrt.TblAct.FldNamAct).ToList();
                }
            }
        }


        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamUntMsrt")
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
            get { return this.FldNamUntMsrt; }
        }


        public string Type
        {
            get { return this.TblSbjMsrt.FldNamSbjMsrt; }
        }

    }
}
