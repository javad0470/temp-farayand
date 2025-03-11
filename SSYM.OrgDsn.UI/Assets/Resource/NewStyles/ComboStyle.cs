using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace SSYM.OrgDsn.UI
{
    public partial class ComboStyle : ResourceDictionary
    {
        public ComboStyle()
        {
            
        }
        public CustomPopupPlacement[] placePopup(Size popupSize,
                                          Size targetSize,
                                          Point offset)
        {
            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(-50, 100), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement placement2 =
                new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1, placement2 };
            return ttplaces;
        }
    }


    //public static class comboPos
    //{
    //    public comboPos()
    //    {

    //    }

    //    public static CustomPopupPlacement[] placePopup(Size popupSize,
    //                                      Size targetSize,
    //                                      Point offset)
    //    {
    //        CustomPopupPlacement placement1 =
    //           new CustomPopupPlacement(new Point(-50, 100), PopupPrimaryAxis.Vertical);

    //        CustomPopupPlacement placement2 =
    //            new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);

    //        CustomPopupPlacement[] ttplaces =
    //                new CustomPopupPlacement[] { placement1, placement2 };
    //        return ttplaces;
    //    }
    //}
}
