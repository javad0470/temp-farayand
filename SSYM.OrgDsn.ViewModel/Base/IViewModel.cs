using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public interface IViewModel
    {
        bool ConfirmAndClose();
        void SaveContext();
    }
}
