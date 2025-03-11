using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.Model.Enum;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows;
using SSYM.OrgDsn.Model.Base;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.Main
{
    public class ActDefViewModel : BaseViewModel, IViewModel
    {
        #region ' Fields '

        //public GenericInteractionRequest<string> Notification { get; private set; }

        Model.BPMNDBEntities context;

        UserControlViewModel selectedObj;

        bool isActPrgRingActivated;

        bool _isActVisible;

        bool _canUsrEditAct;

        TblAct _selectedAct;

        #endregion

        #region ' Initialaizer '

        public ActDefViewModel()
        {
            context = new BPMNDBEntities();
            OrgPosVM = new PosPstRolViewModel(context);
            ActLstVM = new ActLstViewModel(context);
            ActLstVM.SelectedActChanging += ActListVM_SelectedActChanging;

            ActLstVM.PropertyChanged += ActLstVM_PropertyChanged;
            OrgPosVM.PropertyChanged += OrgPosVM_PropertyChanged;

            AddNewActCommand = new DelegateCommand(ExecuteAddNewActCommand, CanExecuteAddNewActCommand);
            (AddNewActCommand as DelegateCommand).RaiseCanExecuteChanged();

            DeleteActCommand = new DelegateCommand(DeleteAct, CanExecuteDeleteAct);

            initialiazeActDgrm();



        }
        public void DeleteAct()
        {
            if (ActDgrm.Activity == null)
            {
                return;
            }
            if (Util.ShowMessageBox(2, "فعالیت") == System.Windows.MessageBoxResult.Yes)
            {
                ActDgrm.ActDeleteOK();
            }
            else
            {
                ActDgrm.ActDeleteCancel();
            }
        }

        #endregion

        #region ' Properties / Commands '


        public ICommand AddNewActCommand { get; set; }

        public ICommand DeleteActCommand { get; set; }

        /// <summary>
        /// قابل مشاهده بودن یا نبودن فعالیت و رخدادهای آغازگر و نتبجه بر اساس دسترسی کاربر
        /// </summary>
        public bool IsActVisible
        {
            get { return _isActVisible && !ActLstVM.ActListCV.IsEmpty; }
            set
            {
                _isActVisible = value;

                RaisePropertyChanged("IsActVisible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanUsrEditAct
        {
            get { return _canUsrEditAct; }
            set
            {
                _canUsrEditAct = value;

                RaisePropertyChanged("CanUsrEditAct");
            }
        }

        /// <summary>
        /// پروگرس بار لود شدن فعالیت
        /// </summary>
        public bool IsActPrgRingActivated
        {
            get { return isActPrgRingActivated; }
            set
            {
                isActPrgRingActivated = value;

                RaisePropertyChanged("IsActPrgRingActivated");
            }
        }

        public PosPstRolViewModel OrgPosVM { get; set; }

        public ActDgrmViewModel ActDgrm { get; set; }

        public UserControlViewModel SelectedObj
        {
            get
            {
                return selectedObj;
            }
            set
            {
                if (value is DtlActViewModel)
                {
                    var dtl = value as DtlActViewModel;
                    dtl.ActChanged -= dtl_ActChanged;
                    dtl.ActChanged += dtl_ActChanged;
                }
                selectedObj = value;
                RaisePropertyChanged("SelectedObj");
            }
        }


        public ActLstViewModel ActLstVM { get; set; }

        #endregion

        #region ' Public Methods '

        bool _actLstVisible;

        public bool ActLstVisible
        {
            get { return _actLstVisible; }
            set
            {
                _actLstVisible = value;
                RaisePropertyChanged("ActLstVisible");
            }
        }


        public void AddNewAct()
        {
            if (HasContextChanges())
            {

                if (Util.ShowMessageBox(3) == System.Windows.MessageBoxResult.Yes)
                {
                    AddNewActWithSave();
                }
                else
                {
                    AddNewActWithNoSave();
                }
            }
            else
            {
                AddAct();
            }
        }

        public void UnsubscribeOrgPosVM_PropertyChanged()
        {
            OrgPosVM.PropertyChanged -= OrgPosVM_PropertyChanged;
            ActLstVM.SelectedActChanging -= ActListVM_SelectedActChanging;
        }

        public void SubscribeOrgPosVM_PropertyChanged()
        {
            OrgPosVM.PropertyChanged += OrgPosVM_PropertyChanged;
            ActLstVM.SelectedActChanging += ActListVM_SelectedActChanging;
        }

        public TblAct GetAnotherActivityForDisplay(TblAct currAct)
        {
            return this.ActLstVM.ActList.FirstOrDefault();
            //int totalCount = this.OrgPosVM.ActList.Count();
            //int index = this.OrgPosVM.ActList.ToList().FindIndex(m => m.FldCodAct == currAct.FldCodAct);
            //int newIndex = (index + 1) % totalCount;
            //return this.OrgPosVM.ActList[newIndex];
        }

        public void SaveContext()
        {
            if (HasContextChanges())
            {
                if (this.ActDgrm.Activity.HasErrors)
                {
                    Util.ShowMessageBox(26);
                    ActLstVM.CanChangeAct = false;
                    return;
                }

                if (_showSaveConfirm)
                {
                    _showSaveConfirm = false;

                    if (Util.ShowMessageBox(3) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (this.ActDgrm.Activity == null)
                            {
                                ActLstVM.CanChangeAct = true;
                                return;
                            }

                            ActLstVM.CanChangeAct = PublicMethods.ValidateActChange(this.ActDgrm.Activity);
                        }
                        catch (Exception ex)
                        {
                            MenuViewModel.MainMenu.RaisePopup(new Popup.PopupDataObject(ex.Message, "خطا", Popup.MessageBoxType.Error, null), (x) => { }, null);
                            ActLstVM.CanChangeAct = false;
                            return;
                        }


                        PublicMethods.SaveContext(this.ActDgrm.Context);
                        ActLstVM.CanChangeAct = true;

                    }
                    else
                    {
                        PublicMethods.RollBackContext(this.ActDgrm.Context);
                        ActLstVM.CanChangeAct = true;
                    }
                }
                else
                {
                    try
                    {
                        if (this.ActDgrm.Activity == null)
                        {
                            ActLstVM.CanChangeAct = true;
                            return;
                        }

                        ActLstVM.CanChangeAct = PublicMethods.ValidateActChange(this.ActDgrm.Activity);
                    }
                    catch (Exception ex)
                    {
                        MenuViewModel.MainMenu.RaisePopup(new Popup.PopupDataObject(ex.Message, "خطا", Popup.MessageBoxType.Error, null), (x) => { }, null);
                        ActLstVM.CanChangeAct = false;
                        return;
                    }

                    PublicMethods.SaveContext(this.ActDgrm.Context);
                    ActLstVM.CanChangeAct = true;
                }
            }

            //PublicMethods.SaveContext(this.context);
        }

        bool _showSaveConfirm = false;
        public bool ConfirmAndClose()
        {
            if (HasContextChanges())
            {
                if (Util.ShowMessageBox(6) == MessageBoxResult.Yes)
                {
                    _showSaveConfirm = false;
                    this.SaveContext();
                    if (ActLstVM.CanChangeAct)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

        #endregion

        #region ' Private Methods '

        private void initialiazeActDgrm()
        {
            ActDgrm = new ActDgrmViewModel(ActLstVM.SelectedAct, new BPMNDBEntities(), this);
            ActDgrm.PropertyChanged += ActDgrm_PropertyChanged;
            RaisePropertyChanged("ActDgrm");
            (AddNewActCommand as DelegateCommand).RaiseCanExecuteChanged();
            //refreshSelectedActNameInList();
        }

        private void SaveOK()
        {
            //
        }

        private bool HasContextChanges()
        {
            return Util.HasContextChanges(ActDgrm.Context);
        }
        ///// <summary>
        ///// افزودن یک فعالیت جدید
        ///// </summary>
        //public void AddNewAct()
        //{
        //    if (this.Activity != null)
        //    {
        //        parentVM.AddNewAct();
        //    }
        //}

        private bool CanExecuteAddNewActCommand()
        {
            if (ActDgrm.Activity != null && ActDgrm.Acs_AddAct)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// افزودن یک فعالیت جدید
        /// </summary>
        private void ExecuteAddNewActCommand()
        {
            using (var ctx = new BPMNDBEntities())
            {
                if (Util.LcsSfw == null)
                {
                    MessageBox.Show("بنا بر دلایل امنیتی امکان اجرای نرم افزار وجود ندارد. لطفا با شرکت تماس بگیرید");
                    Application.Current.Shutdown();
                }
                if (Util.LcsSfw.TnoAct != -1 &&
                    ctx.TblActs.LongCount(a => a.FldActUspf != true) >= Util.LcsSfw.TnoAct)
                {
                    TblMsg msg = PublicMethods.TblMsgs.Single(m => m.FldCodMsg == 81);
                    MenuViewModel.MainMenu.RaisePopup(new PopupDataObject(
                        msg.FldTxtMsg, msg.FldTtlMsg, (MessageBoxType) msg.FldTypMsg, null),
                        (r) => { },
                        null);
                    return;
                }
            }

            if (ActDgrm.Activity != null)
            {
                this.AddNewAct();
            }
        }

        private bool CanExecuteDeleteAct()
        {
            return ActDgrm.Activity != null && ActDgrm.Activity.FldActUspf != true;
        }

        void ActLstVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedAct")
            {
                IsActPrgRingActivated = true;

                initialiazeActDgrm();
                //= OrgPosVM.SelectedAct;

                IsActPrgRingActivated = false;
                (DeleteActCommand as DelegateCommand).RaiseCanExecuteChanged();
            }

            ActLstVisible = true;

            if (e.PropertyName == "Acs_ViewAct")
            {
                //if (ActLstVM.RecentValue_Acs_ViewAct != null)
                //{
                //    if (!ActLstVM.RecentValue_Acs_ViewAct.Value)
                //    {
                //        ActLstVisible = false;

                //        UserManager.ShowAccessMessage(17, "مشاهده فعالیت های " + OrgPosVM.NodSlcEed.FldNamNod);
                //    }
                //}

                //else
                {
                    if (!ActLstVM.Acs_ViewAct)
                    {
                        ActLstVisible = false;

                        UserManager.ShowAccessMessage(17, "مشاهده فعالیت های " + OrgPosVM.NodSlcEed.FldNamNod);
                    }
                }

            }


        }


        private void OrgPosVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {


            //if (e.PropertyName == "SelectedOrg"/* || e.PropertyName == "ActList"*/)
            //{
            //    if (OrgPosVM.PosPstSlcEed != null && OrgPosVM.ActList != null && OrgPosVM.ActList.Count > 0)
            //    {
            //        UnsubscribeOrgPosVM_PropertyChanged();
            //        OrgPosVM.SelectedAct = OrgPosVM.ActList.First();
            //        //initialiazeActDgrm();
            //        ActDgrm.ActContainerVisibility = (OrgPosVM.RecentValue_Acs_ViewAct != null && OrgPosVM.RecentValue_Acs_ViewAct.Value) ? Visibility.Visible : Visibility.Collapsed;
            //        this.IsActVisible = (OrgPosVM.RecentValue_Acs_ViewAct != null && OrgPosVM.RecentValue_Acs_ViewAct.Value);
            //        SubscribeOrgPosVM_PropertyChanged();
            //    }
            //}


            if (e.PropertyName == "NodSlcEed")
            {
                ActLstVM.SelectedNod = OrgPosVM.NodSlcEed;
                this.IsActVisible = ActLstVM.Acs_ViewAct; //(ActLstVM.RecentValue_Acs_ViewAct != null && ActLstVM.RecentValue_Acs_ViewAct.Value);
            }

            //if (e.PropertyName == "SelectedRol"/* || e.PropertyName == "ActList"*/)
            //{
            //    if (OrgPosVM.SelectedRol != null && OrgPosVM.ActList != null && OrgPosVM.ActList.Count > 0)
            //    {
            //        UnsubscribeOrgPosVM_PropertyChanged();
            //        OrgPosVM.SelectedAct = OrgPosVM.ActList.First();
            //        //initialiazeActDgrm();
            //        ActDgrm.ActContainerVisibility = (OrgPosVM.RecentValue_Acs_ViewAct != null && OrgPosVM.RecentValue_Acs_ViewAct.Value) ? Visibility.Visible : Visibility.Collapsed;
            //        this.IsActVisible = (OrgPosVM.RecentValue_Acs_ViewAct != null && OrgPosVM.RecentValue_Acs_ViewAct.Value);
            //        SubscribeOrgPosVM_PropertyChanged();
            //    }
            //}


        }

        void ActDgrm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "Acs_EditAct")
            //{
            //    CanUsrEditAct = ActDgrm.Acs_EditAct;
            //}
        }

        private void AddNewActWithNoSave()
        {
            PublicMethods.RollBackContext(this.context);
            AddAct();
        }

        private void AddNewActWithSave()
        {
            PublicMethods.SaveContext(context);
            AddAct();
        }

        /// <summary>
        /// Pdr9006
        /// </summary>
        private void AddAct()
        {
            BPMNDBEntities ctx = new BPMNDBEntities();
            TblAct newAct = new TblAct() { FldNamAct = "فعالیت جدید", FldTypAct = (int)SSYM.OrgDsn.Model.Enum.ActivityTypes.Manual, FldActSubHav = 2 };
            newAct.TblEvtSrts = new EntityCollection<TblEvtSrt>();
            newAct.TblEvtRsts = new EntityCollection<TblEvtRst>();
            //OrgPosVM.SelectedNode.TblActs.Add(newAct);
            TblNod nod = ctx.TblNods.Single(m => m.FldCodNod == OrgPosVM.NodSlcEed.FldCodNod);
            PublicMethods.AddActAndChgPrs_3202(ctx, newAct, nod);
            PublicMethods.SaveContext(ctx);
            ActDgrm = new ActDgrmViewModel(newAct, ctx, this);
            RaisePropertyChanged("ActDgrm");


            ActLstVM.ActList.Add(newAct);
            ActLstVM.PropertyChanged -= ActLstVM_PropertyChanged;
            ActLstVM.SelectedAct = context.TblActs.Single(m => m.FldCodAct == newAct.FldCodAct);
            ActLstVM.PropertyChanged += ActLstVM_PropertyChanged;

        }

        private void ShowCannotSaveActError()
        {
            Util.ShowMessageBox(9);
            SaveOK();
        }

        void ActListVM_SelectedActChanging(object sender, EventArgs e)
        {
            try
            {
                if (this.ActDgrm.Activity == null)
                {
                    ActLstVM.CanChangeAct = true;
                    return;
                }

                if (this.ActDgrm.Activity.HasErrors)
                {
                    Util.ShowMessageBox(26);
                    ActLstVM.CanChangeAct = false;
                    return;
                }

                if (HasContextChanges())
                {

                    if (Util.ShowMessageBox(3) == System.Windows.MessageBoxResult.Yes)
                    {
                        ActLstVM.CanChangeAct = PublicMethods.ValidateActChange(this.ActDgrm.Activity);

                        PublicMethods.SaveContext(ActDgrm.Context);

                        ActLstVM.CanChangeAct = true;
                        //refreshSelectedActNameInList();
                    }
                    else
                    {
                        PublicMethods.RollBackContext(this.ActDgrm.Context);
                        ActLstVM.CanChangeAct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MenuViewModel.MainMenu.RaisePopup(new Popup.PopupDataObject(ex.Message, "خطا", Popup.MessageBoxType.Error, null), (x) => { }, null);
                ActLstVM.CanChangeAct = false;
            }
        }

        private void refreshSelectedActNameInList()
        {
            try
            {
                if (ActLstVM.SelectedAct.FldNamAct != ActDgrm.Activity.FldNamAct)
                {
                    ActLstVM.SelectedAct.FldNamAct = ActDgrm.Activity.FldNamAct;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// پس از تغییر نام فعالیت در قسمت جزئیات، نام آن را در لیست بروز میکند
        /// </summary>
        /// <param name="obj"></param>
        void dtl_ActChanged(TblAct obj)
        {
            if (obj != null && ActLstVM.SelectedAct != null)
            {
                if (ActLstVM.SelectedAct.FldNamAct != obj.FldNamAct)
                {
                    ActLstVM.SelectedAct.FldNamAct = obj.FldNamAct;
                }
            }
        }

        #endregion
    }
}
