using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class DtlAwrOralViewModel : UserControlViewModel
    {
        #region ' Fields '

        private bool isAwarenessByPhone;
        private SlcActSrcViewModel slcActSrc;
        int _codSelectedNod = 0;
        //BPMNDBEntities context;

        #endregion

        #region ' Initialaizer '

        public DtlAwrOralViewModel(BPMNDBEntities context, EntityObject obj, EntityObject obj2, int codSelectedNod)
            : base(context, obj, obj2)
        {
            this._codSelectedNod = codSelectedNod;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Initialiaze()
        {
            //context = MenuViewModel.MainContext;

            base.Initialiaze();

            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);

            SelectActivityPopupIsOpenCommand = new DelegateCommand(ExecuteSelectActivityPopupIsOpenCommand, CanExecuteSelectActivityPopupIsOpenCommand);

            SlcActSrc = new SlcActSrcViewModel(bpmnEty, justShowOralSenders: true);

            SlcActSrc.FirstTabHeader = "آگاه کننده شفاهی به من";

            (SelectActivityPopupIsOpenCommand as DelegateCommand).RaiseCanExecuteChanged();

            if (this.TblEvtSrt.PreviousActivity != null && this.TblWayAwr_Oral.TblEvtSrt != this.TblEvtSrt)
            {
                if (this.TblWayAwr_Oral.TblEvtSrt == null)
                {
                    this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                }

                //PublicMethods.AddNewObjRstToWayAwr_1017(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, this.TblEvtSrt.PreviousActivity);

                PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, this.TblEvtSrt.PreviousActivity);

                RaisePropertyChanged("TblEvtSrt");
            }

            SlcSrcAndDst = new SlcSrcAndDstViewModel();

            SlcSrcAndDst.IsSelectionModeSingle = true;

            this.SlcSrcAndDst.IsOutOrgVisible = this.TblEvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt;
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// Way of ware
        /// </summary>
        public Model.TblWayAwr_Oral TblWayAwr_Oral
        {
            get
            {

                return Entity2 as Model.TblWayAwr_Oral;
            }
            set
            {
                Entity2 = value;
                //if (value.TblWayIfrm_Oral != null)
                //{
                //    PreviousActivityID = value.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct.FldCodAct;
                //    RaisePropertyChanged("PreviousActivityID");
                //    RaisePropertyChanged("IsAwarenessByPhone");
                //}
            }
        }

        /// <summary>
        /// Start event
        /// </summary>
        public Model.TblEvtSrt TblEvtSrt
        {
            get { return Entity as Model.TblEvtSrt; }
            set
            {
                Entity = value;
                (SelectActivityPopupIsOpenCommand as DelegateCommand).RaiseCanExecuteChanged();
                RaisePropertyChanged("TblEvtSrt");
            }
        }

        /// <summary>
        /// gets or sets the type of awareness
        /// </summary>
        public bool IsAwarenessByPhone
        {
            get
            {
                if (TblWayAwr_Oral.FldTypAwr == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                isAwarenessByPhone = value;
                if (value == false)
                {
                    TblWayAwr_Oral.FldTypAwr = 1;
                }
                else
                {
                    TblWayAwr_Oral.FldTypAwr = 2;
                }

            }
        }

        /// <summary>
        /// save changes command
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// opens popup for input selection command
        /// </summary>
        public ICommand SelectActivityPopupIsOpenCommand { get; set; }

        /// <summary>
        /// SlcActSrcViewModel
        /// </summary>
        public SlcActSrcViewModel SlcActSrc
        {
            get { return slcActSrc; }
            set { slcActSrc = value; }
        }

        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }


        public DefOralViewModel DefOralVM { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcActSrc.Dispose();

            this.SlcSrcAndDst.Dispose();
        }

        #endregion

        #region ' Private Methods '

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
        private void ExecuteSelectActivityPopupIsOpenCommand()
        {

            if (this.TblEvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt)
            {
                Util.ShowPopup(SlcSrcAndDst);

                if (this.SlcSrcAndDst.SelectedItem == null)
                {
                    return;
                }

                if (this.SlcSrcAndDst.SelectedItem.Nod.FldCodNod == _codSelectedNod)
                {
                    Util.ShowMessageBox(74);
                    return;
                }

                if (SlcSrcAndDst.Result == Base.PopupResult.OK && this.SlcSrcAndDst.SelectedItem != null)
                {
                    if (this.TblWayAwr_Oral.TblEvtSrt == null)
                    {
                        this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                    }

                    //TblPosPstOrg posPst = SlcSrcAndDst.SelectedItem as TblPosPstOrg;
                    if (SlcSrcAndDst.SelectedItem.Nod.FldCodEty == _codSelectedNod)
                    {
                        Util.ShowMessageBox(56);
                        RaisePropertyChanged("ActDoesntExist");
                        return;
                    }

                    int codAct = this.SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(n => n.FldActUspf).FldCodAct;
                    TblAct tblAct = this.bpmnEty.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);

                    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, tblAct);

                    RaisePropertyChanged("TblEvtSrt");
                }

                this.SlcSrcAndDst.IsDepOrgVisible = this.SlcSrcAndDst.IsOutOrgVisible = this.TblEvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt;

                return;
            }


            if (this.TblEvtSrt.TblWayAwr_RecvInt.Count() > 0 || this.TblEvtSrt.TblWayAwr_News.Count > 0)
            {
                SlcActSrc.CanUsrSlcAct = false;
            }
            else
            {
                SlcActSrc.CanUsrSlcAct = true;
            }
            this.SlcActSrc.EvtSrt = this.TblEvtSrt;
            SlcActSrc.ActCnt = this.TblEvtSrt.TblAct;
            SlcActSrc.DestinationActivityID = this.TblEvtSrt.FldCodAct;
            SlcActSrc.IsSentTooMeSelected = true;

            Util.ShowPopup(SlcActSrc);

            if (SlcActSrc.Result == Base.PopupResult.OK)
            {


                if (this.SlcActSrc.IsSentTooMeSelected)
                {
                    if (this.SlcActSrc.SentToMeObjectsSelectedItem.Item1.ActSrc.FldActUspf)
                    {
                        Util.ShowMessageBox(75);
                        return;
                    }

                    if (this.TblWayAwr_Oral.TblEvtSrt == null)
                    {
                        this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                    }
                    //TblSbjOral sbjOral = new TblSbjOral();

                    //TblEvtRst evtRst = this.bpmnEty.TblEvtRsts.Single(m => m.FldCodEvtRst == this.SlcActSrc.SentToMeObjectsSelectedItem.Item1.EvtRst.FldCodEvtRst);

                    //evtRst.TblSbjOrals.Add(sbjOral);

                    //if (this.SlcActSrc.SentToMeObjectsSelectedItem.Item2.TblAct.FldActUspf)
                    //{
                    //    PublicMethods.DeleteObjRstOfEvtRstAndChgPrs_709(this.bpmnEty, this.bpmnEty.TblSbjOrals.Single(m => m.FldCodSbjOral == this.SlcActSrc.SentToMeObjectsSelectedItem.Item1.CodObj));
                    //}

                    PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(this.bpmnEty, this.SlcActSrc.SentToMeObjectsSelectedItem.Item1, this.TblWayAwr_Oral);
                }
                else if (this.SlcActSrc.IsActsOfNodCntSelected)
                {
                    if (this.SlcActSrc.ActOfNodCntSelectedItem.FldActUspf)
                    {
                        Util.ShowMessageBox(75);
                        return;
                    }

                    if (this.TblWayAwr_Oral.TblEvtSrt == null)
                    {
                        this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                    }

                    TblAct tblAct = this.bpmnEty.TblActs.Single(m => m.FldCodAct == this.SlcActSrc.ActOfNodCntSelectedItem.FldCodAct);

                    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, tblAct);
                }

                RaisePropertyChanged("TblEvtSrt");
            }

            else if (SlcActSrc.Result == Base.PopupResult.Yes)
            {
                DefOralVM = new DefOralViewModel(bpmnEty, _codSelectedNod, TblEvtSrt);

                Util.ShowPopup(DefOralVM);

                if (DefOralVM.Result == Base.PopupResult.OK && this.DefOralVM.SelectedAct != null)
                {
                    var tblAct = bpmnEty.TblActs.SingleOrDefault(a => a.FldCodAct == this.DefOralVM.SelectedAct.FldCodAct);

                    if (this.TblWayAwr_Oral.TblEvtSrt == null)
                    {
                        this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                    }

                    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, tblAct);
                    RaisePropertyChanged("TblEvtSrt");
                }

                //Util.ShowPopup(SlcSrcAndDst);

                //if (SlcSrcAndDst.Result == Base.PopupResult.OK && this.SlcSrcAndDst.SelectedItem != null)
                //{
                //    if (this.TblWayAwr_Oral.TblEvtSrt == null)
                //    {
                //        this.TblWayAwr_Oral.TblEvtSrt = this.TblEvtSrt;
                //    }

                //if (SlcSrcAndDst.SelectedItem.Nod.FldCodEty == _codSelectedNod)
                //    {
                //        Util.ShowMessageBox(56);
                //        RaisePropertyChanged("ActDoesntExist");
                //        return;
                //    }

                //    int codAct = this.SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(n => n.FldActUspf).FldCodAct;
                //    TblAct tblAct = this.bpmnEty.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);

                //    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(this.bpmnEty, new TblSbjOral(), this.TblWayAwr_Oral, tblAct);

                //    RaisePropertyChanged("TblEvtSrt");
                //}

                //this.SlcSrcAndDst.IsDepOrgVisible = this.SlcSrcAndDst.IsOutOrgVisible = this.TblEvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt;

            }


        }

        /// <summary>
        /// در صورتی که برای این رخداد آغازگر بیش از یک نحوه آگاهی تعریف شده باشد کاربر نمی تواند فعالیت دیگری را انتخاب نماید
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteSelectActivityPopupIsOpenCommand()
        {
            //if ((this.TblEvtSrt.TblWayAwr_Oral.Count() >= 1 && this.TblWayAwr_Oral.TblEvtSrt != this.TblEvtSrt) || this.TblEvtSrt.TblWayAwr_RecvInt.Count > 0 || this.TblEvtSrt.TblWayAwr_News.Count > 0)
            if (PublicMethods.DetectWayAwrOfEvtSrtWithWayIfrm_503(this.bpmnEty, this.TblEvtSrt).Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InsertWayAwr()
        {
            int codAct = this.TblEvtSrt.PreviousActivity.FldCodAct;
            this.TblEvtSrt.TblWayAwr_Oral.Add(this.TblWayAwr_Oral);

            int typEvtRst = (int)Model.PublicMethods.DetectTypEvtRstEqualToTypEvtSrt_508(this.TblEvtSrt.FldTypEvtSrt);
            List<Model.VwAllUsedIfrmOralWithSourceName> tbl = new List<Model.VwAllUsedIfrmOralWithSourceName>
(this.bpmnEty.VwAllUsedIfrmOralWithSourceNames.Where(E => E.FldCodActSrc == codAct && E.FldCodNod == this.TblEvtSrt.TblAct.FldCodNod
&& E.FldActUspf == true && E.FldTypEvtRst == typEvtRst));

            //اگر فعالیت انتخاب شده به یک فعالیت نامشخص از مجری جاری منسوب شده باشد به شرطی که رخداد نتیجه آن از نوع معادل رخداد آغازگر جاری باشد
            if (tbl.Count > 0)
            {

                //تعریف نحوه آگاه سازی معادل با نحوه آگاهی جاری
                int codSbjOral = tbl.FirstOrDefault().FldCodSbjOral;
                this.TblWayAwr_Oral.TblWayIfrm_Oral =
                    new TblWayIfrm_Oral() { FldTypIfrm = this.TblWayAwr_Oral.FldTypAwr, FldCodSbjOral = codSbjOral };
                //حذف نحوه آگاه سازی که قبلا به فعالیت نامشخص منسوب شده است
                int codWayIfrm = tbl.FirstOrDefault().FldCodWayIfrm;
                TblWayIfrm_Oral tblWayIfrm_Oral2 = bpmnEty.TblWayIfrm_Oral.SingleOrDefault(E => E.FldCodWayIfrm == codWayIfrm);
                //bpmnEty.DeleteObject(tblWayIfrm_Oral2);

                if (tblWayIfrm_Oral2.TblWayAwr_Oral.TblEvtSrt.TblWayAwr_RecvInt.Count == 0 && tblWayIfrm_Oral2.TblWayAwr_Oral.TblEvtSrt.TblWayAwr_Oral.Count == 1 && tblWayIfrm_Oral2.TblWayAwr_Oral.TblEvtSrt.TblWayAwr_News.Count == 0)
                {
                    this.bpmnEty.DeleteObject(tblWayIfrm_Oral2.TblWayAwr_Oral.TblEvtSrt);
                    this.bpmnEty.DeleteObject(tblWayIfrm_Oral2);
                }
                else
                {
                    this.bpmnEty.DeleteObject(tblWayIfrm_Oral2);
                }



                //حذف نحوه های آگاهی که معادل با آنها هیچ نحوه آگاه سازی نیست
                List<Model.TblWayAwr_Oral> tbl2 = new List<Model.TblWayAwr_Oral>(this.TblEvtSrt.TblWayAwr_Oral);
                for (int i = 0; i < tbl2.Count(); i++)
                {
                    if (tbl2[i].TblWayIfrm_Oral == null)
                    {
                        this.bpmnEty.DeleteObject(tbl2[i]);
                    }
                }
            }
            else
            {
                //تعریف نحوه آگاه سازی معادل با نحوه آگاهی جاری
                this.TblWayAwr_Oral.TblWayIfrm_Oral =
                    new TblWayIfrm_Oral() { FldTypIfrm = this.TblWayAwr_Oral.FldTypAwr };
                this.TblWayAwr_Oral.TblWayIfrm_Oral.TblSbjOral = new TblSbjOral();
                this.TblWayAwr_Oral.TblWayIfrm_Oral.TblSbjOral.TblEvtRst = new TblEvtRst() { FldCodAct = codAct, FldSttAct = 1, FldTypEvtRst = typEvtRst };


                //حذف نحوه های آگاهی که معادل با آنها هیچ نحوه آگاه سازی نیست
                List<Model.TblWayAwr_Oral> tbl2 = new List<Model.TblWayAwr_Oral>(this.TblEvtSrt.TblWayAwr_Oral);
                for (int i = 0; i < tbl2.Count(); i++)
                {
                    if (tbl2[i].TblWayIfrm_Oral == null)
                    {
                        this.bpmnEty.DeleteObject(tbl2[i]);
                    }
                }
            }

            RaisePropertyChanged("TblEvtSrt");
        }





        #endregion
    }
}
