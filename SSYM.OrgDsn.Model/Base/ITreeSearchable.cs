using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SSYM.OrgDsn.Model.Base
{
    public interface ISearchableTree
    {
        bool IsExpanded { get; set; }

        bool IsSelected { get; set; }

        ListCollectionView ChildsCV { get; }

        void UnselectRecurcive();

        void SetFilterMethodRec(Predicate<object> method);

        void RefreshRec();
        
        string HighlightPhrase { get; set; }

        string SearchableString { get; }

        ISearchableTree SearchParent { get; }

        IList<ISearchableTree> SearchChilds { get; }
    }
}
