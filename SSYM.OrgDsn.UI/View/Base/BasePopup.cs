using SSYM.OrgDsn.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SSYM.OrgDsn.UI.View.Base
{
    public class BasePopup : UserControl
    {
        public BasePopup()
        {
            //this.Width = 

            this.DataContextChanged += BasePopup_DataContextChanged;
        }

        void BasePopup_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null && this.DataContext is PopupViewModel)
            {
                PopupViewModel vm = this.DataContext as PopupViewModel;
                this.Width = vm.Width;
                this.Height = vm.Height;
            }

            this.DataContextChanged -= BasePopup_DataContextChanged;
        }
    }
}
