using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Admin;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using SSYM.OrgDsn.Model.Base;
using System.Windows;

namespace SSYM.OrgDsn.ViewModel
{
    public class MenuViewModel : BaseViewModel, IViewModelBase
    {
        //public GenericInteractionRequest<PopupDataObject> ModalPopup { get; private set; }

        bool isActPrgRingActivated;

        //string connectionStringTest = "DBOrgDsnTest";

        //string connectionStringDemo = "DBOrgDsnDemo";

        string connectionStringTest = @"metadata=res://*/DbModel.csdl|res://*/DbModel.ssdl|res://*/DbModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=group1-pc;Initial Catalog=DBOrgDsnTest;User id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework&quot;";

        string connectionStringDemo = @"metadata=res://*/DbModel.csdl|res://*/DbModel.ssdl|res://*/DbModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=group1-pc;Initial Catalog=DBOrgDsnDemo;User id=sa;password=123;multipleactiveresultsets=True;application name=EntityFramework&quot;";

        public MenuViewModel()
        {
            Initialization();
        }

        private void Initialization()
        {

            //ModalPopup = new GenericInteractionRequest<PopupDataObject>();

            ChangeSelectedObj = new DelegateCommand<string>(ExecuteChangeSelectedObj);

            MainMenu = this;
            
            //MainWindowViewModel.MainContext = MainContext;

            CloseCurrentViewCommand = new DelegateCommand(closeCurrentViewExecute);

            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    if (!Validation.SigCheck())
            //    {
            //        Util.ShowMessageBox(68, "SigCheck Failed");
            //    }
            //}
        }



