using SSYM.OrgDsn.UI.View.ActivityDefinition.UserCtl;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SSYM.OrgDsn.Model;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Main
{
    /// <summary>
    /// Interaction logic for ActDgrm.xaml
    /// </summary>
    public partial class ActDgrm : UserControl
    {
        public ActDgrm()
        {
            InitializeComponent();
            //this.Loaded += ActDgrm_Loaded;
        }

        void ActDgrm_Loaded(object sender, RoutedEventArgs e)
        {
            //(this.DataContext as ActDgrmViewModel).TblEvtSrts.CollectionChanged += TblEvtSrts_CollectionChanged;
            //(this.DataContext as ActDgrmViewModel).TblEvtRsts.CollectionChanged += TblEvtRsts_CollectionChanged;

        }

        //void TblEvtRsts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //        scrlEvtrst.ScrollToEnd();
        //}

        //void TblEvtSrts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //    {
        //        scrlEvtsrt.ScrollToEnd();
        //    }
        //}

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as UIEntity).DeleteCommand.Execute((sender as UIEntity).CommandParameter);
        }

        private void GroupBox_Drop_1(object sender, DragEventArgs e)
        {
            try
            {
                var dragedSrt = e.Data.GetData(typeof(TblEvtSrt)) as TblEvtSrt;


                if (e.Data == null ||
                    (e.Data.GetData(typeof(string)) == null) && dragedSrt == null)
                    return;

                var group = ((sender as System.Windows.Controls.Border).DataContext as System.Windows.Data.CollectionViewGroup);

                if (dragedSrt != null)
                {
                    (this.DataContext as ActDgrmViewModel).ChangeGroup(dragedSrt, group.Items.First() as TblEvtSrt);
                }
                else
                {
                    int groupName = (int)group.Name;
                    (this.DataContext as ActDgrmViewModel).EvtSrtDrop(e.Data.GetData(typeof(string)).ToString(), groupName);
                }
            }
            catch (Exception)
            {
            }
        }

        private void itmEvtRst_Drop_1(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data == null || e.Data.GetData(typeof(string)) == null)
                    return;
                (this.DataContext as ActDgrmViewModel).EvtRstDrop(e.Data.GetData(typeof(string)).ToString());
            }
            catch (Exception)
            {
            }
        }

        private void brdAct_Drop_1(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data == null || e.Data.GetData(typeof(string)) == null)
                    return;
                (this.DataContext as ActDgrmViewModel).ActDrop(e.Data.GetData(typeof(string)).ToString());
            }
            catch (Exception)
            {
            }
        }

        private void brdEvtSrtDropPlaceholder_Drop_1(object sender, DragEventArgs e)
        {
            try
            {
                if (!(e.Data == null || e.Data.GetData(typeof(string)) == null))
                {
                    (this.DataContext as ActDgrmViewModel).EvtSrtDrop(e.Data.GetData(typeof(string)).ToString(), -1);
                    string data = e.Data.GetData(typeof(string)).ToString();
                    if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
                    {
                        Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlinkR");
                        Storyboard.SetTargetName(sb1, "brdEvtSrtDropPlaceholder");
                        BeginStoryboard(sb1);
                    }
                }
                else
                {
                    GroupBox_Drop_1(sender, e);
                }

            }
            catch (Exception)
            {
            }
        }

        private void brdEvtRstDropPlaceholder_Drop_1(object sender, DragEventArgs e)
        {
            try
            {
                if (!(e.Data == null || e.Data.GetData(typeof(string)) == null))
                {
                    (this.DataContext as ActDgrmViewModel).EvtRstDrop(e.Data.GetData(typeof(string)).ToString());
                    string data = e.Data.GetData(typeof(string)).ToString();
                    if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(data))
                    {
                        Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlinkR");
                        Storyboard.SetTargetName(sb1, "brdEvtRstDropPlaceholder");
                        BeginStoryboard(sb1);
                    }
                }
                else
                {
                    GroupBox_Drop_1(sender, e);
                }
            
            }
            catch (Exception)
            {
            }
        }

        private void ActDelete(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ActDgrmViewModel).DeleteAct();
        }

        public void ScrollEvtRsts()
        {
            scrlEvtrst.ScrollToEnd();
        }

        public void ScrollEvtSrts()
        {
            scrlEvtsrt.ScrollToEnd();
        }

        private void ShowDragEnterStoryboard(object sender, DragEventArgs e)
        {
            try
            {
                string data = e.Data.GetData(typeof(string)).ToString();
                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
                {
                    Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlink");
                    Storyboard.SetTargetName(sb1, "brdEvtSrtDropPlaceholder");
                    BeginStoryboard(sb1);
                }

                /*
                switch ((sender as FrameworkElement).Name)
                {
                    case "brdEvtSrtDropPlaceholder":

                        if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }
                        break;
                    case "brdEvtRstDropPlaceholder":
                        if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(data))
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }
                        break;
                    case "brdAct":
                        if (data == "act")
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }

                        break;

                    default:
                        break;
                }*/
            }
            catch (Exception)
            {
            }

        }

        private void btnAddEvtRst_Click_1(object sender, RoutedEventArgs e)
        {
            ActDgrmViewModel vm = this.DataContext as ActDgrmViewModel;
            SlctEvtRstTypeObj.ShouldAnyCdnVisible = vm.ShouldAnyCdnEvtRstVisible;
            SlctEvtRstTypeObj.ShouldCancelVisible = vm.ShouldCancelEvtRstVisible;
            SlctEvtRstTypeObj.ShouldErrCdnVisible = vm.ShouldErrEvtRstVisible;
        }

        private void border_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data == null || e.Data.GetData(typeof(string)) == null)
                    return;
                string data = e.Data.GetData(typeof(string)).ToString();
                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
                {
                    Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlink");
                    Storyboard.SetTargetName(sb1, "brdEvtSrtDropPlaceholder");
                    BeginStoryboard(sb1);
                }
                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(data))
                {
                    Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlink");
                    Storyboard.SetTargetName(sb1, "brdEvtRstDropPlaceholder");
                    BeginStoryboard(sb1);
                }
                /*
                switch ((sender as FrameworkElement).Name)
                {
                    case "brdEvtSrtDropPlaceholder":

                        if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }
                        break;
                    case "brdEvtRstDropPlaceholder":
                        if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(data))
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }
                        break;
                    case "brdAct":
                        if (data == "act")
                        {
                            Storyboard sb = (Storyboard)this.FindResource("DragEnterStoryboard");
                            Storyboard.SetTargetName(sb, (sender as FrameworkElement).Name);
                            BeginStoryboard(sb);
                        }

                        break;

                    default:
                        break;
                }*/
            }
            catch (Exception)
            {
            }
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            if (PublicMethods.IsDraging)
            {

            }
        }

        private void LayoutRoot_Drop(object sender, DragEventArgs e)
        {
            if (e.Data == null || e.Data.GetData(typeof(string)) == null)
                return;
            string data = e.Data.GetData(typeof(string)).ToString();
            if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(data))
            {
                Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlinkR");
                Storyboard.SetTargetName(sb1, "brdEvtSrtDropPlaceholder");
                BeginStoryboard(sb1);
            }
            if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(data))
            {
                Storyboard sb1 = (Storyboard)this.FindResource("DragEnterBlinkR");
                Storyboard.SetTargetName(sb1, "brdEvtRstDropPlaceholder");
                BeginStoryboard(sb1);
            }

        }

        private void grpEvtSrt_DragEnter(object sender, DragEventArgs e)
        {

            var dragData = e.Data.GetData(typeof(TblEvtSrt));
            var strData = e.Data.GetData(typeof(string));

            if (dragData != null || strData != null)
            {

                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(strData)
                    || dragData is TblEvtSrt)
                {
                    Storyboard sb1 = (Storyboard)this.FindResource("EvtSrtItemControlBorder");
                    //Storyboard.SetTargetName(sb1, (sender as Border).Name);
                    Storyboard.SetTarget(sb1, sender as Border);
                    BeginStoryboard(sb1);
                    Storyboard sb2 = (Storyboard)this.FindResource("EvtSrtItemControlColor");
                    //Storyboard.SetTargetName(sb2, (sender as Border).Name);
                    Storyboard.SetTarget(sb1, sender as Border);
                    //  BeginStoryboard(sb2);
                }
                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(strData))
                {
                    /*Storyboard sb1 = (Storyboard)this.FindResource("EvtSrtItemControlBorder");
                    Storyboard.SetTargetName(sb1, "grpEvtSrt");
                    BeginStoryboard(sb1);
                    Storyboard sb2 = (Storyboard)this.FindResource("EvtSrtItemControlColor");
                    Storyboard.SetTargetName(sb2, "grpEvtSrt");
                    BeginStoryboard(sb2);*/
                }
            }

        }

        private void grpEvtSrt_DragLeave(object sender, DragEventArgs e)
        {
            var dragData = e.Data.GetData(typeof(TblEvtSrt));
            var strData = e.Data.GetData(typeof(string));

            if (dragData != null || strData != null)
            {

                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtSrtType)).Contains(strData)
                    || dragData is TblEvtSrt)
                {
                    Storyboard sb1 = (Storyboard)this.FindResource("EvtSrtItemControlBorderR");
                    //Storyboard.SetTargetName(sb1, (sender as Border).Name);
                    Storyboard.SetTarget(sb1, sender as Border);
                    BeginStoryboard(sb1);
                    Storyboard sb2 = (Storyboard)this.FindResource("EvtSrtItemControlColorR");
                    //Storyboard.SetTargetName(sb2, (sender as Border).Name);
                    Storyboard.SetTarget(sb1, sender as Border);
                    // BeginStoryboard(sb2);
                }
                if (Enum.GetNames(typeof(SSYM.OrgDsn.Model.Enum.EvtRstType)).Contains(strData))
                {
                    /*Storyboard sb1 = (Storyboard)this.FindResource("EvtSrtItemControlBorder");
                    Storyboard.SetTargetName(sb1, "grpEvtSrt");
                    BeginStoryboard(sb1);
                    Storyboard sb2 = (Storyboard)this.FindResource("EvtSrtItemControlColor");
                    Storyboard.SetTargetName(sb2, "grpEvtSrt");
                    BeginStoryboard(sb2);*/
                }
            }
        }
    }
}
