using Microsoft.Practices.Prism.Commands;
using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SSYM.OrgDsn.ViewModel.ActivityDefinition.UserCtl
{
    public class EvtSrtAftrCdn : EvtCdn
    {
        public EvtSrtAftrCdn(BPMNDBEntities context, EntityObject obj)
            : base(context, obj)
        {
        }

        //#region ' Fields '

        //ObservableCollection<SSYM.OrgDsn.Model.TblIdx> tblIdx;
        //ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> compareTools;
        //ObservableCollection<SSYM.OrgDsn.Model.TblUntMsrt> tblUntMsrt;

        //#endregion

        //#region ' Initializer '

        //public EvtSrtAftrCdn(BPMNDBEntities context, EntityObject obj)
        //    : base(context, obj)
        //{
        //}

        //protected override void Initialiaze()
        //{
        //    base.Initialiaze();
        //    DeleteCommand = new DelegateCommand<SSYM.OrgDsn.Model.TblCdn>(ExcecuteDeleteCommand);
        //    AddNewRowCommand = new DelegateCommand(ExecuteAddNewRowCommand);
        //    SaveCommand = new DelegateCommand(ExecuteSaveCommand);
        //    RaisePropertyChanged("TblIdx", "TblUntMsrt", "TblCdn");
        //}

        //#endregion

        //#region ' Properties / Commands '

        ///// <summary>
        ///// لیست تمام شرایط ثبت شد ه برای رخداد جاری
        ///// </summary>
        //public ObservableCollection<SSYM.OrgDsn.Model.TblCdn> TblCdn
        //{
        //    get
        //    {
        //        return new ObservableCollection<Model.TblCdn>(this.TblEvtSrt.TblCdns);
        //    }
        //}

        ///// <summary>
        ///// رخداد آغازگر
        ///// </summary>
        //public Model.TblEvtSrt TblEvtSrt
        //{
        //    get
        //    {
        //        return Entity as TblEvtSrt;
        //    }
        //    set
        //    {
        //        Entity = value;
        //    }
        //}

        ///// <summary>
        ///// Delete Command
        ///// </summary>
        //public ICommand DeleteCommand { get; set; }

        ///// <summary>
        ///// جدول شاخص ها
        ///// </summary>
        //public ObservableCollection<SSYM.OrgDsn.Model.TblIdx> TblIdx
        //{
        //    get
        //    {
        //        if (tblIdx == null)
        //        {
        //            //this.bpmnEty.Refresh(System.Data.Objects.RefreshMode.StoreWins, bpmnEty.TblIdxes.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));

        //            PublicMethods.ReloadEntity(this.bpmnEty, bpmnEty.TblIdxes.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));

        //            tblIdx = new ObservableCollection<Model.TblIdx>(bpmnEty.TblIdxes.Where(E => E.FldCodOrg == UserManager.CurrentUser.FldCodOrg));
        //        }
        //        return tblIdx;
        //    }
        //    set { tblIdx = value; }
        //}

        ///// <summary>
        ///// ابزارهای مقایسه
        ///// </summary>
        //public ObservableCollection<SSYM.OrgDsn.Model.TblItmFixSfw> CompareTools
        //{
        //    get
        //    {
        //        if (compareTools == null)
        //        {
        //            compareTools = new ObservableCollection<Model.TblItmFixSfw>(bpmnEty.TblItmFixSfws.Where(E => E.FldCodSbj == 7));
        //        }
        //        return compareTools;
        //    }
        //}

        ///// <summary>
        ///// واحدهای سنجش
        ///// </summary>
        //public ObservableCollection<SSYM.OrgDsn.Model.TblUntMsrt> TblUntMsrt
        //{
        //    get
        //    {
        //        if (tblUntMsrt == null)
        //        {
        //            //this.bpmnEty.Refresh(System.Data.Objects.RefreshMode.StoreWins, bpmnEty.TblUntMsrts);

        //            PublicMethods.ReloadEntity(this.bpmnEty, bpmnEty.TblUntMsrts);

        //            tblUntMsrt = new ObservableCollection<Model.TblUntMsrt>(bpmnEty.TblUntMsrts);
        //        }
        //        return tblUntMsrt;
        //    }
        //    set { tblUntMsrt = value; }
        //}

        ///// <summary>
        ///// Add new row command
        ///// </summary>
        //public ICommand AddNewRowCommand { get; set; }

        ///// <summary>
        ///// save command
        ///// </summary>
        //public ICommand SaveCommand { get; set; }

        //#endregion

        //#region ' Public Methods '


        //#endregion

        //#region ' Private Methods '

        ///// <summary>
        ///// delete command
        ///// </summary>
        ///// <param name="obj"> a parameter of type Model.TblCdn</param>
        //private void ExcecuteDeleteCommand(Model.TblCdn obj)
        //{
        //    if (Util.ShowMessageBox(39) == System.Windows.MessageBoxResult.Yes)
        //    {
        //        this.TblEvtSrt.TblCdns.Remove(obj);
        //        RaisePropertyChanged("TblCdn");
        //    }
        //}

        ///// <summary>
        ///// add new row command
        ///// </summary>
        //private void ExecuteAddNewRowCommand()
        //{
        //    this.TblEvtSrt.TblCdns.Add(new Model.TblCdn() { });
        //    RaisePropertyChanged("TblCdn");

        //}

        ///// <summary>
        ///// ذخیره سازی تغییرات
        ///// </summary>
        //private void ExecuteSaveCommand()
        //{
        //    PublicMethods.SaveContext(bpmnEty);
        //}

        //#endregion
    }
}
