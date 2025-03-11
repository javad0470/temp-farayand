using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class DtlRecvNewsViewModel : UserControlViewModel
    {
        #region ' Fields '

        private Model.TblEvtSrt tblEvtSrt;
        private Model.TblWayAwr_News tblWayAwr_News;
        private Model.TblNew tblNews;
        private int previousActivityID;

        private SlcNewsRecvViewModel slcNews;
        private DefNewsViewModel defNews;

        #endregion

        #region ' Initialaizer '

        public DtlRecvNewsViewModel(BPMNDBEntities context, EntityObject obj, EntityObject obj2, int codSelectedNod)
            : base(context, obj, obj2)
        {
            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            SelectNewsPopupIsOpenCommand = new DelegateCommand(ExecuteSelectNewsPopupIsOpenCommand);
            SlcNews = new SlcNewsRecvViewModel();
            DefNews = new DefNewsViewModel(codSelectedNod);
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();

            //TblWayAwr_News tbl = ((TblEvtSrt)Entity).TblWayAwr_News.FirstOrDefault();
            //TblWayAwr_Oral tbl2 = ((TblEvtSrt)Entity).TblWayAwr_Oral.FirstOrDefault();
            //TblWayAwr_RecvInt tbl3 = ((TblEvtSrt)Entity).TblWayAwr_RecvInt.FirstOrDefault();

            //if (((Model.TblWayAwr_News)Entity2).TblWayIfrm_News != null)
            //{
            //    this.PreviousActivityID = ((TblWayAwr_News)Entity2).TblWayIfrm_News.TblNew.TblEvtRst.TblAct.FldCodAct;
            //}
            //else if (tbl != null)
            //{
            //    this.PreviousActivityID = tbl.TblWayIfrm_News.TblNew.TblEvtRst.FldCodAct;
            //}
            //else if (tbl2 != null)
            //{
            //    this.PreviousActivityID = tbl2.TblWayIfrm_Oral.TblSbjOral.TblEvtRst.FldCodAct;
            //}
            //else if (tbl3 != null)
            //{
            //    this.PreviousActivityID = tbl3.TblWayIfrm_SndOut.TblObj.TblEvtRst.FldCodAct;
            //}

            //TblEvtSrt = bpmnEty.TblEvtSrts.SingleOrDefault(E => E.FldCodEvtSrt == 23);
            //TblWayAwr_News = TblEvtSrt.TblWayAwr_News.FirstOrDefault();

            //previousActivityID = 2;
            //this.TblEvtSrt = new Model.TblEvtSrt() { FldCodAct = 1, FldSttAct = 1, FldGrpEvt = 3, FldTypEvtSrt = 1 };
            //bpmnEty.TblEvtSrts.AddObject(this.TblEvtSrt);
            //this.TblWayAwr_News = new Model.TblWayAwr_News();


        }

        #endregion

        #region ' Properties / Commands '

        ///// <summary>
        ///// شناسه فعالیت قبلی. منظور شناسه فعالیتی است که در قسمت جزئیات رخداد آغازگر آمده است. در صورتی که در قسمت جزئیات فعالیتی نیامده نیازی به ارسال این پراپرتی نیست
        ///// </summary>
        //public int PreviousActivityID
        //{
        //    get { return previousActivityID; }
        //    set
        //    {
        //        previousActivityID = value;
        //        RaisePropertyChanged("PreviousActivityName");
        //        RaisePropertyChanged("PreviousActivityPerformer");
        //    }
        //}

        ///// <summary>
        ///// previous activity name
        ///// </summary>
        //public string PreviousActivityName
        //{
        //    get
        //    {
        //        if (bpmnEty.TblActs.SingleOrDefault(E => E.FldCodAct == PreviousActivityID) != null)
        //        {
        //            return bpmnEty.TblActs.SingleOrDefault(E => E.FldCodAct == PreviousActivityID).FldNamAct;
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}

        ///// <summary>
        ///// previous activity performer
        ///// </summary>
        //public string PreviousActivityPerformer
        //{
        //    get
        //    {
        //        return Model.PublicMethods.ActivityPerformerName_951(PreviousActivityID);

        //        //int entityType = bpmnEty.TblActs.SingleOrDefault(E => E.FldCodAct == PreviousActivityID).TblNod.FldCodTypEty;
        //        //int entityID = bpmnEty.TblActs.SingleOrDefault(E => E.FldCodAct == PreviousActivityID).TblNod.FldCodEty;
        //        //switch (entityType)
        //        //{
        //        //    case 1:
        //        //        return bpmnEty.TblOrgs.SingleOrDefault(E => E.FldCodOrg == entityID).FldNamOrg;
        //        //    case 2:
        //        //        return bpmnEty.TblPosPstOrgs.SingleOrDefault(E => E.FldCodPosPst == entityID).FldNamPosPst;
        //        //    case 3:
        //        //        return bpmnEty.TblPsns.SingleOrDefault(E => E.FldCodPsn == entityID).FldNam1stPsn + " " + bpmnEty.TblPsns.SingleOrDefault(E => E.FldCodPsn == entityID).FldNam2ndPsn;
        //        //    case 4:
        //        //        return bpmnEty.TblRols.SingleOrDefault(E => E.FldCodRol == entityID).FldTtlRol;
        //        //    default:
        //        //        return "";
        //        //}
        //    }
        //}

        /// <summary>
        /// Way of ware
        /// </summary>
        public Model.TblWayAwr_News TblWayAwr_News
        {
            get
            {
                //if (((TblWayAwr_News)Entity2).TblWayIfrm_News != null)
                //{
                //    PreviousActivityID = ((TblWayAwr_News)Entity2).TblWayIfrm_News.TblNew.TblEvtRst.TblAct.FldCodAct;
                //    RaisePropertyChanged("PreviousActivityID");
                //}
                return Entity2 as TblWayAwr_News;

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
            get
            {
                return Entity as TblEvtSrt;
            }
            set
            {
                Entity = value;
                RaisePropertyChanged("TblEvtSrt");
            }
        }

        /// <summary>
        /// save changes command
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// opens popup for input selection command
        /// </summary>
        public ICommand SelectNewsPopupIsOpenCommand { get; set; }

        /// <summary>
        /// SlcNewsViewModel
        /// </summary>
        public SlcNewsRecvViewModel SlcNews
        {
            get
            {
                return slcNews;
            }
            set
            {
                slcNews = value;
            }
        }


        /// <summary>
        /// DefNewsViewModel
        /// </summary>
        public DefNewsViewModel DefNews
        {
            get { return defNews; }
            set { defNews = value; }
        }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcNews.Dispose();

            this.DefNews.Dispose();
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
        private void ExecuteSelectNewsPopupIsOpenCommand()
        {
            this.SlcNews.ActCnt = this.TblEvtSrt.TblAct;

            this.SlcNews.EvtSrt = this.TblEvtSrt;

            if (this.TblWayAwr_News.TblWayIfrm_News != null)
            {
                this.SlcNews.CodNewsCnt = this.TblWayAwr_News.TblWayIfrm_News.TblNew.FldCodNews;
            }

            //Pdr9053(1)
            //if (PublicMethods.DetectWayAwrOfEvtSrtWithWayIfrm_503(bpmnEty, this.TblEvtSrt).Count > 0)
            //{
            //    this.SlcNews.PreviousActivity = this.TblEvtSrt.PreviousActivity;

            //    this.SlcNews.CodActPvs = this.TblEvtSrt.PreviousActivity.FldCodAct;
            //}
            //else
            //{
            //    this.SlcNews.PreviousActivity = null;

            //    this.SlcNews.CodActPvs = 0;
            //}

            this.SlcNews.PreviousActivity = this.TblEvtSrt.PreviousActivity;

            this.SlcNews.CodActPvs = this.TblEvtSrt.PreviousActivity != null ? this.TblEvtSrt.PreviousActivity.FldCodAct : 0;


            this.SlcNews.IsSentTooMeSelected = false;

            this.SlcNews.IsAllNewsSelected = true;


            Util.ShowPopup(this.SlcNews);

            if (this.SlcNews.Result == PopupResult.OK)
            {
                TblNew tblNews;

                if (this.SlcNews.IsSentTooMeSelected)
                {
                    // برای تمامی رخداد های آغازگر غیر از صرفا پس از آگاهی چه ورودی جه خبر و جه مطلب شفاهی، فعالیت مبداء نامشخص قابل انتخاب نباشد
                    if (this.SlcNews.SentToMeObjectsSelectedItem.Item1.ActSrc.FldActUspf && this.TblEvtSrt.TypSrt != Model.Enum.EvtSrtType.aftrAwareEvtSrt)
                    {
                        Util.ShowMessageBox(75);
                        return;
                    }
                    tblNews = this.bpmnEty.TblNews.SingleOrDefault(E => E.FldCodNews == ((TblNew)this.SlcNews.SentToMeObjectsSelectedItem.Item1).FldCodNews);
                }
                else
                {
                    // برای تمامی رخداد های آغازگر غیر از صرفا پس از آگاهی چه ورودی جه خبر و جه مطلب شفاهی، فعالیت مبداء نامشخص قابل انتخاب نباشد
                    if (this.SlcNews.SelectedItem.Item1.ActSrc.FldActUspf && this.TblEvtSrt.TypSrt != Model.Enum.EvtSrtType.aftrAwareEvtSrt)
                    {
                        Util.ShowMessageBox(75);
                        return;
                    }

                    tblNews = this.bpmnEty.TblNews.SingleOrDefault(E => E.FldCodNews == ((TblNew)this.SlcNews.SelectedItem.Item1).FldCodNews);
                }

                if (this.TblWayAwr_News.TblEvtSrt == null)
                {
                    this.TblWayAwr_News.TblEvtSrt = this.TblEvtSrt;
                }

                PublicMethods.AddExistingObjRstToWayAwrAndChgPrs_6692(this.bpmnEty, tblNews, this.TblWayAwr_News);

                if (this.TblWayAwr_News.TblWayIfrm_News != null)
                {
                    RaisePropertyChanged("TblEvtSrt", "TblWayAwr_News");
                }
            }

            if (this.SlcNews.Result == PopupResult.Yes)
            {

                DefNews.EvtSrt = this.TblEvtSrt;

                //if (this.TblEvtSrt.TblWayAwr_RecvInt.Count() > 0 || this.TblEvtSrt.TblWayAwr_Oral.Count > 0 || this.TblEvtSrt.TblWayAwr_News.Count > 1)
                //{
                //    this.DefNews.PreviousActivity = this.TblEvtSrt.PreviousActivity;
                //    this.DefNews.IsSelectSourceEnabel = false;
                //}
                //else
                //{
                //    this.DefNews.PreviousActivity = null;
                //    this.DefNews.IsSelectSourceEnabel = true;
                //}

                this.DefNews.PreviousActivity = this.TblEvtSrt.PreviousActivity;
                this.DefNews.PerformerName = this.TblEvtSrt.PreviousActivityPerformer;
                this.DefNews.IsSelectSourceEnabel = this.TblEvtSrt.PreviousActivity == null;


                Util.ShowPopup(DefNews);

                if (this.DefNews.Result == PopupResult.OK)
                {

                    TblNew tblNews = new TblNew() { FldTtlNews = this.DefNews.TblNews.FldTtlNews, FldTxtNews = this.DefNews.TblNews.FldTxtNews };

                    if (this.TblWayAwr_News.TblEvtSrt == null)
                    {
                        this.TblWayAwr_News.TblEvtSrt = this.TblEvtSrt;
                    }

                    var news = this.TblWayAwr_News as SSYM.OrgDsn.Model.Base.IWayAwr;

                    PublicMethods.AddNewObjRstToWayAwr_1017(this.bpmnEty, tblNews, news, this.bpmnEty.TblActs.Single(m => m.FldCodAct == this.DefNews.PreviousActivity.FldCodAct));
                }

                RaisePropertyChanged("TblEvtSrt", "TblWayAwr_News");

            }


        }

        #endregion
    }
}
