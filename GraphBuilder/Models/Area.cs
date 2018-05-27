using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder.Models {
    public class Area {
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public AreaType Type { get; set; }
    }

    public enum AreaType {
        Positive,
        Negative
    }
}
