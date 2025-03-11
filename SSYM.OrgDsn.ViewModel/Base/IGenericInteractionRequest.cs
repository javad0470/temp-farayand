using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IGenericInteractionRequest<T>
    {
        event EventHandler<GenericInteractionRequestEventArgs<T>> Raised;
    }
}
