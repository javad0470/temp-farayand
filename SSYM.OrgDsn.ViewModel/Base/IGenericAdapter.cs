using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSYM.OrgDsn.ViewModel.Base;
namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IGenericAdapter<T>
    {
        IGenericViewModel<T> ViewModel { get; }
    }
}
