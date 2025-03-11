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
    public class SingleWayAwrViewModel : NotificationObject
    {
        #region ' Fields '
        TblAct _act;
        IWayAwr _wayAwr;

        #endregion

        #region ' Initialaizer '

        public SingleWayAwrViewModel(TblAct act, IWayAwr ety)
        {
            _act = act;
            _wayAwr = ety;
        }

        #endregion

        #region ' Properties / Commands '

        public CollectionViewSource EvtSrtsCV
        {
            get
            {
                List<TblEvtSrt> lst = new List<TblEvtSrt>();
                TblEvtSrt srt = null;
                if (_wayAwr is TblWayAwr_News)
                {
                    srt = _act.TblEvtSrts.SingleOrDefault(m => m.TblWayAwr_News.Contains(_wayAwr as TblWayAwr_News));
                }
                else if (_wayAwr is TblWayAwr_Oral)
                {
                    srt = _act.TblEvtSrts.SingleOrDefault(m => m.TblWayAwr_Oral.Contains(_wayAwr as TblWayAwr_Oral));
                }
                else if (_wayAwr is TblWayAwr_RecvInt)
                {
                    srt = _act.TblEvtSrts.SingleOrDefault(m => m.TblWayAwr_RecvInt.Contains(_wayAwr as TblWayAwr_RecvInt));
                }

                if (srt == null)
                {
                    return null;
                }

                srt.WayAwrs = new ObservableCollection<IWayAwr>();
                srt.WayAwrs.Add(_wayAwr);
                lst.Add(srt);

                CollectionViewSource evtSrtsCV = new CollectionViewSource();
                evtSrtsCV.Source = lst;
                return evtSrtsCV;
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
