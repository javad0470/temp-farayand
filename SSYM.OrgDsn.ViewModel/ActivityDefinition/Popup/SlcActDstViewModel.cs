using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SSYM.OrgDsn.Model.Base;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcActDstViewModel : PopupViewModel
    {
        #region ' Fields '

        TblEvtRst _evtRstCnt;

        private Tuple<IObjRst, TblEvtSrt> fomMeeSlcEedItm;

        ListCollectionView recvFomMee;

        ObservableCollection<TblAct> _actOfNodCnt;

        TblAct _actOfNodCntSelectedItem;

        bool _isActsOfNodCnt;

        bool isFomMeeSlcEed;

        string _firstTabHeader;

        public string FirstTabHeader
        {
            get { return _firstTabHeader; }
            set
            {
                _firstTabHeader = value;
                RaisePropertyChanged("FirstTabHeader");
            }
        }


        #endregion

        #region ' Initialaizer '

        public SlcActDstViewModel()
            : base(new BPMNDBEntities())
        {
            _firstTabHeader = "دریافت کننده از من";
            ActDoesnExistCommand = new DelegateCommand(ExecuteActDoesnExistCommand);
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public bool IsActsOfNodCntSelected
        {
            get { return _isActsOfNodCnt; }
            set
            {
                _isActsOfNodCnt = value;
                if (value)
                {
                    DetectActsOfNodCnt();
                }

                RaisePropertyChanged("IsActsOfNodCntSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblAct> ActOfNodCnt
        {
            get { return _actOfNodCnt; }
            set
            {
                _actOfNodCnt = value;

                RaisePropertyChanged("ActOfNodCnt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblAct ActOfNodCntSelectedItem
        {
            get { return _actOfNodCntSelectedItem; }
            set
            {
                _actOfNodCntSelectedItem = value;

                RaisePropertyChanged("ActOfNodCntSelectedItem");

                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// رخداد نتیجه جاری
        /// </summary>
        public TblEvtRst EvtRstCnt
        {
            get { return _evtRstCnt; }
            set { _evtRstCnt = value; }
        }

        /// <summary>
        /// دریافت شده از من
        /// </summary>
        public ListCollectionView RecvFomMee
        {
            get { return recvFomMee; }
            set { recvFomMee = value; }
        }

        /// <summary>
        /// FromMeSelectedItem
        /// </summary>
        public Tuple<IObjRst, TblEvtSrt> FomMeeSlcEedItm
        {
            get
            {
                return fomMeeSlcEedItm;
            }
            set
            {
                fomMeeSlcEedItm = value;

                RaisePropertyChanged("FomMeeSlcEedItm");

                RaiseOKCanExecute();
            }
        }

        /// <summary>
        /// is from me selected
        /// </summary>
        public bool IsFomMeeSlcEed
        {
            get { return isFomMeeSlcEed; }
            set
            {
                isFomMeeSlcEed = value;
                if (value)
                {
                    DetectFomMeObj();
                }
                RaisePropertyChanged("IsFomMeeSlcEed");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public ICommand ActDoesnExistCommand { get; set; }

        #endregion

        #region ' Public Methods '

        /// <summary>
        /// شناسایی تمامی فعالیتهایی که دریافت کننده از گره جاری  هستند
        /// </summary>
        public void DetectFomMeObj()
        {
            PublicMethods.ReloadEntity(this.bpmnEty, this.bpmnEty.TblSbjOrals);

            List<Tuple<IObjRst, TblEvtSrt>> lst = this.EvtRstCnt.TblAct.TblNod.AllObjRstSentFromNod.Where(m => m.Item1.GetType() == typeof(TblSbjOral)).ToList();

            List<TblSbjOral> objRst = this.EvtRstCnt.TblSbjOrals.ToList();

            List<TblAct> act = new List<TblAct>();

            objRst.ForEach(m => act.AddRange(m.ActTarget));

            List<Tuple<IObjRst, TblEvtSrt>> lst2 = new List<Tuple<IObjRst, TblEvtSrt>>();

            foreach (var item in lst)
            {
                if (act.Where(m => m.FldCodAct == item.Item2.FldCodAct).Count() == 0 && !item.Item2.TblAct.FldActUspf && lst2.Where(x => x.Item2.FldCodAct == item.Item2.FldCodAct).Count() == 0)
                {
                    lst2.Add(item);
                }
            }

            this.RecvFomMee = new ListCollectionView(lst2);

            this.RecvFomMee.GroupDescriptions.Add(new PropertyGroupDescription("Item1.CodObj"));

            RaisePropertyChanged("RecvFomMee");
        }

        /// <summary>
        /// 
        /// </summary>
        public void DetectActsOfNodCnt()
        {
            List<TblAct> act = this.EvtRstCnt.TblAct.TblNod.TblActs.Where(m => m.FldCodAct != this.EvtRstCnt.TblAct.FldCodAct && !m.FldActUspf).ToList();

            List<TblSbjOral> sbjOral = this.EvtRstCnt.TblSbjOrals.ToList();

            List<TblAct> act2 = new List<TblAct>();

            sbjOral.ForEach(m => act2.AddRange(m.ActTarget));

            this.ActOfNodCnt = new ObservableCollection<TblAct>();

            foreach (var item in act)
            {
                if (act2.Where(m => m.FldCodAct == item.FldCodAct).Count() == 0)
                {
                    this.ActOfNodCnt.Add(item);
                }
            }
        }

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteActDoesnExistCommand()
        {
            this.Result = PopupResult.Yes;
        }

        protected override bool CanOKExecute()
        {
            return FomMeeSlcEedItm != null
                || ActOfNodCntSelectedItem != null;
        }
        #endregion

        #region ' events '

        #endregion

    }
}
