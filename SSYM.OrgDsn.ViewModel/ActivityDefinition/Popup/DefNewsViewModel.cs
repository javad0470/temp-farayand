using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class DefNewsViewModel : PopupViewModel
    {
        #region ' Fields '

        //private object parent;
        private bool isSelectSourceEnabel;
        //private string newsName;
        private string performerName;
        private Model.TblNew tblNews;
        private TblAct previousActivity;
        private int _codSelectedNod;
        private bool _selectSourceVisible;

        #endregion

        #region ' Initialaizer '

        public DefNewsViewModel(int codSelectedNod)
            : base(new BPMNDBEntities())
        {
            this.Width = PopupViewModel.SmallWidth;
            this.Height = PopupViewModel.SmallHeight;

            this.SelectSourceVisible = true;
            this._codSelectedNod = codSelectedNod;
            SlcSrcAndDstCommand = new DelegateCommand(ExecuteSlcSrcAndDstCommand);
            ActOfNodCommand = new DelegateCommand(ActOfNodExecute);
            SlcSrcAndDst = new SlcSrcAndDstViewModel();
            this.TblNews = new TblNew();
            this.TblNews.PropertyChanged += TblNews_PropertyChanged;
        }


        void TblNews_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldTtlNews")
            {
                RaiseOKCanExecute();
            }
        }



        #endregion

        #region ' Properties / Commands '

        /// <summary>
        ///  فعالیت تولید کننده خبر
        /// </summary>
        public TblAct PreviousActivity
        {
            get { return previousActivity; }
            set
            {
                previousActivity = value;
                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// can user select the source for the input
        /// </summary>
        public bool IsSelectSourceEnabel
        {
            get
            {
                return isSelectSourceEnabel;
            }
            set
            {
                isSelectSourceEnabel = value;
                RaisePropertyChanged("PerformerName", "IsSelectSourceEnabel");
            }
        }

        /// <summary>
        /// performer name
        /// </summary>
        public string PerformerName
        {
            get
            {
                
                    return performerName;
                
            }
            set
            {
                performerName = value;
                RaisePropertyChanged("PerformerName");
            }
        }

        /// <summary>
        /// TblNews
        /// </summary>
        public Model.TblNew TblNews
        {
            get { return tblNews; }
            set { tblNews = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SelectSourceVisible
        {
            get { return _selectSourceVisible; }
            set
            {
                _selectSourceVisible = value;
                RaisePropertyChanged("SelectSourceVisible");
            }
        }


        /// <summary>
        /// SlcSrcAndDstViewModel
        /// </summary>
        public SlcSrcAndDstViewModel SlcSrcAndDst { get; set; }

        /// <summary>
        /// SlcSrcAndDstCommand
        /// </summary>
        public ICommand SlcSrcAndDstCommand
        {
            get;
            set;
        }

        public ICommand ActOfNodCommand { get; set; }


        public TblEvtSrt EvtSrt { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public SlcActOfNodViewModel SlcActOfNodVM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelectActEnable { get; set; }


        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            SlcSrcAndDst.Dispose();
        }

        #endregion

        #region ' Private Methods '


        /// <summary>
        /// ExecuteSlcSrcAndDstCommand
        /// </summary>
        private void ExecuteSlcSrcAndDstCommand()
        {
            if (this.EvtSrt != null)
            {
                this.SlcSrcAndDst.IsDepOrgVisible = this.SlcSrcAndDst.IsOutOrgVisible = this.EvtSrt.FldTypEvtSrt == (int)Model.Enum.EvtSrtType.aftrAwareEvtSrt;
            }

            SlcSrcAndDst.IsSelectionModeSingle = true;

            Util.ShowPopup(SlcSrcAndDst);

            //if (SlcSrcAndDst.Result == PopupResult.OK && SlcSrcAndDst.SelectedItem != null)
            //{
            //    if (SlcSrcAndDst.SelectedItem.Nod.FldCodEty == _codSelectedNod)
            //    {
            //        Util.ShowMessageBox(56);
            //        return;
            //    }

            //    this.PreviousActivity = this.bpmnEty.TblActs.FirstOrDefault(m => m.FldCodNod == SlcSrcAndDst.SelectedItem.Nod.FldCodNod && m.FldActUspf);

            //    RaisePropertyChanged("PerformerName");
            //}


            if (SlcSrcAndDst.Result == PopupResult.OK && SlcSrcAndDst.SelectedItem != null)
            {
                //if (SlcSrcAndDst.SelectedItem.Nod.FldCodEty == _codSelectedNod)
                //{
                //    Util.ShowMessageBox(56);
                //    return;
                //}

                if (this.EvtSrt.FldTypEvtSrt == (int)EvtSrtType.aftrAwareEvtSrt)
                {
                    if (this.EvtSrt.TblAct.TblNod.FldCodNod == SlcSrcAndDst.SelectedItem.Nod.FldCodNod)
                    {
                        //امکان انتخاب فعالیت نامشخص وجود ندارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, this.EvtSrt.FldCodAct);
                        performerName = SlcActOfNodVM.SelectedAct.FldNamNod;
                        //PreviousActivity = SlcActOfNodVM.SelectedAct;
                        //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(a => !a.FldActUspf);
                    }
                    else
                    {
                        //امکان انتخاب فعالیت نامشخص وجود دارد
                        SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, SlcSrcAndDst.SelectedItem.Nod.FldCodNod != _codSelectedNod, this.EvtSrt.FldCodAct);
                        performerName = SlcActOfNodVM.SelectedAct.FldNamNod;
                        //PreviousActivity = SlcActOfNodVM.SelectedAct;
                        //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.Single(a => a.FldActUspf);
                    }
                }
                else
                {
                    //امکان انتخاب فعالیت نامشخص وجود ندارد
                    SlcActOfNodVM = new SlcActOfNodViewModel(this.bpmnEty, SlcSrcAndDst.SelectedItem.Nod, false, this.EvtSrt.FldCodAct);
                    performerName = SlcActOfNodVM.SelectedAct.FldNamNod;
                    //PreviousActivity = SlcActOfNodVM.SelectedAct;
                    //PreviousActivity = SlcSrcAndDst.SelectedItem.Nod.TblActs.FirstOrDefault(a => !a.FldActUspf);
                }

                IsSelectActEnable = true;
                IsSelectSourceEnabel = false;
                RaisePropertyChanged("SlcActOfNodVM", "IsSelectActEnable", "PreviousActivity", "PerformerName");
            }

        }

        protected override bool CanOKExecute()
        {
            if (this.PreviousActivity != null)
            {
                return !this.TblNews.HasErrors;
            }
            return false;
        }

        private void ActOfNodExecute()
        {
            Util.ShowPopup(SlcActOfNodVM);

            if (SlcActOfNodVM != null && PreviousActivity != SlcActOfNodVM.SelectedAct)
            {
                PreviousActivity = SlcActOfNodVM.SelectedAct;
                RaisePropertyChanged("PreviousActivity");
                RaiseOKCanExecute();
            }
        }

        #endregion
    }
}
