
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PZ2.Models
{
   public  class PowerEntity
    {
        public PowerEntity()
        {

        }

        public long Id { get; set; }

        public string Name { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
          public void ClickFunction(object sender, EventArgs e)
        {
     
        }
        virtual  public Brush SetDefaultColor()
        {
            return Brushes.White;
        }
    }
}
