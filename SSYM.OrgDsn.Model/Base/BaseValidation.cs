using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model.Base
{
    public class BaseValidation : IDataErrorInfo, INotifyDataErrorInfo
    {
        public BaseValidation(object obj)
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        [NonSerialized]
        private readonly DataErrorInfoSupport dataErrorInfoSupport;

        public string Error { get { return dataErrorInfoSupport.Error; } }

        public string this[string memberName]
        {
            get
            {
                return dataErrorInfoSupport[memberName];
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { dataErrorInfoSupport.ErrorsChanged += value; }
            remove { dataErrorInfoSupport.ErrorsChanged -= value; }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            dataErrorInfoSupport.RaiseErrorsChanged(propertyName);
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return dataErrorInfoSupport.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get
            {
                return dataErrorInfoSupport.HasErrors;
            }
        }

    }
}
