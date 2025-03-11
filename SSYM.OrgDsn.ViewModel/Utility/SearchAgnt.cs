using SSYM.OrgDsn.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Utility
{
    internal class SearchAgnt
    {
        public static string SerachTerm { get; set; }

        public static bool TreeSearch(object o)
        {
            ISearchableTree obj = o as ISearchableTree;

            if (obj == null || string.IsNullOrEmpty(SerachTerm))
            {
                if (obj != null)
                {
                    obj.UnselectRecurcive();
                }
                return true;
            }


            obj.IsSelected = false;

            obj.HighlightPhrase = null;


            var str = SerachTerm.Trim().ToLower();

            var contains = obj.SearchableString.Trim().ToLower().Contains(str);


            if (contains
    ||
    obj.SearchChilds.Any(p => p.IsSelected)
    )
            {
                if (obj.SearchParent != null)
                {
                    obj.SearchParent.IsExpanded = true;
                }

                obj.IsSelected = true;

                if (obj.SearchableString.Trim().ToLower().Contains(str))
                    obj.HighlightPhrase = SerachTerm;

                return true;
            }
            else
            {
                obj.IsExpanded = obj.IsSelected = false;
            }

            return false;
        }

        //public static bool ListSearch(object o)
        //{

        //}
    }
}
