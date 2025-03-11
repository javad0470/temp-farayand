using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Lanes;
using MindFusion.Layout;
using SSYM.OrgDsn.Model.BPMNShapes;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SSYM.OrgDsn.Model.Base;
using System.Collections.ObjectModel;
using yWorks.yFiles.UI.Model;
using yWorks.yFiles.UI;
using SSYM.OrgDsn.Model.BPMNDgm.Model;
using SSYM.OrgDsn.Model.BPMNDgm.Styles;
using System.Windows.Documents;
using System.Data.Objects;

namespace SSYM.OrgDsn.Model
{
    public partial class PublicMethods
    {
        static TblUsr currentUser;

        /// <summary>
        /// 
        /// </summary>
        public static List<TblItmAc> AllItmAcs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void DetectAllItmAcs(BPMNDBEntities context)
        {
            //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, context.TblItmAcs);
            AllItmAcs = context.TblItmAcs.ToList();
        }

        public static void DeleteAllWayIfrmWotWayAwr(BPMNDBEntities context)
        {
            List<IWayIfrm> wayIfrm = new List<IWayIfrm>();

            foreach (IWayIfrm item in context.TblWayIfrm_News.Where(m => m.TblWayAwr_News == null))
            {
                wayIfrm.Add(item);
            }

            foreach (IWayIfrm item in context.TblWayIfrm_Oral.Where(m => m.TblWayAwr_Oral == null))
            {
                wayIfrm.Add(item);
            }

            foreach (IWayIfrm item in context.TblWayIfrm_SndOut.Where(m => m.TblWayAwr_RecvInt == null))
            {
                wayIfrm.Add(item);
            }

            foreach (IWayIfrm item in wayIfrm)
            {
                if (item.WayAwr == null)
                {
                    context.DeleteObject(item);
                }
            }

            SaveContext(context);
        }

        public static TblUsr CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        /// <summary>
        /// یک لیست از تمامی ایتم های فیکس نرم افزار برمیگرداند
        /// این لیست در زمان اجرای نرم افزار پر میشود
        /// </summary>
        public static List<TblItmFixSfw> TblItmFixSfws;

        public static Dictionary<int, string> TblHelpDynm;

        /// <summary>
        /// یک لیست از تمامی پیام های نرم افزار برمیگرداند
        /// این لیست در زمان اجرای نرم افزار پر میشود
        /// </summary>
        public static List<TblMsg> TblMsgs;

