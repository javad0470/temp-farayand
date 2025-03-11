using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{
    public class DefErrViewModel : BaseDefItemViewModel<TblEror, TblTypEror>
    {

        public DefErrViewModel(BPMNDBEntities ctx)
            : base(ctx)
        {
            Items2CV = new System.Windows.Data.ListCollectionView(_context.TblTypErors.ToList());
            ItemsCV = new System.Windows.Data.ListCollectionView(_context.TblErors.ToList());

            ItemsCV.Filter = filter;

        }

        protected override void addExecute()
        {

            if (_context.TblErors.Any(e => e.FldNamEror.ToLower() == NamItmAdding.ToLower()))
            {
                Util.ShowMessageBox(60);
                return;
            }

            if (this.SelectedItmAdding2 == null)
            {
                this.SelectedItmAdding2 = new TblTypEror() { FldTtlTypEror = this.NamItmAdding2.Trim() };
                _context.TblTypErors.AddObject(this.SelectedItmAdding2);
                Items2CV.AddNewItem(this.SelectedItmAdding2);
                PublicMethods.SaveContext(_context);
            }

            var newError = new TblEror() { FldCodTypEror = this.SelectedItmAdding2.FldCodTypEror, FldNamEror = NamItmAdding };

            _context.TblErors.AddObject(newError);
            PublicMethods.SaveContext(_context);
            ItemsCV.AddNewItem(newError);
            NamItmAdding = "";
            NamItmAdding2 = "";
            RaisePropertyChanged("NamItmAdding", "NamItmAdding2");
        }

        protected override void deleteExecute(TblEror obj)
        {
            if (obj == null)
            {
                return;
            }

            if (obj.TblEvtRsts.Count > 0)
            {
                Util.ShowMessageBox(32, "این خطا");
                return;
            }

            base.deleteExecute(obj);
        }

        protected override bool filter(object obj)
        {
            if (!string.IsNullOrWhiteSpace(TxtSrch))
            {
                var eror = obj as TblEror;

                return eror.FldNamEror.Trim().ToLower().Contains(TxtSrch.Trim().ToLower());
            }
            else
            {
                return true;
            }
        }



        public override string NamEty
        {
            get { return "تعریف خطا"; }
        }

        public override string Lbl1
        {
            get { return "نام خطا"; }
        }

        public override string Lbl2
        {
            get { return "نوع خطا"; }
        }
        public string header1
        {
            get
            {
                return "نام خطا";
            }
        }

    }
}
