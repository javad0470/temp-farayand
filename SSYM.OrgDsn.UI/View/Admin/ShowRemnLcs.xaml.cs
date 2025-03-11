using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel;

namespace SSYM.OrgDsn.UI.View.Admin
{
    /// <summary>
    /// Interaction logic for ShowRemnLcs.xaml
    /// </summary>
    public partial class ShowRemnLcs
    {
        public ShowRemnLcs()
        {
            using (var ctx = new BPMNDBEntities())
            {
                InitializeComponent();
                object max = new
                {
                    MaxTnoPosPst = Util.LcsSfw.MaxTnoPosPst == -1 ? "نامحدود" : Util.LcsSfw.MaxTnoPosPst.ToString(),
                    NamOrg = Util.LcsSfw.NamOrg,
                    TnoAct = Util.LcsSfw.TnoAct == -1 ? "نامحدود" : Util.LcsSfw.TnoAct.ToString(),
                    TnoNod = Util.LcsSfw.TnoNod == -1 ? "نامحدود" : Util.LcsSfw.TnoNod.ToString(),
                    TnoOrgSub = Util.LcsSfw.TnoOrgSub == -1 ? "نامحدود" : Util.LcsSfw.TnoOrgSub.ToString(),
                    TnoPrs = Util.LcsSfw.TnoPrs == -1 ? "نامحدود" : Util.LcsSfw.TnoPrs.ToString(),
                    TnoUsr = Util.LcsSfw.TnoUsr == -1 ? "نامحدود" : Util.LcsSfw.TnoUsr.ToString()
                };
                var ActsNo = ctx.TblActs.LongCount(a => a.FldActUspf != true);
                var OrgsNo = ctx.TblOrgs.LongCount(a => a.FldCodUpl != null && a.FldCodOrg != 1);
                var posPstNo = ctx.TblPosPstOrgs.LongCount();
                object used = new
                {
                    MaxTnoPosPst = posPstNo,
                    TnoOrgSub = OrgsNo,
                    TnoAct = ActsNo,
                    TnoNod = "غیر قابل دسترس",
                    TnoPrs = "غیر قابل دسترس",
                    TnoUsr = "غیر قابل دسترس",
                };
                object remn = new
                {
                    MaxTnoPosPst =
                        Util.LcsSfw.MaxTnoPosPst == -1 ? "نامحدود" : (Util.LcsSfw.MaxTnoPosPst - posPstNo).ToString(),
                    TnoOrgSub = Util.LcsSfw.TnoOrgSub == -1 ? "نامحدود" : (Util.LcsSfw.TnoOrgSub - OrgsNo).ToString(),
                    TnoAct = Util.LcsSfw.TnoAct == -1 ? "نامحدود" : (Util.LcsSfw.TnoAct - ActsNo).ToString(),
                    TnoNod = "نامحدود",
                    TnoPrs = "نامحدود",
                    TnoUsr = "نامحدود",
                };
                object obj = new
                {
                    max,
                    used,
                    remn
                };
                grdNam.DataContext = max;
                grdlicenseInfo.DataContext = obj;

            }
        }
    }
}
