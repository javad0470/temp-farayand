using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;

namespace SSYM.OrgDsn.ViewModel.Dson
{
    public class CvsnViewModel : NotificationObject
    {
        #region ' Fields '

        private ListCollectionView listCvsn;
        private TblNod cvsnPosPstSrc;
        private TblNod cvsnPosPstDst;
        private TblCvsn cvsn;
        BPMNDBEntities context;
        bool _isSrc;
        #endregion

        #region ' Initialaizer '

        public CvsnViewModel(BPMNDBEntities context, IWayAwrIfrm wayawrifrm, TblNod pospstsrc, TblNod pospstdst, bool isSrc)
        {
            CvsnIWayAwrIfrm = wayawrifrm;
            cvsnPosPstSrc = pospstsrc;
            cvsnPosPstDst = pospstdst;
            SaveFormCommand = new DelegateCommand(OKCvsn);
            this._isSrc = isSrc;
            this.context = context;
            cvsn = TblCvsn.GetCvsn(context,
                pospstsrc,
                pospstdst,
                wayawrifrm.FldCod, wayawrifrm.EtyForCvsnTyp);
            DsonOverviewVM = new DsonOverviewViewModel();
            DsonOverviewVM.CurrAwrIfrm = wayawrifrm;
        }

        #endregion

        #region ' Properties / Commands '

        public bool IsSrc
        {
            get
            {
                return _isSrc;
            }
        }

        public int DefineInOutDson
        {
            get
            {
                switch (CvsnIWayAwrIfrm.DsonType)
                {
                    case SSYM.OrgDsn.Model.Enum.TypDson.NoDson:
                        return 0;

                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndNewsFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.OutSpcfToUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.SndOralFromSpcfToUnspcf:

                        return 1;

                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralInUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvNewsToSpcfFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.InSpcfFromUnspcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromSpcf:
                    case SSYM.OrgDsn.Model.Enum.TypDson.RcvOralToSpcfFromUnspcf:

                        return 2;


                    default:
                        return 0;
                }
            }
        }

        public string CvsnTxt { get; set; }

        public IWayAwrIfrm CvsnIWayAwrIfrm { get; set; }

        public string CvsnPosPstSrcName
        {
            get
            {
                if (DefineInOutDson == 1)
                    return cvsnPosPstSrc.FldNamNod;
                else
                    return cvsnPosPstDst.FldNamNod;
            }

        }

        public string CvsnPosPstDstName
        {
            get
            {
                if (DefineInOutDson == 1)
                    return cvsnPosPstDst.FldNamNod;
                else
                    return cvsnPosPstSrc.FldNamNod;

            }
        }

        //public ListCollectionView ListCvsn
        //{
        //    get
        //    {
        //        listCvsn = new ListCollectionView(PublicMethods.IdentifyCvsnDson_19468(MainWindowViewModel.MainContext, CvsnIWayAwrIfrm));
        //        listCvsn.SortDescriptions.Add(new SortDescription("FldDte", ListSortDirection.Ascending));
        //        return listCvsn;

        //    }

        //}

        public ObservableCollection<TblCvsnDtl> DtlList
        {
            get
            {
                if (cvsn != null)
                {
                    return new ObservableCollection<TblCvsnDtl>(cvsn.TblCvsnDtls);
                }
                return null;
            }
        }

        public ICommand SaveFormCommand { get; set; }

        public TblAct CvsnActSrc
        {
            get
            {
                if (DefineInOutDson == 1)
                    return CvsnIWayAwrIfrm.ActSrc;
                else
                    return CvsnIWayAwrIfrm.ActDst;

            }
        }

        public TblAct CvsnActDst
        {
            get
            {
                if (DefineInOutDson == 1)
                    return CvsnIWayAwrIfrm.ActDst;
                else
                    return CvsnIWayAwrIfrm.ActSrc;

            }
        }


        public DsonOverviewViewModel DsonOverviewVM { get; set; }


        //FldDte

        #endregion

        #region ' Private Methods '

        private void OKCvsn()
        {

            if (Util.ShowMessageBox(73) == System.Windows.MessageBoxResult.Yes)
            {
                if (!string.IsNullOrEmpty(CvsnTxt))
                {
                    if (cvsn == null)
                    {
                        cvsn = TblCvsn.CreateCvsn(context, cvsnPosPstSrc, cvsnPosPstDst, CvsnIWayAwrIfrm.FldCod, CvsnIWayAwrIfrm.EtyForCvsnTyp);
                    }

                    TblCvsnDtl cvsndtl = new TblCvsnDtl()
                    {
                        FldTellEedBySrc = _isSrc,
                        FldTxt = CvsnTxt,
                        FldDte = DateTime.Now
                    };

                    cvsn.TblCvsnDtls.Add(cvsndtl);

                    PublicMethods.SaveContext(context);

                    RaisePropertyChanged("DtlList");
                }

                if (CvsnOKClicked != null)
                {
                    CvsnOKClicked(this, new EventArgs());
                }
            }
        }

        #endregion

        #region ' events '

        public event EventHandler CvsnOKClicked;

        #endregion

    }
}
