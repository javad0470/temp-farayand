using System.Collections.Specialized;
using System.Diagnostics;
using Telerik.Windows.Controls.Diagrams.Extensions.ViewModels;

namespace SSYM.OrgDsn.ViewModel.EntityDefinition.ChartViewModel
{
    public class Node : HierarchicalNodeViewModel
    {
        public Node()
        {
            this.Children.CollectionChanged += Children_CollectionChanged;
        }
        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasChildren");
        }
        public bool AreChildrenCollapsed { get; set; }
        public bool AreChildrenSelected { get; set; }
        public Branch Branch { get; set; }
        public string Name
        {
            get
            {
                return CurrentNode.Name;
            }
            set { CurrentNode.Name = value; }
        }
        public string ImagePath { get; set; }
        public string LastName { get; set; }
        public Model.Base.IOrgChart CurrentNode { get; set; }

        public bool HasChildren { get { return this.Children != null && this.Children.Count > 0; } }

        private bool isAddNodPopupOpen;
        public bool IsAddNodPopupOpen
        {
            get { return isAddNodPopupOpen; }
            set
            {
                isAddNodPopupOpen = value;
                OnPropertyChanged("IsAddNodPopupOpen");
            }
        }

        private bool isChangeNodPopupOpen;
        public bool IsChangeNodPopupOpen
        {
            get { return isChangeNodPopupOpen; }
            set
            {
                isChangeNodPopupOpen = value;
                OnPropertyChanged("IsChangeNodPopupOpen");
            }
        }

        private bool isRenamePopupOpen;
        public bool IsRenamePopupOpen
        {
            get { return isRenamePopupOpen; }
            set
            {
                isRenamePopupOpen = value;
                OnPropertyChanged("IsRenamePopupOpen");
            }
        }
    }
}