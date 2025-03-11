using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class DtlEvtSrtViewModel : UserControlViewModel
    {
        #region ' Fields '

        private Model.TblAct tblAct;


        #endregion

        #region ' Initialaizer '

        //public DtlEvtSrtViewModel(BPMNDBEntities context)
        //    : base(context)
        //{
        //}

        public DtlEvtSrtViewModel(BPMNDBEntities context, Model.TblEvtSrt evtsrt)
            : base(context, evtsrt)
        {
            SlcActSrc = new SlcActSrcViewModel(context, justShowOralSenders: false);
            SelectActCommand = new DelegateCommand(ExecuteSelectActCommand, CanExecuteSelectActCommand);
            (SelectActCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
            //RaisePropertyChanged("TypeOfEvent");
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// رخداد آغازگر
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
            }
        }

        /// <summary>
        /// نوع رخداد آغازگر
        /// </summary>
        public TblItmFixSfw TypeOfEvent
        {
            get
            {
                switch ((Model.Enum.EvtSrtType)TblEvtSrt.FldTypEvtSrt)
                {
                    case EvtSrtType.aftrAnyCdnEvtSrt:
                        return PublicMethods.TblItmFixSfws.SingleOrDefault(E => E.FldCodSbj == 35 && E.FldCodItm == 1);
                    case EvtSrtType.spcCdnEvtSrtAftr:
                        return PublicMethods.TblItmFixSfws.SingleOrDefault(E => E.FldCodSbj == 35 && E.FldCodItm == 2);
                    case EvtSrtType.errAccurEvtSrt:
                        return PublicMethods.TblItmFixSfws.SingleOrDefault(E => E.FldCodSbj == 35 && E.FldCodItm == 3);
                    case EvtSrtType.cancelEvtSrt:
                        return PublicMethods.TblItmFixSfws.SingleOrDefault(E => E.FldCodSbj == 35 && E.FldCodItm == 4);
                    case EvtSrtType.spcCdnEvtSrt:
                        return PublicMethods.TblItmFixSfws.SingleOrDefault(E => E.FldCodSbj == 35 && E.FldCodItm == 5);
                    default:
                        break;
                }

                return null;
            }
        }

        /// <summary>
        /// بازه های زمانی
        /// </summary>
        public IEnumerable Time
        {
            get
            {
                try
                {
                    return bpmnEty.TblItmFixSfws.Where(I => I.FldCodSbj == 3).AsEnumerable();
                }
                catch (Exception e)
                {

                    //SSYM.OrgDsn.Common.ExceptionManagement.ExceptionLogger.LogException(new SSYM.OrgDsn.Base.CustomException(UserManager.CurrentUser.FldCodUsr, "Eror", e));
                    return null;
                }
            }
            set
            {
                //TblItmFixSfw tbl = (TblItmFixSfw)value;
                //this.TblEvtSrt_InnSgmtTim.FldTypDurtSchmTim = tbl.FldCodItm;
            }
        }

        /// <summary>
        /// انتخاب فعالیت مبدأ
        /// </summary>
        public ICommand SelectActCommand { get; set; }


        /// <summary>
        /// میزان تأخیر زمانی
        /// </summary>
        public int LengthOfLagTime
        {
            get
            {
                return this.TblEvtSrt.FldLgthLagTim ?? 0;
            }
            set
            {
                this.TblEvtSrt.FldLgthLagTim = value;
            }
        }

        /// <summary>
        /// نوع تأخیر زمانی
        /// </summary>
        public int TimeSelectedValue
        {
            get
            {
                return this.TblEvtSrt.FldTypLagTim ?? (int)Model.Enum.Time.Day;
            }
            set
            {
                this.TblEvtSrt.FldTypLagTim = value;
            }
        }

        /// <summary>
        /// SlcActSrcViewModel
        /// </summary>
        public SlcActSrcViewModel SlcActSrc { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcActSrc.Dispose();
        }

        #endregion

        #region ' Private Methods '

        private bool CanExecuteSelectActCommand()
        {
            if (this.TblEvtSrt.TblWayAwr_News.Count + this.TblEvtSrt.TblWayAwr_Oral.Count + this.TblEvtSrt.TblWayAwr_RecvInt.Count > 0)
            {
                return false;
            }
            return true;
        }

        private void ExecuteSelectActCommand()
        {
            this.SlcActSrc.EvtSrt = this.TblEvtSrt;
            this.SlcActSrc.ActCnt = this.TblEvtSrt.TblAct;
            this.SlcActSrc.CanUsrSlcAct = true;
            this.SlcActSrc.DestinationActivityID = this.TblEvtSrt.FldCodAct;
            this.SlcActSrc.IsSentTooMeSelected = true;

            Util.ShowPopup(SlcActSrc);

            if (this.SlcActSrc.Result == PopupResult.OK)
            {
                if (this.SlcActSrc.IsActsOfNodCntSelected)
                {
                    this.TblEvtSrt.PreviousActivity = this.bpmnEty.TblActs.Single(m => m.FldCodAct == this.SlcActSrc.ActOfNodCntSelectedItem.FldCodAct);
                }
                else
                {
                    this.TblEvtSrt.PreviousActivity = this.bpmnEty.TblActs.Single(m => m.FldCodAct == this.SlcActSrc.SentToMeObjectsSelectedItem.Item1.ActSrc.FldCodAct);
                }
            }

            if (this.SlcActSrc.Result == PopupResult.Yes)
            {
                SlcNodAndActViewModel SlcNodAndActVM = new SlcNodAndActViewModel(this.bpmnEty, codAct: this.TblEvtSrt.FldCodAct, actUspfEnabled: false);

                SlcNodAndActVM.IsDepOrgVisible = SlcNodAndActVM.IsOutOrgVisible = false;

                Util.ShowPopup(SlcNodAndActVM);

                if (SlcNodAndActVM.Result == PopupResult.OK)
                {
                    this.TblEvtSrt.PreviousActivity = bpmnEty.TblActs.Single(a => a.FldCodAct == SlcNodAndActVM.SelectedAct.FldCodAct);
                }
            }

            RaisePropertyChanged("TblEvtSrt");

        }

        #endregion

        #region ' events '

        #endregion

    }
}
