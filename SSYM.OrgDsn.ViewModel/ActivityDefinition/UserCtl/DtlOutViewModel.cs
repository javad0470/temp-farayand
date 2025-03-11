using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    class CompareActDst : IEqualityComparer<TblWayIfrm_SndOut>
    {
        public bool Equals(TblWayIfrm_SndOut x, TblWayIfrm_SndOut y)
        {
            return x.ActDst.FldCodAct == y.ActDst.FldCodAct;
        }

        public int GetHashCode(TblWayIfrm_SndOut obj)
        {
            return obj.FldCodActDst.GetHashCode();
        }
    }

    public class DtlOutViewModel : UserControlViewModel
    {
        #region ' Fields '

        private Model.TblEvtRst tblEvtRst;
        private Model.TblObj tblObj;

        private ObservableCollection<Model.TblWayIfrm_SndOut> tblWayIfrm_SndOut;
        private Model.TblItmFixSfw compareToolsSelectedValue;
        private bool isSelectOutputOpen;
        private bool outputDoesntExist;
        private bool isAddDestinationOpen;
        private SlcSfwViewModel slcSfw;
        private bool isSelectSfwPopupOpen;
        private SSYM.OrgDsn.Model.TblItmFixSfw manualOrSoftwareSelectedValue;
        private bool softwareDoesntExist;
        private SlcUntViewModel slcUnt;
        private bool isSelectUntPopupOpen;
        private bool unitDoesntExist;
        private bool isConfirmDestinationOpen;
        int _codSelectedNod = 0;
        #endregion

        #region ' Initialaizer '

        public DtlOutViewModel(BPMNDBEntities context, EntityObject obj, int codSelectedNod)
            : base(context, obj)
        {
            _codSelectedNod = codSelectedNod;
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();

            //TblObj = bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == 399);
            SelectOutputCommand = new DelegateCommand(ExecuteSelectOutputCommand);
            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            AddDestinationCommand = new DelegateCommand(ExecuteAddDestinationCommand);
            DeleteDestinationCommand = new DelegateCommand<Model.TblWayIfrm_SndOut>(ExecuteDeleteDestinationCommand);
            SelectSfwPopupIsOpenCommand = new DelegateCommand(ExecuteSelectSfwPopupIsOpenCommand);
            SlcOut = new Popup.SlcOutViewModel();
            DefOut = new Popup.DefOutViewModel();
            SlcSrcAndDst = new Popup.SlcSrcAndDstViewModel();
            SlcSfw = new SlcSfwViewModel();
            DefSfw = new DefSfwViewModel();
            SelectUntPopupIsOpenCommand = new DelegateCommand(ExecuteSelectUntPopupIsOpenCommand);
            SlcUnt = new SlcUntViewModel();
            DefUnt = new DefUntViewModel();
            SlcDstForOut = new SlcDstForOutViewModel();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// TblEvtRst
        /// </summary>
        //public Model.TblEvtRst TblEvtRst
        //{
        //    get { return tblEvtRst; }
        //    set { tblEvtRst = value; }
        //}

        public bool IsOutputSelected { get; set; }

        /// <summary>
        /// output
        /// </summary>
        public Model.TblObj TblObj
        {
            get
            {
                if ((Entity as TblObj).FldNamObj != null)
                {
                    IsOutputSelected = true;
                    RaisePropertyChanged("IsOutputSelected");
                }
                else
                {
                    IsOutputSelected = false;
                    RaisePropertyChanged("IsOutputSelected");
                }
                return Entity as TblObj;
            }
            set
            {
                Entity = value;
            }
        }


        ObservableCollection<Model.TblWayIfrm_SndOut> wayIfrms;

        /// <summary>
        /// TblWayIfrm_SndOut
        /// </summary>
        public ObservableCollection<Model.TblWayIfrm_SndOut> TblWayIfrm_SndOut
        {
            get
            {
                if (wayIfrms == null)
                {
                    wayIfrms = new ObservableCollection<TblWayIfrm_SndOut>(this.TblObj.TblWayIfrm_SndOut.Distinct(new CompareActDst()));

                    wayIfrms.CollectionChanged -= wayIfrms_CollectionChanged;
                    wayIfrms.CollectionChanged += wayIfrms_CollectionChanged;
                    RaisePropertyChanged("DtlVisible");
                }


                return wayIfrms;
                //return new ObservableCollection<Model.TblWayIfrm_SndOut>();
            }
            //set
            //{
            //    tblWayIfrm_SndOut = value;
            //    RaisePropertyChanged("TblWayIfrm_SndOut");
            //}
        }

        void wayIfrms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("DtlVisible");
        }

        bool _dtlVisible;

        public bool DtlVisible
        {
            get
            {
                return TblWayIfrm_SndOut.Count > 0;
            }
        }

        /// <summary>
        /// نوع خروجی از نوع نامه است؟
        /// </summary>
        public bool IsMail
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Mail ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Mail;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// نوع خروجی از نوع فرم است؟
        /// </summary>
        public bool IsForm
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Form ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Form;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// نوع خروجی از نوع فایل است؟
        /// </summary>
        public bool IsFile
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.File ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.File;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// نوع خروجی از نوع کالا است؟
        /// </summary>
        public bool IsGoods
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Goods ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Goods;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// نوع خروجی از نوع نیروی انسانی است؟
        /// </summary>
        public bool IsHuman
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Human ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Human;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// نوع خروجی از نوع ساختمان و تأسیسات است؟
        /// </summary>
        public bool IsBuilding
        {
            get
            {
                return TblObj.FldTypObj == (int)Model.Enum.ObjectTypes.Building ? true : false;
            }
            set
            {
                if (value)
                {
                    TblObj.FldTypObj = (int)Model.Enum.ObjectTypes.Building;
                }
                RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding");
            }
        }

        /// <summary>
        /// software name
        /// </summary>
        public string SoftwareName
        {
            get
            {
                Model.TblWayIfrm_SndOut tbl = TblWayIfrm_SndOut.FirstOrDefault();
                if (tbl != null)
                {
                    int softwareID = tbl.FldCodSfw ?? 0;
                    if (softwareID != 0)
                    {
                        return bpmnEty.TblSfws.SingleOrDefault(E => E.FldCodSfw == softwareID).FldNamSfw;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// عنوان واحد سنجش
        /// </summary>
        public string NamUntMsrt
        {
            get
            {
                Model.TblWayIfrm_SndOut tbl = TblWayIfrm_SndOut.FirstOrDefault();
                if (tbl != null)
                {
                    int codUnt = tbl.FldCodUntMsrtOut;

                    if (codUnt != 0)
                    {
                        return bpmnEty.TblUntMsrts.SingleOrDefault(E => E.FldCodUntMsrt == codUnt).FldNamUntMsrt;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// manual or with software
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> ManualOrSoftware
        {
            get
            {
                return new ObservableCollection<Model.TblItmFixSfw>(bpmnEty.TblItmFixSfws.Where(E => E.FldCodSbj == 6));
            }
        }

        Model.Enum.ManualOrSoftware? _selectedSendType = null;

        /// <summary>
        /// manual of software selected value
        /// </summary>
        public SSYM.OrgDsn.Model.TblItmFixSfw ManualOrSoftwareSelectedValue
        {
            get
            {
                //if (_selectedSendType.HasValue)
                //{
                //    return ManualOrSoftware.SingleOrDefault(E => E.FldCodSbj == 6 && E.FldCodItm == (int)_selectedSendType.Value);
                //}
                //else
                //{
                //    return null;
                //}


                if (TblWayIfrm_SndOut.Count() > 0)
                {
                    Model.TblWayIfrm_SndOut tbl = TblWayIfrm_SndOut.FirstOrDefault();
                    if (tbl != null)
                    {
                        if (tbl.FldCodSfw != null)
                        {
                            return ManualOrSoftware.SingleOrDefault(E => E.FldCodSbj == 6 && E.FldCodItm == (int)Model.Enum.ManualOrSoftware.Software);
                        }
                        else
                        {
                            return ManualOrSoftware.SingleOrDefault(E => E.FldCodSbj == 6 && E.FldCodItm == (int)Model.Enum.ManualOrSoftware.Manual);
                        }
                    }
                    return ManualOrSoftware.FirstOrDefault();
                }
                return ManualOrSoftware.FirstOrDefault();
            }
            set
            {
                //_selectedSendType = (Model.Enum.ManualOrSoftware?)value.FldCodItm;

                if (value.FldCodItm == 1)
                {
                    foreach (var item in TblWayIfrm_SndOut)
                    {
                        item.FldCodSfw = null;
                    }
                    RaisePropertyChanged("SoftwareName");
                }
                if (value.FldCodItm == 2)
                {
                    foreach (var item in TblWayIfrm_SndOut)
                    {
                        item.FldCodSfw = 0;
                    }
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
                return new ObservableCollection<Model.TblItmFixSfw>(bpmnEty.TblItmFixSfws.Where(E => E.FldCodSbj == 7));
            }
        }

        /// <summary>
        /// CompareToolsSelectedValue
        /// </summary>
        public Model.TblItmFixSfw CompareToolsSelectedValue
        {
            get
            {
                if (TblWayIfrm_SndOut.Count() > 0)
                {
                    int i = TblWayIfrm_SndOut.FirstOrDefault().FldCodCmrOutPerSnd;
                    return bpmnEty.TblItmFixSfws.SingleOrDefault(E => E.FldCodItm == i && E.FldCodSbj == 7);
                }
                else
                    return null;
            }
            set
            {
                foreach (var item in TblWayIfrm_SndOut)
                {
                    item.FldCodCmrOutPerSnd = value.FldCodItm;
                }
            }
        }

        /// <summary>
        /// تعداد خروجی های ارسال شده در هر بار ارسال
        /// </summary>
        public double TnoOutPerSnd
        {
            get
            {
                if (TblWayIfrm_SndOut.Count() > 0)
                {
                    return this.TblWayIfrm_SndOut.FirstOrDefault().FldTnoOutPerSnd;
                }
                else
                    return 0;
            }
            set
            {
                foreach (var item in TblWayIfrm_SndOut)
                {
                    item.FldTnoOutPerSnd = value;
                }
                RaisePropertyChanged("TnoOutPerSnd");
            }
        }

        /// <summary>
        /// توضیحات ارسال خروجی
        /// </summary>
        public string Description
        {
            get
            {
                if (TblWayIfrm_SndOut.Count() > 0)
                {
                    return this.TblWayIfrm_SndOut.FirstOrDefault().FldDsc;
                }
                else
                    return string.Empty;
            }
            set
            {
                foreach (var item in TblWayIfrm_SndOut)
                {
                    item.FldDsc = value;
                }
                RaisePropertyChanged("Description");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SlcDstForOutViewModel SlcDstForOut { get; set; }

        /// <summary>
        /// SelectOutputCommand
        /// </summary>
        public ICommand SelectOutputCommand { get; set; }

        /// <summary>
        /// SlcOutViewModel
        /// </summary>
        public ViewModel.ActivityDefinition.Popup.SlcOutViewModel SlcOut { get; set; }

        /// <summary>
        /// SaveChangesCommand
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// SoftwareDoesntExist
        /// </summary>
        public bool SoftwareDoesntExist
        {
            get { return softwareDoesntExist; }
            set
            {
                softwareDoesntExist = value;
                RaisePropertyChanged("SoftwareDoesntExist");
                if (!value && this.DefSfw.Result == PopupResult.OK)
                {
                    TblSfw sfw = new TblSfw() { FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldNamSfw = this.DefSfw.TblSfw.FldNamSfw };

                    this.bpmnEty.TblSfws.AddObject(sfw);

                    PublicMethods.SaveContext(this.bpmnEty);

                    foreach (var item in this.TblWayIfrm_SndOut)
                    {
                        item.FldCodSfw = sfw.FldCodSfw;
                    }

                    RaisePropertyChanged("SoftwareName", "ManualOrSoftwareSelectedValue");
                }
            }
        }

        /// <summary>
        /// UnitDoesntExist
        /// </summary>
        public bool UnitDoesntExist
        {
            get { return unitDoesntExist; }
            set
            {
                unitDoesntExist = value;
                RaisePropertyChanged("UnitDoesntExist");
                if (!value && this.DefUnt.Result == PopupResult.OK)
                {
                    foreach (var item in this.TblWayIfrm_SndOut)
                    {
                        item.FldCodUntMsrtOut = DefUnt.TblUntMsrt.FldCodUntMsrt;
                    }
                    RaisePropertyChanged("NamUntMsrt");
                }
            }
        }

        /// <summary>
        /// DefOutViewModel
        /// </summary>
        public Popup.DefOutViewModel DefOut { get; set; }

        /// <summary>
        /// DefSfwViewModel
        /// </summary>
        public Popup.DefSfwViewModel DefSfw { get; set; }

        /// <summary>
        /// DefUntViewModel
        /// </summary>
        public Popup.DefUntViewModel DefUnt { get; set; }

        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public Popup.SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

        /// <summary>
        /// AddDestinationCommand
        /// </summary>
        public ICommand AddDestinationCommand { get; set; }

        /// <summary>
        /// حذف مقصد خروجی
        /// </summary>
        public ICommand DeleteDestinationCommand { get; set; }

        /// <summary>
        /// opens popup for software selection command
        /// </summary>
        public ICommand SelectSfwPopupIsOpenCommand { get; set; }

        /// <summary>
        /// SlcSfwViewModel
        /// </summary>
        public SlcSfwViewModel SlcSfw
        {
            get { return slcSfw; }
            set { slcSfw = value; }
        }

        ///// <summary>
        ///// Is select software popup open
        ///// </summary>
        //public bool IsSelectSfwPopupOpen
        //{
        //    get { return isSelectSfwPopupOpen; }
        //    set
        //    {
        //        isSelectSfwPopupOpen = value;
        //        if (!value && this.SlcSfw.Result == PopupResult.OK)
        //        {
        //            foreach (var item in this.TblWayIfrm_SndOut)
        //            {
        //                item.FldCodSfw = this.SlcSfw.SelectedItem.FldCodSfw;
        //            }
        //            RaisePropertyChanged("TblWayIfrm_SndOut", "SoftwareName");
        //        }
        //        if (!value && this.SlcSfw.Result == PopupResult.Yes)
        //        {
        //            this.SoftwareDoesntExist = true;
        //        }
        //        RaisePropertyChanged("IsSelectSfwPopupOpen");
        //    }
        //}

        /// <summary>
        /// SlcSfwViewModel
        /// </summary>
        public SlcUntViewModel SlcUnt
        {
            get { return slcUnt; }
            set { slcUnt = value; }
        }

        /// <summary>
        /// Is select measurement unit popup open
        /// </summary>
        public bool IsSelectUntPopupOpen
        {
            get { return isSelectUntPopupOpen; }
            set
            {
                isSelectUntPopupOpen = value;
                if (!value && this.SlcUnt.Result == PopupResult.OK)
                {
                    foreach (var item in this.TblWayIfrm_SndOut)
                    {
                        item.FldCodUntMsrtOut = SlcUnt.SelectedItem.FldCodUntMsrt;
                    }
                }
                if (!value && this.SlcUnt.Result == PopupResult.Yes)
                {
                    this.UnitDoesntExist = true;
                }

                RaisePropertyChanged("IsSelectUntPopupOpen");
                RaisePropertyChanged("TblWayIfrm_SndOut");
                RaisePropertyChanged("NamUntMsrt");
            }
        }

        /// <summary>
        /// opens popup for unit selection command
        /// </summary>
        public ICommand SelectUntPopupIsOpenCommand { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            SlcOut.Dispose();

            DefOut.Dispose();

            SlcSrcAndDst.Dispose();

            SlcSfw.Dispose();

            DefSfw.Dispose();

            SlcUnt.Dispose();

            DefUnt.Dispose();

            SlcDstForOut.Dispose();
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void RaiseProperties()
        {
            RaisePropertyChanged("TblWayIfrm_SndOut", "TblObj", "ManualOrSoftwareSelectedValue");
            RaisePropertyChanged("IsMail", "IsForm", "IsFile", "IsGoods", "IsHuman", "IsBuilding", "SoftwareName", "NamUntMsrt", "ManualOrSoftwareSelectedValue");
            RaisePropertyChanged("CompareToolsSelectedValue", "TnoOutPerSnd", "Description");
        }

        /// <summary>
        /// ExecuteSelectOutputCommand
        /// </summary>
        private void ExecuteSelectOutputCommand()
        {
            //SlcOut.Parent = this;
            this.SlcOut.ActSrc = this.TblObj.TblEvtRst.TblAct;
            this.SlcOut.ObjCnt = this.TblObj;
            this.SlcOut.IsFomActCntSlcEed = true;
            this.SlcOut.IsFomMeeSlcEed = false;


            Util.ShowPopup(this.SlcOut);

            if (this.SlcOut.Result == PopupResult.OK)
            {
                TblObj objRst = null;

                if (this.SlcOut.IsFomMeeSlcEed)
                {
                    objRst = this.bpmnEty.TblObjs.SingleOrDefault(m => m.FldCodObj == this.SlcOut.FomMeeSlcEedItm.Item1.CodObj);
                }
                else
                {
                    objRst = this.bpmnEty.TblObjs.SingleOrDefault(m => m.FldCodObj == this.SlcOut.FomActCntSlcEedItm.Item1.CodObj);
                }

                this.SlcDstForOut.ObjCnt = objRst;

                ///در صورتی که خروجی انتخاب شده یک خروجی نسبت داده شده به فعالیت نامشخص همین واحد باشد نباید فرم تأیید مقصد برای خروجی ها نشان داده شود
                if (objRst.TblEvtRst.TblAct.FldActUspf)
                {
                    List<IWayIfrm> wayIfrm = PublicMethods.DetectWayIfrmObj_589(bpmnEty, objRst).ToList<IWayIfrm>();

                    foreach (IWayIfrm item in wayIfrm)
                    {
                        item.ActDst.IsSelectedAsDstForObjRst = true;
                    }

                    addObjRst(objRst, this.TblObj);
                }

                else
                {
                    showConfirm();
                }
            }

            else if (this.SlcOut.Result == PopupResult.Yes)
            {
                Util.ShowPopup(this.DefOut);

                if (this.DefOut.Result == PopupResult.OK)
                {
                    TblObj objRst = this.TblObj;

                    this.TblObj.FldNamObj = this.DefOut.TblObj.FldNamObj;

                    this.TblObj.FldTypObj = 1;

                    List<TblWayIfrm_SndOut> lst = PublicMethods.DetectWayIfrmObj_589(bpmnEty, objRst);

                    foreach (TblWayIfrm_SndOut item in lst)
                    {
                        PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst, item, DirectionForDelete.Left);
                    }

                    RaiseProperties();
                }
            }
            wayIfrms = null;
            RaisePropertyChanged("TblWayIfrm_SndOut");
        }

        private void showConfirm()
        {
            TblObj objRst = null;
            TblObj objRst2 = this.TblObj;

            if (this.SlcOut.IsFomMeeSlcEed)
            {
                objRst = this.bpmnEty.TblObjs.SingleOrDefault(m => m.FldCodObj == this.SlcOut.FomMeeSlcEedItm.Item1.CodObj);
            }
            else
            {
                objRst = this.bpmnEty.TblObjs.SingleOrDefault(m => m.FldCodObj == this.SlcOut.FomActCntSlcEedItm.Item1.CodObj);
            }

            List<TblWayIfrm_SndOut> lst = PublicMethods.DetectWayIfrmObj_589(bpmnEty, objRst2);

            foreach (TblWayIfrm_SndOut item in lst)
            {
                PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst2, item, DirectionForDelete.Left);
            }

            Util.ShowPopup(this.SlcDstForOut);

            if (this.SlcDstForOut.Result == PopupResult.OK)
            {
                addObjRst(objRst, objRst2);
            }

            else if (this.SlcDstForOut.Result == PopupResult.Cancel)
            {
                this.TblObj.FldNamObj = objRst.FldNamObj;

                RaiseProperties();
            }
        }

        private void addObjRst(TblObj objRst, TblObj objRst2)
        {
            List<IWayIfrm> wayIfrm2 = new List<IWayIfrm>();
            TblEvtRst evtRst = this.TblObj.TblEvtRst;
            List<IWayIfrm> wayIfrm = null;
            wayIfrm = PublicMethods.DetectWayIfrmObj_589(bpmnEty, objRst).ToList<IWayIfrm>();

            foreach (IWayIfrm item in wayIfrm)
            {
                if (item.ActDst.IsSelectedAsDstForObjRst)
                {
                    this.TblWayIfrm_SndOut.Add(item as TblWayIfrm_SndOut);
                    wayIfrm2.Add(item);
                }
            }

            PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(bpmnEty, objRst, evtRst, objRst2, wayIfrm2);

            foreach (IWayIfrm item in wayIfrm)
            {
                item.ActDst.IsSelectedAsDstForObjRst = false;
            }

            RaiseProperties();

        }

        /// <summary>
        /// ExecuteSaveChangesCommand
        /// </summary>
        private void ExecuteSaveChangesCommand()
        {
            PublicMethods.SaveContext(bpmnEty);
        }

        /// <summary>
        /// ExecuteAddDestinationCommand
        /// </summary>
        private void ExecuteAddDestinationCommand()
        {
            var vm = new SlcNodAndActViewModel(bpmnEty, _codSelectedNod, this.TblObj.ActSrc.FldCodAct);

            vm.LblAct = vm.LblNod = "مقصد";
            vm.LblObj = "خروجی";

            Util.ShowPopup(vm);

            if (vm.Result == PopupResult.OK)
            {
                if (vm.SelectedAct != null)
                {
                    AddDestination(vm.SelectedAct.FldCodAct, vm.SelectedAct.FldCodNod);
                }
            }

            //SlcSrcAndDst.IsSelectionModeSingle = true;

            //Util.ShowPopup(SlcSrcAndDst);

            //if (SlcSrcAndDst.SelectedItem != null && this.SlcSrcAndDst.Result == PopupResult.OK)
            //{
            //    TblNod nod = SlcSrcAndDst.SelectedItem.Nod;
            //    TblNod selectedNod = bpmnEty.TblNods.Single(n => n.FldCodNod == _codSelectedNod);

            //    if (nod.FldCodNod == _codSelectedNod)
            //    {
            //        Util.ShowMessageBox(56);
            //        return;
            //    }

            //    AddDestination(SlcSrcAndDst.SelectedItem.Nod.FldCodNod);
            //}

            //RaisePropertyChanged("TblWayIfrm_SndOut");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codNod"></param>
        private void AddDestination(int codAct, int codNod)
        {
            Model.TblWayIfrm_SndOut tbl = new Model.TblWayIfrm_SndOut()
            {
                FldWaySnd = (int)Model.Enum.ManualOrSoftware.Manual,
                FldCodCmrOutPerSnd = (int)Model.Enum.Comparers.EqualTo,
                FldTnoOutPerSnd = 1,
                FldCodUntMsrtOut = 0
            };

            // یک خروجی فقط یک بار می تواند به یک فعالیت ارسال شود
            if (!this.TblObj.TblWayIfrm_SndOut.Any(m => m.TblWayAwr_RecvInt.TblEvtSrt.TblAct.FldCodAct == codAct))
            {
                PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, tbl, this.TblObj, bpmnEty.TblActs.Single(a => a.FldCodAct == codAct));
                TblWayIfrm_SndOut.Add(tbl);
            }
            else
            {
                Util.ShowMessageBox(57);
            }
        }

        /// <summary>
        /// execute select software popoup command
        /// </summary>
        private void ExecuteSelectSfwPopupIsOpenCommand()
        {
            SlcSfw.IsSelectionModeSingle = true;
            if (this.TblWayIfrm_SndOut.Count != 0)
            {
                int codSfw = this.TblWayIfrm_SndOut.FirstOrDefault().FldCodSfw ?? 0;
                if (codSfw != 0)
                {
                    SlcSfw.SelectedItem = bpmnEty.TblSfws.SingleOrDefault(E => E.FldCodSfw == codSfw);
                }
            }

            try
            {
                this.SlcSfw.DetectAllSfws();
            }
            catch (Exception)
            {
            }

            Util.ShowPopup(SlcSfw);

            if (SlcSfw.Result == PopupResult.OK)
            {
                foreach (var item in this.TblWayIfrm_SndOut)
                {
                    item.FldCodSfw = this.SlcSfw.SelectedItem.FldCodSfw;
                }
                RaisePropertyChanged("TblWayIfrm_SndOut", "SoftwareName");
            }
            else if (this.SlcSfw.Result == PopupResult.Yes)
            {
                Util.ShowPopup(this.DefSfw);

                if (this.DefSfw.Result == PopupResult.OK)
                {
                    TblSfw sfw = new TblSfw() { FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldNamSfw = this.DefSfw.TblSfw.FldNamSfw };

                    this.bpmnEty.TblSfws.AddObject(sfw);

                    PublicMethods.SaveContext(this.bpmnEty);

                    foreach (var item in this.TblWayIfrm_SndOut)
                    {
                        item.FldCodSfw = sfw.FldCodSfw;
                    }

                    RaisePropertyChanged("SoftwareName", "ManualOrSoftwareSelectedValue");
                }
            }

            //IsSelectSfwPopupOpen = true;
        }

        /// <summary>
        /// execute select unit popoup command
        /// </summary>
        private void ExecuteSelectUntPopupIsOpenCommand()
        {
            //SlcUnt.Parent = this;
            if (this.TblWayIfrm_SndOut.Count != 0)
            {
                int codUnt = this.TblWayIfrm_SndOut.FirstOrDefault().FldCodUntMsrtOut;
                if (codUnt != 0)
                {
                    SlcUnt.SelectedItem = bpmnEty.TblUntMsrts.SingleOrDefault(E => E.FldCodUntMsrt == codUnt);
                }
            }

            Util.ShowPopup(this.SlcUnt);

            if (this.SlcUnt.Result == PopupResult.OK)
            {
                foreach (var item in this.TblWayIfrm_SndOut)
                {
                    item.FldCodUntMsrtOut = SlcUnt.SelectedItem.FldCodUntMsrt;
                }
            }
            if (this.SlcUnt.Result == PopupResult.Yes)
            {
                Util.ShowPopup(this.DefUnt);

                if (this.DefUnt.Result == PopupResult.OK)
                {
                    foreach (var item in this.TblWayIfrm_SndOut)
                    {
                        item.FldCodUntMsrtOut = DefUnt.TblUntMsrt.FldCodUntMsrt;
                    }
                    RaisePropertyChanged("NamUntMsrt");
                }
            }

            RaisePropertyChanged("TblWayIfrm_SndOut");
            RaisePropertyChanged("NamUntMsrt");
        }

        /// <summary>
        /// inserts selected object into database
        /// </summary>
        /// <param name="selected">selected object</param>
        private void InsertObject(Model.VwAllUsedOut selected)
        {
            Model.TblObj tbl = this.bpmnEty.TblObjs.SingleOrDefault(E => E.FldCodObj == selected.FldCodObj);
            //اگر خروجی انتخاب شده قبلا حذف نشده باشد
            if (tbl.TblWayIfrm_SndOut.Count() > 0)
            {
                //اگر خروجی انتخاب شده قبلا به یک فعالیت نامشخص از مجری جاری نسبت داده شده باشد
                if (tbl.TblEvtRst.TblAct.FldCodNod == this.TblObj.TblEvtRst.TblAct.TblNod.FldCodNod && tbl.TblEvtRst.TblAct.FldActUspf == true)
                {
                    List<Model.TblWayIfrm_SndOut> tbl2 = new List<Model.TblWayIfrm_SndOut>(tbl.TblWayIfrm_SndOut);
                    this.TblObj.FldNamObj = tbl.FldNamObj;
                    this.TblObj.FldTypObj = tbl.FldTypObj;
                    this.bpmnEty.Detach(tbl);
                    List<Model.TblWayIfrm_SndOut> tbl3 = new List<Model.TblWayIfrm_SndOut>(this.TblObj.TblWayIfrm_SndOut);
                    for (int i = 0; i < tbl3.Count(); i++)
                    {
                        //اگر رخدادهای آغازگر معادل با این خروجی تنها دریافت کننده همین خروجی بوده و نحوه آگاهی دیگری نداشته باشند، حذف می شوند
                        DeleteEvtSrt(tbl3);
                        this.bpmnEty.DeleteObject(tbl3[i]);
                    }
                    foreach (var item in tbl2)
                    {
                        this.TblObj.TblWayIfrm_SndOut.Add(item);
                    }
                    this.bpmnEty.Attach(tbl);
                    //در صورتی که رخداد نتیجه قبلی که مربوط به فعالیت نامشخص بود تنها دارای همین خروجی بوده است، حذف می شود
                    if (tbl.TblEvtRst.TblNews.Count == 0 && tbl.TblEvtRst.TblObjs.Count == 1 && tbl.TblEvtRst.TblSbjOrals.Count == 0)
                    {
                        this.bpmnEty.DeleteObject(tbl.TblEvtRst);
                    }
                    else
                    {
                        this.bpmnEty.DeleteObject(tbl);
                    }

                }
                //در غیر این صورت
                else
                {
                    this.TblObj.FldNamObj = selected.FldNamObj;
                    this.TblObj.FldTypObj = selected.FldTypObj;
                    List<Model.TblWayIfrm_SndOut> tbl3 = new List<Model.TblWayIfrm_SndOut>(this.TblObj.TblWayIfrm_SndOut);
                    for (int i = 0; i < tbl3.Count(); i++)
                    {
                        //اگر رخدادهای آغازگر معادل با این خروجی تنها دریافت کننده همین خروجی بوده و نحوه آگاهی دیگری نداشته باشند، حذف می شوند
                        DeleteEvtSrt(tbl3);
                        this.bpmnEty.DeleteObject(tbl3[i]);
                    }
                    foreach (var item in tbl.TblWayIfrm_SndOut)
                    {
                        Model.TblWayIfrm_SndOut tbl4 = new Model.TblWayIfrm_SndOut()
                        {
                            FldWaySnd = item.FldWaySnd,
                            FldCodSfw = item.FldCodSfw,
                            FldCodCmrOutPerSnd = item.FldCodCmrOutPerSnd,
                            FldTnoOutPerSnd = item.FldTnoOutPerSnd,
                            FldCodUntMsrtOut = item.FldCodUntMsrtOut,
                            FldDsc = item.FldDsc
                        };
                        Model.TblWayAwr_RecvInt tbl5 = new Model.TblWayAwr_RecvInt()
                        {
                            //FldCodEvtSrt = item.TblWayAwr_RecvInt.FldCodEvtSrt,
                            FldWayRecv = item.TblWayAwr_RecvInt.FldWayRecv,
                            FldCodSfw = item.TblWayAwr_RecvInt.FldCodSfw,
                            FldCodCmrIntPerRecv = item.TblWayAwr_RecvInt.FldCodCmrIntPerRecv,
                            FldTnoIntPerRecv = item.TblWayAwr_RecvInt.FldTnoIntPerRecv,
                            FldCodUntMsrtInt = item.TblWayAwr_RecvInt.FldCodUntMsrtInt,
                            FldIntNeedPrsg = item.TblWayAwr_RecvInt.FldIntNeedPrsg,
                            FldTnoIntPerPrsg = item.TblWayAwr_RecvInt.FldTnoIntPerPrsg,
                            FldCodTypPrsg = item.TblWayAwr_RecvInt.FldCodTypPrsg,
                            FldCodWayPrsg = item.TblWayAwr_RecvInt.FldCodWayPrsg
                        };
                        Model.TblEvtSrt tbl6 = new TblEvtSrt()
                        {
                            FldCodAct = item.TblWayAwr_RecvInt.TblEvtSrt.FldCodAct,
                            FldSttAct = 1,
                            FldGrpEvt = 1,
                            FldTypEvtSrt = (int)PublicMethods.DetectTypEvtSrtEqualToTypEvtRst_594(this.TblObj.TblEvtRst.FldTypEvtRst)
                        };
                        tbl4.TblWayAwr_RecvInt = tbl5;
                        tbl5.TblEvtSrt = tbl6;
                        this.TblObj.TblWayIfrm_SndOut.Add(tbl4);

                    }
                }
            }
            else
            {
                this.TblObj.FldNamObj = selected.FldNamObj;
                this.TblObj.FldTypObj = selected.FldTypObj;
                List<Model.TblWayIfrm_SndOut> tbl3 = new List<Model.TblWayIfrm_SndOut>(this.TblObj.TblWayIfrm_SndOut);
                for (int i = 0; i < tbl3.Count(); i++)
                {
                    //اگر رخدادهای آغازگر معادل با این خروجی تنها دریافت کننده همین خروجی بوده و نحوه آگاهی دیگری نداشته باشند، حذف می شوند
                    DeleteEvtSrt(tbl3);
                    this.bpmnEty.DeleteObject(tbl3[i]);
                }

            }
        }

        /// <summary>
        /// حذف رخداد های آغازگر معادل با خروجی جاری
        /// </summary>
        /// <param name="tbl"></param>
        private void DeleteEvtSrt(List<Model.TblWayIfrm_SndOut> tbl)
        {
            for (int i = 0; i < tbl.Count; i++)
            {
                if (tbl[i].TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_News.Count == 0 && tbl[i].TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_Oral.Count == 0 && tbl[i].TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_RecvInt.Count == 1)
                {
                    this.bpmnEty.DeleteObject(tbl[i].TblWayAwr_RecvInt.TblEvtSrt);
                }
            }
        }

        /// <summary>
        /// ExecuteDeleteDestinationCommand
        /// </summary>
        private void ExecuteDeleteDestinationCommand(Model.TblWayIfrm_SndOut obj)
        {
            ////اگر رخدادهای آغازگر معادل با این خروجی تنها دریافت کننده همین خروجی بوده و نحوه آگاهی دیگری نداشته باشند، حذف می شوند
            //if (obj.TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_News.Count == 0 && obj.TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_Oral.Count == 0 && obj.TblWayAwr_RecvInt.TblEvtSrt.TblWayAwr_RecvInt.Count == 1)
            //{
            //    this.bpmnEty.DeleteObject(obj.TblWayAwr_RecvInt.TblEvtSrt);
            //}

            //this.bpmnEty.TblWayIfrm_SndOut.DeleteObject(obj);

            PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(this.bpmnEty, obj.TblObj, obj, DirectionForDelete.Left);

            this.TblWayIfrm_SndOut.Remove(obj);

            //RaisePropertyChanged("TblWayIfrm_SndOut");
        }


        #endregion

        #region ' events '

        #endregion

    }
}
