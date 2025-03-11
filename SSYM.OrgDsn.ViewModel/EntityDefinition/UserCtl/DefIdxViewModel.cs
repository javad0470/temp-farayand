using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefIdxViewModel : BaseDefItemViewModel<TblIdx, TblSbjMsrt>
    {
        public DefIdxViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            ItemsCV = new System.Windows.Data.ListCollectionView(_context.TblIdxes.Where(i => i.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList());
            Items2CV = new System.Windows.Data.ListCollectionView(_context.TblSbjMsrts.ToList());

            ItemsCV.Filter = filter;

        }

        protected override void addExecute()
        {

            if (_context.TblIdxes.Any(u => u.FldNamIdx.ToLower() == NamItmAdding.ToLower() && u.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg))
            {
                Util.ShowMessageBox(60);
                return;
            }



            if (this.SelectedItmAdding2 == null)
            {
                this.SelectedItmAdding2 = new TblSbjMsrt() { FldNamSbjMsrt = this.NamItmAdding2.Trim() };
                _context.TblSbjMsrts.AddObject(this.SelectedItmAdding2);
                Items2CV.AddNewItem(this.SelectedItmAdding2);
                PublicMethods.SaveContext(_context);
            }

            var newIdx = new TblIdx() { FldCodSbjMsrt = this.SelectedItmAdding2.FldCodSbjMsrt, FldNamIdx = NamItmAdding.Trim(), FldCodOrg = PublicMethods.CurrentUser.FldCodOrg };

            _context.TblIdxes.AddObject(newIdx);
            PublicMethods.SaveContext(_context);
            NamItmAdding = "";
            NamItmAdding2 = "";
            RaisePropertyChanged("NamItmAdding", "NamItmAdding2");
            ItemsCV.AddNewItem(newIdx);
        }

        protected override bool filter(object obj)
        {
            if (!string.IsNullOrWhiteSpace(TxtSrch))
            {
                var idx = obj as TblIdx;

                return idx.FldNamIdx.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }

        public override string NamEty
        {
            get { return "تعریف شاخص"; }
        }

        public override string Lbl1
        {
            get { return "نام شاخص"; }
        }

        public override string Lbl2
        {
            get { return "واحد سنجش"; }
        }

    }
}
