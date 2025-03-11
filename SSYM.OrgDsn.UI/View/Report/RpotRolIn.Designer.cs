namespace SSYM.OrgDsn.UI.View.Report
{
    partial class RpotRolIn
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.fldTtlRolCaptionTextBox = new Telerik.Reporting.TextBox();
            this.posPstsCaptionTextBox = new Telerik.Reporting.TextBox();
            this.depOrgCaptionTextBox = new Telerik.Reporting.TextBox();
            this.objectDataSource1 = new Telerik.Reporting.ObjectDataSource();
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
            this.list1 = new Telerik.Reporting.List();
            this.panel1 = new Telerik.Reporting.Panel();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.fldTtlRolDataTextBox = new Telerik.Reporting.TextBox();
            this.list2 = new Telerik.Reporting.List();
            this.panel2 = new Telerik.Reporting.Panel();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.reportNameTextBox = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.1058331727981567D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.fldTtlRolCaptionTextBox,
            this.posPstsCaptionTextBox,
            this.depOrgCaptionTextBox});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // fldTtlRolCaptionTextBox
            // 
            this.fldTtlRolCaptionTextBox.CanGrow = true;
            this.fldTtlRolCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.fldTtlRolCaptionTextBox.Name = "fldTtlRolCaptionTextBox";
            this.fldTtlRolCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldTtlRolCaptionTextBox.StyleName = "Caption";
            this.fldTtlRolCaptionTextBox.Value = "سازمان";
            // 
            // posPstsCaptionTextBox
            // 
            this.posPstsCaptionTextBox.CanGrow = true;
            this.posPstsCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.3340001106262207D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.posPstsCaptionTextBox.Name = "posPstsCaptionTextBox";
            this.posPstsCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2008333206176758D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.posPstsCaptionTextBox.StyleName = "Caption";
            this.posPstsCaptionTextBox.Value = "جایگاه و سمت";
            // 
            // depOrgCaptionTextBox
            // 
            this.depOrgCaptionTextBox.CanGrow = true;
            this.depOrgCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.668000221252441D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.depOrgCaptionTextBox.Name = "depOrgCaptionTextBox";
            this.depOrgCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0403332710266113D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.depOrgCaptionTextBox.StyleName = "Caption";
            this.depOrgCaptionTextBox.Value = "نام نقش";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataMember = "getrols";
            this.objectDataSource1.DataSource = typeof(SSYM.OrgDsn.ViewModel.Report.RpotRolInViewModel);
            this.objectDataSource1.Name = "objectDataSource1";
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
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(3.7201669216156006D);
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
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.099999986588954926D), Telerik.Reporting.Drawing.Unit.Inch(0.72916668653488159D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.3927073478698731D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox8.Style.Font.Name = "B Nazanin";
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "=Parameters.reportDate.Value";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.8000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(1.03125D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox7.Style.Font.Name = "B Nazanin";
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "=Parameters.ownerName.Value";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.8000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.72916668653488159D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox6.Style.Font.Name = "B Nazanin";
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Value = "=Parameters.orgName.Value";
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.5999997854232788D), Telerik.Reporting.Drawing.Unit.Inch(0.72916668653488159D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.31354066729545593D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox5.Style.Font.Name = "B Nazanin";
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "تاریخ";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0083355903625488D), Telerik.Reporting.Drawing.Unit.Inch(1.03125D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox4.Style.Font.Name = "B Nazanin";
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.Value = "مجری تهیه کننده";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0083355903625488D), Telerik.Reporting.Drawing.Unit.Inch(0.72916668653488159D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            this.textBox3.Style.Font.Name = "B Nazanin";
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "سازمان";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Bindings.Add(new Telerik.Reporting.Binding("Value", "=Parameters.title.Value"));
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.0799999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.34395831823349D));
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
            this.reportFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(2.6552085876464844D);
            this.reportFooter.Name = "reportFooter";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(4.7754173278808594D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.list1,
            this.fldTtlRolDataTextBox,
            this.list2});
            this.detail.Name = "detail";
            // 
            // list1
            // 
            this.list1.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "=Fields.PosPsts"));
            this.list1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.0799999237060547D)));
            this.list1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.0529166460037231D)));
            this.list1.Body.SetCellContent(0, 0, this.panel1);
            tableGroup1.Name = "ColumnGroup";
            this.list1.ColumnGroups.Add(tableGroup1);
            this.list1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel1});
            this.list1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.306666374206543D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.list1.Name = "list1";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup2.Name = "DetailGroup";
            this.list1.RowGroups.Add(tableGroup2);
            this.list1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0799999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.0529166460037231D));
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1});
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0799999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.0529166460037231D));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.02733360230922699D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0799999237060547D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.StyleName = "Data";
            this.textBox1.Value = "=Fields.FldNamPosPst";
            // 
            // fldTtlRolDataTextBox
            // 
            this.fldTtlRolDataTextBox.CanGrow = true;
            this.fldTtlRolDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.668000221252441D), Telerik.Reporting.Drawing.Unit.Cm(0.0530167892575264D));
            this.fldTtlRolDataTextBox.Name = "fldTtlRolDataTextBox";
            this.fldTtlRolDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0403332710266113D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.fldTtlRolDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.fldTtlRolDataTextBox.StyleName = "Data";
            this.fldTtlRolDataTextBox.Value = "=Fields.FldTtlRol";
            // 
            // list2
            // 
            this.list2.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "=Fields.DepOrg"));
            this.list2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.2535495758056641D)));
            this.list2.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.0002001523971558D)));
            this.list2.Body.SetCellContent(0, 0, this.panel2);
            tableGroup3.Name = "ColumnGroup";
            this.list2.ColumnGroups.Add(tableGroup3);
            this.list2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.panel2});
            this.list2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052816543728113174D));
            this.list2.Name = "list2";
            tableGroup4.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup4.Name = "DetailGroup";
            this.list2.RowGroups.Add(tableGroup4);
            this.list2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2535495758056641D), Telerik.Reporting.Drawing.Unit.Cm(1.0002001523971558D));
            // 
            // panel2
            // 
            this.panel2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2});
            this.panel2.Name = "panel2";
            this.panel2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2535495758056641D), Telerik.Reporting.Drawing.Unit.Cm(1.0002001523971558D));
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.1208336278796196D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.0799994468688965D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "=Fields.FldNamOrg";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(1.2539998292922974D);
            this.pageHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.reportNameTextBox});
            this.pageHeader.Name = "pageHeader";
            // 
            // reportNameTextBox
            // 
            this.reportNameTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.reportNameTextBox.Name = "reportNameTextBox";
            this.reportNameTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.708333015441895D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.reportNameTextBox.StyleName = "PageInfo";
            this.reportNameTextBox.Value = "=\'صفحه\' + PageNumber + \' از \' + PageCount";
            // 
            // RpotRolIn
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
            this.Name = "RpotRolIn";
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(15.769172668457031D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.ObjectDataSource objectDataSource1;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox fldTtlRolCaptionTextBox;
        private Telerik.Reporting.TextBox posPstsCaptionTextBox;
        private Telerik.Reporting.TextBox depOrgCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.ReportFooterSection reportFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox fldTtlRolDataTextBox;
        private Telerik.Reporting.List list1;
        private Telerik.Reporting.Panel panel1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.List list2;
        private Telerik.Reporting.Panel panel2;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox reportNameTextBox;
        private Telerik.Reporting.TextBox titleTextBox;

    }
}