using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WetLand.ModelParameters
{
    public class Parameters
    {
        public int type;
        public double min;
        public double max;
        public double c;
        public Parameters(int type,double min,double max,double c) {
            this.type = type;
            this.min = min;
            this.max = max;
            this.c = c;
        }
    }
}
