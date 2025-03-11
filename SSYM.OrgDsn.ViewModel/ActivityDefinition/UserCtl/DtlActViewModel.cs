using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
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
    public class DtlActViewModel : UserControlViewModel
    {

        #region ' Fields '

        private Model.TblAct tblAct;
        private SlcSfwViewModel slcSfw;
        private DefSfwViewModel defSfw;
        private bool isSlcSfwPopupOpen;
        private bool softwareDoesntExist;
        private bool activityHasInputSoftware;
        private bool activityHasOutputSoftware;
        private bool activityHasSoftware;
        private int activityTypeSelectedItem;

        ObservableCollection<Model.TblAct_Sfw> activitySoftwares;
        ObservableCollection<Model.TblAct_Sfw> outputActivitySoftwares;
        ObservableCollection<Model.TblAct_Sfw> inputActivitySoftwares;



        #endregion

        #region ' Initialaizer '

        public DtlActViewModel(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
            //DeleteSoftwareQuestion = new GenericInteractionRequest<PopupDataObject>();

            TblAct.PropertyChanged -= act_PropertyChanged;
            TblAct.PropertyChanged += act_PropertyChanged;
        }



        protected override void Initialiaze()
        {
            base.Initialiaze();

            //P1528--------------------------/

            this.ActivitySoftwares = new ObservableCollection<TblAct_Sfw>(PublicMethods.DetectSfwOfAct_1520(this.bpmnEty, this.TblAct));

            this.InputActivitySoftwares = new ObservableCollection<TblAct_Sfw>(PublicMethods.DetectInputSfwOfAct_1521(this.bpmnEty, this.TblAct));

            this.OutputActivitySoftwares = new ObservableCollection<TblAct_Sfw>(PublicMethods.DetectOutputSfwOfAct_1522(this.bpmnEty, this.TblAct));

            //-------------------------------/

            SlcSfw = new SlcSfwViewModel();
            DefSfw = new DefSfwViewModel();
            OpenSlcSfwCommand = new DelegateCommand(ExecuteOpenSlcSfwCommand);
            OpenSlcInputSfwCommand = new DelegateCommand(ExecuteOpenSlcInputSfwCommand);
            OpenSlcOutputSfwCommand = new DelegateCommand(ExecuteOpenSlcOutputSfwCommand);
            saveChangesCommand = new DelegateCommand(ExecutesaveChangesCommand);
            DeleteSfwOfActCommand = new DelegateCommand<TblAct_Sfw>(ExecuteDeleteSfwOfActCommand);
            if (this.TblAct != null)
            {
                this.TblAct.PropertyChanged += TblAct_PropertyChanged;
                if (this.TblAct.TblAct_Sfw.Where(E => E.FldTypUseSfw == (int)Model.Enum.ActivitySoftwareTypes.ActivitySoftware).Count() > 0)
                {
                    this.ActivityHasSoftware = true;
                }
                else
                {
                    this.ActivityHasSoftware = false;
                }
                if (this.TblAct.TblAct_Sfw.Where(E => E.FldTypUseSfw == (int)Model.Enum.ActivitySoftwareTypes.InputActivitySoftware).Count() > 0)
                {
                    this.ActivityHasInputSoftware = true;
                }
                else
                {
                    this.ActivityHasInputSoftware = false;
                }
                if (this.TblAct.TblAct_Sfw.Where(E => E.FldTypUseSfw == (int)Model.Enum.ActivitySoftwareTypes.OutputActivitySoftware).Count() > 0)
                {
                    this.ActivityHasOutputSoftware = true;
                }
                else
                {
                    this.ActivityHasOutputSoftware = false;
                }
            }


        }


        ~DtlActViewModel()
        {
            TblAct.PropertyChanged -= act_PropertyChanged;
        }

        #endregion

        #region ' Properties / Commands '

        private bool _tabsEnabled;

        public bool TabsEnabled
        {
            get { return _tabsEnabled; }
            set
            {
                _tabsEnabled = value;
                RaisePropertyChanged("TabsEnabled");
            }
        }

        //public GenericInteractionRequest<PopupDataObject> DeleteSoftwareQuestion { get; private set; }

        /// <summary>
        /// gets or sets the activity
        /// </summary>
        public Model.TblAct TblAct
        {
            get
            {
                var act = Entity as TblAct;
                return act;
            }
            set
            {
                Entity = value;
            }
        }


        /// <summary>
        /// انواع فعالیت
        /// </summary>
        public ObservableCollection<Model.TblItmFixSfw> ActivityTypes
        {
            get
            {
                return new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 27));
            }
        }

        /// <summary>
        /// دارد/ندارد
        /// </summary>
        public ObservableCollection<Model.TblItmFixSfw> HaveOrNotHave
        {
            get
            {
                return new ObservableCollection<TblItmFixSfw>(PublicMethods.TblItmFixSfws.Where(E => E.FldCodSbj == 19));
            }
        }

        /// <summary>
        /// نرم افزارهای فعالیت
        /// </summary>
        public ObservableCollection<Model.TblAct_Sfw> ActivitySoftwares
        {
            get { return activitySoftwares; }
            set { activitySoftwares = value; }
        }

        /// <summary>
        /// نرم افزارهای ورودی دهنده به فعالیت
        /// </summary>
        public ObservableCollection<Model.TblAct_Sfw> InputActivitySoftwares
        {
            get { return inputActivitySoftwares; }
            set { inputActivitySoftwares = value; }
        }

        /// <summary>
        /// نرم افزارهای دریافت کننده خروجی از فعالیت
        /// </summary>
        public ObservableCollection<Model.TblAct_Sfw> OutputActivitySoftwares
        {
            get { return outputActivitySoftwares; }
            set { outputActivitySoftwares = value; }
        }

        /// <summary>
        /// type of ativity
        /// </summary>
        public int ActivityTypeSelectedItem
        {
            get { return this.TblAct.FldTypAct; }
            set
            {
                this.TblAct.FldTypAct = value;
                if (value == (int)Model.Enum.ActivityTypes.UserTask)
                {
                    this.ActivityHasSoftware = true;
                }
                else
                {
                    this.ActivityHasSoftware = false;
                }
                RaisePropertyChanged("ActivityTypeSelectedItem");
            }
        }

        /// <summary>
        /// ActivityHasSoftware
        /// </summary>
        public bool ActivityHasSoftware
        {
            get
            {
                if (this.TblAct.FldTypAct == (int)Model.Enum.ActivityTypes.UserTask)
                {
                    return true;
                }
                return false;
            }
            set
            {
                if (!value && ActivitySoftwares.Count > 0)
                {
                    if (Util.ShowMessageBox(2, "نرم افزارهای ثبت شده")  == System.Windows.MessageBoxResult.Yes)
                    {
                        List<Model.TblAct_Sfw> tbl = new List<TblAct_Sfw>(this.ActivitySoftwares);
                        for (int i = 0; i < tbl.Count; i++)
                        {
                            this.TblAct.TblAct_Sfw.Remove(tbl[i]);

                            this.ActivitySoftwares.Remove(tbl[i]);
                        }
                        RaisePropertyChanged("ActivityHasSoftware", "ActivitySoftwares");

                    }
                    else
                    {
                        ActivityTypeSelectedItem = (int)Model.Enum.ActivityTypes.UserTask;
                    }
                }
                else
                {
                    RaisePropertyChanged("ActivityHasSoftware");
                }
            }
        }

        /// <summary>
        /// ActivityHasOutputSoftware
        /// </summary>
        public bool ActivityHasOutputSoftware
        {
            get
            {
                return activityHasOutputSoftware;
            }
            set
            {
                if (!value && activityHasOutputSoftware && OutputActivitySoftwares.Count > 0)
                {
                    if (Util.ShowMessageBox(2, "نرم افزارهای ثبت شده") == System.Windows.MessageBoxResult.Yes)
                    {
                        List<Model.TblAct_Sfw> tbl = new List<TblAct_Sfw>(this.OutputActivitySoftwares);
                        for (int i = 0; i < tbl.Count; i++)
                        {
                            this.TblAct.TblAct_Sfw.Remove(tbl[i]);

                            this.OutputActivitySoftwares.Remove(tbl[i]);
                        }
                        activityHasOutputSoftware = value;
                        RaisePropertyChanged("ActivityHasOutputSoftware", "OutputActivitySoftwares");

                    }
                    else
                    {
                        return; 
                    }
                }
                else
                {
                    activityHasOutputSoftware = value;
                    RaisePropertyChanged("ActivityHasOutputSoftware");
                }
            }
        }

        /// <summary>
        /// ActivityHasInputSoftware
        /// </summary>
        public bool ActivityHasInputSoftware
        {
            get
            {
                return activityHasInputSoftware;
                //if (this.TblAct.TblAct_Sfw.Where(E => E.FldTypUseSfw == (int)Model.Enum.ActivitySoftwareTypes.InputActivitySoftware).Count() > 0)
                //{
                //    return true;
                //}
                //return false;
            }
            set
            {
                if (!value && activityHasInputSoftware && InputActivitySoftwares.Count > 0)
                {
                    if (Util.ShowMessageBox(2, "نرم افزارهای ثبت شده") == System.Windows.MessageBoxResult.Yes)
                    {
                        List<Model.TblAct_Sfw> tbl = new List<TblAct_Sfw>(this.InputActivitySoftwares);
                        for (int i = 0; i < tbl.Count; i++)
                        {
                            this.TblAct.TblAct_Sfw.Remove(tbl[i]);

                            this.InputActivitySoftwares.Remove(tbl[i]);
                        }
                        activityHasInputSoftware = value;
                        RaisePropertyChanged("ActivityHasInputSoftware", "InputActivitySoftwares");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    activityHasInputSoftware = value;
                    RaisePropertyChanged("ActivityHasInputSoftware");
                }
            }
        }

        /// <summary>
        /// تعریف نرم افزار
        /// </summary>
        public DefSfwViewModel DefSfw
        {
            get { return defSfw; }
            set { defSfw = value; }
        }

        /// <summary>
        /// انتخاب نرم افزار
        /// </summary>
        public SlcSfwViewModel SlcSfw
        {
            get { return slcSfw; }
            set { slcSfw = value; }
        }

        /// <summary>
        /// فرم انتخاب نرم افزار باز است
        /// </summary>
        public bool IsSlcSfwPopupOpen
        {
            get { return isSlcSfwPopupOpen; }
            set
            {
                isSlcSfwPopupOpen = value;
                RaisePropertyChanged("IsSlcSfwPopupOpen");
                if (!value && this.SlcSfw.Result == PopupResult.OK)
                {
                    List<Model.TblAct_Sfw> tbl = new List<Model.TblAct_Sfw>(this.TblAct.TblAct_Sfw);
                    for (int i = 0; i < tbl.Count(); i++)
                    {
                        if (tbl[i].FldTypUseSfw == (int)this.ActivitySoftwareType)
                        {
                            this.TblAct.TblAct_Sfw.Remove(tbl[i]);
                        }
                    }

                    foreach (var item in this.SlcSfw.SelectedItems)
                    {
                        int i = (int)item.GetType().GetProperty("FldCodSfw").GetValue(item, null);

                        int j = (int)ActivitySoftwareType;

                        TblAct_Sfw tblAct_Sfw = new Model.TblAct_Sfw() { FldCodSfw = i, FldTypUseSfw = j };

                        this.TblAct.TblAct_Sfw.Add(tblAct_Sfw);

                        if (this.ActivitySoftwareType == ActivitySoftwareTypes.ActivitySoftware)
                        {
                            this.ActivitySoftwares.Add(tblAct_Sfw);
                        }
                        else if (this.ActivitySoftwareType == ActivitySoftwareTypes.InputActivitySoftware)
                        {
                            this.InputActivitySoftwares.Add(tblAct_Sfw);
                        }
                        else if (this.ActivitySoftwareType == ActivitySoftwareTypes.OutputActivitySoftware)
                        {
                            this.OutputActivitySoftwares.Add(tblAct_Sfw);
                        }
                    }
                }
                if (!value && this.SlcSfw.Result == PopupResult.Yes)
                {
                    this.SoftwareDoesntExist = true;
                }
            }
        }

        /// <summary>
        /// نرم افزار مورد نظر پیدا نشد
        /// </summary>
        public bool SoftwareDoesntExist
        {
            get { return softwareDoesntExist; }
            set
            {
                softwareDoesntExist = value;
                RaisePropertyChanged("SoftwareDoesntExist");
                if (!value && this.DefSfw.Result == PopupResult.OK)
                {
                    TblSfw sfw = new TblSfw() { FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldNamSfw = this.DefSfw.TblSfw.FldNamSfw };

                    this.bpmnEty.TblSfws.AddObject(sfw);

                    PublicMethods.SaveContext(this.bpmnEty);

                    int i = (int)ActivitySoftwareType;

                    TblAct_Sfw tblAct_Sfw = new TblAct_Sfw() { FldCodAct = this.TblAct.FldCodAct, FldCodSfw = sfw.FldCodSfw, FldTypUseSfw = i };

                    this.TblAct.TblAct_Sfw.Add(tblAct_Sfw);

                    switch (ActivitySoftwareType)
                    {
                        case ActivitySoftwareTypes.ActivitySoftware:
                            this.ActivitySoftwares.Add(tblAct_Sfw);
                            break;

                        case ActivitySoftwareTypes.InputActivitySoftware:
                            this.InputActivitySoftwares.Add(tblAct_Sfw);
                            break;

                        case ActivitySoftwareTypes.OutputActivitySoftware:
                            this.OutputActivitySoftwares.Add(tblAct_Sfw);
                            break;

                        default:
                            break;
                    }

                    RaisePropertyChanged("SoftwareDoesntExist", "ActivitySoftwares", "InputActivitySoftwares", "OutputActivitySoftwares");
                }
            }
        }

        /// <summary>
        /// نوع نرم افزار برای فعالیت
        /// </summary>
        public Model.Enum.ActivitySoftwareTypes ActivitySoftwareType
        {
            get;
            set;
        }

        /// <summary>
        /// این فعالیت رخداد نتیجه ندارد
        /// </summary>
        public bool EvtRstDoesntExist
        {
            get
            {
                if (this.TblAct.TblEvtRsts.Count() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                RaisePropertyChanged("EvtRstDoesntExist");
            }
        }

        /// <summary>
        /// انتخاب نرم افزار فعالیت
        /// </summary>
        public ICommand OpenSlcSfwCommand { get; set; }

        /// <summary>
        /// انتخاب نرم افزار ورودی دهنده به فعالیت
        /// </summary>
        public ICommand OpenSlcInputSfwCommand { get; set; }

        /// <summary>
        /// انتخاب نرم افزار خروجی گیرنده از فعالیت
        /// </summary>
        public ICommand OpenSlcOutputSfwCommand { get; set; }

        /// <summary>
        /// saveChangesCommand
        /// </summary>
        public ICommand saveChangesCommand { get; set; }

        /// <summary>
        /// حذف نرم افزارهای فعالیت
        /// </summary>
        public ICommand DeleteSfwOfActCommand { get; set; }

        #endregion

        #region ' Public Methods '

        public override void Dispose()
        {
            base.Dispose();

            this.SlcSfw.Dispose();

            this.DefSfw.Dispose();
        }


        #endregion

        #region ' Private Methods '
        void TblAct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldTypAct")
            {
                RaisePropertyChanged("ActivityTypeSelectedItem", "ActivityHasSoftware");
            }
        }

        private void ExecuteOpenSlcSfwCommand()
        {
            SlcSfw.IsSelectionModeSingle = false;
            this.ActivitySoftwareType = Model.Enum.ActivitySoftwareTypes.ActivitySoftware;

            //حذف موارد تکراری
            this.SlcSfw.DetectAllSfws(ActivitySoftwares.ToList().Select(a_s => a_s.TblSfw).ToList());

            openSlcSfw();
        }

        private void openSlcSfw()
        {

            Util.ShowPopup(SlcSfw);

            if (SlcSfw.Result == PopupResult.OK)
            {
                List<Model.TblAct_Sfw> tbl = new List<Model.TblAct_Sfw>(this.TblAct.TblAct_Sfw);
                for (int i = 0; i < tbl.Count(); i++)
                {
                    if (tbl[i].FldTypUseSfw == (int)this.ActivitySoftwareType)
                    {
                        this.TblAct.TblAct_Sfw.Remove(tbl[i]);
                    }
                }

                switch (this.ActivitySoftwareType)
                {
                    case ActivitySoftwareTypes.ActivitySoftware:
                        this.ActivitySoftwares.Clear();
                        break;
                    case ActivitySoftwareTypes.InputActivitySoftware:
                        this.InputActivitySoftwares.Clear();
                        break;
                    case ActivitySoftwareTypes.OutputActivitySoftware:
                        this.OutputActivitySoftwares.Clear();
                        break;
                    default:
                        break;
                }

                foreach (var item in this.SlcSfw.SelectedItems)
                {
                    int j = (int)ActivitySoftwareType;

                    //var sfw = this.TblAct.TblAct_Sfw.SingleOrDefault(a_s=>a_s.FldCodSfw == item.FldCodSfw);

                    //if (sfw != null)
                    //{
                    //    this.TblAct.TblAct_Sfw.Remove(sfw);
                    //    continue;
                    //}

                    TblAct_Sfw tblAct_Sfw = new Model.TblAct_Sfw() { FldCodSfw = item.FldCodSfw, FldTypUseSfw = j };

                    this.TblAct.TblAct_Sfw.Add(tblAct_Sfw);

                    if (this.ActivitySoftwareType == ActivitySoftwareTypes.ActivitySoftware)
                    {
                        this.ActivitySoftwares.Add(tblAct_Sfw);
                    }
                    else if (this.ActivitySoftwareType == ActivitySoftwareTypes.InputActivitySoftware)
                    {
                        this.InputActivitySoftwares.Add(tblAct_Sfw);
                    }
                    else if (this.ActivitySoftwareType == ActivitySoftwareTypes.OutputActivitySoftware)
                    {
                        this.OutputActivitySoftwares.Add(tblAct_Sfw);
                    }
                }
            }

            else if (SlcSfw.Result == PopupResult.Yes)
            {
                openDefSfw();
            }
        }


        private void openDefSfw()
        {

            Util.ShowPopup(DefSfw);

            if (this.DefSfw.Result == PopupResult.OK)
            {
                TblSfw sfw = new TblSfw() { FldCodOrg = PublicMethods.CurrentUser.TblOrg.FldCodOrg, FldNamSfw = this.DefSfw.TblSfw.FldNamSfw };

                this.bpmnEty.TblSfws.AddObject(sfw);

                PublicMethods.SaveContext(this.bpmnEty);

                int i = (int)ActivitySoftwareType;

                TblAct_Sfw tblAct_Sfw = new TblAct_Sfw() { FldCodAct = this.TblAct.FldCodAct, FldCodSfw = sfw.FldCodSfw, FldTypUseSfw = i };

                this.TblAct.TblAct_Sfw.Add(tblAct_Sfw);

                switch (ActivitySoftwareType)
                {
                    case ActivitySoftwareTypes.ActivitySoftware:
                        this.ActivitySoftwares.Add(tblAct_Sfw);
                        break;

                    case ActivitySoftwareTypes.InputActivitySoftware:
                        this.InputActivitySoftwares.Add(tblAct_Sfw);
                        break;

                    case ActivitySoftwareTypes.OutputActivitySoftware:
                        this.OutputActivitySoftwares.Add(tblAct_Sfw);
                        break;

                    default:
                        break;
                }

                RaisePropertyChanged("ActivitySoftwares", "InputActivitySoftwares", "OutputActivitySoftwares");
            }

        }

        private void ExecuteOpenSlcOutputSfwCommand()
        {
            SlcSfw.IsSelectionModeSingle = false;
            this.ActivitySoftwareType = Model.Enum.ActivitySoftwareTypes.OutputActivitySoftware;
            this.SlcSfw.DetectAllSfws();

            //حذف موارد تکراری
            this.SlcSfw.DetectAllSfws(OutputActivitySoftwares.ToList().Select(a_s => a_s.TblSfw).ToList());

            openSlcSfw();
            //IsSlcSfwPopupOpen = true;
        }

        private void ExecuteOpenSlcInputSfwCommand()
        {
            SlcSfw.IsSelectionModeSingle = false;
            this.ActivitySoftwareType = Model.Enum.ActivitySoftwareTypes.InputActivitySoftware;
            this.SlcSfw.DetectAllSfws();

            //حذف موارد تکراری
            this.SlcSfw.DetectAllSfws(InputActivitySoftwares.ToList().Select(a_s => a_s.TblSfw).ToList());


            openSlcSfw();
            //IsSlcSfwPopupOpen = true;
        }

        private void ExecutesaveChangesCommand()
        {
            PublicMethods.SaveContext(bpmnEty);
        }

        private void ExecuteDeleteSfwOfActCommand(TblAct_Sfw obj)
        {
            //1544
            this.TblAct.TblAct_Sfw.Remove(obj);

            //1545
            if (obj.FldTypUseSfw == (int)ActivitySoftwareTypes.ActivitySoftware)
            {
                this.ActivitySoftwares.Remove(obj);
            }
            else if (obj.FldTypUseSfw == (int)ActivitySoftwareTypes.InputActivitySoftware)
            {
                this.InputActivitySoftwares.Remove(obj);
            }
            else if (obj.FldTypUseSfw == (int)ActivitySoftwareTypes.OutputActivitySoftware)
            {
                this.OutputActivitySoftwares.Remove(obj);
            }
        }

        void act_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FldNamAct")
            {
                if (ActChanged != null)
                {
                    ActChanged(this.TblAct);
                }
            }
        }


        #endregion

        #region ' events '

        public event Action<TblAct> ActChanged;

        #endregion
    }
}
