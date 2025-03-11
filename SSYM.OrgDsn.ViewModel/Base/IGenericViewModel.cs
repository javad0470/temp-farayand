using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IGenericViewModel<T> :  INotifyPropertyChanged , IGenericInteractionView<T>
    {
        T Entity { get; set; }
    }
}