        /// <summary>
        /// return the name of the performer of activity
        /// </summary>
        /// <param name="codAct">activity code</param>
        /// <returns></returns>
        public static string ActivityPerformerName_951(int codAct)
        {
            if (codAct != 0)
            {
                using (Model.BPMNDBEntities context = new BPMNDBEntities())
                {
                    int codNod = context.TblActs.SingleOrDefault(E2 => E2.FldCodAct == codAct).FldCodNod;
                    int codTypEty = context.TblNods.SingleOrDefault(E => E.FldCodNod == codNod).FldCodTypEty;
                    int codEty = context.TblNods.SingleOrDefault(E => E.FldCodNod == codNod).FldCodEty;
                    switch (codTypEty)
                    {
                        case 1:
                            return context.TblOrgs.SingleOrDefault(E => E.FldCodOrg == codEty).FldNamOrg;
                        case 2:
                            return context.TblPosPstOrgs.SingleOrDefault(E => E.FldCodPosPst == codEty).FldNamPosPst;
                        case 3:
                            return context.TblPsns.SingleOrDefault(E => E.FldCodPsn == codEty).FldNam1stPsn + " " + context.TblPsns.SingleOrDefault(E => E.FldCodPsn == codEty).FldNam2ndPsn;
                        case 4:
                            return context.TblRols.SingleOrDefault(E => E.FldCodRol == codEty).FldTtlRol;
                        default:
                            return "";
                    }
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// returns the name of a node based on its type and code
        /// </summary>
        /// <param name="codEty">entity code</param>
        /// <param name="codTypEty">entity type</param>
        /// <returns></returns>
        public static string PerformerName_950(int codEty, int codTypEty)
        {
            if (codEty != 0)
            {
                using (Model.BPMNDBEntities context = new BPMNDBEntities())
                {
                    switch (codTypEty)
                    {
                        case 1:
                            return context.TblOrgs.SingleOrDefault(E => E.FldCodOrg == codEty).FldNamOrg;
                        case 2:
                            return context.TblPosPstOrgs.SingleOrDefault(E => E.FldCodPosPst == codEty).FldNamPosPst;
                        case 3:
                            return context.TblPsns.SingleOrDefault(E => E.FldCodPsn == codEty).FldNam1stPsn + " " + context.TblPsns.SingleOrDefault(E => E.FldCodPsn == codEty).FldNam2ndPsn;
                        case 4:
                            return context.TblRols.SingleOrDefault(E => E.FldCodRol == codEty).FldTtlRol;
                        default:
                            return "";

                    }
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// returns the title of a node that is pstpos
        /// </summary>
        /// <param name="codEty">entity code</param>
        /// <param name="codTypEty">entity type</param>
        /// <returns></returns>
        public static string PstPosNodTitle(int codEty, int codTypEty)
        {
            if (codEty != 0)
            {
                using (Model.BPMNDBEntities context = new BPMNDBEntities())
                {
                    switch (codTypEty)
                    {
                        case 2:
                            TblPosPstOrg pospst = context.TblPosPstOrgs.SingleOrDefault(E => E.FldCodPosPst == codEty);
                            switch ((Enum.PosPst)pospst.FldCodTyp)
                            {
                                case PosPst.Pos:
                                    return "جایگاه سازمانی";

                                case PosPst.Pst:
                                    return "سمت سازمانی";

                                default:
                                    return "";
                            }
                        default:
                            return "";
                    }
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// F594
        /// رخداد آغازگر معادل با رخداد نتیحه جاری را می دهد
        /// </summary>
        /// <param name="evtRstType">نوع رخداد نتیجه</param>
        /// <returns></returns>
        public static Enum.EvtSrtType DetectTypEvtSrtEqualToTypEvtRst_594(int evtRstType)
        {
            switch (evtRstType)
            {
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.anyCdnEvtRst:
                    return Enum.EvtSrtType.aftrAnyCdnEvtSrt;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.spcCdnEvtRstAftr:
                    return Enum.EvtSrtType.spcCdnEvtSrtAftr;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.getNewAwrAftrAct:
                    return Enum.EvtSrtType.aftrAnyCdnEvtSrt;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.errAccurEvtRst:
                    return Enum.EvtSrtType.errAccurEvtSrt;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.cancelEvtRst:
                    return Enum.EvtSrtType.cancelEvtSrt;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.spcCdnEvtRstInnTim:
                    return Enum.EvtSrtType.spcCdnEvtSrt;
                case (int)SSYM.OrgDsn.Model.Enum.EvtRstType.getNewAwr:
                    return Enum.EvtSrtType.aftrAnyCdnEvtSrt;
                default:
                    return Enum.EvtSrtType.aftrAnyCdnEvtSrt;
            }
        }

        /// <summary>
        /// شناسایی نوع رخداد نتیجه معادل یک نوع رخداد آغازگر 
        /// </summary>
        /// <param name="evtSrtTyp">نوع رخداد آغازگر</param>
        /// <returns></returns>
        public static Enum.EvtRstType DetectTypEvtRstEqualToTypEvtSrt_508(int evtSrtTyp)
        {
            switch (evtSrtTyp)
            {
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.aftrAwareEvtSrt:
                    return Enum.EvtRstType.anyCdnEvtRst;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.aftrAnyCdnEvtSrt:
                    return Enum.EvtRstType.anyCdnEvtRst;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.spcCdnEvtSrtAftr:
                    return Enum.EvtRstType.spcCdnEvtRstAftr;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.errAccurEvtSrt:
                    return Enum.EvtRstType.errAccurEvtRst;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.cancelEvtSrt:
                    return Enum.EvtRstType.cancelEvtRst;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.spcCdnEvtSrt:
                    return Enum.EvtRstType.spcCdnEvtRstInnTim;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.getNewAwrAftr:
                    return Enum.EvtRstType.anyCdnEvtRst;
                case (int)SSYM.OrgDsn.Model.Enum.EvtSrtType.getNewAwrInnTim:
                    return Enum.EvtRstType.anyCdnEvtRst;
                default:
                    return Enum.EvtRstType.anyCdnEvtRst;
            }
        }

        /// <summary>
        /// حذف فعالیت های یک نود
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="nod">گرهی که می خواهیم فعالیت های  آن را حذف نماییم</param>
        /// <param name="deleteUspfAct">آیا فعالیت نامشخص نود حذف شود</param>
        public static void DeleteActOfNod(BPMNDBEntities context, TblNod nod, bool deleteUspfAct)
        {
            List<TblAct> lstTblAct;
            if (deleteUspfAct)
            {
                lstTblAct = nod.TblActs.ToList();
            }
            else
            {
                lstTblAct = nod.TblActs.Where(E => !E.FldActUspf).ToList();
            }
            foreach (TblAct item in lstTblAct)
            {
                List<TblEvtSrt> lstEvtSrt = item.TblEvtSrts.ToList();
                foreach (TblEvtSrt item2 in lstEvtSrt)
                {
                    context.DeleteObject(item2);
                }
                context.DeleteObject(item);
            }
        }

        /// <summary>
        /// Pdr9019
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblNew(BPMNDBEntities context, TblNew deletingObj)
        {
            foreach (TblWayIfrm_News item in deletingObj.TblWayIfrm_News)
            {
                TblEvtSrt srt = item.TblWayAwr_News.TblEvtSrt;
                if (srt.TblEvt_GainAwrNew.Count == 0 && srt.TblWayAwr_News.Count == 1 && srt.TblWayAwr_Oral.Count == 0 && srt.TblWayAwr_RecvInt.Count == 0)
                {
                    context.DeleteObject(srt);
                }
            }

            context.DeleteObject(deletingObj);
        }

        /// <summary>
        /// Pdr9018
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblSbjOral(BPMNDBEntities context, TblSbjOral deletingObj)
        {
            foreach (TblWayIfrm_Oral item in deletingObj.TblWayIfrm_Oral)
            {
                TblEvtSrt srt = item.TblWayAwr_Oral.TblEvtSrt;
                if (srt.TblEvt_GainAwrNew.Count == 0 && srt.TblWayAwr_News.Count == 0 && srt.TblWayAwr_Oral.Count == 1 && srt.TblWayAwr_RecvInt.Count == 0)
                {
                    context.DeleteObject(srt);
                }
            }

            context.DeleteObject(deletingObj);
        }

        /// <summary>
        /// Pdr9020
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblObj(BPMNDBEntities context, TblObj deletingObj)
        {
            foreach (TblWayIfrm_SndOut item in deletingObj.TblWayIfrm_SndOut)
            {
                TblEvtSrt srt = item.TblWayAwr_RecvInt.TblEvtSrt;
                if (srt.TblEvt_GainAwrNew.Count == 0 && srt.TblWayAwr_News.Count == 0 && srt.TblWayAwr_Oral.Count == 0 && srt.TblWayAwr_RecvInt.Count == 1)
                {
                    context.DeleteObject(srt);
                }
            }
            context.DeleteObject(deletingObj);
        }

        /// <summary>
        /// Pdr9013
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayAwr_RecvInt(BPMNDBEntities context, TblWayAwr_RecvInt deletingObj)
        {
            if (deletingObj.TblWayIfrm_SndOut != null && deletingObj.TblWayIfrm_SndOut.TblObj != null)
            {
                if (deletingObj.TblWayIfrm_SndOut.TblObj.TblWayIfrm_SndOut.Count > 1)
                {
                    context.DeleteObject(deletingObj.TblWayIfrm_SndOut);
                }
                else
                {
                    TblObj obj = deletingObj.TblWayIfrm_SndOut.TblObj;
                    if (obj.TblEvtRst.TblObjs.Count == 1 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(deletingObj.TblWayIfrm_SndOut.TblObj);
                    }
                }
            }

            System.Data.Objects.ObjectStateEntry entry;
            if (context.ObjectStateManager.TryGetObjectStateEntry(deletingObj, out entry))
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// Pdr9015
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayAwr_News(BPMNDBEntities context, TblWayAwr_News deletingObj)
        {
            if (deletingObj.TblWayIfrm_News != null && deletingObj.TblWayIfrm_News.TblNew != null)
            {
                if (deletingObj.TblWayIfrm_News.TblNew.TblWayIfrm_News.Count > 1)
                {
                    context.DeleteObject(deletingObj.TblWayIfrm_News);
                }
                else
                {

                    TblNew obj = deletingObj.TblWayIfrm_News.TblNew;
                    if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 1 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(deletingObj.TblWayIfrm_News.TblNew);
                    }
                }
            }
            System.Data.Objects.ObjectStateEntry entry;
            if (context.ObjectStateManager.TryGetObjectStateEntry(deletingObj, out entry))
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// Pdr9014
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayAwr_Oral(BPMNDBEntities context, TblWayAwr_Oral deletingObj)
        {
            if (deletingObj.TblWayIfrm_Oral != null && deletingObj.TblWayIfrm_Oral.TblSbjOral != null)
            {
                if (deletingObj.TblWayIfrm_Oral.TblSbjOral.TblWayIfrm_Oral.Count > 1)
                {
                    context.DeleteObject(deletingObj.TblWayIfrm_Oral);
                }
                else
                {


                    TblSbjOral obj = deletingObj.TblWayIfrm_Oral.TblSbjOral;
                    if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 1)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(deletingObj.TblWayIfrm_Oral.TblSbjOral);
                    }
                }
            }
            System.Data.Objects.ObjectStateEntry entry;
            if (context.ObjectStateManager.TryGetObjectStateEntry(deletingObj, out entry))
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// حذف یک نحوه آگاه سازی با ارسال خروجی
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayIfrm_SndOut(BPMNDBEntities context, TblWayIfrm_SndOut deletingObj)
        {
            if (deletingObj.TblObj != null)
            {
                if (deletingObj.TblObj.TblWayIfrm_SndOut.Count > 1)
                {
                    context.DeleteObject(deletingObj);
                }
                else
                {
                    TblObj obj = deletingObj.TblObj;
                    if (obj.TblEvtRst.TblObjs.Count == 1 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(obj);
                    }
                }
            }

            else
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// حذف یک نحوه آگاه سازی با اعلان خبر
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayIfrm_News(BPMNDBEntities context, TblWayIfrm_News deletingObj)
        {
            if (deletingObj.TblNew != null)
            {
                if (deletingObj.TblNew.TblWayIfrm_News.Count > 1)
                {
                    context.DeleteObject(deletingObj);
                }
                else
                {
                    TblNew obj = deletingObj.TblNew;
                    if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 1 && obj.TblEvtRst.TblSbjOrals.Count == 0)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(obj);
                    }
                }
            }
            else
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// حذف یک نحوه آگاه سازی شفاهی
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteTblWayIfrm_Oral(BPMNDBEntities context, TblWayIfrm_Oral deletingObj)
        {
            if (deletingObj.TblSbjOral != null)
            {
                if (deletingObj.TblSbjOral.TblWayIfrm_Oral.Count > 1)
                {
                    context.DeleteObject(deletingObj);
                }
                else
                {


                    TblSbjOral obj = deletingObj.TblSbjOral;
                    if (obj.TblEvtRst.TblObjs.Count == 0 && obj.TblEvtRst.TblNews.Count == 0 && obj.TblEvtRst.TblSbjOrals.Count == 1)
                    {
                        context.DeleteObject(obj.TblEvtRst);
                    }
                    else
                    {
                        context.DeleteObject(obj);
                    }
                }
            }
            else
            {
                context.DeleteObject(deletingObj);
            }
        }

        /// <summary>
        /// Pdr9012
        /// </summary>
        /// <param name="evtSrt"></param>
        private static void DeleteEvtSrtWthAllRelations_808(BPMNDBEntities context, TblEvtSrt evtSrt, bool stsExsSrcPelUpl = false)
        {
            List<TblWayAwr_News> wayAwr1 = DetectWayAwrOfEvtSrtOfTypNews_485(context, evtSrt);

            foreach (TblWayAwr_News wayAwr1_i in wayAwr1)
            {
                //DeleteWayAwrNews_810(context, wayAwr1_i);
                ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, evtSrt, wayAwr1_i);
            }

            List<TblWayAwr_Oral> wayAwr2 = DetectWayAwrOfEvtSrtOfTypSbjOral_486(context, evtSrt);

            foreach (TblWayAwr_Oral wayAwr2_i in wayAwr2)
            {
                //DeleteWayAwrSbjOral_819(context, wayAwr2_i);
                ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, evtSrt, wayAwr2_i, stsExsSrcPelUpl);
            }

            List<TblWayAwr_RecvInt> wayAwr3 = DetectWayAwrOfEvtSrtOfTypObj_487(context, evtSrt);

            foreach (TblWayAwr_RecvInt wayAwr3_i in wayAwr3)
            {
                //DeleteWayAwrObj_820(context, wayAwr3_i);
                ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, evtSrt, wayAwr3_i, stsExsSrcPelUpl);
            }

            context.DeleteObject(evtSrt);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayAwr"></param>
        public static void DeleteWayAwrNews_810(BPMNDBEntities context, TblWayAwr_News wayAwr)
        {
            TblNew news = wayAwr.TblWayIfrm_News.TblNew;

            List<TblWayIfrm_News> wayIfrm = DetectWayIfrmOfNews_587(context, news);

            if (wayIfrm.Count > 1)
            {
                context.DeleteObject(wayAwr.TblWayIfrm_News);
            }

            else if (wayIfrm.Count == 1)
            {
                TblEvtRst evtRst = news.TblEvtRst;

                List<IObjRst> objRst = DetectObjRstOfEvtRst_572(context, evtRst);

                if (objRst.Count > 1)
                {
                    context.DeleteObject(news);
                }

                else if (objRst.Count == 1)
                {
                    context.DeleteObject(evtRst);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayAwr"></param>
        public static void DeleteWayAwrSbjOral_819(BPMNDBEntities context, TblWayAwr_Oral wayAwr)
        {
            TblSbjOral sbjOral = wayAwr.TblWayIfrm_Oral.TblSbjOral;

            TblEvtRst evtRst = sbjOral.TblEvtRst;

            List<IObjRst> objRst = DetectObjRstOfEvtRst_572(context, evtRst);

            if (objRst.Count > 1)
            {
                context.DeleteObject(sbjOral);
            }

            else if (objRst.Count == 1)
            {
                context.DeleteObject(evtRst);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayAwr"></param>
        public static void DeleteWayAwrObj_820(BPMNDBEntities context, TblWayAwr_RecvInt wayAwr)
        {
            TblObj obj = wayAwr.TblWayIfrm_SndOut.TblObj;

            List<TblWayIfrm_SndOut> wayIfrm = DetectWayIfrmObj_589(context, obj);

            if (wayIfrm.Count > 1)
            {
                context.DeleteObject(wayAwr.TblWayIfrm_SndOut);
            }

            else if (wayIfrm.Count == 1)
            {
                TblEvtRst evtRst = obj.TblEvtRst;

                List<IObjRst> objRst = DetectObjRstOfEvtRst_572(context, evtRst);

                if (objRst.Count > 1)
                {
                    context.DeleteObject(objRst);
                }

                else
                {
                    context.DeleteObject(evtRst);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<TblWayIfrm_SndOut> DetectWayIfrmObj_589(BPMNDBEntities context, TblObj obj)
        {
            ReloadEntity(context, obj, obj.TblWayIfrm_SndOut, "TblWayIfrm_SndOut");

            return obj.TblWayIfrm_SndOut.ToList();
        }

        /// <summary>
        /// برای به روز رسانی اطلاعات حافظه مبتنی بر اطلاعات دیتا بیس مورد استفاده قرار می گیرد
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="ety">entity</param>
        /// <param name="collection">collection of entity</param>
        /// <param name="collectionName">name of collection</param>
        public static void ReloadEntity(BPMNDBEntities context, EntityObject ety, IEnumerable collection, string collectionName)
        {
            //SaveContext(context);

            //try
            //{
            //    SaveContext(context);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, collection);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    context.LoadProperty(ety, collectionName);
            //}
            //catch (Exception)
            //{
            //}
        }

        public static void ReloadEntity(BPMNDBEntities context, EntityObject ety)
        {
            //SaveContext(context);

            //try
            //{
            //    SaveContext(context);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, ety);
            //}
            //catch (Exception)
            //{
            //}
        }

        public static void ReloadEntity(BPMNDBEntities context, IEnumerable collection)
        {
            //SaveContext(context);

            //try
            //{
            //    SaveContext(context);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, collection);
            //}
            //catch (Exception)
            //{
            //}
        }

        /// <summary>
        /// برای به روز رسانی اطلاعات حافظه مبتنی بر اطلاعات دیتا بیس مورد استفاده قرار می گیرد_ بر خلاف متد قبلی کانتکست ذخیره نمی شود
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="ety">entity</param>
        /// <param name="collection">collection of entity</param>
        /// <param name="collectionName">name of collection</param>
        public static void ReloadEntityWotSave(BPMNDBEntities context, EntityObject ety, IEnumerable collection, string collectionName)
        {
            //SaveContext(context);

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, collection);
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    context.LoadProperty(ety, collectionName);
            //}
            //catch (Exception)
            //{
            //}
        }

        public static void ReloadEntityWotSave(BPMNDBEntities context, EntityObject ety)
        {
            //SaveContext(context);

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, ety);
            //}
            //catch (Exception)
            //{
            //}
        }

        public static void ReloadEntityWotSave(BPMNDBEntities context, IEnumerable collection)
        {
            //SaveContext(context);

            //try
            //{
            //    context.Refresh(System.Data.Objects.RefreshMode.StoreWins, collection);
            //}
            //catch (Exception)
            //{
            //}
        }

        /// <summary>
        /// Pdr9016
        /// </summary>
        /// <param name="deletingObj"></param>
        public static void DeleteEvtRstWthAllRelations_838(BPMNDBEntities context, TblEvtRst evtRst, bool stsExsSrcPelUpl = false)
        {
            List<TblNew> news = DetectNewsOfEvtRst_573(context, evtRst);

            //840
            foreach (TblNew news_i in news)
            {
                DeleteObjRstOfEvtRstAndChgPrs_709(context, news_i, stsExsSrcPelUpl);
            }

            List<TblObj> obj = DetectObjsOfEvtRst_574(context, evtRst);

            foreach (TblObj obj_i in obj)
            {
                DeleteObjRstOfEvtRstAndChgPrs_709(context, obj_i, stsExsSrcPelUpl);
            }

            List<TblSbjOral> sbjOral = DetectSbjOralsOfEvtRst_575(context, evtRst);

            foreach (TblSbjOral sbjOral_i in sbjOral)
            {
                DeleteObjRstOfEvtRstAndChgPrs_709(context, sbjOral_i, stsExsSrcPelUpl);
            }

            if (evtRst.FldTypEvtRst == (int)Enum.EvtRstType.getNewAwr || evtRst.FldTypEvtRst == (int)Enum.EvtRstType.getNewAwrAftrAct)
            {
                TblEvtSrt evtSrt = evtRst.TblEvt_GainAwrNew.First().TblEvtSrt;

                DeleteEvtSrtWthAllRelations_808(context, evtSrt, stsExsSrcPelUpl);
            }

            context.DeleteObject(evtRst);
        }

        /// <summary>
        /// حذف یک فعالیت و تمامی رخدادهای آغازگر و نتیجه
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DeleteAct_806(BPMNDBEntities context, TblAct act)
        {
            TblPr prs = DetectPrsOfAct_1839(context, act);

            List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act);

            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                DeleteEvtSrtWthAllRelations_808(context, evtSrt_i, true);
            }

            List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act);

            foreach (TblEvtRst evtRst_i in evtRst)
            {
                DeleteEvtRstWthAllRelations_838(context, evtRst_i, true);
            }

            SaveContext(context);

            DelectActOfPrs_8818(context, act);

            context.DeleteObject(act);

            EditPrsActsOfPrsAfterDelete_8820(context, prs);

            SaveContext(context);
        }

        /// <summary>
        /// transfered
        /// شناسایی رخداد نتیجه معادل یک رخداد آغازگر 
        /// </summary>
        /// <param name="tblEvtSrt">رخداد آغازگر</param>
        /// <param name="tblEvtRst">رخداد نتیجه معادل با رخداد آغازگر</param>
        public static TblEvtRst DetectEvtRstEqualToEvtSrt_527(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

            //528
            if (wayAwr.Count > 0)
            {
                return DetectEvtRstOfWayAwr_529(wayAwr[0]);
            }

            return null;
        }

        /// <summary>
        /// Pdr9059
        /// شناسایی رخداد نتیجه معادل یک نحوه آگاهی 
        /// </summary>
        /// <param name="wayAwr">نحوه آگاهی</param>
        /// <returns>رخداد نتیجه</returns>
        public static TblEvtRst DetectEvtRstOfWayAwr_529(IWayAwr wayAwr)
        {
            return wayAwr.WayIfrm.ObjRst.EvtRst;

            //if (wayAwr.GetType() == typeof(TblWayAwr_News))
            //{
            //    return ((TblWayAwr_News)wayAwr).TblWayIfrm_News.TblNew.TblEvtRst;
            //}

            //else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            //{
            //    return ((TblWayAwr_Oral)wayAwr).TblWayIfrm_Oral.TblSbjOral.TblEvtRst;
            //}

            //else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            //{
            //    return ((TblWayAwr_RecvInt)wayAwr).TblWayIfrm_SndOut.TblObj.TblEvtRst;
            //}
            //return null;
        }

        /// <summary>
        /// Pdr9023
        /// افزودن یک شی نتیجه موجود به یک نحوه آگاهی
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="objRst">شی نتیجه</param>
        /// <param name="wayAwr">نحوه آگاهی</param>
        public static void AddExistingObjRstToWayAwr_1479(BPMNDBEntities context, IObjRst objRst, IWayAwr wayAwr)
        {
            TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

            TblNod nod = DetectNodOfEvtsrt(evtSrt);

            List<IWayAwr> lst = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

            bool stsSnd = StsSndObjRstToAct_22034(objRst, nod.TblActs.SingleOrDefault(m => m.FldActUspf));

            DeleteWayIfrmObjRstFomActUspfNod_758(context, nod, objRst);

            if (lst.Count == 0)
            {
                IWayIfrm wayIfrm = AddWayIfrmToWayAwr_625(wayAwr, objRst);

                //dynamic wayIfrm_d = wayIfrm;

                ////19068
                //wayIfrm_d.FldTypDson = null;
                ////19201
                //wayIfrm_d.FldStsDsonWayIfrm = null;

                AddWayIfrmToObjRst_757(wayIfrm, objRst);
            }

            else
            {
                TblEvtRst evtRst1 = DetectEvtRstOfObjRst_562(objRst);

                TblEvtRst evtRst2 = DetectEvtRstEqualToEvtSrt_527(context, evtSrt);

                if (evtRst1 != evtRst2)
                {
                    IObjRst objRst1 = CopyObjRst_638(objRst);

                    AddObjRstToEvtRst_770(evtRst2, objRst1);

                    IWayIfrm wayIfrm = AddWayIfrmToWayAwr_625(wayAwr, objRst1);

                    AddWayIfrmToObjRst_757(wayIfrm, objRst1);
                }

                else
                {
                    IWayIfrm wayIfrm = AddWayIfrmToWayAwr_625(wayAwr, objRst);

                    AddWayIfrmToObjRst_757(wayIfrm, objRst);
                }
            }

            //22042
            if (stsSnd)
            {
                ((dynamic)wayAwr).FldTypDson = null;

                ((dynamic)wayAwr).FldStsDsonWayAwr = null;

                ((dynamic)wayAwr.WayIfrm).FldTypDson = null;

                ((dynamic)wayAwr.WayIfrm).FldStsDsonWayIfrm = null;
            }
        }


        public static IWayAwr copyWayAwr(IWayAwr awr)
        {
            IWayAwr newAwr = null;
            if (awr is TblWayAwr_RecvInt)
            {
                var rcvInt = awr as TblWayAwr_RecvInt;
                newAwr = new TblWayAwr_RecvInt()
                {
                    FldWayRecv = rcvInt.FldWayRecv,
                    FldCodSfw = rcvInt.FldCodSfw,
                    FldCodCmrIntPerRecv = rcvInt.FldCodCmrIntPerRecv,
                    FldTnoIntPerRecv = rcvInt.FldTnoIntPerRecv,
                    FldCodUntMsrtInt = rcvInt.FldCodUntMsrtInt,
                    FldIntNeedPrsg = rcvInt.FldIntNeedPrsg,
                    FldTnoIntPerPrsg = rcvInt.FldTnoIntPerPrsg,
                    FldCodTypPrsg = rcvInt.FldCodTypPrsg,
                    FldCodWayPrsg = rcvInt.FldCodWayPrsg,
                    FldTypDson = rcvInt.FldTypDson,
                    FldStsDsonWayAwr = rcvInt.FldStsDsonWayAwr
                };

            }

            return newAwr;
        }

        /// <summary>
        /// Pdr9057
        /// افزودن یک شی نتیجه جدید به یک فعالیت تولید کننده و یک نحوه آگاهی به عنوان مقصد آن
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="ObjRst">شی نتیجه</param>
        /// <param name="wayAwr">نحوه آگاهی دریافت کننده شی نتیجه</param>
        /// <param name="nod">گرهی که تولید کننده این شی نتیجه است</param>
        public static IWayAwr AddNewObjRstToWayAwr_1017(BPMNDBEntities context, IObjRst ObjRst, IWayAwr wayAwr, TblAct act)
        {
            TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_716(wayAwr);

            var newAwr = wayAwr;


            //1019
            if (wayIfrm != null)
            {
                //newAwr = copyWayAwr(wayAwr);

                TblPr prsAct = DetectPrsOfEvtSrt_2347(context, evtSrt);

                DeleteWayIfrm_723(context, wayIfrm, Model.Enum.DirectionForDelete.Right);

                AddWayAwrToEvtSrt_624(evtSrt, newAwr);

                if (newAwr.GetType() != typeof(TblWayAwr_News))
                {
                    EditPrsActsOfPrsAfterDelete_8820(context, prsAct);
                }
            }

            List<IWayAwr> wayAwr1 = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

            //1021
            if (wayAwr1.Count == 0)
            {
                Model.Enum.EvtRstType typEvtRst = DetectTypEvtRstEqualToTypEvtSrt_508(evtSrt.FldTypEvtSrt);

                TblEvtRst evtRst = AddEvtRstToAct_458(context, act, typEvtRst);

                AddObjRstToEvtRst_770(evtRst, ObjRst);
            }

            //1024
            else
            {
                TblEvtRst evtRst = DetectEvtRstEqualToEvtSrt_527(context, evtSrt);

                AddObjRstToEvtRst_770(evtRst, ObjRst);
            }

            //AddWayAwrToEvtSrt_624(evtSrt, wayAwr);

            IWayIfrm wayIfrm1 = AddWayIfrmToWayAwr_625(newAwr, ObjRst);

            AddWayIfrmToObjRst_757(wayIfrm1, ObjRst);

            return newAwr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ObjRst"></param>
        /// <param name="wayAwr"></param>
        /// <param name="act"></param>
        public static void AddNewObjRstToWayAwrAndChgPrs_6724(BPMNDBEntities context, IObjRst ObjRst, IWayAwr wayAwr, TblAct act)
        {
            var newAwr = AddNewObjRstToWayAwr_1017(context, ObjRst, wayAwr, act);

            TblAct actOth = act;

            //6726
            if (ObjRst.GetType() != typeof(TblNew))
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(newAwr);

                TblPr prsAct = DetectPrsOfEvtSrt_2347(context, evtSrt);

                TblPr prsActOth = DetectPrsOfAct_1839(context, actOth);

                EditPrsActsOfPrsAfterAdd_8830(context, prsAct, actOth, prsActOth);
            }
        }

        /// <summary>
        /// 503
        /// شناسایی نحوه های اگاهی یک رخداد اغازگر که دارای نحوه آگاه سازی هستند
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="tblEvtSrt">رخداد آغازگر</param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrOfEvtSrtWithWayIfrm_503(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<IWayAwr> lst = new List<IWayAwr>();

            lst.AddRange(DetectWayAwrOfEvtSrtWithWayIfrmOfTypNews_488(context, evtSrt));

            lst.AddRange(DetectWayAwrOfEvtSrtWithWayIfrmOfTypSbjOral_495(context, evtSrt));

            lst.AddRange(DetectWayAwrOfEvtSrtWithWayIfrmOfTypObj_499(context, evtSrt));

            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_RecvInt> DetectWayAwrOfEvtSrtWithWayIfrmOfTypObj_499(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<TblWayAwr_RecvInt> wayAwr = new List<TblWayAwr_RecvInt>();

            List<TblWayAwr_RecvInt> wayAwr1 = DetectWayAwrOfEvtSrtOfTypObj_487(context, evtSrt);

            //500
            foreach (TblWayAwr_RecvInt item in wayAwr1)
            {
                if (item.TblWayIfrm_SndOut != null)
                {
                    wayAwr.Add(item);
                }
            }

            return wayAwr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_Oral> DetectWayAwrOfEvtSrtWithWayIfrmOfTypSbjOral_495(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<TblWayAwr_Oral> wayAwr = new List<TblWayAwr_Oral>();

            List<TblWayAwr_Oral> wayAwr1 = DetectWayAwrOfEvtSrtOfTypSbjOral_486(context, evtSrt);

            foreach (TblWayAwr_Oral item in wayAwr1)
            {
                if (item.TblWayIfrm_Oral != null)
                {
                    wayAwr.Add(item);
                }
            }

            return wayAwr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_News> DetectWayAwrOfEvtSrtWithWayIfrmOfTypNews_488(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<TblWayAwr_News> wayAwr = new List<TblWayAwr_News>();

            List<TblWayAwr_News> wayAwr1 = DetectWayAwrOfEvtSrtOfTypNews_485(context, evtSrt);

            foreach (TblWayAwr_News item in wayAwr1)
            {
                if (item.TblWayIfrm_News != null)
                {
                    wayAwr.Add(item);
                }
            }

            return wayAwr;
        }

        /// <summary>
        /// Pdr9026: حذف نحوه های اگاه سازی یک شی نتیجه با مقصد فعالیت نامشخص از یک گره
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="nod">گره</param>
        /// <param name="ObjRst">شی نتیجه</param>
        public static void DeleteWayIfrmObjRstFomActUspfNod_758(BPMNDBEntities context, TblNod nod, IObjRst ObjRst)
        {
            TblAct act1 = DetectActUspfNod_565(context, nod);

            List<IWayIfrm> wayIfrm = DetectWayIfrmOfObjRst_578(context, ObjRst);

            foreach (IWayIfrm wayIfrm_i in wayIfrm)
            {
                TblAct act2 = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                if (act1 == act2)
                {
                    TblEvtSrt evtSrt = DetectEvtSrtOfWayIfrm_609(wayIfrm_i);

                    List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

                    if (wayAwr.Count == 1)
                    {
                        context.DeleteObject(evtSrt);
                    }

                    context.DeleteObject(wayIfrm_i);
                }
            }
        }

        /// <summary>
        /// F565
        /// شناسایی فعالیت نامشخص یک گره
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="nod">گره</param>
        /// <returns>فعالیت نامشخص</returns>
        public static TblAct DetectActUspfNod_565(BPMNDBEntities context, TblNod nod)
        {
            return context.TblActs.SingleOrDefault(E => E.FldActUspf && E.FldCodNod == nod.FldCodNod);
        }

        /// <summary>
        /// لیست نحوه های آگاه سازی یک خبر را می دهد
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="tblNew">خبر</param>
        /// <returns></returns>
        public static List<TblWayIfrm_News> WayIfrmNewsOfOneNew(BPMNDBEntities context, TblNew tblNew)
        {
            List<TblWayIfrm_News> lst = new List<TblWayIfrm_News>();
            foreach (TblWayIfrm_News item in tblNew.TblWayIfrm_News)
            {
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// لیست نحوه های آگاه سازی یک مطلب شفاهی را می دهد
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="tblNew">مطلب شفاهی</param>
        /// <returns></returns>
        public static List<TblWayIfrm_Oral> WayIfrmOralOfOneSbjOral(BPMNDBEntities context, TblSbjOral tblSbjOral)
        {
            List<TblWayIfrm_Oral> lst = new List<TblWayIfrm_Oral>();
            foreach (TblWayIfrm_Oral item in tblSbjOral.TblWayIfrm_Oral)
            {
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// لیست نحوه های آگاه سازی یک خروجی را می دهد
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="tblNew">خروجی</param>
        /// <returns></returns>
        public static List<TblWayIfrm_SndOut> WayIfrmSndOutOfOneObj(BPMNDBEntities context, TblObj tblObj)
        {
            List<TblWayIfrm_SndOut> lst = new List<TblWayIfrm_SndOut>();
            foreach (TblWayIfrm_SndOut item in tblObj.TblWayIfrm_SndOut)
            {
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// حذف نحوه های آگاهی از یک رخداد آغازگر که دارای نحوه آگاه سازی نیستند
        /// </summary>
        /// <param name="tblEvtSrt">رخداد آغازگر</param>
        public static void DeleteUnusableWayAwrOfEvtSrt(TblEvtSrt tblEvtSrt)
        {
            List<IWayAwr> lst = DetectWayAwrOfEvtSrt(tblEvtSrt);
            foreach (var item in lst)
            {
                if (item.GetType() == typeof(TblWayAwr_News))
                {
                    TblWayAwr_News tbl = (TblWayAwr_News)item;
                    if (tbl.TblWayIfrm_News == null)
                    {
                        tblEvtSrt.TblWayAwr_News.Remove(tbl);
                    }
                }
                else if (item.GetType() == typeof(TblWayAwr_Oral))
                {
                    TblWayAwr_Oral tbl = (TblWayAwr_Oral)item;
                    if (tbl.TblWayIfrm_Oral == null)
                    {
                        tblEvtSrt.TblWayAwr_Oral.Remove(tbl);
                    }
                }
                else if (item.GetType() == typeof(TblWayAwr_RecvInt))
                {
                    TblWayAwr_RecvInt tbl = (TblWayAwr_RecvInt)item;
                    if (tbl.TblWayIfrm_SndOut == null)
                    {
                        tblEvtSrt.TblWayAwr_RecvInt.Remove(tbl);
                    }
                }
            }
        }

        /// <summary>
        /// F674
        /// شناسایی رخداد آغازگر معادل یک نحوه آگاهی
        /// </summary>
        /// <param name="wayAwr"></param>
        /// <returns>رخداد آغازگر</returns>
        public static TblEvtSrt DetectEvtSrtOfWayAwr_674(IWayAwr wayAwr)
        {
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                TblWayAwr_News tbl = (TblWayAwr_News)wayAwr;
                return tbl.TblEvtSrt;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                TblWayAwr_Oral tbl = (TblWayAwr_Oral)wayAwr;
                return tbl.TblEvtSrt;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                TblWayAwr_RecvInt tbl = (TblWayAwr_RecvInt)wayAwr;
                return tbl.TblEvtSrt;
            }
            return null;
        }

        /// <summary>
        /// Pdr9029
        /// </summary>
        /// <param name="evtSrt"></param>
        /// <returns>گره</returns>
        public static TblNod DetectNodOfEvtsrt(TblEvtSrt evtSrt)
        {
            return evtSrt.TblAct.TblNod;
        }

        /// <summary>
        /// F625
        /// افزودن یک نحوه آگاه سازی به یک نحوه آگاهی
        /// </summary>
        /// <param name="wayAwr"></param>
        /// <returns>نحوه آگاه سازی</returns>
        public static IWayIfrm AddWayIfrmToWayAwr_625(IWayAwr wayAwr, IObjRst objRst)
        {
            //628
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                TblWayIfrm_News tblWayIfrm = new TblWayIfrm_News();
                TblWayAwr_News tblWayAwr = (TblWayAwr_News)wayAwr;
                tblWayAwr.TblWayIfrm_News = tblWayIfrm;
                tblWayIfrm.FldTypDson = 3;
                DetermineDsonSttForWayIfrm_19144(tblWayIfrm, objRst);
                return tblWayIfrm;
            }
            //627
            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                TblWayIfrm_Oral tblWayIfrm = new TblWayIfrm_Oral();
                TblWayAwr_Oral tblWayAwr = (TblWayAwr_Oral)wayAwr;
                tblWayAwr.TblWayIfrm_Oral = tblWayIfrm;
                tblWayIfrm.FldTypDson = 2;
                DetermineDsonSttForWayIfrm_19144(tblWayIfrm, objRst);
                return tblWayIfrm;
            }
            //626
            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                TblWayIfrm_SndOut tblWayIfrm = new TblWayIfrm_SndOut();
                TblWayAwr_RecvInt tblWayAwr = (TblWayAwr_RecvInt)wayAwr;
                tblWayAwr.TblWayIfrm_SndOut = tblWayIfrm;
                tblWayIfrm.FldTypDson = 1;
                DetermineDsonSttForWayIfrm_19144(tblWayIfrm, objRst);
                return tblWayIfrm;
            }
            return null;
        }

        /// <summary>
        /// F757
        /// افزودن یک نحوه آگاه سازی به یک شی نتیجه 
        /// </summary>
        /// <param name="wayIfrm">نحوه آگاه سازی</param>
        /// <param name="ObjRst">شی نتیجه</param>
        public static void AddWayIfrmToObjRst_757(IWayIfrm wayIfrm, IObjRst ObjRst)
        {
            if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            {
                TblWayIfrm_News tblWayIfrm = (TblWayIfrm_News)wayIfrm;
                TblNew tblObj = (TblNew)ObjRst;
                tblObj.TblWayIfrm_News.Add(tblWayIfrm);

            }
            else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            {
                TblWayIfrm_Oral tblWayIfrm = (TblWayIfrm_Oral)wayIfrm;
                TblSbjOral tblObj = (TblSbjOral)ObjRst;
                tblObj.TblWayIfrm_Oral.Add(tblWayIfrm);
            }
            else if (wayIfrm.GetType() == typeof(TblWayIfrm_SndOut))
            {
                TblWayIfrm_SndOut tblWayIfrm = (TblWayIfrm_SndOut)wayIfrm;
                TblObj tblObj = (TblObj)ObjRst;
                //if (!tblObj.TblWayIfrm_SndOut.Any(w => w.FldCodWayIfrm == tblWayIfrm.FldCodWayIfrm))
                {
                    tblObj.TblWayIfrm_SndOut.Add(tblWayIfrm);
                }
            }
        }

        /// <summary>
        /// F638
        /// ایجاد یک شی نتیجه مشابه یک شی نتیجه دیگر 
        /// </summary>
        /// <param name="objRst">شی نتیجه</param>
        /// <returns>شی نتیجه</returns>
        public static IObjRst CopyObjRst_638(IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                return CreateNewsLikNewsOth_643((TblNew)objRst);
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                return CreateSbjOralLikSbjOralOth_645((TblSbjOral)objRst);
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                return CreateObjLikObjOth_644((TblObj)objRst);
            }
            return null;
        }

        /// <summary>
        /// F694
        /// کپی کردن مشخصات یک شی نتیجه در یک شی نتیجه دیگر 
        /// </summary>
        /// <param name="objRst1"></param>
        /// <param name="objRst2"></param>
        public static void CopyObjRstToAnotherObjRst_694(IObjRst objRst1, IObjRst objRst2)
        {
            if (objRst1.GetType() == typeof(TblNew))
            {
                TblNew tbl1 = (TblNew)objRst1;
                TblNew tbl2 = (TblNew)objRst2;
                tbl2.FldTtlNews = tbl1.FldTtlNews;
                tbl2.FldTxtNews = tbl1.FldTxtNews;
            }
            else if (objRst1.GetType() == typeof(TblSbjOral))
            {
            }
            else if (objRst1.GetType() == typeof(TblObj))
            {
                TblObj tbl1 = (TblObj)objRst1;
                TblObj tbl2 = (TblObj)objRst2;
                tbl2.FldNamObj = tbl1.FldNamObj;
                tbl2.FldTypObj = tbl1.FldTypObj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtRst"></param>
        /// <param name="objRst"></param>
        public static void AddObjRstToEvtRst_770(TblEvtRst evtRst, IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                evtRst.TblNews.Add((TblNew)objRst);
            }

            else if (objRst.GetType() == typeof(TblObj))
            {
                evtRst.TblObjs.Add((TblObj)objRst);
            }

            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                evtRst.TblSbjOrals.Add((TblSbjOral)objRst);
            }
        }

        /// <summary>
        /// F723
        /// حذف یک نحوه آگاه سازی
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="wayIfrm">نحوه آگاه سازی</param>
        /// <param name="dirDel">جهت حذف</param>
        public static void DeleteWayIfrm_723(BPMNDBEntities context, IWayIfrm wayIfrm, Model.Enum.DirectionForDelete dirDel)
        {
            IWayAwr wayAwr1 = DetectWayAwrOfWayIfrm_623(wayIfrm);

            //725
            if (dirDel == DirectionForDelete.Left || dirDel == DirectionForDelete.Both)
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr1);

                List<IWayAwr> wayAwr2 = DetectWayAwrOfEvtSrt(evtSrt);

                //733
                if (wayAwr2.Count == 1 && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt);
                }

                    //738
                else
                {
                    context.DeleteObject(wayAwr1);
                }
            }
            if (dirDel == DirectionForDelete.Right)
            {
                context.Detach(wayAwr1);
            }
            if (dirDel == DirectionForDelete.Right || dirDel == DirectionForDelete.Both)
            {
                IObjRst objRst = DetectObjRstOfWayIfrm(context, wayIfrm);

                List<IWayIfrm> wayIfrm1 = DetectWayIfrmOfObjRst_578(context, objRst);

                if (wayIfrm1.Count == 1)
                {
                    TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

                    List<IObjRst> objRst1 = DetectObjRstOfEvtRst_572(context, evtRst);

                    if (objRst1.Count == 1)
                    {
                        context.DeleteObject(evtRst);
                    }
                    else
                    {
                        context.DeleteObject(objRst);
                    }
                }

                else
                {
                    context.DeleteObject(wayIfrm);
                }
            }
        }

        public static void DeleteWayIfrm_723_1(BPMNDBEntities context, IWayIfrm wayIfrm, Model.Enum.DirectionForDelete dirDel)
        {
            IWayAwr wayAwr1 = DetectWayAwrOfWayIfrm_623(wayIfrm);

            //725
            if (dirDel == DirectionForDelete.Left || dirDel == DirectionForDelete.Both)
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr1);

                List<IWayAwr> wayAwr2 = DetectWayAwrOfEvtSrt(evtSrt);

                //733
                if (wayAwr2.Count == 1 && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt);
                }

                    //738
                else
                {
                    context.DeleteObject(wayAwr1);
                }
            }
            if (dirDel == DirectionForDelete.Right)
            {
                //context.Detach(wayAwr1);
            }
            if (dirDel == DirectionForDelete.Right || dirDel == DirectionForDelete.Both)
            {
                IObjRst objRst = DetectObjRstOfWayIfrm(context, wayIfrm);

                List<IWayIfrm> wayIfrm1 = DetectWayIfrmOfObjRst_578(context, objRst);

                if (wayIfrm1.Count == 1)
                {
                    TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

                    List<IObjRst> objRst1 = DetectObjRstOfEvtRst_572(context, evtRst);

                    if (objRst1.Count == 1)
                    {
                        context.DeleteObject(evtRst);
                    }
                    else
                    {
                        context.DeleteObject(objRst);
                    }
                }

                else
                {
                    context.DeleteObject(wayIfrm);
                }
            }
        }


        public static void DeleteObjRst(BPMNDBEntities context, IObjRst objRst)
        {
            List<IWayIfrm> wayIfrm1 = DetectWayIfrmOfObjRst_578(context, objRst);

            var wayIfrm = objRst.WayIfrms.FirstOrDefault();

            if (wayIfrm1.Count == 1)
            {
                TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

                List<IObjRst> objRst1 = DetectObjRstOfEvtRst_572(context, evtRst);

                if (objRst1.Count == 1)
                {
                    context.DeleteObject(evtRst);
                }
                else
                {
                    context.DeleteObject(objRst);
                }
            }

            else
            {
                context.DeleteObject(wayIfrm);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="wayIfrm"></param>
        public static void DeleteWayIfrmOfObjRstAndChgPrs_3426(BPMNDBEntities context, IObjRst objRst, IWayIfrm wayIfrm, DirectionForDelete direction)
        {
            TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

            TblPr prsAct1 = DetectPrsOfEvtRst_2329(context, evtRst);

            TblPr prsAct2 = DetectPrsOfAct_1839(context, wayIfrm.ActDst);

            TblAct actDst = wayIfrm.ActDst;

            DeleteWayIfrm_723(context, wayIfrm, direction);

            //DeleteWayIfrmOfObjRst_726(context, objRst);

            context.DeleteObject(wayIfrm);

            //21749
            if (prsAct1 == prsAct2)
            {
                EditPrsActsOfPrsAfterDelete_8820(context, prsAct1);
            }

            //21751
            else if (prsAct1 != prsAct2)
            {
                EditPrsActsOfPrsAfterAdd_8830(context, prsAct1, actDst, prsAct2);
            }
        }

        /// <summary>
        /// F623
        /// شناسایی نحوه آگاهی یک نحوه آگاه سازی
        /// </summary>
        /// <param name="wayIfrm">نحوه آگاه سازی</param>
        /// <returns>نحوه آگاهی</returns>
        public static IWayAwr DetectWayAwrOfWayIfrm_623(IWayIfrm wayIfrm)
        {
            return wayIfrm.WayAwr;

            //if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            //{
            //    return ((TblWayIfrm_News)wayIfrm).TblWayAwr_News;
            //}
            //else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            //{
            //    return ((TblWayIfrm_Oral)wayIfrm).TblWayAwr_Oral;
            //}
            //else if (wayIfrm.GetType() == typeof(TblWayIfrm_SndOut))
            //{
            //    return ((TblWayIfrm_SndOut)wayIfrm).TblWayAwr_RecvInt;
            //}
            //return null;
        }

        /// <summary>
        /// Pdr9034
        /// شناسایی شی نتیجه یک نحوه آگاه سازی
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="wayIfrm">نحوه آگاه سازی</param>
        /// <returns>شی نتیجه</returns>
        public static IObjRst DetectObjRstOfWayIfrm(BPMNDBEntities context, IWayIfrm wayIfrm)
        {
            return wayIfrm.ObjRst;
            //if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            //{
            //    return ((TblWayIfrm_News)wayIfrm).TblNew;
            //}
            //else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            //{
            //    return ((TblWayIfrm_Oral)wayIfrm).TblSbjOral;
            //}
            //else if (wayIfrm.GetType() == typeof(TblWayIfrm_SndOut))
            //{
            //    return ((TblWayIfrm_SndOut)wayIfrm).TblObj;
            //}
            //return null;
        }

        /// <summary>
        /// F578
        /// شناسایی نحوه های آگاه سازی یک شی نتیجه
        /// </summary>
        /// <param name="ObjRst">شی نتیجه</param>
        /// <returns>نحوه های آگاه سازی</returns>
        public static List<IWayIfrm> DetectWayIfrmOfObjRst_578(BPMNDBEntities context, IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                TblNew tblObj = (TblNew)objRst;
                return new List<IWayIfrm>(DetectWayIfrmOfNews_587(context, tblObj));
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                TblSbjOral tblObj = (TblSbjOral)objRst;
                return new List<IWayIfrm>(DetectWayIfrmOfSbjOral_588(context, tblObj));
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                TblObj tblObj = (TblObj)objRst;
                return new List<IWayIfrm>(DetectWayIfrmObj_589(context, tblObj));
            }
            return null;
        }

        /// <summary>
        /// F562
        /// شناسایی رخداد نتیجه یک شی نتیجه
        /// </summary>
        /// <param name="objRst">شی نتیجه</param>
        /// <returns>رخداد نتیجه</returns>
        public static TblEvtRst DetectEvtRstOfObjRst_562(IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                TblNew tblObj = (TblNew)objRst;
                return tblObj.TblEvtRst;
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                TblSbjOral tblObj = (TblSbjOral)objRst;
                return tblObj.TblEvtRst;
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                TblObj tblObj = (TblObj)objRst;
                return tblObj.TblEvtRst;
            }
            return null;
        }

        /// <summary>
        /// transfered
        /// F572
        /// شناسایی اشیاء نتیجه یک رخداد نتیجه
        /// </summary>
        /// <param name="evtRst">رخداد نتیجه</param>
        /// <returns>اشیاء نتیجه</returns>
        public static List<IObjRst> DetectObjRstOfEvtRst_572(BPMNDBEntities context, TblEvtRst evtRst)
        {
            List<IObjRst> lst = new List<IObjRst>();

            lst.AddRange(DetectNewsOfEvtRst_573(context, evtRst));

            lst.AddRange(DetectObjsOfEvtRst_574(context, evtRst));

            lst.AddRange(DetectSbjOralsOfEvtRst_575(context, evtRst));

            return lst;
        }

        /// <summary>
        /// Pdr9046
        /// شناسایی نحوه های آگاهی یک رخداد آغازگر
        /// </summary>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrOfEvtSrt(TblEvtSrt evtSrt)
        {
            List<IWayAwr> lst = new List<IWayAwr>();
            foreach (TblWayAwr_News item in evtSrt.TblWayAwr_News)
            {
                lst.Add(item);
            }
            foreach (TblWayAwr_Oral item in evtSrt.TblWayAwr_Oral)
            {
                lst.Add(item);
            }
            foreach (TblWayAwr_RecvInt item in evtSrt.TblWayAwr_RecvInt)
            {
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// Pdr9049
        /// شناسایی نحوه آگاه سازی یک نحوه آگاهی
        /// </summary>
        /// <param name="wayAwr">نحوه آگاهی</param>
        /// <returns>نحوه آگاه سازی</returns>
        public static IWayIfrm DetectWayIfrmOfWayAwr_716(IWayAwr wayAwr)
        {
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                return ((TblWayAwr_News)wayAwr).TblWayIfrm_News;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                return ((TblWayAwr_Oral)wayAwr).TblWayIfrm_Oral;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                return ((TblWayAwr_RecvInt)wayAwr).TblWayIfrm_SndOut;
            }
            return null;
        }

        /// <summary>
        /// Pdr9051
        /// انتخاب شی نتیجه موجود برای یک نحوه آگاهی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="wayAwr"></param>
        public static void SelectExistingObjRstForWayAwr(BPMNDBEntities context, IObjRst objRst, IWayAwr wayAwr)
        {
            TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_716(wayAwr);

            if (wayIfrm != null)
            {
                DeleteWayIfrm_723(context, wayIfrm, DirectionForDelete.Right);
            }

            AddWayAwrToEvtSrt_624(evtSrt, wayAwr);

            AddExistingObjRstToWayAwr_1479(context, objRst, wayAwr);
        }

        /// <summary>
        /// Pdr9039
        /// انتخاب یک شی نتیجه جدید برای یک نحوه اگاهی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="wayAwr"></param>
        public static void SelectNewObjRstForWayAwr(BPMNDBEntities context, IObjRst objRst, IWayAwr wayAwr)
        {
            TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_716(wayAwr);

            if (wayIfrm != null)
            {
                DeleteWayIfrm_723(context, wayIfrm, DirectionForDelete.Right);
            }

            AddWayAwrToEvtSrt_624(evtSrt, wayAwr);

            //AddExistingObjRstToWayAwr(context, objRst, wayAwr);
        }

        /// <summary>
        /// 624
        /// افزودن یک نحوه آگاهی به یک رخداد آغازگر
        /// </summary>
        /// <param name="evtSrt">رخداد آغازگر</param>
        /// <param name="wayAwr">نحوه آگاهی</param>
        public static void AddWayAwrToEvtSrt_624(TblEvtSrt evtSrt, IWayAwr wayAwr)
        {
            //var lst = evtSrt.GetContext<BPMNDBEntities>().ObjectStateManager.GetObjectStateEntries(EntityState.Unchanged | EntityState.Modified | EntityState.Added).Where(e => e.EntityKey != null &&
            //    e.EntityKey.EntityKeyValues != null
            //    && e.EntityKey.EntityKeyValues.Count() > 0
            //    &&
            //    e.EntityKey.EntityKeyValues[0].Value.ToString() == wayAwr.FldCod.ToString());


            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                evtSrt.TblWayAwr_News.Add((TblWayAwr_News)wayAwr);
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                evtSrt.TblWayAwr_Oral.Add((TblWayAwr_Oral)wayAwr);
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                //if (!evtSrt.TblWayAwr_RecvInt.Any(w => w.FldCodWayAwr == wayAwr.FldCod))
                {
                    evtSrt.TblWayAwr_RecvInt.Add((TblWayAwr_RecvInt)wayAwr);
                }
            }
        }

        /// <summary>
        /// 
        /// لیست فعالیت هایی که یک شی نتیجه به آنها وارد می شود را می دهد
        /// </summary>
        /// <param name="objRst">شی نتیجه</param>
        /// <returns></returns>
        public static List<TblAct> DetectActTargetedBySpecificObjRst(IObjRst objRst)
        {
            List<TblAct> lst = new List<TblAct>();
            if (objRst.GetType() == typeof(TblNew))
            {
                TblNew tblObj = (TblNew)objRst;
                foreach (TblWayIfrm_News item in tblObj.TblWayIfrm_News)
                {
                    if (item.TblWayAwr_News == null)
                    {
                        continue;
                    }
                    lst.Add(item.TblWayAwr_News.TblEvtSrt.TblAct);
                }
                return lst;
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                TblSbjOral tblObj = (TblSbjOral)objRst;
                foreach (TblWayIfrm_Oral item in tblObj.TblWayIfrm_Oral)
                {
                    if (item.TblWayAwr_Oral == null)
                    {
                        continue;
                    }
                    lst.Add(item.TblWayAwr_Oral.TblEvtSrt.TblAct);
                }
                return lst;
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                TblObj tblObj = (TblObj)objRst;
                foreach (TblWayIfrm_SndOut item in tblObj.TblWayIfrm_SndOut)
                {

                    if (item.TblWayAwr_RecvInt == null)
                    {
                        continue;
                    }

                    lst.Add(item.TblWayAwr_RecvInt.TblEvtSrt.TblAct);
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// 
        /// لیست گره هایی که یک شی نتیجه به آنها وارد می شود را می دهد
        /// </summary>
        /// <param name="objRst">شی نتیجه</param>
        /// <returns></returns>
        public static List<TblNod> DetectNodTargetedBySpecificObjRst(IObjRst objRst)
        {
            List<TblNod> lst = new List<TblNod>();
            if (objRst.GetType() == typeof(TblNew))
            {
                TblNew tblObj = (TblNew)objRst;
                foreach (TblWayIfrm_News item in tblObj.TblWayIfrm_News)
                {
                    lst.Add(item.TblWayAwr_News.TblEvtSrt.TblAct.TblNod);
                }
                return lst;
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                TblSbjOral tblObj = (TblSbjOral)objRst;
                foreach (TblWayIfrm_Oral item in tblObj.TblWayIfrm_Oral)
                {
                    lst.Add(item.TblWayAwr_Oral.TblEvtSrt.TblAct.TblNod);
                }
                return lst;
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                TblObj tblObj = (TblObj)objRst;
                foreach (TblWayIfrm_SndOut item in tblObj.TblWayIfrm_SndOut)
                {
                    lst.Add(item.TblWayAwr_RecvInt.TblEvtSrt.TblAct.TblNod);
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// Pdr9055
        /// شناسایی اشیاء نتیجه با مبدأ یک فعالیت و مقصد یک گره 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="nod"></param>
        /// <param name="typObj"></param>
        /// <returns></returns>
        public static List<IObjRst> DetectObjRstOfActToNod(BPMNDBEntities context, TblAct act, TblNod nod, TypeOfObjRst typObj)
        {
            List<IObjRst> lst = new List<IObjRst>();

            if (typObj == TypeOfObjRst.TblNews || typObj == TypeOfObjRst.All)
            {
                List<TblNew> tbl = context.TblNews.Where(m => m.TblEvtRst.TblAct == act).ToList<TblNew>();
                foreach (TblNew item in tbl)
                {
                    if (item.NodTarget.Contains(nod))
                    {
                        lst.Add(item);
                    }
                }
            }

            if (typObj == TypeOfObjRst.TblSbjOral || typObj == TypeOfObjRst.All)
            {
                List<TblSbjOral> tbl = context.TblSbjOrals.Where(m => m.TblEvtRst.TblAct == act).ToList<TblSbjOral>();
                foreach (TblSbjOral item in tbl)
                {
                    if (item.NodTarget.Contains(nod))
                    {
                        lst.Add(item);
                    }
                }
            }

            if (typObj == TypeOfObjRst.TblObj || typObj == TypeOfObjRst.All)
            {
                List<TblObj> tbl = context.TblObjs.Where(m => m.TblEvtRst.TblAct == act).ToList<TblObj>();
                foreach (TblObj item in tbl)
                {
                    if (item.NodTarget.Contains(nod))
                    {
                        lst.Add(item);
                    }
                }
            }

            return lst;
        }

        /// <summary>
        /// Pdr9056
        /// شناسایی اشیاء نتیجه با مقصد یک گره 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="nod"></param>
        /// <param name="typObj"></param>
        /// <returns></returns>
        public static List<IObjRst> DetectObjRstToNod(BPMNDBEntities context, TblNod nod, TypeOfObjRst typObj)
        {
            List<IObjRst> lst = new List<IObjRst>();

            if (typObj == TypeOfObjRst.TblNews || typObj == TypeOfObjRst.All)
            {
                List<TblNew> tbl = context.TblNews.ToList<TblNew>();
                foreach (TblNew item in tbl)
                {
                    lst.Add(item);
                }
            }

            if (typObj == TypeOfObjRst.TblSbjOral || typObj == TypeOfObjRst.All)
            {
                List<TblSbjOral> tbl = context.TblSbjOrals.ToList<TblSbjOral>();
                foreach (TblSbjOral item in tbl)
                {
                    lst.Add(item);
                }
            }

            if (typObj == TypeOfObjRst.TblObj || typObj == TypeOfObjRst.All)
            {
                List<TblObj> tbl = context.TblObjs.ToList<TblObj>();
                foreach (TblObj item in tbl)
                {
                    lst.Add(item);
                }
            }

            return lst;
        }

        /// <summary>
        /// Pdr9035
        /// </summary>
        /// <param name="evtSrtList"></param>
        /// <returns></returns>
        public static bool ValidateSaveEvtSrtInPointOfViewWayAwr(TblEvtSrt[] evtSrtList)
        {
            foreach (TblEvtSrt evtSrt in evtSrtList)
            {
                if (evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr
                    && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim
                    && evtSrt.FldTypEvtSrt != (int)EvtSrtType.inSgmtTime
                    && evtSrt.FldTypEvtSrt != (int)EvtSrtType.aftrAnyCdnEvtSrt
                    && evtSrt.WayAwrs.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Pdr9036
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static bool ValidateActChange(TblAct act)
        {
            if (act.TblNod.TblActs.FirstOrDefault(m => m.FldNamAct.Trim().ToLower() == act.FldNamAct.Trim().ToLower() && m.FldCodAct != act.FldCodAct) != null)
            {
                TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 25);
                throw new InvalidOperationException(msg.FldTxtMsg);
            }

            if (!act.FldActUspf && act.TblEvtRsts.Any(m =>
                m.FldTypEvtRst == (int)EvtRstType.errAccurEvtRst
                || m.FldTypEvtRst == (int)EvtRstType.cancelEvtRst
                || m.FldTypEvtRst == (int)EvtRstType.spcCdnEvtRstInnTim
                ))
            {
                if (!act.FldActUspf && !act.TblEvtRsts.Any(m =>
                    m.FldTypEvtRst == (int)EvtRstType.getNewAwrAftrAct
                    || m.FldTypEvtRst == (int)EvtRstType.anyCdnEvtRst
                    || m.FldTypEvtRst == (int)EvtRstType.spcCdnEvtRstAftr))
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 24);
                    throw new InvalidOperationException(msg.FldTxtMsg);
                }
            }

            return ValidateSaveEvtSrtInPointOfViewWayAwr(act.TblEvtSrts.ToArray());
        }


        /// <summary>
        /// F551
        /// شناسایی فعالیت مبدأ یا شی نتیجه
        /// </summary>
        /// <returns></returns>
        public static TblAct DetectActSrcOfObjRst_551(IObjRst objRst)
        {
            TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

            return evtRst.TblAct;
        }

        /// <summary>
        /// 609
        /// شناسایی رخداد آغازگر معادل یک نحوه آگاه سازی
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static TblEvtSrt DetectEvtSrtOfWayIfrm_609(IWayIfrm wayIfrm)
        {
            IWayAwr wayAwr = DetectWayAwrOfWayIfrm_623(wayIfrm);

            if (wayAwr != null)
            {
                return DetectEvtSrtOfWayAwr_674(wayAwr);
            }

            return null;
        }

        /// <summary>
        /// F676
        /// متصل کردن یک نحوه آگاه سازی به یک نحوه آگاهی
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <param name="wayAwr"></param>
        public static void AddWayAwrToWayIfrm_722(BPMNDBEntities context, IWayIfrm wayIfrm, IWayAwr wayAwr)
        {
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                ((TblWayAwr_News)wayAwr).TblWayIfrm_News = (TblWayIfrm_News)wayIfrm;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                ((TblWayAwr_Oral)wayAwr).TblWayIfrm_Oral = (TblWayIfrm_Oral)wayIfrm;
            }
            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                ((TblWayAwr_RecvInt)wayAwr).TblWayIfrm_SndOut = (TblWayIfrm_SndOut)wayIfrm;
            }

            //19036
            if (wayAwr is TblWayAwr_RecvInt)
            {
                (wayAwr as TblWayAwr_RecvInt).FldTypDson = 4;
                DetermineDsonSttForWayAwr_19082(wayAwr, DetectObjRstWayIfrm_721(context, wayIfrm));
            }
            //19035
            if (wayAwr is TblWayAwr_Oral)
            {
                (wayAwr as TblWayAwr_Oral).FldTypDson = 5;
                DetermineDsonSttForWayAwr_19082(wayAwr, DetectObjRstWayIfrm_721(context, wayIfrm));
            }
        }

        /// <summary>
        /// F651
        /// شناسایی رخدادهای آغازگر معادل یک خبر
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static List<TblEvtSrt> DetectEvtsrtOfNews_651(BPMNDBEntities context, TblNew news)
        {
            List<TblEvtSrt> lst = new List<TblEvtSrt>();

            List<TblWayIfrm_News> wayIfrm = DetectWayIfrmOfNews_587(context, news);

            foreach (TblWayIfrm_News item in wayIfrm)
            {
                lst.Add(item.TblWayAwr_News.TblEvtSrt);
            }

            return lst;
        }

        /// <summary>
        /// F654
        /// شناسایی رخدادهای آغازگر معادل یک مطلب شفاهی
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static List<TblEvtSrt> DetectEvtsrtOfSbjOral_654(BPMNDBEntities context, TblSbjOral sbjOral)
        {
            List<TblEvtSrt> lst = new List<TblEvtSrt>();

            List<TblWayIfrm_Oral> wayIfrm = DetectWayIfrmOfSbjOral_588(context, sbjOral);

            foreach (TblWayIfrm_Oral item in wayIfrm)
            {
                lst.Add(item.TblWayAwr_Oral.TblEvtSrt);
            }

            return lst;
        }

        /// <summary>
        /// F657
        /// شناسایی رخدادهای آغازگر معادل یک خروجی
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static List<TblEvtSrt> DetectEvtsrtOfObj_657(BPMNDBEntities context, TblObj obj)
        {
            List<TblEvtSrt> lst = new List<TblEvtSrt>();

            List<TblWayIfrm_SndOut> wayIfrm = DetectWayIfrmObj_589(context, obj);

            foreach (TblWayIfrm_SndOut item in wayIfrm)
            {
                lst.Add(item.TblWayAwr_RecvInt.TblEvtSrt);
            }

            return lst;
        }

        /// <summary>
        /// F647
        /// شناسایی رخدادهای آغازگر معادل یک شی نتیجه
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static List<TblEvtSrt> DetectEvtsrtOfObjRst_647(BPMNDBEntities context, IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                return DetectEvtsrtOfNews_651(context, (TblNew)objRst);
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                return DetectEvtsrtOfSbjOral_654(context, (TblSbjOral)objRst);
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                return DetectEvtsrtOfObj_657(context, (TblObj)objRst);
            }
            return null;
        }

        /// <summary>
        /// متصل کردن یک شی نتیجه به یک نحوه آگاه سازی
        /// </summary>
        /// <param name="objRst"></param>
        /// <param name="wayIfrm"></param>
        public static void ConnectObjRstToWayIfrm_660(IObjRst objRst, IWayIfrm wayIfrm)
        {
            if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            {
                ((TblWayIfrm_News)wayIfrm).TblNew = (TblNew)objRst;
            }
            else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            {
                ((TblWayIfrm_Oral)wayIfrm).TblSbjOral = (TblSbjOral)objRst;
            }
            else if (wayIfrm.GetType() == typeof(TblWayIfrm_SndOut))
            {
                ((TblWayIfrm_SndOut)wayIfrm).TblObj = (TblObj)objRst;
            }
        }

        /// <summary>
        /// F677
        /// حذف کردن یک شی نتیجه از لیست اشیاء نتیجه یک رخداد نتیجه
        /// </summary>
        /// <param name="evtRst"></param>
        /// <param name="objRst"></param>
        public static void RemoveObjRstOfEvtRst_677(TblEvtRst evtRst, IObjRst objRst)
        {
            if (objRst.GetType() == typeof(TblNew))
            {
                evtRst.TblNews.Remove((TblNew)objRst);
            }
            else if (objRst.GetType() == typeof(TblSbjOral))
            {
                evtRst.TblSbjOrals.Remove((TblSbjOral)objRst);
            }
            else if (objRst.GetType() == typeof(TblObj))
            {
                evtRst.TblObjs.Remove((TblObj)objRst);
            }
        }

        /// <summary>
        /// F687
        /// کپی کردن تمامی مشخصات یک شی نتیجه به همراه جداول مربوطه در یک شی نتیجه دیگر و حذف شی نتیجه اول
        /// </summary>
        /// <param name="objRst1"></param>
        /// <param name="objRst2"></param>
        public static void CopyAllAttributesOfObjRstToAnotherObjRst_687(BPMNDBEntities context, IObjRst objRst1, IObjRst objRst2)
        {
            if (objRst1.GetType() == typeof(TblNew))
            {
                ((TblNew)objRst2).FldTtlNews = ((TblNew)objRst1).FldTtlNews;
                ((TblNew)objRst2).FldTxtNews = ((TblNew)objRst1).FldTxtNews;
                if (((TblNew)objRst1).TblWayIfrm_News.Count > 0)
                {
                    List<TblWayIfrm_News> lst = new List<TblWayIfrm_News>();
                    lst.AddRange(((TblNew)objRst1).TblWayIfrm_News);
                    foreach (TblWayIfrm_News item in lst)
                    {
                        ((TblNew)objRst1).TblWayIfrm_News.Remove(item);
                        ((TblNew)objRst2).TblWayIfrm_News.Add(item);
                    }
                    context.DeleteObject(objRst1);
                }
            }
            else if (objRst1.GetType() == typeof(TblSbjOral))
            {
                if (((TblSbjOral)objRst1).TblWayIfrm_Oral.Count > 0)
                {
                    List<TblWayIfrm_Oral> lst = new List<TblWayIfrm_Oral>();
                    lst.AddRange(((TblSbjOral)objRst1).TblWayIfrm_Oral);
                    foreach (TblWayIfrm_Oral item in lst)
                    {
                        ((TblSbjOral)objRst1).TblWayIfrm_Oral.Remove(item);
                        ((TblSbjOral)objRst2).TblWayIfrm_Oral.Add(item);
                    }
                    context.DeleteObject(objRst1);
                }
            }
            else if (objRst1.GetType() == typeof(TblObj))
            {
                ((TblObj)objRst2).FldNamObj = ((TblObj)objRst1).FldNamObj;
                ((TblObj)objRst2).FldTypObj = ((TblObj)objRst1).FldTypObj;
                if (((TblObj)objRst1).TblWayIfrm_SndOut.Count > 0)
                {
                    List<TblWayIfrm_SndOut> lst = new List<TblWayIfrm_SndOut>();
                    lst.AddRange(((TblObj)objRst1).TblWayIfrm_SndOut);
                    foreach (TblWayIfrm_SndOut item in lst)
                    {
                        ((TblObj)objRst1).TblWayIfrm_SndOut.Remove(item);
                        ((TblObj)objRst2).TblWayIfrm_SndOut.Add(item);
                    }
                    context.DeleteObject(objRst1);
                }
            }
        }

        /// <summary>
        /// P709
        /// حذف یک شی نتیجه از یک رخداد نتیجه
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        public static void DeleteObjRstOfEvtRstAndChgPrs_709(BPMNDBEntities context, IObjRst objRst, bool stsExsSrcPelUpl = false)
        {
            TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

            TblPr prsAct1 = DetectPrsOfEvtRst_2329(context, evtRst);

            List<IWayIfrm> wayIfrm = DetectWayIfrmOfObjRst_578(context, objRst);

            //21781
            foreach (IWayIfrm wayIfrm_i in wayIfrm)
            {
                IWayAwr wayAwr1 = DetectWayAwrOfWayIfrm_623(wayIfrm_i);

                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr1);

                TblPr prsAct2 = DetectPrsOfEvtSrt_2347(context, evtSrt);

                TblAct actDst = evtSrt.TblAct;

                List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

                //21778
                if (wayAwr.Count > 1 && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(wayAwr1);
                }

                //21776
                else if (wayAwr.Count == 1 && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt);
                }

                //21774
                if (!stsExsSrcPelUpl)
                {
                    //21775
                    if (prsAct2 == prsAct1)
                    {
                        EditPrsActsOfPrsAfterDelete_8820(context, prsAct1);
                    }
                }

                //21773
                if (prsAct1 != prsAct2 && prsAct2 != null)
                {
                    EditPrsActsOfPrsAfterAdd_8830(context, prsAct1, actDst, prsAct2);
                }
            }

            context.DeleteObject(objRst);
        }

        /// <summary>
        /// F726
        /// حذف نحوه های آگاه سازی یک شی نتیجه 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        public static void DeleteWayIfrmOfObjRst_726(BPMNDBEntities context, IObjRst objRst)
        {
            List<IWayIfrm> wayIfrm = DetectWayIfrmOfObjRst_578(context, objRst);

            foreach (IWayIfrm item in wayIfrm)
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayIfrm_609(item);

                //14970
                if (evtSrt != null)
                {
                    List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

                    //732
                    if (wayAwr.Count == 1)
                    {
                        context.DeleteObject(evtSrt);
                    }
                }

                context.DeleteObject(item);
            }

        }

        /// <summary>
        /// F549
        /// افزودن یک شی نتیجه موجود به یک رخداد نتیجه (شی نتیجه جدیدی که باید اطلاعات شی نتیجه موجود به آن منتقل شود مشخص است)
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="objRst">شی نتیجه ای که می خواهیم به رخداد نتیجه بیفزاییم</param>
        /// <param name="evtRst">رخداد نتیجه ای که می خواهیم شی نتیجه را به آن بیفزاییم</param>
        /// <param name="objRst2">شی نتیجه جدیدی که می خواهیم اطلاعات شی نتیجه موجود به آن منتقل شود</param>
        public static void AddExistingObjRstToEvtRstAndChgPrs_549(BPMNDBEntities context, IObjRst objRst, TblEvtRst evtRst, IObjRst objRst2, List<IWayIfrm> wayIfrm3)
        {
            TblAct act = PublicMethods.DetectActSrcOfObjRst_551(objRst);

            TblNod nod = evtRst.TblAct.TblNod;

            TblAct act1 = PublicMethods.DetectActUspfNod_565(context, nod);

            TblEvtRst evtRst1 = PublicMethods.DetectEvtRstOfObjRst_562(objRst);

            List<IObjRst> objRst1 = PublicMethods.DetectObjRstOfEvtRst_572(context, evtRst1);

            TblPr prsAct = DetectPrsOfEvtRst_2329(context, evtRst);

            //661
            if (act == act1)
            {
                PublicMethods.CopyAllAttributesOfObjRstToAnotherObjRst_687(context, objRst, objRst2);

                //662
                if (objRst1.Count == 1)
                {
                    context.DeleteObject(evtRst1);
                }

                List<IWayIfrm> wayIfrm = PublicMethods.DetectWayIfrmOfObjRst_578(context, objRst2);

                Model.Enum.EvtSrtType typEvtSrt = PublicMethods.DetectTypEvtSrtEqualToTypEvtRst_594(evtRst.FldTypEvtRst);

                //664
                foreach (IWayIfrm wayIfrm_i in wayIfrm)
                {
                    TblEvtSrt evtSrt = PublicMethods.DetectEvtSrtOfWayIfrm_609(wayIfrm_i);

                    List<IWayAwr> wayAwr = PublicMethods.DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

                    TblAct actOth = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                    //665
                    if (wayAwr.Count > 1)
                    {
                        //6758
                        TblEvtSrt evtSrt1 = new TblEvtSrt() { FldTypEvtSrt = (int)typEvtSrt };

                        //6759
                        TblAct act2 = evtSrt.TblAct;

                        //474
                        act2.TblEvtSrts.Add(evtSrt1);

                        IWayAwr wayAwr1 = PublicMethods.DetectWayAwrOfWayIfrm_623(wayIfrm_i);

                        //667
                        context.DeleteObject(wayAwr1);

                        //1473, 1472, 1471
                        IWayAwr wayAwr2;

                        if (objRst.GetType() == typeof(TblNew))
                        {
                            wayAwr2 = new TblWayAwr_News();
                        }

                        else if (objRst.GetType() == typeof(TblSbjOral))
                        {
                            wayAwr2 = new TblWayAwr_Oral();
                        }

                        else
                        {
                            wayAwr2 = new TblWayAwr_RecvInt();
                        }

                        PublicMethods.AddWayAwrToEvtSrt_624(evtSrt1, wayAwr2);

                        PublicMethods.AddWayAwrToWayIfrm_722(context, wayIfrm_i, wayAwr2);
                    }

                        //8853
                    else if (wayAwr.Count == 1)
                    {
                        IWayAwr wayAwr1 = PublicMethods.DetectWayAwrOfWayIfrm_623(wayIfrm_i);

                        //8858
                        context.DeleteObject(wayAwr1);

                        //1473, 1472, 1471
                        IWayAwr wayAwr2;

                        if (objRst.GetType() == typeof(TblNew))
                        {
                            wayAwr2 = new TblWayAwr_News();
                        }

                        else if (objRst.GetType() == typeof(TblSbjOral))
                        {
                            wayAwr2 = new TblWayAwr_Oral();
                        }

                        else
                        {
                            wayAwr2 = new TblWayAwr_RecvInt();
                        }

                        PublicMethods.AddWayAwrToEvtSrt_624(evtSrt, wayAwr2);



                        PublicMethods.AddWayAwrToWayIfrm_722(context, wayIfrm_i, wayAwr2);


                        evtSrt.PreviousActivity = evtSrt.GetPrevAct();
                        evtSrt.ChangeGrpBasedOnPrevAct();

                        //evtSrt.FldGrpEvt = TblAct.GetNewEvtSrtGroupID(evtSrt.TblAct);


                        //dynamic d = wayAwr2;
                        ////19065
                        //d.FldTypDson = null;
                        ////19200
                        //d.FldStsDsonWayAwr = null;
                    }

                    ((dynamic)wayIfrm_i).FldTypDson = null;

                    ((dynamic)wayIfrm_i).FldStsDsonWayIfrm = null;

                    ((dynamic)wayIfrm_i.WayAwr).FldTypDson = null;

                    ((dynamic)wayIfrm_i.WayAwr).FldStsDsonWayAwr = null;

                    //14974
                    if (objRst.GetType() != typeof(TblNew))
                    {
                        TblPr prsActOth = DetectPrsOfAct_1839(context, actOth);

                        EditPrsActsOfPrsAfterAdd_8830(context, prsAct, actOth, prsActOth);
                    }

                }
            }

                //669
            else
            {
                PublicMethods.CopyObjRstToAnotherObjRst_694(objRst, objRst2);

                //List<TblEvtSrt> evtSrt = PublicMethods.DetectEvtsrtOfObjRst_647(context, objRst);

                //19038
                List<TblEvtSrt> evtSrt = new List<TblEvtSrt>();
                foreach (IWayIfrm wayIfrm_i in wayIfrm3)
                {
                    evtSrt.Add(DetectEvtSrtOfWayIfrm_609(wayIfrm_i));
                }

                Model.Enum.EvtSrtType typEvtSrt = PublicMethods.DetectTypEvtSrtEqualToTypEvtRst_594(evtRst.FldTypEvtRst);

                //670
                foreach (TblEvtSrt item in evtSrt)
                {
                    TblAct actOth = item.TblAct;

                    TblEvtSrt evtSrt1 = new TblEvtSrt() { FldTypEvtSrt = (int)typEvtSrt };

                    actOth.TblEvtSrts.Add(evtSrt1);

                    //1477, 1476, 1475
                    IWayIfrm wayIfrm;
                    IWayAwr wayAwr;
                    if (objRst.GetType() == typeof(TblNew))
                    {
                        wayIfrm = new TblWayIfrm_News();
                        wayAwr = new TblWayAwr_News();
                    }
                    else if (objRst.GetType() == typeof(TblSbjOral))
                    {
                        wayIfrm = new TblWayIfrm_Oral();
                        wayAwr = new TblWayAwr_Oral();
                    }
                    else
                    {
                        wayIfrm = new TblWayIfrm_SndOut();
                        wayAwr = new TblWayAwr_RecvInt();
                    }

                    PublicMethods.AddWayAwrToEvtSrt_624(evtSrt1, wayAwr);

                    AddWayAwrToWayIfrm_722(context, wayIfrm, wayAwr);

                    PublicMethods.ConnectObjRstToWayIfrm_660(objRst2, wayIfrm);

                    //14975
                    if (objRst.GetType() != typeof(TblNew))
                    {
                        TblPr prsActOth = DetectPrsOfAct_1839(context, actOth);

                        EditPrsActsOfPrsAfterAdd_8830(context, prsAct, actOth, prsActOth);
                    }
                }
            }
        }

        /// <summary>
        /// P790
        /// افزودن یک نحوه اگاه سازی به یک شی نتیجه با مقصد یک فعالیت
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <param name="objRst"></param>
        /// <param name="act"></param>
        public static void AddWayIfrmToObjRstAimAtAct_790(BPMNDBEntities context, IWayIfrm wayIfrm, IObjRst objRst, TblAct act)
        {
            AddWayIfrmToObjRst_757(wayIfrm, objRst);

            TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

            Model.Enum.EvtSrtType typEvtsrt = DetectTypEvtSrtEqualToTypEvtRst_594(evtRst.FldTypEvtRst);

            TblEvtSrt evtSrt = new TblEvtSrt() { FldTypEvtSrt = (int)typEvtsrt };

            act.TblEvtSrts.Add(evtSrt);

            IWayAwr wayAwr;

            if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            {
                wayAwr = new TblWayAwr_News() { FldDsc = ((TblWayIfrm_News)wayIfrm).FldDsc };
            }
            else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            {
                wayAwr = new TblWayAwr_Oral() { FldTypAwr = ((TblWayIfrm_Oral)wayIfrm).FldTypIfrm };
            }
            else
            {
                wayAwr = new TblWayAwr_RecvInt() { FldWayRecv = ((TblWayIfrm_SndOut)wayIfrm).FldWaySnd, FldCodSfw = ((TblWayIfrm_SndOut)wayIfrm).FldCodSfw, FldCodCmrIntPerRecv = ((TblWayIfrm_SndOut)wayIfrm).FldCodCmrOutPerSnd, FldTnoIntPerRecv = ((TblWayIfrm_SndOut)wayIfrm).FldTnoOutPerSnd, FldCodUntMsrtInt = ((TblWayIfrm_SndOut)wayIfrm).FldCodUntMsrtOut, FldIntNeedPrsg = 1 };
            }

            AddWayAwrToEvtSrt_624(evtSrt, wayAwr);

            AddWayAwrToWayIfrm_722(context, wayIfrm, wayAwr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayIfrm"></param>
        /// <param name="objRst"></param>
        /// <param name="act"></param>
        public static void AddWayIfrmToObjRstAimAtActAndChgPrs_3435(BPMNDBEntities context, IWayIfrm wayIfrm, IObjRst objRst, TblAct act)
        {
            AddWayIfrmToObjRstAimAtAct_790(context, wayIfrm, objRst, act);

            TblEvtRst evtRst = DetectEvtRstOfObjRst_562(objRst);

            TblPr prsAct1 = DetectPrsOfEvtRst_2329(context, evtRst);

            TblPr prsAct2 = DetectPrsOfAct_1839(context, act);

            EditPrsActsOfPrsAfterAdd_8830(context, prsAct1, act, prsAct2);
        }

        /// <summary>
        /// F1520
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct_Sfw> DetectSfwOfAct_1520(BPMNDBEntities context, TblAct act)
        {
            ReloadEntity(context, act, act.TblAct_Sfw, "TblAct_Sfw");

            return new List<TblAct_Sfw>(act.TblAct_Sfw.Where(m => m.FldTypUseSfw == (int)Enum.ActivitySoftwareTypes.ActivitySoftware));
        }

        /// <summary>
        /// F1521
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct_Sfw> DetectInputSfwOfAct_1521(BPMNDBEntities context, TblAct act)
        {
            ReloadEntity(context, act, act.TblAct_Sfw, "TblAct_Sfw");

            return new List<TblAct_Sfw>(act.TblAct_Sfw.Where(m => m.FldTypUseSfw == (int)Enum.ActivitySoftwareTypes.InputActivitySoftware));
        }

        /// <summary>
        /// F1522
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct_Sfw> DetectOutputSfwOfAct_1522(BPMNDBEntities context, TblAct act)
        {
            ReloadEntity(context, act, act.TblAct_Sfw, "TblAct_Sfw");
            return new List<TblAct_Sfw>(act.TblAct_Sfw.Where(m => m.FldTypUseSfw == (int)Enum.ActivitySoftwareTypes.OutputActivitySoftware));
        }

        /// <summary>
        /// F1570
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<TblPr> DetectAllPrs_1570(BPMNDBEntities context)
        {



            //#if DEBUG

            //#else       

            //#endif


            DeleteAllWayIfrmWotWayAwr(context);

            DeleteAllEvtSrtWotWayAwr_21737(context);

            DeleteAllPrsWotAct_19381(context);

            //1572
            //ReloadEntity(context, context.TblPrs);



            //1571
            List<TblPr> lst = new List<TblPr>();
            List<TblPr> lst2 = new List<TblPr>();
            //int nodId = PublicMethods.CurrentUser.TblOrg.Nod.FldCodNod;
            //var query = context.TblPrs.Join(context.TblActPrs, p => p.FldCodPrs, ap => ap.FldCodPrs, (p, ap) => new { prs = p, actPrs = ap })
            //    .Join(context.TblActs, prs_act => prs_act.actPrs.FldCodAct, act => act.FldCodAct, (prs_act, act) => new { prs2 = prs_act.prs, act2 = act })
            //    .GroupBy(act3 => act3.act2.FldCodNod).OrderByDescending(g => g.Count()).FirstOrDefault();

            //if (query != null && query.Key == nodId)
            //{
            //    lst = query.Select(p => p.prs2).ToList();
            //}


            lst2 = context.TblPrs.ToList();

            //List<TblNod> nod = PublicMethods.CurrentUser.TblOrg.AllNodesOfCurrentOrg.ToList();

            //lst2 = lst.Where(a => nod.Where(x => x.FldCodNod == a.TblActPrs.GroupBy(b => b.TblAct.FldCodNod).OrderByDescending(c => c.Count()).FirstOrDefault().Key).Count() > 0).ToList();


            foreach (TblPr item in lst2)
            {
                TblOrg org = item.Org;
                if (org != null && org.FldCodOrg == CurrentUser.TblOrg.FldCodOrg)
                {
                    lst.Add(item);
                }

            }

            return lst;

            //#if DEBUG

            //            return lst2;

            //#else       


            //#endif

        }

        /// <summary>
        /// 1590
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static double ComputeVluVotNodForNamPrpsPrs_1590(BPMNDBEntities context, TblPr prs, TblNod nod)
        {
            //1606
            double i = ComputeTnoActOfNodInnPrs_1606(context, prs, nod);

            //1592
            return i / prs.TblActPrs.Count * 100;
        }

        /// <summary>
        /// 1593
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namPrpsPrs"></param>
        /// <returns></returns>
        public static double ComputeWeightVluNamPrpsPrs_1593(BPMNDBEntities context, TblNamPrpsPr namPrpsPrs)
        {
            //1594
            ReloadEntity(context, namPrpsPrs, namPrpsPrs.TblVotNamPrpsPrs, "TblVotNamPrpsPrs");

            List<TblVotNamPrpsPr> lst = namPrpsPrs.TblVotNamPrpsPrs.ToList<TblVotNamPrpsPr>();

            //1597
            double i = 0;

            //1596
            foreach (TblVotNamPrpsPr item in lst)
            {
                //1590
                double j = ComputeVluVotNodForNamPrpsPrs_1590(context, namPrpsPrs.TblPr, item.TblNod);

                //1595
                if (item.FldVot == (int)Enum.TypVot.Agree)
                {
                    i += j;
                }
                else if (item.FldVot == (int)Enum.TypVot.DisAgree)
                {
                    i -= j;
                }
            }

            return i;
        }

        /// <summary>
        /// 1593
        /// </summary>
        /// <param name="context"></param>
        /// <param name="owrPrpsPr"></param>
        /// <returns></returns>
        public static double ComputeWeightVluOwrPrpsPrs_1624(BPMNDBEntities context, TblOwrPrpsPr owrPrpsPr)
        {
            //1625
            ReloadEntity(context, owrPrpsPr, owrPrpsPr.TblVotOwrPrps, "TblVotOwrPrps");
            List<TblVotOwrPrp> lst = owrPrpsPr.TblVotOwrPrps.ToList<TblVotOwrPrp>();

            //1626
            double i = 0;

            //1627
            foreach (TblVotOwrPrp item in lst)
            {
                //1590
                double j = ComputeVluVotNodForNamPrpsPrs_1590(context, owrPrpsPr.TblPr, item.TblNod);

                //1630
                if (item.FldVot == (int)Enum.TypVot.Agree)
                {
                    i += j;
                }
                else if (item.FldVot == (int)Enum.TypVot.DisAgree)
                {
                    i -= j;
                }
            }

            return i;
        }

        /// <summary>
        /// 1601
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static List<TblNamPrpsPr> DetectAllNamPrpsPrs_1601(BPMNDBEntities context, TblPr prs)
        {
            ReloadEntity(context, prs, prs.TblNamPrpsPrs, "TblNamPrpsPrs");

            return prs.TblNamPrpsPrs.ToList<TblNamPrpsPr>();
        }

        /// <summary>
        /// 1606
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static int ComputeTnoActOfNodInnPrs_1606(BPMNDBEntities context, TblPr prs, TblNod nod)
        {
            ReloadEntity(context, prs, prs.TblActPrs, "TblActPrs");

            ReloadEntity(context, nod, nod.TblActs, "TblActs");

            int i = 0;

            foreach (TblActPr item in prs.TblActPrs)
            {
                if (nod.TblActs.Contains(item.TblAct))
                {
                    i++;
                }
            }

            return i;
        }

        /// <summary>
        /// F1607
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> DetectPosPstOrg_1607(BPMNDBEntities context, TblOrg org)
        {
            ReloadEntity(context, org, org.TblPosPstOrgs, "TblPosPstOrgs");

            return org.TblPosPstOrgs.Where(m => m.FldCodUpl == null).ToList<TblPosPstOrg>();
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که شخص داده شده نماینده آنهاست
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> DetectPosPstWithAgntOfPsn_22179(BPMNDBEntities context, TblPsn psn)
        {
            TblPsn psn1 = context.TblPsns.Single(m => m.FldCodPsn == psn.FldCodPsn);

            List<TblPosPstOrg> result = new List<TblPosPstOrg>();

            foreach (var agnt in psn1.TblAgntNods)
            {
                if (agnt.TblNod.EtyNod is TblPosPstOrg)
                {
                    result.Add(agnt.TblNod.EtyNod as TblPosPstOrg);
                }
            }

            return result;
        }

        /// <summary>
        /// شناسایی نقش هایی که شخص داده شده نماینده آنهاست
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <returns></returns>
        public static List<TblRol> DetectRolWithAgntOfPsn_22180(BPMNDBEntities context, TblPsn psn)
        {
            TblPsn psn1 = context.TblPsns.Single(m => m.FldCodPsn == psn.FldCodPsn);

            List<TblRol> result = new List<TblRol>();

            foreach (var agnt in psn1.TblAgntNods)
            {
                if (agnt.TblNod.EtyNod is TblRol)
                {
                    result.Add(agnt.TblNod.EtyNod as TblRol);
                }
            }

            return result;
        }


        /// <summary>
        /// شناسایی نقش هایی که در یک فرایند مشخص شرکت کرده اند
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static List<TblRol> DetectRolInPrs_22182(BPMNDBEntities context, TblPr prs)
        {
            List<TblRol> result = new List<TblRol>();
            List<TblAct> acts = DetectActsOfPrs_946(context, prs);
            foreach (var act in acts)
            {
                if (act.TblNod.FldCodTypEty == (int)Enum.FldTypEty.Rol)
                {
                    if (!result.Any(m => m.FldCodRol == act.TblNod.FldCodEty))
                    {
                        result.Add(act.TblNod.EtyNod as TblRol);
                    }
                }
            }

            return result;
        }



        /// <summary>
        /// شناسایی جایگاه و سمت هایی که در یک فرایند مشخص شرکت کرده اند
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> DetectPosPstInPrs_22181(BPMNDBEntities context, TblPr prs)
        {
            List<TblPosPstOrg> result = new List<TblPosPstOrg>();
            List<TblAct> acts = DetectActsOfPrs_946(context, prs);
            foreach (var act in acts)
            {
                if (act.TblNod.FldCodTypEty == (int)Enum.FldTypEty.PosPst)
                {
                    if (!result.Any(m => m.FldCodPosPst == act.TblNod.FldCodEty))
                    {
                        result.Add(act.TblNod.EtyNod as TblPosPstOrg);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// F1619
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namPrpsPr"></param>
        /// <returns></returns>
        public static List<TblVotNamPrpsPr> DetectVotToOneNamPrpsPrs_1619(BPMNDBEntities context, TblNamPrpsPr namPrpsPr)
        {
            ReloadEntity(context, namPrpsPr, namPrpsPr.TblVotNamPrpsPrs, "TblVotNamPrpsPrs");

            return namPrpsPr.TblVotNamPrpsPrs.ToList<TblVotNamPrpsPr>();
        }

        /// <summary>
        /// F1800
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namPrpsPr"></param>
        /// <returns></returns>
        public static List<TblVotOwrPrp> DetectVotToOneOwrPrpsPrs_1800(BPMNDBEntities context, TblOwrPrpsPr owrPrpsPr)
        {
            ReloadEntity(context, owrPrpsPr, owrPrpsPr.TblVotOwrPrps, "TblVotOwrPrps");

            return owrPrpsPr.TblVotOwrPrps.ToList<TblVotOwrPrp>();
        }

        /// <summary>
        /// F753
        /// </summary>
        /// <param name="context"></param>
        /// <param name="posPst"></param>
        /// <returns></returns>
        public static TblNod DetectNodOfPosPst_753(BPMNDBEntities context, TblPosPstOrg posPst)
        {
            TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst && m.FldCodEty == posPst.FldCodPosPst);

            return nod;
        }

        /// <summary>
        /// گرهی که از نوع سازمان باشد و دارای کدی معادل کد سازمان مورد نظر باشد را شناسایی کن و به خروجی ببر
        /// F1738
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static TblNod DetectNodOfOrg_1738(BPMNDBEntities context, TblOrg org)
        {
            TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodTypEty == (int)Model.Enum.FldTypEty.Org && m.FldCodEty == org.FldCodOrg);

            return nod;
        }

        /// <summary>
        /// گرهی که از نوع شخص  باشد و دارای کدی معادل کد شخص  مورد نظر باشد را شناسایی کن و به خروجی ببر
        /// F1740
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static TblNod DetectNodOfPsn_1740(BPMNDBEntities context, TblPsn psn)
        {
            TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodTypEty == (int)Model.Enum.FldTypEty.Psn && m.FldCodEty == psn.FldCodPsn);

            return nod;
        }

        /// <summary>
        /// گرهی که از نوع نقش   باشد و دارای کدی معادل کد نقش    مورد نظر باشد را شناسایی کن و به خروجی ببر
        /// F1742
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static TblNod DetectNodOfRol_1742(BPMNDBEntities context, TblRol rol)
        {
            TblNod nod = context.TblNods.SingleOrDefault(m => m.FldCodTypEty == (int)Model.Enum.FldTypEty.Rol && m.FldCodEty == rol.FldCodRol);

            return nod;
        }

        /// <summary>
        /// F1754
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<TblPsn> DetectPsnByName_1754(BPMNDBEntities context, string name)
        {
            return context.TblPsns.Where(m => m.FldNam2ndPsn.ToLower().Contains(name.Trim().ToLower())).ToList();
        }

        /// <summary>
        /// F1745
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<TblOrg> DetectOrgByName_17545(BPMNDBEntities context, string name)
        {
            return context.TblOrgs.Where(m => m.FldNamOrg.ToLower().Contains(name.Trim().ToLower())).ToList();
        }

        /// <summary>
        /// F1748
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> DetectPosPstByName_1748(BPMNDBEntities context, string name)
        {
            return context.TblPosPstOrgs.Where(m => m.FldNamPosPst.ToLower().Contains(name.Trim().ToLower())).ToList();
        }

        /// <summary>
        /// F1758
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<TblRol> DetectRolByName(BPMNDBEntities context, string name)
        {
            return context.TblRols.Where(m => m.FldTtlRol.ToLower().Contains(name.Trim().ToLower())).ToList();
        }

        /// <summary>
        /// F1737
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namNod"></param>
        /// <returns></returns>
        public static List<TblNod> DetectNodByName_1737(BPMNDBEntities context, string namNod)
        {
            List<TblNod> nods = new List<TblNod>();

            List<TblRol> rols = DetectRolByName(context, namNod);
            foreach (TblRol rol in rols)
            {
                nods.Add(DetectNodOfRol_1742(context, rol));
            }

            List<TblPsn> psns = DetectPsnByName_1754(context, namNod);
            foreach (var psn in psns)
            {
                nods.Add(DetectNodOfPsn_1740(context, psn));
            }

            List<TblPosPstOrg> posPsts = DetectPosPstByName_1748(context, namNod);
            foreach (var posPst in posPsts)
            {
                nods.Add(DetectNodOfPosPst_753(context, posPst));
            }

            List<TblOrg> orgs = DetectOrgByName_17545(context, namNod);
            foreach (var org in orgs)
            {
                nods.Add(DetectNodOfOrg_1738(context, org));
            }

            return nods;
        }

        /// <summary>
        /// F1768
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nods"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActsOfNods_1768(BPMNDBEntities context, List<TblNod> nods)
        {
            List<TblAct> acts = new List<TblAct>();

            foreach (var nod in nods)
            {
                acts.AddRange(nod.TblActs);
            }

            return acts;
        }

        /// <summary>
        /// F1736
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nameNod"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByNod_1736(BPMNDBEntities context, string nameNod)
        {
            List<TblNod> nods = DetectNodByName_1737(context, nameNod);

            List<TblAct> acts = DetectActsOfNods_1768(context, nods);

            return DetectPrsByActs_1774(context, acts);
        }

        /// <summary>
        /// F1774
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nods"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByActs_1774(BPMNDBEntities context, List<TblAct> acts)
        {
            List<TblPr> prs = new List<TblPr>();

            foreach (var act in acts)
            {
                foreach (var actPrs in act.TblActPrs)
                {
                    prs.Add(actPrs.TblPr);
                }
            }

            return prs;
        }

        /// <summary>
        /// F1777
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namPrs"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByName_1777(BPMNDBEntities context, string namPrs)
        {
            return context.TblPrs.Where(m => m.FldNamPrs.ToLower().Contains(namPrs.Trim().ToLower())).ToList().Where(p => p.TblNod.EtyNod.Org.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList();
        }

        /// <summary>
        /// F1779
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namObj"></param>
        /// <returns></returns>
        public static List<TblObj> DetectObjByName_1779(BPMNDBEntities context, string namObj)
        {
            return context.TblObjs.Where(m => m.FldNamObj.Trim().ToLower().Contains(namObj.Trim().ToLower())).ToList();
        }

        /// <summary>
        /// F1778
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblAct DetectActByObj_1778(TblObj obj)
        {
            return obj.TblEvtRst.TblAct;
        }

        /// <summary>
        /// F1778
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namObj"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByObjName_1778(BPMNDBEntities context, string namObj)
        {
            List<TblObj> objs = DetectObjByName_1779(context, namObj);

            List<TblAct> acts = new List<TblAct>();
            foreach (var obj in objs)
            {
                acts.Add(DetectActByObj_1778(obj));
            }

            return DetectPrsByActs_1774(context, acts);
        }

        /// <summary>
        /// F1782
        /// </summary>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByNewsName_1782(BPMNDBEntities context, string namNews)
        {
            //1783
            List<TblNew> news = context.TblNews.Where(m => m.FldTxtNews.Trim().ToLower().Contains(namNews.Trim().ToLower())).ToList();

            List<TblAct> acts = new List<TblAct>();

            //1784
            foreach (var n in news)
            {
                //1785
                acts.Add(n.TblEvtRst.TblAct);
            }

            return DetectPrsByActs_1774(context, acts);
        }

        /// <summary>
        /// F1786
        /// </summary>
        /// <param name="context"></param>
        /// <param name="namAct"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActByName_1786(BPMNDBEntities context, string namAct)
        {
            return context.TblActs.Where(m => m.FldNamAct.Trim().ToLower().Contains(namAct.Trim().ToLower())).ToList().Where(a => a.TblNod.EtyNod.Org.FldCodOrg == CurrentUser.TblOrg.FldCodOrg).ToList();
        }

        /// <summary>
        /// F1885
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByTypEvtSrt_1885(BPMNDBEntities context, EvtSrtType type)
        {
            List<TblEvtSrt> evtSrts = context.TblEvtSrts.Where(m => m.FldTypEvtSrt == (int)type).ToList();

            List<TblAct> acts = new List<TblAct>();

            foreach (var evtSrt in evtSrts)
            {
                acts.Add(evtSrt.TblAct);
            }

            return DetectPrsByActs_1774(context, acts);
        }

        /// <summary>
        /// F1886
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsByTypEvtRst_1886(BPMNDBEntities context, EvtRstType type)
        {
            List<TblEvtRst> evtRsts = context.TblEvtRsts.Where(m => m.FldTypEvtRst == (int)type).ToList();

            List<TblAct> acts = new List<TblAct>();

            foreach (var evtRst in evtRsts)
            {
                acts.Add(evtRst.TblAct);
            }

            return DetectPrsByActs_1774(context, acts);

        }

        /// <summary>
        /// F454
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="evtRstTyp"></param>
        /// <returns></returns>
        public static List<TblEvtRst> DetectEvtRstOfActWthSpcTyp_454(BPMNDBEntities context, TblAct act, Model.Enum.EvtRstType evtRstTyp)
        {
            ReloadEntity(context, act, act.TblEvtRsts, "TblEvtRsts");

            return act.TblEvtRsts.Where(m => m.FldTypEvtRst == (int)evtRstTyp).ToList<TblEvtRst>();
        }

        /// <summary>
        /// F752
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static List<TblEror> DetectErorOfEvtRst_752(BPMNDBEntities context, TblEvtRst evtRst)
        {
            ReloadEntity(context, evtRst, evtRst.TblErors, "TblErors");

            return evtRst.TblErors.ToList<TblEror>();
        }

        /// <summary>
        /// F2072
        /// شناسایی سازمان های زیر مجموعه یک سازمان خاص
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <param name="org1"></param>
        public static void DetectSubOrgOfOrg_2072(TblOrg org, List<TblOrg> org1)
        {
            org1.Add(org);
            foreach (Model.TblOrg item in org.TblOrg1)
            {
                DetectSubOrgOfOrg_2072(item, org1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblOrg> DetectOrgNotSubOrgOfOrg_2073(BPMNDBEntities context, TblOrg org)
        {
            ReloadEntity(context, context.TblOrgs);

            List<TblOrg> lstOut = new List<TblOrg>();

            List<TblOrg> lstSubOrg = new List<TblOrg>();

            List<TblOrg> lstAllOrg = context.TblOrgs.ToList();

            DetectSubOrgOfOrg_2072(org, lstSubOrg);

            foreach (TblOrg item in lstAllOrg)
            {
                if (!lstSubOrg.Contains(item))
                {
                    lstOut.Add(item);
                }
            }

            return lstOut;
        }

        /// <summary>
        /// F2074
        /// افزودن گره به سازمان هایی که برای آن ها گره تعریف نشده است
        /// </summary>
        /// <param name="context"></param>
        public static void AddNodToOrgs_2074(BPMNDBEntities context)
        {
            foreach (TblOrg item in context.TblOrgs)
            {
                if (context.TblNods.Where(E => E.FldCodTypEty == 1 && E.FldCodEty == item.FldCodOrg).Count() == 0)
                {
                    Model.TblNod tbl = new TblNod() { FldCodEty = item.FldCodOrg, FldCodTypEty = 1 };
                    context.TblNods.AddObject(tbl);
                }
            }
        }

        /// <summary>
        /// F2075
        /// افزودن فعالیت نامشخص به سازمانهایی که دارای فعالیت نامشخص نیستند
        /// </summary>
        /// <param name="context"></param>
        public static void AddActUspfToOrgs_2075(BPMNDBEntities context)
        {
            List<TblNod> nod = context.TblNods.Where(E => E.TblActs.Where(M => M.FldActUspf == true).Count() == 0).ToList();
            foreach (TblNod item in nod)
            {
                TblAct tbl = new TblAct() { FldNamAct = "فعالیت نامشخص", FldTypAct = (int)Model.Enum.ActivityTypes.Manual, FldActUspf = true };
                item.TblActs.Add(tbl);
            }
        }

        /// <summary>
        /// F2191
        /// شناسایی یک جایگاه یا سمت سازمانی به همراه تمامی زیر مجموعه های آن
        /// </summary>
        /// <param name="posPst"></param>
        /// <param name="lstPosPst"></param>
        public static void DetectSubPosPst_2191(TblPosPstOrg posPst, List<TblPosPstOrg> lstPosPst)
        {
            lstPosPst.Add(posPst);
            foreach (TblPosPstOrg item in posPst.TblPosPstOrg1)
            {
                DetectSubPosPst_2191(item, lstPosPst);
            }
        }

        /// <summary>
        /// F2190
        /// بررسی قابل حذف بودن یا نبودن یک جایگاه یا سمت سازمانی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="posPst"></param>
        /// <returns></returns>
        public static bool IsPosPstDeletable_2190(BPMNDBEntities context, TblPosPstOrg posPst)
        {
            //2191
            List<TblPosPstOrg> lstPosPst = new List<TblPosPstOrg>();
            DetectSubPosPst_2191(posPst, lstPosPst);

            //2192
            foreach (TblPosPstOrg item in lstPosPst)
            {
                TblNod nod = DetectNodOfPosPst_753(context, item);

                //2212
                if (!TryDeleteNod_2076(context, nod))
                {
                    return false;
                }
            }

            //2218
            return true;
        }

        /// <summary>
        /// F2221
        /// حذف یک جایگاه یا سمت سازمانی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="posPst"></param>
        /// <returns></returns>
        public static bool DeletePosPst_2221(BPMNDBEntities context, TblPosPstOrg posPst)
        {
            //2224
            if (IsPosPstDeletable_2190(context, posPst))
            {
                //2191
                List<TblPosPstOrg> lstPosPst = new List<TblPosPstOrg>();
                DetectSubPosPst_2191(posPst, lstPosPst);

                foreach (TblPosPstOrg item in lstPosPst)
                {
                    TblNod nod = DetectNodOfPosPst_753(context, item);

                    DeleteNod_2194(context, nod);

                    //2223
                    context.DeleteObject(item);
                }

                //2225
                return true;
            }

            //2226
            return false;
        }

        /// <summary>
        /// F2242
        /// بررسی قابل حذف بودن یا نبودن یک نقش
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rol"></param>
        /// <returns></returns>
        public static bool IsRolDeletable_2242(BPMNDBEntities context, TblRol rol)
        {
            TblNod nod = DetectNodOfRol_1742(context, rol);

            if (nod == null)
            {
                return true;
            }

            return TryDeleteNod_2076(context, nod);
        }

        /// <summary>
        /// F2230
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static bool IsOrgDeletable_2230(BPMNDBEntities context, TblOrg org)
        {
            //2072
            List<TblOrg> org1 = new List<TblOrg>();
            DetectSubOrgOfOrg_2072(org, org1);

            //2249
            foreach (TblOrg org1_i in org1)
            {
                TblNod nod = DetectNodOfOrg_1738(context, org1_i);

                //2250 
                if (!TryDeleteNod_2076(context, nod))
                {
                    //2251
                    return false;
                }

                //1607
                List<TblPosPstOrg> posPos = DetectPosPstOrg_1607(context, org1_i);

                //2235
                foreach (TblPosPstOrg item1 in posPos)
                {
                    //2237
                    if (!IsPosPstDeletable_2190(context, item1))
                    {
                        //2238
                        return false;
                    }
                }

                List<TblRol> rol = DetectRolOfOrg_2243(context, org1_i);

                //2244
                foreach (TblRol rol_i in rol)
                {
                    //2245
                    if (!IsRolDeletable_2242(context, rol_i))
                    {
                        //2246
                        return false;
                    }
                }

                //2240
                if (org1_i.TblIdxes.Count > 0)
                {
                    return false;
                }

                //2247
                if (DetectUsrsOfOrg_2253(context, org1_i).Count(u => u.FldNamUsrActv) > 0)
                {
                    return false;
                }
            }

            //2252
            return true;
        }

        /// <summary>
        /// F2253
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblUsr> DetectUsrsOfOrg_2253(BPMNDBEntities context, TblOrg org)
        {
            ReloadEntity(context, org.TblUsrs);

            return org.TblUsrs.ToList();
        }

        /// <summary>
        /// F2243
        /// شناسایی نقش های موجود در یک سازمان
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblRol> DetectRolOfOrg_2243(BPMNDBEntities context, TblOrg org)
        {
            return org.TblRols.ToList();
        }

        /// <summary>
        /// F2261
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static bool DeleteOrg_2261(BPMNDBEntities context, TblOrg org)
        {
            //2262
            if (IsOrgDeletable_2230(context, org))
            {
                //2072
                List<TblOrg> org1 = new List<TblOrg>();
                DetectSubOrgOfOrg_2072(org, org1);

                //2264
                foreach (TblOrg item in org1)
                {
                    TblNod nod = DetectNodOfOrg_1738(context, org);

                    DeleteNod_2194(context, nod);

                    //2263
                    context.DeleteObject(item);

                    //2265
                    return true;
                }
            }

            //2266
            return false;
        }

        /// <summary>
        /// F2267
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static bool DeleteRol_2267(BPMNDBEntities context, TblRol rol)
        {
            //2268
            if (IsRolDeletable_2242(context, rol))
            {

                TblNod nod = DetectNodOfRol_1742(context, rol);

                //19391
                if (nod != null)
                {
                    DeleteNod_2194(context, nod);
                }

                //2269
                context.DeleteObject(rol);

                //2270
                return true;
            }

            //2271
            return false;
        }

        /// <summary>
        /// F2272
        /// </summary>
        /// <param name="context"></param>
        /// <param name="org"></param>
        public static void AddOrg_2272(BPMNDBEntities context, TblOrg org)
        {
            context.TblOrgs.AddObject(org);

            PublicMethods.SaveContext(context);

            TblNod nod = new TblNod() { FldCodEty = org.FldCodOrg, FldCodTypEty = (int)Enum.FldTypEty.Org };

            context.TblNods.AddObject(nod);

            TblAct act = new TblAct() { FldNamAct = "فعالیت نامشخص", FldActUspf = true, FldTypAct = (int)Enum.ActivityTypes.Manual };

            nod.TblActs.Add(act);

            PublicMethods.SaveContext(context);
        }

        /// <summary>
        /// اعمال تغییر فرآیند
        /// </summary>
        /// <param name="context"></param>
        /// <param name="typChg"></param>
        /// <param name="act"></param>
        /// <param name="evtSrt"></param>
        /// <param name="evtRst"></param>
        public static void ImpChgPrs_3463(BPMNDBEntities context,
            Model.Enum.TypChgInnPrs typChg,
            TblAct act = null,
            TblEvtSrt evtSrt = null,
            TblEvtRst evtRst = null,
            TblNod nod = null,
            IWayAwr wayAwr = null,
            TblAct actOth = null,
            IWayAwrIfrm wayIfrm = null,
             TblAct actDstObjRst = null,
            IObjRst objRst = null)
        {
            switch (typChg)
            {
                //3466
                case TypChgInnPrs.DelAct:

                    DeleteActAndChgPrs_3173(context, act);

                    break;

                //3465
                case TypChgInnPrs.AddAct:

                    AddActAndChgPrs_3202(context, act, nod);

                    break;

                //3471
                case TypChgInnPrs.DelEvtSrt:

                    DeleteEvtSrtAndChgPrs_3205(context, evtSrt);

                    break;

                //3470
                case TypChgInnPrs.ChgActSrcOfEvtSrt:

                    DeleteObjRstFromWayAwrAndChgPrs_6668(context, objRst, wayAwr);

                    AddExistingObjRstToWayAwrAndChgPrs_6692(context, objRst, wayAwr);

                    AddNewObjRstToWayAwrAndChgPrs_6724(context, objRst, wayAwr, act);

                    break;

                //3469
                case TypChgInnPrs.DelWayAwrOfEvtSrt:

                    ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(context, evtSrt, wayAwr);

                    break;

                //3480
                case TypChgInnPrs.DelEvtRst:

                    DeleteEvtRstAndChgPrs_3290(context, evtRst);

                    break;

                //3479
                case TypChgInnPrs.AddEvtRst:

                    DeleteObjRstFromWayAwrAndChgPrs_6668(context, objRst, wayAwr);

                    AddExistingObjRstToWayAwrAndChgPrs_6692(context, objRst, wayAwr);

                    AddNewObjRstToWayAwrAndChgPrs_6724(context, objRst, wayAwr, act);

                    break;

                //3478
                case TypChgInnPrs.ChgActDstWayIfrm:

                    DeleteObjRstOfEvtRstAndChgPrs_709(context, objRst);

                    //AddExistingObjRstToEvtRstAndChgPrs_549(context, objRst, evtRst, obj

                    break;

                //3474
                //??????

                //3473
                //??????

                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="actDstObjRst"></param>
        public static void ImpChgPrsAftrChgActSrcOfEvtRstOFtypGainAwrNew_3401(BPMNDBEntities context, IObjRst objRst, TblAct actDstObjRst)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="actDstObjRst"></param>
        public static void ImpChgPrsAftrDelDstOfOut_3392(BPMNDBEntities context, IObjRst objRst, TblAct actDstObjRst)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="actDstObjRst"></param>
        public static void ImpChgPrsAftrAddDstToOut_3367(BPMNDBEntities context, IObjRst objRst, TblAct actDstObjRst)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="actOth"></param>
        /// <param name="wayIfrm"></param>
        public static void ImpChgPrsAftrChgActDstWayIfrm_3340(BPMNDBEntities context, TblAct actOth, IWayIfrm wayIfrm)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <param name="act"></param>
        public static void ImpChgPrsAftrAddEvtRstToAct_3315(BPMNDBEntities context, TblEvtRst evtRst, TblAct act)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DeleteEvtRstAndChgPrs_3290(BPMNDBEntities context, TblEvtRst evtRst)
        {
            TblPr prsAct = DetectPrsOfEvtRst_2329(context, evtRst);

            DeleteEvtRstWthAllRelations_838(context, evtRst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DeleteActAndChgPrs_3173(BPMNDBEntities context, TblAct act)
        {
            TblPr prsAct = DetectPrsOfAct_1839(context, act);

            DeleteAct_806(context, act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs1"></param>
        /// <param name="prs2"></param>
        public static void CopyAllAtrributesOfPrsInPrsOth_21754(BPMNDBEntities context, TblPr prs1, TblPr prs2)
        {
            prs2.FldNamPrs = prs1.FldNamPrs;

            prs2.FldCodOwrPrs = prs1.FldCodOwrPrs;

            prs2.FldSttPrs = prs1.FldSttPrs;

            foreach (TblOwrPrpsPr item in prs1.TblOwrPrpsPrs)
            {
                TblOwrPrpsPr tbl = new TblOwrPrpsPr() { FldCodNodPrpsEer = item.FldCodNodPrpsEer, FldCodOwrPrpsPrs = item.FldCodOwrPrpsPrs };

                prs2.TblOwrPrpsPrs.Add(tbl);

                foreach (TblVotOwrPrp item1 in item.TblVotOwrPrps)
                {
                    TblVotOwrPrp tbl1 = new TblVotOwrPrp() { FldCodNodVotEer = item1.FldCodNodVotEer, FldVot = item1.FldVot };

                    tbl.TblVotOwrPrps.Add(tbl1);
                }
            }

            foreach (TblNamPrpsPr item in prs1.TblNamPrpsPrs)
            {
                TblNamPrpsPr tbl = new TblNamPrpsPr() { FldNamPrpsPrs = item.FldNamPrpsPrs, FldCodNodPrpsEer = item.FldCodNodPrpsEer };

                prs2.TblNamPrpsPrs.Add(tbl);

                foreach (TblVotNamPrpsPr item1 in item.TblVotNamPrpsPrs)
                {
                    TblVotNamPrpsPr tbl1 = new TblVotNamPrpsPr() { FldCodNodVotEer = item1.FldCodNodVotEer, FldVot = item1.FldVot };

                    tbl.TblVotNamPrpsPrs.Add(tbl1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        public static void EditPrsActsOfPrsAfterDelete_8820(BPMNDBEntities context, TblPr prs)
        {
            List<TblAct> act1 = DetectActsOfPrs_946(context, prs);

            //22112
            if (act1.Count == 0)
            {
                DeletePrs_1726(context, prs);
            }

                //22114
            else
            {
                List<IsleAct> isleAct = DetectIsleActOfActs_22064(context, act1);

                //8826
                if (isleAct.Count == 1)
                {
                    //8827
                    if (prs.FldSttPrs != (int)Enum.SttPrs.Raw)
                    {
                        prs.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                    }
                }

                    //8821
                else if (isleAct.Count > 1)
                {
                    //8824
                    if (prs.FldSttPrs == (int)Enum.SttPrs.Raw)
                    {
                        //8825
                        foreach (IsleAct isleAct_i in isleAct)
                        {
                            TblPr prsRaw = AddPrsRaw_1721(context);

                            SetPrsOfActs_1724(context, isleAct_i.Acts, prsRaw);
                        }
                    }

                        //8822
                    else
                    {
                        prs.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;

                        //8823
                        int i = 0;
                        foreach (IsleAct isleAct_i in isleAct)
                        {
                            TblPr sgmtPrs = AddSgmtPrsFromPrsConsolidated_1728(context, prs, i++);

                            SetPrsOfActs_1724(context, isleAct_i.Acts, sgmtPrs);
                        }
                    }

                    DeletePrs_1726(context, prs);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<IsleAct> DetectIsleActOfActs_22064(BPMNDBEntities context, List<TblAct> act)
        {
            List<IsleAct> isleAct = new List<IsleAct>();

            List<TblAct> allAct = new List<TblAct>();

            foreach (TblAct act_i in act)
            {
                List<TblAct> act1 = new List<TblAct>();

                if (!allAct.Contains(act_i))
                {
                    AddSubActs(allAct, act1, act_i);

                    IsleAct isleAct1 = new IsleAct();

                    isleAct1.Acts = act1;

                    isleAct.Add(isleAct1);
                }
            }

            return isleAct;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allAct"></param>
        /// <param name="act1"></param>
        /// <param name="act2"></param>
        private static void AddSubActs(List<TblAct> allAct, List<TblAct> act1, TblAct act2)
        {
            if (!act1.Contains(act2))
            {
                act1.Add(act2);

                allAct.Add(act2);
            }

            foreach (TblAct item in act2.ActDstForIsleAct)
            {
                if (!act1.Contains(item))
                {
                    AddSubActs(allAct, act1, item);
                }
            }

            foreach (TblAct item in act2.ActSrcForIsleAct)
            {
                if (!act1.Contains(item))
                {
                    AddSubActs(allAct, act1, item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DetectIsleActOfActs_3069(BPMNDBEntities context, List<TblAct> act, List<TblPr> isleAct, List<TblAct> actIsle)
        {
            List<TblAct> actWotEvtRstDepd = DetectActWotEvtRstDepd_3087(context, act);

            //3074
            foreach (TblAct actWotEvtRstDepd_i in actWotEvtRstDepd)
            {
                //----------------------------------------------
                List<TblPr> c = new List<TblPr>();
                List<TblAct> d = new List<TblAct>();
                //----------------------------------------------

                //3093
                List<TblAct> b = new List<TblAct>();

                //TblPr prsAct1 = DetectPrsOfAct_1839(context, actWotEvtRstDepd_i);

                //3089
                List<TblAct> allActPvs = new List<TblAct>();

                //DetectAllActsPvsAct_3089(context, actWotEvtRstDepd_i, allActPvs);

                DetectAllActsPvsAct_22045(context, actWotEvtRstDepd_i, allActPvs);


                //21837
                foreach (TblAct allActPvs_i in allActPvs)
                {
                    //1424
                    if (!b.Contains(allActPvs_i))
                    {
                        b.Add(allActPvs_i);
                    }


                    //TblPr prsAct2 = DetectPrsOfAct_1839(context, allActPvs_i);

                    //21838
                    //if (prsAct2 == prsAct1)
                    //{
                    //    //1424
                    //    if (!b.Contains(allActPvs_i))
                    //    {
                    //        b.Add(allActPvs_i);
                    //    }
                    //}
                }

                //1283
                if (!b.Contains(actWotEvtRstDepd_i))
                {
                    b.Add(actWotEvtRstDepd_i);
                }

                //3075
                if (isleAct.Count == 0)
                {
                    TblPr isleAct_j = AddPrsRaw_1721(context);

                    isleAct.Add(isleAct_j);

                    foreach (TblAct item in b)
                    {
                        //7818
                        TblActPr actPrs = new TblActPr() { TblPr = isleAct_j, FldSttAct = 1 };
                        if (item.TblActPrs.Count > 0)
                        {
                            context.TblActPrs.DeleteObject(item.TblActPrs.First());

                            //item.TblActPrs.Remove(item.TblActPrs.First());
                        }
                        item.TblActPrs.Add(actPrs);

                        //1424
                        if (!actIsle.Contains(item))
                        {
                            actIsle.Add(item);
                        }
                    }

                }

                //3101
                else
                {
                    List<TblPr> lst = new List<TblPr>(isleAct);

                    bool isMerged = false;

                    //3083
                    foreach (TblPr isleAct_j in lst)
                    {
                        //3098
                        bool stsIsc = actIsle.Where(m => m.TblActPrs.First().TblPr == isleAct_j).Intersect(b).Count() > 0;

                        //isMerged = false;
                        //3099
                        if (stsIsc)
                        {
                            isMerged = true;

                            c.Add(isleAct_j);

                            d.AddRange(isleAct_j.Acts);
                            d = d.Distinct().ToList();

                            ////7821
                            //foreach (TblAct b_j in b)
                            //{
                            //    //7822
                            //    TblActPr actPrs = new TblActPr() { TblPr = isleAct_j, FldSttAct = 1 };
                            //    if (b_j.TblActPrs.Count > 0)
                            //    {
                            //        context.TblActPrs.DeleteObject(b_j.TblActPrs.First());

                            //        //item.TblActPrs.Remove(item.TblActPrs.First());
                            //    }
                            //    b_j.TblActPrs.Add(actPrs);

                            //    //1424
                            //    if (!actIsle.Contains(b_j))
                            //    {
                            //        actIsle.Add(b_j);
                            //    }
                            //}
                        }

                        //    //3086
                        //else
                        //{
                        //    //2908
                        //    TblPr isleAct_k = AddPrsRaw_1721(context);

                        //    //1424
                        //    isleAct.Add(isleAct_k);

                        //    //7819
                        //    foreach (TblAct item in b)
                        //    {
                        //        //7820
                        //        TblActPr actPrs = new TblActPr() { TblPr = isleAct_k, FldSttAct = 1 };
                        //        if (item.TblActPrs.Count > 0)
                        //        {
                        //            context.TblActPrs.DeleteObject(item.TblActPrs.First());

                        //            //item.TblActPrs.Remove(item.TblActPrs.First());
                        //        }
                        //        item.TblActPrs.Add(actPrs);

                        //        //1424
                        //        if (!actIsle.Contains(item))
                        //        {
                        //            actIsle.Add(item);
                        //        }
                        //    }
                        //}
                    }

                    //22056
                    if (isMerged)
                    {
                        TblPr isleAct_n = AddPrsRaw_1721(context);

                        isleAct.Add(isleAct_n);

                        d.AddRange(b);
                        d = d.Distinct().ToList();
                    }

                    //3086
                    if (!isMerged)
                    {
                        TblPr isleAct_k = AddPrsRaw_1721(context);

                        //1424
                        isleAct.Add(isleAct_k);

                        //7819
                        foreach (TblAct item in b)
                        {
                            //7820
                            TblActPr actPrs = new TblActPr() { TblPr = isleAct_k, FldSttAct = 1 };
                            if (item.TblActPrs.Count > 0)
                            {
                                context.TblActPrs.DeleteObject(item.TblActPrs.First());

                                //item.TblActPrs.Remove(item.TblActPrs.First());
                            }
                            item.TblActPrs.Add(actPrs);

                            //1424
                            if (!actIsle.Contains(item))
                            {
                                actIsle.Add(item);
                            }
                        }

                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static string DetectNamPrs_17972(BPMNDBEntities context, TblPr prs)
        {
            double? d = null;

            foreach (TblNamPrpsPr item in prs.TblNamPrpsPrs)
            {
                double d1 = ComputeWeightVluNamPrpsPrs_1593(context, item);
                if (d == null || d1 > d)
                {
                    d = d1;
                    if (prs.FldNamPrs != item.FldNamPrpsPrs)
                    {
                        prs.FldNamPrs = item.FldNamPrpsPrs;
                    }

                }
            }

            return prs.FldNamPrs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static TblNod DetectOwrPrs_17974(BPMNDBEntities context, TblPr prs)
        {
            try
            {
                double? d = null;

                foreach (TblOwrPrpsPr item in prs.TblOwrPrpsPrs)
                {
                    double d1 = ComputeWeightVluOwrPrpsPrs_1624(context, item);
                    if (d == null || d1 > d)
                    {
                        d = d1;
                        if ((prs.TblNod != null && item.TblNod != null && prs.TblNod.FldCodNod != item.TblNod.FldCodNod) || (prs.TblNod == null && item.TblNod != null))
                        {
                            prs.TblNod = item.TblNod;
                        }

                    }
                }

                return prs.TblNod;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public static INode DetectOrDisShpAtchEedToShpOthInDgm_19216(BPMNDBEntities context, GraphControl grph, INode dgm, Enum.TypShp typShp, INode shpOth, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        {
            //-----------------------------/

            INode shpAtchEed;

            //-----------------------------/

            INode shpHmlg = (INode)DetectShpEqualToEty_2326(context, dgm, etyHmlgWthShp);

            //19224
            if (shpHmlg != null)
            {
                shpAtchEed = shpHmlg;
            }

            //19222
            else
            {
                shpAtchEed = DislayShpAttachedToAnotherShp_2330(context, grph, dgm, typShp, shpOth, etyHmlgWthShp, ttlShp);
            }

            return shpAtchEed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shpAgntTmpEvtRst"></param>
        /// <param name="wayIfrmIdpd"></param>
        /// <param name="wayIfrmDepd"></param>
        /// <returns></returns>
        public static INode DisWayIfrmIdpdOfEvtRstInDgm_19277(
            BPMNDBEntities context,
            GraphControl grph,
            INode dgm,
            INode shpAgntTmpEvtRst,
            List<IWayIfrm> wayIfrmIdpd,
            List<IWayIfrm> wayIfrmDepd,
            List<IWayIfrm> wayIfrmWthDstUspf,
            List<IWayIfrm> wayIfrmWthDstPrsOth)
        {
            //------------------------------/

            INode shpAgntWayIfrmIdpdEvtRst;
            INode shpWayIfrm = null;
            INode shpEvtMsg = null;

            //------------------------------/

            //19278
            for (int i = 0; i < wayIfrmIdpd.Count; i++)
            {
                //19287
                if (i != wayIfrmIdpd.Count - 1)
                {
                    //19289
                    if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_News))
                    {
                        shpWayIfrm = DetectOrDisplayShpOfEtyInDgm_21962(context, grph, dgm, TypShp.E32, DetectColOfShp(shpAgntTmpEvtRst), null, (EntityObject)wayIfrmIdpd[i].ObjRst);
                    }

                    //19288
                    else
                    {
                        shpWayIfrm = DetectOrDisplayShpOfEtyInDgm_21962(context, grph, dgm, TypShp.E19, DetectColOfShp(shpAgntTmpEvtRst), null, (EntityObject)wayIfrmIdpd[i].ObjRst);

                        TblNod nodDst = DetectNodDstOfWayIfrm_9866(context, wayIfrmIdpd[i]);

                        INode shpNodOsdOrg = (INode)DetectOrDisplayColOfNod_2953(context, grph, dgm, nodDst);

                        //21641
                        if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_SndOut))
                        {
                            INode shpObj = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)wayIfrmIdpd[i].ObjRst, TypShp.DO2);

                            //21998
                            if (shpObj == null)
                            {
                                shpObj = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAgntTmpEvtRst), TypShp.DO2, (EntityObject)wayIfrmIdpd[i].ObjRst);

                                DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpObj, false);
                            }

                            DisConDconTwoShps_2919(grph, dgm, shpObj, shpNodOsdOrg, true);
                        }

                        //21640
                        if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_Oral))
                        {
                            DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpNodOsdOrg, true);
                        }
                    }
                }

                //19290
                else
                {
                    //19294
                    if (wayIfrmDepd.Count > 0 || wayIfrmWthDstUspf.Count > 0 || wayIfrmWthDstPrsOth.Count > 0)
                    {
                        //19296
                        if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_News))
                        {
                            shpWayIfrm = DetectOrDisplayShpOfEtyInDgm_21962(context, grph, dgm, TypShp.E32, DetectColOfShp(shpAgntTmpEvtRst), null, (EntityObject)wayIfrmIdpd[i].ObjRst);
                        }

                        //19295
                        else
                        {
                            shpWayIfrm = DetectOrDisplayShpOfEtyInDgm_21962(context, grph, dgm, TypShp.E19, DetectColOfShp(shpAgntTmpEvtRst), null, (EntityObject)wayIfrmIdpd[i].ObjRst);

                            TblNod nodDst = DetectNodDstOfWayIfrm_9866(context, wayIfrmIdpd[i]);

                            INode shpNodOsdOrg = (INode)DetectOrDisplayColOfNod_2953(context, grph, dgm, nodDst);

                            //21638
                            if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                INode shpObj = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)wayIfrmIdpd[i].ObjRst, TypShp.DO2);

                                //21999
                                if (shpObj == null)
                                {
                                    shpObj = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAgntTmpEvtRst), TypShp.DO2, (EntityObject)wayIfrmIdpd[i].ObjRst);

                                    DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpObj, false);
                                }

                                DisConDconTwoShps_2919(grph, dgm, shpObj, shpNodOsdOrg, true);
                            }

                            //21637
                            if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_Oral))
                            {
                                DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpNodOsdOrg, true);
                            }
                        }
                    }

                    //19291
                    else if (wayIfrmDepd.Count == 0 && wayIfrmWthDstUspf.Count == 0 && wayIfrmWthDstPrsOth.Count == 0)
                    {
                        //19293
                        if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_News))
                        {
                            shpWayIfrm = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)wayIfrmIdpd[i].ObjRst);

                            //21996
                            if (shpWayIfrm != null)
                            {
                                List<INode> shpConEed = DetectShpConnecterdToBeforeShpInDgm_11892(grph, shpWayIfrm);

                                shpAgntTmpEvtRst = shpConEed[0];

                                DeleteNodFromGrph_21988(grph, shpWayIfrm, (EntityObject)wayIfrmIdpd[i].ObjRst);
                            }

                            shpWayIfrm = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAgntTmpEvtRst), TypShp.E33, (EntityObject)wayIfrmIdpd[i].ObjRst);
                        }

                        //19292
                        else
                        {
                            shpWayIfrm = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)wayIfrmIdpd[i].ObjRst);

                            //22000
                            if (shpWayIfrm != null)
                            {
                                List<INode> shpConEed = DetectShpConnecterdToBeforeShpInDgm_11892(grph, shpWayIfrm);

                                shpAgntTmpEvtRst = shpConEed[0];

                                DeleteNodFromGrph_21988(grph, shpWayIfrm, (EntityObject)wayIfrmIdpd[i].ObjRst);
                            }

                            shpWayIfrm = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAgntTmpEvtRst), TypShp.E20, (EntityObject)wayIfrmIdpd[i].ObjRst);

                            TblNod nodDst = DetectNodDstOfWayIfrm_9866(context, wayIfrmIdpd[i]);

                            INode shpNodOsdOrg = (INode)DetectOrDisplayColOfNod_2953(context, grph, dgm, nodDst);

                            //21633
                            if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                INode shpObj = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)wayIfrmIdpd[i].ObjRst, TypShp.DO2);

                                //22002
                                if (shpObj == null)
                                {
                                    shpObj = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAgntTmpEvtRst), TypShp.DO2, (EntityObject)wayIfrmIdpd[i].ObjRst);
                                }

                                DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpObj, false);

                                DisConDconTwoShps_2919(grph, dgm, shpObj, shpNodOsdOrg, true);
                            }

                            //21635
                            if (wayIfrmIdpd[i].GetType() == typeof(TblWayIfrm_Oral))
                            {
                                DisConDconTwoShps_2919(grph, dgm, shpWayIfrm, shpNodOsdOrg, true);
                            }
                        }
                    }
                }

                DisplayConCnulTwoShp_1253(grph, dgm, shpAgntTmpEvtRst, shpWayIfrm);

                shpAgntTmpEvtRst = shpWayIfrm;
            }

            shpAgntWayIfrmIdpdEvtRst = shpAgntTmpEvtRst;
            return shpAgntWayIfrmIdpdEvtRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="evtRst"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static INode DisEvtRstInDgm_19231(BPMNDBEntities context, GraphControl grph, INode dgm, TblEvtRst evtRst, TblAct act)
        {
            //-----------------------------/

            INode shpAgntEvtRst = null;
            INode shpAgntTmpEvtRst = null;
            INode shpAgntWayIfrmIdpdEvtRst;
            List<IWayIfrm> wayIfrmDepd = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmIdpd = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmWthDstUspf = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmWthDstPrsOth = new List<IWayIfrm>();

            //-----------------------------/

            List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst);

            DetectWayIfrmDepdAndIdpdOfWayIfrm_9876(context, wayIfrm, wayIfrmIdpd, wayIfrmDepd, wayIfrmWthDstUspf, wayIfrmWthDstPrsOth);

            //19234
            if (evtRst.FldTypEvtRst == (int)Enum.EvtRstType.anyCdnEvtRst)
            {
                shpAgntTmpEvtRst = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act);
            }

            //19264
            else if (evtRst.FldTypEvtRst == (int)Enum.EvtRstType.spcCdnEvtRstAftr)
            {
                shpAgntTmpEvtRst = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), TypShp.G1)[0];
            }

                //19274
            else if (evtRst.FldTypEvtRst == (int)Enum.EvtRstType.cancelEvtRst ||
                evtRst.FldTypEvtRst == (int)Enum.EvtRstType.errAccurEvtRst ||
                evtRst.FldTypEvtRst == (int)Enum.EvtRstType.getNewAwr ||
                evtRst.FldTypEvtRst == (int)Enum.EvtRstType.spcCdnEvtRstInnTim)
            {
                shpAgntTmpEvtRst = DetectOrDisShpAtchEedToShpOthInDgm_19216(context, grph, dgm, DetectTypShpEqualToEvtRstBoundary_3114(context, evtRst), DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), evtRst);
            }

            DisNewUnusedEvtRstInDgm_19327(context, grph, dgm, evtRst, shpAgntTmpEvtRst);

            shpAgntWayIfrmIdpdEvtRst = DisWayIfrmIdpdOfEvtRstInDgm_19277(context, grph, dgm, shpAgntTmpEvtRst, wayIfrmIdpd, wayIfrmDepd, wayIfrmWthDstUspf, wayIfrmWthDstPrsOth);

            INode shpAgntWayIfrmWthDstPrsOthEvtRst = DisWayIfrmWthDstPrsOth_21800(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, wayIfrmDepd, wayIfrmWthDstUspf, wayIfrmWthDstPrsOth, act);

            shpAgntEvtRst = DisWayIfrmWthDstUspfInDgm_21695(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, wayIfrmDepd, wayIfrmWthDstUspf, wayIfrmWthDstPrsOth, act);

            return shpAgntEvtRst;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp"></param>
        /// <returns></returns>
        public static List<INode> DetectShpConnecterdToBeforeShpInDgm_11892(GraphControl grph, INode shp)
        {
            List<INode> shpConEed = new List<INode>();

            foreach (IEdge lnk in grph.Graph.InEdgesAt(shp))
            {
                if (!shpConEed.Contains(lnk.SourcePort.Owner))
                {
                    shpConEed.Add((INode)lnk.SourcePort.Owner);
                }
            }

            return shpConEed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayIfrm"></param>
        /// <param name="wayIfrmIdpd"></param>
        /// <param name="wayIfrmDepd"></param>
        public static void DetectWayIfrmDepdAndIdpdOfWayIfrm_9876(BPMNDBEntities context, List<IWayIfrm> wayIfrm, List<IWayIfrm> wayIfrmIdpd, List<IWayIfrm> wayIfrmDepd, List<IWayIfrm> wayIfrmWthDstUspf, List<IWayIfrm> wayIfrmWthDstPrsOth)
        {
            //9877
            foreach (IWayIfrm wayIfrm_i in wayIfrm)
            {
                Enum.StsIdpdWayIfrm stsIdpdWayIfrm = DetectStsIdpdWayIfrm_8852(context, wayIfrm_i);

                //21794
                if (stsIdpdWayIfrm == StsIdpdWayIfrm.WithDestinationUnspecified)
                {
                    wayIfrmWthDstUspf.Add(wayIfrm_i);
                }

                //21722
                else if (stsIdpdWayIfrm == StsIdpdWayIfrm.WithDestinationProcessOther)
                {
                    wayIfrmWthDstPrsOth.Add(wayIfrm_i);
                }

                //9878
                else if (stsIdpdWayIfrm == StsIdpdWayIfrm.Independent)
                {
                    wayIfrmIdpd.Add(wayIfrm_i);
                }

                //9879
                else
                {
                    wayIfrmDepd.Add(wayIfrm_i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static Enum.StsIdpdWayIfrm DetectStsIdpdWayIfrm_8852(BPMNDBEntities context, IWayIfrm wayIfrm)
        {
            //-----------------------------/

            StsIdpdWayIfrm stsIdpdWayIfrm = StsIdpdWayIfrm.dependent;

            //-----------------------------/

            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrm);

            //9860
            if (actDst == null)
            {
                stsIdpdWayIfrm = StsIdpdWayIfrm.Independent;

                return stsIdpdWayIfrm;
            }

            //21765
            if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            {
                stsIdpdWayIfrm = StsIdpdWayIfrm.Independent;

                return stsIdpdWayIfrm;
            }

            TblNod nodDst = DetectNodDstOfWayIfrm_9866(context, wayIfrm);

            bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nodDst, CurrentUser.TblOrg);

            //9867
            if (stsOsdOrgNod)
            {
                stsIdpdWayIfrm = StsIdpdWayIfrm.Independent;

                return stsIdpdWayIfrm;
            }

            //21648
            if (actDst.FldActUspf)
            {
                stsIdpdWayIfrm = StsIdpdWayIfrm.WithDestinationUnspecified;

                return stsIdpdWayIfrm;
            }

            TblAct actSrc = DetectSourceActOfWayIfrm_19105(wayIfrm);

            TblPr prsActSrc = DetectPrsOfAct_1839(context, actSrc);

            TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

            //21782
            if (prsActSrc != prsActDst)
            {
                //22004
                if (prsActSrc != null)
                {
                    stsIdpdWayIfrm = StsIdpdWayIfrm.WithDestinationProcessOther;

                    return stsIdpdWayIfrm;
                }
            }

            //21720
            //در مقدار دهی پیشفرض دیده شده

            return stsIdpdWayIfrm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static TblNod DetectNodDstOfWayIfrm_9866(BPMNDBEntities context, IWayIfrm wayIfrm)
        {
            TblAct act = DetectActDstOfWayIfrm_720(context, wayIfrm);

            return act.TblNod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="actDisEed"></param>
        //public static void DisActAndActPvsInDgm_3026_OLD(BPMNDBEntities context, Diagram dgm, TblAct act, List<TblAct> actDisEed)
        //{
        //    DisActInDgm_3022(context, dgm, act, actDisEed);

        //    List<TblAct> actPvs = DetectActsPvsAct_3032(context, act);

        //    //3027
        //    foreach (TblAct actPvs_i in actPvs)
        //    {
        //        DisActAndActPvsInDgm_3026(context, dgm, actPvs_i, actDisEed);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="actDisEed"></param>
        public static void DisActAndActPvsInDgm_3026(BPMNDBEntities context, GraphControl grph, INode dgm, TblAct act, List<TblAct> actDisEed)
        {
            //21952
            if (!actDisEed.Contains(act))
            {
                //21632
                if (!act.FldActUspf)
                {
                    DisActInDgm_3022(context, grph, dgm, act, actDisEed);

                    List<TblAct> actPvs = DetectActsPvsAct_3032(context, act);

                    //3027
                    foreach (TblAct actPvs_i in actPvs)
                    {
                        bool stsOsdOrgNod = IsNodOsdOrg_1085(context, actPvs_i.TblNod, CurrentUser.TblOrg);

                        TblPr prs = DetectPrsOfAct_1839(context, act);

                        TblPr prsActPvs = DetectPrsOfAct_1839(context, actPvs_i);

                        //21631
                        if (!stsOsdOrgNod && prs == prsActPvs)
                        {
                            DisActAndActPvsInDgm_3026(context, grph, dgm, actPvs_i, actDisEed);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="shpAgntWayIfrmIdpdEvtRst"></param>
        /// <param name="wayIfrmDepd"></param>
        /// <param name="wayIfrmWthDstUspf"></param>
        /// <returns></returns>
        public static INode DisWayIfrmWthDstUspfInDgm_21695(BPMNDBEntities context,
            GraphControl grph,
            INode dgm,
            INode shpAgntWayIfrmIdpdEvtRst,
            List<IWayIfrm> wayIfrmDepd,
            List<IWayIfrm> wayIfrmWthDstUspf,
            List<IWayIfrm> wayIfrmWthDstPrsOth,
            TblAct act)
        {

            //-------------------------------------/

            int tnoWayIfrmOth = 0;

            //-------------------------------------/


            tnoWayIfrmOth = wayIfrmDepd.Count + wayIfrmWthDstPrsOth.Count;

            //21697
            if (tnoWayIfrmOth == 0)
            {
                //21701
                if (wayIfrmWthDstUspf.Count == 0)
                {
                    return shpAgntWayIfrmIdpdEvtRst;
                }

                //21698
                else
                {
                    //21700
                    if (wayIfrmWthDstUspf.Count == 1)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDstUspf.First());

                        INode shpActUspf = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, actDst);

                        DisplayConCnulTwoShp_1253(grph, dgm, shpAgntWayIfrmIdpdEvtRst, shpActUspf);

                        //21723
                        if (wayIfrmWthDstUspf.First().GetType() == typeof(TblWayIfrm_SndOut))
                        {
                            IObjRst objRst = DetectObjRstWayIfrm_721(context, wayIfrmWthDstUspf.First());

                            DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpActUspf);
                        }

                        return shpAgntWayIfrmIdpdEvtRst;
                    }

                    //21699
                    else
                    {
                        INode shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3)[0];

                        //21709
                        foreach (IWayIfrm wayIfrmWthDesUspf_i in wayIfrmWthDstUspf)
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDesUspf_i);

                            INode shpActUspf = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, actDst);

                            DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll, shpActUspf);

                            //21725
                            if (wayIfrmWthDesUspf_i.GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                IObjRst objRst = DetectObjRstWayIfrm_721(context, wayIfrmWthDesUspf_i);

                                DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpActUspf);
                            }
                        }

                        return shpGatWayPrll;
                    }
                }
            }

            //21703
            else
            {
                //21706
                if (tnoWayIfrmOth == 1)
                {
                    //21712
                    if (wayIfrmWthDstUspf.Count == 0)
                    {
                        return shpAgntWayIfrmIdpdEvtRst;
                    }

                        //21713
                    else
                    {
                        INode shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3)[0];

                        //21718
                        foreach (IWayIfrm wayIfrmWthDesUspf_i in wayIfrmWthDstUspf)
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDesUspf_i);

                            INode shpActUspf = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, actDst);

                            DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll, shpActUspf);

                            //21726
                            if (wayIfrmWthDesUspf_i.GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                IObjRst objRst = DetectObjRstWayIfrm_721(context, wayIfrmWthDesUspf_i);

                                DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpActUspf);
                            }
                        }

