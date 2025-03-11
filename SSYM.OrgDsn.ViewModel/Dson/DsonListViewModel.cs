using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class DsonListViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        ListCollectionView dsonsCV;
        Tuple<IWayAwrIfrm, TblNod> selectedDson;
        BPMNDBEntities context;

        string _dsonDescNod;
        string _dsonDescObj;
        #endregion

        #region ' Initialaizer '

        public DsonListViewModel()
        {
            context = new BPMNDBEntities();
            OrgPosVM = new PosPstRolViewModel(this.context);
            OrgPosVM.PropertyChanged += OrgPosVM_PropertyChanged;
            PrevCommand = new DelegateCommand(PrevCommandExecute, CanPrevCommand);
            NextCommand = new DelegateCommand(NextCommandExecute, CanNextCommand);
        }

        private bool CanNextCommand()
        {
            if (DsonsCV != null)
            {
                return DsonsCV.CurrentPosition < DsonsCV.Count - 1;
            }

            return false;
        }

        private void NextCommandExecute()
        {
            DsonsCV.MoveCurrentToNext();
            (NextCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        private bool CanPrevCommand()
        {
            if (DsonsCV != null)
            {
                return DsonsCV.CurrentPosition > 0;
            }

            return false;

        }

        private void PrevCommandExecute()
        {
            DsonsCV.MoveCurrentToPrevious();
            (PrevCommand as DelegateCommand).RaiseCanExecuteChanged();
        }


        #endregion

        #region ' Properties / Commands '

        public ICommand PrevCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public DsonOverviewViewModel DsonOverviewVM { get; set; }

        public PosPstRolViewModel OrgPosVM { get; set; }

        public DsonDtlViewModel SelectedDsonVM { get; set; }

        public Tuple<IWayAwrIfrm, TblNod> SelectedDson
        {
            get
            {
                return selectedDson;
            }
            set
            {
                if (value == null)
                {
                    DsonOverviewVM = null;
                    SelectedDsonVM = null;
                    DsonDescNod = null;
                    DsonDescObj = null;
                    RaisePropertyChanged("SelectedDsonVM", "DsonOverviewVM", "DsonDescObj", "DsonDescNod");
                    return;
                }
                if (selectedDson == value)
                {
                    return;
                }
                selectedDson = value;

                IWayAwrIfrm wayAwrIfrm = selectedDson.Item1 as IWayAwrIfrm;
                switch (wayAwrIfrm.DsonType)
                {
                    case SSYM.OrgDsn.Model.Enum.TypDson.NoDson:
                        break;
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInUnspcf:
                        SelectedDsonVM = new DsonDtlAssignedToMeViewModel(this.context, selectedDson.Item1 as IWayAwrIfrm, selectedDson.Item2, OrgPosVM.NodSlcEed);
                        SelectedDsonVM.FormClosed -= AssignedToMeSelectedDson_FormClosed;
                        SelectedDsonVM.FormClosed += AssignedToMeSelectedDson_FormClosed;
                        RaisePropertyChanged("SelectedDsonVM");
                        break;
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromUnspcf:
                        SelectedDsonVM = new DsonDtlAssignedByMeViewModel(this.context, selectedDson.Item1 as IWayAwrIfrm, selectedDson.Item2, OrgPosVM.NodSlcEed);
                        SelectedDsonVM.FormClosed -= AssignedToMeSelectedDson_FormClosed;
                        SelectedDsonVM.FormClosed += AssignedToMeSelectedDson_FormClosed;
                        RaisePropertyChanged("SelectedDsonVM");
                        break;
                    default:
                        break;
                }

                DsonOverviewVM = new DsonOverviewViewModel();
                DsonOverviewVM.CurrAwrIfrm = selectedDson.Item1;
                DsonIsVisible = true;
                RaisePropertyChanged("DsonIsVisible", "DsonOverviewVM");
                (PrevCommand as DelegateCommand).RaiseCanExecuteChanged();
                (NextCommand as DelegateCommand).RaiseCanExecuteChanged();
                var curr = getItemPosition(DsonsCV, selectedDson);
                if (curr >= 0)
                {
                    DsonsCV.MoveCurrentToPosition(curr);
                }
                DsonDescNod = string.Format("{0} {1} :", selectedDson.Item2.FldTtlNod, selectedDson.Item2.FldNamNod);

                DsonDescObj = string.Format("{0}", PublicMethods.GetDsonDesc(selectedDson.Item1));
            }
        }

        private int getItemPosition(ListCollectionView DsonsCV, object item)
        {
            int i = -1;

            if (DsonsCV == null)
            {
                return i;
            }

            foreach (var item1 in DsonsCV.SourceCollection)
            {
                i++;
                if (item1 == item)
                {
                    return i;
                }
            }

            return i;
        }

        public ListCollectionView DsonsCV
        {
            get { return dsonsCV; }
            set
            {
                dsonsCV = value;
                RaisePropertyChanged("DsonsCV");
            }
        }

        public bool DsonIsVisible { get; set; }

        /// <summary>
        /// متنی که در وسط دکمه های بعدی و قبلی نمایش داده می شود
        /// </summary>
        public string DsonDescNod
        {
            get { return _dsonDescNod; }
            set
            {
                _dsonDescNod = value;
                RaisePropertyChanged("DsonDescNod");
            }
        }

        public string DsonDescObj
        {
            get { return _dsonDescObj; }
            set
            {
                _dsonDescObj = value;
                RaisePropertyChanged("DsonDescObj");
            }
        }


        #region ' Access '

        /// <summary>
        /// 
        /// </summary>
        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewDson
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "View", namTypEtyMjr: "Dson", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["ViewDson"];
            }
        }


        #endregion

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        List<Tuple<IWayAwrIfrm, TblNod>> dsons;

        void OrgPosVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NodSlcEed")
            {
                if (OrgPosVM.NodSlcEed != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(OrgPosVM.NodSlcEed, "View", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "Dson");

                    if (PublicMethods.CurrentUser.AcsUsr["ViewDson"])
                    {
                        refreshDsons();
                    }
                    else
                    {
                        Util.ShowMessageBox(49, Util.GetNodTypeString((FldTypEty)OrgPosVM.NodSlcEed.FldCodTypEty));
                    }
                }

            }
        }

        private void refreshDsons()
        {
            dsons = PublicMethods.DetectDsonsClaimedByNod_19020(this.context, OrgPosVM.NodSlcEed);

            // ناهمسانی های جایگاه جاری نمایش داده نشود
            dsons.RemoveAll(d => d.Item2.FldCodNod == OrgPosVM.NodSlcEed.FldCodNod);

            if (dsons.Count == 0)
                SelectedDson = null;
            else
                SelectedDson = dsons.FirstOrDefault();
            DsonsCV = new ListCollectionView(dsons);
            DsonsCV.GroupDescriptions.Add(new PropertyGroupDescription("Item2"));
        }

        void AssignedToMeSelectedDson_FormClosed(object sender, EventArgs e)
        {

            DsonIsVisible = false;
            RaisePropertyChanged("DsonIsVisible");

            refreshDsons();

            //if (dsons != null)
            //{
            //    dsons.Remove(selectedDson);
            //    DsonsCV = new ListCollectionView(dsons);
            //    DsonsCV.GroupDescriptions.Add(new PropertyGroupDescription("Item2"));
            //    DsonIsVisible = false;
            //    RaisePropertyChanged("DsonIsVisible");
            //}
        }


        #endregion

        #region ' events '

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);
        }

        public bool ConfirmAndClose()
        {
            return true;

            //if (Util.ShowMessageBox(41) == MessageBoxResult.Yes)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


    }
}
