using MindFusion.Diagramming.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MindFusion.Diagramming.Wpf.Lanes;

namespace SSYM.OrgDsn.Model.BPMNShapes
{
    public class ShpBase : ShapeNode
    {
        #region ' Fields '

        private AnchorPattern pattern;


        #endregion

        #region ' Initialaizer '

        public ShpBase()
        {
        }

        public ShpBase(Enum.TypShp typShp)
        {
            
        }

        #endregion

        #region ' Properties / Commands '

        Enum.TypShp typ;

        public Enum.TypShp Typ
        {
            get { return typ; }
            set
            {
                typ = value;

                this.Brush = System.Windows.Media.Brushes.SkyBlue;
                this.StrokeThickness = 0;
                this.ImageAlign = MindFusion.Diagramming.Wpf.ImageAlign.Stretch;

                try
                {
                    this.Image = new BitmapImage(new Uri(string.Format("pack://application:,,,/SSYM.OrgDsn.UI;component/Assets/img/BPMN/{0}.png", value.ToString())));
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public Header hdr { get; set; }

        public AnchorPattern Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }




        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' events '

        #endregion

    }
}
