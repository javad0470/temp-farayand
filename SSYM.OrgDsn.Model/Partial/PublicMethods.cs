using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Lanes;
using MindFusion.Diagramming.Wpf.Layout;
using MindFusion.Layout;
using SSYM.OrgDsn.Model.BPMNDgm.Model;
using SSYM.OrgDsn.Model.BPMNShapes;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using yWorks.Canvas.Geometry.Structs;
using yWorks.yFiles.UI;
using yWorks.yFiles.UI.Drawing;
using yWorks.yFiles.UI.Model;
using yWorks.Canvas.Geometry;

using yWorks.Canvas;
using yWorks.Canvas.Drawing;
using yWorks.Canvas.Input;
using yWorks.Canvas.Model;
using yWorks.Support;
using yWorks.Support.Annotations;
using yWorks.Support.Extensions;
using yWorks.yFiles.Layout;
using yWorks.yFiles.Layout.Hierarchic;
using yWorks.yFiles.Layout.Hierarchic.Incremental;
using yWorks.yFiles.UI.Input;
using yWorks.yFiles.UI.LabelModels;
using yWorks.yFiles.UI.Drawing.Common;
using SSYM.OrgDsn.Model.BPMNDgm.Styles;
using SSYM.OrgDsn.Model.Base;

namespace SSYM.OrgDsn.Model
{
    public partial class PublicMethods
    {