        #region Access

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterPrs
        {
            get
            {
                return this.Usr.AcsUsr["EnterPrs"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterAct
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "Act", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["EnterAct"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterRpot
        {
            get
            {
                return this.Usr.AcsUsr["EnterRpot"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterDson
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "Dson", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["EnterDson"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterEblUsr
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "EblUsr", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["EnterEblUsr"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterPosPst
        {
            get
            {
                var orgs = PublicMethods.GetOrgForPsnAgntOfThemInCurrOrgAndSubOrg(PublicMethods.CurrentUser.TblPsn);

                return orgs.Any(o =>
                    {
                        PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22090(o.FldCodOrg, null, "Enter", namTypEtyMjr: "PosPst", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());
                        return this.Usr.AcsUsr["EnterPosPst"];
                    }
                    );

                //PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "PosPst", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                //return this.Usr.AcsUsr["EnterPosPst"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterlvlAcs
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "lvlAcs", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["EnterlvlAcs"];
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public bool Acs_EnterOrg
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "Enter", namTypEtyMjr: "Org", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["EnterOrg"];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Acs_ViewDson
        {
            get
            {
                PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(null, "View", namTypEtyMjr: "Dson", nodMomEty: PublicMethods.CurrentUser.NodAgntEedByUsr.ToArray());

                return this.Usr.AcsUsr["ViewDson"];
            }
        }





        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TblUsr Usr
        {
            get
            {
                return PublicMethods.CurrentUser;
            }
        }

        bool isTestDBSelected = true;

        public bool IsTestDBSelected
        {
            get
            {
                return isTestDBSelected;
            }

            set
            {
                isTestDBSelected = value;

                if (value)
                {
                    updateConfigFile(connectionStringTest);
                    //to refresh connection string each time else it will use previous connection string
                    ConfigurationManager.RefreshSection("connectionStrings");
                }

                else
                {
                    updateConfigFile(connectionStringDemo);
                    //to refresh connection string each time else it will use previous connection string
                    ConfigurationManager.RefreshSection("connectionStrings");
                }

                Initialization();

                RaisePropertyChanged("IsTestDBSelected");
            }
        }

        public void updateConfigFile(string conNew)
        {
            ////updating config file
            //XmlDocument XmlDoc = new XmlDocument();
            ////Loading the Config file
            //XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            //foreach (var xElement in XmlDoc.DocumentElement)
            //{
            //    if (xElement.GetType() == typeof(XmlElement))
            //    {
            //        XmlElement x = (XmlElement)xElement;
            //        if (x.Name == "connectionStrings")
            //        {
            //            //x.FirstChild.Value = x.FirstChild.Value.ToString().Replace(conOld, conNew);

            //            x.FirstChild.Attributes[1].Value = conNew;
            //        }
            //    }
            //}
            ////writing the connection string in config file
            //XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }

        public MessageBoxResult RaisePopup(PopupDataObject obj, Action<PopupDataObject> callback, Action cancelCallback)
        {
            //ModalPopup.Raise(obj, callback, cancelCallback);

            return this.View.RaisePopup(obj, callback, cancelCallback);
        }

        public MessageBoxResult ShowPopup(PopupViewModel dataContext)
        {
            return this.View.ShowPopupWindow(dataContext);
        }


        public IView View { get; set; }

        /// <summary>
        /// this context is main context of application
        /// </summary>
        public static BPMNDBEntities MainContext;

        BaseViewModel selectedObj;

        public BaseViewModel SelectedObj
        {
            get { return selectedObj; }
            set
            {
                selectedObj = value;
                RaisePropertyChanged("SelectedObj");
            }
        }

        /// <summary>
        /// پروگرس بار لود شدن فعالیت
        /// </summary>
        public bool IsActPrgRingActivated
        {
            get { return isActPrgRingActivated; }
            set
            {
                isActPrgRingActivated = value;

                RaisePropertyChanged("IsActPrgRingActivated");
            }
        }

        public ICommand ChangeSelectedObj { get; set; }

        public ICommand CloseCurrentViewCommand { get; set; }

        public static MenuViewModel MainMenu { get; set; }

        private void ExecuteChangeSelectedObj(string obj)
        {
            IsActPrgRingActivated = true;

            if (obj == "DefAct")
            {
                this.SelectedObj = new ViewModel.ActivityDefinition.Main.ActDefViewModel();
            }

            else if (obj == "DefOrg")
            {
                this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefOrg();
            }

            else if (obj == "DefPosPst")
            {
                this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefPosPstViewModel();
            }

            else if (obj == "DefPsn")
            {
                this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefPsnViewModel();
            }

            //else if (obj == "DefPsnOutsideOrg")
            //{
            //    this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefPsnOutsideOrgViewModel();
            //}

            //else if (obj == "DefRolIsgOrg")
            //{
            //    this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefRolIsdOrgViewModel();
            //}

            else if (obj == "DefRol")
            {
                this.SelectedObj = new ViewModel.EntityDefinition.UserCtl.DefRolViewModel();
            }

            else if (obj == "DisPrs")
            {
                this.SelectedObj = new ViewModel.Process.UserCtl.DisPrsViewModel() { DtlVisible = true };
            }
            else if (obj == "DisDson")
            {
                this.SelectedObj = new ViewModel.Dson.DsonListViewModel();
            }
            else if (obj == "Report")
            {
                this.SelectedObj = new ViewModel.Report.MainReportMenuViewModel();
            }

            else if (obj == "UserSetting")
            {
                this.SelectedObj = new ViewModel.UserSetting.UsrSettingViewModel();
            }

            else if (obj == "ActiveUsr")
            {
                this.SelectedObj = new ViewModel.Admin.ActiveUsrViewModel();
            }

            else if (obj == "DefLvlAcs")
            {
                this.SelectedObj = new ViewModel.Admin.DefLvlAcsViewModel();
            }

            else if (obj == "DefItms")
            {
                this.SelectedObj = new SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.DefItmsViewModel(new BPMNDBEntities());
            }

            this.SelectedObj.Parent = this;
            
            IsActPrgRingActivated = false;
        }

        public void SaveChanges()
        {
            (this.SelectedObj as IViewModel).SaveContext();
        }

        private void closeCurrentViewExecute()
        {
            if (this.SelectedObj != null && (this.SelectedObj as IViewModel).ConfirmAndClose())
            {
                if (this.View != null)
                {
                    this.View.HideCurrentView();
                    this.SelectedObj = null;
                    GC.Collect();
                }
            }
        }


        public void RaiseProperties()
        {
            RaisePropertyChanged("IsServerVrsn", "IsClientVrsn", "IsCompeleteVrsn", "IsNotCompeleteVrsn", "IsLocalPathExeVrsnSrv", "IsInslVrsnClntOnnSysCnt");
        }

        public void SaveContext()
        {
            throw new NotImplementedException();
        }

        public bool IsServerVrsn
        {
            get
            {
                return Util.VrsnTyp == TypVrsn.SERVER || Util.VrsnTyp == TypVrsn.COMPELETE;
            }
        }

        public bool IsClientVrsn
        {
            get
            {
                return Util.VrsnTyp == TypVrsn.CLIENT || Util.VrsnTyp == TypVrsn.COMPELETE;
            }
        }

        public bool IsCompeleteVrsn
        {
            get
            {
                return Util.VrsnTyp == TypVrsn.COMPELETE;
            }
        }

        public bool IsNotCompeleteVrsn
        {
            get
            {
                return Util.VrsnTyp != TypVrsn.COMPELETE;
            }
        }


        /// <summary>
        /// مسیر فایل اجرایی در صورتی که نسخه سرور باشد
        /// </summary>
        public bool IsLocalPathExeVrsnSrv { get { return Util.IsLocalPathExeVrsnSrv; } }


        /// <summary>
        /// آیا نسخه کلاینت روی سیستم جاری نصب شده است؟
        /// </summary>
        public bool IsInslVrsnClntOnnSysCnt { get { return Util.IsInslVrsnClntOnnSysCnt; } }


        private bool _sett;
        public bool Sett
        {
            get { return _sett; }
            set
            {
                _sett = value;
                RaisePropertyChanged("Sett");
            }
        }
        public void AdminInformation()
        {
            AdminInfViewModel adminInfViewModel=new AdminInfViewModel();

            if (!adminInfViewModel.ValidAdminInfo() && Util.ShowMessageBox(80) == MessageBoxResult.Yes)
            {
                Util.ShowPopup(adminInfViewModel);
                if (adminInfViewModel.Result == PopupResult.OK)
                {
                    Sett = true;
                }
                else
                {
                    Sett = false;
                }
            }
            else if (!adminInfViewModel.ValidAdminInfo())
            {
                Sett = false;
            }
        }

    }
}
