using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Main
{
    public class ActDgrmViewModel : BaseViewModel
    {

        #region ' Fields '

        BPMNDBEntities context;

        ActDefViewModel parentVM;
        TblAct activity;
        Visibility actContainerVisibility = Visibility.Collapsed;
        //TblNod _nodCntForPrtSrt_Acs;
        //TblNod _nodCntForPrtRst_Acs;
        //TblNod _nodCnt_Acs;
        TblNod _nodCntForEdit_Acs;
        TblNod _nodCntForDel_Acs;


        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// create new instance of ActDgrmViewModel
        /// </summary>
        /// <param name="act"></param>
        /// <param name="context"></param>
        /// <param name="parent"></param>
        public ActDgrmViewModel(TblAct act, BPMNDBEntities context, ActDefViewModel parent)
        {
            this.context = context;
            this.parentVM = parent;
            InitialaizeCollections();

            if (act != null)
            {
                this.Activity = context.TblActs.Single(m => m.FldCodAct == act.FldCodAct);
            }
            else
            {
                this.Activity = null;
            }
            //Rebind();

            this.TblEvtSrts.CollectionChanged += TblEvtSrts_CollectionChanged;
            this.TblEvtRsts.CollectionChanged += TblEvtRsts_CollectionChanged;

            //ChangeSelectedObjExecute(activity);
            RaisePropertyChanged("Activity", "TblEvtRsts", "TblEvtSrts", "TblEvtRstsCV", "TblEvtSrtsCV");
        }


        void Rebind()
        {
            this.DeleteObjCommand = new DelegateCommand<object>(DeleteObjExecute);
            AddEvtSrtSlcAwrTypeCommand = new DelegateCommand<object/*[]*/>(AddEvtSrtSlcAwrTypeExecute, CanAddEvtSrtSlcAwrTypeExecute);
            AddEvtRstSlcAwrTypeCommand = new DelegateCommand<object/*[]*/>(AddEvtSrtRstAwrTypeExecute, CanAddEvtRstSlcAwrTypeExecute);




            //this.TblEvtRsts = new ObservableCollection<TblEvtRst>();
            //this.TblEvtSrts = new ObservableCollection<TblEvtSrt>();
            //this.TblEvtSrts.CollectionChanged += TblEvtSrts_CollectionChanged;
            //this.TblEvtRsts.CollectionChanged += TblEvtRsts_CollectionChanged;
            this.ChangeActTypeCommand = new DelegateCommand<string>(ChangeActTypeExecute, ChangeActTypeCanExecute);
            this.ChangeHasSubActTypeCommand = new DelegateCommand<string>(ChangeHasSubActTypeExecute, ChangeHasSubActTypeCanExecute);
            this.AddEvtSrtCommand = new DelegateCommand<object>(AddEvtSrtExecute);
            this.AddEvtRstCommand = new DelegateCommand<object>(AddEvtRstExecute);

            this.ChangeEvtSrtCommand = new DelegateCommand<object>(ChangeEvtSrtExecute);
            this.ChangeEvtRstCommand = new DelegateCommand<object>(ChangeEvtRstExecute);

            this.ChangeSelectedObjCommand = new DelegateCommand<object>(ChangeSelectedObjExecute);

            if (Activity == null)
            {
                return;
            }

            //context.LoadProperty(this.Activity, "TblEvtSrts");
            //context.LoadProperty(this.Activity, "TblEvtRsts");

            //RefreshEntity(context, Activity);
            //RefreshEntity(context, this.Activity.TblEvtSrts);
            //RefreshEntity(context, this.Activity.TblEvtRsts);

            //PublicMethods.ReloadEntity(context, Activity);
            //PublicMethods.ReloadEntity(context, Activity, Activity.TblEvtSrts, "TblEvtSrts");
            //PublicMethods.ReloadEntity(context, Activity, Activity.TblEvtRsts, "TblEvtRsts");

            //Pdr9003
            foreach (var EvtSrt in this.Activity.TblEvtSrts)
            {
                EvtSrt.PropertyChanged += EvtSrt.evtSrt_PropertyChanged;
                EvtSrt.GrpChanged -= EvtSrt_GrpChanged;
                EvtSrt.GrpChanged += EvtSrt_GrpChanged;
                if (EvtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr
                    && EvtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    this.TblEvtSrts.Add(EvtSrt);
                }

                EvtSrt.WayAwrs = new ObservableCollection<IWayAwr>();
                EvtSrt.WayAwrs.CollectionChanged += EvtSrt.WayAwrs_CollectionChanged;

                //PublicMethods.SaveContext(context);
                //context.LoadProperty(EvtSrt, "TblWayAwr_News");
                //RefreshEntity(context, EvtSrt.TblWayAwr_News);
                //PublicMethods.ReloadEntity(context, EvtSrt, EvtSrt.TblWayAwr_News, "TblWayAwr_News");
                foreach (var item in EvtSrt.TblWayAwr_News)
                {
                    EvtSrt.WayAwrs.Add(item);
                }

                //context.LoadProperty(EvtSrt, "TblWayAwr_Oral");
                //RefreshEntity(context, EvtSrt.TblWayAwr_Oral);
                //PublicMethods.ReloadEntity(context, EvtSrt, EvtSrt.TblWayAwr_Oral, "TblWayAwr_Oral");
                foreach (var item in EvtSrt.TblWayAwr_Oral)
                {
                    EvtSrt.WayAwrs.Add(item);
                }

                //context.LoadProperty(EvtSrt, "TblWayAwr_RecvInt");
                //RefreshEntity(context, EvtSrt.TblWayAwr_RecvInt);
                //PublicMethods.ReloadEntity(context, EvtSrt, EvtSrt.TblWayAwr_RecvInt, "TblWayAwr_RecvInt");
                foreach (var item in EvtSrt.TblWayAwr_RecvInt)
                {
                    EvtSrt.WayAwrs.Add(item);
                }
            }

            //Pdr9004
            foreach (var evtRst in this.Activity.TblEvtRsts)
            {

                evtRst.PropertyChanged -= evtRst.evtRst_PropertyChanged;
                evtRst.PropertyChanged += evtRst.evtRst_PropertyChanged;
                this.TblEvtRsts.Add(evtRst);

                evtRst.ObjRsts = new ObservableCollection<IObjRst>();
                evtRst.ObjRsts.CollectionChanged += evtRst.WayAwrs_CollectionChanged;

                //context.LoadProperty(evtRst, "TblNews");
                //RefreshEntity(context, evtRst.TblNews);
                //PublicMethods.ReloadEntity(context, evtRst, evtRst.TblNews, "TblNews");
                foreach (var item in evtRst.TblNews)
                {
                    evtRst.ObjRsts.Add(item);
                }

                //context.LoadProperty(evtRst, "TblSbjOrals");
                //RefreshEntity(context, evtRst.TblSbjOrals);
                //PublicMethods.ReloadEntity(context, evtRst, evtRst.TblSbjOrals, "TblSbjOrals");
                foreach (var item in evtRst.TblSbjOrals)
                {
                    //در نمایش نحوه‏های آگاه سازی یک رخداد نتیجه، اگر دارای دو مطلب شفاهی با یک مقصد فعالیت مشخص باشد، تنها یکی از آن‏ها نشان داده شود.
                    if (item.ActTarget != null && item.ActTarget.Any(a => !a.FldActUspf)) // اگر حداقل یک فعالیت مشخص مقصد دارد
                    {
                        if (!evtRst.ObjRsts.Any(o => o.ActTarget != null && o is TblSbjOral && o.ActTarget.Any(a => !a.FldActUspf && a.FldCodAct == item.ActTarget.First().FldCodAct)))
                        {// اگر هیچ رخداد نتیجه با این شرط که حداقل یک فعالیت مقصد مشخص داشته باشد که آن فعالیت مشخص همان اولین فعالیت مقصد رخداد نتیجه است،  ندارد 
                            evtRst.ObjRsts.Add(item);
                        }
                    }
                    else
                    {
                        evtRst.ObjRsts.Add(item);
                    }
                }

                //context.LoadProperty(evtRst, "TblObjs");
                //RefreshEntity(context, evtRst.TblObjs);
                //PublicMethods.ReloadEntity(context, evtRst, evtRst.TblObjs, "TblObjs");
                foreach (var item in evtRst.TblObjs)
                {
                    evtRst.ObjRsts.Add(item);
                }
                //evtRst.TblObjs.AssociationChanged += TblObjs_AssociationChanged;
            }
        }


        //private void RefreshEntity(BPMNDBEntities context, EntityObject ety)
        //{
        //    try
        //    {
        //        context.Refresh(System.Data.Objects.RefreshMode.StoreWins, ety);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void RefreshEntity(BPMNDBEntities context, IEnumerable<EntityObject> etys)
        //{
        //    try
        //    {
        //        context.Refresh(System.Data.Objects.RefreshMode.StoreWins, etys);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        void TblObjs_AssociationChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            TblEvtRst tbl = (e.Element as TblObj).TblEvtRst;
            tbl.ObjRsts.CollectionChanged -= tbl.WayAwrs_CollectionChanged;
            tbl.ObjRsts.Add(e.Element as TblObj);
            tbl.ObjRsts.CollectionChanged += tbl.WayAwrs_CollectionChanged;
        }

        private void InitialaizeCollections()
        {
            this.TblEvtRsts = new ObservableCollection<TblEvtRst>();
            this.TblEvtSrts = new ObservableCollection<TblEvtSrt>();
            //this.ChangeActTypeCommand = new DelegateCommand<string>(ChangeActTypeExecute, ChangeActTypeCanExecute);
            //this.ChangeHasSubActTypeCommand = new DelegateCommand<string>(ChangeHasSubActTypeExecute, ChangeHasSubActTypeCanExecute);
            //this.DeleteObjCommand = new DelegateCommand<object>(DeleteObjExecute);
            //AddEvtSrtSlcAwrTypeCommand = new DelegateCommand<object/*[]*/>(AddEvtSrtSlcAwrTypeExecute, CanAddEvtSrtSlcAwrTypeExecute);
            //AddEvtRstSlcAwrTypeCommand = new DelegateCommand<object/*[]*/>(AddEvtSrtRstAwrTypeExecute, CanAddEvtRstSlcAwrTypeExecute);
        }

        #endregion

        #region ' Properties / Commands '

        #region Access

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddAct
        {
            get
            {
                if (this.Activity != null)// && (_nodCnt_Acs == null || this.Activity.FldCodNod != _nodCnt_Acs.FldCodNod))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088("Add", "Act", AllTypEty.Act, this.Activity.TblNod);

                    //this._nodCnt_Acs = this.Activity.TblNod;
                }

                return PublicMethods.CurrentUser.AcsUsr["AddAct"];

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewPrtSrtAct
        {
            get
            {
                if (this.Activity != null)// && (_nodCntForPrtSrt_Acs == null || this.Activity.FldCodNod != _nodCntForPrtSrt_Acs.FldCodNod))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "View", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtSrtAct");

                    //this._nodCntForPrtSrt_Acs = this.Activity.TblNod;
                }

                return PublicMethods.CurrentUser.AcsUsr["ViewPrtSrtAct"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewPrtRstAct
        {
            get
            {
                if (this.Activity != null)// && (_nodCntForPrtRst_Acs == null || this.Activity.FldCodNod != _nodCntForPrtRst_Acs.FldCodNod))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "View", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtRstAct");

                    //this._nodCntForPrtRst_Acs = this.Activity.TblNod;
                }

                return PublicMethods.CurrentUser.AcsUsr["ViewPrtRstAct"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditAtbtAct
        {
            get
            {

                if (this.Activity != null && (_nodCntForEdit_Acs == null || this.Activity.FldCodNod != _nodCntForEdit_Acs.FldCodNod))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Edit", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "AtbtAct");

                    this._nodCntForEdit_Acs = this.Activity.TblNod;
                }

                return PublicMethods.CurrentUser.AcsUsr["EditAtbtAct"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelAct
        {
            get
            {
                if (this.Activity != null && (_nodCntForDel_Acs == null || this.Activity.FldCodNod != _nodCntForDel_Acs.FldCodNod))
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Del", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, typEtyCnt: AllTypEty.Act);

                    this._nodCntForDel_Acs = this.Activity.TblNod;
                }

                return PublicMethods.CurrentUser.AcsUsr["DelAct"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_AddPrtSrtAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Add", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtSrtAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["AddPrtSrtAct"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EditPrtSrtAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Edit", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtSrtAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["EditPrtSrtAct"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_DelPrtSrtAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Del", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtSrtAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["DelPrtSrtAct"];
            }
        }


        public bool Acs_AddPrtRstAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Add", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtRstAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["AddPrtRstAct"];
            }
        }

        public bool Acs_EditPrtRstAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Edit", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtRstAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["EditPrtRstAct"];
            }
        }


        public bool Acs_DelPrtRstAct
        {
            get
            {
                if (this.Activity != null)
                {
                    PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(this.Activity, "Del", TypRlnEtyMjrWthEtyMom.PerformerOfActivity, namTypEtyMjr: "PrtRstAct");
                }

                return PublicMethods.CurrentUser.AcsUsr["DelPrtRstAct"];
            }
        }


        #endregion

        public BPMNDBEntities Context
        {
            get { return context; }
            set { context = value; }
        }

        public bool ShouldAnyCdnEvtRstVisible
        {
            get
            {
                bool b = !this.TblEvtRsts.Any(m => (EvtRstType)m.FldTypEvtRst == EvtRstType.anyCdnEvtRst);
                return b;
            }
        }

        public bool ShouldCancelEvtRstVisible
        {
            get
            {
                bool b = !this.TblEvtRsts.Any(m => (EvtRstType)m.FldTypEvtRst == EvtRstType.cancelEvtRst);
                return b;
            }
        }

        public bool ShouldErrEvtRstVisible
        {
            get
            {
                bool b = !this.TblEvtRsts.Any(m => (EvtRstType)m.FldTypEvtRst == EvtRstType.errAccurEvtRst);
                return b;
            }
        }


        public ListCollectionView TblEvtRstsCV
        {
            get
            {
                ListCollectionView cv = (ListCollectionView)CollectionViewSource.GetDefaultView(TblEvtRsts);

                PropertyGroupDescription groupDescription
                        = new PropertyGroupDescription("FldGrpEvt");
                cv.GroupDescriptions.Clear();
                cv.GroupDescriptions.Add(groupDescription);
                return cv;
            }
        }

        public ListCollectionView TblEvtSrtsCV
        {
            get
            {
                ListCollectionView cv = (ListCollectionView)CollectionViewSource.GetDefaultView(TblEvtSrts);

                PropertyGroupDescription groupDescription
                        = new PropertyGroupDescription("FldGrpEvt");
                cv.GroupDescriptions.Clear();
                cv.GroupDescriptions.Add(groupDescription);
                return cv;
            }
        }

        public TblAct Activity
        {
            get
            {
                return activity;
            }
            set
            {
                int i = activity != null ? activity.FldCodNod : -1;

                int j = value != null ? value.FldCodNod : -1;


                if (activity != value)
                {
                    activity = value;
                    ChangeSelectedObjExecute(activity);
                    Rebind();
                    RaisePropertyChanged("Activity", "TblEvtRsts", "TblEvtSrts", "TblEvtRstsCV", "TblEvtSrtsCV");
                }

                if (i == -1 || j == -1 || i != j)
                {
                    RaisePropertyChanged("Acs_DelAct");

                    RaisePropertyChanged("Acs_AddAct", "Acs_ViewPrtSrtAct", "Acs_ViewPrtRstAct");
                }


                //if (AddNewActCommand != null)
                //{
                //    (AddNewActCommand as DelegateCommand).RaiseCanExecuteChanged();
                //}
            }
        }

        public ObservableCollection<TblEvtRst> TblEvtRsts { get; set; }

        public ObservableCollection<TblEvtSrt> TblEvtSrts { get; set; }

        public ICommand ChangeActTypeCommand { get; set; }

        public ICommand ChangeHasSubActTypeCommand { get; set; }

        public ICommand AddEvtSrtCommand { get; set; }

        public ICommand AddEvtRstCommand { get; set; }

        public ICommand ChangeEvtSrtCommand { get; set; }

        public ICommand ChangeEvtRstCommand { get; set; }

        public ICommand DeleteObjCommand { get; set; }

        public ICommand AddEvtSrtSlcAwrTypeCommand { get; set; }

        public ICommand AddEvtRstSlcAwrTypeCommand { get; set; }

        public ICommand ChangeSelectedObjCommand { get; set; }

        public Visibility ActContainerVisibility
        {
            get
            {
                return actContainerVisibility;
            }
            set
            {
                actContainerVisibility = value;
                RaisePropertyChanged("ActContainerVisibility");
            }
        }
        #endregion

        #region ' Public Methods '

        public void ActDrop(string data)
        {
            //if (parentVM.OrgPosVM.SelectedNode == null)
            //{
            //    return;
            //}
            if (data == "act")
            {
                parentVM.AddNewAct();
            }
        }

        public void EvtSrtDrop(string data, int group)
        {
            var srtTyp = (EvtSrtType)Enum.Parse(typeof(EvtSrtType), data);
            int groupCode = 0;
            TblEvtSrt evtSrt = null;
            switch (srtTyp)
            {
                case EvtSrtType.inSgmtTime:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.inSgmtTime, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.aftrCdnEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.aftrCdnEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.aftrAwareEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.aftrAwareEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.aftrAnyCdnEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.aftrAnyCdnEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.spcCdnEvtSrtAftr:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.spcCdnEvtSrtAftr, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.errAccurEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.errAccurEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.cancelEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.cancelEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.spcCdnEvtSrt:
                    groupCode = group == -1 ? TblAct.GetNewEvtSrtGroupID(this.Activity) : group;
                    evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)EvtSrtType.spcCdnEvtSrt, FldGrpEvt = groupCode };
                    AddEvtSrtExecute(evtSrt);

                    break;
                case EvtSrtType.getNewAwrAftr:
                    break;
                case EvtSrtType.getNewAwrInnTim:
                    break;
                default:
                    break;
            }
        }

        public void EvtRstDrop(string data)
        {
            var rstTyp = (EvtRstType)Enum.Parse(typeof(EvtRstType), data);
            TblEvtRst evtRst = null;
            switch (rstTyp)
            {
                case EvtRstType.anyCdnEvtRst:
                    if (this.Activity.TblEvtRsts.SingleOrDefault(m => m.FldTypEvtRst == (int)EvtRstType.anyCdnEvtRst) != null)
                    {
                        Util.ShowMessageBox(59, "وقوع هر شرایطی هنگام فعالیت");
                        return;
                    }
                    evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)EvtRstType.anyCdnEvtRst };
                    AddEvtRstExecute(evtRst, EvtRstType.anyCdnEvtRst);

                    break;
                case EvtRstType.spcCdnEvtRstAftr:
                    evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)EvtRstType.spcCdnEvtRstAftr };
                    AddEvtRstExecute(evtRst, EvtRstType.spcCdnEvtRstAftr);

                    break;
                case EvtRstType.getNewAwrAftrAct:
                    break;
                case EvtRstType.errAccurEvtRst:
                    if (this.Activity.TblEvtRsts.Any(m => m.FldTypEvtRst == (int)EvtRstType.errAccurEvtRst))
                    {
                        Util.ShowMessageBox(59, "وقوع خطا هنگام فعالیت");
                        return;
                    }
                    evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)EvtRstType.errAccurEvtRst };
                    AddEvtRstExecute(evtRst, EvtRstType.errAccurEvtRst);

                    break;
                case EvtRstType.cancelEvtRst:

                    if (this.Activity.TblEvtRsts.SingleOrDefault(m => m.FldTypEvtRst == (int)EvtRstType.cancelEvtRst) != null)
                    {
                        Util.ShowMessageBox(59, "لغو فعالیت");
                        return;
                    }
                    evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)EvtRstType.cancelEvtRst };
                    AddEvtRstExecute(evtRst, EvtRstType.cancelEvtRst);

                    break;
                case EvtRstType.spcCdnEvtRstInnTim:
                    evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)EvtRstType.spcCdnEvtRstInnTim };
                    AddEvtRstExecute(evtRst, EvtRstType.spcCdnEvtRstInnTim);

                    break;
                case EvtRstType.getNewAwr:
                    break;
                default:
                    break;
            }
        }

        public void SaveChanges()
        {
            PublicMethods.SaveContext(context);
            Rebind();
            //ChangeSelectedObjExecute(Activity);
        }

        public void ChangeGroup(TblEvtSrt source, TblEvtSrt dest)
        {

            #region شناسایی فعالیت مبدا رخداد آغازگر دراپ شده

            TblAct actSrc = null;

            foreach (var oral in source.TblWayAwr_Oral)
            {
                if (actSrc != null)
                {
                    break;
                }

                actSrc = oral.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
            }

            foreach (var rcv in source.TblWayAwr_RecvInt)
            {
                if (actSrc != null)
                {
                    break;
                }

                actSrc = rcv.TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
            }

            #endregion

            if (actSrc != null && !actSrc.FldActUspf)
            {

                #region شناسایی تمام فعالیت های مشخص مبدا گروه رخدادهای آغازگر مقصد

                CollectionViewGroup grpItems = (CollectionViewGroup)TblEvtSrtsCV.Groups.Single(g => int.Parse((g as CollectionViewGroup).Name.ToString()) == dest.FldGrpEvt);

                List<TblAct> acts = new List<TblAct>();

                foreach (TblEvtSrt srt in grpItems.Items)
                {
                    foreach (var oral in srt.TblWayAwr_Oral)
                    {
                        try
                        {
                            TblAct act = oral.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
                            if (!act.FldActUspf)
                            {
                                acts.Add(act);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    foreach (var rcv in srt.TblWayAwr_RecvInt)
                    {
                        try
                        {
                            TblAct act = rcv.TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
                            if (!act.FldActUspf)
                            {
                                acts.Add(act);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                #endregion


                if (acts.FirstOrDefault(m => m.FldCodAct == actSrc.FldCodAct) != null)
                {
                    Util.ShowMessageBox(23);
                    return;
                }
            }

            source.FldGrpEvt = dest.FldGrpEvt;
            RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");

        }

        public void DeleteAct()
        {
            if (Activity == null)
            {
                return;
            }
            if (Util.ShowMessageBox(2, "فعالیت") == System.Windows.MessageBoxResult.Yes)
            {
                ActDeleteOK();
            }
            else
            {
                ActDeleteCancel();
            }
        }

        public void ActDeleteCancel()
        {

        }

        public void ActDeleteOK()
        {
            //PublicMethods.DeleteAct_806(context, this.Activity);
            PublicMethods.DeleteActAndChgPrs_3173(context, this.Activity);

            PublicMethods.SaveContext(context);

            parentVM.UnsubscribeOrgPosVM_PropertyChanged();

            //parentVM.OrgPosVM.ActList.Remove(Activity);

            parentVM.ActLstVM.RemoveActFromList(this.Activity);

            parentVM.SubscribeOrgPosVM_PropertyChanged();

            TblAct nextAct = parentVM.GetAnotherActivityForDisplay(Activity);

            context.Dispose();
            context = new BPMNDBEntities();

            if (nextAct != null)
            {
                Activity = context.TblActs.Single(m => m.FldCodAct == nextAct.FldCodAct);
            }

            Rebind();

            ChangeSelectedObjExecute(Activity); //RaisePropertyChanged("TblAct");
        }

        #endregion

        #region ' Private Methods '

        ///// <summary>
        ///// افزودن یک فعالیت جدید
        ///// </summary>
        //public void AddNewAct()
        //{
        //    if (this.Activity != null)
        //    {
        //        parentVM.AddNewAct();
        //    }
        //}

        /// <summary>
        ///Pdr9002
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeSelectedObjExecute(object obj)
        {
            if (obj == null)
            {
                parentVM.SelectedObj = new EmptyViewModel();
                return;
            }
            if (parentVM == null)
            {
                return;
            }

            Type t = obj.GetType();
            switch (t.Name)
            {
                case "TblAct":
                    parentVM.SelectedObj = new DtlActViewModel(this.context, this.Activity);
                    parentVM.SelectedObj.IsEnabled = true;

                    (parentVM.SelectedObj as DtlActViewModel).TabsEnabled = Acs_EditAtbtAct;

                    break;
                case "TblEvtSrt":
                    TblEvtSrt evtsrt = obj as TblEvtSrt;
                    evtsrt.IsSelected = true;
                    if (evtsrt.FldTypEvtSrt == (int)EvtSrtType.inSgmtTime)
                    {
                        parentVM.SelectedObj = new EvtSrtInnSgmtTimViewModel(this.context, evtsrt.TblEvtSrt_InnSgmtTim);
                    }
                    else if (evtsrt.FldTypEvtSrt == (int)EvtSrtType.aftrCdnEvtSrt)
                    {
                        parentVM.SelectedObj = new EvtCdn(context, evtsrt);
                    }
                    else if (evtsrt.FldTypEvtSrt == (int)EvtSrtType.aftrAnyCdnEvtSrt ||
                        evtsrt.FldTypEvtSrt == (int)EvtSrtType.spcCdnEvtSrtAftr)
                    {
                        parentVM.SelectedObj = new DtlEvtSrtViewModel(context, evtsrt);
                    }
                    else if (evtsrt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt)
                    {
                        parentVM.SelectedObj = new EmptyViewModel();
                    }
                    else
                    {
                        parentVM.SelectedObj = new DtlEvtSrtViewModel(context, evtsrt);
                    }

                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtSrtAct;
                    break;
                case "TblWayAwr_News":
                    parentVM.SelectedObj = new DtlRecvNewsViewModel(context, FindEvtSrtByChild(obj), obj as TblWayAwr_News, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtSrtAct;
                    break;
                case "TblWayAwr_Oral":
                    parentVM.SelectedObj = new DtlAwrOralViewModel(context, FindEvtSrtByChild(obj), obj as TblWayAwr_Oral, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtSrtAct;
                    break;
                case "TblWayAwr_RecvInt":
                    parentVM.SelectedObj = new DtlIntViewModel(context, FindEvtSrtByChild(obj), obj as TblWayAwr_RecvInt, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtSrtAct;
                    break;
                case "TblEvtRst":
                    TblEvtRst evtRst = obj as TblEvtRst;
                    evtRst.IsSelected = true;
                    switch ((EvtRstType)evtRst.FldTypEvtRst)
                    {
                        case EvtRstType.anyCdnEvtRst:
                            parentVM.SelectedObj = new EmptyViewModel();
                            break;
                        case EvtRstType.spcCdnEvtRstAftr:
                            parentVM.SelectedObj = new EvtCdn(context, evtRst);
                            break;
                        case EvtRstType.getNewAwrAftrAct:
                            parentVM.SelectedObj = new EvtRstGainAwrNewInnTimImpViewModel(context, evtRst);
                            break;
                        case EvtRstType.errAccurEvtRst:
                            parentVM.SelectedObj = new EvtRstErorViewModel(context, evtRst);
                            break;
                        case EvtRstType.cancelEvtRst:
                            parentVM.SelectedObj = new EmptyViewModel();
                            break;
                        case EvtRstType.spcCdnEvtRstInnTim:
                            parentVM.SelectedObj = new EvtCdn(context, evtRst);
                            break;
                        case EvtRstType.getNewAwr:
                            parentVM.SelectedObj = new EvtRstGainAwrNewInnTimImpViewModel(context, evtRst);

                            break;
                        default:
                            break;
                    }

                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtRstAct;
                    break;
                case "TblNew":
                    parentVM.SelectedObj = new DtlSndNewsViewModel(context, obj as TblNew, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtRstAct;
                    break;
                case "TblSbjOral":
                    parentVM.SelectedObj = new DtlIfrmOralViewModel(context, obj as TblSbjOral, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtRstAct;
                    break;
                case "TblObj":
                    parentVM.SelectedObj = new DtlOutViewModel(context, obj as TblObj, parentVM.OrgPosVM.NodSlcEed.FldCodNod);
                    parentVM.SelectedObj.IsEnabled = Acs_EditPrtRstAct;
                    break;

                default:
                    break;
            }

            UnselectAllButThis(obj);
        }

        void TblEvtRsts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        //if ((item as TblEvtRst).FldTypEvtRst == (int)EvtRstType.getNewAwrAftrAct
                        //    || (item as TblEvtRst).FldTypEvtRst == (int)EvtRstType.getNewAwr)
                        //{
                        //    return;
                        //}
                        this.Activity.TblEvtRsts.Add(item as TblEvtRst);
                        (item as TblEvtRst).IsSelected = true;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (((EntityObject)item).EntityState != EntityState.Detached)
                        {
                            this.context.DeleteObject(item);
                        }
                        //this.Activity.TblEvtRsts.Remove(item as TblEvtRst);
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        void TblEvtSrts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        this.Activity.TblEvtSrts.Add(item as TblEvtSrt);
                        (item as TblEvtSrt).IsSelected = true;
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (((EntityObject)item).EntityState != EntityState.Detached)
                        {
                            this.context.DeleteObject(item);
                        }

                        //this.Activity.TblEvtSrts.Remove(item as TblEvtSrt);
                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void AddEvtSrtSlcAwrTypeExecute(object obj)
        {

            if (!Acs_AddPrtSrtAct || !Acs_EditPrtSrtAct)
            {
                Util.ShowMessageBox(51);
                return;
            }

            dynamic d = obj;
            IWayAwr newobj = null;
            TblEvtSrt selectedItem = d.DataContext as TblEvtSrt;
            TblItmFixSfw selectedWayAwr = d.Child.Selected as TblItmFixSfw;
            TypWayAwr t = (TypWayAwr)selectedWayAwr.FldCodItm;
            switch (t)
            {
                case TypWayAwr.Obj:
                    newobj = new TblWayAwr_RecvInt() { FldCodCmrIntPerRecv = 1, FldIntNeedPrsg = 1 };
                    selectedItem.WayAwrs.Add(newobj);
                    break;
                case TypWayAwr.SbjOral:
                    if (selectedItem.TblWayAwr_Oral.Count == 0 && selectedItem.WayAwrs.FirstOrDefault(m => m.GetType() == typeof(TblWayAwr_Oral)) == null)
                    {
                        newobj = new TblWayAwr_Oral() { FldTypAwr = 1 };
                        selectedItem.WayAwrs.Add(newobj);
                    }
                    break;
                case TypWayAwr.News:
                    newobj = new TblWayAwr_News();
                    selectedItem.WayAwrs.Add(newobj);
                    break;
                default:
                    break;
            }
            ChangeSelectedObjExecute(newobj);

            RaisePropertyChanged("TblEvtSrts", "WayAwrs");
        }

        private bool CanAddEvtSrtSlcAwrTypeExecute(object arg)
        {
            return true;
        }

        private bool CanAddEvtRstSlcAwrTypeExecute(object arg)
        {
            return true;
        }

        private void AddEvtSrtRstAwrTypeExecute(object obj)
        {
            if (!Acs_AddPrtRstAct || !Acs_EditPrtRstAct)
            {
                Util.ShowMessageBox(52);
                return;
            }

            dynamic d = obj;
            TblEvtRst selectedItem = d.DataContext as TblEvtRst;
            TblItmFixSfw selectedWayAwr = d.Child.Selected as TblItmFixSfw;
            TypWayIfrm t = (TypWayIfrm)selectedWayAwr.FldCodItm;
            IObjRst newobj = null;
            switch (t)
            {
                case TypWayIfrm.Obj:
                    newobj = new TblObj() { FldNamObj = "خروجی" };
                    selectedItem.ObjRsts.Add(newobj);
                    break;
                case TypWayIfrm.SbjOral:
                    newobj = new TblSbjOral();
                    selectedItem.ObjRsts.Add(newobj);
                    break;
                case TypWayIfrm.News:
                    newobj = new TblNew() { FldTtlNews = "خبر" };
                    selectedItem.ObjRsts.Add(newobj);
                    break;
                default:
                    break;
            }

            ChangeSelectedObjExecute(newobj);

            RaisePropertyChanged("TblEvtRsts", "WayAwrs");

        }

        private bool ChangeActTypeCanExecute(string arg)
        {
            return true;
        }

        private void ChangeActTypeExecute(string obj)
        {
            this.Activity.FldTypAct = int.Parse(obj);
        }

        private bool ChangeHasSubActTypeCanExecute(string arg)
        {
            return true;
        }

        private void ChangeHasSubActTypeExecute(string obj)
        {
            this.Activity.FldActSubHav = int.Parse(obj);
        }

        private void AddEvtSrtExecute(object obj)
        {
            dynamic d = obj;
            EvtSrtType type = (EvtSrtType)(d.SelectedType as TblItmFixSfw).FldCodItm;
            TblEvtSrt evtSrt = new TblEvtSrt() { FldSttAct = 0, FldTypEvtSrt = (int)type, FldGrpEvt = TblAct.GetNewEvtSrtGroupID(this.Activity) };
            AddEvtSrtExecute(evtSrt);
        }

        /// <summary>
        /// Pdr9008
        /// </summary>
        /// <param name="evtSrt"></param>
        private void AddEvtSrtExecute(TblEvtSrt evtSrt)
        {
            if (!Acs_AddPrtSrtAct)
            {
                Util.ShowMessageBox(51);
                return;
            }

            evtSrt.WayAwrs = new ObservableCollection<IWayAwr>();
            evtSrt.WayAwrs.CollectionChanged += evtSrt.WayAwrs_CollectionChanged;
            evtSrt.PropertyChanged += evtSrt.evtSrt_PropertyChanged;
            evtSrt.TblEvtSrt_InnSgmtTim = new TblEvtSrt_InnSgmtTim() { FldTypSgmtTim = 1 };
            evtSrt.GrpChanged += EvtSrt_GrpChanged;
            TblEvtSrts.Add(evtSrt);
            ChangeSelectedObjExecute(evtSrt);
            RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");
        }

        /// <summary>
        /// Pdr9007
        /// </summary>
        /// <param name="obj"></param>
        private void AddEvtRstExecute(object obj)
        {
            dynamic d = obj;
            EvtRstType type = (EvtRstType)(d.SelectedType as TblItmFixSfw).FldCodItm;
            TblEvtRst evtRst = new TblEvtRst() { FldSttAct = 0, FldTypEvtRst = (int)type };
            AddEvtRstExecute(evtRst, type);
        }


        private void AddEvtRstExecute(TblEvtRst evtRst, EvtRstType type)
        {
            if (!Acs_AddPrtRstAct)
            {
                Util.ShowMessageBox(52);
                return;
            }

            evtRst.ObjRsts = new ObservableCollection<IObjRst>();
            evtRst.ObjRsts.CollectionChanged += evtRst.WayAwrs_CollectionChanged;
            evtRst.PropertyChanged += evtRst.evtRst_PropertyChanged;

            TblEvtSrt newobj = null;
            if (type == EvtRstType.getNewAwr)
            {
                newobj = new TblEvtSrt() { FldTypEvtSrt = (int)EvtSrtType.getNewAwrInnTim, TblAct = this.Activity };
                TblEvt_GainAwrNew gain = new TblEvt_GainAwrNew() { TblEvtRst = evtRst, TblEvtSrt = newobj };

                context.AddToTblEvt_GainAwrNew(gain);
                this.Activity.TblEvtSrts.Add(newobj);
            }
            if (type == EvtRstType.getNewAwrAftrAct)
            {
                newobj = new TblEvtSrt() { FldTypEvtSrt = (int)EvtSrtType.getNewAwrAftr, TblAct = this.Activity };
                TblEvt_GainAwrNew gain = new TblEvt_GainAwrNew() { TblEvtRst = evtRst, TblEvtSrt = newobj };

                context.AddToTblEvt_GainAwrNew(gain);
                this.Activity.TblEvtSrts.Add(newobj);

            }

            TblEvtRsts.Add(evtRst);
            ChangeSelectedObjExecute(evtRst);
            RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV", "ShouldAnyCdnEvtRstVisible", "ShouldCancelEvtRstVisible", "ShouldErrEvtRstVisible");
        }

        /// <summary>
        /// Pdr9011
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeEvtSrtExecute(object obj)
        {
            dynamic d = obj;
            EvtSrtType type = (EvtSrtType)(d.SelectedType as TblItmFixSfw).FldCodItm;
            TblEvtSrt evtSrt = d.Parent.Parent.DataContext as TblEvtSrt;
            evtSrt.FldTypEvtSrt = (int)type;
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.inSgmtTime)
            {
                if (evtSrt.TblEvtSrt_InnSgmtTim == null)
                {
                    evtSrt.TblEvtSrt_InnSgmtTim = new TblEvtSrt_InnSgmtTim() { FldTypSgmtTim = 1 };
                }
            }
            else
            {
                if (evtSrt.TblEvtSrt_InnSgmtTim != null)
                {
                    context.DeleteObject(evtSrt.TblEvtSrt_InnSgmtTim);
                }
            }

            ChangeSelectedObjExecute(evtSrt);

            RaisePropertyChanged("Activity", "TblEvtSrts", "IsAddWayAwrEnabled");
        }


        private void ChangeEvtRstExecute(object obj)
        {
            dynamic d = obj;
            EvtRstType type = (EvtRstType)(d.SelectedType as TblItmFixSfw).FldCodItm;
            TblEvtRst evtRst = d.Parent.Parent.DataContext as TblEvtRst;
            //evtRst.FldTypEvtRst = (int)type;
            ChangeEvtRstType(evtRst, type);
            RaisePropertyChanged("Activity", "TblEvtRsts", "ShouldAnyCdnEvtRstVisible", "ShouldCancelEvtRstVisible", "ShouldErrEvtRstVisible");
        }

        /// <summary>
        /// Pdr9017
        /// </summary>
        /// <param name="evtRst"></param>
        /// <param name="newType"></param>
        private void ChangeEvtRstType(TblEvtRst evtRst, EvtRstType newType)
        {
            //evtRst.TblCdns ==>>  TblEvtRst>AftrAct>Cdn
            //evtRst.TblCdns1 ==>>  TblEvtRst>InnTimImp>Cdn

            if (newType == EvtRstType.getNewAwr &&
                (EvtRstType)evtRst.FldTypEvtRst != EvtRstType.getNewAwr && (EvtRstType)evtRst.FldTypEvtRst != EvtRstType.getNewAwrAftrAct)
            {
                TblEvt_GainAwrNew gain = new TblEvt_GainAwrNew();
                gain.TblEvtSrt = new TblEvtSrt() { FldTypEvtSrt = (int)EvtSrtType.getNewAwrInnTim, FldSttAct = 1, FldGrpEvt = TblAct.GetNewEvtSrtGroupID(this.Activity), FldCodAct = this.Activity.FldCodAct };
                evtRst.TblEvt_GainAwrNew.Add(gain);
            }
            if (newType == EvtRstType.getNewAwrAftrAct &&
                (EvtRstType)evtRst.FldTypEvtRst != EvtRstType.getNewAwr && (EvtRstType)evtRst.FldTypEvtRst != EvtRstType.getNewAwrAftrAct)
            {
                TblEvt_GainAwrNew gain = new TblEvt_GainAwrNew();
                gain.TblEvtSrt = new TblEvtSrt() { FldTypEvtSrt = (int)EvtSrtType.getNewAwrAftr, FldSttAct = 1, FldGrpEvt = TblAct.GetNewEvtSrtGroupID(this.Activity), FldCodAct = this.Activity.FldCodAct };
                evtRst.TblEvt_GainAwrNew.Add(gain);
            }

            switch ((EvtRstType)evtRst.FldTypEvtRst)
            {
                case EvtRstType.anyCdnEvtRst:
                    break;
                case EvtRstType.spcCdnEvtRstAftr:
                    if (newType == EvtRstType.spcCdnEvtRstInnTim)//پس از فعالیت- وقوع شرایط خاص -> هنگام فعالیت- وقوع شرایط خاص
                    {//نوع رخداد تغییر کرده و شرایط ثبت شده برای نوع قبلی به این نوع جدید منتقل می شوند.
                        foreach (var item in evtRst.TblCdns)
                        {
                            evtRst.TblCdns1.Add(item);
                        }

                        evtRst.TblCdns.Clear();
                    }
                    else //پس از فعالیت- وقوع شرایط خاص -> هر نوعی به جز هنگام فعالیت- وقوع شرایط خاص
                    {
                        evtRst.TblCdns.Clear();//وع رخداد تغییر کرده و شرایط ثبت شده برای حالت قبلی حذف می شوند
                    }
                    break;
                case EvtRstType.getNewAwrAftrAct:
                    if (newType == EvtRstType.getNewAwr)//پس از فعالیت- کسب آگاهی جدید -> هنگام فعالیت- کسب آگاهی جدید
                    {
                        //فقط رخداد فعالیت تغییر می کند
                    }
                    else//پس از فعالیت- کسب آگاهی جدید -> هر نوعی به جز هنگام فعالیت- کسب آگاهی جدید
                    {
                        //نوع رخداد تغییر کرده و نحوه های آگاهی ثبت شده برای رخداد آغازگر همزاد رخداد نتیجه جاری حذف می شوند
                        TblEvtSrt srt = evtRst.TblEvt_GainAwrNew.First().TblEvtSrt;

                        List<TblWayAwr_RecvInt> TblWayAwr_RecvInts = srt.TblWayAwr_RecvInt.ToList();

                        for (int i = 0; i < TblWayAwr_RecvInts.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_RecvInt(context, TblWayAwr_RecvInts[i]);
                        }

                        List<TblWayAwr_Oral> TblWayAwr_Orals = srt.TblWayAwr_Oral.ToList();

                        for (int i = 0; i < TblWayAwr_Orals.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_Oral(context, TblWayAwr_Orals[i]);
                        }

                        List<TblWayAwr_News> TblWayAwr_News = srt.TblWayAwr_News.ToList();
                        for (int i = 0; i < TblWayAwr_News.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_News(context, TblWayAwr_News[i]);
                        }
                    }

                    break;
                case EvtRstType.errAccurEvtRst: //هنگام فعالیت- وقوع خطا -> هر نوعی
                    evtRst.TblErors.Clear();// نوع رخداد تغییر کرده و خطاهای ثبت شده حذف می شوند
                    break;
                case EvtRstType.cancelEvtRst:
                    break;
                case EvtRstType.spcCdnEvtRstInnTim:
                    if (newType == EvtRstType.spcCdnEvtRstAftr) //هنگام فعالیت- وقوع شرایط خاص -> پس از فعالیت- وقوع شرایط خاص
                    {
                        // نوع رخداد تغییر کرده و شرایط ثبت شده برای نوع قبلی به این نوع جدید منتقل می شوند.
                        foreach (var item in evtRst.TblCdns1)
                        {
                            evtRst.TblCdns.Add(item);
                        }

                        evtRst.TblCdns1.Clear();

                    }
                    else // هنگام فعالیت- وقوع شرایط خاص -> هر نوعی به جز پس از فعالیت- وقوع شرایط خاص
                    {
                        //نوع رخداد تغییر کرده و شرایط ثبت شده برای حالت قبلی حذف می شوند

                        evtRst.TblCdns1.Clear();
                    }
                    break;
                case EvtRstType.getNewAwr:
                    if (newType == EvtRstType.getNewAwrAftrAct) //هنگام فعالیت- کسب آگاهی جدید -> پس از فعالیت- کسب آگاهی جدید
                    {
                        //فقط رخداد فعالیت تغییر می کند
                    }
                    else //هنگام فعالیت- کسب آگاهی جدید -> هر نوعی به جز پس از فعالیت- کسب آگاهی جدید
                    {
                        //نوع رخداد تغییر کرده و نحوه های آگاهی ثبت شده برای رخداد آغازگر همزاد رخداد نتیجه جاری حذف می شوند
                        TblEvtSrt srt = evtRst.TblEvt_GainAwrNew.First().TblEvtSrt;

                        List<TblWayAwr_RecvInt> TblWayAwr_RecvInts = srt.TblWayAwr_RecvInt.ToList();

                        for (int i = 0; i < TblWayAwr_RecvInts.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_RecvInt(context, TblWayAwr_RecvInts[i]);
                        }

                        List<TblWayAwr_Oral> TblWayAwr_Orals = srt.TblWayAwr_Oral.ToList();

                        for (int i = 0; i < TblWayAwr_Orals.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_Oral(context, TblWayAwr_Orals[i]);
                        }

                        List<TblWayAwr_News> TblWayAwr_News = srt.TblWayAwr_News.ToList();
                        for (int i = 0; i < TblWayAwr_News.Count; i++)
                        {
                            PublicMethods.DeleteTblWayAwr_News(context, TblWayAwr_News[i]);
                        }
                    }
                    break;
                default:
                    break;
            }

            evtRst.FldTypEvtRst = (int)newType;
            ChangeSelectedObjExecute(evtRst);
        }


        private void DeleteObjExecute(object obj)
        {
            if (obj.GetType() == typeof(TblEvtSrt))
            {
                if (!Acs_DelPrtSrtAct)
                {
                    Util.ShowMessageBox(53);
                    return;
                }

                if (Util.ShowMessageBox(35) == MessageBoxResult.Yes)
                {
                    TblEvtSrt deletingObj = obj as TblEvtSrt;
                    //DeleteEvtSrt(deletingObj);

                    PublicMethods.DeleteEvtSrtAndChgPrs_3205(context, deletingObj);

                    if (deletingObj.FldTypEvtSrt == (int)EvtSrtType.getNewAwrAftr || deletingObj.FldTypEvtSrt == (int)EvtSrtType.getNewAwrInnTim)
                    {
                        //context.DeleteObject(deletingObj);
                    }
                    else
                    {
                        TblEvtSrts.Remove(deletingObj);
                    }

                    ChangeSelectedObjExecute(Activity);
                    RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");
                }
            }
            else if (obj.GetType() == typeof(TblWayAwr_News))
            {
                if (!Acs_DelPrtSrtAct)
                {
                    Util.ShowMessageBox(53);
                    return;
                }

                if (Util.ShowMessageBox(36) == MessageBoxResult.Yes)
                {
                    TblWayAwr_News deletingObj = obj as TblWayAwr_News;
                    //PublicMethods.DeleteTblWayAwr_News(context, deletingObj);

                    if ((obj as TblWayAwr_News).TblEvtSrt != null)
                    {
                        PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, (obj as TblWayAwr_News).TblEvtSrt, obj as TblWayAwr_News);
                        PublicMethods.SaveContext(context);
                    }

                    ChangeSelectedObjExecute(GetEvtSrtWayAwrParent(deletingObj));
                    RemoveEvtSrtWayAwrFromItsParent(obj);
                    //PublicMethods.SaveContext(context);
                    RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");
                }
            }

            else if (obj.GetType() == typeof(TblWayAwr_RecvInt))
            {
                if (!Acs_DelPrtSrtAct)
                {
                    Util.ShowMessageBox(53);
                    return;
                }

                if (Util.ShowMessageBox(36) == MessageBoxResult.Yes)
                {
                    TblWayAwr_RecvInt deletingObj = obj as TblWayAwr_RecvInt;
                    //PublicMethods.DeleteTblWayAwr_RecvInt(context, deletingObj);

                    if ((obj as TblWayAwr_RecvInt).TblEvtSrt != null)
                    {
                        PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, (obj as TblWayAwr_RecvInt).TblEvtSrt, obj as TblWayAwr_RecvInt);
                        PublicMethods.SaveContext(context);
                    }

                    ChangeSelectedObjExecute(GetEvtSrtWayAwrParent(deletingObj));
                    RemoveEvtSrtWayAwrFromItsParent(obj);
                    //PublicMethods.SaveContext(context);
                    RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");
                }
            }

            else if (obj.GetType() == typeof(TblWayAwr_Oral))
            {
                if (!Acs_DelPrtSrtAct)
                {
                    Util.ShowMessageBox(53);
                    return;
                }

                if (Util.ShowMessageBox(36) == MessageBoxResult.Yes)
                {
                    TblWayAwr_Oral deletingObj = obj as TblWayAwr_Oral;
                    //PublicMethods.DeleteTblWayAwr_Oral(context, deletingObj);

                    if ((obj as TblWayAwr_Oral).TblEvtSrt != null)
                    {
                        PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, (obj as TblWayAwr_Oral).TblEvtSrt, obj as TblWayAwr_Oral);
                        PublicMethods.SaveContext(context);
                    }


                    ChangeSelectedObjExecute(GetEvtSrtWayAwrParent(deletingObj));
                    RemoveEvtSrtWayAwrFromItsParent(obj);
                    //PublicMethods.SaveContext(context);
                    RaisePropertyChanged("TblEvtSrts", "TblEvtSrtsCV");
                }
            }


            else if (obj.GetType() == typeof(TblEvtRst))
            {
                if (!Acs_DelPrtRstAct)
                {
                    Util.ShowMessageBox(54);
                    return;
                }

                if (Util.ShowMessageBox(37) == MessageBoxResult.Yes)
                {
                    //PublicMethods.DeleteEvtRst_838(context, obj as TblEvtRst);

                    PublicMethods.DeleteEvtRstAndChgPrs_3290(context, obj as TblEvtRst);

                    this.TblEvtRsts.Remove(obj as TblEvtRst);
                    ChangeSelectedObjExecute(Activity);
                    RaisePropertyChanged("ShouldAnyCdnEvtRstVisible", "ShouldCancelEvtRstVisible", "ShouldErrEvtRstVisible");
                    RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV");
                }
            }

            else if (obj.GetType() == typeof(TblNew))
            {
                if (!Acs_DelPrtRstAct)
                {
                    Util.ShowMessageBox(54);
                    return;
                }

                if (Util.ShowMessageBox(38) == MessageBoxResult.Yes)
                {
                    TblNew deletingObj = obj as TblNew;
                    //PublicMethods.DeleteTblNew(context, deletingObj);

                    PublicMethods.DeleteObjRstOfEvtRstAndChgPrs_709(context, obj as IObjRst);

                    ChangeSelectedObjExecute(GetEvtRstWayAwrParent(deletingObj));
                    RemoveEvtRstWayAwrFromItsParent(obj);
                    RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV");
                }
            }
            else if (obj.GetType() == typeof(TblSbjOral))
            {
                if (!Acs_DelPrtRstAct)
                {
                    Util.ShowMessageBox(54);
                    return;
                }

                if (Util.ShowMessageBox(38) == MessageBoxResult.Yes)
                {
                    TblSbjOral deletingObj = obj as TblSbjOral;


                    //در نمایش نحوه‏های آگاه سازی یک رخداد نتیجه، اگر دارای دو مطلب شفاهی با یک مقصد فعالیت مشخص باشد، تنها یکی از آن‏ها نشان داده شود.
                    // در صورت حذف یکی از آن ها سایرین نیز باید حذف شوند
                    if (deletingObj.ActTarget != null && deletingObj.ActTarget.Any(a => !a.FldActUspf))
                    {
                        List<TblSbjOral> sentToSameTarget = deletingObj.TblEvtRst.TblSbjOrals.Where(o => o.GetHashCode() != deletingObj.GetHashCode() && o.ActTarget != null
                            && o.ActTarget.Any(a => !a.FldActUspf && a.FldCodAct == deletingObj.ActTarget.First().FldCodAct)).ToList();

                        foreach (var item in sentToSameTarget)
                        {
                            PublicMethods.DeleteObjRstOfEvtRstAndChgPrs_709(context, item as IObjRst);
                            RemoveEvtRstWayAwrFromItsParent(item);
                        }

                    }

                    PublicMethods.DeleteObjRstOfEvtRstAndChgPrs_709(context, obj as IObjRst);
                    ChangeSelectedObjExecute(GetEvtRstWayAwrParent(deletingObj));
                    RemoveEvtRstWayAwrFromItsParent(obj);
                    RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV");
                }
            }
            else if (obj.GetType() == typeof(TblObj))
            {
                if (!Acs_DelPrtRstAct)
                {
                    Util.ShowMessageBox(54);
                    return;
                }

                if (Util.ShowMessageBox(38) == MessageBoxResult.Yes)
                {
                    TblObj deletingObj = obj as TblObj;
                    //PublicMethods.DeleteTblObj(context, deletingObj);

                    PublicMethods.DeleteObjRstOfEvtRstAndChgPrs_709(context, obj as IObjRst);

                    ChangeSelectedObjExecute(GetEvtRstWayAwrParent(deletingObj));
                    RemoveEvtRstWayAwrFromItsParent(obj);
                    RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV");
                }
            }


        }

        TblEvtSrt GetEvtSrtWayAwrParent(EntityObject obj)
        {
            foreach (TblEvtSrt evtSrt in Activity.TblEvtSrts)
            {
                if (evtSrt.WayAwrs.Contains(obj as IWayAwr))
                {
                    return evtSrt;
                }
            }
            return null;
        }

        TblEvtRst GetEvtRstWayAwrParent(IObjRst obj)
        {
            foreach (TblEvtRst evtRst in Activity.TblEvtRsts)
            {
                if (evtRst.ObjRsts.Contains(obj))
                {
                    return evtRst;
                }
            }
            return null;
        }

        void EvtSrt_GrpChanged(object sender, EventArgs e)
        {
            TblEvtSrtsCV.Refresh();
            RaisePropertyChanged("TblEvtRsts", "TblEvtRstsCV");
        }


        /// <summary>
        /// Pdr9012
        /// </summary>
        /// <param name="deletingObj"></param>
        private void DeleteEvtSrt(TblEvtSrt deletingObj)
        {
            List<TblWayAwr_RecvInt> sndOutList = deletingObj.TblWayAwr_RecvInt.ToList();

            for (int i = 0; i < sndOutList.Count; i++)
            {

                TblWayIfrm_SndOut t1 = sndOutList[i].TblWayIfrm_SndOut;

                TblObj t2 = sndOutList[i].TblWayIfrm_SndOut.TblObj;

                TblEvtRst t3 = t2.TblEvtRst;

                if (t3.TblObjs.Count == 1 && t3.TblSbjOrals.Count == 0 && t3.TblNews.Count == 0)
                {
                    context.DeleteObject(t3);
                }
                else
                {
                    if (t2.TblWayIfrm_SndOut.Count == 1)
                    {
                        context.DeleteObject(t2);
                    }
                    else
                    {
                        context.DeleteObject(t1);
                    }
                }

            }

            List<TblWayAwr_Oral> oralList = deletingObj.TblWayAwr_Oral.ToList();

            for (int i = 0; i < oralList.Count; i++)
            {
                TblWayIfrm_Oral t1 = oralList[i].TblWayIfrm_Oral;

                TblSbjOral t2 = oralList[i].TblWayIfrm_Oral.TblSbjOral;

                TblEvtRst t3 = t2.TblEvtRst;

                if (t3.TblSbjOrals.Count == 1 && t3.TblObjs.Count == 0 && t3.TblNews.Count == 0)
                {
                    context.DeleteObject(t3);
                }
                else
                {
                    if (t2.TblWayIfrm_Oral.Count == 1)
                    {
                        context.DeleteObject(t2);
                    }
                    else
                    {
                        context.DeleteObject(t1);
                    }
                }
            }

            List<TblWayAwr_News> newsList = deletingObj.TblWayAwr_News.ToList();

            for (int i = 0; i < newsList.Count; i++)
            {
                TblWayIfrm_News t1 = newsList[i].TblWayIfrm_News;

                TblNew t2 = newsList[i].TblWayIfrm_News.TblNew;

                TblEvtRst t3 = t2.TblEvtRst;

                if (t3.TblNews.Count == 1 && t3.TblObjs.Count == 0 && t3.TblSbjOrals.Count == 0)
                {
                    context.DeleteObject(t3);
                }
                else
                {
                    if (t2.TblWayIfrm_News.Count == 1)
                    {
                        context.DeleteObject(t2);
                    }
                    else
                    {
                        context.DeleteObject(t1);
                    }
                }
            }

            if (deletingObj.FldTypEvtSrt == (int)EvtSrtType.getNewAwrAftr || deletingObj.FldTypEvtSrt == (int)EvtSrtType.getNewAwrInnTim)
            {
                context.DeleteObject(deletingObj);
            }
            else
            {
                TblEvtSrts.Remove(deletingObj);
            }
        }

        private void RemoveEvtSrtWayAwrFromItsParent(object obj)
        {
            foreach (TblEvtSrt evtSrt in Activity.TblEvtSrts)
            {
                if (evtSrt.WayAwrs.Contains(obj))
                {
                    evtSrt.WayAwrs.Remove(obj as IWayAwr);
                    if (evtSrt.WayAwrs.Count == 0)
                    {
                        evtSrt.PreviousActivity = null;
                    }
                    break;
                }
            }
        }

        private TblEvtSrt FindEvtSrtByChild(object obj)
        {
            foreach (TblEvtSrt evtSrt in Activity.TblEvtSrts)
            {
                if (evtSrt.WayAwrs.Contains(obj))
                {
                    return evtSrt;
                }
            }
            return null;
        }

        private void RemoveEvtRstWayAwrFromItsParent(object obj)
        {
            foreach (TblEvtRst evtRst in Activity.TblEvtRsts)
            {
                if (evtRst.ObjRsts.Contains(obj))
                {
                    evtRst.ObjRsts.Remove(obj as IObjRst);
                    break;
                }
            }
        }

       private void UnselectAllButThis(object obj)
        {

            if (obj == this.Activity)
            {
                this.Activity.IsSelected = true;
            }
            else
            {
                this.Activity.IsSelected = false;
            }

            foreach (var item in TblEvtSrts)
            {
                if (item != obj)
                {
                    item.IsSelected = false;
                }
                else
                {
                    item.IsSelected = true;
                }

                foreach (var awr in item.WayAwrs)
                {
                    dynamic d = awr;
                    if (awr != obj)
                    {
                        d.IsSelected = false;
                    }
                    else
                    {
                        d.IsSelected = true;
                    }
                }
            }

            foreach (var item in TblEvtRsts)
            {
                if (item != obj)
                {
                    item.IsSelected = false;
                }
                else
                {
                    item.IsSelected = true;
                }

                foreach (var awr in item.ObjRsts)
                {
                    dynamic d = awr;
                    if (awr != obj)
                    {
                        d.IsSelected = false;
                    }
                    else
                    {
                        d.IsSelected = true;
                    }
                }
            }
        }

        #endregion
    }
}
