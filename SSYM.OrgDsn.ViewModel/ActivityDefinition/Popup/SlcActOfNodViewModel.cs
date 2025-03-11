using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Windows.Data;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcActOfNodViewModel : PopupViewModel
    {
        #region ' Fields '

        TblNod _nod;

        #endregion

        #region ' Initialaizer '

        public SlcActOfNodViewModel(BPMNDBEntities context, TblNod nod, bool showActUspf, int? excludedActCod = null,bool emptyAct=false)
            : base(context)
        {
            Nod = nod;

            if (!showActUspf)
            {
                if (excludedActCod.HasValue)
                {
                    Acts = new List<TblAct>(Nod.TblActs.Where(a => a.FldActUspf == false && a.FldCodAct != excludedActCod.Value));
                }
                else
                {
                    Acts = new List<TblAct>(Nod.TblActs.Where(a => a.FldActUspf == false));
                }
            }
            else
            {
                if (excludedActCod.HasValue)
                {
                    Acts = new List<TblAct>(Nod.TblActs.Where(a => a.FldCodAct != excludedActCod.Value));
                }
                else
                {
                    Acts = new List<TblAct>(Nod.TblActs);
                }
            }

            SelectedAct = Acts.FirstOrDefault();
            if(emptyAct)
            {
                SelectedAct = null;
            }
        }

        #endregion

        #region ' Properties / Commands '

        public TblNod Nod
        {
            get { return _nod; }
            set
            {
                _nod = value;
                RaisePropertyChanged("Nod");
            }
        }

        public List<TblAct> Acts { get; set; }

        TblAct _selectedAct;

        public TblAct SelectedAct
        {
            get { return _selectedAct; }
            set
            {
                _selectedAct = value;

                RaisePropertyChanged("SelectedAct");
                RaiseOKCanExecute();
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '


        protected override bool CanOKExecute()
        {
            return SelectedAct != null;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        protected override void OKExecute()
        {
            base.OKExecute();
        }
        #endregion
    }
}