        /// <summary>
        /// حذف تمامی رخدادهای آغازگری که دارای نحوه آگاهی نیستند از پایگاه داده
        /// </summary>
        /// <param name="context"></param>
        public static void DeleteAllEvtSrtWotWayAwr_21737(BPMNDBEntities context)
        {
            foreach (TblEvtSrt evtSrt in context.TblEvtSrts)
            {
                if (evtSrt.FldTypEvtSrt != (int)EvtSrtType.inSgmtTime && evtSrt.FldTypEvtSrt != (int)EvtSrtType.aftrCdnEvtSrt)
                {
                    if (evtSrt.TblWayAwr_News.Count == 0 &&
                        evtSrt.TblWayAwr_Oral.Count == 0 &&
                        evtSrt.TblWayAwr_RecvInt.Count == 0)
                    {
                        context.DeleteObject(evtSrt);
                    }
                }
            }

            SaveContext(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shpAgntGrpSrtIdpd"></param>
        /// <param name="shpLane"></param>
        /// <returns></returns>
        //public static NodBase DisplayCmnSrtIdpdFromGrpSrtIdpdInDgm_1329(BPMNDBEntities context, Diagram dgm, List<Tuple<NodBase, NodBase>> shpAgntGrpSrtIdpd, Header shpLane)
        //{
        //    NodBase shpAgtCmnSrtIdpd = new NodBase();

        //    //1350
        //    if (shpAgntGrpSrtIdpd.Count == 1)
        //    {
        //        shpAgtCmnSrtIdpd = shpAgntGrpSrtIdpd.First().Item2;
        //    }

        //    //1383
        //    else if (shpAgntGrpSrtIdpd.Count > 1)
        //    {
        //        NodBase shpGatWayEvtBsoExls = DisShpInnColInnDgm_1325(context, dgm, shpLane, Enum.TypShp.G6);

        //        DisplayConShpToShps_1254(dgm, shpGatWayEvtBsoExls, shpAgntGrpSrtIdpd.Select(m => m.Item1).ToList());

        //        NodBase shpGatWayExls = DisShpInnColInnDgm_1325(context, dgm, shpLane, Enum.TypShp.G1);

        //        DisplayConShpsToShp_1255(dgm, shpAgntGrpSrtIdpd.Select(m => m.Item2).ToList(), shpGatWayExls);

        //        shpAgtCmnSrtIdpd = shpGatWayExls;
        //    }

        //    return shpAgtCmnSrtIdpd;
        //}


        public static INode DisplayCmnSrtIdpdFromGrpSrtIdpdInDgm_1329(BPMNDBEntities context, GraphControl grph, INode dgm, List<Tuple<INode, INode>> shpAgntGrpSrtIdpd, IRow shpLane)
        {
            INode shpAgtCmnSrtIdpd;

            //1350
            if (shpAgntGrpSrtIdpd.Count == 1)
            {
                shpAgtCmnSrtIdpd = shpAgntGrpSrtIdpd.First().Item2;
            }

            //1383
            else if (shpAgntGrpSrtIdpd.Count > 1)
            {
                INode shpGatWayEvtBsoExls = DisShpInnColInnDgm_1325(context, grph, dgm, shpLane, Enum.TypShp.G6);

                DisplayConShpToShps_1254(grph, dgm, shpGatWayEvtBsoExls, shpAgntGrpSrtIdpd.Select(m => m.Item1).ToList());

                INode shpGatWayExls = DisShpInnColInnDgm_1325(context, grph, dgm, shpLane, Enum.TypShp.G1);

                DisplayConShpsToShp_1255(grph, dgm, shpAgntGrpSrtIdpd.Select(m => m.Item2).ToList(), shpGatWayExls);

                shpAgtCmnSrtIdpd = shpGatWayExls;
            }

            //for technical reasons----doesnt exist in documentation/

            else
            {
                shpAgtCmnSrtIdpd = grph.Graph.CreateNode();
            }

            return shpAgtCmnSrtIdpd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="evtRst"></param>
        /// <param name="shpAct"></param>
        /// <returns></returns>
        public static INode DisplayEvtRstOfTypAfterActGainAwrNew_3512(BPMNDBEntities context, GraphControl grph, INode dgm, TblEvtRst evtRst, INode shpAct)
        {
            //3519
            if (DetectWayAwrOfEvtSrtWithWayIfrmOfTypSbjOral_495(context, evtRst.TblEvt_GainAwrNew.First().TblEvtSrt).Count > 0 ||
                DetectWayAwrOfEvtSrtWithWayIfrmOfTypObj_499(context, evtRst.TblEvt_GainAwrNew.First().TblEvtSrt).Count > 0)
            {
                TblNod nodSrcEvtSrtTwin = evtRst.TblEvt_GainAwrNew.First().TblEvtSrt.TblAct.TblNod;

                bool osdOrg = IsNodOsdOrg_1085(context, nodSrcEvtSrtTwin, CurrentUser.TblOrg);

                //3513
                if (!osdOrg)
                {
                    INode shpActFomTypRecv = DisplayShpInColInDgm_2318(context, grph, dgm, Enum.TypShp.A2, nodSrcEvtSrtTwin, evtRst, "کسب آگاهی");

                    TblAct actSrcEvtSrtTwin = evtRst.TblEvt_GainAwrNew.First().TblEvtSrt.TblAct;

                    INode shpActSrcEvtSrtTwin = DetectOrDisplayShpActInDgm_3007(context, grph, dgm, actSrcEvtSrtTwin);

                    DisplayConCnulTwoShp_1253(grph, dgm, shpActSrcEvtSrtTwin, shpActFomTypRecv);

                    DisplayConCnulTwoShp_1253(grph, dgm, shpAct, shpActFomTypRecv);

                    if (DetectWayAwrOfEvtSrtWithWayIfrmOfTypObj_499(context, evtRst.TblEvt_GainAwrNew.First().TblEvtSrt).Count > 0)
                    {
                        //DisplayIntInDgm_3601(context, dgm, DetectWayAwrOfEvtSrtWithWayIfrmOfTypObj_499(context,evtRst.TblEvt_GainAwrNew.First().TblEvtSrt).First().TblWayIfrm_SndOut.TblObj,shpLane,
                    }

                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="allActPvs"></param>
        public static void DetectAllActsPvsAct_3089(BPMNDBEntities context, TblAct act, List<TblAct> allActPvs)
        {
            List<TblAct> actPvs = DetectActsPvsAct_3032(context, act);

            foreach (TblAct item in actPvs)
            {
                if (!allActPvs.Contains(item))
                {
                    allActPvs.Add(item);
                }
            }

            foreach (TblAct actPvs_i in actPvs)
            {
                DetectAllActsPvsAct_3089(context, actPvs_i, allActPvs);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="allActPvs"></param>
        public static void DetectAllActsPvsAct_22045(BPMNDBEntities context, TblAct act, List<TblAct> allActPvs)
        {
            List<TblAct> actPvs = DetectActsPvsAct_22046(context, act);

            foreach (TblAct item in actPvs)
            {
                if (!allActPvs.Contains(item))
                {
                    allActPvs.Add(item);
                }
            }

            foreach (TblAct actPvs_i in actPvs)
            {
                DetectAllActsPvsAct_22045(context, actPvs_i, allActPvs);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActsPvsAct_3032(BPMNDBEntities context, TblAct act)
        {
            List<IWayAwr> wayAwr = DetectWayAwrOfAct_3039(context, act);

            //3063
            List<TblAct> actPvs = new List<TblAct>();

            //3053
            foreach (IWayAwr wayAwr_i in wayAwr)
            {
                TblAct act2 = DetectActSrcOfWayAwr_715(context, wayAwr_i);

                //15973
                if (!actPvs.Contains(act2) && wayAwr_i.GetType() != typeof(TblWayAwr_News))
                {
                    actPvs.Add(act2);
                }
            }

            return actPvs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActsPvsAct_22046(BPMNDBEntities context, TblAct act)
        {
            List<IWayAwr> wayAwr = DetectWayAwrOfAct_3039(context, act);

            TblPr prs1 = DetectPrsOfAct_1839(context, act);

            //3063
            List<TblAct> actPvs = new List<TblAct>();

            //3053
            foreach (IWayAwr wayAwr_i in wayAwr)
            {
                TblAct act2 = DetectActSrcOfWayAwr_715(context, wayAwr_i);
                TblPr prs2 = DetectPrsOfAct_1839(context, act2);

                //15973
                //if (!actPvs.Contains(act2) && wayAwr_i.GetType() != typeof(TblWayAwr_News))
                //{
                //    actPvs.Add(act2);
                //}

                if (wayAwr_i.GetType() != typeof(TblWayAwr_News))
                {
                    if (prs2 != null)
                    {
                        if ((prs2.FldSttPrs.Value == (int)SttPrs.Raw) || prs1.FldCodPrs == prs2.FldCodPrs)
                        {
                            actPvs.Add(act2);
                        }
                    }
                }
            }

            return actPvs;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayAwr"></param>
        /// <returns></returns>
        public static TblAct DetectActSrcOfWayAwr_715(BPMNDBEntities context, IWayAwr wayAwr)
        {
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                return ((TblWayAwr_News)wayAwr).TblWayIfrm_News.TblNew.TblEvtRst.TblAct;
            }

            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                return ((TblWayAwr_Oral)wayAwr).TblWayIfrm_Oral.TblSbjOral.TblEvtRst.TblAct;
            }

            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                return ((TblWayAwr_RecvInt)wayAwr).TblWayIfrm_SndOut.TblObj.TblEvtRst.TblAct;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrOfAct_3039(BPMNDBEntities context, TblAct act)
        {
            List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act);

            //3062
            List<IWayAwr> wayAwr = new List<IWayAwr>();

            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                List<IWayAwr> wayAwr1 = DetectWayAwrOfEvtSrt_610(context, evtSrt_i);

                //1283
                wayAwr = wayAwr.Union(wayAwr1).ToList();
            }

            return wayAwr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrOfEvtSrt_610(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<IWayAwr> wayAwr = new List<IWayAwr>();

            wayAwr.AddRange(DetectWayAwrOfEvtSrtOfTypNews_485(context, evtSrt));

            wayAwr.AddRange(DetectWayAwrOfEvtSrtOfTypSbjOral_486(context, evtSrt));

            wayAwr.AddRange(DetectWayAwrOfEvtSrtOfTypObj_487(context, evtSrt));

            return wayAwr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_News> DetectWayAwrOfEvtSrtOfTypNews_485(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            ReloadEntity(context, evtSrt, evtSrt.TblWayAwr_News, "TblWayAwr_News");

            return evtSrt.TblWayAwr_News.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_Oral> DetectWayAwrOfEvtSrtOfTypSbjOral_486(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            ReloadEntity(context, evtSrt, evtSrt.TblWayAwr_Oral, "TblWayAwr_Oral");

            return evtSrt.TblWayAwr_Oral.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static List<TblWayAwr_RecvInt> DetectWayAwrOfEvtSrtOfTypObj_487(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            ReloadEntity(context, evtSrt, evtSrt.TblWayAwr_RecvInt, "TblWayAwr_RecvInt");

            return evtSrt.TblWayAwr_RecvInt.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblEvtSrt> DetectEvtSrtOfAct_452(BPMNDBEntities context, TblAct act)
        {
            ReloadEntity(context, act, act.TblEvtSrts, "TblEvtSrts");

            return act.TblEvtSrts.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActWotEvtRstDepd_3087(BPMNDBEntities context, List<TblAct> act)
        {
            //3152
            List<TblAct> actWotEvtRstDepd = new List<TblAct>();

            //3135
            foreach (TblAct act_i in act)
            {
                actWotEvtRstDepd.Add(act_i);

                List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act_i);

                //3140
                foreach (TblEvtRst evtRst_i in evtRst)
                {
                    bool exit = false;
                    List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst_i);

                    //3144
                    foreach (IWayIfrm wayIfrm_i in wayIfrm)
                    {
                        Enum.StsIdpdWayIfrm stsIdpdWayIfrm = DetectStsIdpdWayIfrm_8852(context, wayIfrm_i);

                        //3155
                        if (stsIdpdWayIfrm == StsIdpdWayIfrm.dependent)
                        {
                            //1719
                            actWotEvtRstDepd.Remove(act_i);

                            //3157
                            exit = true;
                            break;
                        }
                    }

                    if (exit)
                    {
                        break;
                    }
                }
            }

            return actWotEvtRstDepd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static TblAct DetectActDstOfWayIfrm_720(BPMNDBEntities context, IWayIfrm wayIfrm)
        {
            IWayAwr wayAwr = DetectWayAwrOfWayIfrm_623(wayIfrm);

            //21764
            if (wayAwr != null)
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

                return evtSrt.TblAct;
            }

            return null;


        }

        /// <summary>
        /// Transfered to TblNod
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static EntityObject DetectEtyOfNod_1082(BPMNDBEntities context, TblNod nod)
        {
            if (nod.FldCodTypEty == (int)Enum.FldTypEty.Org)
            {
                return context.TblOrgs.SingleOrDefault(m => m.FldCodOrg == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.PosPst)
            {
                return context.TblPosPstOrgs.SingleOrDefault(m => m.FldCodPosPst == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.Psn)
            {
                return context.TblPsns.SingleOrDefault(m => m.FldCodPsn == nod.FldCodEty);
            }

            else if (nod.FldCodTypEty == (int)Enum.FldTypEty.Rol)
            {
                return context.TblRols.SingleOrDefault(m => m.FldCodRol == nod.FldCodEty);
            }

            return null;
        }

        /// <summary>
        /// Transfered
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <param name="orgCnt"></param>
        /// <returns></returns>
        public static bool IsNodOsdOrg_1085(BPMNDBEntities context, TblNod nod, TblOrg orgCnt)
        {
            EntityObject obj = DetectEtyOfNod_1082(context, nod);

            if (obj.GetType() == typeof(TblOrg))
            {
                return ((TblOrg)obj).FldCodOrg != orgCnt.FldCodOrg;
            }

            else if (obj.GetType() == typeof(TblPosPstOrg))
            {
                return ((TblPosPstOrg)obj).FldCodOrg != orgCnt.FldCodOrg;
            }

            else if (obj.GetType() == typeof(TblPsn))
            {
                TblPsn psn = (TblPsn)obj;

                foreach (TblUsr item in psn.TblUsrs)
                {
                    if (item.TblOrg.FldCodOrg == orgCnt.FldCodOrg && psn.FldIsdOrg)
                    {
                        return false;
                    }
                }

                return true;
            }

            else if (obj.GetType() == typeof(TblRol))
            {
                TblRol rol = (TblRol)obj;

                if (rol.FldCodOrg == orgCnt.FldCodOrg && rol.FldIsdOrg)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static List<IWayIfrm> DetectWayIfrmOfEvtRst_1912(BPMNDBEntities context, TblEvtRst evtRst)
        {
            ReloadEntity(context, evtRst, evtRst.TblSbjOrals, "TblSbjOrals");

            ReloadEntity(context, evtRst, evtRst.TblNews, "TblNews");

            ReloadEntity(context, evtRst, evtRst.TblObjs, "TblObjs");

            List<IWayIfrm> lst = new List<IWayIfrm>();

            foreach (TblNew item in evtRst.TblNews)
            {
                lst.AddRange(item.TblWayIfrm_News);
            }

            foreach (TblSbjOral item in evtRst.TblSbjOrals)
            {
                lst.AddRange(item.TblWayIfrm_Oral);
            }

            foreach (TblObj item in evtRst.TblObjs)
            {
                lst.AddRange(item.TblWayIfrm_SndOut);
            }

            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblEvtRst> DetectEvtRstOfAct_453(BPMNDBEntities context, TblAct act)
        {
            ReloadEntity(context, act, act.TblEvtRsts, "TblEvtRsts");

            return act.TblEvtRsts.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        public static List<TblAct> DetectActsOfPrs_946(BPMNDBEntities context, TblPr prs)
        {
            ReloadEntity(context, prs, prs.TblActPrs, "TblActPrs");

            List<TblAct> lst = new List<TblAct>();

            foreach (TblActPr item in prs.TblActPrs)
            {
                lst.Add(item.TblAct);
            }

            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        public static void DeleteActOfPrs_3587(BPMNDBEntities context, TblAct act, TblPr prs)
        {
            List<TblActPr> lst = context.TblActPrs.Where(m => m.FldCodAct == act.FldCodAct && m.FldCodPrs == prs.FldCodPrs).ToList();

            foreach (TblActPr item in lst)
            {
                context.DeleteObject(item);
            }
        }

        /// <summary>
        /// transfered
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static TblPr DetectPrsOfAct_1839(BPMNDBEntities context, TblAct act)
        {
            //ReloadEntity(context, act, act.TblActPrs, "TblActPrs");

            return act.TblActPrs.Count > 0 ? act.TblActPrs.First().TblPr : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="nod"></param>
        public static void AddActAndChgPrs_3202(BPMNDBEntities context, TblAct act, TblNod nod)
        {
            //898
            nod.TblActs.Add(act);

            //1721
            TblPr prsAct = AddPrsRaw_1721(context);

            SetPrsOfAct_1723(act, prsAct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="prsNew"></param>
        public static void SetPrsOfAct_1723(TblAct act, TblPr prsNew)
        {
            TblActPr actPrs = new TblActPr() { FldSttAct = 1 };

            actPrs.TblAct = act;

            actPrs.TblPr = prsNew;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <param name="prsNew"></param>
        public static void SetPrsOfActs_1724(BPMNDBEntities context, List<TblAct> act, TblPr prsNew)
        {
            foreach (TblAct act_i in act)
            {
                DelectActOfPrs_8818(context, act_i);

                TblActPr actPrs = new TblActPr() { FldSttAct = 1 };

                actPrs.TblAct = act_i;

                actPrs.TblPr = prsNew;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        public static void DeleteEvtSrtAndChgPrs_3205(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            TblPr prsAct = DetectPrsOfEvtSrt_2347(context, evtSrt);

            DeleteEvtSrtWthAllRelations_808(context, evtSrt);

            //EditPrsActsOfPrsAfterDelete_8820(context, prsAct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        public static void ImpChgPrsAftrChgActSrcOfEvtSrt_3222(BPMNDBEntities context, TblAct actOth, TblEvtSrt evtSrt)
        {
            //2347
            TblPr prsAct = evtSrt.TblAct.TblActPrs.Count > 0 ? evtSrt.TblAct.TblActPrs.First().TblPr : null;

            List<TblAct> act = DetectActsOfPrs_946(context, prsAct);

            //1839
            TblPr prsActOth = actOth.TblActPrs.Count > 0 ? actOth.TblActPrs.First().TblPr : null;

            List<TblAct> actPrsOth = DetectActsOfPrs_946(context, prsActOth);

            //3245
            if (prsAct == prsActOth && !actOth.FldActUspf)
            {
                //3246
                if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    //3247
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }
            }

                //3240
            else if (prsAct != prsActOth && !actOth.FldActUspf && prsActOth.FldSttPrs == (int)Enum.SttPrs.Raw)
            {
                //3244
                if (prsAct.FldSttPrs == (int)Enum.SttPrs.Raw)
                {
                    DeletePrs_1726(context, prsAct);

                    //1721
                    TblPr prsRaw = AddPrsRaw_1721(context);
                    prsRaw.FldCodOwrPrs = evtSrt.TblAct.TblNod.FldCodNod;

                    SetPrsOfActs_1724(context, act, prsRaw);

                    SetPrsOfActs_1724(context, actPrsOth, prsRaw);
                }

                    //3242
                else if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    //3243
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;

                    SetPrsOfActs_1724(context, actPrsOth, prsAct);

                    DeletePrs_1726(context, prsActOth);
                }

                    //3241
                else if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed)
                {
                    SetPrsOfActs_1724(context, actPrsOth, prsAct);

                    DeletePrs_1726(context, prsActOth);
                }
            }

                //3232
            else if (prsAct != prsActOth && !actOth.FldActUspf && prsActOth.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
            {
                //3238
                if (prsAct.FldSttPrs == (int)Enum.SttPrs.Raw)
                {
                    SetPrsOfActs_1724(context, act, prsActOth);

                    DeletePrs_1726(context, prsAct);

                    //3239
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }

                    //3235
                else if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    //3237
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;

                    //3236
                    prsActOth.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }

                    //3233
                else if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed)
                {
                    //3234
                    prsActOth.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }
            }

                //3228
            else if (prsAct != prsActOth && !actOth.FldActUspf && prsActOth.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed)
            {
                //3231
                if (prsAct.FldSttPrs == (int)Enum.SttPrs.Raw)
                {
                    SetPrsOfActs_1724(context, act, prsActOth);

                    DeletePrs_1726(context, prsAct);
                }

                    //3229
                else if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    //3230
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }
            }

                //3225
            else if (actOth.FldActUspf)
            {
                //3226
                if (prsAct.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    //3227
                    prsAct.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        public static void DeletePrs_1726(BPMNDBEntities context, params TblPr[] prs)
        {
            foreach (TblPr item in prs)
            {
                context.DeleteObject(item);
            }
        }

        public static void DeletePrs_1726(BPMNDBEntities context, List<TblPr> prs)
        {
            foreach (TblPr item in prs)
            {
                context.DeleteObject(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        public static void ImpChgPrsAftrDelWayAwrOfEvtSrt_3249(BPMNDBEntities context, TblEvtSrt evtSrt, IWayAwr wayAwr, bool stsExsSrcPelUpl = false)
        {
            TblPr prsAct = DetectPrsOfEvtSrt_2347(context, evtSrt);

            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_716(wayAwr);

            IObjRst objRst = DetectObjRstWayIfrm_721(context, wayIfrm);

            DeleteObjRstFromWayAwrAndChgPrs_6668(context, objRst, wayAwr, stsExsSrcPelUpl);




            //// با توجه به اینکه تابع 723 که داخل تابع 6668 فراخوانی شده حذف نحوه آگاه سازی را از سمت راست انجام می دهد نیاز است که نحوه آگاهی در این تابع مستقیم پاک شود
            //if ((wayAwr as EntityObject).EntityState == System.Data.EntityState.Detached)
            //{
            //    context.Attach(wayAwr as EntityObject);
            //}

            //context.DeleteObject(wayAwr);




            //List<IWayAwr> wayAwr1 = DetectWayAwrOfEvtSrt_610(context, evtSrt);

            ////5634
            //if (wayAwr1.Count == 0)
            //{
            //    DeleteEvtSrtAndChgPrs_3205(context, evtSrt);
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <param name="wayAwr"></param>
        public static void ImpChgPrsAftrAddWayAwrOfEvtSrt_3260(BPMNDBEntities context, TblEvtSrt evtSrt, IWayAwr wayAwr)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <param name="evtSrtDepd"></param>
        /// <param name="evtSrtIdpd"></param>
        public static void DetectEvtSrtDepdAndNotDepd_2276(BPMNDBEntities context, List<TblEvtSrt> evtSrt, List<TblEvtSrt> evtSrtDepd, List<TblEvtSrt> evtSrtIdpd)
        {
            //2277
            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                bool stsIdpdEvtSrt = DetectSttDepdEvtSrt_2883(context, evtSrt_i);

                if (stsIdpdEvtSrt)
                {
                    evtSrtIdpd.Add(evtSrt_i);
                }

                else
                {
                    evtSrtDepd.Add(evtSrt_i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <param name="typWayAwr"></param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrsOfWvtSrt_2904(BPMNDBEntities context, TblEvtSrt evtSrt, Enum.TypWayAwr typWayAwr)
        {
            switch (typWayAwr)
            {
                case TypWayAwr.News:

                    return new List<IWayAwr>(DetectWayAwrOfEvtSrtOfTypNews_485(context, evtSrt));

                case TypWayAwr.SbjOral:

                    return new List<IWayAwr>(DetectWayAwrOfEvtSrtOfTypSbjOral_486(context, evtSrt));

                case TypWayAwr.Obj:

                    return new List<IWayAwr>(DetectWayAwrOfEvtSrtOfTypObj_487(context, evtSrt));

                default:

                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static bool DetectSttDepdEvtSrt_2883(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            //2890
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.inSgmtTime)
            {
                return true;
            }

            //2889
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.aftrCdnEvtSrt)
            {
                return true;
            }

            TblNod nodSrcEvtSrt = DetectNodSrcEvtSrc_2891(context, evtSrt);

            bool stsOsdOrgNodEvtSrt = IsNodOsdOrg_1085(context, nodSrcEvtSrt, CurrentUser.TblOrg);

            //2888
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt && stsOsdOrgNodEvtSrt)
            {
                return true;
            }

            List<IWayAwr> wayAwrFomTypNews = DetectWayAwrsOfWvtSrt_2904(context, evtSrt, TypWayAwr.News);

            //2887
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt && !stsOsdOrgNodEvtSrt && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            //2895
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.aftrAnyCdnEvtSrt && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            //2898
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.spcCdnEvtSrtAftr && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            //2901
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.errAccurEvtSrt && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            //2899
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.cancelEvtSrt && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            //2900
            if (evtSrt.FldTypEvtSrt == (int)EvtSrtType.spcCdnEvtSrt && wayAwrFomTypNews.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shpLane"></param>
        /// <param name="typShp"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        //public static NodBase DisShpInnColInnDgm_1325_OLD(BPMNDBEntities context, Diagram dgm, Header shpLane, TypShp typShp, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        //{
        //    BPMNShapes.ShpBase sn;

        //    if ((int)typShp >= 1 && (int)typShp <= 8)
        //    {
        //        sn = new BPMNShapes.ShpGateway(typShp);
        //    }

        //    else if ((int)typShp >= 9 && (int)typShp <= 42)
        //    {
        //        sn = new BPMNShapes.ShpEvt(typShp);
        //    }

        //    else if ((int)typShp >= 43 && (int)typShp <= 57)
        //    {
        //        sn = new BPMNShapes.ShpActivity(typShp);
        //    }

        //    else if ((int)typShp == 58)
        //    {
        //        sn = new BPMNShapes.ShpObj(typShp);
        //    }

        //    else if ((int)typShp == 59)
        //    {
        //        sn = new BPMNShapes.ShpMsg(typShp);
        //    }

        //    else
        //    {
        //        sn = new BPMNShapes.ShpNodOsdOrg(typShp);
        //    }

        //    if (etyHmlgWthShp != null)
        //    {
        //        if (etyHmlgWthShp.GetType() == typeof(TblAct))
        //        {
        //            ((TblAct)etyHmlgWthShp).Shp = sn;
        //        }

        //        else if (etyHmlgWthShp.GetType() == typeof(TblEvtRst))
        //        {
        //            ((TblEvtRst)etyHmlgWthShp).Shp = sn;
        //        }

        //        else if (etyHmlgWthShp.GetType() == typeof(TblEvtSrt))
        //        {
        //            ((TblEvtSrt)etyHmlgWthShp).Shp = sn;
        //        }

        //        else if (etyHmlgWthShp.GetType() == typeof(TblPr))
        //        {
        //            ((TblPr)etyHmlgWthShp).Shp = sn;
        //        }
        //    }

        //    if (shpLane != null)
        //    {
        //        sn.LayoutTraits[SwimlaneLayoutTraits.Lane] = dgm.LaneGrid.RowHeaders.IndexOf(shpLane);

        //        sn.hdr = shpLane;
        //    }

        //    dgm.Nodes.Add(sn);

        //    return sn;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="i"></param>
        /// <param name="siz"></param>
        /// <returns></returns>
        private static RectD FindRect(IRow Row, SizeD siz)
        {
            return new yWorks.Canvas.Geometry.Structs.RectD(new PointD(Row.Layout.ToRectD().X, Row.Layout.ToRectD().Y), siz);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shpLane"></param>
        /// <param name="typShp"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        public static INode DisShpInnColInnDgm_1325(BPMNDBEntities context, GraphControl grph, INode dgm, IRow shpLane, TypShp typShp, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        {
            INode nod;

            //ILabelStyle lblStyle = new SimpleLabelStyle(new Typeface("B Koodak"), 16, Brushes.DarkSeaGreen);

            //DefaultLabelModelParameterFinder lpf = new DefaultLabelModelParameterFinder();

            if (typShp == TypShp.LNE)
            {
                Table tbl = new Table();

                IRow row = tbl.CreateRow(50);

                tbl.CreateColumn(200);

                nod = grph.Graph.GetGroupedGraph().CreateGroupNode(grph.Graph.GetGroupedGraph().Hierarchy.Root, new RectD(new PointD(10, 10), tbl.Layout.GetSize()),
                                                             new TableNodeStyle(tbl)
                                                             {
                                                                 TableRenderingOrder = TableRenderingOrder.RowsFirst,
                                                                 BackgroundStyle = new ShapeNodeStyle() { Brush = new SolidColorBrush(Colors.Orange) }
                                                             });

                if (etyHmlgWthShp.GetType() == typeof(TblNod))
                {
                    ((TblNod)etyHmlgWthShp).GrpNod = nod;

                    ((TblNod)etyHmlgWthShp).Tbl = tbl;

                    ((TblNod)etyHmlgWthShp).Col = row;
                }
            }

            else if ((int)typShp >= 1 && (int)typShp <= 8)
            {
                GatewayNodeStyle style = new GatewayNodeStyle { Gateway = new Gateway { Type = typShp } };

                style.Gateway.Col = shpLane;

                nod = grph.Graph.GetGroupedGraph().CreateNode(dgm, FindRect(shpLane, style.Gateway.Siz), style);
            }

            else if (((int)typShp >= 9 && (int)typShp <= 42) || ((int)typShp >= 62 && (int)typShp <= 64))
            {
                EventNodeStyle style = new EventNodeStyle { Event = new Event { Type = typShp } };

                style.Event.Col = shpLane;

                nod = grph.Graph.GetGroupedGraph().CreateNode(dgm, FindRect(shpLane, style.Event.Siz), style);

                if (etyHmlgWthShp != null)
                {
                    if (etyHmlgWthShp.GetType() == typeof(TblEvtRst))
                    {
                        ((TblEvtRst)etyHmlgWthShp).Shp = nod;
                    }

                    else if (etyHmlgWthShp.GetType() == typeof(TblNew))
                    {
                        ((TblNew)etyHmlgWthShp).Shp = nod;

                        grph.Graph.AddLabel(nod, ExteriorLabelModel.South, ((TblNew)etyHmlgWthShp).FldTtlNews);
                    }

                    else if (etyHmlgWthShp.GetType() == typeof(TblObj))
                    {
                        ((TblObj)etyHmlgWthShp).Shp = nod;

                        grph.Graph.AddLabel(nod, ExteriorLabelModel.South, ((TblObj)etyHmlgWthShp).FldNamObj);
                    }

                    else if (etyHmlgWthShp.GetType() == typeof(TblSbjOral))
                    {
                        ((TblSbjOral)etyHmlgWthShp).Shp = nod;
                    }

                    else if (etyHmlgWthShp.GetType() == typeof(TblEvtSrt))
                    {
                        ((TblEvtSrt)etyHmlgWthShp).Shp = nod;
                    }
                }

            }

            else if ((int)typShp >= 43 && (int)typShp <= 57)
            {
                ActivityNodeStyle style = new ActivityNodeStyle { Activity = new Activity { Type = typShp } };

                style.Activity.Col = shpLane;

                nod = grph.Graph.GetGroupedGraph().CreateNode(dgm, FindRect(shpLane, style.Activity.Siz), style);

                if (etyHmlgWthShp.GetType() == typeof(TblAct))
                {
                    grph.Graph.AddLabel(nod, InteriorLabelModel.Center, etyHmlgWthShp != null ? ((TblAct)etyHmlgWthShp).FldNamAct : ttlShp);
                }

                else
                {
                    grph.Graph.AddLabel(nod, InteriorLabelModel.Center, etyHmlgWthShp != null ? ((TblPr)etyHmlgWthShp).FldNamPrs : ttlShp);
                }

                if (etyHmlgWthShp.GetType() == typeof(TblAct))
                {
                    ((TblAct)etyHmlgWthShp).Shp = nod;
                }

            }

            else
            {
                ArtifactNodeStyle style = new ArtifactNodeStyle { Artifact = new Artifact { Type = typShp } };

                style.Artifact.Col = shpLane;

                nod = grph.Graph.GetGroupedGraph().CreateNode(dgm, FindRect(shpLane, style.Artifact.Siz), style);

                if (etyHmlgWthShp.GetType() == typeof(TblObj))
                {
                    if (typShp == TypShp.DO1)
                    {
                        ((TblObj)etyHmlgWthShp).Shp = nod;
                    }

                    else if (typShp == TypShp.DO2)
                    {
                        ((TblObj)etyHmlgWthShp).Shp1 = nod;
                    }


                    grph.Graph.AddLabel(nod, ExteriorLabelModel.South, ((TblObj)etyHmlgWthShp).FldNamObj);
                }

                else if (etyHmlgWthShp.GetType() == typeof(TblNew))
                {
                    ((TblNew)etyHmlgWthShp).Shp1 = nod;

                    grph.Graph.AddLabel(nod, ExteriorLabelModel.South, ((TblNew)etyHmlgWthShp).FldTtlNews);
                }

                else
                {
                    ((TblSbjOral)etyHmlgWthShp).Shp1 = nod;

                    grph.Graph.AddLabel(nod, ExteriorLabelModel.South, ttlShp ?? string.Empty);
                }
            }

            return nod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        /// <param name="stsExsHedArw"></param>
        //public static void DisConDconTwoShps_2919_OLD(Diagram dgm, NodBase shp1, NodBase shp2, bool stsExsHedArw)
        //{
        //    DiagramLink lnk = new DiagramLink(dgm, shp1, shp2);

        //    //lnk.Origin = shp1;

        //    //lnk.Destination = shp2;

        //    lnk.HeadShape = stsExsHedArw ? ArrowHeads.Triangle : ArrowHeads.None;

        //    lnk.Stroke = Brushes.Blue;

        //    lnk.HeadStroke = Brushes.Blue;

        //    lnk.HeadShapeSize = 10;

        //    lnk.Brush = Brushes.Blue;

        //    lnk.StrokeDashStyle = DashStyles.Dash;

        //    dgm.Links.Add(lnk);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        /// <param name="stsExsHedArw"></param>
        public static void DisConDconTwoShps_2919(GraphControl grph, INode dgm, INode shp1, INode shp2, bool stsExsHedArw)
        {
            if ((shp1.Style.GetType() == typeof(ArtifactNodeStyle) && ((ArtifactNodeStyle)shp1.Style).Artifact.Type == TypShp.DO2) ||
                (shp2.Style.GetType() == typeof(ArtifactNodeStyle) && ((ArtifactNodeStyle)shp2.Style).Artifact.Type == TypShp.DO2))
            {
                if (stsExsHedArw)
                {
                    grph.Graph.CreateEdge(shp1, shp2, new RelationEdgeStyle { Relation = new Relation { Type = RelationType.MessageFlow } });
                }

                else
                {
                    grph.Graph.CreateEdge(shp1, shp2, new RelationEdgeStyle { Relation = new Relation { Type = RelationType.NonDirectedMessageFlow } });
                }
            }

            else
            {
                if (stsExsHedArw)
                {
                    grph.Graph.CreateEdge(shp1, shp2, new RelationEdgeStyle { Relation = new Relation { Type = RelationType.DirectedAssociation } });
                }

                else
                {
                    grph.Graph.CreateEdge(shp1, shp2, new RelationEdgeStyle { Relation = new Relation { Type = RelationType.Association } });
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        //public static NodBase DisShpBtwnColIsdAndOsdOrg_2950(Diagram dgm, TypShp typShp)
        //{

        //    BPMNShapes.ShpBase sn;

        //    if ((int)typShp >= 1 && (int)typShp <= 8)
        //    {
        //        sn = new BPMNShapes.ShpGateway(typShp);
        //    }

        //    else if ((int)typShp >= 9 && (int)typShp <= 42)
        //    {
        //        sn = new BPMNShapes.ShpEvt(typShp);
        //    }

        //    else if ((int)typShp >= 43 && (int)typShp <= 57)
        //    {
        //        sn = new BPMNShapes.ShpActivity(typShp);
        //    }

        //    else if ((int)typShp == 58)
        //    {
        //        sn = new BPMNShapes.ShpObj(typShp);
        //    }

        //    else
        //    {
        //        sn = new BPMNShapes.ShpMsg(typShp);
        //    }

        //    sn.Id = 0;

        //    sn.LayoutTraits[SwimlaneLayoutTraits.Lane] = 1;

        //    sn.hdr = dgm.LaneGrid.RowHeaders[1];

        //    dgm.Nodes.Add(sn);

        //    return sn;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <returns></returns>
        public static INode DisShpBtwnColIsdAndOsdOrg_2950(GraphControl grph, INode dgm, TypShp typShp)
        {
            INode nod;

            if ((int)typShp >= 1 && (int)typShp <= 8)
            {
                GatewayNodeStyle style = new GatewayNodeStyle { Gateway = new Gateway { Type = typShp } };

                nod = grph.Graph.CreateNode(new PointD(0, 0), style);
            }

            else if ((int)typShp >= 9 && (int)typShp <= 42)
            {
                EventNodeStyle style = new EventNodeStyle { Event = new Event { Type = typShp } };

                nod = grph.Graph.CreateNode(new PointD(0, 0), style);
            }

            else if ((int)typShp >= 43 && (int)typShp <= 57)
            {
                ActivityNodeStyle style = new ActivityNodeStyle { Activity = new Activity { Type = typShp } };

                nod = grph.Graph.CreateNode(new PointD(0, 0), style);
            }

            else if ((int)typShp >= 58 && (int)typShp <= 59)
            {
                ArtifactNodeStyle style = new ArtifactNodeStyle { Artifact = new Artifact { Type = typShp } };

                nod = grph.Graph.CreateNode(new PointD(0, 0), style);
            }

            else
            {
                nod = grph.Graph.CreateNode();
            }

            return nod;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="ety"></param>
        /// <returns></returns>
        public static object DetectShpEqualToEty_2326(BPMNDBEntities context, INode dgm, EntityObject ety, TypShp typShp = TypShp.A1)
        {
            if (ety.GetType() == typeof(TblNod))
            {
                if (!IsNodOsdOrg_1085(context, (TblNod)ety, CurrentUser.TblOrg))
                {
                    return ((TblNod)ety).Col;
                }

                else
                {
                    return ((TblNod)ety).GrpNod;
                }
            }

            else if (ety.GetType() == typeof(TblAct))
            {
                return ((TblAct)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblEvtRst))
            {
                return ((TblEvtRst)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblEvtSrt))
            {
                return ((TblEvtSrt)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblPr))
            {
                return ((TblPr)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblNew))
            {
                return ((TblNew)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblObj))
            {
                if (typShp == TypShp.DO2)
                {
                    return ((TblObj)ety).Shp1;
                }

                return ((TblObj)ety).Shp;
            }

            else if (ety.GetType() == typeof(TblSbjOral))
            {
                return ((TblSbjOral)ety).Shp;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        //public static Header DisplayColForNodIsdOrgInDgm_3013_OLD(Diagram dgm, TblNod nod)
        //{
        //    Header h = new MindFusion.Diagramming.Wpf.Lanes.Header();

        //    if (nod.HdrNod == null)
        //    {
        //        h.FontSize = 14;

        //        h.RotateTitle = true;

        //        h.Height = 100;

        //        h.Text = nod.FldNamNod;

        //        nod.HdrNod = h;

        //        dgm.LaneGrid.RowHeaders.Add(h);
        //    }

        //    return h;
        //}


        public static IRow DisplayColForNodIsdOrgInDgm_3013(INode dgm, TblNod nod)
        {
            Table tbl = (Table)(dgm.Style as TableNodeStyle).Table;

            IRow row = tbl.CreateRow(100);

            nod.Tbl = tbl;

            nod.GrpNod = dgm;

            nod.Col = row;

            return row;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static INode DisplayColForNodOsdOrgInDgm_3012(GraphControl grph, INode dgm, TblNod nod)
        {
            Table tbl = new Table();

            IRow row = tbl.CreateRow(100);

            tbl.CreateColumn(200);

            INode groupNod = grph.Graph.GetGroupedGraph().CreateGroupNode(grph.Graph.GetGroupedGraph().Hierarchy.Root, new RectD(new PointD(10, 10), tbl.Layout.GetSize()),
                                                         new TableNodeStyle(tbl)
                                                         {
                                                             TableRenderingOrder = TableRenderingOrder.RowsFirst,
                                                             BackgroundStyle = new ShapeNodeStyle() { Brush = new SolidColorBrush(Colors.LightBlue) }
                                                         });

            nod.Tbl = tbl;

            nod.GrpNod = dgm;

            nod.Col = row;

            return groupNod;
        }


        public static object DetectOrDisplayColOfNod_2953(BPMNDBEntities context, GraphControl grph, INode dgm, TblNod nod)
        {
            object shpLane = DetectShpEqualToEty_2326(context, dgm, nod);

            if (shpLane == null)
            {
                bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nod, CurrentUser.TblOrg);

                if (!stsOsdOrgNod)
                {
                    shpLane = DisplayColForNodIsdOrgInDgm_3013(dgm, nod);
                }

                else
                {
                    shpLane = DisplayColForNodOsdOrgInDgm_3012(grph, dgm, nod);
                }
            }

            return shpLane;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="evtSrtIdpd"></param>
        /// <param name="shpLaneAct"></param>
        /// <param name="stsExsGrpSrtDepdInnGrpSrtRef"></param>
        /// <param name="shpAct"></param>
        /// <returns></returns>
        public static INode DisplayEvtSrtIdpdInDgm_2200(BPMNDBEntities context, GraphControl grph, INode dgm, TblEvtSrt evtSrtIdpd, IRow shpLaneAct, bool stsExsGrpSrtDepdInnGrpSrtRef, INode shpAct)
        {
            //2201
            if (evtSrtIdpd.FldTypEvtSrt == (int)Enum.EvtSrtType.inSgmtTime)
            {
                //2207
                if (!stsExsGrpSrtDepdInnGrpSrtRef)
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E1, evtSrtIdpd);
                }
                //2209
                else
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E4, evtSrtIdpd);
                }
            }
            //2203
            else if (evtSrtIdpd.FldTypEvtSrt == (int)Enum.EvtSrtType.aftrCdnEvtSrt)
            {
                //2210
                if (!stsExsGrpSrtDepdInnGrpSrtRef)
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E7, evtSrtIdpd);
                }
                //2211
                else
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E10, evtSrtIdpd);
                }
            }

                //2202
            else if (evtSrtIdpd.FldTypEvtSrt == (int)Enum.EvtSrtType.aftrAwareEvtSrt)
            {
                List<IWayAwr> wayAwrFomTypRecvInt = DetectWayAwrsOfWvtSrt_2904(context, evtSrtIdpd, TypWayAwr.Obj);

                //2943
                if (wayAwrFomTypRecvInt.Count > 0)
                {
                    TblNod nodSrc = DetectNodSrcEvtSrc_2891(context, evtSrtIdpd);

                    object shpLaneNodSrc = DetectOrDisplayColOfNod_2953(context, grph, dgm, nodSrc);

                    INode shpEvtSrtIdpd;

                    //2215
                    if (!stsExsGrpSrtDepdInnGrpSrtRef)
                    {
                        shpEvtSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E13, evtSrtIdpd);
                    }
                    //2216
                    else
                    {
                        shpEvtSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E16, evtSrtIdpd);
                    }

                    INode shpMsg = DisShpBtwnColIsdAndOsdOrg_2950(grph, dgm, TypShp.DO2);

                    DisConDconTwoShps_2919(grph, dgm, (INode)shpLaneNodSrc, shpMsg, false);

                    DisConDconTwoShps_2919(grph, dgm, shpMsg, shpEvtSrtIdpd, true);

                    return shpEvtSrtIdpd;
                }

                List<IWayAwr> wayAwrFomTypOral = DetectWayAwrsOfWvtSrt_2904(context, evtSrtIdpd, TypWayAwr.SbjOral);

                //2957
                if (wayAwrFomTypOral.Count > 0)
                {
                    TblNod nodSrc = DetectNodSrcEvtSrc_2891(context, evtSrtIdpd);

                    object shpLaneNodSrc = DetectOrDisplayColOfNod_2953(context, grph, dgm, nodSrc);

                    INode shpEvtSrtIdpd;

                    //2959
                    if (!stsExsGrpSrtDepdInnGrpSrtRef)
                    {
                        shpEvtSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E13, evtSrtIdpd);
                    }
                    //2958
                    else
                    {
                        shpEvtSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E16, evtSrtIdpd);
                    }

                    DisConDconTwoShps_2919(grph, dgm, (INode)shpLaneNodSrc, shpEvtSrtIdpd, true);

                    return shpEvtSrtIdpd;
                }

                List<IWayAwr> wayAwrFomTypNews = DetectWayAwrsOfWvtSrt_2904(context, evtSrtIdpd, TypWayAwr.News);

                if (wayAwrFomTypNews.Count > 0)
                {
                    //2947
                    if (!stsExsGrpSrtDepdInnGrpSrtRef)
                    {
                        return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E26, evtSrtIdpd);
                    }
                    //2948
                    else
                    {
                        return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E29, evtSrtIdpd);
                    }
                }

            }
            //2969
            else
            {
                //2972
                if (!stsExsGrpSrtDepdInnGrpSrtRef)
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E26, evtSrtIdpd);
                }
                //2971
                else
                {
                    return DisShpInnColInnDgm_1325(context, grph, dgm, shpLaneAct, TypShp.E29, evtSrtIdpd);
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static TblNod DetectNodSrcEvtSrc_2891(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            TblAct actSrc = DetectActSrcOfEvtSrt_586(context, evtSrt);

            return actSrc.TblNod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        //public static void DisplayConCnulTwoShp_1253_OLD(Diagram dgm, ShpBase shp1, ShpBase shp2)
        //{
        //    DiagramLink lnk = new DiagramLink(dgm, shp1, shp2);

        //    //lnk.Origin = shp1;

        //    //lnk.Destination = shp2;

        //    lnk.HeadShape = ArrowHeads.Triangle;

        //    lnk.Stroke = Brushes.Blue;

        //    lnk.HeadStroke = Brushes.Blue;

        //    lnk.HeadShapeSize = 10;

        //    lnk.Brush = Brushes.Blue;

        //    lnk.StrokeDashStyle = DashStyles.Solid;

        //    dgm.Links.Add(lnk);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        public static void DisplayConCnulTwoShp_1253(GraphControl grph, INode dgm, INode shp1, INode shp2)
        {
            grph.Graph.CreateEdge(shp1, shp2, new RelationEdgeStyle { Relation = new Relation { Type = RelationType.SequenceFlow } });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        //public static void DisplayConShpToShps_1254(Diagram dgm, NodBase shp1, List<NodBase> shp2)
        //{
        //    foreach (NodBase shp2_i in shp2)
        //    {
        //        DisplayConCnulTwoShp_1253(dgm, shp1, shp2_i);
        //    }
        //}


        public static void DisplayConShpToShps_1254(GraphControl grph, INode dgm, INode shp1, List<INode> shp2)
        {
            foreach (INode shp2_i in shp2)
            {
                DisplayConCnulTwoShp_1253(grph, dgm, shp1, shp2_i);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        //public static void DisplayConShpsToShp_1255_OLD(Diagram dgm, List<NodBase> shp1, NodBase shp2)
        //{
        //    foreach (NodBase shp1_i in shp1)
        //    {
        //        DisplayConCnulTwoShp_1253(dgm, shp1_i, shp2);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp1"></param>
        /// <param name="shp2"></param>
        public static void DisplayConShpsToShp_1255(GraphControl grph, INode dgm, List<INode> shp1, INode shp2)
        {
            foreach (INode shp1_i in shp1)
            {
                DisplayConCnulTwoShp_1253(grph, dgm, shp1_i, shp2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shpLane"></param>
        /// <param name="prs"></param>
        /// <param name="shpAct"></param>
        public static void DisplayGrpSrtInDgm_1418(BPMNDBEntities context, GraphControl grph, INode dgm, List<TblEvtSrt> evtSrt, IRow shpLane, TblPr prs, INode shpAct, ref INode shpAgnt1stGrpSrt, ref INode shpAgnt2ndGrpSrt)
        {
            bool stsExsGrpSrtDepdInnGrpSrtRef = false;
            INode shpAgntGrpSrtDepd;

            //2276
            List<TblEvtSrt> evtSrtDepd = new List<TblEvtSrt>();
            List<TblEvtSrt> evtSrtIdpd = new List<TblEvtSrt>();
            DetectEvtSrtDepdAndNotDepd_2276(context, evtSrt, evtSrtDepd, evtSrtIdpd);

            //2231
            if (evtSrtIdpd.Count > 0)
            {
                //3000
                if (evtSrtDepd.Count > 0)
                {
                    stsExsGrpSrtDepdInnGrpSrtRef = true;
                }

                //2284
                else
                {
                    stsExsGrpSrtDepdInnGrpSrtRef = false;
                }

                //1414
                INode shpAgnt1stGrpSrtIdpd = null;
                INode shpAgnt2ndGrpSrtIdpd = null;
                DisplayGrpSrtIdpdInDgm_1414(context, grph, dgm, evtSrtIdpd, stsExsGrpSrtDepdInnGrpSrtRef, shpLane, shpAct, ref shpAgnt1stGrpSrtIdpd, ref shpAgnt2ndGrpSrtIdpd);

                //3003
                if (evtSrtDepd.Count > 0)
                {
                    shpAgntGrpSrtDepd = DisplayGrpSrtDepdInDgm_1415(context, grph, dgm, prs, evtSrtDepd, shpAct);

                    DisplayConCnulTwoShp_1253(grph, dgm, shpAgntGrpSrtDepd, shpAgnt1stGrpSrtIdpd);

                    shpAgnt1stGrpSrt = shpAgnt2ndGrpSrtIdpd;

                    shpAgnt2ndGrpSrt = null;
                }

                //3005
                else
                {
                    shpAgnt1stGrpSrt = shpAgnt1stGrpSrtIdpd;

                    shpAgnt2ndGrpSrt = shpAgnt2ndGrpSrtIdpd;
                }
            }

            //2232
            else
            {
                //15995
                if (evtSrtDepd.Count > 0)
                {
                    shpAgntGrpSrtDepd = DisplayGrpSrtDepdInDgm_1415(context, grph, dgm, prs, evtSrtDepd, shpAct);

                    shpAgnt1stGrpSrt = shpAgntGrpSrtDepd;
                    shpAgnt2ndGrpSrt = null;
                }

                //for technical reasons---- doesnt exist in documentation/
                else
                {
                    shpAgnt1stGrpSrt = grph.Graph.CreateNode();
                    shpAgnt2ndGrpSrt = grph.Graph.CreateNode();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="prs"></param>
        /// <param name="evtSrtDepd"></param>
        /// <param name="shpAct"></param>
        /// <returns></returns>
        //public static NodBase DisplayGrpSrtDepdInDgm_1415(BPMNDBEntities context, Diagram dgm, TblPr prs, List<TblEvtSrt> evtSrtDepd, NodBase shpAct)
        //{
        //    List<NodBase> shpAgntEvtSrtDepd = new List<NodBase>();

        //    NodBase shpAgntGrpSrtDepd = new NodBase();

        //    NodBase shpAgntEvtSrtDepd_X = new NodBase();

        //    //1417
        //    foreach (TblEvtSrt evtSrtDepd_i in evtSrtDepd)
        //    {
        //        shpAgntEvtSrtDepd_X = DisplayEvtSrtDepdInDgm_1450(context, dgm, evtSrtDepd_i, prs, shpAct);

        //        shpAgntEvtSrtDepd.Add(shpAgntEvtSrtDepd_X);
        //    }

        //    //2255
        //    if (shpAgntEvtSrtDepd.Count == 1)
        //    {
        //        shpAgntGrpSrtDepd = shpAgntEvtSrtDepd_X;
        //    }
        //    //2256
        //    else if (shpAgntEvtSrtDepd.Count > 1)
        //    {
        //        shpAgntGrpSrtDepd = DisShpInnColInnDgm_1325(context, dgm, shpAct.hdr, TypShp.G3);

        //        DisplayConShpsToShp_1255(dgm, shpAgntEvtSrtDepd, shpAgntGrpSrtDepd);
        //    }

        //    return shpAgntGrpSrtDepd;
        //}


        public static INode DisplayGrpSrtDepdInDgm_1415(BPMNDBEntities context, GraphControl grph, INode dgm, TblPr prs, List<TblEvtSrt> evtSrtDepd, INode shpAct)
        {
            List<INode> shpAgntEvtSrtDepd = new List<INode>();

            INode shpAgntGrpSrtDepd = null;

            INode shpAgntEvtSrtDepd_X = null;

            //1417
            foreach (TblEvtSrt evtSrtDepd_i in evtSrtDepd)
            {
                shpAgntEvtSrtDepd_X = DisplayEvtSrtDepdInDgm_1450(context, grph, dgm, evtSrtDepd_i, prs, shpAct);

                shpAgntEvtSrtDepd.Add(shpAgntEvtSrtDepd_X);
            }

            //2255
            if (shpAgntEvtSrtDepd.Count == 1)
            {
                shpAgntGrpSrtDepd = shpAgntEvtSrtDepd_X;
            }
            //2256
            else if (shpAgntEvtSrtDepd.Count > 1)
            {
                shpAgntGrpSrtDepd = DisShpInnColInnDgm_1325(context, grph, dgm, (shpAct.Style as ActivityNodeStyle).Activity.Col, TypShp.G3);

                DisplayConShpsToShp_1255(grph, dgm, shpAgntEvtSrtDepd, shpAgntGrpSrtDepd);
            }

            return shpAgntGrpSrtDepd;
        }


        public static void DisplaySrtOfActInDgm_1262(BPMNDBEntities context, GraphControl grph, INode dgm, TblAct act)
        {
            //---------//
            //List<ShpBase> shpAgntGrpSrtIdpd = new List<ShpBase>();
            List<Tuple<INode, INode>> shpAgntGrpSrtIdpd = new List<Tuple<INode, INode>>();
            List<INode> shpAgntGrpSrtUnnIdpd = new List<INode>();
            List<INode> shpAgntGrpAndCmn = new List<INode>();
            INode shpAgnt1stGrpSrt = null;
            INode shpAgnt2ndGrpSrt = null;
            //---------//

            List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act);

            //22135
            if (evtSrt.Count == 0)
            {
                INode shpEvtSrtUspf = DisShpInnColInnDgm_1325(context, grph, dgm, (act.Shp.Style as IStyle).Col, TypShp.E34, null, null);
                DisplayConCnulTwoShp_1253(grph, dgm, shpEvtSrtUspf, act.Shp);

            }
            //22136
            else
            {
                //2373
                for (int i = 0; i < DetectTnoGrpOfEvtSrtOfAct_13947(context, act); i++)
                {
                    List<TblEvtSrt> evtSrtGrp = evtSrt.Where(m => m.FldGrpEvt == (DetectEvtSrtOfAct_452(context, act).Select(s => s.FldGrpEvt).Distinct().ToList())[i]).ToList();

                    //2374
                    if (evtSrtGrp.Count == 0)
                    {
                        continue;
                    }

                    //2377
                    else
                    {
                        //ShpBase shpHmlg = DetectOrDisplayShpActInDgm_3007(context, dgm, act);

                        //1418
                        DisplayGrpSrtInDgm_1418(context, grph, dgm, evtSrtGrp, act.TblNod.Col, DetectPrsOfAct_1839(context, act), (INode)DetectShpEqualToEty_2326(context, dgm, act), ref shpAgnt1stGrpSrt, ref shpAgnt2ndGrpSrt);

                        //2380
                        if (shpAgnt2ndGrpSrt != null)
                        {
                            //1284
                            shpAgntGrpSrtIdpd.Add(new Tuple<INode, INode>(shpAgnt1stGrpSrt, shpAgnt2ndGrpSrt));
                        }
                        //2381
                        else
                        {
                            //1424
                            shpAgntGrpSrtUnnIdpd.Add(shpAgnt1stGrpSrt);
                        }
                    }
                }

                //2527
                if (shpAgntGrpSrtIdpd.Count > 0)
                {
                    //1329
                    //ShpBase shp1stAgntGrpSrtIdpd = new ShpBase();
                    //ShpBase shp2stAgntGrpSrtIdpd = new ShpBase();
                    //Tuple<ShpBase, ShpBase> t = new Tuple<ShpBase, ShpBase>(shpAgnt1stGrpSrt, shpAgnt2ndGrpSrt);
                    //List<Tuple<ShpBase, ShpBase>> lst = new List<Tuple<ShpBase, ShpBase>>();
                    //lst.Add(t);
                    INode shpAgtCmnSrtIdpd = DisplayCmnSrtIdpdFromGrpSrtIdpdInDgm_1329(context, grph, dgm, shpAgntGrpSrtIdpd, act.TblNod.Col);

                    //1283
                    shpAgntGrpSrtUnnIdpd.Add(shpAgtCmnSrtIdpd);
                    shpAgntGrpAndCmn = shpAgntGrpSrtUnnIdpd;
                }

                //19104
                if (shpAgntGrpSrtIdpd.Count == 0)
                {
                    shpAgntGrpAndCmn = shpAgntGrpSrtUnnIdpd;
                }

                //2528
                if (shpAgntGrpAndCmn.Count == 1)
                {
                    DisplayConCnulTwoShp_1253(grph, dgm, shpAgntGrpAndCmn.First(), act.Shp);
                }

                //2529
                else if (shpAgntGrpAndCmn.Count > 1)
                {
                    INode shpGatWayExls = DisShpInnColInnDgm_1325(context, grph, dgm, act.TblNod.Col, TypShp.G1);

                    DisplayConShpsToShp_1255(grph, dgm, shpAgntGrpAndCmn, shpGatWayExls);

                    DisplayConCnulTwoShp_1253(grph, dgm, shpGatWayExls, act.Shp);
                }


            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="act"></param>
        //public static void DisplaySrtOfActInDgm_1262(BPMNDBEntities context, Diagram dgm, TblAct act)
        //{
        //    //---------//
        //    //List<ShpBase> shpAgntGrpSrtIdpd = new List<ShpBase>();
        //    List<Tuple<NodBase, NodBase>> shpAgntGrpSrtIdpd = new List<Tuple<NodBase, NodBase>>();
        //    List<NodBase> shpAgntGrpSrtUnnIdpd = new List<NodBase>();
        //    List<NodBase> shpAgntGrpAndCmn = new List<NodBase>();
        //    NodBase shpAgnt1stGrpSrt = new NodBase();
        //    NodBase shpAgnt2ndGrpSrt = new NodBase();
        //    //---------//

        //    List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act);

        //    //2373
        //    for (int i = 0; i < DetectTnoGrpOfEvtSrtOfAct_13947(context, act); i++)
        //    {
        //        List<TblEvtSrt> evtSrtGrp = evtSrt.Where(m => m.FldGrpEvt == (DetectEvtSrtOfAct_452(context, act).Select(s => s.FldGrpEvt).Distinct().ToList())[i]).ToList();

        //        //2374
        //        if (evtSrtGrp.Count == 0)
        //        {
        //            continue;
        //        }

        //        //2377
        //        else
        //        {
        //            //ShpBase shpHmlg = DetectOrDisplayShpActInDgm_3007(context, dgm, act);

        //            //1418
        //            DisplayGrpSrtInDgm_1418(context, dgm, evtSrtGrp, act.TblNod.HdrNod, DetectPrsOfAct_1839(context, act), (NodBase)DetectShpEqualToEty_2326(context, dgm, act), ref shpAgnt1stGrpSrt, ref shpAgnt2ndGrpSrt);

        //            //2380
        //            if (shpAgnt2ndGrpSrt != null)
        //            {
        //                //1284
        //                shpAgntGrpSrtIdpd.Add(new Tuple<NodBase, NodBase>(shpAgnt1stGrpSrt, shpAgnt2ndGrpSrt));
        //            }
        //            //2381
        //            else
        //            {
        //                //1424
        //                shpAgntGrpSrtUnnIdpd.Add(shpAgnt1stGrpSrt);
        //            }
        //        }
        //    }

        //    //2527
        //    if (shpAgntGrpSrtIdpd.Count > 0)
        //    {
        //        //1329
        //        //ShpBase shp1stAgntGrpSrtIdpd = new ShpBase();
        //        //ShpBase shp2stAgntGrpSrtIdpd = new ShpBase();
        //        //Tuple<ShpBase, ShpBase> t = new Tuple<ShpBase, ShpBase>(shpAgnt1stGrpSrt, shpAgnt2ndGrpSrt);
        //        //List<Tuple<ShpBase, ShpBase>> lst = new List<Tuple<ShpBase, ShpBase>>();
        //        //lst.Add(t);
        //        NodBase shpAgtCmnSrtIdpd = DisplayCmnSrtIdpdFromGrpSrtIdpdInDgm_1329(context, dgm, shpAgntGrpSrtIdpd, act.TblNod.HdrNod);

        //        //1283
        //        shpAgntGrpSrtUnnIdpd.Add(shpAgtCmnSrtIdpd);
        //        shpAgntGrpAndCmn = shpAgntGrpSrtUnnIdpd;
        //    }

        //    //19104
        //    if (shpAgntGrpSrtIdpd.Count == 0)
        //    {
        //        shpAgntGrpAndCmn = shpAgntGrpSrtUnnIdpd;
        //    }

        //    //2528
        //    if (shpAgntGrpAndCmn.Count == 1)
        //    {
        //        DisplayConCnulTwoShp_1253(dgm, shpAgntGrpAndCmn.First(), act.Shp);
        //    }

        //    //2529
        //    else if (shpAgntGrpAndCmn.Count > 1)
        //    {
        //        NodBase shpGatWayExls = DisShpInnColInnDgm_1325(context, dgm, act.TblNod.HdrNod, TypShp.G1);

        //        DisplayConShpsToShp_1255(dgm, shpAgntGrpAndCmn, shpGatWayExls);

        //        DisplayConCnulTwoShp_1253(dgm, shpGatWayExls, act.Shp);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayAwr"></param>
        /// <returns></returns>
        public static TblAct DetectActDstOfWayAwr_19101(IWayAwr wayAwr)
        {
            if (wayAwr.GetType() == typeof(TblWayAwr_News))
            {
                return ((TblWayAwr_News)wayAwr).TblEvtSrt.TblAct;
            }

            else if (wayAwr.GetType() == typeof(TblWayAwr_Oral))
            {
                return ((TblWayAwr_Oral)wayAwr).TblEvtSrt.TblAct;
            }

            else if (wayAwr.GetType() == typeof(TblWayAwr_RecvInt))
            {
                return ((TblWayAwr_RecvInt)wayAwr).TblEvtSrt.TblAct;
            }

            throw new InvalidOperationException("parameter is not a wayAwr");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static int DetectTnoGrpOfEvtSrtOfAct_13947(BPMNDBEntities context, TblAct act)
        {
            return DetectEvtSrtOfAct_452(context, act).Select(m => m.FldGrpEvt).Distinct().Count();
        }

        public static void DisplayGrpSrtIdpdInDgm_1414(BPMNDBEntities context,
            GraphControl grph,
           INode dgm,
           List<TblEvtSrt> evtSrtIdpd,
           bool stsExsGrpSrtDepdInnGrpSrtRef,
           IRow shpLane,
           INode shpAct,
           ref INode shpAgnt1stGrpSrtIdpd,
           ref INode shpAgnt2ndGrpSrtIdpd)
        {
            List<INode> shpEvtSrtIdpd = new List<INode>();

            //2181
            foreach (TblEvtSrt evtSrtIdpd_i in evtSrtIdpd)
            {
                INode shpEvtSrt = DisplayEvtSrtIdpdInDgm_2200(context, grph, dgm, evtSrtIdpd_i, shpLane, stsExsGrpSrtDepdInnGrpSrtRef, shpAct);

                shpEvtSrtIdpd.Add(shpEvtSrt);
            }

            if (shpEvtSrtIdpd.Count == 1)
            {
                shpAgnt1stGrpSrtIdpd = shpEvtSrtIdpd[0];

                shpAgnt2ndGrpSrtIdpd = shpEvtSrtIdpd[0];
            }

            else if (shpEvtSrtIdpd.Count > 1)
            {
                //1436
                if (!stsExsGrpSrtDepdInnGrpSrtRef)
                {
                    shpAgnt1stGrpSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLane, TypShp.G8);
                }

                    //1437
                else
                {
                    shpAgnt1stGrpSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLane, TypShp.G7);
                }

                DisplayConShpToShps_1254(grph, dgm, shpAgnt1stGrpSrtIdpd, shpEvtSrtIdpd);

                shpAgnt2ndGrpSrtIdpd = DisShpInnColInnDgm_1325(context, grph, dgm, shpLane, TypShp.G3);

                DisplayConShpsToShp_1255(grph, dgm, shpEvtSrtIdpd, shpAgnt2ndGrpSrtIdpd);
            }


            //for technical reasons--- doesnt exist in documentation/
            else
            {
                shpAgnt1stGrpSrtIdpd = grph.Graph.CreateNode();
                shpAgnt2ndGrpSrtIdpd = grph.Graph.CreateNode();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="evtSrtIdpd"></param>
        /// <param name="stsExsGrpSrtDepdInnGrpSrtRef"></param>
        /// <param name="shpLane"></param>
        /// <param name="shpAct"></param>
        /// <param name="shpAgnt1stGrpSrtIdpd"></param>
        /// <param name="shpAgnt2ndGrpSrtIdpd"></param>
        //public static void DisplayGrpSrtIdpdInDgm_1414_OLD(BPMNDBEntities context,
        //    Diagram dgm,
        //    List<TblEvtSrt> evtSrtIdpd,
        //    bool stsExsGrpSrtDepdInnGrpSrtRef,
        //    Header shpLane,
        //    NodBase shpAct,
        //    ref NodBase shpAgnt1stGrpSrtIdpd,
        //    ref NodBase shpAgnt2ndGrpSrtIdpd)
        //{
        //    List<NodBase> shpEvtSrtIdpd = new List<NodBase>();

        //    //2181
        //    foreach (TblEvtSrt evtSrtIdpd_i in evtSrtIdpd)
        //    {
        //        NodBase shpEvtSrt = DisplayEvtSrtIdpdInDgm_2200(context, dgm, evtSrtIdpd_i, shpLane, stsExsGrpSrtDepdInnGrpSrtRef, shpAct);

        //        shpEvtSrtIdpd.Add(shpEvtSrt);
        //    }

        //    if (shpEvtSrtIdpd.Count == 1)
        //    {
        //        shpAgnt1stGrpSrtIdpd = shpEvtSrtIdpd[0];

        //        shpAgnt2ndGrpSrtIdpd = shpEvtSrtIdpd[0];
        //    }

        //    else if (shpEvtSrtIdpd.Count > 1)
        //    {
        //        //1436
        //        if (!stsExsGrpSrtDepdInnGrpSrtRef)
        //        {
        //            shpAgnt1stGrpSrtIdpd = DisShpInnColInnDgm_1325(context, dgm, shpLane, TypShp.G8);
        //        }

        //            //1437
        //        else
        //        {
        //            shpAgnt1stGrpSrtIdpd = DisShpInnColInnDgm_1325(context, dgm, shpLane, TypShp.G7);
        //        }

        //        DisplayConShpToShps_1254(dgm, shpAgnt1stGrpSrtIdpd, shpEvtSrtIdpd);

        //        shpAgnt2ndGrpSrtIdpd = DisShpInnColInnDgm_1325(context, dgm, shpLane, TypShp.G3);

        //        DisplayConShpsToShp_1255(dgm, shpEvtSrtIdpd, shpAgnt2ndGrpSrtIdpd);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <param name="shpOth"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        /// <returns></returns>
        //public static ShpBase DislayShpAttachedToAnotherShp_2330_OLD(BPMNDBEntities context, Diagram dgm, Enum.TypShp typShp, ShpBase shpOth, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        //{
        //    Group grp;

        //    if (dgm.Groups.SingleOrDefault(m => m.MainItem == shpOth) == null)
        //    {
        //        grp = new Group(dgm);

        //        grp.MainItem = shpOth;

        //        dgm.Groups.Add(grp);
        //    }

        //    else
        //    {
        //        grp = dgm.Groups.SingleOrDefault(m => m.MainItem == shpOth);
        //    }

        //    ShpBase shp = DisShpInnColInnDgm_1325(context, dgm, null, typShp, etyHmlgWthShp, ttlShp);

        //    AttachTwoNodes_19250(grp, shpOth, shp);

        //    return shp;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="grph"></param>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <param name="shpOth"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        /// <returns></returns>
        public static INode DislayShpAttachedToAnotherShp_2330(BPMNDBEntities context, GraphControl grph, INode dgm, Enum.TypShp typShp, INode shpOth, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        {
            INode nod;

            EventNodeStyle style = new EventNodeStyle { Event = new Event { Type = typShp } };

            style.Event.Col = (shpOth.Style as ActivityNodeStyle).Activity.Col;

            RectD rec = new RectD(new PointD(shpOth.Layout.ToRectD().X, shpOth.Layout.ToRectD().Y), style.Event.Siz);

            //RectD rec = new RectD(new PointD(-5,-5), style.Event.Siz);

            //nod = grph.Graph.GetGroupedGraph().CreateNode(shpOth, rec, style);

            nod = grph.Graph.CreateNode(rec, style);


            //if ((shpOth.Style as ActivityNodeStyle).Activity.TblChld == null)
            //{
            //    //grph.Graph.GetGroupedGraph().CreateNode(shpOth, rec, new ShapeNodeStyle(ShapeNodeShape.RoundRectangle, new Pen(Brushes.Transparent, 0), Brushes.Transparent));

            //    //grph.Graph.GetGroupedGraph().CreateNode(shpOth, rec, new ShapeNodeStyle(ShapeNodeShape.RoundRectangle, new Pen(Brushes.Transparent, 0), Brushes.Transparent));

            //    //yWorks.yFiles.UI.Model.Table tbl = new yWorks.yFiles.UI.Model.Table();

            //    //tbl.CreateRow(style.Event.Siz.Height);

            //    //tbl.CreateColumn(style.Event.Siz.Width);

            //    //tbl.CreateGrid(1, 1);

            //    //INode tblPrn = grph.Graph.GetGroupedGraph().CreateNode(shpOth, rec, 
            //        //new TableNodeStyle(tbl)
            //        //{
            //        //    TableRenderingOrder = TableRenderingOrder.RowsFirst,
            //        //    BackgroundStyle = new ShapeNodeStyle() { Brush = new SolidColorBrush(Colors.Transparent) }
            //        //});

            //    //nod = grph.Graph.GetGroupedGraph().CreateNode(tblPrn, new RectD(new PointD(tbl.Columns.First().Layout.ToRectD().X, tbl.Rows.First().Layout.ToRectD().Y), style.Event.Siz), style);

            //    //(shpOth.Style as ActivityNodeStyle).Activity.TblChld = tblPrn;
            //}

            //else
            //{
            //    INode tbl = (shpOth.Style as ActivityNodeStyle).Activity.TblChld;

            //    (tbl.Style as TableNodeStyle).Table.CreateColumn(style.Event.Siz.Width);

            //    int i = ((tbl.Style as TableNodeStyle).Table as Table).Columns.Count();

            //    double x = ((tbl.Style as TableNodeStyle).Table as Table).Columns.Skip(i-1).First().Layout.ToRectD().X;

            //    double y = ((tbl.Style as TableNodeStyle).Table as Table).Rows.First().Layout.ToRectD().Y;

            //    RectD rec2 = new RectD(new PointD(x, y),style.Event.Siz);

            //    //nod = grph.Graph.GetGroupedGraph().CreateNode(tbl, rec2, style);
            //}


            if (etyHmlgWthShp.GetType() == typeof(TblAct))
            {
                ((TblAct)etyHmlgWthShp).Shp = nod;
            }

            else if (etyHmlgWthShp.GetType() == typeof(TblEvtRst))
            {
                ((TblEvtRst)etyHmlgWthShp).Shp = nod;
            }

            else if (etyHmlgWthShp.GetType() == typeof(TblEvtSrt))
            {
                ((TblEvtSrt)etyHmlgWthShp).Shp = nod;
            }

            else if (etyHmlgWthShp.GetType() == typeof(TblPr))
            {
                ((TblPr)etyHmlgWthShp).Shp = nod;
            }



            (shpOth.Style as ActivityNodeStyle).Activity.AttachedNodes.Add(nod);

            return nod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="shpPrn"></param>
        /// <param name="shpChld"></param>
        //public static void AttachTwoNodes_19250(Group grp, NodBase shpPrn, NodBase shpChld)
        //{
        //    double i = 0;

        //    foreach (NodBase item in grp.AttachedNodes)
        //    {
        //        if (item != shpPrn)
        //        {
        //            i += item.Bounds.Width;
        //        }
        //    }

        //    i += shpChld.Bounds.Width / 2;

        //    if (i > shpPrn.Bounds.Width)
        //    {
        //        shpPrn.Bounds = new Rect(new Size() { Width = i });
        //    }

        //    grp.AttachProportional(shpChld, 0, 0, 0, 100);

        //    Point p = new Point() { X = shpPrn.Bounds.X + i, Y = shpPrn.Bounds.Y + shpPrn.Bounds.Height - 10 };

        //    Size s = new Size() { Width = shpChld.Bounds.Width, Height = shpChld.Bounds.Height };

        //    shpChld.Bounds = new Rect(p, s);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="shp"></param>
        /// <param name="typShpConEed"></param>
        /// <returns></returns>
        //public static List<NodBase> DetectStsConShpToShps_2327_OLD(Diagram dgm, ShpBase shp, params Enum.TypShp[] typShpConEed)
        //{
        //    List<ShpBase> lst = new List<NodBase>();

        //    List<ShpBase> targetShapes = new List<NodBase>();

        //    foreach (DiagramLink lnk in shp.OutgoingLinks)
        //    {
        //        if (!targetShapes.Contains(lnk.Destination))
        //        {
        //            targetShapes.Add((NodBase)lnk.Destination);
        //        }
        //    }

        //    foreach (Enum.TypShp item in typShpConEed)
        //    {
        //        foreach (ShpBase item1 in targetShapes)
        //        {
        //            if (item1.Typ == item)
        //            {
        //                lst.Add(item1);
        //            }
        //        }
        //    }

        //    return lst;
        //}

        public static List<INode> DetectStsConShpToShps_2327(GraphControl grph, INode dgm, INode shp, params TypShp[] typShpConEed)
        {
            List<INode> lst = new List<INode>();

            List<INode> targetShapes = new List<INode>();

            foreach (IEdge lnk in grph.Graph.OutEdgesAt(shp))
            {
                if (!targetShapes.Contains(lnk.TargetPort.Owner))
                {
                    targetShapes.Add((INode)lnk.TargetPort.Owner);
                }
            }

            foreach (TypShp item in typShpConEed)
            {
                foreach (INode item1 in targetShapes)
                {
                    if (item1.Style.GetType() == typeof(ActivityNodeStyle) && ((ActivityNodeStyle)item1.Style).Activity.Type == item)
                    {
                        lst.Add(item1);
                    }

                    else if (item1.Style.GetType() == typeof(EventNodeStyle) && ((EventNodeStyle)item1.Style).Event.Type == item)
                    {
                        lst.Add(item1);
                    }

                    else if (item1.Style.GetType() == typeof(GatewayNodeStyle) && ((GatewayNodeStyle)item1.Style).Gateway.Type == item)
                    {
                        lst.Add(item1);
                    }
                }
            }

            return lst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="shp"></param>
        /// <param name="typShpConEed"></param>
        /// <returns></returns>
        //public static List<NodBase> DetectOrDisplayTypShpConEedToShp_3104_OLD(BPMNDBEntities context, Diagram dgm, NodBase shp, Enum.TypShp typShpConEed)
        //{
        //    List<NodBase> shpConEed = DetectStsConShpToShps_2327(dgm, shp, typShpConEed);

        //    if (shpConEed.Count == 0)
        //    {
        //        NodBase shpConEed_1 = DisShpInnColInnDgm_1325(context, dgm, shp.hdr, typShpConEed);

        //        DisplayConCnulTwoShp_1253(dgm, shp, shpConEed_1);

        //        return new List<NodBase>() { shpConEed_1 };
        //    }

        //    else
        //    {
        //        return shpConEed;
        //    }
        //}

        public static List<INode> DetectOrDisplayTypShpConEedToShp_3104(BPMNDBEntities context, GraphControl grph, INode dgm, INode shp, Enum.TypShp typShpConEed)
        {
            List<INode> shpConEed = DetectStsConShpToShps_2327(grph, dgm, shp, typShpConEed);

            //3107
            if (shpConEed.Count == 0)
            {
                INode shpConEed_1 = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shp), typShpConEed);

                DisplayConCnulTwoShp_1253(grph, dgm, shp, shpConEed_1);

                return new List<INode>() { shpConEed_1 };
            }

                //7816
            else
            {
                return shpConEed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shp"></param>
        /// <returns></returns>
        private static IRow DetectColOfShp(INode shp)
        {
            //if (shp.Style.GetType() == typeof(ActivityNodeStyle))
            //{
            //    return ((ActivityNodeStyle)shp.Style).Activity.Col;
            //}

            //else if (shp.Style.GetType() == typeof(EventNodeStyle))
            //{
            //    return ((EventNodeStyle)shp.Style).Event.Col;
            //}

            //else if (shp.Style.GetType() == typeof(GatewayNodeStyle))
            //{
            //    return ((GatewayNodeStyle)shp.Style).Gateway.Col; ;
            //}

            //else
            //{
            //    return null;
            //}

            return ((IStyle)shp.Style).Col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="obj"></param>
        /// <param name="shpLane"></param>
        /// <param name="shpAct1"></param>
        /// <param name="shpAct2"></param>
        /// <returns></returns>
        //public static NodBase DisplayIntInDgm_3601(BPMNDBEntities context, Diagram dgm, TblObj obj, Header shpLane, NodBase shpAct1, NodBase shpAct2)
        //{
        //    NodBase shpObj = DisShpInnColInnDgm_1325(context, dgm, shpLane, TypShp.DO1, obj);

        //    DisConDconTwoShps_2919(dgm, shpAct1, shpObj, false);

        //    DisConDconTwoShps_2919(dgm, shpObj, shpAct2, true);

        //    return shpObj;
        //}


        public static INode DisplayIntInDgm_3601(BPMNDBEntities context, GraphControl grph, INode dgm, TblObj obj, INode shpAct1, INode shpAct2)
        {
            INode shpObj = (INode)DetectShpEqualToEty_2326(context, dgm, (EntityObject)obj);

            //22008
            if (shpObj == null)
            {
                shpObj = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpAct1), TypShp.DO1, obj);

                DisConDconTwoShps_2919(grph, dgm, shpAct1, shpObj, false);
            }

            DisConDconTwoShps_2919(grph, dgm, shpObj, shpAct2, true);

            return shpObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static IRow DetectStsDisColNodInDgm_2323(TblNod nod)
        {
            if (nod == null)
                return null;
            return nod.Col;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        //public static object DisplayColNodInDgm_2325_OLD(BPMNDBEntities context, Diagram dgm, TblNod nod)
        //{
        //    bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nod, CurrentUser.TblOrg);

        //    if (stsOsdOrgNod)
        //    {
        //        return DisplayColForNodOsdOrgInDgm_3012(dgm, nod);
        //    }

        //    else
        //    {
        //        return DisplayColForNodIsdOrgInDgm_3013(dgm, nod);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static object DisplayColNodInDgm_2325(BPMNDBEntities context, GraphControl grph, INode dgm, TblNod nod)
        {
            bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nod, CurrentUser.TblOrg);

            if (stsOsdOrgNod)
            {
                return DisplayColForNodOsdOrgInDgm_3012(grph, dgm, nod);
            }

            else
            {
                return DisplayColForNodIsdOrgInDgm_3013(dgm, nod);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <param name="nodOwrLane"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        /// <returns></returns>
        //public static NodBase DisplayShpInColInDgm_2318_OLD(BPMNDBEntities context, Diagram dgm, TypShp typShp, TblNod nodOwrLane, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        //{
        //    Header shpLane = DetectStsDisColNodInDgm_2323(nodOwrLane);

        //    if (shpLane == null)
        //    {
        //        DisplayColNodInDgm_2325(context, dgm, nodOwrLane);
        //    }

        //    return DisShpInnColInnDgm_1325(context, dgm, DetectStsDisColNodInDgm_2323(nodOwrLane), typShp, etyHmlgWthShp, ttlShp);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="typShp"></param>
        /// <param name="nodOwrLane"></param>
        /// <param name="etyHmlgWthShp"></param>
        /// <param name="ttlShp"></param>
        /// <returns></returns>
        public static INode DisplayShpInColInDgm_2318(BPMNDBEntities context, GraphControl grph, INode dgm, TypShp typShp, TblNod nodOwrLane, EntityObject etyHmlgWthShp = null, string ttlShp = null)
        {
            IRow shpLane = DetectStsDisColNodInDgm_2323(nodOwrLane);

            if (shpLane == null)
            {
                DisplayColNodInDgm_2325(context, grph, dgm, nodOwrLane);
            }

            return DisShpInnColInnDgm_1325(context, grph, dgm, DetectStsDisColNodInDgm_2323(nodOwrLane), typShp, etyHmlgWthShp, ttlShp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        //public static NodBase DetectOrDisplayShpActInDgm_3007_OLD(BPMNDBEntities context, Diagram dgm, TblAct act)
        //{
        //    if (!act.FldActUspf)
        //    {
        //        NodBase shpHmlg = (NodBase)DetectShpEqualToEty_2326(context, dgm, act);

        //        //3010
        //        if (shpHmlg == null)
        //        {
        //            shpHmlg = act.Shp = DisplayShpInColInDgm_2318(context, dgm, TypShp.A1, act.TblNod, act);
        //        }

        //        return shpHmlg;
        //    }

        //    else
        //    {
        //        return DisplayShpInColInDgm_2318(context, dgm, TypShp.A1, act.TblNod, act);
        //    }
        //}


        public static INode DetectOrDisplayShpActInDgm_3007(BPMNDBEntities context, GraphControl grph, INode dgm, TblAct act)
        {
            if (!act.FldActUspf)
            {
                INode shpHmlg = (INode)DetectShpEqualToEty_2326(context, dgm, act);

                //3010
                if (shpHmlg == null)
                {
                    shpHmlg = act.Shp = DisplayShpInColInDgm_2318(context, grph, dgm, TypShp.A1, act.TblNod, act);
                }

                return shpHmlg;
            }

            else
            {
                return DisplayShpInColInDgm_2318(context, grph, dgm, TypShp.A1, act.TblNod, act);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="posPst"></param>
        /// <param name="act"></param>
        public static void AddActToPosPst_756(BPMNDBEntities context, TblPosPstOrg posPst, TblAct act)
        {
            TblNod nod = DetectNodOfPosPst_753(context, posPst);

            nod.TblActs.Add(act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prsGntr"></param>
        /// <param name="numSgmt"></param>
        /// <returns></returns>
        public static TblPr AddSgmtPrsFromPrsConsolidated_1728(BPMNDBEntities context, TblPr prsGntr, int numSgmt)
        {
            TblPr prs = new TblPr();

            CopyAllAtrributesOfPrsInPrsOth_21754(context, prsGntr, prs);

            prs.FldNamPrs = prs.FldNamPrs + "- قطعه" + numSgmt;

            foreach (TblNamPrpsPr item in prs.TblNamPrpsPrs)
            {
                item.FldNamPrpsPrs = item.FldNamPrpsPrs + " قطعه - " + numSgmt;
            }

            prs.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;

            context.TblPrs.AddObject(prs);

            return prs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="news"></param>
        /// <returns></returns>
        public static List<TblWayIfrm_News> DetectWayIfrmOfNews_587(BPMNDBEntities context, TblNew news)
        {
            ReloadEntity(context, news, news.TblWayIfrm_News, "TblWayIfrm_News");

            return news.TblWayIfrm_News.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sbjOral"></param>
        /// <returns></returns>
        public static List<TblWayIfrm_Oral> DetectWayIfrmOfSbjOral_588(BPMNDBEntities context, TblSbjOral sbjOral)
        {
            ReloadEntity(context, sbjOral, sbjOral.TblWayIfrm_Oral, "TblWayIfrm_Oral");

            return sbjOral.TblWayIfrm_Oral.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static List<TblNew> DetectNewsOfEvtRst_573(BPMNDBEntities context, TblEvtRst evtRst)
        {
            ReloadEntity(context, evtRst, evtRst.TblNews, "TblNews");

            return evtRst.TblNews.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static List<TblObj> DetectObjsOfEvtRst_574(BPMNDBEntities context, TblEvtRst evtRst)
        {
            ReloadEntity(context, evtRst, evtRst.TblObjs, "TblObjs");

            return evtRst.TblObjs.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static List<TblSbjOral> DetectSbjOralsOfEvtRst_575(BPMNDBEntities context, TblEvtRst evtRst)
        {
            ReloadEntity(context, evtRst, evtRst.TblSbjOrals, "TblSbjOrals");

            return evtRst.TblSbjOrals.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="news"></param>
        public static void DeleteNews_854(BPMNDBEntities context, TblNew news)
        {
            List<TblEvtSrt> evtSrt = DetectEvtsrtOfNews_651(context, news);

            //855
            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt_i);

                //856
                if (wayAwr.Count == 1 && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt_i);
                }
            }

            context.DeleteObject(news);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        public static void DeleteObj_866(BPMNDBEntities context, TblObj obj)
        {
            List<TblEvtSrt> evtSrt = DetectEvtsrtOfObj_657(context, obj);

            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt_i);

                if (wayAwr.Count == 1 && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt_i);
                }
            }

            context.DeleteObject(obj);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sbjOral"></param>
        public static void DeleteSbjOral_861(BPMNDBEntities context, TblSbjOral sbjOral)
        {
            List<TblEvtSrt> evtSrt = DetectEvtsrtOfSbjOral_654(context, sbjOral);

            foreach (TblEvtSrt evtSrt_i in evtSrt)
            {
                List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt_i);

                if (wayAwr.Count == 1 && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrAftr && evtSrt_i.FldTypEvtSrt != (int)EvtSrtType.getNewAwrInnTim)
                {
                    context.DeleteObject(evtSrt_i);
                }
            }

            context.DeleteObject(sbjOral);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static TblPr DetectPrsOfEvtSrt_2347(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            TblAct act = evtSrt.TblAct;

            return DetectPrsOfAct_1839(context, act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static IObjRst DetectObjRstWayIfrm_721(BPMNDBEntities context, IWayIfrm wayIfrm)
        {
            if (wayIfrm.GetType() == typeof(TblWayIfrm_News))
            {
                return ((TblWayIfrm_News)wayIfrm).TblNew;
            }

            else if (wayIfrm.GetType() == typeof(TblWayIfrm_Oral))
            {
                return ((TblWayIfrm_Oral)wayIfrm).TblSbjOral;
            }

            else if (wayIfrm.GetType() == typeof(TblWayIfrm_SndOut))
            {
                return ((TblWayIfrm_SndOut)wayIfrm).TblObj;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="wayAwr"></param>
        public static void DeleteObjRstFromWayAwrAndChgPrs_6668(BPMNDBEntities context, IObjRst objRst, IWayAwr wayAwr, bool stsExsSrcPelUpl = false)
        {
            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_716(wayAwr);

            TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

            TblPr prsAct1 = DetectPrsOfEvtSrt_2347(context, evtSrt);

            //17985
            if (wayIfrm != null)
            {
                TblAct act = wayIfrm.ObjRst.EvtRst.TblAct;

                TblPr prsAct2 = DetectPrsOfEvtRst_2329(context, wayIfrm.ObjRst.EvtRst);

                DeleteWayIfrm_723(context, wayIfrm, DirectionForDelete.Right);

                //21770
                if (!stsExsSrcPelUpl)
                {
                    //21747
                    if (prsAct1 == prsAct2)
                    {
                        //17986
                        if (objRst.GetType() != typeof(TblNew))
                        {
                            EditPrsActsOfPrsAfterDelete_8820(context, prsAct2);
                        }
                    }
                }


                //21748
                if (prsAct1 != prsAct2)
                {
                    EditPrsActsOfPrsAfterAdd_8830(context, prsAct1, act, prsAct2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static TblNew CreateNewsLikNewsOth_643(TblNew news)
        {
            TblNew news2 = new TblNew() { FldTtlNews = news.FldTtlNews, FldTxtNews = news.FldTxtNews };

            return news2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TblObj CreateObjLikObjOth_644(TblObj obj)
        {
            TblObj obj2 = new TblObj() { FldNamObj = obj.FldNamObj, FldTypObj = obj.FldTypObj };

            return obj2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbjOral"></param>
        /// <returns></returns>
        public static TblSbjOral CreateSbjOralLikSbjOralOth_645(TblSbjOral sbjOral)
        {
            TblSbjOral sbjOral2 = new TblSbjOral();

            return sbjOral2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objRst"></param>
        /// <param name="wayAwr"></param>
        public static void AddExistingObjRstToWayAwrAndChgPrs_6692(BPMNDBEntities context, IObjRst objRst, IWayAwr wayAwr)
        {
            TblEvtSrt evtSrt1 = DetectEvtSrtOfWayAwr_674(wayAwr);

            DeleteObjRstFromWayAwrAndChgPrs_6668(context, objRst, wayAwr);

            AddWayAwrToEvtSrt_624(evtSrt1, wayAwr);

            AddExistingObjRstToWayAwr_1479(context, objRst, wayAwr);

            //17988
            if (objRst.GetType() != typeof(TblNew))
            {
                TblAct actOth = DetectActSrcOfObjRst_551(objRst);

                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwr);

                TblPr prsAct = DetectPrsOfEvtSrt_2347(context, evtSrt);

                TblPr prsActOth = DetectPrsOfAct_1839(context, actOth);

                EditPrsActsOfPrsAfterAdd_8830(context, prsAct, actOth, prsActOth);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="prsAct1"></param>
        /// <param name="act2"></param>
        /// <param name="prsAct2"></param>
        public static void EditPrsActsOfPrsAfterAdd_8830(BPMNDBEntities context, TblPr prsAct1, TblAct act, TblPr prsAct2)
        {
            //8831
            if (!act.FldActUspf)
            {
                //8849
                if (prsAct1 == prsAct2)
                {
                    //8850
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                    {
                        prsAct1.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                    }
                }

                    //8844
                else if (prsAct2.FldSttPrs == (int)Enum.SttPrs.Raw && prsAct2 != prsAct1)
                {
                    //8848
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.Raw)
                    {
                        TblPr prsRaw = AddPrsRaw_1721(context);

                        List<TblAct> act1 = DetectActsOfPrs_946(context, prsAct1);

                        SetPrsOfActs_1724(context, act1, prsRaw);

                        List<TblAct> act2 = DetectActsOfPrs_946(context, prsAct2);

                        SetPrsOfActs_1724(context, act2, prsRaw);

                        DeletePrs_1726(context, prsAct1, prsAct2);
                    }

                    //8846
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                    {
                        List<TblAct> act2 = DetectActsOfPrs_946(context, prsAct2);

                        prsAct1.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;

                        SetPrsOfActs_1724(context, act2, prsAct1);

                        DeletePrs_1726(context, prsAct2);
                    }

                    //8845
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed)
                    {
                        List<TblAct> act2 = DetectActsOfPrs_946(context, prsAct2);

                        SetPrsOfActs_1724(context, act2, prsAct1);

                        DeletePrs_1726(context, prsAct2);
                    }
                }

                    //8836
                else if (prsAct2.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed && prsAct2 != prsAct1)
                {
                    //8842
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.Raw)
                    {
                        List<TblAct> act1 = DetectActsOfPrs_946(context, prsAct1);

                        SetPrsOfActs_1724(context, act1, prsAct2);

                        DeletePrs_1726(context, prsAct1);

                        prsAct2.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
                    }

                    //8839
                    else if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                    {
                        prsAct1.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;

                        prsAct2.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
                    }

                    //8837
                    else if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed)
                    {
                        prsAct2.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
                    }
                }

                    //8832
                else if (prsAct2.FldSttPrs == (int)Enum.SttPrs.ConsolidatedNotEndorsed && prsAct2 != prsAct1)
                {
                    //8835
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.Raw)
                    {
                        List<TblAct> act1 = DetectActsOfPrs_946(context, prsAct1);

                        SetPrsOfActs_1724(context, act1, prsAct2);

                        DeletePrs_1726(context, prsAct1);
                    }

                    //8833  
                    if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                    {
                        prsAct1.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
                    }
                }
            }

            //22018
            else
            {
                //22020
                if (prsAct1.FldSttPrs == (int)Enum.SttPrs.ConsolidatedEndorsed)
                {
                    prsAct1.FldSttPrs = (int)Enum.SttPrs.ConsolidatedNotEndorsed;
                }
            }

            //    //21738
            //else
            //{
            //    //21740
            //    if (prsAct1.FldSttPrs == (int)SttPrs.ConsolidatedEndorsed)
            //    {
            //        prsAct1.FldSttPrs = (int)SttPrs.ConsolidatedNotEndorsed;
            //    }
            //}

            DeleteAllPrsWotAct_19381(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void DeleteAllPrsWotAct_19381(BPMNDBEntities context)
        {
            //List<TblPr> prs = context.TblPrs.Where(m => m.TblActPrs.Count == 0).ToList();

            //foreach (TblPr item in prs)
            //{
            //    context.DeleteObject(item);
            //}

            //context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static TblPr AddPrsRaw_1721(BPMNDBEntities context)
        {
            TblPr prs = new TblPr() { FldNamPrs = "فرآیند خام جدید", FldSttPrs = (int)SttPrs.Raw };

            context.TblPrs.AddObject(prs);

            return prs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtRst"></param>
        /// <param name="typEvtSrt"></param>
        /// <returns></returns>
        public static TblEvtSrt AddEvtSrtTwinToEvtRst_6756(TblEvtRst evtRst, EvtSrtType typEvtSrt)
        {
            TblEvtSrt evtSrt = new TblEvtSrt() { FldGrpEvt = 1, FldSttAct = 1, FldTypEvtSrt = (int)typEvtSrt };

            TblEvt_GainAwrNew tbl = new TblEvt_GainAwrNew();

            tbl.TblEvtSrt = evtSrt;

            evtRst.TblEvt_GainAwrNew.Add(tbl);

            return evtSrt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <param name="evtRst"></param>
        public static TblEvtRst AddEvtRstToAct_458(BPMNDBEntities context, TblAct act, EvtRstType typEvtRst)
        {
            //459
            if (typEvtRst == EvtRstType.anyCdnEvtRst)
            {
                List<TblEvtRst> evtRst1 = DetectEvtRstOfActWthSpcTyp_454(context, act, EvtRstType.anyCdnEvtRst);

                //463
                if (evtRst1.Count == 0)
                {
                    TblEvtRst evtRst = new TblEvtRst() { FldTypEvtRst = (int)typEvtRst };

                    act.TblEvtRsts.Add(evtRst);

                    return evtRst;
                }
                //14958
                else
                {
                    return evtRst1[0];
                }
            }

                //466
            else if (typEvtRst == EvtRstType.cancelEvtRst)
            {
                List<TblEvtRst> evtRst2 = DetectEvtRstOfActWthSpcTyp_454(context, act, EvtRstType.cancelEvtRst);

                //470
                if (evtRst2.Count == 0)
                {
                    TblEvtRst evtRst = new TblEvtRst() { FldTypEvtRst = (int)typEvtRst };

                    act.TblEvtRsts.Add(evtRst);

                    return evtRst;
                }

                    //14961
                else
                {
                    return evtRst2[0];
                }
            }

            else if (typEvtRst != EvtRstType.getNewAwr && typEvtRst != EvtRstType.getNewAwrAftrAct)
            {
                TblEvtRst evtRst = new TblEvtRst() { FldTypEvtRst = (int)typEvtRst };

                act.TblEvtRsts.Add(evtRst);

                return evtRst;
            }

            else if (typEvtRst == EvtRstType.getNewAwr || typEvtRst == EvtRstType.getNewAwrAftrAct)
            {
                TblEvtRst evtRst = new TblEvtRst() { FldTypEvtRst = (int)typEvtRst };

                act.TblEvtRsts.Add(evtRst);

                AddEvtSrtTwinToEvtRst_6756(evtRst, (EvtSrtType)evtRst.FldTypEvtRst);

                return evtRst;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static TblPr DetectPrsOfEvtRst_2329(BPMNDBEntities context, TblEvtRst evtRst)
        {
            TblAct act = evtRst.TblAct;

            return DetectPrsOfAct_1839(context, act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtRstBnry"></param>
        /// <returns></returns>
        public static TypShp DetectTypShpEqualToEvtRstBoundary_3114(BPMNDBEntities context, TblEvtRst evtRstBnry)
        {
            switch ((EvtRstType)evtRstBnry.FldTypEvtRst)
            {
                //3124
                case EvtRstType.errAccurEvtRst:

                    return TypShp.E22;

                //3122
                case EvtRstType.cancelEvtRst:

                    return TypShp.E24;

                //3117
                case EvtRstType.spcCdnEvtRstInnTim:

                    if (evtRstBnry.FldStopAct == true)
                    {
                        return TypShp.E11;
                    }

                    else
                    {
                        return TypShp.E12;
                    }

                //3129
                case EvtRstType.getNewAwr:

                    List<TblWayAwr_News> wayAwrEvtSrtTwinFomTypNews = DetectWayAwrOfEvtSrtOfTypNews_485(context, evtRstBnry.TblEvt_GainAwrNew.First().TblEvtSrt);

                    //3136
                    if (wayAwrEvtSrtTwinFomTypNews.Count == 0)
                    {
                        if (evtRstBnry.FldStopAct == true)
                        {
                            return TypShp.E17;
                        }

                        else
                        {
                            return TypShp.E18;
                        }
                    }

                        //3160
                    else
                    {
                        if (evtRstBnry.FldStopAct == true)
                        {
                            return TypShp.E30;
                        }

                        else
                        {
                            return TypShp.E31;
                        }
                    }

                default:

                    return TypShp.E22;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrtBnry"></param>
        /// <returns></returns>
        public static TypShp DetectTypShpEqualToEvtSrtBoundary_2332(BPMNDBEntities context, TblEvtSrt evtSrtBnry)
        {
            //2333
            if (evtSrtBnry.FldTypEvtSrt == (int)EvtSrtType.errAccurEvtSrt)
            {
                return TypShp.E22;
            }

                //2336
            else if (evtSrtBnry.FldTypEvtSrt == (int)EvtSrtType.cancelEvtSrt)
            {
                return TypShp.E24;
            }

                //2335
            else if (evtSrtBnry.FldTypEvtSrt == (int)EvtSrtType.spcCdnEvtSrt)
            {
                TblEvtRst evtRstBnry = DetectEvtRstEqualToEvtSrt_527(context, evtSrtBnry);

                //2339
                if (evtRstBnry.FldStopAct == true)
                {
                    return TypShp.E11;
                }

                    //2341
                else
                {
                    return TypShp.E12;
                }
            }

            return TypShp.E22;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ary"></param>
        /// <returns></returns>
        public static List<object> DetectCptWthMaxRepeatInArray_21973(BPMNDBEntities context, List<object> ary)
        {
            List<object> lst = ary.Distinct().ToList();

            List<Tuple<long, object>> lst1 = new List<Tuple<long, object>>();

            long l = 0;

            foreach (var item in lst)
            {
                long l1 = ary.LongCount(m => m == item);

                lst1.Add(new Tuple<long, object>(l1, item));

                if (l1 > l)
                {
                    l = l1;
                }
            }

            List<object> lst2 = new List<object>();

            foreach (var item in lst1)
            {
                if (item.Item1 == l && !lst2.Contains(item.Item2))
                {
                    lst2.Add(item.Item2);
                }
            }

            return lst2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="evtSrtDepd"></param>
        /// <param name="prs"></param>
        /// <param name="shpAct"></param>
        /// <returns></returns>
        //public static NodBase DisplayEvtSrtDepdInDgm_1450(BPMNDBEntities context, Diagram dgm, TblEvtSrt evtSrtDepd, TblPr prs, NodBase shpAct)
        //{
        //    NodBase shpAgntEvtSrtDepd = new NodBase();

        //    TblAct actPvs = DetectActSrcOfEvtSrt_586(context, evtSrtDepd);

        //    TblPr prsActPvs = DetectPrsOfAct_1839(context, actPvs);

        //    //2114
        //    if (prsActPvs == prs || prsActPvs == null)
        //    {

        //        TblEvtRst evtRstEqvl = DetectEvtRstEqualToEvtSrt_527(context, evtSrtDepd);

        //        shpAgntEvtSrtDepd = DisEvtRstInDgm_19231(context, dgm, evtRstEqvl, actPvs);

        //        List<EntityObject> wayAwrTypRecvInt = DetectWayAwrsOfWvtSrt_2904(context, evtSrtDepd, TypWayAwr.Obj);

        //        //2910
        //        if (wayAwrTypRecvInt.Count > 0)
        //        {
        //            //12915
        //            foreach (EntityObject wayAwrTypRecvInt_i in wayAwrTypRecvInt)
        //            {
        //                EntityObject objRst = DetectObjRstOfWayAwr_12949(context, wayAwrTypRecvInt_i);

        //                DisplayIntInDgm_3601(context, dgm, (TblObj)objRst, actPvs.Shp.hdr, actPvs.Shp, shpAct);
        //            }
        //        }
        //    }

        //        //2116
        //    else
        //    {
        //        NodBase shpCallAct = (NodBase)DetectShpEqualToEty_2326(context, dgm, prsActPvs);

        //        //2130
        //        if (shpCallAct == null)
        //        {
        //            shpCallAct = DisplayShpInColInDgm_2318(context, dgm, TypShp.CA1, prsActPvs.TblNod, prsActPvs);
        //        }

        //        shpAgntEvtSrtDepd = shpCallAct;

        //        List<EntityObject> wayAwrTypRecvInt = DetectWayAwrsOfWvtSrt_2904(context, evtSrtDepd, TypWayAwr.Obj);

        //        //2935
        //        if (wayAwrTypRecvInt.Count > 0)
        //        {
        //            //13946
        //            foreach (EntityObject wayAwrTypRecvInt_i in wayAwrTypRecvInt)
        //            {
        //                EntityObject objRst = DetectObjRstOfWayAwr_12949(context, wayAwrTypRecvInt_i);

        //                NodBase shpObj = DisShpInnColInnDgm_1325(context, dgm, shpCallAct.hdr, TypShp.DO1, objRst);

        //                DisConDconTwoShps_2919(dgm, shpCallAct, shpObj, false);

        //                DisConDconTwoShps_2919(dgm, shpObj, shpAct, true);
        //            }
        //        }
        //    }

        //    return shpAgntEvtSrtDepd;
        //}


        public static INode DisplayEvtSrtDepdInDgm_1450(BPMNDBEntities context, GraphControl grph, INode dgm, TblEvtSrt evtSrtDepd, TblPr prs, INode shpAct)
        {
            INode shpAgntEvtSrtDepd = null;

            TblAct actPvs = DetectActSrcOfEvtSrt_586(context, evtSrtDepd);

            TblPr prsActPvs = DetectPrsOfAct_1839(context, actPvs);

            //2114
            if (prsActPvs == prs || prsActPvs == null)
            {

                TblEvtRst evtRstEqvl = DetectEvtRstEqualToEvtSrt_527(context, evtSrtDepd);

                //22137
                if (evtRstEqvl.ShpAgntEvtRst == null)
                {
                    shpAgntEvtSrtDepd = DisEvtRstInDgm_19231(context, grph, dgm, evtRstEqvl, actPvs);

                    //1424
                    evtRstEqvl.ShpAgntEvtRst = shpAgntEvtSrtDepd;

                    List<IWayAwr> wayAwrTypRecvInt = DetectWayAwrsOfWvtSrt_2904(context, evtSrtDepd, TypWayAwr.Obj);
                    //2910
                    if (wayAwrTypRecvInt.Count > 0)
                    {
                        //12915
                        foreach (IWayAwr wayAwrTypRecvInt_i in wayAwrTypRecvInt)
                        {
                            IObjRst objRst = DetectObjRstOfWayAwr_12949(context, wayAwrTypRecvInt_i);

                            DisplayIntInDgm_3601(context, grph, dgm, (TblObj)objRst, actPvs.Shp, shpAct);
                        }
                    }
                }
                else
                {
                    //22138
                    shpAgntEvtSrtDepd = evtRstEqvl.ShpAgntEvtRst;
                }
            }

            //2116
            else
            {
                INode shpCallAct = (INode)DetectShpEqualToEty_2326(context, dgm, prsActPvs);

                //2130
                if (shpCallAct == null)
                {
                    shpCallAct = DisplayShpInColInDgm_2318(context, grph, dgm, TypShp.CA1, prsActPvs.TblNod, prsActPvs);
                }

                shpAgntEvtSrtDepd = shpCallAct;

                List<IWayAwr> wayAwrTypRecvInt = DetectWayAwrsOfWvtSrt_2904(context, evtSrtDepd, TypWayAwr.Obj);

                //2935
                if (wayAwrTypRecvInt.Count > 0)
                {
                    //13946
                    foreach (IWayAwr wayAwrTypRecvInt_i in wayAwrTypRecvInt)
                    {
                        IObjRst objRst = DetectObjRstOfWayAwr_12949(context, wayAwrTypRecvInt_i);

                        INode shpObj = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpCallAct), TypShp.DO1, objRst as EntityObject);

                        DisConDconTwoShps_2919(grph, dgm, shpCallAct, shpObj, false);

                        DisConDconTwoShps_2919(grph, dgm, shpObj, shpAct, true);
                    }
                }
            }

            return shpAgntEvtSrtDepd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wayAwr"></param>
        /// <returns></returns>
        public static IObjRst DetectObjRstOfWayAwr_12949(BPMNDBEntities context, IWayAwr wayAwr)
        {
            IWayIfrm wayIfrm = DetectWayIfrmOfWayAwr_12950(wayAwr);

            return DetectObjRstWayIfrm_721(context, wayIfrm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wayAwr"></param>
        /// <returns></returns>
        public static IWayIfrm DetectWayIfrmOfWayAwr_12950(IWayAwr wayAwr)
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evtSrt"></param>
        /// <returns></returns>
        public static TblAct DetectActSrcOfEvtSrt_586(BPMNDBEntities context, TblEvtSrt evtSrt)
        {
            List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt);

            return DetectActSrcOfWayAwr_715(context, wayAwr[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="evtRst"></param>
        /// <param name="shpEvtRst"></param>
        //public static void DisNewUnusedEvtRstInDgm_19327_OLD(BPMNDBEntities context, Diagram dgm, TblEvtRst evtRst, ShpBase shpEvtRst)
        //{
        //    List<TblNew> news = DetectNewsOfEvtRst_573(context, evtRst);

        //    //19328
        //    foreach (TblNew news_i in news)
        //    {
        //        List<TblWayIfrm_News> wayIfrm = DetectWayIfrmOfNews_587(context, news_i);

        //        //19329
        //        if (wayIfrm.Count == 0)
        //        {
        //            NodBase shpNews = DisShpInnColInnDgm_1325(context, dgm, shpEvtRst.hdr, TypShp.E33, news_i);

        //            DisplayConCnulTwoShp_1253(dgm, shpEvtRst, shpNews);
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="evtRst"></param>
        /// <param name="shpEvtRst"></param>
        public static void DisNewUnusedEvtRstInDgm_19327(BPMNDBEntities context, GraphControl grph, INode dgm, TblEvtRst evtRst, INode shpEvtRst)
        {
            List<TblNew> news = DetectNewsOfEvtRst_573(context, evtRst);

            //19328
            foreach (TblNew news_i in news)
            {
                List<TblWayIfrm_News> wayIfrm = DetectWayIfrmOfNews_587(context, news_i);

                //19329
                if (wayIfrm.Count == 0)
                {
                    INode shpNews = DisShpInnColInnDgm_1325(context, grph, dgm, DetectColOfShp(shpEvtRst), TypShp.E33, news_i);

                    DisplayConCnulTwoShp_1253(grph, dgm, shpEvtRst, shpNews);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="act"></param>
        /// <param name="actDisEed"></param>
        //public static void DisActInDgm_3022_OLD(BPMNDBEntities context, Diagram dgm, TblAct act, List<TblAct> actDisEed)
        //{
        //    //3028
        //    if (!actDisEed.Contains(act))
        //    {
        //        DetectOrDisplayShpActInDgm_3007(context, dgm, act);

        //        List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act);

        //        //19303
        //        foreach (TblEvtRst evtRst_i in evtRst)
        //        {
        //            bool stsExsWayIfrmDepd = DetectStsExsWayIfrmDepdForEvtRst_19305(context, evtRst_i);

        //            //19310
        //            if (!stsExsWayIfrmDepd)
        //            {
        //                DisEvtRstInDgm_19231(context, dgm, evtRst_i, act);
        //            }
        //        }

        //        DisplaySrtOfActInDgm_1262(context, dgm, act);

        //        actDisEed.Add(act);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dgm"></param>
        /// <param name="act"></param>
        /// <param name="actDisEed"></param>
        public static void DisActInDgm_3022(BPMNDBEntities context, GraphControl grph, INode dgm, TblAct act, List<TblAct> actDisEed)
        {
            DetectOrDisplayShpActInDgm_3007(context, grph, dgm, act);

            List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act);

            //22029
            if (evtRst.Count == 0)
            {
                INode shpEvtRstUspf = DisShpInnColInnDgm_1325(context, grph, dgm, act.TblNod.Col, TypShp.E36);

                DisplayConCnulTwoShp_1253(grph, dgm, act.Shp, shpEvtRstUspf);
            }

            //19303
            foreach (TblEvtRst evtRst_i in evtRst)
            {
                bool stsExsWayIfrmDepd = DetectStsExsWayIfrmDepdForEvtRst_19305(context, evtRst_i);

                //19310
                if (!stsExsWayIfrmDepd)
                {
                    DisEvtRstInDgm_19231(context, grph, dgm, evtRst_i, act);
                }
            }

            DisplaySrtOfActInDgm_1262(context, grph, dgm, act);

            actDisEed.Add(act);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="evtRst"></param>
        /// <returns></returns>
        public static bool DetectStsExsWayIfrmDepdForEvtRst_19305(BPMNDBEntities context, TblEvtRst evtRst)
        {
            //-------------------------------/

            List<IWayIfrm> wayIfrmIdpd = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmDepd = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmWthDesUspf = new List<IWayIfrm>();
            List<IWayIfrm> wayIfrmWthDstPrsOth = new List<IWayIfrm>();

            //-------------------------------/

            List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst);

            DetectWayIfrmDepdAndIdpdOfWayIfrm_9876(context, wayIfrm, wayIfrmIdpd, wayIfrmDepd, wayIfrmWthDesUspf, wayIfrmWthDstPrsOth);

            //19306
            if (wayIfrmDepd.Count == 0)
            {
                return false;
            }

            //19307
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="prs"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActUspfPrs_21839(BPMNDBEntities context, TblPr prs)
        {
            //-------------------------------------/

            List<TblAct> actUspf = new List<TblAct>();

            //-------------------------------------/

            List<TblAct> act = DetectActsOfPrs_946(context, prs);

            //21840
            foreach (TblAct act_i in act)
            {
                List<TblEvtSrt> evtSrt = DetectEvtSrtOfAct_452(context, act_i);

                //21841
                foreach (var evtSrt_i in evtSrt)
                {
                    List<IWayAwr> wayAwr = DetectWayAwrOfEvtSrt_610(context, evtSrt_i);

                    //21842
                    foreach (IWayAwr wayAwr_i in wayAwr)
                    {
                        TblAct actSrc = DetectActSrcOfWayAwr_715(context, wayAwr_i);

                        //21843
                        if (actSrc.FldActUspf)
                        {
                            actUspf.Add(actSrc);
                        }
                    }
                }

                List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act_i);

                //21844
                foreach (TblEvtRst evtRst_i in evtRst)
                {
                    List<IObjRst> objRst = DetectObjRstOfEvtRst_572(context, evtRst_i);

                    //21845
                    foreach (IObjRst objRst_i in objRst)
                    {
                        List<IWayIfrm> wayIfrm = DetectWayIfrmOfObjRst_578(context, objRst_i);

                        //21846
                        foreach (IWayIfrm wayIfrm_i in wayIfrm)
                        {
                            TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                            //21847
                            if (actDst.FldActUspf)
                            {
                                actUspf.Add(actDst);
                            }
                        }
                    }
                }
            }

            return actUspf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static List<TblAct> DetectActMereContEvtRstWthDstUspf_21728(BPMNDBEntities context, List<TblAct> act)
        {
            //--------------------------------------------/

            List<TblAct> actMereContEvtRstWthDstUspf = new List<TblAct>();

            //--------------------------------------------/


            //21729
            foreach (TblAct act_i in act)
            {
                actMereContEvtRstWthDstUspf.Add(act_i);

                List<TblEvtRst> evtRst = DetectEvtRstOfAct_453(context, act_i);

                //21730
                foreach (TblEvtRst evtRst_i in evtRst)
                {
                    List<IWayIfrm> wayIfrm = DetectWayIfrmOfEvtRst_1912(context, evtRst_i);

                    //21731
                    foreach (IWayIfrm wayIfrm_i in wayIfrm)
                    {
                        TblAct actDst = DetectActDstOfWayIfrm_720(context, wayIfrm_i);

                        TblNod nod = actDst.TblNod;

                        bool stsOsdOrgNod = IsNodOsdOrg_1085(context, nod, CurrentUser.TblOrg);

                        //21733
                        if (stsOsdOrgNod)
                        {
                            actMereContEvtRstWthDstUspf.Remove(act_i);

                            break;
                        }

                        //21735
                        if (!actDst.FldActUspf)
                        {
                            actMereContEvtRstWthDstUspf.Remove(act_i);
                        }
                    }
                }
            }

            return actMereContEvtRstWthDstUspf;
        }

        public static void DeleteNodFromGrph_21988(GraphControl grph, INode shp, EntityObject ety)
        {
            grph.Graph.Remove(shp);

            if (ety.GetType() == typeof(TblAct))
            {
                ((TblAct)ety).Shp = null;
            }

            else if (ety.GetType() == typeof(TblObj))
            {
                if (shp.Style.GetType() == typeof(EventNodeStyle))
                {
                    ((TblObj)ety).Shp = null;
                }

                else if (shp.Style.GetType() == typeof(ArtifactNodeStyle))
                {
                    ((TblObj)ety).Shp1 = null;
                }
            }

            else if (ety.GetType() == typeof(TblNew))
            {
                if (shp.Style.GetType() == typeof(EventNodeStyle))
                {
                    ((TblNew)ety).Shp = null;
                }

                else if (shp.Style.GetType() == typeof(ArtifactNodeStyle))
                {
                    ((TblNew)ety).Shp1 = null;
                }
            }

            else if (ety.GetType() == typeof(TblSbjOral))
            {
                if (shp.Style.GetType() == typeof(EventNodeStyle))
                {
                    ((TblSbjOral)ety).Shp = null;
                }

                else if (shp.Style.GetType() == typeof(ArtifactNodeStyle))
                {
                    ((TblSbjOral)ety).Shp1 = null;
                }
            }

            else if (ety.GetType() == typeof(TblEvtRst))
            {
                ((TblEvtRst)ety).Shp = null;
            }

            else if (ety.GetType() == typeof(TblEvtSrt))
            {
                ((TblEvtSrt)ety).Shp = null;
            }
        }

        //------layout process diagram ------/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        //public static void LayoutDgmOfPrs_19260(Diagram dgm)
        //{
        //    ChangeLaneWidth_19242(dgm);

        //    SwimlaneLayout layout = LayoutAllNodesAndLinks_19246(dgm);

        //    List<DiagramLink> lnk = RemoveBetweenLaneLinks_19247(dgm);

        //    layoutAsTree_19249(dgm);

        //    ReorderBoundaryEventsAtchEedToActs_19251(dgm);

        //    AddLnkToDgm_19261(dgm, lnk);

        //    LayoutLinks_19252(dgm, layout);

        //    dgm.LaneGrid.ColumnHeaders[0].Width = 10000;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        //public static void ChangeLaneWidth_19242(Diagram dgm)
        //{
        //    List<NodBase> shpPrn = DetectParentsOfTrees_11899(dgm);

        //    //19243
        //    foreach (NodBase shpPrn_i in shpPrn)
        //    {
        //        int tnoOutgoingNodes = 0;
        //        DetectTnoOutgoingNodes_19225(shpPrn_i, ref tnoOutgoingNodes);

        //        //19244
        //        if (shpPrn_i.hdr != null)
        //            shpPrn_i.hdr.Height = shpPrn_i.hdr.Height < (tnoOutgoingNodes * 50) + ((tnoOutgoingNodes - 1) * 40) + 10 ? (tnoOutgoingNodes * 50) + ((tnoOutgoingNodes - 1) * 40) + 10 : shpPrn_i.hdr.Height;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <returns></returns>
        //public static List<DiagramLink> RemoveBetweenLaneLinks_19247(Diagram dgm)
        //{
        //    List<DiagramLink> lnk = dgm.Links.Where(m => (((NodBase)m.Origin).hdr != ((NodBase)m.Destination).hdr) || (NodBase)m.Origin == (NodBase)m.Destination).ToList();

        //    foreach (DiagramLink item in lnk)
        //    {
        //        dgm.Links.Remove(item);
        //    }

        //    return lnk;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static SwimlaneLayout LayoutAllNodesAndLinks_19246(Diagram dgm)
        //{
        //    SwimlaneLayout layout = new SwimlaneLayout();
        //    layout.NodeDistance = 80;
        //    //layout.KeepGroupLayout = true;            
        //    layout.LaneDistance = 500;
        //    layout.Direction = Direction.Straight;
        //    layout.EnableParallelism = true;
        //    layout.KeepLaneSizes = true;
        //    layout.MaxDegreeOfParallelism = 10;
        //    layout.CustomSortOrder = true;
        //    layout.MultipleGraphsPlacement = MultipleGraphsPlacement.HorizontalSortDescending;
        //    try
        //    {
        //        layout.Arrange(dgm);
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return layout;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        //public static void layoutAsTree_19249(Diagram dgm)
        //{
        //    List<NodBase> lst = new List<NodBase>();

        //    foreach (NodBase item in dgm.Nodes)
        //    {
        //        lst.Add(item);
        //    }

        //    lst = lst.Where(m => m.IncomingLinks.Count == 0).OrderBy(m => m.Bounds.X).OrderBy(m => m.LayoutTraits[SwimlaneLayoutTraits.Lane]).ToList();

        //    double i1 = 0;
        //    double i2 = 0;
        //    Header hdr = new Header();

        //    foreach (NodBase shp in lst)
        //    {
        //        if (hdr == shp.hdr)
        //        {
        //            Point p = new Point() { X = shp.Bounds.X - (i1 - i2) + 300, Y = shp.Bounds.Y };
        //            Size s = new Size() { Width = shp.Bounds.Width, Height = shp.Bounds.Height };

        //            //if (shp.MasterGroup != null)
        //            //{

        //            //}

        //            //else
        //            //{
        //            //    shp.Bounds = new Rect(p, s);
        //            //}
        //        }

        //        else
        //        {
        //            hdr = shp.hdr;
        //        }

        //        List<DiagramNode> nodOfTree = new List<DiagramNode>();
        //        DetectNodesOfTree(dgm, shp, nodOfTree);
        //        i1 = nodOfTree.OrderBy(m => m.Bounds.X).Last().Bounds.X;

        //        TreeLayout t = new TreeLayout(shp, TreeLayoutType.Centered, false, TreeLayoutLinkType.Cascading3, TreeLayoutDirections.LeftToRight, 60, 40, true, new Size(5, 5));
        //        t.LinkStyle = TreeLayoutLinkType.Cascading3;
        //        t.Arrange(dgm);

        //        i2 = nodOfTree.OrderBy(m => m.Bounds.X).Last().Bounds.X;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <param name="lst"></param>
        private static void DetectNodesOfTree(Diagram dgm, DiagramNode nod, List<DiagramNode> lst)
        {
            lst.Add(nod);

            foreach (DiagramLink lnk in nod.OutgoingLinks)
            {
                if (!lst.Contains(lnk.Destination))
                {
                    DetectNodesOfTree(dgm, lnk.Destination, lst);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="nod"></param>
        /// <param name="lst"></param>
        private static void DetectNodesOfReverseTree(Diagram dgm, DiagramNode nod, List<DiagramNode> lst)
        {
            lst.Add(nod);

            foreach (DiagramLink lnk in nod.IncomingLinks)
            {
                if (!lst.Contains(lnk.Destination))
                {
                    DetectNodesOfTree(dgm, lnk.Destination, lst);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        //public static void ReorderBoundaryEventsAtchEedToActs_19251(Diagram dgm)
        //{
        //    foreach (Group grp in dgm.Groups)
        //    {
        //        double i = 0;

        //        foreach (NodBase nod in grp.AttachedNodes)
        //        {
        //            if (nod != grp.MainItem)
        //            {
        //                Point p = new Point() { X = ((NodBase)grp.MainItem).Bounds.X + i, Y = ((NodBase)grp.MainItem).Bounds.Y + ((NodBase)grp.MainItem).Bounds.Height - nod.Bounds.Height / 2 };

        //                Size s = new Size() { Width = nod.Bounds.Width, Height = nod.Bounds.Height };

        //                nod.Bounds = new Rect(p, s);

        //                i += nod.Bounds.Width;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="lnk"></param>
        public static void AddLnkToDgm_19261(Diagram dgm, List<DiagramLink> lnk)
        {
            foreach (DiagramLink item in lnk)
            {
                dgm.Links.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <param name="layout"></param>
        //public static void LayoutLinks_19252(Diagram dgm, SwimlaneLayout layout)
        //{
        //    DiagramItemCollection dic = new DiagramItemCollection();

        //    foreach (DiagramLink item in dgm.Links)
        //    {
        //        if (!dic.Contains(item))
        //        {
        //            dic.Add(item);
        //        }
        //    }

        //    List<DiagramNode> nd = new List<DiagramNode>();

        //    foreach (Header item in dgm.LaneGrid.RowHeaders)
        //    {
        //        NodBase shp = new NodBase();
        //        nd.Add(shp);
        //        shp.LayoutTraits[SwimlaneLayoutTraits.Lane] = dgm.LaneGrid.RowHeaders.IndexOf(item);
        //        dgm.Nodes.Add(shp);
        //        dic.Add(shp);
        //    }

        //    layout.Arrange(dgm, dic);


        //    foreach (NodBase item in nd)
        //    {
        //        dgm.Nodes.Remove(item);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shp"></param>
        /// <param name="tnoNod"></param>
        //public static void DetectTnoOutgoingNodes_19225(INode shp, ref int tnoNod)
        //{
        //    //19228
        //    tnoNod += shp.OutgoingLinks.Count > 1 ? shp.OutgoingLinks.Count : 0;

        //    //19236
        //    foreach (DiagramLink lnk in shp.OutgoingLinks)
        //    {
        //        //19237
        //        if (((NodBase)lnk.Origin).hdr == ((NodBase)lnk.Destination).hdr && (NodBase)lnk.Origin != (NodBase)lnk.Destination)
        //        {
        //            DetectTnoOutgoingNodes_19225((NodBase)lnk.Destination, ref tnoNod);
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgm"></param>
        /// <returns></returns>
        //public static List<NodBase> DetectParentsOfTrees_11899(Diagram dgm)
        //{
        //    List<NodBase> lst = new List<NodBase>();

        //    foreach (NodBase shp in dgm.Nodes)
        //    {
        //        bool b = true;

        //        foreach (DiagramLink lnk in shp.IncomingLinks)
        //        {
        //            if (((NodBase)lnk.Origin).hdr == ((NodBase)lnk.Destination).hdr && (NodBase)lnk.Origin != (NodBase)lnk.Destination)
        //            {
        //                b = false;

        //                break;
        //            }
        //        }

        //        if (!lst.Contains(shp) && b)
        //        {
        //            lst.Add(shp);
        //        }
        //    }

        //    return lst;
        //}

        /// <summary>
        /// شناسایی نحوه های آگاهی دارای ناهمسانی از یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<IWayAwr> DetectWayAwrsWithDson_19039(BPMNDBEntities context, TblNod nod)
        {
            List<IWayAwr> wayAwrsRes = new List<IWayAwr>();

            List<TblAct> acts = DetectActsOfNod_754(context, nod);

            //19051
            foreach (var act_i in acts)
            {
                List<TblEvtSrt> evtSrts = DetectEvtSrtOfAct_452(context, act_i);

                //19052
                foreach (var evtSrt_i in evtSrts)
                {
                    List<IWayAwr> wayAwrs = DetectWayAwrOfEvtSrtWithWayIfrm_503(context, evtSrt_i);

                    //19055
                    foreach (var wayAwr_i in wayAwrs)
                    {
                        dynamic d = wayAwr_i;
                        if (d.FldTypDson != null)
                        {
                            wayAwrsRes.Add(wayAwr_i);
                        }
                    }
                }
            }

            return wayAwrsRes;
        }

        /// <summary>
        /// شناسایی نحوه های آگاه سازی دارای ناهمسانی از یک گره
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<IWayIfrm> DetectWayIfrmsWithDson_19040(BPMNDBEntities context, TblNod nod)
        {
            List<IWayIfrm> wayIfrms = new List<IWayIfrm>();

            List<TblAct> acts = DetectActsOfNod_754(context, nod);

            //19041
            foreach (var act_i in acts)
            {
                List<TblEvtRst> evtRsts = DetectEvtRstOfAct_453(context, act_i);

                //19042
                foreach (var evtRst_i in evtRsts)
                {
                    List<IObjRst> objRsts = DetectObjRstOfEvtRst_572(context, evtRst_i);

                    //19043
                    foreach (var objRst_i in objRsts)
                    {
                        List<IWayIfrm> wayIfrm1 = DetectWayIfrmOfObjRst_578(context, objRst_i);

                        //19044
                        foreach (var wayIfrm1_i in wayIfrm1)
                        {
                            dynamic d = wayIfrm1_i;
                            if (d.FldTypDson != null)
                            {
                                wayIfrms.Add(wayIfrm1_i);
                            }
                        }
                    }
                }
            }

            return wayIfrms;
        }

        /// <summary>
        /// شناسایی ادعاهای ناهمسان در مورد یک گره در ارتباط با گره های دیگر از طریق نحوه های آگاهی و آگاه سازی
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<Tuple<IWayAwrIfrm, TblNod>> DetectDsonsClaimedByNod_19020(BPMNDBEntities context, TblNod nod)
        {
            List<Tuple<IWayAwrIfrm, TblNod>> res = new List<Tuple<IWayAwrIfrm, TblNod>>();

            List<IWayAwr> wayAwrs = DetectWayAwrsWithDson_19039(context, nod);

            //19056
            foreach (var wayAwrs_i in wayAwrs)
            {
                TblEvtSrt evtSrt = DetectEvtSrtOfWayAwr_674(wayAwrs_i);
                TblNod nodSrc = DetectNodSrcEvtSrc_2891(context, evtSrt);
                res.Add(new Tuple<IWayAwrIfrm, TblNod>(wayAwrs_i, nodSrc));
            }

            List<IWayIfrm> wayIfrms = DetectWayIfrmsWithDson_19040(context, nod);

            //19057
            foreach (var wayIfrm_i in wayIfrms)
            {
                TblNod nodDst = DetectNodDstOfWayIfrm_9866(context, wayIfrm_i);
                res.Add(new Tuple<IWayAwrIfrm, TblNod>(wayIfrm_i, nodDst));
            }

            return res;
        }

        /// <summary>
        /// در این تابع فعالیتی یافت می شود که رخداد شی نتیجه و رخداد نتیجه مربوط به نحوه آگاه سازی به آن متصل است.
        /// </summary>
        /// <param name="wayIfrm"></param>
        /// <returns></returns>
        public static TblAct DetectSourceActOfWayIfrm_19105(IWayIfrm wayIfrm)
        {
            if (wayIfrm is TblWayIfrm_News)
            {
                return (wayIfrm as TblWayIfrm_News).TblNew.TblEvtRst.TblAct;
            }
            if (wayIfrm is TblWayIfrm_Oral)
            {
                return (wayIfrm as TblWayIfrm_Oral).TblSbjOral.TblEvtRst.TblAct;
            }
            if (wayIfrm is TblWayIfrm_SndOut)
            {
                return (wayIfrm as TblWayIfrm_SndOut).TblObj.TblEvtRst.TblAct;
            }

            throw new InvalidOperationException("parameter is not a wayIfrm");
        }

        public static void DetermineDsonSttForWayIfrm_19144(IWayIfrm wayIfrm, IObjRst objRst)
        {
            dynamic wayIfrm_d = wayIfrm;
            TblAct act = DetectActSrcOfObjRst_551(objRst);

            if (wayIfrm_d.FldTypDson != null)
            {
                switch ((int)wayIfrm_d.FldTypDson)
                {
                    case 1:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 1;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 2;
                        }
                        break;
                    case 2:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 3;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 4;
                        }

                        break;
                    case 3:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 5;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 6;
                        }

                        break;
                    case 4:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 7;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 8;
                        }


                        break;
                    case 5:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 9;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 10;
                        }

                        break;
                    case 6:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 11;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 12;
                        }

                        break;
                    case 7:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 13;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 14;
                        }

                        break;
                    case 8:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 15;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 16;
                        }

                        break;
                    case 9:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 17;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 18;
                        }

                        break;
                    case 10:
                        if (!act.FldActUspf)
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 19;
                        }
                        else
                        {
                            wayIfrm_d.FldStsDsonWayIfrm = 20;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static void DetermineDsonSttForWayAwr_19082(IWayAwr wayAwr, IObjRst objRst)
        {
            dynamic wayAwr_d = wayAwr;
            TblAct act = DetectActDstOfWayAwr_19101(wayAwr);

            if (wayAwr_d.FldTypDson != null)
            {
                switch ((int)wayAwr_d.FldTypDson)
                {
                    //19107
                    case 1:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 1;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 2;
                        }
                        break;

                    case 2:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 3;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 4;
                        }

                        break;

                    case 3:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 5;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 6;
                        }

                        break;

                    case 4:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 7;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 8;
                        }

                        break;

                    case 5:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 9;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 10;
                        }

                        break;

                    case 6:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 11;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 12;
                        }

                        break;

                    case 7:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 13;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 14;
                        }

                        break;

                    case 8:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 15;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 16;
                        }

                        break;

                    case 9:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 17;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 18;
                        }

                        break;

                    case 10:
                        if (!act.FldActUspf)
                        {
                            wayAwr_d.FldStsDsonWayAwr = 19;
                        }
                        else
                        {
                            wayAwr_d.FldStsDsonWayAwr = 20;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public static void AddDsonByWayAwrInfrm_19072(BPMNDBEntities context, Enum.TypeStsDson dsnType, IWayAwrIfrm ety)
        {
            //19074
            if (ety is TblWayAwr_News || ety is TblWayAwr_Oral || ety is TblWayAwr_RecvInt)
            {
                dynamic wayIfrm = DetectWayIfrmOfWayAwr_12950(ety as IWayAwr);
                wayIfrm.FldTypDson = (int)dsnType;
                DetermineDsonSttForWayIfrm_19144(wayIfrm, DetectObjRstWayIfrm_721(context, wayIfrm));
                //DetermineDsonSttForWayAwr_19082(ety, DetectObjRstOfWayAwr_12949(context, ety));
            }
            //19073
            else if (ety is TblWayIfrm_News || ety is TblWayIfrm_Oral || ety is TblWayIfrm_SndOut)
            {
                dynamic wayAwr = DetectWayAwrOfWayIfrm_623(ety as IWayIfrm);
                wayAwr.FldTypDson = (int)dsnType;
                DetermineDsonSttForWayAwr_19082(wayAwr, DetectObjRstOfWayAwr_12949(context, wayAwr));
            }
        }

        public static IObjRst DetectObjResOfWayAwrIfrm_19077(IWayAwrIfrm wayAwrAndIfrm)
        {
            if (wayAwrAndIfrm is TblWayAwr_News)
            {
                return (wayAwrAndIfrm as TblWayAwr_News).TblWayIfrm_News.TblNew;
            }
            else if (wayAwrAndIfrm is TblWayAwr_Oral)
            {
                return (wayAwrAndIfrm as TblWayAwr_Oral).TblWayIfrm_Oral.TblSbjOral;
            }
            else if (wayAwrAndIfrm is TblWayAwr_RecvInt)
            {
                return (wayAwrAndIfrm as TblWayAwr_RecvInt).TblWayIfrm_SndOut.TblObj;

            }

            else if (wayAwrAndIfrm is TblWayIfrm_News)
            {
                return (wayAwrAndIfrm as TblWayIfrm_News).TblNew;

            }
            else if (wayAwrAndIfrm is TblWayIfrm_Oral)
            {
                return (wayAwrAndIfrm as TblWayIfrm_Oral).TblSbjOral;

            }
            else if (wayAwrAndIfrm is TblWayIfrm_SndOut)
            {
                return (wayAwrAndIfrm as TblWayIfrm_SndOut).TblObj;
            }
            return null;
        }

        /// <summary>
        /// شناسایی نقش هایی که یک گره در آن بازی میکند 
        /// </summary>
        /// <param name="nod"></param>
        /// <returns></returns>
        public static List<TblRol> DetectRolPlayedInNod_23213(TblNod nod)
        {
            return nod.TblPlyrRols.Select(p => p.TblRol).ToList();
        }

        public static string GetDsonDesc(SSYM.OrgDsn.Model.Base.IWayAwrIfrm value)
        {
            dynamic d = value;

            int dsonType = 0;

            if (value is TblWayAwr_News || value is TblWayAwr_Oral || value is TblWayAwr_RecvInt)
            {
                dsonType = d.FldStsDsonWayAwr;
            }
            else
            {
                dsonType = d.FldStsDsonWayIfrm;
            }

            string dsonTitle = string.Empty;

            switch ((Model.Enum.TypDson)dsonType)
            {
                case SSYM.OrgDsn.Model.Enum.TypDson.NoDson:
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.OutUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcf:
                    dsonTitle = " {0} را از شما دریافت میکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToSpcf:
                    dsonTitle = " {0} را از شما دریافت نمیکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.InUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.InSpcf:
                    dsonTitle = " {0} را به شما ارسال میکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromSpcf:
                    dsonTitle = " {0} را به شما ارسال نمیکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInSpcf:
                    dsonTitle = "شما را به صورت شفاهی آگاه میکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromSpcf:
                    dsonTitle = "شما را به صورت شفاهی آگاه نمیکنم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcf:
                    dsonTitle = "به صورت شفاهی توسط شما آگاه می شوم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToUnspcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToSpcf:
                    dsonTitle = "به صورت شفاهی توسط شما آگاه نمی شوم";
                    break;
                case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromUnspcf:
                case Model.Enum.TypDson.SndNewsFromSpcf:
                    dsonTitle = " {0} را از شما دریافت می کنم";
                    break;

                case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromSpcf:
                case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromUnspcf:
                    dsonTitle = " {0} را ارسال نمی کنم";
                    break;
                default:
                    break;
            }

            if (value is TblWayAwr_RecvInt)
            {
                dsonTitle = string.Format(dsonTitle, (value as TblWayAwr_RecvInt).TblWayIfrm_SndOut.TblObj.FldNamObj);
            }
            else if (value is TblWayAwr_Oral)
            {

            }
            else if (value is TblWayAwr_News)
            {
                dsonTitle = string.Format(dsonTitle, (value as TblWayAwr_News).TblWayIfrm_News.TblNew.FldTtlNews);
            }
            else if (value is TblWayIfrm_SndOut)
            {
                dsonTitle = string.Format(dsonTitle, (value as TblWayIfrm_SndOut).TblObj.FldNamObj);
            }
            else if (value is TblWayIfrm_Oral)
            {

            }
            else if (value is TblWayIfrm_News)
            {
                dsonTitle = string.Format(dsonTitle, (value as TblWayIfrm_News).TblNew.FldTtlNews);
            }

            return dsonTitle;
        }

        //public static void RemoveDsonOfWayAwrIfrm_19202(EntityObject wayAwrAndIfrm)
        //{
        //    dynamic wayAwrAndIfrm_d = wayAwrAndIfrm;
        //    wayAwrAndIfrm_d.FldTypDson = null;

        //    if (wayAwrAndIfrm is TblWayAwr_News || wayAwrAndIfrm is TblWayAwr_Oral || wayAwrAndIfrm is TblWayAwr_RecvInt)
        //    {
        //        wayAwrAndIfrm_d.FldStsDsonWayAwr = null;
        //    }
        //    else if (wayAwrAndIfrm is TblWayIfrm_News || wayAwrAndIfrm is TblWayIfrm_Oral || wayAwrAndIfrm is TblWayIfrm_SndOut)
        //    {
        //        wayAwrAndIfrm_d.FldStsDsonWayIfrm = null;
        //    }
        //}
    }
}
