using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefUntMsrtViewModel : BaseDefItemViewModel<TblUntMsrt, TblSbjMsrt>
    {

        public DefUntMsrtViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            ItemsCV = new System.Windows.Data.ListCollectionView(_context.TblUntMsrts.ToList());
            Items2CV = new System.Windows.Data.ListCollectionView(_context.TblSbjMsrts.ToList());

            ItemsCV.Filter = filter;

        }


        protected override void addExecute()
        {
            if (_context.TblUntMsrts.Any(u => u.FldNamUntMsrt.ToLower() == NamItmAdding.ToLower()))
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

            var newUntMsrt = new TblUntMsrt() { FldCodSbjMsrt = this.SelectedItmAdding2.FldCodSbjMsrt, FldNamUntMsrt = NamItmAdding.Trim() };

            _context.TblUntMsrts.AddObject(newUntMsrt);
            PublicMethods.SaveContext(_context);
            NamItmAdding = "";
            NamItmAdding2 = "";
            RaisePropertyChanged("NamItmAdding", "NamItmAdding2");
            ItemsCV.AddNewItem(newUntMsrt);
        }

        protected override bool filter(object obj)
        {
            if (!string.IsNullOrWhiteSpace(TxtSrch))
            {
                var untMsrt = obj as TblUntMsrt;

                return untMsrt.FldNamUntMsrt.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }

        public override string NamEty
        {
            get { return "تعریف واحد سنجش"; }
        }

        public override string Lbl1
        {
            get { return "نام واحد سنجش"; }
        }

        public override string Lbl2
        {
            get { return "موضوع سنجش"; }
        }

    }
}
