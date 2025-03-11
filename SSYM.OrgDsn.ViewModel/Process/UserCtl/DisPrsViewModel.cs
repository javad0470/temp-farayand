using System.Threading;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SSYM.OrgDsn.Model;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.Process.Popup;
using MindFusion.Diagramming.Wpf;
using MindFusion.Layout;
using yWorks.yFiles.UI.Model;
using yWorks.yFiles.UI;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System.Windows;
using SSYM.OrgDsn.Model.Enum;


namespace SSYM.OrgDsn.ViewModel.Process.UserCtl
{
    public class DisPrsViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        BPMNDBEntities context;
        TblPr selectedPrs;
        int selectedNamPrpsPrs;
        
        int selectedOwrPrpsPrs;
        ObservableCollection<object> dtlNamPrs;
        ObservableCollection<object> dtlOwrPrs;
        bool isSttPrsPopupOpen;
        ObservableCollection<DgmOfPrs> openedDgms;
        DgmOfPrs currentDgm;

        #endregion

        #region ' Initialaizer '

        public DisPrsViewModel()
        {
            this.context = new BPMNDBEntities();
            OpenDtlNamPrpsPrsCommand = new DelegateCommand(ExecuteOpenDtlNamPrpsPrsCommand);
            OpenVotForNamPrpsPrsCommand = new DelegateCommand(ExecuteOpenVotForNamPrpsPrsCommand);
            OpenVotForOwrPrpsPrsCommand = new DelegateCommand(ExecuteOpenVotForOwrPrpsPrsCommand);
            OpenDtlOwrPrpsPrsCommand = new DelegateCommand(ExecuteOpenDtlOwrPrpsPrsCommand);
            OpenPrpsNamForPrsCommand = new DelegateCommand(ExecuteOpenPrpsNamForPrsCommand);
            OpenPrpsOwrForPrsCommand = new DelegateCommand(ExecuteOpenPrpsOwrForPrsCommand);
            OpenSttPrsCommand = new DelegateCommand<TblPr>(ExecuteOpenSttPrsCommand, CanExecuteOpenSttPrsCommand);
            DisplayingPrs = false;
            //this.DtlVotNamPrpsPrs = new DtlVotNamPrpsPrsViewModel(context);
            //this.VotForNamPrpsPrs = new VotForNamPrpsPrsViewModel(context);
            //this.DtlVotOwrPrpsPrs = new DtlVotOwrPrpsPrsViewModel(context);
            //this.VotForOwrPrpsPrs = new VotForOwrPrpsPrsViewModel(context);
            //this.PrpsNamForPrs = new PrpsNamForPrsViewModel(context);
            //this.PrpsOwrForPrs = new PrpsOwrForPrsViewModel(context);
            //this.SttPrs = new SttPrsViewModel(context, this);
            FilterPrsGridCommand = new DelegateCommand(ShowFilterPopup);
            //P1566
            var thTemp = new Thread(() => Rebind(PublicMethods.DetectAllPrs_1570(this.context)));
            thTemp.Start();
            //this.AllPrs = new ObservableCollection<object>(PublicMethods.DetectAllPrs_1570(this.context).Select((item) => new { item.FldCodPrs, item.FldNamPrs, item.TblNod.FldNamNod, item.FldSttPrs }));
            //DtlNamPrs = new ObservableCollection<object>();
            OpenedDgms = new ObservableCollection<DgmOfPrs>();
            RemoveCurrentDgm = new DelegateCommand<DgmOfPrs>(ExecuteRemoveCurrentDgm);
            FlagVisible = true;
        }

        #endregion

        #region ' Properties / Commands '

        private bool _displayingPrs;
        //فرآیند در حال  رسم است
        public bool DisplayingPrs
        {
            get
            {
                return _displayingPrs;
            }
            set
            {
                _displayingPrs = value;
                RaisePropertyChanged("DisplayingPrs");
            }
        }

        /// <summary>
        /// تعیین میکند که آیا جزئیات فرایند قابل مشاهده است یا خیر
        /// </summary>
        public bool DtlVisible { get; set; }

