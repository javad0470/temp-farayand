using SSYM.OrgDsn.ViewModel.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Reporting;
using Telerik.ReportViewer.Wpf;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.UI.View.Process.UserCtl;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Converter;

namespace SSYM.OrgDsn.UI.View.Report
{
    /// <summary>
    /// Interaction logic for ucRpotMainMenu.xaml
    /// </summary>
    public partial class ucRpotMainMenu : UserControl
    {
        public ucRpotMainMenu()
        {
            InitializeComponent();

            this.DataContextChanged += ucRpotMainMenu_DataContextChanged;
        }

        void ucRpotMainMenu_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                (this.DataContext as MainReportMenuViewModel).ReportChanged += ucRpotMainMenu_ReportChanged;
            }

            this.DataContextChanged -= ucRpotMainMenu_DataContextChanged;
        }


        private void ucRpotMainMenu_ReportChanged(IReport selectedReport, System.Collections.IEnumerable reportResult)
        {
            Telerik.Reporting.Report r = null;
            //r.ReportParameters.First(m => m.Name == "depOrg").Value = "abc";//vm.DependentOrgs;
            //r.ReportParameters.First(m => m.Name == "namOrg").Value = "133";//vm.OrgName;
            //r.ReportParameters.First(m => m.Name == "namCreator").Value = "qwe";//vm.ReportCreatorNod;
            //r.ReportParameters.First(m => m.Name == "dateRpot").Value = DateTime.Now.ToString(); //vm.ReportDate;
            List<Tuple<IWayAwrIfrm, TblNod, string>> lst = new List<Tuple<IWayAwrIfrm, TblNod, string>>();

            if (selectedReport is RpotActViewModel)
            {
                r = new RpotAct();
            }
            else if (selectedReport is RpotOrgViewModel)
            {
                r = new RpotOrg();
            }
            else if (selectedReport is RpotDsonViewModel)
            {
                if (reportResult == null)
                {
                    Menu.RaisePopupStatic(new ViewModel.ActivityDefinition.Popup.PopupDataObject("لطفا حداقل یک شرط انتخاب کنید.", "خطا", ViewModel.ActivityDefinition.Popup.MessageBoxType.Error, null)
                    , (c => { }), null);
                    return;
                }
                r = new RpotDson();

                DsonDescConverter dc = new DsonDescConverter();

                foreach (var item in (reportResult as IQueryable<Tuple<IWayAwrIfrm, TblNod>>))
                {
                    Tuple<IWayAwrIfrm, TblNod, string> itm = new Tuple<IWayAwrIfrm, TblNod, string>(item.Item1, item.Item2, dc.Convert(item.Item1, null, null, null).ToString());
                    lst.Add(itm);
                }

                lst = lst.OrderBy(m => m.Item2.FldCodNod).ToList();

            }
            else if (selectedReport is RpotErrViewModel)
            {
                r = new RpotErr();
            }
            else if (selectedReport is RpotIdxViewModel)
            {
                r = new RpotIdx();
            }
            else if (selectedReport is RpotInOutViewModel)
            {
                r = new RpotInOut();
            }
            else if (selectedReport is RpotMsrtUnitViewModel)
            {
                r = new RpotUntMsrt();
            }
            else if (selectedReport is RpotNewsViewModel)
            {
                r = new RpotNews();
            }
            else if (selectedReport is RpotOutOrgViewModel)
            {
            }
            else if (selectedReport is RpotPosPstViewModel)
            {
                r = new RpotPosPst();
            }
            else if (selectedReport is RpotPrsViewModel)
            {
                r = new RpotPrs();
            }
            else if (selectedReport is RpotPsnInViewModel)
            {
                r = new RpotPsnIn();
            }
            else if (selectedReport is RpotPsnOutViewModel)
            {
                r = new RpotPsnOut();
            }
            else if (selectedReport is RpotRolInViewModel)
            {
                r = new RpotRolIn();
            }
            else if (selectedReport is RpotRolOutViewModel)
            {
                r = new RpotRolOut();
            }
            else if (selectedReport is RpotSoftViewModel)
            {
                r = new RpotSfw();
            }

            r.ReportParameters.Add("title", ReportParameterType.String, selectedReport.ReportTitle);

            r.ReportParameters.Add("orgName", ReportParameterType.String, PublicMethods.CurrentUser.TblOrg.FldNamOrg);

            r.ReportParameters.Add("reportDate", ReportParameterType.String, DateTime.Now.ToString());

            r.ReportParameters.Add("ownerName", ReportParameterType.String, PublicMethods.CurrentUser.FldNamUsr);

            r.DataSource = reportResult;

            if (selectedReport is RpotDsonViewModel)
            {
                r.DataSource = lst;
            }

            Telerik.Reporting.InstanceReportSource irs = new Telerik.Reporting.InstanceReportSource();
            irs.ReportDocument = r;

            ReportViewer rp1 = new ReportViewer();
            rp1.ReportSource = irs;


            Window wnd1 = new Window();
            wnd1.Content = rp1;
            wnd1.Show();
        }

        private void report_Clicked(object sender, RoutedEventArgs e)
        {
            ViewModel.Process.UserCtl.DisPrsViewModel vm = new ViewModel.Process.UserCtl.DisPrsViewModel() { DtlVisible = false, FlagVisible = false };

            DisPrs prsView = new DisPrs() { DataContext = vm };

            PrsReport repWindow = new PrsReport();

            repWindow.cntCtrl.Content = prsView;

            repWindow.ShowDialog();
        }
    }
}
