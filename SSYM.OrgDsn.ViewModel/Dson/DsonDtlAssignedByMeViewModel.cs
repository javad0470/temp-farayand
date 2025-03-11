using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class DsonDtlAssignedByMeViewModel : DsonDtlViewModel
    {

        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_dsonObj">ناهمسانی که من نسبت داده ام</param>
        /// <param name="nod">نود مر بوط به ناهمسانی که من نسبت داده ام</param>
        /// <param name="selectedPosPst">جایگاه یا سمت که ناهمسانی از آن نسبت داده شده یا چایگاه من</param>
        public DsonDtlAssignedByMeViewModel(BPMNDBEntities context, IWayAwrIfrm _dsonObj, TblNod nod, TblNod selectedPosPst)
            : base(context, _dsonObj, nod, selectedPosPst)
        {
            SaveEnabled = false;
            SlcSrcAndDstVM = new SlcSrcAndDstViewModel();
            SelectSrcCommand = new DelegateCommand(SlcSrcExecute, CanOK);
            SelectDestCommand = new DelegateCommand(SlcDestExecute, CanOK);
        }


        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// فعالیت مربوط به ایجاد کننده ناهمسانی
        /// </summary>
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

        public override string AcceptRdbCnt
        {
            get
            {
                string str = "{0}  را به اشتباه به {1}این جایگاه/سمت نسبت داده ام و {2} آن جایگاه دیگری است.";
                string str0 = "";
                string str1 = IsActSpec ? "این فعالیت " : "";
                string str2 = this.InputVisibility == System.Windows.Visibility.Visible ? "مبدا" : "مقصد";

                str0 = GetDsonString();

                str = string.Format(str, str0, str1, str2);

                return str;
            }
        }

        public override string RejectRdbCnt
        {
            get
            {
                string str = "همچنان بر این نظرم که {0} {1} {2}این جایگاه/سمت است.";

                string str1 = "";
                string str2 = IsActSpec ? "همین فعالیت " : "";
                string str0 = this.InputVisibility == System.Windows.Visibility.Visible ? "مبدا" : "مقصد";
                str0 = GetDsonString();
                str = string.Format(str, str0, str1, str2);
                return str;

            }
        }

        public override string FrstFtrRgnText
        {
            get
            {
                if (this.InputVisibility == Visibility.Visible)
                {
                    return string.Format("مبدا مناسب برای {0} دریافتی انتخاب کنید.", WayAwrIfrm.Title);
                }
                else
                {
                    return string.Format("{0} ارسالی را به مقصد مناسب ارسال کنید", WayAwrIfrm.Title);
                }
            }
        }

        public SingleWayAwrViewModel WayAwrVM
        {
            get
            {
                if (WayAwr == null)
                {
                    return null;
                }
                return new SingleWayAwrViewModel(WayAwrIfrm.ActDst, WayAwr);
            }
        }

        public SingleWayIfrmViewModel WayIfrmVM
        {
            get
            {
                if (WayIfrm == null)
                {
                    return null;
                }

                return new SingleWayIfrmViewModel(this.WayAwrIfrm.ActSrc, WayIfrm);
            }
        }

        public TblAct ActSrc
        {
            get
            {
                if (this.InputVisibility == Visibility.Collapsed)
                {
                    return this.WayAwrIfrm.ActSrc;
                }
                else
                {
                    if (NewActSrcOrDst != null)
                    {
                        return NewActSrcOrDst;
                    }
                    else
                    {
                        return this.WayAwrIfrm.ActSrc;
                    }
                }
            }
        }

        public TblAct ActDst
        {
            get
            {
                if (this.OutputVisibility == Visibility.Collapsed)
                {
                    return this.WayAwrIfrm.ActDst;
                }
                else
                {
                    if (NewActSrcOrDst != null)
                    {
                        return NewActSrcOrDst;
                    }
                    else
                    {
                        return this.WayAwrIfrm.ActDst;
                    }
                }
            }
        }

        public ICommand SelectSrcCommand { get; set; }

        public ICommand SelectDestCommand { get; set; }

        /// <summary>
        /// فعالیت جدیدی که کاربر جایگزین قبلی کرده است
        /// </summary>
        /// 
        TblAct newActSrcOrDst;
        public TblAct NewActSrcOrDst
        {
            get
            {
                return newActSrcOrDst;
            }
            set
            {
                newActSrcOrDst = value;
                SaveEnabled = newActSrcOrDst != null;
                RaisePropertyChanged("ActDst", "ActSrc", "SaveEnabled");
            }
        }

        ///// <summary>
        ///// نشان دهنده باز بودن پاپا انتخاب مبداء و مقصد
        ///// </summary>
        //bool isSlcSrcAndDstOpen = false;
        //public bool IsSlcSrcAndDstOpen
        //{
        //    get
        //    {
        //        return isSlcSrcAndDstOpen;
        //    }
        //    set
        //    {
        //        isSlcSrcAndDstOpen = value;
        //        RaisePropertyChanged("IsSlcSrcAndDstOpen");

        //        if (!isSlcSrcAndDstOpen)
        //        {
        //        }
        //    }
        //}


        /// <summary>
        /// پاپاپی که برای انتخاب مبدا و مقصد باز میشود.
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDstVM { get; set; }

        public SlcNodAndActViewModel SlcNodAndActVM { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        private void SlcSrcExecute()
        {

            bool canSelectUspcf = false;

            canSelectUspcf = (WayAwrIfrm as IWayAwr).EvtSrt_Temp.TypSrt == EvtSrtType.aftrAwareEvtSrt;

            if (canSelectUspcf)
            {

                if (WayAwrIfrm is TblWayAwr_Oral) // اگر نحوه آگاهی رخداد آغازگر صرفا پس از آگاهی از نوع مطلب شفاهی باشد، تنها فعالیت نامشخص یک جایگاه باید انتخاب شود
                {
                    Util.ShowPopup(SlcSrcAndDstVM);

                    if (SlcSrcAndDstVM.Result == Base.PopupResult.OK)
                    {
                        if (SlcSrcAndDstVM.IsInsideOrganization)
                        {
                            if (SlcSrcAndDstVM.SelectedItem == null)
                            {
                                return;
                            }

                            //امکان انتخاب جایگاه جاری برای رخداد آغازگر صرفا پس از آگاهی وجود ندارد.
                            if (SlcSrcAndDstVM.SelectedItem.Nod.FldCodNod == this.SelectedNode.FldCodNod)
                            {
                                Util.ShowMessageBox(74);
                                return;
                            }

                            //امکان انتخاب مجدد مجری که با آن ناهمسانی داشته‏اید وجود ندارد. لطفا یک مجری دیگر انتخاب نمایید
                            if (SlcSrcAndDstVM.SelectedItem.Nod.FldCodNod == this.Node.FldCodNod)
                            {
                                Util.ShowMessageBox(45);
                                return;
                            }

                            int codAct = SlcSrcAndDstVM.SelectedItem.Nod.TblActs.FirstOrDefault(n => n.FldActUspf).FldCodAct;
                            NewActSrcOrDst = this.context.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);
                        }
                        else
                        {
                            if (SlcSrcAndDstVM.SelectedItem == null)
                            {
                                return;
                            }
                            NewActSrcOrDst = this.context.TblActs.Single(m => m.FldCodAct == SlcSrcAndDstVM.SelectedItem.Nod.TblActs.Single(n => n.FldActUspf).FldCodAct);
                        }
                    }

                    return;
                }
            }

            SlcNodAndActVM = new SlcNodAndActViewModel(context, codSelectedNod: this.Node.FldCodNod, codAct: WayAwrIfrm.ActDst.FldCodAct, actUspfEnabled: canSelectUspcf);

            SlcNodAndActVM.IsDepOrgVisible = SlcNodAndActVM.IsOutOrgVisible = canSelectUspcf;

            Util.ShowPopup(SlcNodAndActVM);

            if (SlcNodAndActVM.Result == Base.PopupResult.OK)
            {
                if (SlcNodAndActVM.SelectedAct.FldCodAct == WayAwrIfrm.ActSrc.FldCodAct)
                {
                    Util.ShowMessageBox(72);
                    return;
                }
                NewActSrcOrDst = context.TblActs.Single(a => a.FldCodAct == SlcNodAndActVM.SelectedAct.FldCodAct);
            }


        }

        private void SlcDestExecute()
        {
            SlcNodAndActVM = new SlcNodAndActViewModel(context, codSelectedNod: this.Node.FldCodNod, codAct: WayAwrIfrm.ActSrc.FldCodAct);

            Util.ShowPopup(SlcNodAndActVM);

            if (SlcNodAndActVM.Result == Base.PopupResult.OK)
            {
                if (SlcNodAndActVM.SelectedAct.FldCodAct == WayAwrIfrm.ActDst.FldCodAct)
                {
                    Util.ShowMessageBox(72);
                    return;
                }
                NewActSrcOrDst = this.context.TblActs.Single(a => a.FldCodAct == SlcNodAndActVM.SelectedAct.FldCodAct);
            }



            //Util.ShowPopup(SlcSrcAndDstVM);

            //if (SlcSrcAndDstVM.Result == Base.PopupResult.OK)
            //{
            //    if (SlcSrcAndDstVM.IsInsideOrganization)
            //    {
            //        if (SlcSrcAndDstVM.SelectedItem == null)
            //        {
            //            return;
            //        }

            //        if (SlcSrcAndDstVM.SelectedItem.Nod.FldCodNod == this.Node.FldCodEty)
            //        {
            //            Util.ShowMessageBox(45);
            //            return;
            //        }

            //        int codAct = SlcSrcAndDstVM.SelectedItem.Nod.TblActs.FirstOrDefault(n => n.FldActUspf).FldCodAct;
            //        NewActSrcOrDst = this.context.TblActs.FirstOrDefault(m => m.FldCodAct == codAct);
            //    }
            //    else
            //    {
            //        if (SlcSrcAndDstVM.SelectedItem == null)
            //        {
            //            return;
            //        }
            //        NewActSrcOrDst = this.context.TblActs.Single(m => m.FldCodAct == SlcSrcAndDstVM.SelectedItem.Nod.TblActs.Single(n => n.FldActUspf).FldCodAct);
            //    }
            //}
        }

        private bool CanOK()
        {
            return true;
        }

        private string GetDsonString()
        {
            string str0 = "";
            switch (TypDsonCur)
            {
                case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToSpcf:
                    str0 = "خروجی";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToSpcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToUnspcf:
                    str0 = "مطلب شفاهی ارسالی";
                    break;

                case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromSpcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromUnspcf:
                    str0 = "خبر دریافتی";
                    break;

                case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromSpcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromUnspcf:
                    str0 = "ورودی";
                    break;

                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromSpcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromUnspcf:
                    str0 = "مطلب شفاهی دریافتی";
                    break;
            }
            return str0;
        }

        protected override void SaveExecute()
        {
            if (!CanSettleDson())
            {
                return;
            }



            if (Util.ShowMessageBox(73) == System.Windows.MessageBoxResult.Yes)
            {
                if (NewActSrcOrDst == null)
                {
                    return;
                }

                if (WayAwr != null)
                {
                    IObjRst objRst = CopyObjRst(WayAwr.ObjRst);

                    TblEvtSrt srt = WayAwr.EvtSrt_Temp;

                    //حذف نحوه آگاهی و به تبع آن در صورت نیاز حذف شی نتیجه ای که به آن وارد شده
                    PublicMethods.ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, WayAwr.EvtSrt_Temp, WayAwr);

                    IWayAwr wayAwr = CreateWayAwrByObjRst(objRst);

                    dynamic d = wayAwr;

                    d.TblEvtSrt = srt;

                    PublicMethods.AddNewObjRstToWayAwrAndChgPrs_6724(context, objRst, wayAwr, NewActSrcOrDst);

                    PublicMethods.AddDsonByWayAwrInfrm_19072(this.context, this.GetPeerStateForRejectedDson(WayAwr.DsonType), wayAwr);
                }
                else if (WayIfrm != null)
                {
                    // نحوه آگاه سازی قبلی که به فعالیت قبلی میرفته پاک شده و 
                    // نحوه آگاه سازی جدیدی برای فعالیت جاری با مقصد فعالیت جدید ایجاد میشود

                    IObjRst objRst = WayIfrm.ObjRst;

                    PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(context, WayIfrm.ObjRst, WayIfrm, Model.Enum.DirectionForDelete.Left);

                    IWayIfrm wayIfrm = CreateWayIfrmByObjRst(objRst);

                    PublicMethods.AddWayIfrmToObjRstAimAtActAndChgPrs_3435(context, wayIfrm, objRst, NewActSrcOrDst);

                    PublicMethods.AddDsonByWayAwrInfrm_19072(this.context, this.GetPeerStateForRejectedDson(WayIfrm.DsonType), wayIfrm);
                }

                result = MessageBoxResult.Yes;
                base.SaveExecute();
            }
            else
            {
                base.SaveExecute();
            }
        }

        private IObjRst CopyObjRst(IObjRst objRst)
        {
            dynamic d = objRst;
            if (objRst is TblNew)
            {
                TblNew news = new TblNew()
                {
                    FldCodEvtPrdr = d.FldCodEvtPrdr,
                    FldTtlNews = d.FldTtlNews,
                    FldTxtNews = d.FldTxtNews
                };

                return news;
            }

            else if (objRst is TblSbjOral)
            {
                TblSbjOral oral = new TblSbjOral() { FldCodEvtPrdr = d.FldCodEvtPrdr };
                return oral;
            }

            else if (objRst is TblObj)
            {
                TblObj obj = new TblObj()
                {
                    FldCodEvtPrdr = d.FldCodEvtPrdr,
                    FldNamObj = d.FldNamObj,
                    FldTypObj = d.FldTypObj
                };
                return obj;
            }

            return null;
        }


        private IWayAwr CreateWayAwrByObjRst(IObjRst objRst)
        {
            if (objRst is TblNew)
            {
                TblWayAwr_News news = new TblWayAwr_News();
                return news;
            }

            else if (objRst is TblSbjOral)
            {
                TblWayAwr_Oral oral = new TblWayAwr_Oral() { FldTypAwr = 1 };
                return oral;
            }

            else if (objRst is TblObj)//TblWayAwr_RecvInt
            {
                Model.TblWayAwr_RecvInt tbl = new Model.TblWayAwr_RecvInt()
                {
                    FldWayRecv = (int)Model.Enum.ManualOrSoftware.Manual,
                    FldCodCmrIntPerRecv = (int)Model.Enum.Comparers.EqualTo,
                    FldTnoIntPerRecv = 1,
                    FldCodUntMsrtInt = 0,
                    FldIntNeedPrsg = 1
                };

                return tbl;
            }

            return null;
        }


        private IWayIfrm CreateWayIfrmByObjRst(IObjRst objRst)
        {
            if (objRst is TblNew)
            {
                TblWayIfrm_News news = new TblWayIfrm_News();
                return news;
            }

            else if (objRst is TblSbjOral)
            {
                TblWayIfrm_Oral oral = new TblWayIfrm_Oral() { FldTypIfrm = 1 };
                return oral;
            }

            else if (objRst is TblObj)
            {
                Model.TblWayIfrm_SndOut tbl = new Model.TblWayIfrm_SndOut()
                {
                    FldWaySnd = (int)Model.Enum.ManualOrSoftware.Manual,
                    FldCodCmrOutPerSnd = (int)Model.Enum.Comparers.EqualTo,
                    FldTnoOutPerSnd = 1,
                    FldCodUntMsrtOut = 0
                };

                return tbl;
            }

            return null;
        }

        #endregion

        #region ' Events '

        #endregion

    }
}
