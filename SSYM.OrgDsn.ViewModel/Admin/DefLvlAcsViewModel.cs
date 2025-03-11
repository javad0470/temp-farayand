using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;

namespace SSYM.OrgDsn.ViewModel.Admin
{
    public class DefLvlAcsViewModel : UserControlViewModel, IViewModel
    {
        #region ' Fields '

        ObservableCollection<TblLvlAc> lvlAcs;

        ObservableCollection<TblItmAc> itmAcsForOrg;

        ObservableCollection<TblItmAc> itmAcsForRol;

        ObservableCollection<TblItmAc> itmAcsForPosPst;

        TblLvlAc selectedLvlAcs;

        BPMNDBEntities context;




        #endregion

        #region ' Initialaizer '

        public DefLvlAcsViewModel()
        {
            //this.context = MenuViewModel.MainContext;
            this.context = new BPMNDBEntities();

            this.LvlAcs = new ObservableCollection<TblLvlAc>(this.context.TblLvlAcs);

            AddLvlAcsCommand = new DelegateCommand(ExecuteAddLvlAcsCommand);

            DelLvlAcsCommand = new DelegateCommand(ExecuteDelLvlAcsCommand);

            ItmAcsForOrg = new ObservableCollection<TblItmAc>();

            ItmAcsForPosPst = new ObservableCollection<TblItmAc>();

            ItmAcsForRol = new ObservableCollection<TblItmAc>();
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllAcsForRolSelected
        {
            get
            {
                bool b = true;

                foreach (TblItmAc item in ItmAcsForRol)
                {
                    if (item.IsChecked == null || !item.IsChecked.Value)
                    {
                        b = false;

                        break;
                    }
                }

                return b;
            }
            set
            {
                foreach (TblItmAc item in ItmAcsForRol)
                {
                    item.IsChecked = value;
                }

                RaisePropertyChanged("IsAllAcsForRolSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllAcsForPosPstSelected
        {
            get
            {
                bool b = true;

                foreach (TblItmAc item in ItmAcsForPosPst)
                {
                    if (item.IsChecked == null || !item.IsChecked.Value)
                    {
                        b = false;

                        break;
                    }
                }

                return b;
            }
            set
            {
                foreach (TblItmAc item in ItmAcsForPosPst)
                {
                    item.IsChecked = value;
                }

                RaisePropertyChanged("IsAllAcsForPosPstSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllAcsForOrgSelected
        {
            get
            {
                bool b = true;

                foreach (TblItmAc item in ItmAcsForOrg)
                {
                    if (item.IsChecked == null || !item.IsChecked.Value)
                    {
                        b = false;

                        break;
                    }
                }

                return b;
            }
            set
            {
                foreach (TblItmAc item in ItmAcsForOrg)
                {
                    item.IsChecked = value;
                }

                RaisePropertyChanged("IsAllAcsForOrgSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblItmAc> ItmAcsForOrg
        {
            get { return itmAcsForOrg; }
            set
            {
                itmAcsForOrg = value;

                RaisePropertyChanged("ItmAcsForOrg");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblItmAc> ItmAcsForPosPst
        {
            get { return itmAcsForPosPst; }
            set
            {
                itmAcsForPosPst = value;

                RaisePropertyChanged("ItmAcsForPosPst");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblItmAc> ItmAcsForRol
        {
            get { return itmAcsForRol; }
            set
            {
                itmAcsForRol = value;

                RaisePropertyChanged("ItmAcsForRol");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TblLvlAc SelectedLvlAcs
        {
            get { return selectedLvlAcs; }
            set
            {
                selectedLvlAcs = value;

                DetetItmAcs();

                RaisePropertyChanged("SelectedLvlAcs", "IsAllAcsForRolSelected", "IsAllAcsForPosPstSelected", "IsAllAcsForOrgSelected", "IsLvlAcsReadOnly");
            }
        }

        public bool IsLvlAcsReadOnly
        {
            get
            {
                return SelectedLvlAcs == null || SelectedLvlAcs.FldNam == "سطح دسترسی کامل";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TblLvlAc> LvlAcs
        {
            get { return lvlAcs; }
            set
            {
                lvlAcs = value;

                RaisePropertyChanged("LvlAcs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand AddLvlAcsCommand { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICommand DelLvlAcsCommand { get; set; }

        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteAddLvlAcsCommand()
        {
            TblLvlAc lvl = new TblLvlAc() { FldNam = TblLvlAc.GenerateUniqueName(this.context) };

            this.LvlAcs.Add(lvl);

            this.context.TblLvlAcs.AddObject(lvl);

            PublicMethods.SaveContext(this.context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteDelLvlAcsCommand()
        {
            if (SelectedLvlAcs == null)
            {
                return;
            }
            if (SelectedLvlAcs.TblAgntNods.Count > 0)
            {
                Util.ShowMessageBox(43);
                return;
            }

            if (Util.ShowMessageBox(2, SelectedLvlAcs.FldNam) == System.Windows.MessageBoxResult.Yes)
            {
                this.context.TblLvlAcs.DeleteObject(SelectedLvlAcs);

                this.LvlAcs.Remove(SelectedLvlAcs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetetItmAcs()
        {
            DetectItmAcsForOrg();

            DetectItmAcsForPosPst();

            DetectItmAcsForRol();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectItmAcsForOrg()
        {
            this.ItmAcsForOrg = new ObservableCollection<TblItmAc>(this.context.TblItmAcs.Where(m => m.FldCodTypEtyMom == (int)Model.Enum.AllTypEty.Org && m.FldCodItmAcsPrn == null));

            foreach (TblItmAc item in this.ItmAcsForOrg)
            {
                ChangeLvlAcsCntOfItmAcs(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectItmAcsForPosPst()
        {
            this.ItmAcsForPosPst = new ObservableCollection<TblItmAc>(this.context.TblItmAcs.Where(m => m.FldCodTypEtyMom == (int)Model.Enum.AllTypEty.Pos && m.FldCodItmAcsPrn == null));

            foreach (TblItmAc item in this.ItmAcsForPosPst)
            {
                ChangeLvlAcsCntOfItmAcs(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectItmAcsForRol()
        {
            this.ItmAcsForRol = new ObservableCollection<TblItmAc>(this.context.TblItmAcs.Where(m => m.FldCodTypEtyMom == (int)Model.Enum.AllTypEty.Rol && m.FldCodItmAcsPrn == null));

            foreach (TblItmAc item in this.ItmAcsForRol)
            {
                ChangeLvlAcsCntOfItmAcs(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itmAcsCnt"></param>
        private void ChangeLvlAcsCntOfItmAcs(TblItmAc itmAcsCnt)
        {
            itmAcsCnt.LvlAcsCnt = this.SelectedLvlAcs;

            foreach (TblItmAc item in itmAcsCnt.TblItmAcs1)
            {
                ChangeLvlAcsCntOfItmAcs(item);
            }
        }

        #endregion

        #region ' Events '

        #endregion

        public void SaveContext()
        {
            PublicMethods.SaveContext(this.context);
               
            Util.ConfirmAndRestartApp();


            //MenuViewModel.MainContext.Refresh(System.Data.Objects.RefreshMode.StoreWins, MenuViewModel.MainContext.TblItmAcs);
            //PublicMethods.AllItmAcs = MenuViewModel.MainContext.TblItmAcs.ToList();
            //Model.PublicMethods.CurrentUser.AcsUsr.ExeAcsWotEtyMom_22061();
        }

        public bool ConfirmAndClose()
        {
            if (Util.HasContextChanges(this.context))
            {
                if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                {
                    this.SaveContext();
                    return true;
                }
                else
                {
                    PublicMethods.RollBackContext(this.context);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