        /// <summary>
        /// تعیین میکند که آیا ستون وضعیت فرایند قابل مشاهده است یا خیر
        /// </summary>
        public bool FlagVisible { get; set; }

        #region Access

        public bool Acs_ViewPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "View", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                // شخص جاری نماینده سازمان باشد یا 
                return TblNod.GetNodOfPsnIsdOrg_22192(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(n => n.FldCodTypEty == (int)FldTypEty.Org)
                    ||
                PublicMethods.CurrentUser.AcsUsr["ViewPrs"];
            }
        }

        public bool Acs_EditPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["EditPrs"];
            }
        }

        public bool Acs_PrpsNamPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "PrpsNam", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["PrpsNamPrs"];
            }
        }

        public bool Acs_VotNamPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "VotNam", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["VotNamPrs"];
            }
        }

        public bool Acs_PrpsOwrPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "PrpsOwr", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["PrpsOwrPrs"];
            }
        }

        public bool Acs_VotOwrPrs
        {
            get
            {
                if (this.SelectedPrs != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.SelectedPrs, "VotOwr", Model.Enum.TypRlnEtyMjrWthEtyMom.ContributerInProcess, null);
                }

                return PublicMethods.CurrentUser.AcsUsr["VotOwrPrs"];
            }
        }

        #endregion

        public ICommand RemoveCurrentDgm { get; set; }

        public ObservableCollection<DgmOfPrs> OpenedDgms
        {
            get { return openedDgms; }
            set { openedDgms = value; }
        }

        /// <summary>
        /// لیست تمام فرآیندها
        /// </summary>
        public ObservableCollection<TblPr> AllPrs { get; set; }

        /// <summary>
        /// فرآیندی که در حالت انتخاب قرار دارد
        /// </summary>
        public TblPr SelectedPrs
        {
            get { return selectedPrs; }
            set
            {
                selectedPrs = value;
                DisplayingPrs = true;
               // DtlOwrPrs = null;
                //DtlNamPrs = null;
                DetectDtlNamPrs();
                DetectDtlOwrPrs();
                RaisePropertyChanged("SelectedPrs", "DtlNamPrs", "DtlOwrPrs");
                RaisePropertyChanged("Acs_PrpsNamPrs", "Acs_VotNamPrs", "Acs_PrpsOwrPrs", "Acs_VotOwrPrs", "Acs_EditPrs");
            }
        }

        /// <summary>
        /// جزئیات نام فرآیند
        /// </summary>
        public ObservableCollection<object> DtlNamPrs
        {
            get { return dtlNamPrs; }
            set
            {
                dtlNamPrs = value;
                RaisePropertyChanged("DtlNamPrs");
            }
        }

        /// <summary>
        /// فرآیندی که هم اکنون در حال نمایش است
        /// </summary>
        public DgmOfPrs CurrentDgm
        {
            get { return currentDgm; }
            set
            {
                currentDgm = value;
                RaisePropertyChanged("CurrentDgm");
            }
        }

        /// <summary>
        /// جزئیات مالک فرآیند
        /// </summary>
        public ObservableCollection<object> DtlOwrPrs
        {
            get { return dtlOwrPrs; }
            set
            {
                dtlOwrPrs = value;
                RaisePropertyChanged("DtlOwrPrs");
            }
        }

        /// <summary>
        /// باز کردن فرم جزئیات نام پیشنهادی فرآیند
        /// </summary>
        public ICommand OpenDtlNamPrpsPrsCommand { get; set; }

        /// <summary>
        /// باز کردن فرم جزئیات مالک پیشنهادی فرآیند
        /// </summary>
        public ICommand OpenDtlOwrPrpsPrsCommand { get; set; }

        /// <summary>
        /// باز کردن فرم پیشنهاد نام برای فرآیند
        /// </summary>
        public ICommand OpenPrpsNamForPrsCommand { get; set; }

        /// <summary>
        /// باز کردن فرم پیشنهاد مالک برای فرآیند
        /// </summary>
        public ICommand OpenPrpsOwrForPrsCommand { get; set; }

        /// <summary>
        /// نام پیشنهادی انتخاب شده
        /// </summary>
        public int SelectedNamPrpsPrs
        {
            get { return selectedNamPrpsPrs; }
            set
            {
                selectedNamPrpsPrs = value;
                RaisePropertyChanged("SelectedNamPrpsPrs");
            }
        }


        //public bool IsDtlOwrPrpsPrsPopupOpen
        //{
        //    get { return isDtlOwrPrpsPrsPopupOpen; }
        //    set
        //    {
        //        isDtlOwrPrpsPrsPopupOpen = value;
        //        RaisePropertyChanged("IsDtlOwrPrpsPrsPopupOpen");
        //    }
        //}

        public DtlVotNamPrpsPrsViewModel DtlVotNamPrpsPrs { get; set; }

        public VotForNamPrpsPrsViewModel VotForNamPrpsPrs { get; set; }

        public DtlVotOwrPrpsPrsViewModel DtlVotOwrPrpsPrs { get; set; }

        public VotForOwrPrpsPrsViewModel VotForOwrPrpsPrs { get; set; }

        public PrpsNamForPrsViewModel PrpsNamForPrs { get; set; }

        public PrpsOwrForPrsViewModel PrpsOwrForPrs { get; set; }

        public SttPrsViewModel SttPrs { get; set; }

        public FndPrsViewModel FndPrsVM { get; set; }

        public DelegateCommand FilterPrsGridCommand { get; set; }

        public bool FilterPrsPupopIsOpen { get; set; }

        /// <summary>
        /// باز کردن فرم رأی دادن به نام های پیشنهادی
        /// </summary>
        public ICommand OpenVotForNamPrpsPrsCommand { get; set; }

        /// <summary>
        /// باز کردن فرم رأی دادن به مالک پیشنهادی
        /// </summary>
        public ICommand OpenVotForOwrPrpsPrsCommand { get; set; }

        public ICommand OpenSttPrsCommand { get; set; }

        //public bool IsVotForNamPrpsPrsPopupOpen
        //{
        //    get { return isVotForNamPrpsPrsPopupOpen; }
        //    set
        //    {
        //        isVotForNamPrpsPrsPopupOpen = value;

        //        RaisePropertyChanged("IsVotForNamPrpsPrsPopupOpen");

        //        if (!value && VotForNamPrpsPrs.Result == Base.PopupResult.OK)
        //        {
        //            GiveVotToNamPrpsPrs();

        //            DetectDtlNamPrs();
        //        }
        //    }
        //}

        /// <summary>
        /// مالک پیشنهادی انتخاب شده
        /// </summary>
        public int SelectedOwrPrpsPrs
        {
            get { return selectedOwrPrpsPrs; }
            set
            {
                selectedOwrPrpsPrs = value;
                RaisePropertyChanged("SelectedOwrPrpsPrs");
            }
        }

        //public bool IsDtlNamPrpsPrsPopupOpen
        //{
        //    get { return isDtlNamPrpsPrsPopupOpen; }
        //    set
        //    {
        //        isDtlNamPrpsPrsPopupOpen = value;
        //        RaisePropertyChanged("IsDtlNamPrpsPrsPopupOpen");
        //    }
        //}


        //public bool IsVotForOwrPrpsPrsPopupOpen
        //{
        //    get { return isVotForOwrPrpsPrsPopupOpen; }
        //    set
        //    {
        //        isVotForOwrPrpsPrsPopupOpen = value;

        //        RaisePropertyChanged("IsVotForOwrPrpsPrsPopupOpen");

        //        if (!value && VotForOwrPrpsPrs.Result == Base.PopupResult.OK)
        //        {
        //            GiveVotToOwrPrpsPrs();

        //            DetectDtlOwrPrs();
        //        }
        //    }
        //}

        //public bool IsPrpsNamForPsrOpen
        //{
        //    get { return isPrpsNamForPsrOpen; }
        //    set
        //    {
        //        isPrpsNamForPsrOpen = value;
        //        RaisePropertyChanged("IsPrpsNamForPsrOpen");
        //        if (!value && PrpsNamForPrs.Result == Base.PopupResult.OK)
        //        {
        //            AddNamPrpsToPrs();

        //            PublicMethods.SaveContext(this.context);

        //            DetectDtlNamPrs();
        //        }
        //    }
        //}

        //public bool IsPrpsOwrForPsrOpen
        //{
        //    get { return isPrpsOwrForPsrOpen; }

        //    set
        //    {
        //        isPrpsOwrForPsrOpen = value;

        //        RaisePropertyChanged("IsPrpsOwrForPsrOpen");

        //        if (!value && PrpsOwrForPrs.Result == Base.PopupResult.OK)
        //        {
        //            AddOwrPrpsToPrs();

        //            PublicMethods.SaveContext(this.context);

        //            DetectDtlOwrPrs();

        //            (OpenSttPrsCommand as DelegateCommand<TblPr>).RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        //public bool IsSttPrsPopupOpen
        //{
        //    get { return isSttPrsPopupOpen; }
        //    set
        //    {
        //        if (value && (this.SelectedPrs.TblNod == null || PublicMethods.CurrentUser.TblPsn.TblAgntNods.Where(m => m.FldCodNod == this.SelectedPrs.TblNod.FldCodNod).Count() == 0))
        //        {
        //            Util.ShowMessageBox(18);
        //            isSttPrsPopupOpen = false;
        //        }

        //        else
        //        {
        //            isSttPrsPopupOpen = value;
        //        }

        //        //if (!value)
        //        //{
        //        //    isSttPrsPopupOpen = value;
        //        //    //P1566
        //        //    Rebind(PublicMethods.DetectAllPrs_1570(this.context));
        //        //}

        //        RaisePropertyChanged("IsSttPrsPopupOpen");

        //        RaisePropertyChanged("SelectedPrs");
        //    }
        //}




        #endregion

        #region ' Public Methods '

        public void ShowFilterPopup()
        {
            if (FndPrsVM == null)
            {
                FndPrsVM = new FndPrsViewModel();

                FndPrsVM.PropertyChanged += FndPrsVM_PropertyChanged;

                RaisePropertyChanged("FndPrsVM");
            }


            Util.ShowPopup(FndPrsVM);

            //FilterPrsPupopIsOpen = true;
            //RaisePropertyChanged("FilterPrsPupopIsOpen");
        }




        #endregion

        #region ' Private Methods '

        private void ExecuteRemoveCurrentDgm(DgmOfPrs obj)
        {
            this.OpenedDgms.Remove(obj);
        }


        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenSttPrsCommand(TblPr obj)
        {
            this.SttPrs = new SttPrsViewModel(context, this);
            if (SelectedPrs.TblNod != null)
            {
                //TblPr prs = this.context.TblPrs.SingleOrDefault(m => m.FldCodPrs == SelectedPrs.FldCodPrs);
                if (SelectedPrs.FldSttPrs == (int)Model.Enum.SttPrs.ConsolidatedEndorsed)
                {
                    SttPrs.IsFirstVisible = false;
                    SttPrs.IsSecondVisible = true;
                    SttPrs.IsThirdVisible = false;
                }
                else if (SelectedPrs.FldSttPrs == (int)Model.Enum.SttPrs.ConsolidatedNotEndorsed)
                {
                    SttPrs.IsFirstVisible = false;
                    SttPrs.IsSecondVisible = true;
                    SttPrs.IsThirdVisible = true;
                }
                else if (SelectedPrs.FldSttPrs == (int)Model.Enum.SttPrs.Raw)
                {
                    SttPrs.IsFirstVisible = true;
                    SttPrs.IsSecondVisible = false;
                    SttPrs.IsThirdVisible = false;
                }
                SttPrs.CurrentPrs = SelectedPrs;
                SttPrs.OwrPrs = SelectedPrs.TblNod.FldNamNod;
            }

            SttPrs.CommandBarVisible = false;
            Util.ShowPopup(SttPrs);

            //// آیا شخص جاری نماینده مالک فرایند انتخاب شده نیست؟ 

            ////|| PublicMethods.CurrentUser.TblPsn.TblAgntNods.Where(m => m.FldCodNod == this.SelectedPrs.TblNod.FldCodNod).Count() == 0
            //if (this.SelectedPrs.TblNod == null
            //    || !PublicMethods.GetPosPstOfPsnAgntOfThem(this.context, PublicMethods.CurrentUser.TblPsn,PublicMethods.CurrentUser.TblOrg).Any(a => a.Nod.FldCodNod == this.SelectedPrs.TblNod.FldCodNod)
            //    || !PublicMethods.DetectRolWithAgntOfPsn_22180(this.context, PublicMethods.CurrentUser.TblPsn).Any(r => r.Nod.FldCodNod == this.SelectedPrs.TblNod.FldCodNod)
            //    )
            //{
            //    Util.ShowMessageBox(18);
            //}
            //else
            //{
            //    Util.ShowPopup(SttPrs);
            //}

            RaisePropertyChanged("SelectedPrs");
        }

        private bool CanExecuteOpenSttPrsCommand(TblPr arg)
        {
            //List<TblAgntNod> lst = PublicMethods.CurrentUser.TblPsn.TblAgntNods.ToList();

            //foreach (TblAgntNod item in lst)
            //{
            //    if (arg != null && item.TblNod.FldCodNod == arg.TblNod.FldCodNod)
            //    {
            //        return true;
            //    }
            //}

            return true;
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private int scapeId = 0;
        /// <summary>
        /// 
        /// </summary>
        public void Rebind(List<TblPr> lst)
        {
            if (lst == null)
            {
                lst = PublicMethods.DetectAllPrs_1570(context);
            }
            if (lst == null || lst.Count == 0)
            {
                this.AllPrs = new ObservableCollection<TblPr>();
                return;
            }
            this.AllPrs = new ObservableCollection<TblPr>();
            AllPrs.Add(lst.First());
            scapeId = 1;
            RaisePropertyChanged("AllPrs");
            
            dispatcherTimer.Tick += (sender, args) =>
            {
                if (scapeId < lst.Count)
                {
                    AllPrs.Add(lst[scapeId++]);
                }
                else if (sender is System.Windows.Threading.DispatcherTimer)
                {
                    (sender as  System.Windows.Threading.DispatcherTimer).Stop();
                }
            };
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            dispatcherTimer.Start();
        }

        void FndPrsVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FilterResult")
            {
                if (FndPrsVM != null)
                {
                    if (FndPrsVM.FilterResult != null)
                    {
                        Rebind(FndPrsVM.FilterResult);
                    }
                    else
                    {
                        Rebind(PublicMethods.DetectAllPrs_1570(this.context));
                    }
                }
                else
                {
                    Rebind(PublicMethods.DetectAllPrs_1570(this.context));
                }
            }
        }

        private void ExecuteOpenPrpsNamForPrsCommand()
        {
            this.PrpsNamForPrs = new PrpsNamForPrsViewModel(context);
            this.PrpsNamForPrs.CurrentPrs = this.SelectedPrs;

            Util.ShowPopup(PrpsNamForPrs);

            if (PrpsNamForPrs.Result == PopupResult.OK)
            {
                AddNamPrpsToPrs();

                PublicMethods.SaveContext(this.context);

                DetectDtlNamPrs();
                RaisePropertyChanged("DtlOwrPrs","DtlNamPrs");
            }
            //this.IsPrpsNamForPsrOpen = true;
        }

        /// <summary>
        /// P1826
        /// </summary>
        private void AddNamPrpsToPrs()
        {
            //TblNod nod = PublicMethods.DetectNodOfPosPst_753(this.context, PrpsNamForPrs.SelectedNod);
            TblNod nod = PrpsNamForPrs.SelectedNod.Nod;

            TblNamPrpsPr namPrpsPr = new TblNamPrpsPr() { FldNamPrpsPrs = PrpsNamForPrs.NamPrpsPrs, FldCodNodPrpsEer = nod.FldCodNod };

            TblVotNamPrpsPr vot = new TblVotNamPrpsPr() { FldVot = 1, FldCodNodVotEer = nod.FldCodNod };

            namPrpsPr.TblVotNamPrpsPrs.Add(vot);

            this.SelectedPrs.TblNamPrpsPrs.Add(namPrpsPr);


        }

        /// <summary>
        /// P1849
        /// </summary>
        private void AddOwrPrpsToPrs()
        {
            //TblNod nod = PublicMethods.DetectNodOfPosPst_753(this.context, this.PrpsOwrForPrs.SelectedNod);
            TblNod nod = this.PrpsOwrForPrs.SelectedNod.Nod;

            //TblNod nod1 = PublicMethods.DetectNodOfPosPst_753(this.context, this.PrpsOwrForPrs.SelectedOwr);
            TblNod nod1 = this.PrpsOwrForPrs.SelectedOwr.Nod;

            TblOwrPrpsPr owrPrpsPr = new TblOwrPrpsPr() { FldCodOwrPrpsPrs = nod1.FldCodNod, FldCodNodPrpsEer = nod.FldCodNod };

            TblVotOwrPrp vot = new TblVotOwrPrp() { FldVot = 1, FldCodNodVotEer = nod.FldCodNod };

            owrPrpsPr.TblVotOwrPrps.Add(vot);

            this.SelectedPrs.TblOwrPrpsPrs.Add(owrPrpsPr);
        }

        /// <summary>
        /// شناسایی جزئیات نام فرآیند انتخاب شده
        /// </summary>
        private void DetectDtlNamPrs()
        {
            if (SelectedPrs == null)
            {
                return;
            }

            PublicMethods.ReloadEntity(this.context, SelectedPrs, SelectedPrs.TblNamPrpsPrs, "TblNamPrpsPrs");

            List<object> lst = new List<object>();

            foreach (TblNamPrpsPr item in SelectedPrs.TblNamPrpsPrs)
            {
                var o = new { item.FldCodNamPrpsPrs, item.FldNamPrpsPrs, FldVluVot = PublicMethods.ComputeWeightVluNamPrpsPrs_1593(this.context, item) };
                lst.Add(o);
            }

            PublicMethods.DetectNamPrs_17972(this.context, SelectedPrs);
            if (this.DtlNamPrs == null)
                this.DtlNamPrs = new ObservableCollection<object>();
            this.DtlNamPrs .Clear();
            foreach(object obj in lst)
            {
                    this.DtlNamPrs.Add(obj);
            }
            RaisePropertyChanged("SelectedPrs");
            //this.DtlNamPrs = new ObservableCollection<object>(lst);
        }

        /// <summary>
        /// شناسایی جزئیات مالک فرآیند انتخاب شده
        /// </summary>
        private void DetectDtlOwrPrs()
        {
            //PublicMethods.ReloadEntity(this.context, SelectedPrs.TblOwrPrpsPrs);

            List<object> lst = new List<object>();

            foreach (TblOwrPrpsPr item in SelectedPrs.TblOwrPrpsPrs)
            {
                var o = new { item.FldCod, FldNamNod = item.TblNod.FldNamNod, FldVluVot = PublicMethods.ComputeWeightVluOwrPrpsPrs_1624(this.context, item) };
                lst.Add(o);
            }

            SelectedPrs.TblNod = PublicMethods.DetectOwrPrs_17974(this.context, SelectedPrs);
            
            RaisePropertyChanged("SelectedPrs");
            if (this.DtlOwrPrs == null)
                this.DtlOwrPrs = new ObservableCollection<object>();
            this.DtlOwrPrs.Clear();
            foreach(object obj in lst)
            {
                this.DtlOwrPrs.Add(obj);
            }
            RaisePropertyChanged("SelectedPrs");
            //this.DtlOwrPrs = new ObservableCollection<object>(lst);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenDtlNamPrpsPrsCommand()
        {
            this.DtlVotNamPrpsPrs = new DtlVotNamPrpsPrsViewModel(context);
            TblNamPrpsPr namPrpsPrs = this.context.TblNamPrpsPrs.SingleOrDefault(m => m.FldCodNamPrpsPrs == this.SelectedNamPrpsPrs);
            DtlVotNamPrpsPrs.NamPrpsPrs = namPrpsPrs;
            Util.ShowPopup(DtlVotNamPrpsPrs);

            //IsDtlNamPrpsPrsPopupOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenVotForNamPrpsPrsCommand()
        {
            this.VotForNamPrpsPrs = new VotForNamPrpsPrsViewModel(context);
            TblNamPrpsPr namPrpsPrs = this.context.TblNamPrpsPrs.SingleOrDefault(m => m.FldCodNamPrpsPrs == this.SelectedNamPrpsPrs);
            VotForNamPrpsPrs.NamPrpsPrs = namPrpsPrs;
            VotForNamPrpsPrs.SelectedNod = null;
            Util.ShowPopup(VotForNamPrpsPrs);

            if (VotForNamPrpsPrs.Result == PopupResult.OK)
            {
                GiveVotToNamPrpsPrs();

                DetectDtlNamPrs();
            }
            //IsVotForNamPrpsPrsPopupOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenVotForOwrPrpsPrsCommand()
        {
            this.VotForOwrPrpsPrs = new VotForOwrPrpsPrsViewModel(context);
            TblOwrPrpsPr owrPrpsPr = this.context.TblOwrPrpsPrs.SingleOrDefault(m => m.FldCod == this.SelectedOwrPrpsPrs);
            VotForOwrPrpsPrs.OwrPrpsPr = owrPrpsPr;
            VotForOwrPrpsPrs.SelectedNod = null;

            Util.ShowPopup(VotForOwrPrpsPrs);

            if (VotForOwrPrpsPrs.Result == PopupResult.OK)
            {
                GiveVotToOwrPrpsPrs();

                DetectDtlOwrPrs();
            }

            //IsVotForOwrPrpsPrsPopupOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenDtlOwrPrpsPrsCommand()
        {
            this.DtlVotOwrPrpsPrs = new DtlVotOwrPrpsPrsViewModel(context);
            TblOwrPrpsPr owrPrpsPr = this.context.TblOwrPrpsPrs.SingleOrDefault(m => m.FldCod == this.SelectedOwrPrpsPrs);
            DtlVotOwrPrpsPrs.OwrPrpsPrs = owrPrpsPr;

            Util.ShowPopup(DtlVotOwrPrpsPrs);

            //IsDtlOwrPrpsPrsPopupOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteOpenPrpsOwrForPrsCommand()
        {
            this.PrpsOwrForPrs = new PrpsOwrForPrsViewModel(context);
            this.PrpsOwrForPrs.CurrentPrs = this.SelectedPrs;

            Util.ShowPopup(PrpsOwrForPrs);

            if (PrpsOwrForPrs.Result == PopupResult.OK)
            {
                AddOwrPrpsToPrs();
                
                PublicMethods.SaveContext(this.context);

                DetectDtlOwrPrs();

                (OpenSttPrsCommand as DelegateCommand<TblPr>).RaiseCanExecuteChanged();
            }
            //this.IsPrpsOwrForPsrOpen = true;
        }


        /// <summary>
        /// P1612
        /// </summary>
        private void GiveVotToNamPrpsPrs()
        {
            //1619
            List<TblVotNamPrpsPr> votNamPrpsPr = PublicMethods.DetectVotToOneNamPrpsPrs_1619(this.context,
                this.context.TblNamPrpsPrs.SingleOrDefault(m => m.FldCodNamPrpsPrs == this.SelectedNamPrpsPrs));

            //1620
            int i = 0;
            if (VotForNamPrpsPrs.IsAgreeSelected)
            {
                i = (int)Model.Enum.TypVot.Agree;
            }
            else if (VotForNamPrpsPrs.IsDisAgreeSelected)
            {
                i = (int)Model.Enum.TypVot.DisAgree;
            }
            else
            {
                i = (int)Model.Enum.TypVot.Neutral;
            }

            //1613
            TblNod nod = VotForNamPrpsPrs.SelectedNod.Nod;
            if (IsSpcNodVotToSpcNamPrpsPrs(votNamPrpsPr, nod))
            {
                //1615
                TblNamPrpsPr namPrpsPrs = this.context.TblNamPrpsPrs.SingleOrDefault(m => m.FldCodNamPrpsPrs == this.SelectedNamPrpsPrs);
                TblVotNamPrpsPr tbl = namPrpsPrs.TblVotNamPrpsPrs.SingleOrDefault(m => m.FldCodNodVotEer == nod.FldCodNod);
                tbl.FldVot = i;
            }
            //1614
            else
            {
                //1616
                TblNamPrpsPr namPrpsPrs = this.context.TblNamPrpsPrs.SingleOrDefault(m => m.FldCodNamPrpsPrs == this.SelectedNamPrpsPrs);
                namPrpsPrs.TblVotNamPrpsPrs.Add(new TblVotNamPrpsPr() { FldCodNodVotEer = nod.FldCodNod, FldVot = i });
            }
        }

        /// <summary>
        /// P1633
        /// </summary>
        private void GiveVotToOwrPrpsPrs()
        {
            //1800
            List<TblVotOwrPrp> votOwrPrp = PublicMethods.DetectVotToOneOwrPrpsPrs_1800(this.context,
                this.context.TblOwrPrpsPrs.SingleOrDefault(m => m.FldCod == this.SelectedOwrPrpsPrs));

            //1801
            int i = 0;
            if (VotForOwrPrpsPrs.IsAgreeSelected)
            {
                i = (int)Model.Enum.TypVot.Agree;
            }
            else if (VotForOwrPrpsPrs.IsDisAgreeSelected)
            {
                i = (int)Model.Enum.TypVot.DisAgree;
            }
            else
            {
                i = (int)Model.Enum.TypVot.Neutral;
            }

            //1803
            if (IsSpcNodVotToSpcOwrPrpsPrs(votOwrPrp, VotForOwrPrpsPrs.SelectedNod.Nod))
            {
                //1804
                TblOwrPrpsPr owrPrpsPrs = this.context.TblOwrPrpsPrs.SingleOrDefault(m => m.FldCod == this.SelectedOwrPrpsPrs);
                TblVotOwrPrp tbl = owrPrpsPrs.TblVotOwrPrps.SingleOrDefault(m => m.FldCodNodVotEer == VotForOwrPrpsPrs.SelectedNod.Nod.FldCodNod);
                tbl.FldVot = i;
            }
            //1802
            else
            {
                //1805
                TblOwrPrpsPr owrPrpsPrs = this.context.TblOwrPrpsPrs.SingleOrDefault(m => m.FldCod == this.SelectedOwrPrpsPrs);
                owrPrpsPrs.TblVotOwrPrps.Add(new TblVotOwrPrp() { FldCodNodVotEer = VotForOwrPrpsPrs.SelectedNod.Nod.FldCodNod, FldVot = i });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="votNamPrpsPr"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        private bool IsSpcNodVotToSpcNamPrpsPrs(List<TblVotNamPrpsPr> votNamPrpsPr, TblNod nod)
        {
            foreach (TblVotNamPrpsPr item in votNamPrpsPr)
            {
                if (item.TblNod == nod)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="votNamPrpsPr"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        private bool IsSpcNodVotToSpcOwrPrpsPrs(List<TblVotOwrPrp> votOwrPrpsPr, TblNod nod)
        {
            foreach (TblVotOwrPrp item in votOwrPrpsPr)
            {
                if (item.TblNod == nod)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region ' events '

        #endregion

        #region ' Classes '

        public class DgmOfPrs
        {
            GraphControl grphCtl;
            string namDgm;
            int codPrs;

            public int CodPrs
            {
                get { return codPrs; }
                set { codPrs = value; }
            }
            TblPr prs;

            public TblPr Prs
            {
                get { return prs; }
                set { prs = value; }
            }

            public string NamDgm
            {
                get { return namDgm; }
                set { namDgm = value; }
            }

            public GraphControl GrphCtl
            {
                get { return grphCtl; }
                set { grphCtl = value; }
            }
        }

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);
        }

        public bool ConfirmAndClose()
        {
            if (Util.HasContextChanges(this.context))
            {
                if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                {
                    this.SaveContext();
                    return true;
                }
                else
                {
                    PublicMethods.RollBackContext(this.context);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

    }
}
