using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcDstForOutViewModel : PopupViewModel
    {
        #region ' Fields '

        IObjRst objCnt;

        ObservableCollection<TblAct> _acs;



        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// شی نتیجه جاری
        /// </summary>
        public IObjRst ObjCnt
        {
            get { return objCnt; }
            set
            {
                objCnt = value;

                RaisePropertyChanged("ObjCnt", "Acs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblAct> Acs
        {
            get
            {
                if (ObjCnt == null)
                {
                    return null;
                }
                return new ObservableCollection<TblAct>(ObjCnt.ActTarget.Distinct());
            }
        }


        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion
    }
}
