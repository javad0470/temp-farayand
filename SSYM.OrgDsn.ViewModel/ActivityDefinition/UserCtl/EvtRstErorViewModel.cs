using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using SSYM.OrgDsn.Model;
using System.Data.Objects.DataClasses;
using System.Collections.ObjectModel;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class EvtRstErorViewModel : UserControlViewModel
    {
        #region ' Fields '

        private Model.TblEvtRst tblEvtRst;
        bool isForAllErorSelected;



        #endregion

        #region ' Initialaizer '

        public EvtRstErorViewModel(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
        }

        protected override void Initialiaze()
        {
            base.Initialiaze();
            //TblEvtRst = new Model.TblEvtRst() { FldCodAct = 2, FldSttAct = 1, FldTypEvtRst = 3 };
            //bpmnEty.TblEvtRsts.AddObject(this.TblEvtRst);
            //TblEvtRst.TblErors.Add(bpmnEty.TblErors.SingleOrDefault(E => E.FldCodEror == 7));
            //TblEvtRst.TblErors.Add(bpmnEty.TblErors.SingleOrDefault(E => E.FldCodEror == 8));
            //TblEvtRst.TblErors.Add(bpmnEty.TblErors.SingleOrDefault(E => E.FldCodEror == 9));


            DeleteCommand = new DelegateCommand<TblEror>(ExecuteDeleteCommand);
            AddCommand = new DelegateCommand(ExecuteAddCommand);
            SlcEror = new Popup.SlcErorViewModel();
            SaveCommand = new DelegateCommand(ExecuteSaveCommand);
            DefErorViewModel = new Popup.DefErorViewModel();
            //this.IsForAllErorSelected = this.TblEvtRst.FldForAllEror ?? false;
            this.TblEvtRst.PropertyChanged += TblEvtRst_PropertyChanged;
        }

        void TblEvtRst_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldForAllEror")
            {
                if (this.TblEvtRst.FldForAllEror == true)
                {
                    SelectForAllErors();
                }
            }
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// TblEvtRst
        /// </summary>
        public Model.TblEvtRst TblEvtRst
        {
            get { return Entity as TblEvtRst; }
            set
            {
                Entity = value;
                RaisePropertyChanged("TblEvtRst");
            }
        }

        /// <summary>
        /// DeleteCommand
        /// </summary>
        public ICommand DeleteCommand { get; set; }


        /// <summary>
        /// add command
        /// </summary>
        public ICommand AddCommand { get; set; }

        /// <summary>
        /// SlcErorViewModel
        /// </summary>
        public Popup.SlcErorViewModel SlcEror { get; set; }

        /// <summary>
        /// SaveCommand
        /// </summary>
        public ICommand SaveCommand { get; set; }


        /// <summary>
        /// DefErorViewModel
        /// </summary>
        public Popup.DefErorViewModel DefErorViewModel { get; set; }

        /// <summary>
        /// به ازای تمامی خطاهایی که در این فعالیت رخ دهد
        /// </summary>
        //public bool IsForAllErorSelected
        //{
        //    get { return isForAllErorSelected; }
        //    set
        //    {
        //        isForAllErorSelected = value;
        //        TblEvtRst.FldForAllEror = value;
        //        if (value)
        //        {
        //            SelectForAllErors();
        //        }
        //        RaisePropertyChanged("IsForAllErorSelected");
        //    }
        //}

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcEror.Dispose();

            this.DefErorViewModel.Dispose();
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// ExecuteDeleteCommand
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDeleteCommand(TblEror obj)
        {
            TblEvtRst.TblErors.Remove(obj);
            RaisePropertyChanged("TblEvtRst");
        }

        /// <summary>
        /// ExecuteAddCommand
        /// </summary>
        private void ExecuteAddCommand()
        {
            this.SlcEror.IsSelectionModeSingle = false;

            SlcEror.DetectAllErors(this.TblEvtRst.TblErors.ToList());

            Util.ShowPopup(SlcEror);


            if (SlcEror.Result == PopupResult.OK)
            {
                //List<TblEror> tblEror = new List<TblEror>(this.TblEvtRst.TblErors);

                //for (int i = 0; i < tblEror.Count; i++)
                //{
                //    this.TblEvtRst.TblErors.Remove(tblEror[i]);
                //}

                this.TblEvtRst.TblErors.Clear();

                foreach (var item in this.SlcEror.SelectedItems)
                {
                    TblEror eror = this.bpmnEty.TblErors.SingleOrDefault(m => m.FldCodEror == item.FldCodEror);

                    this.TblEvtRst.TblErors.Add(eror);
                }

                RaisePropertyChanged("TblEvtRst");
            }
            if (SlcEror.Result == PopupResult.Yes)
            {
                Util.ShowPopup(DefErorViewModel);

                if (DefErorViewModel.Result == PopupResult.OK)
                {
                    Model.TblEror tbl = new Model.TblEror() { FldNamEror = DefErorViewModel.TblEror.FldNamEror, FldCodTypEror = DefErorViewModel.SelectedItem.FldCodTypEror };
                    this.bpmnEty.TblErors.AddObject(tbl);
                    this.TblEvtRst.TblErors.Add(tbl);
                }

            }

        }

        /// <summary>
        /// ExecuteSaveCommand
        /// </summary>
        private void ExecuteSaveCommand()
        {
            PublicMethods.SaveContext(bpmnEty);
        }

        /// <summary>
        /// P1744
        /// </summary>
        private void SelectForAllErors()
        {
            //454
            List<TblEvtRst> evtRst = PublicMethods.DetectEvtRstOfActWthSpcTyp_454(this.bpmnEty, this.TblEvtRst.TblAct, Model.Enum.EvtRstType.errAccurEvtRst);

            //1746
            if (evtRst.Count > 1)
            {
                //1750
                Util.ShowMessageBox(10);
                this.TblEvtRst.FldForAllEror = false;
            }
            //1749
            else
            {
                //752
                List<TblEror> eror = PublicMethods.DetectErorOfEvtRst_752(this.bpmnEty, this.TblEvtRst);

                //1752
                if (eror.Count > 0)
                {
                    if (Util.ShowMessageBox(40) == System.Windows.MessageBoxResult.Yes)
                    {
                        foreach (TblEror item in eror)
                        {
                            this.TblEvtRst.TblErors.Remove(item);

                            RaisePropertyChanged("TblEvtRst");
                        }

                        this.TblEvtRst.FldForAllEror = true;
                    }
                }
                //1751
                else
                {
                    //1752
                }
            }

        }

        #endregion

        #region ' events '

        #endregion

    }
}
