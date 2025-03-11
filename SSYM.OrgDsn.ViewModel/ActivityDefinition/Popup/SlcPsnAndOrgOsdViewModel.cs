using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    /// <summary>
    /// انتخاب شخص و سازمان بیرونی
    /// </summary>
    public class SlcPsnAndOrgOsdViewModel : PopupViewModel
    {
        public SlcPsnAndOrgOsdViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            SlcOsdOrgVM = new SlcOrgOsdViewModel(ctx);
            SlcPsnOsdOrgVM = new SlcPsnOsdOrgViewModel(ctx);

            SlcOsdOrgVM.SelectionChanged += SlcOsdOrgVM_SelectionChanged;
            SlcPsnOsdOrgVM.SelectionChanged += SlcPsnOsdOrgVM_SelectionChanged;
        }

        void SlcPsnOsdOrgVM_SelectionChanged(TblPsn obj)
        {
            if (obj.IsSelected)
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

        void SlcOsdOrgVM_SelectionChanged(TblOrg obj)
        {
            if (obj.IsListSelected)
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

        private SlcOrgOsdViewModel _slcOsdOrgVM;
        private SlcPsnOsdOrgViewModel _slcPsnOsdOrgVM;

        internal void SelectItems(List<IEtyNod> nods)
        {
            if (SelectedItems == null)
            {
                SelectedItems = new ObservableCollection<IEtyNod>();
            }

            SelectedItems.Clear();

            SlcOsdOrgVM.SelectItems(nods.Where(n => n is TblOrg).Select(n => n as TblOrg).ToList());

            SlcPsnOsdOrgVM.SelectItems(nods.Where(n => n is TblPsn).Select(n => n as TblPsn).ToList());

        }

        public SlcOrgOsdViewModel SlcOsdOrgVM
        {
            get { return _slcOsdOrgVM; }
            set { _slcOsdOrgVM = value; }
        }

        public SlcPsnOsdOrgViewModel SlcPsnOsdOrgVM
        {
            get { return _slcPsnOsdOrgVM; }
            set { _slcPsnOsdOrgVM = value; }
        }

        public ObservableCollection<IEtyNod> SelectedItems { get; set; }
    }
}
