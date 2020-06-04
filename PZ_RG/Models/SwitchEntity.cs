using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RG_PZ2.Models
{
    class SwitchEntity:PowerEntity
    {
        public string Status { get; set; }
        public override Brush SetDefaultColor()
        {
            return Brushes.DeepSkyBlue;
        }
        public override string ToString()
        {
            string retVal;
            retVal = $"TYPE: {(this.GetType().Name)} \nID:{(Id)}\nName:{(Name)}\nStatus{(Status)}";
            return retVal;
        }
    }
}
