using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class ActLstViewModel : UserControlViewModel
    {
        #region ' Fields '

        //internal bool? RecentValue_Acs_ViewAct = null;
        BPMNDBEntities _context;
        TblNod _selectedNod;
        TblAct _selectedAct;
        bool lastCanceled = false;
        string _txtSrch;
        ListCollectionView _actListCV;


        #endregion

        #region ' Initialaizer '

        public ActLstViewModel(BPMNDBEntities ctx)
        {
            this._context = ctx;
            this.CanChangeAct = true;
        }

        #endregion

        #region ' Properties / Commands '

        public ListCollectionView ActListCV
        {
            get
            {
                if (ActList == null)
                {
                    return null;
                }
                if (_actListCV == null)
                {
                    _actListCV = new ListCollectionView(ActList);
                    _actListCV.Filter = new Predicate<object>(filterActs);
                }
                return _actListCV;
            }
            set { _actListCV = value; }
        }


        public TblNod SelectedNod
        {
            get { return _selectedNod; }
            set
            {
                _selectedNod = value;
                _actListCV = null;
                var acts = _context.TblActs.Where(a => a.FldCodNod == _selectedNod.FldCodNod);

                foreach (var item in acts)
                {
                    _context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
                }

                ActList = new ObservableCollection<TblAct>(_context.TblActs.Where(a => a.FldCodNod == _selectedNod.FldCodNod));
                RaisePropertyChanged("ActListCV", "Acs_ViewAct");
            }
        }


        public TblAct SelectedAct
        {
            get { return _selectedAct; }
            set
            {
                _selectedAct = value;
                RaisePropertyChanged("SelectedAct");
            }
        }

        public ObservableCollection<TblAct> ActList { get; set; }

        public bool Acs_ViewAct
        {
            get
            {
                if (this._selectedNod != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("View", "Act", AllTypEty.Act, this._selectedNod);

                    return PublicMethods.CurrentUser.AcsUsr["ViewAct"];
                    //RecentValue_Acs_ViewAct = 

                    //return RecentValue_Acs_ViewAct ?? true;
                }

                return true;
            }
        }

        public bool CanChangeAct { get; set; }

        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                if (_txtSrch != value)
                {
                    _txtSrch = value;
                    ActListCV.Refresh();
                }
            }
        }


        #endregion

        #region ' Public Methods '

        internal void RemoveActFromList(TblAct act)
        {
            this.ActList.Remove(this.ActList.Single(m => m.FldCodAct == act.FldCodAct));
            ActListCV.Refresh();
        }

        public bool CanChangeCurrentItem()
        {
            if (SelectedActChanging != null)
            {
                SelectedActChanging(this, null);

                return CanChangeAct;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region ' Private Methods '

        private void CurrentItemChanging(object sender, System.ComponentModel.CurrentChangingEventArgs e)
        {
            if (e.IsCancelable)
            {
                if (SelectedActChanging != null)
                {
                    SelectedActChanging(this, null);

                    lastCanceled = e.Cancel = !CanChangeAct;

                    CanChangeAct = true;
                }
                else
                {
                    lastCanceled = false;
                }
            }
            else
            {
                lastCanceled = false;
            }
        }

        private bool filterActs(object obj)
        {
            if (string.IsNullOrWhiteSpace(TxtSrch))
            {
                return true;
            }

            var act = obj as TblAct;

            if (act == null)
            {
                return true;
            }


            return act.Name.Trim().ToLower().Contains(TxtSrch);

        }


        #endregion

        #region ' Events '

        public event EventHandler SelectedActChanging;


        #endregion

    }

}
