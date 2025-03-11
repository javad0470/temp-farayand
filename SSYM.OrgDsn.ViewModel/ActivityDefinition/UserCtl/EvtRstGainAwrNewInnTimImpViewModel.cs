using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
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
    public class EvtRstGainAwrNewInnTimImpViewModel : UserControlViewModel
    {
        #region ' Fields '

        //private Model.TblWayAwr_News tblWayAwr_News;
        //private Model.TblWayIfrm_Oral tblWayIfrm_Oral;
        //private Model.TblWayAwr_RecvInt tblWayAwr_RecvInt;

        private DtlIntViewModel dtlInt;
        private DtlAwrOralViewModel dtlAwrOral;
        private DtlRecvNewsViewModel dtlRecvNews;
        object selectedObject;

        #endregion

        #region ' Initialaizer '

        public EvtRstGainAwrNewInnTimImpViewModel(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
            Model.TblEvt_GainAwrNew TblEvt_GainAwrNew = this.TblEvtRst.TblEvt_GainAwrNew.FirstOrDefault();
            this.TblEvtSrt = TblEvt_GainAwrNew.TblEvtSrt;

            this.IsCheckEventEnabled = false;

            if (this.TblEvtSrt.TblWayAwr_RecvInt.Count > 0)
            {
                Model.TblWayAwr_RecvInt tbl = this.TblEvtSrt.TblWayAwr_RecvInt.FirstOrDefault();
                this.DtlInt = new DtlIntViewModel(this.bpmnEty, this.TblEvtSrt, tbl, 0);
                this.IsRecvIntSelected = true;
            }
            else
            {
                this.DtlInt = new DtlIntViewModel(this.bpmnEty, this.TblEvtSrt, new Model.TblWayAwr_RecvInt(), 0);
                this.IsRecvIntSelected = true;
            }
            if (this.TblEvtSrt.TblWayAwr_Oral.Count > 0)
            {
                this.DtlAwrOral = new DtlAwrOralViewModel(this.bpmnEty, this.TblEvtSrt, this.TblEvtSrt.TblWayAwr_Oral.FirstOrDefault(), 0);
                this.IsAwrOralSelected = true;
            }
            else
            {
                this.DtlAwrOral = new DtlAwrOralViewModel(this.bpmnEty, this.TblEvtSrt, new Model.TblWayAwr_Oral(), 0);
            }
            if (this.TblEvtSrt.TblWayAwr_News.Count > 0)
            {
                this.DtlRecvNews = new DtlRecvNewsViewModel(this.bpmnEty, this.TblEvtSrt, this.TblEvtSrt.TblWayAwr_News.FirstOrDefault(), 0);
                this.IsRecvNewsSelected = true;
            }
            else
            {
                this.DtlRecvNews = new DtlRecvNewsViewModel(this.bpmnEty, this.TblEvtSrt, new Model.TblWayAwr_News(), 0);
            }

            UpdatePreviouslySelected();

            this.IsCheckEventEnabled = true;
        }

        #endregion

        #region ' Properties / Commands '

        public bool IsCheckEventEnabled { get; set; }

        public DtlRecvNewsViewModel DtlRecvNews
        {
            get { return dtlRecvNews; }
            set { dtlRecvNews = value; }
        }

        public DtlAwrOralViewModel DtlAwrOral
        {
            get { return dtlAwrOral; }
            set { dtlAwrOral = value; }
        }

        public DtlIntViewModel DtlInt
        {
            get { return dtlInt; }
            set { dtlInt = value; }
        }

        public object SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                RaisePropertyChanged("SelectedObject");
            }
        }

        public TblEvtRst TblEvtRst
        {
            get
            {
                return Entity as TblEvtRst;
            }
            set
            {
                Entity = value;
            }
        }

        /// <summary>
        /// رخداد آغازگر همزاد رخداد نتیجه جاری
        /// </summary>
        public TblEvtSrt TblEvtSrt { get; set; }

        private bool isRecvIntSelected;
        private bool isAwrOralSelected;
        private bool isRecvNewsSelected;
        /// <summary>
        /// دریافت ورودی انتخاب شده است
        /// </summary>
        public bool IsRecvNewsSelected
        {
            get { return isRecvNewsSelected; }
            set
            {
                //Model.TblEvt_GainAwrNew TblEvt_GainAwrNew = this.TblEvtRst.TblEvt_GainAwrNew.FirstOrDefault();
                //if (isRecvNewsSelected && !value && TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_News.Count > 0)
                //{
                //    if (TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News != null)
                //    {
                //        PublicMethods.DeleteWayIfrm_723(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News, DirectionForDelete.Right);
                //        this.DtlRecvNews = new DtlRecvNewsViewModel(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt, new Model.TblWayAwr_News());
                //    }
                //    //DeleteTblWayAwr_News(TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_News.First());
                //}
                isRecvNewsSelected = value;
                if (value)
                {
                    this.SelectedObject = DtlRecvNews;
                }
                RaisePropertyChanged("IsRecvNewsSelected");
            }
        }

        public bool IsPreviouslyRecvIntSelected { get; set; }

        public bool IsPreviouslyAwrOralSelected { get; set; }

        public bool IsPreviouslyRecvNewsSelected { get; set; }

        /// <summary>
        /// آگاهی شفاهی انتخاب شده است
        /// </summary>
        public bool IsAwrOralSelected
        {
            get { return isAwrOralSelected; }
            set
            {
                //Model.TblEvt_GainAwrNew TblEvt_GainAwrNew = this.TblEvtRst.TblEvt_GainAwrNew.FirstOrDefault();
                //if (isAwrOralSelected && !value && TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_Oral.Count > 0)
                //{
                //    if (TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral != null)
                //    {
                //        PublicMethods.DeleteWayIfrm_723(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral, DirectionForDelete.Right);
                //        this.DtlAwrOral = new DtlAwrOralViewModel(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt, new Model.TblWayAwr_Oral());
                //    }
                //    //DeleteTblWayAwr_Oral(TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_Oral.First());
                //}
                isAwrOralSelected = value;
                if (value)
                {
                    this.SelectedObject = DtlAwrOral;
                }
                RaisePropertyChanged("isAwrOralSelected");
            }
        }

        /// <summary>
        /// خبر انتخاب شده است
        /// </summary>
        public bool IsRecvIntSelected
        {
            get { return isRecvIntSelected; }
            set
            {
                //Model.TblEvt_GainAwrNew TblEvt_GainAwrNew = this.TblEvtRst.TblEvt_GainAwrNew.FirstOrDefault();
                //if (isRecvIntSelected && !value && TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_RecvInt.Count > 0)
                //{
                //    if (TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut != null)
                //    {
                //        PublicMethods.DeleteWayIfrm_723(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut, DirectionForDelete.Right);
                //        this.DtlInt = new DtlIntViewModel(this.bpmnEty, TblEvt_GainAwrNew.TblEvtSrt, new Model.TblWayAwr_RecvInt());
                //    }
                //    //DeleteTblWayAwr_RecvInt(TblEvt_GainAwrNew.TblEvtSrt.TblWayAwr_RecvInt.First());
                //}
                isRecvIntSelected = value;
                if (value)
                {
                    this.SelectedObject = DtlInt;
                }
                RaisePropertyChanged("IsRecvIntSelected");
            }
        }

        #endregion

        #region ' Public Methods '

        public void AnnounceUser()
        {
            if (IsCheckEventEnabled && this.TblEvtSrt.PreviousActivity != null)
            {
                if (Util.ShowMessageBox(14) == System.Windows.MessageBoxResult.Yes)
                {
                    ChangeWayAwrOfGetNewAwr();
                    IsCheckEventEnabled = false;
                    UpdatePreviouslySelected();
                    IsCheckEventEnabled = true;

                    RefreshEvtSrt();
                }
                else
                {
                    IsCheckEventEnabled = false;
                    ReverseSelected();
                    IsCheckEventEnabled = true;

                }
            }
        }

        public void ChangeWayAwrOfGetNewAwr()
        {

            if (this.TblEvtSrt.TblWayAwr_News.Count > 0)
            {
                if (this.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News != null)
                {
                    IWayAwr wayAwr = PublicMethods.DetectWayAwrOfWayIfrm_623(this.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News);
                    //PublicMethods.DeleteWayIfrm_723(this.bpmnEty, this.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News, DirectionForDelete.Right);

                    PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(this.bpmnEty, this.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News.TblNew, this.TblEvtSrt.TblWayAwr_News.First().TblWayIfrm_News, Model.Enum.DirectionForDelete.Left);
                }
            }
            if (this.TblEvtSrt.TblWayAwr_Oral.Count > 0)
            {
                if (this.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral != null)
                {
                    IWayAwr wayAwr = PublicMethods.DetectWayAwrOfWayIfrm_623(this.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral);
                    //PublicMethods.DeleteWayIfrm_723(this.bpmnEty, this.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral, DirectionForDelete.Right);

                    PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(this.bpmnEty, this.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral.TblSbjOral, this.TblEvtSrt.TblWayAwr_Oral.First().TblWayIfrm_Oral, Model.Enum.DirectionForDelete.Left);
                }
            }
            if (this.TblEvtSrt.TblWayAwr_RecvInt.Count > 0)
            {
                if (this.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut != null)
                {
                    IWayAwr wayAwr = PublicMethods.DetectWayAwrOfWayIfrm_623(this.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut);
                    //PublicMethods.DeleteWayIfrm_723(this.bpmnEty, this.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut, DirectionForDelete.Right);

                    PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(this.bpmnEty, this.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut.TblObj, this.TblEvtSrt.TblWayAwr_RecvInt.First().TblWayIfrm_SndOut, Model.Enum.DirectionForDelete.Left);
                }
            }

            this.DtlRecvNews.TblEvtSrt = this.TblEvtSrt;
            this.DtlRecvNews.TblWayAwr_News = new TblWayAwr_News();


            this.DtlAwrOral.TblEvtSrt = this.TblEvtSrt;
            this.DtlAwrOral.TblWayAwr_Oral = new TblWayAwr_Oral();

            this.DtlInt.TblEvtSrt = this.TblEvtSrt;
            this.DtlInt.TblWayAwr_RecvInt = new TblWayAwr_RecvInt();

        }

        #endregion

        #region ' Private Methods '

        private void RefreshEvtSrt()
        {
            //this.PublicMethods.SaveContext(bpmnEty);
            //this.bpmnEty.LoadProperty(this.TblEvtSrt, "TblWayAwr_News");
            //this.bpmnEty.LoadProperty(this.TblEvtSrt, "TblWayAwr_Oral");
            //this.bpmnEty.LoadProperty(this.TblEvtSrt, "TblWayAwr_RecvInt");
            //this.bpmnEty.Refresh(System.Data.Objects.RefreshMode.StoreWins, this.TblEvtSrt.TblWayAwr_News);
            //this.bpmnEty.Refresh(System.Data.Objects.RefreshMode.StoreWins, this.TblEvtSrt.TblWayAwr_Oral);
            //this.bpmnEty.Refresh(System.Data.Objects.RefreshMode.StoreWins, this.TblEvtSrt.TblWayAwr_RecvInt);

            PublicMethods.ReloadEntity(this.bpmnEty, this.TblEvtSrt, this.TblEvtSrt.TblWayAwr_News, "TblWayAwr_News");
            PublicMethods.ReloadEntity(this.bpmnEty, this.TblEvtSrt, this.TblEvtSrt.TblWayAwr_Oral, "TblWayAwr_Oral");
            PublicMethods.ReloadEntity(this.bpmnEty, this.TblEvtSrt, this.TblEvtSrt.TblWayAwr_RecvInt, "TblWayAwr_RecvInt");
        }

        private void UpdatePreviouslySelected()
        {
            IsPreviouslyAwrOralSelected = IsAwrOralSelected;
            IsPreviouslyRecvIntSelected = IsRecvIntSelected;
            IsPreviouslyRecvNewsSelected = IsRecvNewsSelected;
        }

        private void ReverseSelected()
        {
            IsAwrOralSelected = IsPreviouslyAwrOralSelected;
            IsRecvIntSelected = IsPreviouslyRecvIntSelected;
            IsRecvNewsSelected = IsPreviouslyRecvNewsSelected;
        }

        /// <summary>
        /// نحوه آگاهی دریافت ورودی را حذف می کند
        /// </summary>
        /// <param name="deletingObj"></param>
        private void DeleteTblWayAwr_RecvInt(TblWayAwr_RecvInt deletingObj)
        {
            try
            {
                if (deletingObj.TblWayIfrm_SndOut != null && deletingObj.TblWayIfrm_SndOut.TblObj != null)
                {
                    if (deletingObj.TblWayIfrm_SndOut.TblObj.TblWayIfrm_SndOut.Count > 1)
                    {
                        this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_SndOut);
                    }
                    else
                    {
                        TblObj obj = deletingObj.TblWayIfrm_SndOut.TblObj;
                        if (obj.TblEvtRst.TblObjs.Count == 1 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                        {
                            this.bpmnEty.DeleteObject(obj.TblEvtRst);
                        }
                        else
                        {
                            this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_SndOut.TblObj);
                        }
                    }
                }

                else
                {
                    this.bpmnEty.DeleteObject(deletingObj);
                }

            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(new OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "error while deleting TblWayAwr_RecvInt from Context in " + this.ToString(), ex));
            }
        }

        /// <summary>
        /// نحوه آگاهی خبر را حذف می کند
        /// </summary>
        /// <param name="deletingObj"></param>
        private void DeleteTblWayAwr_News(TblWayAwr_News deletingObj)
        {
            try
            {
                if (deletingObj.TblWayIfrm_News != null && deletingObj.TblWayIfrm_News.TblNew != null)
                {
                    if (deletingObj.TblWayIfrm_News.TblNew.TblWayIfrm_News.Count > 1)
                    {
                        this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_News);
                    }
                    else
                    {

                        TblNew obj = deletingObj.TblWayIfrm_News.TblNew;
                        if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 1 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                        {
                            this.bpmnEty.DeleteObject(obj.TblEvtRst);
                        }
                        else
                        {
                            this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_News.TblNew);
                        }
                    }
                }
                else
                {
                    this.bpmnEty.DeleteObject(deletingObj);
                }

            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(new OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "error while deleting TblWayAwr_News from Context in " + this.ToString(), ex));
            }
        }

        /// <summary>
        /// آگاهی شفاهی را حذف می کند
        /// </summary>
        /// <param name="deletingObj"></param>
        private void DeleteTblWayAwr_Oral(TblWayAwr_Oral deletingObj)
        {
            try
            {
                if (deletingObj.TblWayIfrm_Oral != null && deletingObj.TblWayIfrm_Oral.TblSbjOral != null)
                {
                    if (deletingObj.TblWayIfrm_Oral.TblSbjOral.TblWayIfrm_Oral.Count > 1)
                    {
                        this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_Oral);
                    }
                    else
                    {
                        TblSbjOral obj = deletingObj.TblWayIfrm_Oral.TblSbjOral;
                        if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 1)
                        {
                            this.bpmnEty.DeleteObject(obj.TblEvtRst);
                        }
                        else
                        {
                            this.bpmnEty.DeleteObject(deletingObj.TblWayIfrm_Oral.TblSbjOral);
                        }
                    }
                }
                else
                {
                    this.bpmnEty.DeleteObject(deletingObj);
                }


            }
            catch (Exception ex)
            {
                //ExceptionLogger.LogException(new OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "error while deleting TblWayAwr_Oral from Context in " + this.ToString(), ex));
            }
        }



        #endregion

        #region ' events '

        #endregion

    }
}
