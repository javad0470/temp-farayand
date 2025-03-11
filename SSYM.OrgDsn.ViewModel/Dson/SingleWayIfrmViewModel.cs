using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class SingleWayIfrmViewModel : NotificationObject
    {
        #region ' Fields '
        TblAct _act;
        IWayIfrm _wayIfrm;
        #endregion

        #region ' Initialaizer '

        public SingleWayIfrmViewModel(TblAct act, IWayIfrm wayIfrm)
        {
            _act = act;
            _wayIfrm = wayIfrm;
        }

        #endregion

        #region ' Properties / Commands '

        public CollectionViewSource EvtRstsCV
        {
            get
            {
                List<TblEvtRst> lst = new List<TblEvtRst>();
                TblEvtRst rst = null;
                if (_wayIfrm.ObjRst is TblNew)
                {
                    rst = _act.TblEvtRsts.SingleOrDefault(m => m.TblNews.Contains(_wayIfrm.ObjRst as TblNew));
                }
                else if (_wayIfrm.ObjRst is TblSbjOral)
                {
                    rst = _act.TblEvtRsts.SingleOrDefault(m => m.TblSbjOrals.Contains(_wayIfrm.ObjRst as TblSbjOral));

                }
                else if (_wayIfrm.ObjRst is TblObj)
                {
                    rst = _act.TblEvtRsts.SingleOrDefault(m => m.TblObjs.Contains(_wayIfrm.ObjRst as TblObj));
                }

                if (rst == null)
                {
                    return null;
                }

                rst.ObjRsts = new ObservableCollection<IObjRst>();
                rst.ObjRsts.Add(_wayIfrm.ObjRst);
                lst.Add(rst);
                CollectionViewSource evtRstsCV = new CollectionViewSource();
                evtRstsCV.Source = lst;
                return evtRstsCV;
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
