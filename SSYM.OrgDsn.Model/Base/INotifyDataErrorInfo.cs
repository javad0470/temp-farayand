using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{

    public class DataErrorsChangedEventArgs : EventArgs
    {
        // Fields
        private readonly string propertyName;

        // Methods
        public DataErrorsChangedEventArgs(string propertyName)
        {
            this.propertyName = propertyName;
        }

        // Properties
        public virtual string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }
    }

    public interface INotifyDataErrorInfo
    {
        // Events
        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Methods
        IEnumerable GetErrors(string propertyName);

        // Properties
        bool HasErrors { get; }
    }
}
