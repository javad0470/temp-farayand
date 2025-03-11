using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class DsonDtlAssignedToMeViewModel : DsonDtlViewModel
    {
        #region ' Fields '
        
        List<TblAct> allActs = null;
        
        #endregion

        #region ' Initialaizer '

        public DsonDtlAssignedToMeViewModel(BPMNDBEntities context, IWayAwrIfrm _dsonObj, TblNod nod, TblNod selectedPosPst)
            : base(context, _dsonObj, nod, selectedPosPst)
        {

            allActs = selectedPosPst.TblActs.ToList();

            allActs = allActs.Where(m => !m.FldActUspf).ToList();

            LstActNod = new ObservableCollection<TblAct>(allActs);
            ActListFull = LstActNod.Any();
            AssignedCorrectlyCommand = new DelegateCommand(AssignedCorrectlyExecute);

            WrongActCommand = new DelegateCommand(WrongActExecute);

            WrongEvtCommand = new DelegateCommand(WrongEvtExecute);

            if (this.IsActSpec)
            {
                SaveEnabled = true;
                this.AssignStatus = AssignStatusType.AssignedCorrectly;
                AssignedCorrectlyExecute();
            }
            else
            {
                SaveEnabled = false;

                if (allActs.Count > 0)
                {
                    this.SelectedAct = allActs.First();
                }
            }

            IsAccepted = true;
        }

        #endregion

        #region ' Properties / Commands '
        public bool ActListFull { get; set; }
        public bool ActListVisibility
        {
            get
            {
                if (this.TypDsonCur == SSYM.OrgDsn.Model.Enum.TypDson.OutUnspcf
                    || this.TypDsonCur == SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromUnspcf
                    || this.TypDsonCur == SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromUnspcf
                    || this.TypDsonCur == SSYM.OrgDsn.Model.Enum.TypDson.InUnspcf
                    || this.TypDsonCur == SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInUnspcf
                    || this.AssignStatus == AssignStatusType.WrongAct)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        AssignStatusType assignStatus;

        public AssignStatusType AssignStatus
        {
            get { return assignStatus; }
            set
            {
                assignStatus = value;
                addedCount = 0;
                if (assignStatus == AssignStatusType.AssignedCorrectly)
                {
                    SaveEnabled = true;
                }
                else
                {
                    SaveEnabled = false;
                }
                RaisePropertyChanged("AssignStatus", "ScndFtrRgnVsbl", "ActListVisibility");
            }
        }

        public override TblAct DsonAct
        {
            get
            {
                if (InputVisibility == System.Windows.Visibility.Visible)
                {
                    return this.WayAwrIfrm.ActSrc;
                }
                else
                {
                    return this.WayAwrIfrm.ActDst;
                }
            }
        }

        public string FrstRdbCont
        {
            get
            {
                return "به درستی نسبت داده شده است.";
            }
        }

        public string ScndRdbCont
        {
            get
            {
                string evtType = "";

                if (InputVisibility == System.Windows.Visibility.Visible)
                {
                    evtType = "آغازگر";
                }
                else
                {
                    evtType = "نتیجه";
                }

                return string.Format("به اشتباه به این رخداد {0} نسبت داده شده و مربوط به رخداد {0} دیگر این فعالیت است.", evtType);
            }
        }

        public string ThrdRdbCont { get { return "به اشتباه به این فعالیت من نسبت داده شده و مربوط به فعالیت دیگر من است."; } }

        public override string AcceptRdbCnt
        {
            get
            {
                string str = "از قلم افتاده است و باید به عنوان {0} یکی از فعالیت های من انتخاب شود";

                string str1 = "";

                switch (TypDsonCur)
                {
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutUnspcf:
                        str1 = "خروجی";
                        break;
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromUnspcf:
                        str1 = "مطلب شفاهی ارسالی";
                        break;

                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromUnspcf:
                        str1 = "خبر ارسالی";
                        break;

                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InUnspcf:
                        str1 = "ورودی";
                        break;

                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInUnspcf:
                        str1 = "مطلب شفاهی دریافتی";
                        break;
                }

                str = string.Format(str, str1);
                return str;
            }
        }

        public override string RejectRdbCnt
        {
            get
            {
                string str = "به اشتباه به من نسبت داده شده است.";
                return str;
            }
        }

        public override string FrstFtrRgnText
        {
            get
            {
                string res = "{0} را به یک یا چند فعالیت مرتبط کنید.";
                return string.Format(res, WayAwrIfrm.Title);
            }
        }

        #endregion

        #region ' Public Methods '

        public ICommand AssignedCorrectlyCommand { get; set; }

        public ICommand WrongEvtCommand { get; set; }

        public ICommand WrongActCommand { get; set; }

        #endregion

        #region ' Private Methods '

        private void WrongEvtExecute()
        {
            this.AssignStatus = AssignStatusType.WrongEvt;
            //فقط فعالیت مورد بحث را نمایش بده
            if (this.InputVisibility == System.Windows.Visibility.Visible)
            {
                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct == WayAwrIfrm.ActDst.FldCodAct));
            }
            else
            {
                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct == WayAwrIfrm.ActSrc.FldCodAct));
            }
            ActListFull = LstActNod.Any();
            RaisePropertyChanged("LstActNod", "ActListFull");

            //ممکن است تعداد صفر باشد
            //این شرایط در مواقعی پیش میآید که 
            //فعالیتی که مقصد ناهمسانی است، فعالیت نامشخص باشد. بنا براین در لیست دیده نمیشود
            if (LstActNod.Count > 0)
            {
                SelectedAct = LstActNod.First();
            }

            if (this.WayAwrVM != null)
            {
                this.WayAwrVM.WrongEvt();
            }
            if (this.WayIfrmVM != null)
            {
                this.WayIfrmVM.WrongEvt();
            }
        }

        private void WrongActExecute()
        {
            this.AssignStatus = AssignStatusType.WrongAct;

            if (this.WayAwrVM != null)
            {
                //int codExcluded = 0;

                //if (WayAwrIfrm)
                //{

                //}

                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct != WayAwrIfrm.ActDst.FldCodAct
                    && m.FldCodAct != WayAwrIfrm.ActSrc.FldCodAct
                    ));
                ActListFull = LstActNod.Any();
                foreach (var act in LstActNod)
                {
                    foreach (var evtSrt in act.TblEvtSrts)
                    {
                        //PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(MainWindowViewModel.MainContext, evtSrt, (EntityObject)WayAwrIfrm);
                        if (evtSrt.WayAwrs != null)
                        {
                            IWayAwr wa = evtSrt.WayAwrs.SingleOrDefault(m => m.ObjRst == WayAwrIfrm.ObjRst);
                            if (wa != null)
                            {
                                evtSrt.WayAwrs.Remove(wa);
                            }
                        }
                    }
                }

                if (LstActNod.Count > 0)
                {
                    if (SelectedAct != LstActNod.First())
                    {
                        SelectedAct = LstActNod.First();
                        RaisePropertyChanged("SelectedAct");
                    }
                }
                else
                {
                    this.WayAwrVM.Clear();
                }

                RaisePropertyChanged("LstActNod", "ActListFull");

            }
            if (this.WayIfrmVM != null)
            {
                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct != WayAwrIfrm.ActSrc.FldCodAct
                    && m.FldCodAct != WayAwrIfrm.ActDst.FldCodAct
                    ));
                ActListFull = LstActNod.Any();
                foreach (var act in LstActNod)
                {
                    foreach (var rst in act.TblEvtRsts)
                    {
                        if (rst.ObjRsts != null)
                        {
                            IObjRst objRst = rst.ObjRsts.SingleOrDefault(m => m.HasDson);
                            if (objRst != null)
                            {
                                rst.ObjRsts.Remove(objRst);
                            }
                        }
                    }
                }

                if (LstActNod.Count > 0)
                {
                    if (SelectedAct != LstActNod.First())
                    {
                        SelectedAct = LstActNod.First();
                        RaisePropertyChanged("SelectedAct");
                    }
                }
                else
                {
                    WayIfrmVM.Clear();
                }

                RaisePropertyChanged("LstActNod", "ActListFull");

            }

        }

        private void AssignedCorrectlyExecute()
        {
            this.AssignStatus = AssignStatusType.AssignedCorrectly;
            //فقط فعالیت مورد بحث را نمایش بده
            if (this.InputVisibility == System.Windows.Visibility.Visible)
            {
                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct == WayAwrIfrm.ActDst.FldCodAct));
            }
            else
            {
                LstActNod = new ObservableCollection<TblAct>(allActs.Where(m => m.FldCodAct == WayAwrIfrm.ActSrc.FldCodAct));
            }
            ActListFull = LstActNod.Any();
            RaisePropertyChanged("LstActNod", "ActListFull");

            SelectedAct = LstActNod.First();

            // فقط نحوه آگاهی مورد بحث را نمایش بده
            if (this.WayAwrVM != null)
            {
                this.WayAwrVM.FilterByCurrentWayAwr();
            }
            if (this.WayIfrmVM != null)
            {
                this.WayIfrmVM.FilterByCurrentWayIfrm();
            }
        }

        protected override void SaveExecute()
        {

            if (!CanSettleDson())
            {
                return;
            }

            var res = Util.ShowMessageBox(73);

            if (res == MessageBoxResult.Yes)
            {
                TypDson dsonType = WayAwrIfrm.DsonType;

                if (this.IsActSpec)
                {
                    if (WayAwr != null) // فعالیت مشخص ورودی
                    {
                        int dson = (int)dsonType;
                        if (dson == 9 || dson == 10)
                        {
                            dson = 2;
                        }
                        if (dson == 7 || dson == 8)
                        {
                            dson = 1;
                        }

                        switch (this.AssignStatus)
                        {
                            case AssignStatusType.AssignedCorrectly:
                                PublicMethods.SettleDson_19202(WayAwrIfrm);
                                break;

                            case AssignStatusType.WrongEvt:

                                //if (WayAwr.ActDst.TblEvtSrts.ToList().Any(e => e way))
                                //{

                                //}
                                foreach (var evt in WayAwr.ActDst.TblEvtSrts)
                                {
                                    if (WayAwr.EvtSrt_Temp != evt)
                                    {
                                        CreateDsonForEvt(dson, evt);
                                    }
                                }

                                PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, WayAwr.EvtSrt_Temp, WayAwr);
                                break;
                            case AssignStatusType.WrongAct:

                                foreach (var act in allActs)
                                {
                                    if ((act != WayAwr.ActSrc) && (act != WayAwr.ActDst))
                                    {
                                        for (int i = 0; i < act.TblEvtSrts.Count; i++)
                                        {
                                            CreateDsonForEvt(dson, act.TblEvtSrts.Skip(i).Take(1).First());
                                        }
                                    }
                                }

                                PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, WayAwr.EvtSrt_Temp, WayAwr);

                                break;
                            default:
                                break;
                        }
                    }
                    else //فعالیت مشخص خروجی
                    {
                        switch (this.AssignStatus)
                        {
                            case AssignStatusType.AssignedCorrectly:

                                PublicMethods.SettleDson_19202(WayAwrIfrm);

                                this.WayIfrm.ObjRst.HasDson = false;

                                break;
                            case AssignStatusType.WrongEvt:

                                foreach (var evt in WayIfrm.ActSrc.TblEvtRsts)
                                {
                                    if (evt != WayIfrm.ObjRst.EvtRst)
                                    {
                                        foreach (var objRst in evt.ObjRsts)
                                        {
                                            dynamic d = objRst;
                                            d.TblEvtRst = evt;

                                            //List<EntityObject> l = new List<EntityObject>();

                                            //WayIfrm.ObjRst.WayIfrms.ForEach(m => l.Add(m as EntityObject));

                                            PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(context, WayIfrm.ObjRst, evt, objRst, new List<IWayIfrm> { WayIfrm });

                                            AddDsonForObjRst(objRst);
                                        }
                                    }
                                }

                                PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(context, WayIfrm.ObjRst, WayIfrm, DirectionForDelete.Both);

                                break;
                            case AssignStatusType.WrongAct:
                                foreach (var act in allActs)
                                {
                                    if (act == WayIfrm.ActSrc)
                                    {
                                        continue;
                                    }

                                    foreach (var evt in act.TblEvtRsts)
                                    {
                                        if (evt.ObjRsts != null)
                                        {
                                            foreach (var objRst in evt.ObjRsts)
                                            {
                                                if (objRst.HasDson)
                                                {
                                                    dynamic d = objRst;
                                                    d.TblEvtRst = evt;

                                                    //List<EntityObject> l = new List<EntityObject>();

                                                    //WayIfrm.ObjRst.WayIfrms.ForEach(m => l.Add(m as EntityObject));

                                                    PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(context, WayIfrm.ObjRst, evt, objRst, new List<IWayIfrm> { WayIfrm });

                                                    AddDsonForObjRst(objRst);
                                                }
                                            }
                                        }
                                    }
                                }

                                PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(context, WayIfrm.ObjRst, WayIfrm, DirectionForDelete.Both);

                                break;
                            default:
                                break;
                        }
                    }
                }
                else//فعالیت نا مشخص 
                {
                    if (WayAwr != null) //فعالیت نا مشخص ورودی
                    {
                        IObjRst rst = WayAwr.ObjRst;

                        foreach (var act in allActs)
                        {
                            TblEvtSrt firstEvt = act.TblEvtSrts.FirstOrDefault();
                            if (firstEvt == null)
                            {
                                continue;
                            }
                            for (int i = 0; i < act.TblEvtSrts.Count; i++)
                            {
                                if (i == 0)
                                {
                                    if (firstEvt.WayAwrs != null)
                                    {
                                        foreach (var wayAwr in firstEvt.WayAwrs)
                                        {
                                            if (wayAwr.IsDson)
                                            {
                                                int dson = 0;

                                                if (wayAwr is TblWayAwr_News)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_News.Add(wayAwr as TblWayAwr_News);
                                                    //(wayAwr as TblWayAwr_News)
                                                }
                                                if (wayAwr is TblWayAwr_Oral)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_Oral.Add(wayAwr as TblWayAwr_Oral);
                                                    dson = 2;
                                                }
                                                if (wayAwr is TblWayAwr_RecvInt)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_RecvInt.Add(wayAwr as TblWayAwr_RecvInt);
                                                    dson = 1;
                                                }

                                                PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(context, rst, wayAwr);

                                                rst = wayAwr.ObjRst;

                                                //PublicMethods.AddNewDsonFromWayAwrIfrm_19072(context, wayAwr, dson);

                                                PublicMethods.SettleDson_19202(wayAwr);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    TblEvtSrt otherEvt = act.TblEvtSrts.Skip(i).Take(1).First();

                                    if (otherEvt.WayAwrs != null)
                                    {
                                        foreach (var wayAwr in otherEvt.WayAwrs)
                                        {
                                            if (wayAwr.IsDson)
                                            {
                                                int dson = 0;

                                                if (wayAwr is TblWayAwr_News)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_News.Add(wayAwr as TblWayAwr_News);
                                                    //(wayAwr as TblWayAwr_News)
                                                }
                                                if (wayAwr is TblWayAwr_Oral)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_Oral.Add(wayAwr as TblWayAwr_Oral);
                                                    dson = 2;
                                                }
                                                if (wayAwr is TblWayAwr_RecvInt)
                                                {
                                                    wayAwr.EvtSrt_Temp.TblWayAwr_RecvInt.Add(wayAwr as TblWayAwr_RecvInt);
                                                    dson = 1;
                                                }

                                                PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(context, rst, wayAwr);

                                                //PublicMethods.AddNewDsonFromWayAwrIfrm_19072(context, wayAwr, dson);

                                                PublicMethods.SettleDson_19202(wayAwr);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else //فعالیت نا مشخص خروجی
                    {
                        foreach (var act in allActs)
                        {
                            IObjRst objRstFirst = null;
                            for (int i = 0; i < act.TblEvtRsts.Count; i++)
                            {
                                TblEvtRst rst = act.TblEvtRsts.Skip(i).Take(1).First();

                                if (rst.ObjRsts == null)
                                {
                                    continue;
                                }

                                foreach (var objRst in rst.ObjRsts)
                                {
                                    if (objRst.HasDson)
                                    {

                                        dynamic d = objRst;

                                        d.TblEvtRst = rst;


                                        if (objRstFirst == null)
                                        {
                                            PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(context, WayIfrm.ObjRst, rst, objRst, WayIfrm.ObjRst.WayIfrms);
                                            //AddDsonForObjRst(WayIfrm.ObjRst);
                                            objRstFirst = objRst;
                                        }
                                        else
                                        {
                                            PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(context, objRstFirst, rst, objRst, objRstFirst.WayIfrms);
                                            //AddDsonForObjRst(objRstFirst);
                                        }

                                        PublicMethods.SettleDson_19202(objRst.WayIfrms.First());
                                    }
                                }
                            }
                        }
                    }
                }

                if (WayAwr != null)
                {
                    WayAwr.IsAdded = false;
                    WayAwr.IsDson = false;
                }

                if (WayIfrm != null)
                {
                    WayIfrm.IsAdded = false;
                    WayIfrm.IsDson = false;
                }

                result = MessageBoxResult.Yes;
                base.SaveExecute();


                base.SaveExecute();
            }
        }

        private void CreateDsonForEvt(int dson, TblEvtSrt evt)
        {
            if (evt.WayAwrs != null)
            {
                foreach (var addedWayAwr in evt.WayAwrs)
                {
                    if (addedWayAwr.IsDson && addedWayAwr.IsAdded)
                    {
                        if (addedWayAwr is TblWayAwr_News)
                        {
                            addedWayAwr.EvtSrt_Temp.TblWayAwr_News.Add(addedWayAwr as TblWayAwr_News);
                        }
                        if (addedWayAwr is TblWayAwr_Oral)
                        {
                            addedWayAwr.EvtSrt_Temp.TblWayAwr_Oral.Add(addedWayAwr as TblWayAwr_Oral);
                        }
                        if (addedWayAwr is TblWayAwr_RecvInt)
                        {
                            addedWayAwr.EvtSrt_Temp.TblWayAwr_RecvInt.Add(addedWayAwr as TblWayAwr_RecvInt);
                        }

                        PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(context, WayAwr.ObjRst, addedWayAwr);

                        PublicMethods.AddNewDsonFromWayAwrIfrm_19072(context, addedWayAwr, dson);

                        addedWayAwr.IsDson = false;
                    }
                }
            }
        }

        private void AddDsonForObjRst(IObjRst objRst)
        {
            if (objRst.HasDson)
            {
                if (objRst is TblNew)
                {
                }
                if (objRst is TblSbjOral)
                {
                    //objRst.WayIfrms.Single(m => m.IsDson)
                    PublicMethods.AddNewDsonFromWayAwrIfrm_19072(context, objRst.WayIfrms.First(), 5);//شما را به صورت شفاهی آگاه می کند
                }
                if (objRst is TblObj)
                {
                    //objRst.WayIfrms.Single(m => m.IsDson)
                    PublicMethods.AddNewDsonFromWayAwrIfrm_19072(context, objRst.WayIfrms.First(), 4);//ورودی 1 را به شما ارسال می کند
                }

                objRst.HasDson = false;
            }
        }

        #endregion

        #region ' Events '

        #endregion

    }
}
