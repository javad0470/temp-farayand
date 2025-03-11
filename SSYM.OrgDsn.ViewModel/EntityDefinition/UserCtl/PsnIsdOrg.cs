using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl.Ywork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Diagrams.Core;
using SSYM.OrgDsn.Model.Base;
using System.Windows;
using System.Windows.Data;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.UserCtl
{

    public class PsnIsdOrgViewModel : SSYM.OrgDsn.ViewModel.Base.PopupViewModel
    {


        #region ' Fields '

        ObservableCollection<TblPsn> psnIsdOrg;
        ObservableCollection<TblPsn> selectedPsnIsdOrg;

        #endregion

        #region ' Initialaizer '

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblPsn> PsnIsdOrg
        {
            get { return psnIsdOrg; }
            set
            {
                psnIsdOrg = value;

                PsnIsdOrgCV = new ListCollectionView(value);

                PsnIsdOrgCV.Filter = srchPsn;

                SelectedPsnIsdOrg = psnIsdOrg.FirstOrDefault(); 

                RaisePropertyChanged("PsnIsdOrg");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public TblPsn SelectedPsnIsdOrg
        {
            get {
                if (selectedPsnIsdOrg.Count == 0)
                    return null;
                TblPsn first = selectedPsnIsdOrg.First();
                return first;
            }
            set
            {
                    selectedPsnIsdOrg = new ObservableCollection<TblPsn>();
                    selectedPsnIsdOrg.Add(value);
                RaisePropertyChanged("SelectedPsnIsdOrg");
            }
        }
        public void RemoveSelectedPsnIsdOrg(TblPsn i)
        {
            if (selectedPsnIsdOrg.Contains(i))
                selectedPsnIsdOrg.Remove(i);
        }
        public void AddSelectedPsnIsdOrg(TblPsn i)
        {
            if (!selectedPsnIsdOrg.Contains(i))
                selectedPsnIsdOrg.Add(i);
        }
        public ListCollectionView PsnIsdOrgCV { get; set; }

        //public string SrchStr { get; set; }

        string _srchStr;

        public string SrchStr
        {
            get { return _srchStr; }
            set
            {
                _srchStr = value;
                PsnIsdOrgCV.Refresh();
            }
        }

        #endregion

        #region ' Public Methods '

        protected override void OKExecute()
        {
            base.OKExecute();
            if (OnOKExecute != null)
            {
                OnOKExecute(this, new EventArgs());
            }
        }
        #endregion

        #region ' Private Methods '

        private bool srchPsn(object obj)
        {
            TblPsn psn = obj as TblPsn;

            bool nam1 = false;

            bool nam2 = false;

            if (string.IsNullOrWhiteSpace(SrchStr))
            {
                return true;
            }

            string str = SrchStr.Trim().ToLower();

            if (obj == null)
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(psn.FldNam1stPsn))
            {
                nam1 = psn.FldNam1stPsn.Trim().ToLower().Contains(str);
            }

            if (!string.IsNullOrWhiteSpace(psn.FldNam2ndPsn))
            {
                nam2 = psn.FldNam2ndPsn.Trim().ToLower().Contains(str);
            }

            if (nam1 | nam2)
            {
                return true;
            }
            else
            {
                return false;
            }

            //throw new NotImplementedException();
        }

        #endregion

        #region ' Events '

        public event EventHandler OnOKExecute;

        #endregion
    }
}
