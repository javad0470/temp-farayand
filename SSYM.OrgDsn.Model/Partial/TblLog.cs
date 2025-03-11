using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Media;
using yWorks.yFiles.UI.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System.Data;

namespace SSYM.OrgDsn.Model
{
    public partial class TblLog
    {
        public static void CreateLog(IAllEty ety, EntityState state)
        {
            using (BPMNDBEntities ctx = new BPMNDBEntities())
            {

                string str = string.Format("{0} -- {1}", ety.Name, System.Enum.GetName(typeof(EntityState), state));

                if (str.Length > 50)
                {
                    str = str.Substring(0, 50);

                }

                TblLog log = new TblLog()
                {
                    FldActnImpEed = str,
                    FldCodTypEty = (int)ety.CodTypEty,
                    FLdCodEty = ety.CodEty,
                    FldCodUsr = PublicMethods.CurrentUser.FldCodUsr,
                    FldDteLog = DateTime.Now,
                    FldTypLog = (int)state,
                };

                ctx.TblLogs.AddObject(log);

                ctx.SaveChanges();
            }
        }


        public static void CreateLog(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            try
            {
                using (BPMNDBEntities ctx = new BPMNDBEntities())
                {
                    TblLog log = new TblLog()
                    {
                        FldActnImpEed = "",
                        FldCodUsr = PublicMethods.CurrentUser != null ? PublicMethods.CurrentUser.FldCodUsr : 0,
                        FldDteLog = DateTime.Now,
                        FldTypLog = 13,
                        FldStkTrc = ex.StackTrace != null ? ex.StackTrace : "",
                        FldDsc = string.Format(@"message:{0} 
                                            innerEx:{1}
                                            source:{2}
                                            target:{3}",
                                                ex.Message != null ? ex.Message : "",
                                                ex.InnerException != null ? ex.InnerException.Message : "",
                                                ex.Source != null ? ex.Source : "",
                                                ex.TargetSite != null ? ex.TargetSite.ToString() : ""
                                                )
                    };

                    ctx.TblLogs.AddObject(log);

                    ctx.SaveChanges();
                }

            }
            catch (Exception)
            {
            }
        }


    }
}
