using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RG_PZ2.Models
{
    class NodeEntity : PowerEntity
    {
        public NodeEntity()
        {
            
        }
        public override Brush SetDefaultColor()
        {
            return Brushes.Lime;
        }
        public override string ToString()
        {
            string retVal;
            retVal = $"TYPE: {(this.GetType().Name)} \nID:{(Id)}\nName:{(Name)}";
            return retVal;
        }


    }
}
