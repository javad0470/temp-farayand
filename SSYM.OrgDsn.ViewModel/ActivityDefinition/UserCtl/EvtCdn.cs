using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
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
    public class EvtCdn : UserControlViewModel
    {
        #region ' Fields '

        ObservableCollection<SSYM.OrgDsn.Model.TblIdx> tblIdx;
        ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> compareTools;
        ObservableCollection<SSYM.OrgDsn.Model.TblUntMsrt> tblUntMsrt;

        string newIdxTxt;
        string newSbjMsrtTxt;
        string newUntMsrtTxt;


        TblCdn selectedCdn;




        #endregion

        #region ' Initializer '

        public EvtCdn(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
            DeleteCommand = new DelegateCommand<SSYM.OrgDsn.Model.TblCdn>(ExcecuteDeleteCommand);
            AddNewRowCommand = new DelegateCommand(ExecuteAddNewRowCommand);
            DetectAllIdx();
            DetectAllUnt();
            RaisePropertyChanged("TblCdn");
            SlcIdx = new Popup.SlcIdxViewModel();
            SlcUnt = new Popup.SlcUntViewModel();
            DefIdx = new Popup.DefIdxViewModel();
            DefUnt = new Popup.DefUntViewModel();
            SlcIdxCommand = new DelegateCommand<TblCdn>(ExecuteSlcIdxCommand);
            SlcUntCommand = new DelegateCommand<TblCdn>(ExecuteSlcUntCommand);
        }

        #endregion

        #region ' Properties / Commands '




        /// <summary>
        /// 
        /// </summary>
        public TblCdn SelectedCdn
        {
            get { return selectedCdn; }
            set
            {
                selectedCdn = value;

                RaisePropertyChanged("SelectedCdn");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ViewModel.ActivityDefinition.Popup.DefIdxViewModel DefIdx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewModel.ActivityDefinition.Popup.DefUntViewModel DefUnt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewModel.ActivityDefinition.Popup.SlcIdxViewModel SlcIdx { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewModel.ActivityDefinition.Popup.SlcUntViewModel SlcUnt { get; set; }


        /// <summary>
        /// لیست تمام شرایط ثبت شد ه برای رخداد جاری
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblCdn> TblCdn
        {
            get
            {
                return new ObservableCollection<Model.TblCdn>(this.Evt.TblCdns);
            }
        }

        /// <summary>
        /// رخداد نتیجه
        /// </summary>
        public IEvt Evt
        {
            get
            {
                return Entity as IEvt;
            }
            set
            {
                Entity = (EntityObject)value;
            }
        }

        /// <summary>
        /// Delete Command
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// جدول شاخص ها
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblIdx> TblIdx
        {
            get
            {
                return tblIdx;
            }
            set
            {
                tblIdx = value;

                RaisePropertyChanged("TblIdx");
            }
        }




        /// <summary>
        /// ابزارهای مقایسه
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> CompareTools
        {
            get
            {
                if (compareTools == null)
                {
                    compareTools = new ObservableCollection<Model.TblItmFixSfw>(bpmnEty.TblItmFixSfws.Where(E => E.FldCodSbj == 7));
                }
                return compareTools;
            }
        }

        /// <summary>
        /// واحدهای سنجش
        /// </summary>
        public ObservableCollection<SSYM.OrgDsn.Model.TblUntMsrt> TblUntMsrt
        {
            get
            {
                return tblUntMsrt;
            }
            set
            {
                tblUntMsrt = value;

                RaisePropertyChanged("TblUntMsrt");
            }
        }

        /// <summary>
        /// Add new row command
        /// </summary>
        public ICommand AddNewRowCommand { get; set; }

        /// <summary>
        /// save command
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// SlcIdxCommand
        /// </summary>
        public ICommand SlcIdxCommand { get; set; }

        /// <summary>
        /// SlcUntCommand
        /// </summary>
        public ICommand SlcUntCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NewIdxTxt
        {
            get { return newIdxTxt; }
            set
            {
                newIdxTxt = value;
                RaisePropertyChanged("NewIdxTxt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NewSbjMsrtTxt
        {
            get { return newSbjMsrtTxt; }
            set
            {
                newSbjMsrtTxt = value;
                RaisePropertyChanged("NewSbjMsrtTxt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NewUntMsrtTxt
        {
            get { return newUntMsrtTxt; }
            set
            {
                newUntMsrtTxt = value;
                RaisePropertyChanged("NewUntMsrtTxt");
            }
        }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// 
        /// </summary>
        public void AddIdx()
        {
            if (NewSbjMsrtTxt != null && NewIdxTxt != null)
            {
                TblSbjMsrt tbl = new TblSbjMsrt() { FldNamSbjMsrt = this.NewSbjMsrtTxt };
                if (bpmnEty.TblSbjMsrts.Where(m => m.FldNamSbjMsrt == NewSbjMsrtTxt).Count() == 0)
                {
                    bpmnEty.TblSbjMsrts.AddObject(tbl);
                }
                else
                {
                    tbl = bpmnEty.TblSbjMsrts.SingleOrDefault(m => m.FldNamSbjMsrt == NewSbjMsrtTxt);
                }
                TblIdx tbl1 = new TblIdx() { FldNamIdx = NewIdxTxt, FldCodOrg = UserManager.CurrentUser.FldCodOrg };
                tbl.TblIdxes.Add(tbl1);
                this.TblIdx.Add(tbl1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddUntMsrt()
        {
            if (NewSbjMsrtTxt != null && NewUntMsrtTxt != null)
            {
                TblSbjMsrt tbl = new TblSbjMsrt() { FldNamSbjMsrt = this.NewSbjMsrtTxt };
                if (bpmnEty.TblSbjMsrts.Where(m => m.FldNamSbjMsrt == NewSbjMsrtTxt).Count() == 0)
                {
                    bpmnEty.TblSbjMsrts.AddObject(tbl);
                }
                else
                {
                    tbl = bpmnEty.TblSbjMsrts.SingleOrDefault(m => m.FldNamSbjMsrt == NewSbjMsrtTxt);
                }
                TblUntMsrt tbl1 = new TblUntMsrt() { FldNamUntMsrt = this.NewUntMsrtTxt };
                tbl.TblUntMsrts.Add(tbl1);
                this.TblUntMsrt.Add(tbl1);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            SlcIdx.Dispose();

            SlcUnt.Dispose();

            DefIdx.Dispose();

            DefUnt.Dispose();
        }




        #endregion

        #region ' Private Methods '

        /// <summary>
        /// delete command
        /// </summary>
        /// <param name="obj"> a parameter of type Model.TblCdn</param>
        private void ExcecuteDeleteCommand(Model.TblCdn obj)
        {
            if (Util.ShowMessageBox(39) == System.Windows.MessageBoxResult.Yes)
            {
                this.bpmnEty.DeleteObject(obj);
                this.Evt.TblCdns.Remove(obj);
                RaisePropertyChanged("TblCdn");
            }
        }

        /// <summary>
        /// add new row command
        /// </summary>
        private void ExecuteAddNewRowCommand()
        {
            TblIdx idx = bpmnEty.TblIdxes.FirstOrDefault();
            TblUntMsrt unt = bpmnEty.TblUntMsrts.FirstOrDefault();
            this.Evt.TblCdns.Add(new Model.TblCdn() { FldQntyIdx = 1, FldCodIdx = idx != null ? idx.FldCodIdx : 0, FldCodRlnIdx = CompareTools.First().FldCodItm, FldCodUntMsrtIdx = unt != null ? unt.FldCodUntMsrt : 0 });
            RaisePropertyChanged("TblCdn");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSlcIdxCommand(Model.TblCdn obj)
        {
            this.SelectedCdn = obj;

            Util.ShowPopup(this.SlcIdx);

            if (this.SlcIdx.Result == PopupResult.OK)
            {
                this.SelectedCdn.TblIdx = this.bpmnEty.TblIdxes.SingleOrDefault(m => m.FldCodIdx == this.SlcIdx.SelectedIdx.FldCodIdx);
            }

            else if (this.SlcIdx.Result == PopupResult.Yes)
            {
                Util.ShowPopup(DefIdx);

                if (this.DefIdx.Result == PopupResult.OK)
                {
                    DetectAllIdx();

                    this.SelectedCdn.TblIdx = this.TblIdx.SingleOrDefault(m => m.FldCodIdx == this.DefIdx.NewIdx.FldCodIdx);
                }

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSlcUntCommand(Model.TblCdn obj)
        {
            this.SelectedCdn = obj;

            Util.ShowPopup(this.SlcUnt);

            if (this.SlcUnt.Result == PopupResult.OK)
            {
                this.SelectedCdn.TblUntMsrt = this.bpmnEty.TblUntMsrts.SingleOrDefault(m => m.FldCodUntMsrt == this.SlcUnt.SelectedItem.FldCodUntMsrt);
            }

            else if (this.SlcUnt.Result == PopupResult.Yes)
            {
                Util.ShowPopup(DefUnt);


                if (this.DefUnt.Result == PopupResult.OK)
                {
                    DetectAllUnt();

                    this.SelectedCdn.TblUntMsrt = this.TblUntMsrt.SingleOrDefault(m => m.FldCodUntMsrt == this.DefUnt.TblUntMsrt.FldCodUntMsrt);
                }


            }


        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteIdxDoesnExistCommand()
        {
            Util.ShowPopup(DefIdx);

            if (this.DefIdx.Result == PopupResult.OK)
            {
                DetectAllIdx();

                this.SelectedCdn.TblIdx = this.TblIdx.SingleOrDefault(m => m.FldCodIdx == this.DefIdx.NewIdx.FldCodIdx);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DetectAllIdx()
        {
            TblIdx = new ObservableCollection<Model.TblIdx>(bpmnEty.TblIdxes.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectAllUnt()
        {
            TblUntMsrt = new ObservableCollection<Model.TblUntMsrt>(bpmnEty.TblUntMsrts);
        }

        #endregion

    }
}
