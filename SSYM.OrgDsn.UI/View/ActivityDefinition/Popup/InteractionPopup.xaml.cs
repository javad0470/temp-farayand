using SSYM.OrgDsn.UI.View.Base;
using SSYM.OrgDsn.ViewModel.ActivityDefinition.Popup;
using SSYM.OrgDsn.ViewModel.Base;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSYM.OrgDsn.UI.View.ActivityDefinition.Popup
{
    /// <summary>
    /// Interaction logic for InteractionPopup.xaml
    /// </summary>
    public partial class InteractionPopup : BaseInteractionDialog, IGenericInteractionView<PopupDataObject>, IGenericAdapter<PopupDataObject>
    {
        private readonly IGenericAdapter<PopupDataObject> adapter;

        public InteractionPopup()
        {
            this.adapter = new GenericAdapter<PopupDataObject>();
            this.DataContext = this.ViewModel;
            InitializeComponent();
        }

        public void SetEntity(PopupDataObject entity)
        {
            this.ViewModel.SetEntity(entity);
        }

        public PopupDataObject GetEntity()
        {
            return this.ViewModel.GetEntity();
        }

        public IGenericViewModel<PopupDataObject> ViewModel
        {
            get { return this.adapter.ViewModel; }
        }

    }
}
