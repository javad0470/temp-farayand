using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup
{
    public class SlcRolViewModel : PopupViewModel
    {
        #region ' Fields '

        string _txtSrch;

        ListCollectionView _rolsCV;

        #endregion

        #region ' Initialaizer '

        public SlcRolViewModel(BPMNDBEntities context)
            : base(context)
        {
            RolsCV = new ListCollectionView(context.TblRols.Where(m => m.FldCodOrg == SSYM.OrgDsn.ViewModel.Base.UserManager.CurrentUser.FldCodOrg).ToList());
        }

        /// <summary>
        /// نقشهایی که شخص خاصی نماینده آنها باشد
        /// </summary>
        /// <param name="context"></param>
        public SlcRolViewModel(BPMNDBEntities context, TblPsn psn)
            : base(context)
        {
            // نقش های شهص جاری در سازمن جاری 
            RolsCV = new ListCollectionView(PublicMethods.DetectRolWithAgntOfPsn_22180(context, psn).Where(r => r.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg).ToList());
        }

        /// <summary>
        /// نقشهایی که در فرایند خاصی شرکت کرده اند
        /// </summary>
        /// <param name="context"></param>
        public SlcRolViewModel(BPMNDBEntities context, TblPr prs)
            : base(context)
        {
            RolsCV = new ListCollectionView(PublicMethods.DetectRolInPrs_22182(context, prs));
        }


        #endregion

        #region ' Properties / Commands '

        TblRol selectedRol;
        public TblRol SelectedRol
        {
            get
            {
                return selectedRol;
            }
            set
            {
                selectedRol = value;
                RaisePropertyChanged("SelectedRol");
            }
        }


        public TblRol SelectedRolTmp { get; set; }


        public ListCollectionView RolsCV
        {
            get { return _rolsCV; }
            set
            {
                _rolsCV = value;
                if (_rolsCV != null)
                {
                    _rolsCV.Filter = searchRols;
                }
            }
        }

        //public ListCollectionView RolsCV { get; set; }


        public string TxtSrch
        {
            get { return _txtSrch; }
            set
            {
                _txtSrch = value;
                RolsCV.Refresh();
            }
        }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '


        protected override void OKExecute()
        {
            base.OKExecute();
            SelectedRol = SelectedRolTmp;
        }

        public void OK()
        {
            OKExecute();
        }


        private bool searchRols(object obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(TxtSrch))
                return true;

            return (obj as TblRol).Name.Trim().ToLower().Contains(TxtSrch);
        }

        #endregion
    }
}
