using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.Process.Popup
{
    public class DtlVotNamPrpsPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        TblNamPrpsPr namPrpsPrs;
        BPMNDBEntities _context;

        #endregion

        #region ' Initialaizer '

        public DtlVotNamPrpsPrsViewModel(BPMNDBEntities context)
        {
            this._context = context;
            this.CancelVisible = false;
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// نام پیشنهادی فرآیند
        /// </summary>
        public TblNamPrpsPr NamPrpsPrs
        {
            get { return namPrpsPrs; }
            set
            {
                namPrpsPrs = value;
                RaisePropertyChanged("NamPrpsPrs");
                DetectDtlVoter();
            }
        }

        public ObservableCollection<object> DtlVoters { get; set; }


        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        void DetectDtlVoter()
        {
            try
            {
                //MainWindowViewModel.MainContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, this.NamPrpsPrs.TblVotNamPrpsPrs);
                _context.LoadProperty(this.NamPrpsPrs, "TblVotNamPrpsPrs");
            }
            catch (Exception)
            {

            }

            List<object> lst = new List<object>();

            foreach (TblVotNamPrpsPr item in this.NamPrpsPrs.TblVotNamPrpsPrs)
            {
                var o = new { item.TblNod.FldNamNod, TnoActInPrs = PublicMethods.ComputeTnoActOfNodInnPrs_1606(_context, item.TblNamPrpsPr.TblPr, item.TblNod), VluVot = PublicMethods.ComputeVluVotNodForNamPrpsPrs_1590(_context, item.TblNamPrpsPr.TblPr, item.TblNod) };

                lst.Add(o);
            }

            DtlVoters = new ObservableCollection<object>(lst);

            RaisePropertyChanged("DtlVoters");
        }

        #endregion

        #region ' events '

        #endregion

    }
}
