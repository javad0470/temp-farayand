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
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class DtlIfrmOralViewModel : UserControlViewModel
    {
        #region ' Fields '

        private bool isAwarenessByPhone;

        private bool isSelectActivityPopupOpen;

        bool _actDoesntExist;

        int _codSelectedNod = 0;
        //BPMNDBEntities context;

        //private Model.TblSbjOral tblSbjOral;
        //private Model.TblWayIfrm_Oral tblWayIfrm_Oral;



        #endregion

        #region ' Initialaizer '

        public DtlIfrmOralViewModel(BPMNDBEntities context, EntityObject obj, int codSelectedNod)
            : base(context, obj)
        {
            this._codSelectedNod = codSelectedNod;
        }

        protected override void Initialiaze()
        {
            //context = MenuViewModel.MainContext;

            base.Initialiaze();

            //TblSbjOral = bpmnEty.TblSbjOrals.SingleOrDefault(E => E.FldCodSbjOral == 10);

            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);

            SelectActivityPopupIsOpenCommand = new DelegateCommand(ExecuteSelectActivityPopupIsOpenCommand);

            SlcActDst = new SlcActDstViewModel();

            SlcActDst.FirstTabHeader = "آگاه شده شفاهی توسط من";

            RaisePropertyChanged("TblWayIfrm_Oral", "PerformerName", "IsAwarenessByPhone");

            SlcSrcAndDst = new SlcSrcAndDstViewModel();

            SlcSrcAndDst.IsSelectionModeSingle = true;

            this.SlcSrcAndDst.IsOutOrgVisible = true;
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// TblSbjOral
        /// </summary>
        public TblSbjOral TblSbjOral
        {
            get { return Entity as TblSbjOral; }
            set
            {
                Entity = value;
                //RaisePropertyChanged("TblWayIfrm_Oral", "PerformerName", "IsAwarenessByPhone");
            }
        }

        /// <summary>
        /// TblWayIfrm_Oral
        /// </summary>
        public Model.TblWayIfrm_Oral TblWayIfrm_Oral
        {
            get
            {
                return TblSbjOral.TblWayIfrm_Oral.FirstOrDefault();
            }
            //set
            //{
            //    tblWayIfrm_Oral = value;
            //    RaisePropertyChanged("TblWayIfrm_Oral", "PerformerName", "IsAwarenessByPhone");
            //}
        }

        /// <summary>
        /// PerformerName
        /// </summary>
        public string PerformerName
        {
            get
            {
                if (this.TblWayIfrm_Oral != null)
                {
                    return Model.PublicMethods.ActivityPerformerName_951(TblWayIfrm_Oral.TblWayAwr_Oral.TblEvtSrt.FldCodAct);
                }
                return null;
            }
        }

        /// <summary>
        /// gets or sets the type of awareness
        /// </summary>
        public bool IsAwarenessByPhone
        {
            get
            {
                if (this.TblWayIfrm_Oral != null)
                {
                    if (TblWayIfrm_Oral.FldTypIfrm == 1)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            set
            {
                isAwarenessByPhone = value;
                if (TblWayIfrm_Oral != null)
                {
                    if (value == false)
                    {
                        TblWayIfrm_Oral.FldTypIfrm = 1;
                    }
                    else
                    {
                        TblWayIfrm_Oral.FldTypIfrm = 2;
                    }
                }
            }
        }

        /// <summary>
        /// gets or sets the type of awareness
        /// </summary>
        //public bool IsAwarenessByPhone
        //{
        //    get
        //    {
        //        if (TblWayAwr_Oral.FldTypAwr == 1)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    set
        //    {
        //        isAwarenessByPhone = value;
        //        if (value == false)
        //        {
        //            TblWayAwr_Oral.FldTypAwr = 1;
        //        }
        //        else
        //        {
        //            TblWayAwr_Oral.FldTypAwr = 2;
        //        }

        //    }
        //}

        /// <summary>
        /// save changes command
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        /// <summary>
        /// opens popup for input selection command
        /// </summary>
        public ICommand SelectActivityPopupIsOpenCommand { get; set; }

        /// <summary>
        /// Is select input popup opem
        /// </summary>
        public bool IsSelectActivityPopupOpen
        {
            get
            {
                return isSelectActivityPopupOpen;
            }
            set
            {
                isSelectActivityPopupOpen = value;
                RaisePropertyChanged("IsSelectActivityPopupOpen");
                if (!value && SlcActDst.Result == Base.PopupResult.OK)
                {
                    TblSbjOral objRst = this.TblSbjOral;
                    TblAct act;

                    TblEvtRst evtRst = this.TblSbjOral.TblEvtRst;

                    TblWayIfrm_Oral wayIfrm = new Model.TblWayIfrm_Oral() { FldTypIfrm = 1 };

                    List<TblWayIfrm_Oral> lst = PublicMethods.DetectWayIfrmOfSbjOral_588(this.bpmnEty, objRst);

                    foreach (TblWayIfrm_Oral item in lst)
                    {
                        PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst, item, Model.Enum.DirectionForDelete.Left);
                    }

                    if (SlcActDst.IsFomMeeSlcEed)
                    {
                        act = this.bpmnEty.TblActs.SingleOrDefault(m => m.FldCodAct == SlcActDst.FomMeeSlcEedItm.Item2.TblAct.FldCodAct);

                        //PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);

                        TblSbjOral sbj = bpmnEty.TblSbjOrals.Single(m => m.FldCodSbjOral == SlcActDst.FomMeeSlcEedItm.Item1.CodObj);

                        PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(bpmnEty, sbj, evtRst, objRst, sbj.WayIfrms);
                    }
                    else if (SlcActDst.IsActsOfNodCntSelected)
                    {
                        act = this.bpmnEty.TblActs.SingleOrDefault(m => m.FldCodAct == SlcActDst.ActOfNodCntSelectedItem.FldCodAct);

                        PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);
                    }
                }

                else if (!value && SlcActDst.Result == Base.PopupResult.Yes)
                {
                    Util.ShowPopup(SlcSrcAndDst);
                    if (SlcSrcAndDst.Result == Base.PopupResult.OK)
                    {
                        actNotExist();
                    }
                }

                RaisePropertyChanged("TblWayIfrm_Oral", "PerformerName");
            }
        }

        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

        /// <summary>
        /// SlcActDstViewModel
        /// </summary>
        public SlcActDstViewModel SlcActDst { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcSrcAndDst.Dispose();

            this.SlcActDst.Dispose();
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
            SlcActDst.EvtRstCnt = this.TblSbjOral.TblEvtRst;
            SlcActDst.IsFomMeeSlcEed = true;


            Util.ShowPopup(SlcActDst);

            if (SlcActDst.Result == Base.PopupResult.OK)
            {
                TblSbjOral objRst = this.TblSbjOral;
                TblAct act;

                TblEvtRst evtRst = this.TblSbjOral.TblEvtRst;

                TblWayIfrm_Oral wayIfrm = new Model.TblWayIfrm_Oral() { FldTypIfrm = 1 };

                List<TblWayIfrm_Oral> lst = PublicMethods.DetectWayIfrmOfSbjOral_588(this.bpmnEty, objRst);

                foreach (TblWayIfrm_Oral item in lst)
                {
                    PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst, item, Model.Enum.DirectionForDelete.Left);
                }

                if (SlcActDst.IsFomMeeSlcEed)
                {
                    act = this.bpmnEty.TblActs.SingleOrDefault(m => m.FldCodAct == SlcActDst.FomMeeSlcEedItm.Item2.TblAct.FldCodAct);

                    //PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);

                    TblSbjOral sbj = bpmnEty.TblSbjOrals.Single(m => m.FldCodSbjOral == SlcActDst.FomMeeSlcEedItm.Item1.CodObj);

                    PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(bpmnEty, sbj, evtRst, objRst, sbj.WayIfrms);
                }
                else if (SlcActDst.IsActsOfNodCntSelected)
                {
                    act = this.bpmnEty.TblActs.SingleOrDefault(m => m.FldCodAct == SlcActDst.ActOfNodCntSelectedItem.FldCodAct);

                    PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);
                }
            }

            else if (SlcActDst.Result == Base.PopupResult.Yes)
            {
                var vm = new SlcNodAndActViewModel(bpmnEty, _codSelectedNod,this.TblSbjOral.TblEvtRst.FldCodAct);

                vm.LblAct = vm.LblNod = "مقصد";
                vm.LblObj = "مطلب شفاهی";

                Util.ShowPopup(vm);

                if (vm.Result == PopupResult.OK)
                {
                    if (vm.SelectedAct != null)
                    {
                        TblNod nod = vm.SelectedAct.TblNod;

                        TblSbjOral objRst = this.TblSbjOral;

                        TblAct act;

                        TblEvtRst evtRst = this.TblSbjOral.TblEvtRst;

                        TblWayIfrm_Oral wayIfrm = new Model.TblWayIfrm_Oral() { FldTypIfrm = 1 };

                        List<TblWayIfrm_Oral> lst = PublicMethods.DetectWayIfrmOfSbjOral_588(this.bpmnEty, objRst);

                        foreach (TblWayIfrm_Oral item in lst)
                        {
                            PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst, item, Model.Enum.DirectionForDelete.Left);
                        }

                        int codAct = vm.SelectedAct.FldCodAct;
                        act = this.bpmnEty.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);

                        PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);

                    }
                }



                //Util.ShowPopup(SlcSrcAndDst);
                //if (SlcSrcAndDst.Result == Base.PopupResult.OK)
                //{
                //    actNotExist();
                //}


            }

            RaisePropertyChanged("TblWayIfrm_Oral", "PerformerName");
        }

        private void actNotExist()
        {

            TblNod nod = SlcSrcAndDst.SelectedItem.Nod;
            if (nod.FldCodNod == _codSelectedNod)
            {
                Util.ShowMessageBox(56);
                return;
            }

            TblSbjOral objRst = this.TblSbjOral;
            TblAct act;

            TblEvtRst evtRst = this.TblSbjOral.TblEvtRst;

            TblWayIfrm_Oral wayIfrm = new Model.TblWayIfrm_Oral() { FldTypIfrm = 1 };

            List<TblWayIfrm_Oral> lst = PublicMethods.DetectWayIfrmOfSbjOral_588(this.bpmnEty, objRst);

            foreach (TblWayIfrm_Oral item in lst)
            {
                PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst, item, Model.Enum.DirectionForDelete.Left);
            }

            int codAct = this.SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(n => n.FldActUspf).FldCodAct;
            act = this.bpmnEty.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);

            PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(this.bpmnEty, wayIfrm, objRst, act);

            return;
        }

        /// <summary>
        /// حذف رخداد های آغازگر معادل با مطلب شفاهی جاری
        /// </summary>
        /// <param name="tbl"></param>
        private void DeleteEvtSrt(List<Model.TblWayIfrm_Oral> tbl)
        {
            for (int i = 0; i < tbl.Count; i++)
            {
                if (tbl[i].TblWayAwr_Oral.TblEvtSrt.TblWayAwr_News.Count == 0 && tbl[i].TblWayAwr_Oral.TblEvtSrt.TblWayAwr_Oral.Count == 1 && tbl[i].TblWayAwr_Oral.TblEvtSrt.TblWayAwr_RecvInt.Count == 0)
                {
                    this.bpmnEty.DeleteObject(tbl[i].TblWayAwr_Oral.TblEvtSrt);
                }
            }
        }


        #endregion

    }
}
