using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder.Models {
    public struct MyPoint {
        public MyPoint(double x, double y) {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public static MyPoint[] GetMinusGraph(MyPoint[] data, double d) {
            return data.Select(p => new MyPoint() { X = p.X, Y = p.Y - d }).ToArray();
        }

        public static MyPoint[] GetPlusGraph(MyPoint[] data, double d) {
            return data.Select(p => new MyPoint() { X = p.X, Y = p.Y + d }).ToArray();
        }

        internal static MyPoint[] GetPlusKGraph(MyPoint[] data, double d, double alpha) {
            return GetPlusGraph(data, d).Select(p => new MyPoint() { X = p.X, Y = p.Y * alpha }).ToArray();
        }

        internal static MyPoint[] GetMinusKGraph(MyPoint[] data, double d, double alpha) {
            return GetMinusGraph(data, d).Select(p => new MyPoint() { X = p.X, Y = p.Y * alpha }).ToArray();
        }
    }
}
