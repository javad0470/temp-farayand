using SSYM.OrgDsn.Model;
using SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.DragDrop;
using SSYM.OrgDsn.Model.Enum;

namespace SSYM.OrgDsn.UI.View.EntityDefinition.UserCtl
{
    /// <summary>
    /// Interaction logic for Rols.xaml
    /// </summary>
    public partial class DropableWrapPnl : UserControl
    {
        public DropableWrapPnl()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(src, OnDragInitialize);

            DragDropManager.AddGiveFeedbackHandler(src, OnGiveFeedback);

            DragDropManager.AddDragDropCompletedHandler(src, OnDragCompleted);

            DragDropManager.AddDropHandler(src, OnDrop);
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            args.AllowedEffects = DragDropEffects.All;
            //var payload = DragDropPayloadManager.GeneratePayload(null);
            //payload.SetData("DragData", ((FrameworkElement)args.OriginalSource).DataContext);
            //args.Data = payload;
            //args.DragVisual = new ContentControl { Content = args.Data, ContentTemplate = LayoutRoot.Resources["ApplicationTemplate"] as DataTemplate };
        }

        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors.Arrow);
            args.Handled = true;
        }

        private void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs args)
        {
            using (Model.BPMNDBEntities context = new Model.BPMNDBEntities())
            {

                TblRol rol = src.Tag as TblRol;
                //اگر شخص جاری نماینده سازمان جاری نیست
                if (!PublicMethods.GetAgntOfPsnIsdOrg_22230(context, PublicMethods.CurrentUser.FldCodPsn, PublicMethods.CurrentUser.FldCodOrg).Any(a => a.TblNod.EtyNod.TypEty == AllTypEty.Org))
                {
                    if (!canEditRol(rol))
                    {
                        UIUtil.ShowMessageBox(55, null);
                        return;
                    }
                }


                EntityObject eo = (EntityObject)(((DataObject)args.Data).GetData("System.Windows.Controls.Grid") as Grid).DataContext;

                if (eo != null)
                {
                    if (eo.GetType() == typeof(Model.TblOrg))
                    {
                        Model.TblOrg tblOrg = (Model.TblOrg)eo;
                        if (tblOrg.FldCodOrg == PublicMethods.CurrentUser.FldCodOrg)
                        {
                            //امکان افزودن سازمان جاری به یک نقش وجود ندارد.
                            UIUtil.ShowMessageBox(70, null);
                            return;
                        }
                        System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
                        Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblOrg.FldCodOrg && E.FldCodTypEty == 1);
                        Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
                        if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
                        {
                            tblPlyrRol.Add(tblPlyrRol1);
                        }

                    }
                    else if (eo.GetType() == typeof(Model.TblPsn))
                    {
                        System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
                        Model.TblPsn tblPsn = (Model.TblPsn)eo;
                        Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblPsn.FldCodPsn && E.FldCodTypEty == 3);
                        Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
                        if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
                        {
                            tblPlyrRol.Add(tblPlyrRol1);
                        }
                    }

                    else if (eo.GetType() == typeof(Model.TblPosPstOrg))
                    {
                        System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol> tblPlyrRol = (this.src.Content as ItemsControl).ItemsSource as System.Data.Objects.DataClasses.EntityCollection<Model.TblPlyrRol>;
                        Model.TblPosPstOrg tblPosPstOrg = (Model.TblPosPstOrg)eo;
                        Model.TblNod nod = context.TblNods.SingleOrDefault(E => E.FldCodEty == tblPosPstOrg.FldCodPosPst && E.FldCodTypEty == 2);
                        Model.TblPlyrRol tblPlyrRol1 = new Model.TblPlyrRol() { FldCodNod = nod.FldCodNod };
                        if (tblPlyrRol.Where(E => E.FldCodNod == tblPlyrRol1.FldCodNod).Count() == 0)
                        {
                            tblPlyrRol.Add(tblPlyrRol1);
                        }
                    }
                }


            }
        }

        /// <summary>
        /// بررسی دسترسی به ویرایش به یک نقش 
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        private bool canEditRol(TblRol rol)
        {
            PublicMethods.CurrentUser.AcsUsr.DetectSttAcsWthEtyMom_22088(rol.Nod, "Edit", Model.Enum.TypRlnEtyMjrWthEtyMom.NodSelf, null, "RolIsdOrg");
            return PublicMethods.CurrentUser.AcsUsr["EditRolIsdOrg"];
        }


        public void OnDragCompleted(object sender, Telerik.Windows.DragDrop.DragDropCompletedEventArgs args)
        {
            //((IList)(sender as ListBox).ItemsSource).Remove(args.Data);
        }

        private void selectRol(object sender, RoutedEventArgs e)
        {
            var item = UIUtil.FindParent<ListBoxItem>(sender as DependencyObject);

            if (item != null)
            {
                item.IsSelected = true;
            }
        }


        private void unSelectRol(object sender, RoutedEventArgs e)
        {
            //var item = UIUtil.FindParent<ListBoxItem>(sender as Grid);

            //if (item != null)
            //{
            //    item.IsSelected = false;
            //}
        }

        
    }
}
