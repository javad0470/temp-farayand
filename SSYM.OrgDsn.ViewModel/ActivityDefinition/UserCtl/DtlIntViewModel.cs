using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SSYM.OrgDsn.ViewModel.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Data.Entity;
using System.Data.Objects;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class DtlIntViewModel : UserControlViewModel
    {
        #region ' Fields '

        //private Model.TblEvtSrt tblEvtSrt;
        //private Model.TblWayAwr_RecvInt tblWayAwr_RecvInt;
        //private Model.TblObj tblObj;
        private int previousActivityID;
        //private bool isObjectChanged;
        private bool isSelectInputPopupOpen;
        private bool isSelectSfwPopupOpen;
        private bool inputDoesntExist;
        private bool softwareDoesntExist;

        private ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> manualOrSoftware;
        private ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> compareTools;
        private ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> progressingNeeded;
        private ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> typeOfProcessing;
        private ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> wayOfProcessing;
        private SlcIntViewModel slcInt;
        private SlcSfwViewModel slcSfw;
        private SlcUntViewModel slcUnt;
        private DefIntViewModel defInt;
        Model.TblWayAwr_RecvInt _tblWayAwr_RecvInt;
        private int _codSelectedNod = 0;
        //parentVM.OrgPosVM.SelectedNode.FldCodEty

        #endregion

        #region ' Initialaizer '

        public DtlIntViewModel(BPMNDBEntities context, EntityObject obj, EntityObject obj2, int codSelectedNod)
            : base(context, obj, obj2)
        {
            this._codSelectedNod = codSelectedNod;

            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            SelectInputPopupIsOpenCommand = new DelegateCommand(ExecuteSelectInputPopupIsOpenCommand);
            SelectSfwPopupIsOpenCommand = new DelegateCommand(ExecuteSelectSfwPopupIsOpenCommand);
            SelectUntPopupIsOpenCommand = new DelegateCommand(ExecuteSelectUntPopupIsOpenCommand);
            SlcInt = new SlcIntViewModel(context);
            SlcSfw = new SlcSfwViewModel();
            SlcUnt = new SlcUntViewModel();
            DefInt = new DefIntViewModel(context, this._codSelectedNod, this.TblEvtSrt);
            DefSfw = new DefSfwViewModel();
            DefUnt = new DefUntViewModel();

        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
        }

        #endregion

        #region ' Properties / Commands '

        public bool IsInputSelected { get; set; }

        /// <summary>
        /// Way of ware
        /// </summary>
        public Model.TblWayAwr_RecvInt TblWayAwr_RecvInt
        {
            get
            {
                if ((Entity2 as TblWayAwr_RecvInt).TblWayIfrm_SndOut != null && !IsInputSelected)
                {
                    this.IsInputSelected = true;
                    RaisePropertyChanged("IsInputSelected");
                }
                else
                {
                    this.IsInputSelected = false;
                }


                return Entity2 as TblWayAwr_RecvInt;

            }
            set
            {
                Entity2 = value;
            }
        }

        /// <summary>
        /// Start event
        /// </summary>
        public Model.TblEvtSrt TblEvtSrt
        {
            get { return Entity as TblEvtSrt; }
            set
            {
                Entity = value;
                RaisePropertyChanged("TblEvtSrt");
            }

        }

        /// <summary>
        /// نوع ورودی از نوع نامه است؟
        /// </summary>
        public bool IsMail
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Mail ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Mail;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// نوع ورودی از نوع فرم است؟
        /// </summary>
        public bool IsForm
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Form ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Form;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// نوع ورودی از نوع فایل است؟
        /// </summary>
        public bool IsFile
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == 3 ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = 3;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// نوع ورودی از نوع کالا است؟
        /// </summary>
        public bool IsGoods
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Goods ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Goods;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// نوع ورودی از نوع نیروی انسانی است؟
        /// </summary>
        public bool IsHuman
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Human ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Human;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// نوع ورودی از نوع ساختمان و تأسیسات است؟
        /// </summary>
        public bool IsBuilding
        {
            get
            {
                if (TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                {
                    return TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Building ? true : false;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Building;
                }
                RaiseChanges();
            }
        }

        /// <summary>
        /// software name
        /// </summary>
        public string SoftwareName
        {
            get
            {
                Model.TblSfw tbl = bpmnEty.TblSfws.SingleOrDefault(E => E.FldCodSfw == TblWayAwr_RecvInt.FldCodSfw);
                if (tbl != null)
                {
                    return bpmnEty.TblSfws.SingleOrDefault(E => E.FldCodSfw == TblWayAwr_RecvInt.FldCodSfw).FldNamSfw;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// عنوان واحد سنجش
        /// </summary>
        public string NamUntMsrt
        {
            get
            {
                if (TblWayAwr_RecvInt.FldCodUntMsrtInt != 0)
                {
                    return bpmnEty.TblUntMsrts.SingleOrDefault(E => E.FldCodUntMsrt == TblWayAwr_RecvInt.FldCodUntMsrtInt).FldNamUntMsrt;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// manual or with software
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> ManualOrSoftware
        {
            get
            {
                if (manualOrSoftware == null)
                    manualOrSoftware = new ObservableCollection<Model.TblItmFixSfw>(bpmnEty.TblItmFixSfws.Where(E => E.FldCodSbj == 6));
                return manualOrSoftware;
            }
        }

        /// <summary>
        /// manual of software selected value
        /// </summary>
        public SSYM.OrgDsn.Model.TblItmFixSfw ManualOrSoftwareSelectedValue
        {
            get
            {
                if (TblWayAwr_RecvInt.FldCodSfw == null)
                {
                    return ManualOrSoftware.SingleOrDefault(E => E.FldCodSbj == 6 && E.FldCodItm == (int)Model.Enum.ManualOrSoftware.Manual);
                }
                return ManualOrSoftware.SingleOrDefault(E => E.FldCodSbj == 6 && E.FldCodItm == (int)Model.Enum.ManualOrSoftware.Software);
            }
            set
            {
                if (value.FldCodItm == 1)
                {
                    TblWayAwr_RecvInt.FldCodSfw = null;
                    RaisePropertyChanged("SoftwareName");
                }
                if (value.FldCodItm == 2)
                {
                    TblWayAwr_RecvInt.FldCodSfw = 0;
                }
                RaisePropertyChanged("ManualOrSoftwareSelectedValue");
            }
        }

        /// <summary>
        /// compare tools
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> CompareTools
        {
            get
            {
                if (compareTools == null)
                    compareTools = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 7));
                return compareTools;
            }
        }

        /// <summary>
        /// نیاز به پردازش دارد یا ندارد
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> ProgressingNeeded
        {
            get
            {
                if (progressingNeeded == null)
                    progressingNeeded = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 19));
                return progressingNeeded;
            }
        }

        /// <summary>
        /// نوع پردازش
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> TypeOfProcessing
        {
            get
            {
                if (typeOfProcessing == null)
                    typeOfProcessing = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 17));
                return typeOfProcessing;
            }
        }

        /// <summary>
        /// نحوه پردازش
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> WayOfProcessing
        {
            get
            {
                if (wayOfProcessing == null)
                    wayOfProcessing = new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 4));
                return wayOfProcessing;
            }
        }

        /// <summary>
        /// save changes command
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// opens popup for input selection command
        /// </summary>
        public ICommand SelectInputPopupIsOpenCommand { get; set; }

        /// <summary>
        /// opens popup for software selection command
        /// </summary>
        public ICommand SelectSfwPopupIsOpenCommand { get; set; }

        /// <summary>
        /// opens popup for unit selection command
        /// </summary>
        public ICommand SelectUntPopupIsOpenCommand { get; set; }

        /// <summary>
        /// Is select input popup open
        /// </summary>
        public bool IsSelectInputPopupOpen
        {
            get
            {
                return isSelectInputPopupOpen;
            }
            set
            {
                isSelectInputPopupOpen = value;
                //RaisePropertyChanged("IsSelectInputPopupOpen");
                //if (!value && this.SlcInt.Result == Base.PopupResult.OK)
                //{
                //    TblObj tblObj;
                //    if (this.SlcInt.IsSentTooMeSelected)
                //    {
                //        tblObj = this.bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == ((TblObj)this.SlcInt.SentToMeObjectsSelectedItem.Item1).FldCodObj);
                //    }
                //    else
                //    {
                //        tblObj = this.bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == ((TblObj)this.SlcInt.SentToMyCurrentActObjectsSelectedItem.Item1).FldCodObj);
                //    }

                //    if (this.TblWayAwr_RecvInt.TblEvtSrt == null)
                //    {
                //        this.TblWayAwr_RecvInt.TblEvtSrt = this.TblEvtSrt;
                //    }

                //    //PublicMethods.SelectExistingObjRstForWayAwr(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt);

                //    PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt);

                //    RaisePropertyChanged("SoftwareName", "NamUntMsrt", "TblWayAwr_RecvInt", "TblEvtSrt");
                //    RaiseChanges();
                //}
                //if (!value && this.SlcInt.Result == Base.PopupResult.Yes)
                //{

                //    this.InputDoesntExist = true;
                //}
            }
        }


        /// <summary>
        /// SlcIntViewModel
        /// </summary>
        public SlcIntViewModel SlcInt
        {
            get
            {
                return slcInt;
            }
            set
            {
                slcInt = value;
            }
        }

        /// <summary>
        /// SlcSfwViewModel
        /// </summary>
        public SlcSfwViewModel SlcSfw
        {
            get { return slcSfw; }
            set { slcSfw = value; }
        }

        /// <summary>
        /// SlcSfwViewModel
        /// </summary>
        public SlcUntViewModel SlcUnt
        {
            get { return slcUnt; }
            set { slcUnt = value; }
        }

        /// <summary>
        /// DefIntViewModel
        /// </summary>
        public DefIntViewModel DefInt
        {
            get { return defInt; }
            set { defInt = value; }
        }

        /// <summary>
        /// DefUntViewModel
        /// </summary>
        public Popup.DefUntViewModel DefUnt { get; set; }

        /// <summary>
        /// DefSfwViewModel
        /// </summary>
        public Popup.DefSfwViewModel DefSfw { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            SlcInt.Dispose();

            SlcSfw.Dispose();

            SlcUnt.Dispose();

            DefInt.Dispose();

            DefSfw.Dispose();

            DefUnt.Dispose();
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// Raises all changes of radioButtons
        /// </summary>
        private void RaiseChanges()
        {
            RaisePropertyChanged("IsMail");
            RaisePropertyChanged("IsForm");
            RaisePropertyChanged("IsFile");
            RaisePropertyChanged("IsGoods");
            RaisePropertyChanged("IsHuman");
            RaisePropertyChanged("IsBuilding");
        }

        /// <summary>
        /// save changes
        /// </summary>
        private void ExecuteSaveChangesCommand()
        {
            PublicMethods.SaveContext(bpmnEty);
        }

        /// <summary>
        /// execute select input popoup command
        /// </summary>
        private void ExecuteSelectInputPopupIsOpenCommand()
        {
            //this.SlcInt.CurrentNodID = this.TblEvtSrt.TblAct.FldCodNod;

            //PublicMethods.SaveContext(bpmnEty);

            this.SlcInt.ActCnt = this.TblEvtSrt.TblAct;

            this.SlcInt.EvtSrt = this.TblEvtSrt;

            if (this.TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
            {
                this.SlcInt.CurrentObjID = this.TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj.FldCodObj;
            }

            this.SlcInt.PreviousActivity = this.TblEvtSrt.PreviousActivity;


            //Pdr9053(1)
            //if (PublicMethods.DetectWayAwrOfEvtSrtWithWayIfrm_503(this.bpmnEty, this.TblEvtSrt).Count > 0)
            //{
            //    this.SlcInt.PreviousActivity = this.TblEvtSrt.PreviousActivity;
            //}
            //else
            //{
            //    this.SlcInt.PreviousActivity = null;
            //}

            this.SlcInt.IsSentToMyCurrentActSelected = true;

            this.SlcInt.IsSentTooMeSelected = false;


            Util.ShowPopup(this.SlcInt);

            if (this.SlcInt.Result == Base.PopupResult.OK)
            {
                TblObj tblObj = null;
                if (this.SlcInt.IsSentTooMeSelected)
                {
                    if (this.SlcInt.SentToMeObjectsSelectedItem != null)
                    {
                        // برای تمامی رخداد های آغازگر غیر از صرفا پس از آگاهی چه ورودی جه خبر و جه مطلب شفاهی، فعالیت مبداء نامشخص قابل انتخاب نباشد
                        if (SlcInt.SentToMeObjectsSelectedItem.Item1.ActSrc.FldActUspf && this.TblEvtSrt.TypSrt != Model.Enum.EvtSrtType.aftrAwareEvtSrt)
                        {
                            Util.ShowMessageBox(75);
                            return;
                        }
                        tblObj = this.bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == ((TblObj)this.SlcInt.SentToMeObjectsSelectedItem.Item1).FldCodObj);
                    }
                }
                else
                {
                    if (this.SlcInt.SentToMyCurrentActObjectsSelectedItem != null)
                    {
                        // برای تمامی رخداد های آغازگر غیر از صرفا پس از آگاهی چه ورودی جه خبر و جه مطلب شفاهی، فعالیت مبداء نامشخص قابل انتخاب نباشد
                        if (SlcInt.SentToMyCurrentActObjectsSelectedItem.Item1.ActSrc.FldActUspf && this.TblEvtSrt.TypSrt != Model.Enum.EvtSrtType.aftrAwareEvtSrt)
                        {
                            Util.ShowMessageBox(75);
                            return;
                        }

                        tblObj = this.bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == ((TblObj)this.SlcInt.SentToMyCurrentActObjectsSelectedItem.Item1).FldCodObj);
                    }
                }

                if (this.TblWayAwr_RecvInt.TblEvtSrt == null)
                {
                    this.TblWayAwr_RecvInt.TblEvtSrt = this.TblEvtSrt;
                }

                //PublicMethods.SelectExistingObjRstForWayAwr(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt);


                if (tblObj != null)
                {
                    PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt);
                }

                RaisePropertyChanged("SoftwareName", "NamUntMsrt", "TblWayAwr_RecvInt", "TblEvtSrt");
                RaiseChanges();
            }

            else if (this.SlcInt.Result == Base.PopupResult.Yes)
            {

                this.DefInt.PreviousActivity = this.TblEvtSrt.PreviousActivity;

                this.DefInt.EvtSrt = this.TblEvtSrt;
                if (TblWayAwr_RecvInt != null && TblWayAwr_RecvInt.TblWayIfrm_SndOut != null)
                    this.DefInt.TblObj = TblWayAwr_RecvInt.TblWayIfrm_SndOut.TblObj;

                if (this.TblEvtSrt.PreviousActivity != null)
                {
                    SlcActOfNodViewModel temp = new SlcActOfNodViewModel(this.bpmnEty, this.TblEvtSrt.PreviousActivity.TblNod, false, this.TblEvtSrt.PreviousActivity.FldCodAct);
                    this.DefInt.SlcActOfNodVM = temp;
                }
                //در صورتی که رخداد آغازگر جاری بیش از یک نحوه آگاهی داشته باشد امکان تغییر مبداء شیء ورودی وجود ندارد
                if ((this.TblEvtSrt.TblWayAwr_RecvInt.Count() >= 1 && this.TblWayAwr_RecvInt.TblEvtSrt != this.TblEvtSrt) ||
                    this.TblEvtSrt.TblWayAwr_Oral.Count > 0 ||
                    this.TblEvtSrt.TblWayAwr_News.Count > 0)
                {
                    this.DefInt.IsSelectSourceEnabel = false;
                }
                else
                {
                    this.DefInt.IsSelectSourceEnabel = this.DefInt.PreviousActivity==null;
                    if (this.DefInt.PreviousActivity != null)
                        this.DefInt.IsSelectActEnable = true;
                }

                Util.ShowPopup(DefInt);

                if (this.DefInt.Result == PopupResult.OK)
                {
                    TblObj tblObj = new TblObj() { FldNamObj = this.DefInt.TblObj.FldNamObj, FldTypObj = this.DefInt.TblObj.FldTypObj };

                    this.bpmnEty.TblObjs.AddObject(tblObj);


                    if (this.TblWayAwr_RecvInt.TblEvtSrt == null)
                    {
                        this.TblWayAwr_RecvInt.TblEvtSrt = this.TblEvtSrt;
                    }


                    TblAct act = this.bpmnEty.TblActs.Single(m => m.FldCodAct == this.DefInt.PreviousActivity.FldCodAct);

                    this.TblEvtSrt.PreviousActivity = act;


                    //PublicMethods.AddNewObjRstToWayAwr_1017(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt, this.DefInt.PreviousActivity);

                    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, tblObj, this.TblWayAwr_RecvInt, act);


                    //PublicMethods.SaveContext(bpmnEty);
                    RaisePropertyChanged("SoftwareName", "NamUntMsrt", "TblWayAwr_RecvInt", "TblEvtSrt");
                    RaiseChanges();
                }

            }
        }

        /// <summary>
        /// execute select software popoup command
        /// </summary>
        private void ExecuteSelectSfwPopupIsOpenCommand()
        {
            SlcSfw.IsSelectionModeSingle = true;
            //SlcSfw.SelectedItem = bpmnEty.TblSfws.SingleOrDefault(E => E.FldCodSfw == TblWayAwr_RecvInt.FldCodSfw);
            this.SlcSfw.DetectAllSfws();

            Util.ShowPopup(this.SlcSfw);

            if (this.SlcSfw.Result == PopupResult.OK)
            {
                this.TblWayAwr_RecvInt.FldCodSfw = this.SlcSfw.SelectedItem.FldCodSfw;
            }
            if (this.SlcSfw.Result == PopupResult.Yes)
            {
                Util.ShowPopup(DefSfw);

                if (this.DefSfw.Result == Base.PopupResult.OK)
                {
                    TblSfw sfw = new TblSfw() { FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldNamSfw = this.DefSfw.TblSfw.FldNamSfw };

                    this.bpmnEty.TblSfws.AddObject(sfw);

                    PublicMethods.SaveContext(this.bpmnEty);

                    this.TblWayAwr_RecvInt.FldCodSfw = sfw.FldCodSfw;

                    RaisePropertyChanged("SoftwareName", "ManualOrSoftwareSelectedValue");
                }
            }
            RaisePropertyChanged("TblWayAwr_RecvInt", "SoftwareName");
        }

        /// <summary>
        /// execute select software popoup command
        /// </summary>
        private void ExecuteSelectUntPopupIsOpenCommand()
        {
            Util.ShowPopup(SlcUnt);

            if (this.SlcUnt.Result == PopupResult.OK)
            {
                this.TblWayAwr_RecvInt.FldCodUntMsrtInt = this.SlcUnt.SelectedItem.FldCodUntMsrt;
            }
            if (this.SlcUnt.Result == PopupResult.Yes)
            {
                Util.ShowPopup(this.DefUnt);
                if (this.DefUnt.Result == PopupResult.OK)
                {
                    this.TblWayAwr_RecvInt.FldCodUntMsrtInt = this.DefUnt.TblUntMsrt.FldCodUntMsrt;
                    RaisePropertyChanged("NamUntMsrt");
                }

            }

            RaisePropertyChanged("TblWayAwr_RecvInt");
            RaisePropertyChanged("NamUntMsrt");

        }

        /// <summary>
        /// حذف رخداد نتیجه معادل به رخداد آغازگر جاری
        /// </summary>
        /// <param name="tbl"></param>
        private void DeleteEvtRst(TblWayIfrm_SndOut tbl)
        {
            if (tbl.TblObj.TblEvtRst.TblObjs.Count == 1 && tbl.TblObj.TblEvtRst.TblNews.Count == 0 && tbl.TblObj.TblEvtRst.TblSbjOrals.Count == 0)
            {
                this.bpmnEty.DeleteObject(tbl.TblObj.TblEvtRst);
            }
            else
            {
                this.bpmnEty.DeleteObject(tbl.TblObj);
            }

        }

        #endregion

    }
}
