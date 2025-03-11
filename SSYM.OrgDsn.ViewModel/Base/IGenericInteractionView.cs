using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IGenericInteractionView<T>
    {
        void SetEntity(T entity);
        T GetEntity();
    }
}
