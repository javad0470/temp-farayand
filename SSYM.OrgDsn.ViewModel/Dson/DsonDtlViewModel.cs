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
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public abstract class DsonDtlViewModel : NotificationObject
    {
        #region ' Fields '

        bool _saveEnabled;

        private TblNod _nod;

        protected MessageBoxResult result;

        protected BPMNDBEntities context;

        //private EntityObject _dsonObj;

        #endregion

        #region ' Initialaizer '

        public DsonDtlViewModel()
        {
            SaveEnabled = false;
        }

        public DsonDtlViewModel(BPMNDBEntities context, IWayAwrIfrm _dsonObj, TblNod nod, TblNod selectedNode)
        {
            this.context = context;
            WayAwrIfrm = _dsonObj;
            Node = nod;
            SelectedNode = selectedNode;
            this.PropertyChanged += DsonDtlViewModel_PropertyChanged;

            if (InputVisibility == Visibility.Visible)
            {
                dynamic d = WayAwrIfrm;

                (WayAwrIfrm as IWayAwr).EvtSrt_Temp = d.TblEvtSrt;

                SelectedAct = _dsonObj.ActDst;
            }
            else
            {
                //dynamic d = WayAwrIfrm;
                //(WayAwrIfrm as IWayAwr). = d.TblEvtSrt;
                if (IsActSpec)
                {
                    SelectedAct = _dsonObj.ActDst;
                }
                else
                {
                    SelectedAct = nod.TblActs.First(m => !m.FldActUspf);
                }

            }

            SaveFormCommand = new DelegateCommand(SaveExecute, CanSave);
            CancelCommand = new DelegateCommand(CancelExecute);

            AcceptRejectCommand = new DelegateCommand<string>(AcceptRejectExecute);

            IsAccepted = true;
            //BtnSelInpComm = new DelegateCommand();
            //BtnSelOtpComm = new DelegateCommand();


        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// نودی که ادعای ناهمسان مربوط به آن است
        /// </summary>
        public TblNod SelectedNode { get; set; }

        /// <summary>
        /// فعالیت که ادعای ناهمسان مربوط به آن آست
        /// </summary>
        public virtual TblAct DsonAct { get; set; }

        public string TxbPosEraText { get; set; }

        public string TxbPosDstText { get; set; }

        public string TxbPstEraText { get; set; }

        public string TxbPstDstText { get; set; }

        public abstract string FrstFtrRgnText
        {
            get;
        }

        public TypDson TypDsonCur { get { return this.WayAwrIfrm.DsonType; } }

        TblAct selectedAct;
        public TblAct SelectedAct
        {
            get { return selectedAct; }
            set
            {
                selectedAct = value;
                RaisePropertyChanged("SelectedAct");
            }
        }
        //public TblAct SelectedAct { get; set; }

        public ObservableCollection<TblAct> LstActNod { get; set; }

        public Visibility ImgIcnDrgVsbl
        {
            get
            {
                if (this is DsonDtlAssignedToMeViewModel)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public Visibility ImgIcnInVsbl { get; set; }

        public Visibility ImgIcnOutVsbl { get; set; }

        public Visibility BtnSelInpVsbl { get; set; }

        public Visibility BtnSelOtpVsbl { get; set; }

        public Visibility TrvSelActVsbl { get; set; }

        public Visibility BrdActVsbl { get; set; }

        public Visibility TxbPosEraVsbl { get; set; }

        public Visibility TxbPosDstVsbl { get; set; }

        public Visibility TxbPstEraVsbl { get; set; }

        public Visibility TxbPstDstVsbl { get; set; }

        public Visibility FrstFtrRgnVsbl
        {
            get
            {
                return Visibility.Visible;
            }
        }

        //public Visibility ScndFtrRgnVsbl
        //{
        //    get
        //    {
        //        if (this is DsonDtlAssignedToMeViewModel && (AssignStatus == AsignStatusType.WrongAct || AssignStatus == AsignStatusType.WrongEvt))
        //        {
        //            return Visibility.Visible;
        //        }
        //        else
        //        {
        //            return Visibility.Collapsed;
        //        }
        //    }
        //}

        public Visibility UsrCtlEvtSrtVsbl
        {
            get
            {
                return InputVisibility;
            }
        }

        public Visibility UsrCtlEvtRstVsbl
        {
            get
            {
                return InputVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public ICommand BtnSelInpComm { get; set; }

        public ICommand BtnSelOtpComm { get; set; }

        public WayIfrmViewModel WayIfrmVM { get; set; }

        public WayAwrViewModel WayAwrVM { get; set; }

        public IWayAwrIfrm WayAwrIfrm { get; set; }

        public IWayAwr WayAwr
        {
            get
            {
                if (WayAwrIfrm is IWayAwr)
                {
                    return WayAwrIfrm as IWayAwr;
                }
                return null;
            }
        }

        public IWayIfrm WayIfrm
        {
            get
            {
                if (WayAwrIfrm is IWayIfrm)
                {
                    return WayAwrIfrm as IWayIfrm;
                }

                return null;
            }
        }

        public TblNod Node { get; set; }

        public Visibility AssignedToMeVisibility
        {
            get
            {
                if (this is DsonDtlAssignedToMeViewModel)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

        }

        public Visibility AssignedByMeVisibility
        {
            get
            {
                if (this is DsonDtlAssignedToMeViewModel)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }


        public Visibility InputVisibility
        {
            get
            {
                switch (this.TypDsonCur)
                {
                    case TypDson.NoDson:
                        break;
                    case TypDson.OutSpcf:
                    case TypDson.OutUnspcf:
                    case TypDson.OutSpcfToSpcf:
                    case TypDson.OutSpcfToUnspcf:
                    case TypDson.SndOralFromSpcf:
                    case TypDson.SndOralFromUnspcf:
                    case TypDson.SndOralFromSpcfToSpcf:
                    case TypDson.SndOralFromSpcfToUnspcf:
                    case TypDson.SndNewsFromSpcf:
                    case TypDson.SndNewsFromUnspcf:
                        return Visibility.Collapsed;

                    case TypDson.InSpcf:
                    case TypDson.InUnspcf:
                    case TypDson.RcvOralInSpcf:
                    case TypDson.RcvOralInUnspcf:
                    case TypDson.RcvNewsToSpcfFromSpcf:
                    case TypDson.RcvNewsToSpcfFromUnspcf:
                    case TypDson.InSpcfFromSpcf:
                    case TypDson.InSpcfFromUnspcf:
                    case TypDson.RcvOralToSpcfFromSpcf:
                    case TypDson.RcvOralToSpcfFromUnspcf:
                        return Visibility.Visible;
                }

                return Visibility.Visible;
            }
        }

        public Visibility OutputVisibility
        {
            get
            {
                switch (this.TypDsonCur)
                {
                    case TypDson.NoDson:
                        break;
                    case TypDson.OutSpcf:
                    case TypDson.OutUnspcf:
                    case TypDson.OutSpcfToSpcf:
                    case TypDson.OutSpcfToUnspcf:
                    case TypDson.SndOralFromSpcf:
                    case TypDson.SndOralFromUnspcf:
                    case TypDson.SndOralFromSpcfToSpcf:
                    case TypDson.SndOralFromSpcfToUnspcf:
                    case TypDson.SndNewsFromSpcf:
                    case TypDson.SndNewsFromUnspcf:
                        return Visibility.Visible;

                    case TypDson.InSpcf:
                    case TypDson.InUnspcf:
                    case TypDson.RcvOralInSpcf:
                    case TypDson.RcvOralInUnspcf:
                    case TypDson.RcvNewsToSpcfFromSpcf:
                    case TypDson.RcvNewsToSpcfFromUnspcf:
                    case TypDson.InSpcfFromSpcf:
                    case TypDson.InSpcfFromUnspcf:
                    case TypDson.RcvOralToSpcfFromSpcf:
                    case TypDson.RcvOralToSpcfFromUnspcf:
                        return Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }
        }

        public ICommand SaveFormCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// وقتی رخ میدهد که کاربر گزینه 
        /// 'به اشتباه به من نسبت داده شده است.'
        /// را انتخاب کند
        /// </summary>
        public ICommand AcceptRejectCommand { get; set; }

        public bool IsActSpec
        {
            get
            {
                switch (this.TypDsonCur)
                {
                    case TypDson.NoDson:
                        break;
                    case TypDson.InSpcf:
                    case TypDson.OutSpcf:
                    case TypDson.OutSpcfToSpcf:
                    case TypDson.SndOralFromSpcf:
                    case TypDson.RcvOralInSpcf:
                    case TypDson.RcvNewsToSpcfFromSpcf:
                    case TypDson.SndOralFromSpcfToSpcf:
                    case TypDson.SndNewsFromSpcf:
                    case TypDson.RcvOralToSpcfFromSpcf:
                    case TypDson.InSpcfFromSpcf:
                        return true;

                    case TypDson.OutUnspcf:
                    case TypDson.OutSpcfToUnspcf:
                    case TypDson.SndOralFromUnspcf:
                    case TypDson.SndOralFromSpcfToUnspcf:
                    case TypDson.InUnspcf:
                    case TypDson.RcvOralInUnspcf:
                    case TypDson.RcvNewsToSpcfFromUnspcf:
                    case TypDson.InSpcfFromUnspcf:
                    case TypDson.RcvOralToSpcfFromUnspcf:
                    case TypDson.SndNewsFromUnspcf:
                        return false;
                }

                return false;
            }
        }


        /// <summary>
        /// ویو مدل مربوط به فرم ناهمسانی
        /// </summary>
        public CvsnViewModel CvsnViewModel { get; set; }

        /// <summary>
        /// متن رادیو باتن مربوط به پذیرش ناهمسانی
        /// </summary>
        public abstract string AcceptRdbCnt { get; }

        /// <summary>
        /// متن رادیو باتن مربوط به عدم پذیرش ناهمسانی
        /// </summary>
        public abstract string RejectRdbCnt { get; }


        public bool SaveEnabled
        {
            get { return true;
                //_saveEnabled;
            }
            set
            {
                _saveEnabled = value;
                RaisePropertyChanged("SaveEnabled");
            }
        }

        bool isAccepted;

        /// <summary>
        /// نشان دهنده وضعیت پذیرش یا عدم پذیرش ناهمسانی
        /// </summary>
        public bool IsAccepted
        {
            get { return isAccepted; }
            set
            {
                isAccepted = value;

                if (IsAccepted)
                {
                    RaisePropertyChanged("IsAccepted", "IsRejected");
                }
                else
                {
                    //CvsnViewModel = new CvsnViewModel(this.context,this.WayAwrIfrm, DsonAct.PstPosObj, SelectedPosPst);
                    bool isSrc = this.SelectedNode == this.WayAwrIfrm.ActSrc.TblNod;
                    CvsnViewModel = new CvsnViewModel(this.context, this.WayAwrIfrm, WayAwrIfrm.ActSrc.TblNod, WayAwrIfrm.ActDst.TblNod, isSrc);

                    CvsnViewModel.CvsnOKClicked -= CvsnViewModel_CvsnOKClicked;
                    CvsnViewModel.CvsnOKClicked += CvsnViewModel_CvsnOKClicked;

                    RaisePropertyChanged("CvsnViewModel", "IsAccepted", "IsRejected");
                }
            }
        }

        //public bool IsRejected
        //{
        //    get
        //    {
        //        return !isAccepted;
        //    }
        //}

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        void DsonDtlViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAct")
            {
                if (SelectedAct == null)
                {
                    return;
                }

                if (this.InputVisibility == Visibility.Visible)
                {
                    WayAwrVM = new WayAwrViewModel(SelectedAct, SelectedNode, WayAwrIfrm as IWayAwr, IsActSpec);
                    WayAwrVM.WayAwrChanged -= WayAwrVM_WayAwrChanged;
                    WayAwrVM.WayAwrChanged += WayAwrVM_WayAwrChanged;
                    RaisePropertyChanged("WayAwrVM");
                }
                else
                {
                    this.WayIfrmVM = new WayIfrmViewModel(SelectedAct, SelectedNode, WayAwrIfrm as IWayIfrm);
                    this.WayIfrmVM.WayIfrmChanged += WayIfrmVM_WayIfrmChanged;
                    RaisePropertyChanged("WayIfrmVM");
                }
            }
        }


        protected int addedCount = 0;
        void WayIfrmVM_WayIfrmChanged(bool obj)
        {
            adjustSaveEnabled(obj);
        }

        void WayAwrVM_WayAwrChanged(bool obj)
        {
            adjustSaveEnabled(obj);
        }


        /// <summary>
        /// در صورتی امکان تایید ناهمسانی وجود دارد که تغیری بوجود آمده باشد.
        /// </summary>
        /// <param name="obj"></param>
        private void adjustSaveEnabled(bool obj)
        {
            if (obj)
            {
                addedCount++;
            }
            else
            {
                addedCount--;
            }

            if (addedCount > 0)
            {
                SaveEnabled = true;
            }
            else
            {
                SaveEnabled = false;
            }
        }

        protected virtual bool CanSave()
        {
            return true;
        }

        protected virtual void SaveExecute()
        {
            if (result == MessageBoxResult.Yes)
            {
                PublicMethods.SaveContext(context);
                if (FormClosed != null)
                {
                    FormClosed(this, null);
                }
            }
        }

        private void CancelExecute()
        {
            if (FormClosed != null)
            {
                FormClosed(this, null);
            }
        }

        private void AcceptRejectExecute(string obj)
        {
            this.IsAccepted = bool.Parse(obj);
        }

        void CvsnViewModel_CvsnOKClicked(object sender, EventArgs e)
        {
            PublicMethods.AddDsonByWayAwrInfrm_19072(this.context, GetPeerStateForRejectedDson(this.WayAwrIfrm.DsonType), this.WayAwrIfrm);
            PublicMethods.SettleDson_19202(this.WayAwrIfrm);

            if (FormClosed != null)
            {
                FormClosed(this, null);
            }

        }


        /// <summary>
        /// تعیین نوع ناهمسانی نظیر
        /// </summary>
        /// <param name="dson"></param>
        /// <returns></returns>
        protected TypeStsDson GetPeerStateForRejectedDson(TypDson dson)
        {
            switch (dson)
            {
                case TypDson.NoDson:
                    break;
                case TypDson.OutSpcf:
                case TypDson.OutUnspcf:
                    return TypeStsDson.InSpcfFrom;

                case TypDson.SndOralFromSpcf:
                case TypDson.SndOralFromUnspcf:

                    return TypeStsDson.RcvOralToSpcf;
                //return TypeStsDson.SndOralFromSpcf;


                case TypDson.SndNewsFromSpcf:
                case TypDson.SndNewsFromUnspcf:
                    return TypeStsDson.RcvNewsToSpcf;


                case TypDson.InSpcf:
                case TypDson.InUnspcf:
                    return TypeStsDson.OutActSpcf;

                case TypDson.RcvOralInSpcf:
                case TypDson.RcvOralInUnspcf:
                    return TypeStsDson.SndOralFromSpcf;

                case TypDson.OutSpcfToSpcf:
                case TypDson.OutSpcfToUnspcf:
                    return TypeStsDson.InAct;

                case TypDson.SndOralFromSpcfToSpcf:
                case TypDson.SndOralFromSpcfToUnspcf:
                    return TypeStsDson.RcvOralIn;

                case TypDson.RcvNewsToSpcfFromSpcf:
                case TypDson.RcvNewsToSpcfFromUnspcf:
                    return TypeStsDson.SndNews;

                case TypDson.InSpcfFromSpcf:
                case TypDson.InSpcfFromUnspcf:
                    return TypeStsDson.OutAct;


                case TypDson.RcvOralToSpcfFromSpcf:
                case TypDson.RcvOralToSpcfFromUnspcf:
                    return TypeStsDson.SndOral;
                default:
                    break;
            }

            return TypeStsDson.NoDson;
        }

        protected bool CanSettleDson()
        {
            bool b = Acs_SttlDson;

            if (!b)
            {
                Util.ShowMessageBox(50, Util.GetNodTypeString((FldTypEty)SelectedNode.FldCodTypEty));
            }

            return b;
        }


        public bool Acs_SttlDson
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(SelectedNode, "Sttl", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "Dson");
                return PublicMethods.CurrentUser.AcsUsr["SttlDson"];
            }
        }

        #endregion

        #region ' events '

        internal event EventHandler FormClosed;

        #endregion


        public static TypDson GetPeerDson(TypDson dson)
        {
            return dson;
            switch (dson)
            {
                case TypDson.NoDson:
                    break;
                case TypDson.OutSpcf:
                    break;
                case TypDson.OutUnspcf:
                    return TypDson.OutSpcfToUnspcf;

                case TypDson.SndOralFromSpcf:
                    break;
                case TypDson.SndOralFromUnspcf:
                    break;
                case TypDson.SndNewsFromSpcf:
                    break;
                case TypDson.SndNewsFromUnspcf:
                    break;
                case TypDson.InSpcf:
                    break;
                case TypDson.InUnspcf:
                    break;
                case TypDson.RcvOralInSpcf:
                    break;
                case TypDson.RcvOralInUnspcf:
                    break;
                case TypDson.OutSpcfToSpcf:
                    break;
                case TypDson.OutSpcfToUnspcf:
                    break;
                case TypDson.SndOralFromSpcfToSpcf:
                    break;
                case TypDson.SndOralFromSpcfToUnspcf:
                    break;
                case TypDson.RcvNewsToSpcfFromSpcf:
                    break;
                case TypDson.RcvNewsToSpcfFromUnspcf:
                    break;
                case TypDson.InSpcfFromSpcf:
                    break;
                case TypDson.InSpcfFromUnspcf:
                    break;
                case TypDson.RcvOralToSpcfFromSpcf:
                    break;
                case TypDson.RcvOralToSpcfFromUnspcf:
                    break;
                default:
                    break;
            }
        }

    }

    public enum AssignStatusType
    {
        AssignedCorrectly = 0,
        WrongEvt = 1,
        WrongAct = 2
    }
}
