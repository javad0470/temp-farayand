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
    public class DtlSndNewsViewModel : UserControlViewModel
    {
        #region ' Fields '

        private bool isSelectNewsPopupOpen;
        private SlcNewsViewModel slcNews;
        private DefNewsViewModel defNews;
        private bool newsDoesntExist;

        #endregion

        #region ' Initialaizer '

        public DtlSndNewsViewModel(BPMNDBEntities context, EntityObject obj, int codSelectedNod)
            : base(context, obj)
        {

            SaveChangesCommand = new DelegateCommand(ExecuteSaveChangesCommand);
            SelectNewsPopupIsOpenCommand = new DelegateCommand(ExecuteSelectNewsPopupIsOpenCommand);
            SlcNews = new SlcNewsViewModel();
            DefNews = new DefNewsViewModel(codSelectedNod);
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
            //this.TblNews = bpmnEty.TblNews.SingleOrDefault(E => E.FldCodNews == 5);
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public TblNew TblNews
        {
            get
            {
                return Entity as TblNew;
            }
            set
            {
                Entity = value;
                RaisePropertyChanged("TblNews");
            }
        }


        /// <summary>
        /// opens popup for news selection
        /// </summary>
        public ICommand SelectNewsPopupIsOpenCommand { get; set; }

        /// <summary>
        /// SlcNewsViewModel
        /// </summary>
        public SlcNewsViewModel SlcNews
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

        /// <summary>
        /// save changes command
        /// </summary>
        public ICommand SaveChangesCommand { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.DefNews.Dispose();

            this.SlcNews.Dispose();
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
        /// execute select news popoup command
        /// </summary>
        private void ExecuteSelectNewsPopupIsOpenCommand()
        {
            //SlcNews.Parent = this;
            //this.SlcNews.DetectAllNews();

            this.SlcNews.ActSrc = this.TblNews.ActSrc;
            this.SlcNews.IsFomActCntSlcEed = true;
            this.SlcNews.IsFomMeeSlcEed = false;
            this.SlcNews.ObjCnt = this.TblNews;

            Util.ShowPopup(this.SlcNews);

            if (this.SlcNews.Result == PopupResult.OK)
            {
                TblNew objRst = new TblNew();

                TblNew objRst2 = this.TblNews;

                if (this.SlcNews.IsFomMeeSlcEed)
                {
                    objRst = this.bpmnEty.TblNews.SingleOrDefault(m => m.FldCodNews == this.SlcNews.FomMeeSlcEedItm.Item1.CodObj);
                }
                else
                {
                    objRst = this.bpmnEty.TblNews.SingleOrDefault(m => m.FldCodNews == this.SlcNews.FomActCntSlcEedItm.Item1.CodObj);
                }

                TblEvtRst evtRst = this.TblNews.TblEvtRst;

                //PublicMethods.DeleteWayIfrmOfObjRst_726(bpmnEty, objRst2);

                if (objRst.ActSrc.FldActUspf)
                {
                    List<Model.Base.IWayIfrm> lst = PublicMethods.DetectWayIfrmOfObjRst_578(bpmnEty, objRst2);

                    foreach (Model.Base.IWayIfrm item in lst)
                    {
                        PublicMethods.DeleteWayIfrmOfObjRstAndChgPrs_3426(bpmnEty, objRst2, item, Model.Enum.DirectionForDelete.Both);
                    }

                    List<Model.Base.IWayIfrm> wayIfrm = null;

                    List<Model.Base.IWayIfrm> wayIfrm2 = new List<Model.Base.IWayIfrm>();

                    wayIfrm = PublicMethods.DetectWayIfrmOfObjRst_578(bpmnEty, objRst);

                    PublicMethods.AddExistingObjRstToEvtRstAndChgPrs_549(bpmnEty, objRst, evtRst, objRst2, wayIfrm);
                }

                else
                {
                    this.TblNews.FldTtlNews = objRst.FldTtlNews;
                }
            }

            if (this.SlcNews.Result == PopupResult.Yes)
            {
                this.DefNews.PreviousActivity = this.TblNews.TblEvtRst.TblAct;
                this.DefNews.IsSelectSourceEnabel = false;
                this.DefNews.SelectSourceVisible = false;

                Util.ShowPopup(this.DefNews);

                if (this.DefNews.Result == PopupResult.OK)
                {
                    TblNew objRst = this.TblNews;

                    PublicMethods.DeleteWayIfrmOfObjRst_726(bpmnEty, objRst);

                    this.TblNews.FldTtlNews = this.DefNews.TblNews.FldTtlNews;
                }
                RaisePropertyChanged("TblWayAwr_News");
            }
        }

        /// <summary>
        /// حذف رخداد های آغازگر معادل با خبر جاری
        /// </summary>
        /// <param name="tbl"></param>
        private void DeleteEvtSrt(List<Model.TblWayIfrm_News> tbl)
        {
            for (int i = 0; i < tbl.Count; i++)
            {
                if (tbl[i].TblWayAwr_News.TblEvtSrt.TblWayAwr_News.Count == 1 && tbl[i].TblWayAwr_News.TblEvtSrt.TblWayAwr_Oral.Count == 0 && tbl[i].TblWayAwr_News.TblEvtSrt.TblWayAwr_RecvInt.Count == 0)
                {
                    this.bpmnEty.DeleteObject(tbl[i].TblWayAwr_News.TblEvtSrt);
                }
            }
        }


        #endregion

        #region ' events '

        #endregion

    }
}
