using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefSfwViewModel : BaseDefItemViewModel<TblSfw, int>
    {

        public DefSfwViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            ItemsCV = new System.Windows.Data.ListCollectionView(_context.TblSfws.Where(s => s.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList());

            ItemsCV.Filter = filter;

        }



        protected override void addExecute()
        {
            if (_context.TblSfws.Any(s => s.FldNamSfw.ToLower() == NamItmAdding.ToLower()
                && s.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg))
            {
                Util.ShowMessageBox(60);
                return;
            }


            var newSfw = new TblSfw() { FldNamSfw = NamItmAdding, FldCodOrg = PublicMethods.CurrentUser.FldCodOrg };

            _context.TblSfws.AddObject(newSfw);
            PublicMethods.SaveContext(_context);
            NamItmAdding = "";
            NamItmAdding2 = "";
            RaisePropertyChanged("NamItmAdding", "NamItmAdding2");
            ItemsCV.AddNewItem(newSfw);
        }

        protected override bool filter(object obj)
        {
            if (!string.IsNullOrWhiteSpace(TxtSrch))
            {
                var sfw = obj as TblSfw;

                return sfw.FldNamSfw.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
            }
            else
            {
                return true;
            }

        }

        protected override bool canAdd()
        {
            return !string.IsNullOrWhiteSpace(NamItmAdding);
        }

        public override Visibility ItmAdding2Visible
        {
            get
            {
                return Visibility.Hidden;
            }
        }

        public override string NamEty
        {
            get { return "تعریف نرم‏افزار"; }
        }

        public override string Lbl1
        {
            get { return "نام نرم‏افزار"; }
        }

        public override string Lbl2
        {
            get { return ""; }
        }

    }
}
