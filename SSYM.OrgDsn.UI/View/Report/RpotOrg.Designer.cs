namespace SSYM.OrgDsn.UI.View.Report
{
    partial class RpotOrg
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.fldNamOrgCaptionTextBox = new Telerik.Reporting.TextBox();
            this.fldTelOrgCaptionTextBox = new Telerik.Reporting.TextBox();
            this.fldAdrsOrgCaptionTextBox = new Telerik.Reporting.TextBox();
            this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.reportFooter = new Telerik.Reporting.ReportFooterSection();
            this.detail = new Telerik.Reporting.DetailSection();
            this.fldTelOrgDataTextBox = new Telerik.Reporting.TextBox();
            this.fldAdrsOrgDataTextBox = new Telerik.Reporting.TextBox();
            this.fldNamOrgDataTextBox = new Telerik.Reporting.TextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.fldNamOrgCaptionTextBox,
            this.fldTelOrgCaptionTextBox,
            this.fldAdrsOrgCaptionTextBox,
            this.textBox1});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // fldNamOrgCaptionTextBox
            // 
            this.fldNamOrgCaptionTextBox.CanGrow = true;
            this.fldNamOrgCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.fldNamOrgCaptionTextBox.Name = "fldNamOrgCaptionTextBox";
            this.fldNamOrgCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldNamOrgCaptionTextBox.StyleName = "Caption";
            this.fldNamOrgCaptionTextBox.Value = "آدرس";
            // 
            // fldTelOrgCaptionTextBox
            // 
            this.fldTelOrgCaptionTextBox.CanGrow = true;
            this.fldTelOrgCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3066668510437012D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.fldTelOrgCaptionTextBox.Name = "fldTelOrgCaptionTextBox";
            this.fldTelOrgCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.573958158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldTelOrgCaptionTextBox.StyleName = "Caption";
            this.fldTelOrgCaptionTextBox.Value = "تلفن";
            // 
            // fldAdrsOrgCaptionTextBox
            // 
            this.fldAdrsOrgCaptionTextBox.CanGrow = true;
            this.fldAdrsOrgCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.560416221618652D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.fldAdrsOrgCaptionTextBox.Name = "fldAdrsOrgCaptionTextBox";
            this.fldAdrsOrgCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldAdrsOrgCaptionTextBox.StyleName = "Caption";
            this.fldAdrsOrgCaptionTextBox.Value = "سازمان";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataMember = "GetActs";
            this.objectDataSource1.DataSource = typeof(SSYM.OrgDsn.ViewModel.Report.RpotOrgViewModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.708333015441895D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "=\'صفحه\' + PageNumber + \' از \' + PageCount";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8277082443237305D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(3.7941668033599854D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox8,
            this.textBox7,
            this.textBox6,
            this.textBox5,
            this.textBox4,
            this.textBox3,
            this.titleTextBox});
            this.reportHeader.Name = "reportHeader";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.15625D), Telerik.Reporting.Drawing.Unit.Inch(0.74573498964309692D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3927073478698731D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox8.Style.Font.Name = "B Nazanin";
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "=Parameters.reportDate.Value";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.65625D), Telerik.Reporting.Drawing.Unit.Inch(1.0478183031082153D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox7.Style.Font.Name = "B Nazanin";
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "=Parameters.ownerName.Value";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.65625D), Telerik.Reporting.Drawing.Unit.Inch(0.74573498964309692D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox6.Style.Font.Name = "B Nazanin";
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "=Parameters.orgName.Value";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.75D), Telerik.Reporting.Drawing.Unit.Inch(0.74573498964309692D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.31354066729545593D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox5.Style.Font.Name = "B Nazanin";
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "تاریخ";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.8541665077209473D), Telerik.Reporting.Drawing.Unit.Inch(1.0478183031082153D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox4.Style.Font.Name = "B Nazanin";
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "مجری تهیه کننده";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.8541665077209473D), Telerik.Reporting.Drawing.Unit.Inch(0.74573498964309692D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox3.Style.Font.Name = "B Nazanin";
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "سازمان";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Bindings.Add(new Telerik.Reporting.Binding("Value", "=Parameters.title.Value"));
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.2122917175292969D), Telerik.Reporting.Drawing.Unit.Cm(0.094166792929172516D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.5032143592834473D), Telerik.Reporting.Drawing.Unit.Cm(2.0264582633972168D));
            this.titleTextBox.Style.Font.Name = "B Nazanin";
            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "";
            // 
            // reportFooter
            // 
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.fldTelOrgDataTextBox,
            this.fldAdrsOrgDataTextBox,
            this.fldNamOrgDataTextBox,
            this.textBox2});
            this.detail.Name = "detail";
            // 
            // fldTelOrgDataTextBox
            // 
            this.fldTelOrgDataTextBox.CanGrow = true;
            this.fldTelOrgDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3066668510437012D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.fldTelOrgDataTextBox.Name = "fldTelOrgDataTextBox";
            this.fldTelOrgDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.573958158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldTelOrgDataTextBox.StyleName = "Data";
            this.fldTelOrgDataTextBox.Value = "=Fields.FldTelOrg";
            // 
            // fldAdrsOrgDataTextBox
            // 
            this.fldAdrsOrgDataTextBox.CanGrow = true;
            this.fldAdrsOrgDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.10563348978757858D), Telerik.Reporting.Drawing.Unit.Cm(0.10573320835828781D));
            this.fldAdrsOrgDataTextBox.Name = "fldAdrsOrgDataTextBox";
            this.fldAdrsOrgDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldAdrsOrgDataTextBox.StyleName = "Data";
            this.fldAdrsOrgDataTextBox.Value = "=Fields.FldAdrsOrg";
            // 
            // fldNamOrgDataTextBox
            // 
            this.fldNamOrgDataTextBox.CanGrow = true;
            this.fldNamOrgDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.507699966430664D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.fldNamOrgDataTextBox.Name = "fldNamOrgDataTextBox";
            this.fldNamOrgDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldNamOrgDataTextBox.StyleName = "Data";
            this.fldNamOrgDataTextBox.Value = "=Fields.FldNamOrg";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.573958158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "سازمان مافوق";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.9335417747497559D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.573958158493042D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "=Fields.NamParent";
            // 
            // RpotOrg
            // 
            this.DataSource = this.objectDataSource1;
            group1.GroupFooter = this.labelsGroupFooterSection;
            group1.GroupHeader = this.labelsGroupHeaderSection;
            group1.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.reportFooter,
            this.detail});
            this.Name = "RpotOrg";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D), Telerik.Reporting.Drawing.Unit.Mm(25.399999618530273D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(58)))), ((int)(((byte)(112)))));
            styleRule2.Style.Color = System.Drawing.Color.White;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.Black;
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.Black;
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(15.814167022705078D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.ObjectDataSource objectDataSource1;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox fldNamOrgCaptionTextBox;
        private Telerik.Reporting.TextBox fldTelOrgCaptionTextBox;
        private Telerik.Reporting.TextBox fldAdrsOrgCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox fldTelOrgDataTextBox;
        private Telerik.Reporting.TextBox fldAdrsOrgDataTextBox;
        private Telerik.Reporting.TextBox fldNamOrgDataTextBox;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;

    }
}