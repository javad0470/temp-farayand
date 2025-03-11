using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class ItmAsnToPsnViewModel : BaseViewModel
    {

        #region ' Fields '

        TblPsn psn;
        TblOrg org;

        #endregion

        #region ' Initialaizer '

        public ItmAsnToPsnViewModel(TblPsn psn, TblOrg org)
        {
            this.psn = psn;
            this.org = org;

            /*--------------شناسایی و نمایش چارت سازمانی شخص جاری----------------*/
            TblPosPstOrg pospstorg = org.TblPosPstOrgs.SingleOrDefault(m => !m.FldCodUpl.HasValue);
            /*-----------نمایش تمام نقش های شخص در سازمان------------*/
            OrgRols = new OrgRolsViewModel(MenuViewModel.MainContext, org);


            DeleteObjCommand = new DelegateCommand<IOrgChart>(DeleteObj);
            LoadItems();
        }


        #endregion

        #region ' Properties / Commands '


        public OrgRolsViewModel OrgRols { get; set; }

        public ObservableCollection<IOrgChart> AssignedItms { get; set; }

        public DelegateCommand<IOrgChart> DeleteObjCommand { get; set; }
        #endregion

        #region ' Public Methods '

        /// <summary>
        /// این تابع یک نقش را به لیست موارد تخصیصی شخص مورد نظر در سازمان جاری می افزاید
        /// F1554
        /// </summary>
        /// <param name="rol"></param>
        public void AssignRolToPsn(TblRol rol)
        {
            TblNod nod = MenuViewModel.MainContext.TblNods.Single(m => m.FldCodTypEty == 4 && m.FldCodEty == rol.FldCodRol);
            var obj = psn.TblAgntNods.SingleOrDefault(m => m.FldCodNod == nod.FldCodNod && m.FldCodPsn == psn.FldCodPsn);
            if (obj != null)
            {
                return;
            }
            psn.TblAgntNods.Add(new TblAgntNod() { FldCodNod = nod.FldCodNod, FldCodPsn = psn.FldCodPsn });
            AssignedItms.Add(rol);

            PublicMethods.SaveContext(MenuViewModel.MainContext);
        }

        /// <summary>
        /// این تابع یک جایگاه یا سمت سازمانی را به لیست موارد تخصیصی شخص مورد نظر در سازمان جاری می افزاید
        /// F1557
        /// </summary>
        /// <param name="pospst"></param>
        public void AssignPosPstToPsn(TblPosPstOrg pospst)
        {
            TblNod nod = MenuViewModel.MainContext.TblNods.Single(m => m.FldCodTypEty == 2 && m.FldCodEty == pospst.FldCodPosPst);
            var obj = psn.TblAgntNods.SingleOrDefault(m => m.FldCodNod == nod.FldCodNod && m.FldCodPsn == psn.FldCodPsn);
            if (obj != null)
            {
                return;
            }
            psn.TblAgntNods.Add(new TblAgntNod() { FldCodNod = nod.FldCodNod, FldCodPsn = psn.FldCodPsn });
            AssignedItms.Add(pospst);

            PublicMethods.SaveContext(MenuViewModel.MainContext);
        }

        #endregion

        #region ' Private Methods '


        /// <summary>
        /// این متد نقش یا جایگاه و سمت انتخاب شده را حذف میکند
        /// </summary>
        /// <param name="obj"></param>
        private void DeleteObj(IOrgChart obj)
        {
            if (obj is TblPosPstOrg)
            {
                /*------------F1578---------------*/

                TblPosPstOrg pospst = obj as TblPosPstOrg;
                TblNod nod = MenuViewModel.MainContext.TblNods.Single(m => m.FldCodTypEty == 2 && m.FldCodEty == pospst.FldCodPosPst);
                TblAgntNod item = psn.TblAgntNods.SingleOrDefault(m => m.FldCodNod == nod.FldCodNod && m.FldCodPsn == psn.FldCodPsn);
                if (item == null)
                {
                    return;
                }
                MenuViewModel.MainContext.DeleteObject(item);
                AssignedItms.Remove(AssignedItms.Single(m => (m is TblPosPstOrg) && (m as TblPosPstOrg).FldCodPosPst == pospst.FldCodPosPst));

            }
            else // tblrol
            {
                /*------------F1569---------------*/

                TblRol rol = obj as TblRol;
                TblNod nod = MenuViewModel.MainContext.TblNods.Single(m => m.FldCodTypEty == 4 && m.FldCodEty == rol.FldCodRol);
                TblAgntNod item = psn.TblAgntNods.SingleOrDefault(m => m.FldCodNod == nod.FldCodNod && m.FldCodPsn == psn.FldCodPsn);
                if (item == null)
                {
                    return;
                }
                MenuViewModel.MainContext.DeleteObject(item);
                AssignedItms.Remove(AssignedItms.Single(m => (m is TblRol) && (m as TblRol).FldCodRol == rol.FldCodRol));
            }

            PublicMethods.SaveContext(MenuViewModel.MainContext);
            
        }

        /// <summary>
        /// F1586
        /// این متد تمام نقشها و جایگاهها و سمتهای شخص جاری را نمایش میدهد
        /// </summary>
        private void LoadItems()
        {
            List<IOrgChart> items = new List<IOrgChart>();
            foreach (var item in psn.TblAgntNods)
            {
                TblPosPstOrg pospst = MenuViewModel.MainContext.TblPosPstOrgs.SingleOrDefault(m => m.FldCodPosPst == item.TblNod.FldCodEty);
                TblRol rol = MenuViewModel.MainContext.TblRols.SingleOrDefault(m => m.FldCodRol == item.TblNod.FldCodEty);
                if (pospst != null)
                {
                    items.Add(pospst);
                }
                else if (rol != null)
                {
                    items.Add(rol);
                }
            }
            AssignedItms = new ObservableCollection<IOrgChart>(items);
        }

        #endregion

    }
}