                        return shpGatWayPrll;
                    }
                }

                    //21704
                else if (tnoWayIfrmOth > 1)
                {
                    INode shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3)[0];

                    //21719
                    foreach (IWayIfrm wayIfrmWthDesUspf_i in wayIfrmWthDstUspf)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDesUspf_i);

                        INode shpActUspf = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, actDst);

                        DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll, shpActUspf);

                        //21727
                        if (wayIfrmWthDesUspf_i.GetType() == typeof(TblWayIfrm_SndOut))
                        {
                            IObjRst objRst = DetectObjRstWayIfrm_721(context, wayIfrmWthDesUspf_i);

                            DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpActUspf);
                        }
                    }

                    return shpGatWayPrll;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prs"></param>
        public static void DisPrs_3015(BPMNDBEntities context, GraphControl grph, INode dgm, TblPr prs1)
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {
                TblPr prs = ctx.TblPrs.Single(m => m.FldCodPrs == prs1.FldCodPrs);

                List<TblAct> act = DetectActsOfPrs_946(ctx, prs);

                List<TblAct> actPri = DetectActWotEvtRstDepd_3087(ctx, act);

                //21736
                if (actPri.Count == 0)
                {
                    actPri = DetectActMereContEvtRstWthDstUspf_21728(ctx, act);

                    //21785
                    if (actPri.Count == 0)
                    {
                        List<TblAct> actMereContEvtRstWthDstPrsOth = DetectActMereContEvtRstWthDstPrsOth_21786(ctx, actPri);

                        if (actPri.Count == 0)
                        {
                            actPri.Add(act.First());
                        }
                    }
                }

                List<TblAct> actDisEed = new List<TblAct>();

                //3019
                foreach (TblAct actPri_i in actPri)
                {
                    DisActAndActPvsInDgm_3026(ctx, grph, dgm, actPri_i, actDisEed);
                }
            }
        }

        /*--------------------------------------------------------------------*/

        /// <summary>
        /// no pdr
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        public static List<TblOrg> GetOrgForPsn(TblPsn psn)
        {
            List<TblOrg> result = new List<TblOrg>();
            List<TblUsr> usrs = psn.TblUsrs.ToList();
            foreach (var item in usrs)
            {
                result.Add(item.TblOrg);
            }
            return result;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        public static List<TblOrg> GetOrgForPsnAgntOfThemInCurrOrgAndSubOrg(TblPsn psn)
        {

            return psn.TblAgntNods.Where(a => (a.TblNod.EtyNod is TblOrg &&

                (// لیست ساز مانهایی که کاربر جاری دارای یک نمایندگی از نوع سازمان یا جایگاه و سمت در آن است.
                TblOrg.IsOrgAnsestorOfThisOrg(CurrentUser.TblOrg, a.TblNod.EtyNod as TblOrg) ||
                CurrentUser.TblOrg.FldCodOrg == (a.TblNod.EtyNod as TblOrg).FldCodOrg
                ))
                ||
                (
                // از بین جایگاه و سمت ها، آنهایی که در سازمان جاری یا در سازمان تابعه سازمان جاری باشند
                    (a.TblNod.EtyNod is TblPosPstOrg)
                    &&
                    (TblOrg.IsOrgAnsestorOfThisOrg(CurrentUser.TblOrg, (a.TblNod.EtyNod as TblPosPstOrg).Org)
                    || (a.TblNod.EtyNod as TblPosPstOrg).Org.FldCodOrg == CurrentUser.TblOrg.FldCodOrg)

                )

                ).Select(a =>
                {
                    if (a.TblNod.EtyNod is TblOrg)
                    {
                        return new { a = a.TblNod.EtyNod as TblOrg };
                    }
                    else
                    {
                        return new { a = (a.TblNod.EtyNod as TblPosPstOrg).Org };
                    }
                }).Select(b => b.a).Distinct().ToList();

        }



        /// <summary>
        /// RollBack Context to its original state and discards changes
        /// </summary>
        /// <param name="dbContext"></param>
        public static void RollBackContext(BPMNDBEntities dbContext)
        {
            // delete added objects that did not get saved
            foreach (var entry in dbContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added))
            {
                if (entry.State == EntityState.Detached)
                {
                    continue;
                }
                if (entry.Entity != null)
                {
                    //if (entry.Entity is TblAct)
                    //{
                    //    OrgPosVM.ActList.Remove(entry.Entity as TblAct);
                    //}

                    dbContext.DeleteObject(entry.Entity);
                }
            }
            // Refetch modified objects from database
            foreach (var entry in dbContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified))
            {
                if (entry.State == EntityState.Detached)
                {
                    continue;
                }

                if (entry.Entity != null)
                {
                    ReloadEntity(dbContext, (EntityObject)entry.Entity);
                }
            }
            // Recover modified objects that got deleted
            foreach (var entry in dbContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted))
            {
                if (entry.State == EntityState.Detached)
                {
                    continue;
                }

                if (entry.Entity != null)
                    ReloadEntity(dbContext, (EntityObject)entry.Entity);
            }

            dbContext.AcceptAllChanges();
        }

        /// <summary>
        /// F2194
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns>true if successfully delete otherwise false</returns>
        public static bool DeleteNod_2194(BPMNDBEntities context, TblNod nod)
        {
            //2197 and 2076
            if (TryDeleteNod_2076(context, nod))
            {
                //2199
                context.DeleteObject(nod);

                return true;
            }
            //2205
            else
            {
                return false;
            }

        }

        /// <summary>
        /// F2076
        /// بررسی قابل حذف بودن یا نبودن یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static bool TryDeleteNod_2076(BPMNDBEntities context, TblNod nod)
        {

            //754
            List<TblAct> act = DetectActsOfNod_754(context, nod);

            if (act.Count == 0)
            {
                return true;
            }

            //2077
            if (act.Count > 1)
            {
                //2078
                return false;
            }
            //2079
            else if (act.Count == 1)
            {
                //2080
                if (act[0].TblEvtRsts.Count > 0 || act[0].TblEvtSrts.Count > 0)
                {
                    //2081
                    return false;
                }
                //2082
                else
                {
                    //2083
                    if (DetectPrsOwnEedByNod_2085(context, nod).Count > 0)
                    {
                        return false;
                    }
                    //2088
                    else
                    {
                        //2089
                        if (DetectNamPrpsByNod_2090(context, nod).Count > 0)
                        {
                            return false;
                        }
                        //2092
                        else
                        {
                            //2094
                            if (DetectVotNamPrpsByNod_2093(context, nod).Count > 0)
                            {
                                return false;
                            }

                            //2096
                            else
                            {
                                //2099
                                if (DetectOwrPrpsByNod_2097(context, nod).Count > 0)
                                {
                                    return false;
                                }

                                    //2098
                                else
                                {
                                    //2103
                                    if (DetectVotOwrPrpsByNod_2101(context, nod).Count > 0)
                                    {
                                        return false;
                                    }

                                    //2102
                                    else
                                    {
                                        //2111
                                        if (DetectVotOwrPrpsByNod_2101(context, nod).Count > 0)
                                        {
                                            return false;
                                        }

                                        //2110
                                        else
                                        {
                                            if (context.TblPlyrRols.Any(p => p.FldCodNod == nod.FldCodNod))
                                            {
                                                return false;
                                            }

                                            //2113
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return true;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DelectActOfPrs_8818(BPMNDBEntities context, TblAct act)
        {
            if (act.TblActPrs.Count() > 0)
            {
                TblActPr actPrs = act.TblActPrs.First();

                context.DeleteObject(actPrs);


            }

        }

        /// <summary>
        /// F2109
        /// شناسایی لیست مواردی که در آن یک گره به عنوان مالک پیشنهادی معرفی شده است
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblOwrPrpsPr> DetectWhereNodPrpsEedAsOwrPrs_2109(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblOwrPrpsPrs, "TblOwrPrpsPrs");

            return nod.TblOwrPrpsPrs.ToList();
        }

        /// <summary>
        /// F2101
        /// شناسایی آرای داده شده به مالک فرآیندها توسط یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblVotOwrPrp> DetectVotOwrPrpsByNod_2101(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblOwrPrpsPrs, "TblVotOwrPrps");

            return nod.TblVotOwrPrps.ToList();
        }

        /// <summary>
        /// F2097
        /// شناسایی مالک هایی که توسط یک گره برای فرآیندها پیشنهاد شده است
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblOwrPrpsPr> DetectOwrPrpsByNod_2097(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblOwrPrpsPrs1, "TblOwrPrpsPrs1");

            return nod.TblOwrPrpsPrs1.ToList();
        }

        /// <summary>
        /// F2093
        /// شناسایی آرای داده شده به نام فرآیندها توسط یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblVotNamPrpsPr> DetectVotNamPrpsByNod_2093(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblVotNamPrpsPrs, "TblVotNamPrpsPrs");

            return nod.TblVotNamPrpsPrs.ToList();
        }

        /// <summary>
        /// F2090
        /// شناسایی نام هایی که توسط یک گره برای فرآیندها پیشنهاد شده است
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblNamPrpsPr> DetectNamPrpsByNod_2090(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblNamPrpsPrs, "TblNamPrpsPrs");

            return nod.TblNamPrpsPrs.ToList();
        }

        /// <summary>
        /// F2085
        /// شناسایی فرآیندهایی که یک گره مالک آن ها است
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblPr> DetectPrsOwnEedByNod_2085(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblPrs, "TblPrs");

            return nod.TblPrs.ToList();
        }

        /// <summary>
        /// F754
        /// شناسایی فعالیت های یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActsOfNod_754(BPMNDBEntities context, TblNod nod)
        {
            ReloadEntity(context, nod, nod.TblActs, "TblActs");

            return nod.TblActs.ToList();
        }

        /// <summary>
        /// F1524
        /// delete a psn if can
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <returns></returns>
        public static void DeletePsnIsd_22154(BPMNDBEntities context, TblPsn psn, bool isInside)
        {

            TblOrg org = context.TblOrgs.Single(m => m.FldCodOrg == CurrentUser.TblOrg.FldCodOrg);

            List<TblOrg> subOrgs = new List<TblOrg>();


            DetectSubOrgOfOrg_2072(org, subOrgs);

            List<TblUsr> users = new List<TblUsr>();

            //22236
            foreach (var o in subOrgs)
            {
                users.AddRange(o.TblUsrs.Where(u => u.FldCodPsn == psn.FldCodPsn));
            }

            List<TblAgntNod> agnts = new List<TblAgntNod>();

            //22237
            if (users.TrueForAll(u => !u.FldNamUsrActv))
            {
                //22229
                foreach (var o in subOrgs)
                {
                    if (isInside)
                    {
                        agnts = GetAgntOfPsnIsdOrg_22230(context, psn.FldCodPsn, o.FldCodOrg);

                        //22231
                        foreach (var a in agnts)
                        {
                            DelAgnt(context, a, psn.FldCodPsn);
                        }
                    }
                    else
                    {
                        context.TblNods.DeleteObject(context.TblNods.Single(n => n.FldCodTypEty == (int)Enum.FldTypEty.Psn && n.FldCodEty == psn.FldCodPsn));
                    }


                    // کاربری شخص در هر سازمان پاک می شود
                    // 22233
                    TblUsr usr = context.TblUsrs.SingleOrDefault(m => m.FldCodPsn == psn.FldCodPsn && m.FldCodOrg == o.FldCodOrg);

                    if (usr != null)
                    {
                        context.TblUsrs.DeleteObject(usr);
                    }
                }

                //22239
                if (psn.TblUsrs.Count == 0)
                {
                    context.TblPsns.DeleteObject(psn);
                }
            }
            else
            {
                throw new Exception("33");
            }

        }



        /// <summary>
        /// شناسایی نمایندگی های یک شخص در یک سازمان 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fldcodpsn"></param>
        /// <param name="fldCodOrg"></param>
        /// <returns></returns>
        public static List<TblAgntNod> GetAgntOfPsnIsdOrg_22230(BPMNDBEntities context, int fldcodpsn, int fldCodOrg)
        {
            TblPsn psn = context.TblPsns.Single(p => p.FldCodPsn == fldcodpsn);

            //شناسایی تمام نمایندگی های شخص
            List<TblAgntNod> agntOfPsn = new List<TblAgntNod>();

            foreach (var agnt in psn.TblAgntNods)
            {
                if (agnt.TblNod.EtyNod.Org.FldCodOrg == fldCodOrg)
                {
                    agntOfPsn.Add(agnt);
                }
            }

            return agntOfPsn;


            //List<TblNod> nods = TblNod.GetNodOfPsnIsdOrg_22192(context, fldcodpsn, fldCodOrg);
            //List<TblAgntNod> result = new List<TblAgntNod>();


            //foreach (var n in nods)
            //{
            //    foreach (var a in n.TblAgntNods)
            //    {
            //        if (a.FldCodPsn == fldcodpsn)
            //        {
            //            result.Add(a);
            //        }
            //    }
            //}

            //return result;
        }


        //public static bool CanDelNode(BPMNDBEntities context, TblNod nod)
        //{
        //    if (nod.TblActs.Any(a=>!a.FldActUspf||(a.FldActUspf && a.TblEvtSrts.)))
        //    {

        //    }
        //}


        /// <summary>
        /// حذف نمایندگی
        /// 22218 = org => current org
        /// 22212 = org => selected org
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="org"></param>
        public static void DelAgnt(BPMNDBEntities context, TblAgntNod obj, int fldCodPsn)
        {

            TblNod nod = obj.TblNod;

            context.DeleteObject(obj);

            //در صورتی که شخص جاری در سازمان جاری نمایندگی دیگری ندارد یوزر او نیز غیر فعال می شود
            var nods = TblNod.GetNodOfPsnIsdOrg_22192(context, fldCodPsn, nod.EtyNod.Org.FldCodOrg);


            if (nods.Count == 0)
            {
                TblUsr usr = context.TblUsrs.SingleOrDefault(m => m.FldCodPsn == fldCodPsn && m.FldCodOrg == nod.EtyNod.Org.FldCodOrg);

                if (usr != null)
                {
                    usr.FldNamUsrActv = false;
                }
            }
        }

        /// <summary>
        /// 22157 افزودن نمایندگی سازمان برای یک شخص درون سازمانی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <param name="org"></param>
        public static void AddAgntOfOrgForPsn_22157(BPMNDBEntities context, TblPsn psn, TblOrg org)
        {
            //اگر شخص انتخاب شده در سازمان انتخاب شده دارای کاربری باشد، کاربری جدید افزوده نشود
            if (psn.TblUsrs.Any(u => u.TblOrg.FldCodOrg == org.FldCodOrg))
            {
                TblUsr user = new TblUsr()
                {
                    FldCodOrg = org.FldCodOrg,
                    FldNamUsr = psn.FldNam2ndPsn + "#" + psn.FldCodPsn.ToString() + "#" +
                    org.FldCodOrg,
                    FldNamUsrActv = false,
                    FldPassUsr = "123456"
                };

                psn.TblUsrs.Add(user);
            }

            TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = 0, FldCodNod = org.Nod.FldCodNod };

            psn.TblAgntNods.Add(agnt);
        }

        /// <summary>
        /// 22157 افزودن نمایندگی سازمان برای یک شخص درون سازمانی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <param name="org"></param>
        public static TblAgntNod AddAgntOfNodForPsn_22157(BPMNDBEntities context, TblPsn psn, TblNod nod)
        {
            //اگر شخص انتخاب شده در سازمان انتخاب شده دارای کاربری باشد، کاربری جدید افزوده نشود
            if (!psn.TblUsrs.Any(u => u.TblOrg.FldCodOrg == nod.EtyNod.Org.FldCodOrg))
            {
                TblUsr user = new TblUsr()
                {
                    FldCodOrg = nod.EtyNod.Org.FldCodOrg,
                    FldNamUsr = psn.FldNam2ndPsn + "#" + psn.FldCodPsn.ToString() + "#" +
                    nod.EtyNod.Org.FldCodOrg,
                    FldNamUsrActv = false,
                    FldPassUsr = "123456"
                };

                psn.TblUsrs.Add(user);
            }

            TblAgntNod agnt = new TblAgntNod() { FldDmnAgnt = 0, FldCodLvlAcs = context.TblLvlAcs.First().FldCod, FldCodNod = nod.FldCodNod };

            psn.TblAgntNods.Add(agnt);

            return agnt;
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که یک شخص در یک سازمان نماینده آنهاست با توجه به دامنه دسترسی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<TblPosPstOrg> GetPosPstOfPsnAgntOfThem(BPMNDBEntities context, TblPsn psn, TblOrg org)
        {
            var posPst = TblNod.GetNodOfPsnIsdOrg_22192(context, psn.FldCodPsn, org.FldCodOrg).Where(n => n.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst).Select(n => new { PosPst = n.EtyNod as TblPosPstOrg, Agnt = n.TblAgntNods.Single(a => a.FldCodPsn == psn.FldCodPsn) }).ToList();
            foreach (var pos in posPst)
            {
                //if (pos.PosPst.SubPosPst != null)
                //{
                //    pos.PosPst.SubPosPst.Clear();
                //}
                //لیست کد تمام جایگاه و سمت های زیر مجموعه
                pos.PosPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                DetectSubOrgLevel(pos.PosPst, pos.Agnt.FldDmnAgnt);
            }
            return posPst.Select(p => p.PosPst).ToList();
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که یک شخص در یک سازمان نماینده آنهاست با توجه به دامنه دسترسی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="psn"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static List<List<int>> GetPosPstOfPsnAgntOfThem(BPMNDBEntities context, TblPsn psn, TblOrg org, bool UseFldCode)
        {
            var posPst = TblNod.GetNodOfPsnIsdOrg_22192(context, psn.FldCodPsn, org.FldCodOrg).Where(n => n.FldCodTypEty == (int)Model.Enum.FldTypEty.PosPst).Select(n => new { PosPst = n.EtyNod as TblPosPstOrg, Agnt = n.TblAgntNods.Single(a => a.FldCodPsn == psn.FldCodPsn) }).ToList();
            List<List<int>> temp = new List<List<int>>();
            Queue<TblPosPstOrg> q = new Queue<TblPosPstOrg>();
            foreach (var pos in posPst)
            {
                //لیست کد تمام جایگاه و سمت های زیر مجموعه
                List<int> temp1 = new List<int>();
                pos.PosPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();
                DetectSubOrgLevel(pos.PosPst, pos.Agnt.FldDmnAgnt, temp1);
                temp.Add(temp1);
            }
            return temp;
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که یک سازمان تا یک سطح مشخص
        /// </summary>
        /// <param name="posPst"></param>
        /// <param name="level"></param>
        public static void DetectSubOrgLevel(TblPosPstOrg posPst, int level)
        {

            //if (posPst.SubPosPst == null)
            //{
            //    posPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();
            //}
            posPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();

            if (level == 0)
            {
                return;
            }


            foreach (var item in posPst.TblPosPstOrg1)
            {
                posPst.SubPosPst.Add(item);
                DetectSubOrgLevel(item, level - 1);
            }
        }

        /// <summary>
        /// شناسایی جایگاه و سمت هایی که یک سازمان تا یک سطح مشخص
        /// </summary>
        /// <param name="posPst"></param>
        /// <param name="level"></param>
        public static void DetectSubOrgLevel(TblPosPstOrg posPst, int level, List<int> Out)
        {

            //if (posPst.SubPosPst == null)
            //{
            //    posPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();
            //}
            Out.Add(posPst.FldCodPosPst);
            posPst.SubPosPst = new ObservableCollection<TblPosPstOrg>();

            if (level == 0)
            {
                return;
            }


            foreach (var item in posPst.TblPosPstOrg1)
            {
                posPst.SubPosPst.Add(item);
                DetectSubOrgLevel(item, level - 1, Out);
            }
        }
        /// <summary>
        /// saves context!
        /// </summary>
        /// <param name="context"></param>
        public static void SaveContext(BPMNDBEntities context)
        {

            List<ObjectStateEntry> lst = context.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted).ToList();

            foreach (var item in lst)
            {
                if (item.Entity is IAllEty)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            TblLog.CreateLog(item.Entity as IAllEty, item.State);
                            break;
                    }
                }


                // این تغییر موقتی است و برای شناسایی فرایند های بی فعالیت است.
                //if (item.Entity is TblPr && item.State != EntityState.Deleted)
                //{
                //    var prs = item.Entity as TblPr;
                //    // اگر فرایند بی فعالیتی تولید شده است
                //    if (!prs.TblActPrs.Any())
                //    {
                //        throw new InvalidOperationException(string.Format("فرایند {0} با کد {1} فاقد فعالیت میباشد.", prs.Name, prs.FldCodPrs));
                //    }
                //}

                //if (item.Entity is TblAct)
                //{
                //    TblAct act = item.Entity as TblAct;
                //    if (!act.FldActUspf && act.TblActPrs.Count == 0)
                //    {
                //        throw new Exception(string.Format("فعالیت '{0}' با کد '{1}' بدون فرایند است و امکان ذخیره سازی آن وجود ندارد", act.FldNamAct, act.FldCodAct));
                //    }
                //}
            }


            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                //foreach (var error in ex.EntityValidationErrors)
                //{
                //    Console.WriteLine("====================");
                //    Console.WriteLine("Entity {0} in state {1} has validation errors:",
                //        error.Entry.Entity.GetType().Name, error.Entry.State);
                //    foreach (var ve in error.ValidationErrors)
                //    {
                //        Console.WriteLine("\tProperty: {0}, Error: {1}",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //    Console.WriteLine();
                //}

                throw new SSYM.OrgDsn.Common.ContextSaveException("خطایی پیش بینی نشده در حین ذخیره سازی تغییرات ایجاد شده است. نرم افزار نیازمند راه اندازی دوباره می باشد.", ex);
            }

            //try
            //{
            //    context.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    //MessageBox.Show("در ثبت اطلاعات خطایی رخ داده است" + "\r\n" + e.InnerException.ToString(), "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TblAct CreateUnspsifiedAct()
        {
            return new TblAct() { FldNamAct = "فعالیت نامشخص", FldTypAct = (int)ActivityTypes.Manual, FldActUspf = true };
        }

        public static List<TblAct> DetectActMereContEvtRstWthDstPrsOth_21786(BPMNDBEntities context, List<TblAct> act)
        {
            //---------------------------------------------/

            List<TblAct> actMereContEvtRstWthDstPrsOth = new List<TblAct>();

            //---------------------------------------------/


            //21787
            foreach (TblAct act_i in act)
            {
                actMereContEvtRstWthDstPrsOth.Add(act_i);

                List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act_i);

                //21788
                foreach (TblEvtRst evtRst_i in evtRst)
                {
                    List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst_i);

                    //21789
                    foreach (IWayIfrm wayIfrm_i in wayIfrm)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                        TblNod nod = actDst.TblNod;

                        bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nod, CurrentUser.TblOrg);

                        //21791
                        if (actDst.FldActUspf)
                        {
                            actMereContEvtRstWthDstPrsOth.Remove(actDst);

                            break;
                        }

                        //21790
                        else
                        {
                            TblPr prsActSrc = DetectPrsOfAct_1839(context, act_i);

                            TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

                            //21793
                            if (prsActDst == prsActSrc)
                            {
                                actMereContEvtRstWthDstPrsOth.Remove(act_i);
                            }
                        }
                    }
                }
            }

            return actMereContEvtRstWthDstPrsOth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="shpAgntWayIfrmIdpdEvtRst"></param>
        /// <param name="wayIfrmDepd"></param>
        /// <param name="wayIfrmWthDstUspf"></param>
        /// <param name="wayIfrmWthDstPrsOth"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static INode DisWayIfrmWthDstPrsOth_21800(BPMNDBEntities context, GraphControl grph, INode dgm, INode shpAgntWayIfrmIdpdEvtRst, List<IWayIfrm> wayIfrmDepd, List<IWayIfrm> wayIfrmWthDstUspf, List<IWayIfrm> wayIfrmWthDstPrsOth, TblAct act)
        {
            //---------------------------/

            int tnoWayIfrmOth = 0;

            INode shpAgntWayIfrmWthDstPrsOthEvtRst = null;

            //---------------------------/

            tnoWayIfrmOth = wayIfrmDepd.Count + wayIfrmWthDstUspf.Count;

            //21813
            if (tnoWayIfrmOth == 0)
            {
                //21822
                if (wayIfrmWthDstPrsOth.Count == 0)
                {
                    shpAgntWayIfrmWthDstPrsOthEvtRst = shpAgntWayIfrmIdpdEvtRst;
                }

                //21814
                else
                {
                    //21819
                    if (wayIfrmWthDstPrsOth.Count == 1)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDstPrsOth[0]);

                        TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

                        INode shpCallAct = DetectOrDisplayShpPrsInDgm_21825(context, grph, dgm, prsActDst);

                        DisplayConCnulTwoShp_1253(grph, dgm, shpAgntWayIfrmIdpdEvtRst, shpCallAct);

                        //21821
                        if (wayIfrmWthDstPrsOth[0].GetType() == typeof(TblWayIfrm_SndOut))
                        {
                            IObjRst objRst = wayIfrmWthDstPrsOth[0].ObjRst;

                            INode shpObj = DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpCallAct);
                        }

                        shpAgntWayIfrmWthDstPrsOthEvtRst = shpAgntWayIfrmIdpdEvtRst;
                    }

                    //21815
                    else
                    {
                        List<INode> shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3);

                        //21817
                        foreach (IWayIfrm wayIfrmWthDstPrsOth_i in wayIfrmWthDstPrsOth)
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDstPrsOth_i);

                            TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

                            INode shpCallAct = DetectOrDisplayShpPrsInDgm_21825(context, grph, dgm, prsActDst);

                            DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll[0], shpCallAct);

                            //21818
                            if (wayIfrmWthDstPrsOth[0].GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                IObjRst objRst = wayIfrmWthDstPrsOth_i.ObjRst;

                                DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpCallAct);
                            }
                        }

                        shpAgntWayIfrmWthDstPrsOthEvtRst = shpGatWayPrll[0];
                    }
                }
            }

            //21801
            else
            {
                //21806
                if (tnoWayIfrmOth == 1)
                {
                    //21811
                    if (wayIfrmWthDstPrsOth.Count == 0)
                    {
                        shpAgntWayIfrmWthDstPrsOthEvtRst = shpAgntWayIfrmIdpdEvtRst;
                    }

                    //21807
                    else
                    {
                        List<INode> shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3);

                        //21809
                        foreach (IWayIfrm wayIfrmWthDstPrsOth_i in wayIfrmWthDstPrsOth)
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDstPrsOth_i);

                            TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

                            INode shpCallAct = DetectOrDisplayShpPrsInDgm_21825(context, grph, dgm, prsActDst);

                            DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll[0], shpCallAct);

                            //21810
                            if (wayIfrmWthDstPrsOth[0].GetType() == typeof(TblWayIfrm_SndOut))
                            {
                                IObjRst objRst = wayIfrmWthDstPrsOth_i.ObjRst;

                                DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpCallAct);
                            }
                        }

                        shpAgntWayIfrmWthDstPrsOthEvtRst = shpGatWayPrll[0];
                    }
                }

                //21802
                else if (tnoWayIfrmOth > 1)
                {
                    List<INode> shpGatWayPrll = DetectOrDisplayTypShpConEedToShp_3104(context, grph, dgm, shpAgntWayIfrmIdpdEvtRst, TypShp.G3);

                    //21804
                    foreach (IWayIfrm wayIfrmWthDstPrsOth_i in wayIfrmWthDstPrsOth)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrmWthDstPrsOth_i);

                        TblPr prsActDst = DetectPrsOfAct_1839(context, actDst);

                        INode shpCallAct = DetectOrDisplayShpPrsInDgm_21825(context, grph, dgm, prsActDst);

                        DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayPrll[0], shpCallAct);

                        //21805
                        if (wayIfrmWthDstPrsOth[0].GetType() == typeof(TblWayIfrm_SndOut))
                        {
                            IObjRst objRst = wayIfrmWthDstPrsOth_i.ObjRst;

                            DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act), shpCallAct);
                        }
                    }

                    shpAgntWayIfrmWthDstPrsOthEvtRst = shpGatWayPrll[0];
                }
            }

            return shpAgntWayIfrmWthDstPrsOthEvtRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static INode DetectOrDisplayShpPrsInDgm_21825(BPMNDBEntities context, GraphControl grph, INode dgm, TblPr prs)
        {
            INode shpHmlgPrs = (INode)DetectShpEqualToEty_2326(context, dgm, prs);

            //21826
            if (shpHmlgPrs == null)
            {
                shpHmlgPrs = DisplayShpInColInDgm_2318(context, grph, dgm, TypShp.CA1, prs.TblNod, prs);
            }

            return shpHmlgPrs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <param name="ShpLane"></param>
        /// <param name="nodOwrLane"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        /// <returns></returns>
        public static INode DetectOrDisplayShpOfEtyInDgm_21962(BPMNDBEntities context, GraphControl grph, INode dgm, TypShp typShp, IRow ShpLane, TblNod nodOwrLane, EntityObject etyHmlgWthShp, string ttlShp = null)
        {
            INode shpHmlg = (INode)DetectShpEqualToEty_2326(context, dgm, etyHmlgWthShp, typShp);

            //21964
            if (shpHmlg == null)
            {
                //21980
                if (ShpLane != null)
                {
                    shpHmlg = DisShpInnColInnDgm_1325(context, grph, dgm, ShpLane, typShp, etyHmlgWthShp, ttlShp);
                }

                //21981
                else
                {
                    shpHmlg = DisplayShpInColInDgm_2318(context, grph, dgm, typShp, nodOwrLane, etyHmlgWthShp, ttlShp);
                }
            }

            return shpHmlg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <param name="stsCapSlc"></param>
        public static void ChangePrsAftrChangeSttPrs_21953(BPMNDBEntities context, TblPr prsAct1)
        {
            List<TblPr> prs = new List<TblPr>();

            List<TblAct> act1 = DetectActsOfPrs_946(context, prsAct1);

            //21957
            foreach (TblAct act1_i in act1)
            {
                List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act1_i);

                //21958
                foreach (TblEvtSrt evtSrt_i in evtSrt)
                {
                    List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrt_610(context, evtSrt_i);

                    //22023
                    foreach (IWayAwr wayAwr_i in wayAwr)
                    {
                        //22024
                        if (wayAwr_i.GetType() != typeof(TblWayAwr_News))
                        {
                            TblPr prsAct2 = DetectPrsOfAct_1839(context, wayAwr_i.ActSrc);

                            //21963
                            if (prsAct1 != prsAct2 && prsAct2 != null)
                            {
                                prs.Add(prsAct2);
                            }
                        }
                    }
                }

                List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act1_i);

                //21961
                foreach (TblEvtRst evtRst_i in evtRst)
                {
                    List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst_i);

                    //21966
                    foreach (IWayIfrm wayIfrm_i in wayIfrm)
                    {
                        //22026
                        if (wayIfrm_i.GetType() != typeof(TblWayIfrm_News))
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                            TblPr prsAct3 = DetectPrsOfAct_1839(context, actDst);

                            //21967
                            if (prsAct1 != prsAct3 && prsAct3 != null)
                            {
                                prs.Add(prsAct3);
                            }
                        }

                    }
                }
            }

            //22066
            if (prs.Count != 0)
            {
                //21973
                List<TblPr> prsByyMaxRln = new List<TblPr>();
                List<object> lst = new List<object>();
                prs.ForEach(m => lst.Add(m));
                DetectCptWthMaxRepeatInArray_21973(context, lst).ForEach(m => prsByyMaxRln.Add((TblPr)m));

                //21974
                //if (prsByyMaxRln.Count == 1)
                //{

                //21984
                if (prsByyMaxRln[0].FldSttPrs == (int)SttPrs.ConsolidatedEndorsed)
                {
                    prsByyMaxRln[0].FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
                }

                SetPrsOfActs_1724(context, act1, prsByyMaxRln[0]);

                DeletePrs_1726(context, prsAct1);

            }


            //}

            //21975
            //else
            //{
            //stsCapSlc = true;
            //}
        }



        /*--------------------------------------------------------------------*/

        /// <summary>
        /// این متد به صورت بازگشتی تمام اشخاص موجود درون سازمان داده شده و سازمان های تابعه را برمیگرداند
        /// </summary>
        /// <param name="org">سازمان ریشه</param>
        /// <param name="lstPsns">اشخاص موجود در سازمان</param>
        public static List<TblPsn> GetPsnInOrg(BPMNDBEntities context)
        {
            List<TblPsn> result = new List<TblPsn>();
            List<TblOrg> orgs = GetCurrentAndSubOrgs(context, PublicMethods.CurrentUser.TblOrg);

            foreach (var org in orgs)
            {
                foreach (var usr in org.TblUsrs)
                {
                    if (usr.TblPsn.FldIsdOrg)
                    {
                        if (!result.Any(m => m.FldCodPsn == usr.TblPsn.FldCodPsn))
                        {
                            result.Add(usr.TblPsn);
                        }
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// شناسایی اشخاص برون سازمانی یک سازمان
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fldCodOrg"></param>
        /// <returns></returns>
        public static List<TblPsn> GetPsnOutsideOrg_22244(BPMNDBEntities context, int fldCodOrg)
        {

            List<TblPsn> lst = new List<TblPsn>();

            List<TblOrg> orgs = new List<TblOrg>();

            DetectSubOrgOfOrg_2072(context.TblOrgs.Single(m => m.FldCodOrg == fldCodOrg), orgs);

            foreach (var org in orgs)
            {
                lst.AddRange(context.TblPsns.Where(p => !p.FldIsdOrg && p.TblUsrs.Any(u => u.FldCodOrg == org.FldCodOrg && !u.FldNamUsrActv)).ToList());
            }

            return lst;
        }

        /// <summary>
        /// سازمان داده شده به همراه تمامی سازمان های زیر مجموعه آن را بر میگرداند
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentOrg">سازمان سرشاخه</param>
        /// <returns></returns>
        public static List<TblOrg> GetCurrentAndSubOrgs(BPMNDBEntities context, TblOrg currentOrg)
        {
            TblOrg org = context.TblOrgs.Single(m => m.FldCodOrg == currentOrg.FldCodOrg);

            Queue<TblOrg> orgsQ = new Queue<TblOrg>();
            orgsQ.Enqueue(org);

            List<TblOrg> result = new List<TblOrg>();

            while (orgsQ.Count > 0)
            {
                TblOrg o = orgsQ.Dequeue();
                result.Add(o);

                foreach (var subOrg in o.TblOrg1)
                {
                    orgsQ.Enqueue(subOrg);
                }
            }

            return result;
        }

        /// <summary>
        /// F1550
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nam2ndPsn"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static TblUsr CreateNewUser_1550(BPMNDBEntities context, string nam2ndPsn, TblOrg org)
        {

            string newUsername = GenerateUniqueUsername(context, nam2ndPsn);
            TblUsr user = new TblUsr() { FldCodOrg = org.FldCodOrg, FldNamUsr = newUsername, FldNamUsrActv = false, FldPassUsr = "123456" };
            return user;
        }

        /// <summary>
        /// رفع ناهمسانی
        /// </summary>
        /// <param name="w"></param>
        public static void SettleDson_19202(IWayAwrIfrm w)
        {
            dynamic d = w;
            w.IsDson = false;
            w.IsAdded = false;

            if (w is IWayAwr)
            {
                d.FldTypDson = null;
                d.FldStsDsonWayAwr = null;
            }
            else
            {
                d.FldTypDson = null;
                d.FldStsDsonWayIfrm = null;
            }
        }


        /// <summary>
        /// اصلاح وضعیت ناهمسانی یک نحوه آگاه سازی
        /// </summary>
        /// <param name="wayIfrm"></param>
        public static void EditDsonStatusOfWayIfrm_19144(IWayIfrm wayIfrm/*, IObjRst objRst*/)
        {
            TblAct act = wayIfrm.ActSrc;
            dynamic d = wayIfrm;
            int codDson = d.FldTypDson.Value;
            switch (codDson)
            {
                case 1:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 1;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 2;
                    }
                    break;
                case 2:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 3;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 4;
                    }
                    break;
                case 3:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 5;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 6;
                    }

                    break;
                case 4:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 7;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 8;
                    }
                    break;
                case 5:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 9;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 10;
                    }
                    break;
                case 6:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 11;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 12;
                    }
                    break;
                case 7:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 13;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 14;
                    }
                    break;
                case 8:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 15;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 16;
                    }
                    break;
                case 9:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 17;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 18;
                    }
                    break;
                case 10:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayIfrm = 19;
                    }
                    else
                    {
                        d.FldStsDsonWayIfrm = 20;
                    }
                    break;

            }
        }


        /// <summary>
        /// اصلاح وضعیت ناهمسانی یک نحوه آگاهی
        /// </summary>
        /// <param name="wayAwr"></param>
        public static void EditDsonStatusOfWayAwr_19082(IWayAwr wayAwr)
        {
            TblAct act = wayAwr.ActDst;
            dynamic d = wayAwr;
            int codDson = d.FldTypDson.Value;

            switch (codDson)
            {
                case 1:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 1;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 2;
                    }
                    break;
                case 2:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 3;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 4;
                    }
                    break;
                case 3:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 5;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 6;
                    }

                    break;
                case 4:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 7;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 8;
                    }
                    break;
                case 5:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 9;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 10;
                    }
                    break;
                case 6:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 11;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 12;
                    }
                    break;
                case 7:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 13;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 14;
                    }
                    break;
                case 8:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 15;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 16;
                    }
                    break;
                case 9:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 17;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 18;
                    }
                    break;
                case 10:
                    if (!act.FldActUspf)
                    {
                        d.FldStsDsonWayAwr = 19;
                    }
                    else
                    {
                        d.FldStsDsonWayAwr = 20;
                    }
                    break;
            }
        }


        public static void AddNewDsonFromWayAwrIfrm_19072(BPMNDBEntities context, IWayAwrIfrm wayAwrIfrm, int codDson)
        {
            if (wayAwrIfrm is IWayAwr)
            {
                IWayIfrm wayIfrm = (IWayIfrm)DetectWayIfrmOfWayAwr_12950(wayAwrIfrm as IWayAwr);
                dynamic d = wayIfrm;
                d.FldTypDson = codDson;
                DetermineDsonSttForWayIfrm_19144(wayIfrm as IWayIfrm, DetectObjRstWayIfrm_721(context, wayIfrm));
            }
            else
            {
                IWayAwr wayAwr = (IWayAwr)DetectWayAwrOfWayIfrm_623(wayAwrIfrm as IWayIfrm);
                dynamic d = wayAwr;
                d.FldTypDson = codDson;
                DetermineDsonSttForWayAwr_19082(wayAwr, DetectObjRstOfWayAwr_12949(context, wayAwr));
            }
        }

        private static string GenerateUniqueUsername(BPMNDBEntities context, string nam2ndPsn)
        {
            string result = nam2ndPsn;

            Random rand = new Random(100);

            while (true)
            {
                result = nam2ndPsn;

                int num = rand.Next(999);

                result += num.ToString();

                TblUsr usr = context.TblUsrs.SingleOrDefault(m => m.FldNamUsr == result);

                if (usr == null)//username is valid
                {
                    return result;
                }
            }

            //return result;
        }

        public static bool StsSndObjRstToAct_22034(IObjRst objRst, TblAct act)
        {
            foreach (TblAct item in objRst.ActTarget)
            {
                //22036
                if (item == act)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsPsnAgntOfOrg(TblPsn psns, int fldCodOrg)
        {
            return psns.TblAgntNods.Any(a => a.TblNod.FldCodTypEty == (int)FldTypEty.Org && a.TblNod.FldCodEty == fldCodOrg);
        }

        private static string _DragedItemName;
        private static bool _IsDraging;
        public static string DragedItemName
        {
            get
            {
                return _DragedItemName;
            }
            set
            {
                _DragedItemName = value;
            }

        }
        public static bool IsDraging
        {
            get
            {
                return _IsDraging;
            }
            set
            {
                _IsDraging = value;
            }

        }
    }
}
