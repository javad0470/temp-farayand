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
    public class DtlVotOwrPrpsPrsViewModel : PopupViewModel
    {
        #region ' Fields '

        TblOwrPrpsPr owrPrpsPr;
        BPMNDBEntities context;

        #endregion

        #region ' Initialaizer '

        public DtlVotOwrPrpsPrsViewModel(BPMNDBEntities context)
        {
            this.context = context;
            this.CancelVisible = false;
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// مالک پیشنهادی فرآیند
        /// </summary>
        public TblOwrPrpsPr OwrPrpsPrs
        {
            get { return owrPrpsPr; }
            set
            {
                owrPrpsPr = value;
                RaisePropertyChanged("OwrPrpsPrs");
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
            context.LoadProperty(this.OwrPrpsPrs, "TblVotOwrPrps");

            List<object> lst = new List<object>();

            foreach (TblVotOwrPrp item in this.OwrPrpsPrs.TblVotOwrPrps)
            {
                var o = new
                {
                    item.TblNod.FldNamNod,
                    TnoActInPrs = PublicMethods.ComputeTnoActOfNodInnPrs_1606(context,
                    item.TblOwrPrpsPr.TblPr, item.TblNod),
                    VluVot = PublicMethods.ComputeVluVotNodForNamPrpsPrs_1590(context,
                    item.TblOwrPrpsPr.TblPr, item.TblNod)
                };

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
