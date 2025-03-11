using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSYM.OrgDsn.Model.BPMNShapes
{
    public class ShpObj : ShpBase
    {
        #region ' Fields '

        #endregion

        #region ' Initialaizer '

        public ShpObj(Enum.TypShp typShp)
        {
            

            this.Bounds = new Rect(new Size() { Height = 50, Width = 25 });
            this.Shape = MindFusion.Diagramming.Wpf.Shape.FromId("File");
            this.Typ = typShp;


            Pattern = new AnchorPattern(new[] {
				new AnchorPoint(100, 50, false, true, MarkStyle.None),
				new AnchorPoint(0, 50, true, false, MarkStyle.None)
			});

            this.AnchorPattern = Pattern;
        }

        #endregion

        #region ' Properties / Commands '



        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' events '

        #endregion

    }
}
