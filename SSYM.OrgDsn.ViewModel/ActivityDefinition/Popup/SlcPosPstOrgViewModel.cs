using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcPosPstOrgViewModel : PopupViewModel
    {
        #region ' Fields '

        bool _isPosPostSelected;
        bool _isOrgSelected;
        IEtyNod _selectedNod;
        ObservableCollection<IEtyNod> _selectedItems;

        #endregion

        #region ' Initialaizer '

        public SlcPosPstOrgViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            IsOrgSelected = true;
            _selectedItems = new ObservableCollection<IEtyNod>();
        }

        #endregion

        #region ' Properties / Commands '

        public SlcOrgDepViewModel SlcOrgDepVM { get; set; }

        public SlcPstPosViewModel SlcPstPosVM { get; set; }

        public bool IsPosPostSelected
        {
            get { return _isPosPostSelected; }
            set
            {
                _isPosPostSelected = value;
                if (_isPosPostSelected)
                {
                    if (SlcPstPosVM == null)
                    {
                        SlcPstPosVM = new SlcPstPosViewModel(bpmnEty, isMultiSelect: true);// { IsMultiSelect = true };
                        SlcPstPosVM.TblPosPstSelectionChanged += SlcPstPosVM_TblPosPstSelectionChanged;
                        RaisePropertyChanged("SlcPstPosVM");
                    }
                }
                RaisePropertyChanged("IsPosPostSelected");
            }
        }



        public bool IsOrgSelected
        {
            get { return _isOrgSelected; }
            set
            {
                _isOrgSelected = value;

                if (_isOrgSelected)
                {
                    if (SlcOrgDepVM == null)
                    {
                        SlcOrgDepVM = new SlcOrgDepViewModel(bpmnEty);
                        SlcOrgDepVM.OrgSelectionChanged += SlcOrgDepVM_OrgSelectionChanged;
                        RaisePropertyChanged("SlcOrgDepVM");
                    }
                }

                RaisePropertyChanged("IsOrgSelected");
            }
        }


        public ObservableCollection<IEtyNod> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
            }
        }

        #endregion

        #region ' Public Methods '

        internal void SelectItems(List<IEtyNod> nods)
        {
            SelectedItems.Clear();

            SlcOrgDepVM.SelectItems(nods.Where(n => n is TblOrg).Select(n => n as TblOrg).ToList());

            if (SlcPstPosVM == null)
            {
                SlcPstPosVM = new SlcPstPosViewModel(bpmnEty, isMultiSelect: true);// { IsMultiSelect = true };
                SlcPstPosVM.TblPosPstSelectionChanged += SlcPstPosVM_TblPosPstSelectionChanged;
                RaisePropertyChanged("SlcPstPosVM");
            }
            SlcPstPosVM.SelectItems(nods.Where(n => n is TblPosPstOrg).Select(n => n as TblPosPstOrg).ToList());
        }

        #endregion

        #region ' Private Methods '
        void SlcOrgDepVM_OrgSelectionChanged(TblOrg obj)
        {
            if (obj.IsSelectedInTree)
            {
                if (!SelectedItems.Contains(obj))
                {
                    SelectedItems.Add(obj);
                }
            }
            else
            {
                SelectedItems.Remove(obj);
            }
        }

        void SlcPstPosVM_TblPosPstSelectionChanged(TblPosPstOrg obj)
        {
            if (obj.IsSelectedInTree)
            {
                if (!SelectedItems.Contains(obj))
                {
                    SelectedItems.Add(obj);
                }
            }
            else
            {
                SelectedItems.Remove(obj);
            }
        }

        protected override void CancelExecute()
        {
            base.CancelExecute();
        }

        protected override void OKExecute()
        {
            base.OKExecute();
        }
        #endregion

        #region ' Events '

        #endregion

    }
}
