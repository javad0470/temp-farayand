using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Media;
using yWorks.yFiles.UI.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.Model
{
    public interface ITblAct
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNamAct { get; set; }
    }


    [MetadataType(typeof(ITblAct))]
    public partial class TblAct : ITblAct, IDataErrorInfo, INotifyDataErrorInfo, IAllEty
    {
        public TblAct()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }

        //public TblAct(bool validate)
        //{
        //    if (validate)
        //    {
        //        dataErrorInfoSupport = new DataErrorInfoSupport(this);
        //    }
        //}

        bool validateErrors;
        public bool ValidateErrors
        {
            get
            {
                return validateErrors;
            }
            set
            {
                if (value != validateErrors)
                {
                    validateErrors = value;
                    //if (validateErrors)
                    //{
                    //}
                    //else
                    //{
                    //    dataErrorInfoSupport = null;
                    //}
                }
            }
        }

        string fldNamNod;

        public string FldNamNod
        {
            get
            {
                if (fldNamNod == string.Empty || fldNamNod == null)
                {
                    fldNamNod = Model.PublicMethods.ActivityPerformerName_951(this.FldCodAct);
                    return fldNamNod;
                }
                return fldNamNod;
            }
        }

        //BPMNShapes.ShpBase shp;

        //public BPMNShapes.ShpBase Shp
        //{
        //    get { return shp; }
        //    set
        //    {
        //        shp = value;
        //        shp.Id = this.FldCodAct;
        //        shp.Text = this.FldNamAct;
        //    }
        //}

        INode shp;

        public INode Shp
        {
            get { return shp; }
            set
            {
                shp = value;
            }
        }

        /// <summary>
        /// لیست تمام ورودی های فرستاده شده به فعالیت را می دهد
        /// </summary>
        public List<Tuple<TblObj, TblEvtSrt>> AllObjSentToAct
        {
            get
            {
                return DetectAllObjSentToAct();
            }
        }

        /// <summary>
        /// شناسایی تمامی ورودی های فرستاده شده به فعالیت جاری
        /// </summary>
        /// <returns></returns>
        private List<Tuple<TblObj, TblEvtSrt>> DetectAllObjSentToAct()
        {
            List<Tuple<TblObj, TblEvtSrt>> lst = new List<Tuple<TblObj, TblEvtSrt>>();

            foreach (TblEvtSrt item in this.TblEvtSrts)
            {
                foreach (var item1 in item.TblWayAwr_RecvInt)
                {
                    lst.Add(new Tuple<TblObj, TblEvtSrt>(item1.TblWayIfrm_SndOut.TblObj, item));
                }
            }

            return lst.Distinct().ToList<Tuple<TblObj, TblEvtSrt>>();
        }

        /// <summary>
        /// لیست تمام اشیای نتیجه فرستاده شده برای فعالیت جاری
        /// </summary>
        public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentToAct
        {
            get
            {
                return DetectAllObjRstSentToAct();
            }
        }

        /// <summary>
        /// لیست تمام اشیای نتیجه فرستاده شده برای فعالیت جاری را بر می گرداند
        /// </summary>
        /// <returns></returns>
        private List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentToAct()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();


            foreach (TblEvtSrt item in this.TblEvtSrts)
            {
                PublicMethods.DeleteUnusableWayAwrOfEvtSrt(item);

                foreach (var item1 in item.TblWayAwr_RecvInt)
                {
                    lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1.TblWayIfrm_SndOut.TblObj, item));
                }

                foreach (var item1 in item.TblWayAwr_Oral)
                {
                    lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1.TblWayIfrm_Oral.TblSbjOral, item));
                }

                foreach (var item1 in item.TblWayAwr_News)
                {
                    lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1.TblWayIfrm_News.TblNew, item));
                }
            }

            return lst.Distinct().ToList<Tuple<IObjRst, TblEvtSrt>>();
        }

        /// <summary>
        /// لیست تمام اشیای نتیجه ارسال شده از فعالیت را می دهد
        /// </summary>
        public List<Tuple<IObjRst, TblEvtSrt>> AllObjRstSentFromAct
        {
            get
            {
                return DetectAllObjRstSentFromAct();
            }
        }

        private List<Tuple<IObjRst, TblEvtSrt>> DetectAllObjRstSentFromAct()
        {
            List<Tuple<IObjRst, TblEvtSrt>> lst = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (TblEvtRst item in this.TblEvtRsts)
            {
                foreach (var item1 in item.TblObjs)
                {
                    foreach (var item2 in item1.TblWayIfrm_SndOut)
                    {
                        lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1, item2.TblWayAwr_RecvInt.TblEvtSrt));
                    }
                }

                foreach (var item1 in item.TblSbjOrals)
                {
                    foreach (var item2 in item1.TblWayIfrm_Oral)
                    {
                        lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1, item2.TblWayAwr_Oral.TblEvtSrt));
                    }
                }

                foreach (var item1 in item.TblNews)
                {
                    if (item1.TblWayIfrm_News.Count > 0)
                    {
                        foreach (var item2 in item1.TblWayIfrm_News)
                        {
                            lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1, item2.TblWayAwr_News.TblEvtSrt));
                        }
                    }

                    else
                    {
                        lst.Add(new Tuple<IObjRst, TblEvtSrt>(item1, null));
                    }
                }
            }

            return lst.Distinct().ToList<Tuple<IObjRst, TblEvtSrt>>();
        }

        bool isSelected = false;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {

                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public ObservableCollection<TblObj> AllObjOfAct
        {
            get
            {
                List<TblObj> obj = new List<TblObj>();

                foreach (TblEvtRst item in this.TblEvtRsts)
                {
                    obj.AddRange(item.TblObjs);
                }

                return new ObservableCollection<TblObj>(obj);
            }
        }

        public string PstPos
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    TblPosPstOrg pstpos = context.TblPosPstOrgs.Single(m => m.FldCodPosPst == this.TblNod.FldCodEty);
                    if (pstpos.FldCodTyp == (int)Enum.PosPst.Pos)
                    {
                        return string.Format("جایگاه {0}", pstpos.FldNamPosPst);
                    }
                    else
                    {
                        return string.Format("سمت {0}", pstpos.FldNamPosPst);
                    }
                }
            }
        }

        public TblPosPstOrg PstPosObj
        {
            get
            {
                using (BPMNDBEntities context = new BPMNDBEntities())
                {
                    TblPosPstOrg pstpos = context.TblPosPstOrgs.Single(m => m.FldCodPosPst == this.TblNod.FldCodEty);
                    return pstpos;
                }
            }
        }

        SolidColorBrush actColor;

        public SolidColorBrush ActColor
        {
            get { return actColor; }
            set
            {
                actColor = value;
                OnPropertyChanged("ActColor");
            }
        }

        bool isSelectedAsDstForObjRst;

        /// <summary>
        /// آیا این فعالیت به عنوان مقصد برای شی نتیجه انتخاب شده است یا نه
        /// </summary>
        public bool IsSelectedAsDstForObjRst
        {
            get { return isSelectedAsDstForObjRst; }
            set { isSelectedAsDstForObjRst = value; }
        }

        /// <summary>
        /// فرایند مربوط به این فعالیت
        /// </summary>
        public TblPr Prs
        {
            get
            {
                return DetectPrsOfAct_1839(this.GetContext<BPMNDBEntities>(), this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        private TblPr DetectPrsOfAct_1839(BPMNDBEntities context, TblAct act)
        {
            PublicMethods.ReloadEntity(context, act, act.TblActPrs, "TblActPrs");

            return act.TblActPrs.Count > 0 ? act.TblActPrs.First().TblPr : null;
        }

        /// <summary>
        /// فعالیت هایی که فعالیت جاری برای آن ها شی نتیجه ارسال نموده است
        /// </summary>
        public List<TblAct> ActDst
        {
            get
            {
                List<TblEvtRst> evtRst = PublicMethods.DetectEvtRstOfAct_453(this.GetContext<BPMNDBEntities>(), this);

                List<TblAct> actDst = new List<TblAct>();

                foreach (TblEvtRst item in evtRst)
                {
                    foreach (IObjRst item1 in item.ObjRsts)
                    {
                        actDst.AddRange(item1.ActTarget);
                    }
                }

                return actDst.Distinct().ToList();
            }
        }

        /// <summary>
        /// فعالیت هایی که به فعالیت جاری شی نتیجه ارسال نموده اند
        /// </summary>
        public List<TblAct> ActSrc
        {
            get
            {
                List<TblEvtSrt> evtSrt = PublicMethods.DetectEvtSrtOfAct_452(this.GetContext<BPMNDBEntities>(), this);

                List<TblAct> actSrc = new List<TblAct>();

                foreach (TblEvtSrt item in evtSrt)
                {
                    List<IWayAwr> wayAwr = PublicMethods.DetectWayAwrOfEvtSrt(item);

                    foreach (IWayAwr item1 in wayAwr)
                    {
                        actSrc.Add(item1.ActSrc);
                    }
                }

                return actSrc.Distinct().ToList();
            }
        }

        /// <summary>
        /// لیست فعالیت های مقصد با در نظر گرفتن یکسان بودن یا نبودن فرآیند
        /// </summary>
        public List<TblAct> ActDstForIsleAct
        {
            get
            {
                List<TblEvtRst> evtRst = PublicMethods.DetectEvtRstOfAct_453(this.GetContext<BPMNDBEntities>(), this);

                List<TblAct> actDst = new List<TblAct>();

                foreach (TblEvtRst item in evtRst)
                {
                    foreach (IObjRst item1 in item.ObjRst)
                    {
                        foreach (IWayIfrm item2 in item1.WayIfrms)
                        {
                            if (item2.ActDst != null && item2.GetType() != typeof(TblWayIfrm_News) && item2.ActDst.Prs != null)
                            {
                                if (item2.ActDst.Prs.FldSttPrs == (int)Enum.SttPrs.Raw || this.Prs == item2.ActDst.Prs)
                                {
                                    actDst.Add(item2.ActDst);
                                }
                            }
                        }
                    }
                }

                return actDst.Distinct().ToList();
            }
        }

        /// <summary>
        /// لیست فعالیت های مقصد با در نظر گرفتن یکسان بودن یا نبودن فرآیند
        /// </summary>
        public List<TblAct> ActSrcForIsleAct
        {
            get
            {
                List<TblEvtSrt> evtSrt = PublicMethods.DetectEvtSrtOfAct_452(this.GetContext<BPMNDBEntities>(), this);

                List<TblAct> actSrc = new List<TblAct>();

                foreach (TblEvtSrt item in evtSrt)
                {
                    List<IWayAwr> wayAwr = PublicMethods.DetectWayAwrOfEvtSrt(item);

                    foreach (IWayAwr item1 in wayAwr)
                    {
                        if (item1.ActSrc != null && item1.GetType() != typeof(TblWayAwr_News) && item1.ActSrc.Prs != null)
                        {
                            if (item1.ActSrc.Prs.FldSttPrs == (int)Enum.SttPrs.Raw || this.Prs == item1.ActSrc.Prs)
                            {
                                actSrc.Add(item1.ActSrc);
                            }
                        }
                    }
                }

                return actSrc.Distinct().ToList();
            }
        }

        public List<string> EvtSrtNames
        {
            get
            {
                List<string> names = new List<string>();

                foreach (var item in this.TblEvtSrts)
                {
                    names.Add(PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == (int)TypItmFix.EvtSrt && m.FldCodItm == (int)item.FldTypEvtSrt).FldNamItm);
                }

                return names;
            }
        }

        public List<string> EvtRstNames
        {
            get
            {
                List<string> names = new List<string>();

                foreach (var item in this.TblEvtRsts)
                {
                    names.Add(PublicMethods.TblItmFixSfws.Single(m => m.FldCodSbj == (int)TypItmFix.EvtRst && m.FldCodItm == (int)item.FldTypEvtRst).FldNamItm);
                }

                return names;
            }
        }

        public List<string> InputNames
        {
            get
            {
                List<string> lst = new List<string>();

                try
                {
                    foreach (var srt in this.TblEvtSrts)
                    {
                        foreach (var input in srt.TblWayAwr_RecvInt)
                        {
                            lst.Add(input.TblWayIfrm_SndOut.TblObj.FldNamObj);
                        }
                    }
                }
                catch (Exception)
                {
                }

                return lst;
            }
        }

        public List<string> OutputNames
        {
            get
            {
                List<string> lst = new List<string>();

                foreach (var rst in this.TblEvtRsts)
                {
                    foreach (var obj in rst.TblObjs)
                    {
                        lst.Add(obj.FldNamObj);
                    }
                }

                return lst;
            }
        }

        public List<string> InputNews
        {
            get
            {
                List<string> lst = new List<string>();

                try
                {
                    foreach (var srt in this.TblEvtSrts)
                    {
                        foreach (var input in srt.TblWayAwr_News)
                        {
                            lst.Add(input.TblWayIfrm_News.TblNew.FldTtlNews);
                        }
                    }
                }
                catch (Exception)
                {
                }

                return lst;
            }
        }

        public List<string> OutputNews
        {
            get
            {
                List<string> lst = new List<string>();

                foreach (var rst in this.TblEvtRsts)
                {
                    foreach (var obj in rst.TblNews)
                    {
                        lst.Add(obj.FldTtlNews);
                    }
                }

                return lst;
            }
        }


        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamAct")
                {
                    return true;
                }
            }
            return false;
        }

        [NonSerialized]
        private DataErrorInfoSupport dataErrorInfoSupport;


        public string Error
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return null;
                }

                return dataErrorInfoSupport.Error;
            }
        }

        public string this[string memberName]
        {
            get
            {
                if (!shouldCheckErrors(memberName))
                {
                    return null;
                }

                return dataErrorInfoSupport[memberName];
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return;
            }

            dataErrorInfoSupport.RaiseErrorsChanged(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged += value;
                }
            }
            remove
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged -= value;
                }
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return null;
            }

            return dataErrorInfoSupport.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return false;
                }

                return dataErrorInfoSupport.HasErrors;
            }
        }

        #endregion

        public int CodEty
        {
            get
            {
                return this.FldCodAct;
            }
        }


        public string TypeAct
        {
            get
            {
                ActivityTypes type = (ActivityTypes)this.FldTypAct;
                switch (type)
                {
                    case ActivityTypes.Manual:
                        return "انسانی-دستی";
                        break;
                    case ActivityTypes.UserTask:
                        return "انسانی-نرم‏افزاری";
                        break;
                    case ActivityTypes.ServiceTaskHard:
                        return "سرویس خارج از سازمان سخت افزاری";
                        break;
                    case ActivityTypes.ServiceTaskSoft:
                        return "سرویس خارج از سازمان نرم افزاری";
                        break;
                    default:
                        break;
                }

                return null;
            }
        }

        public string HasSubAct
        {
            get
            {
                switch ((HasOrDoesntHave)FldActSubHav)
                {
                    case HasOrDoesntHave.Has:
                        return "دارد";
                    case HasOrDoesntHave.DoesntHave:
                        return "ندارد";
                }
                return null;
            }
        }


        public AllTypEty CodTypEty
        {
            get { return AllTypEty.Act; }
        }

        public string Name
        {
            get { return this.FldNamAct; }
        }

        public static int GetNewEvtSrtGroupID(TblAct act)
        {
            int newId = 1;
            foreach (TblEvtSrt item in act.TblEvtSrts)
            {
                newId = Math.Max(item.FldGrpEvt, newId);
            }

            return newId + 1;
        }

    }
}
