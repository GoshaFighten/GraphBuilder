using GraphBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder
{
    public static class MyMath
    {
        public static MyPoint[] AggregateData(double[] source, double precision)
        {
            var result = new Dictionary<double, double>();
            foreach (var record in source)
            {
                var value = Round(record, precision);
                if (result.ContainsKey(value))
                {
                    result[value] += 1.0 / source.Length;
                }
                else
                {
                    result.Add(value, 1.0 / source.Length);
                }
            }
            return result.ToArray().Select(kvp => new MyPoint(kvp.Key, kvp.Value)).ToArray();
        }

        static double Round(double value, double precision)
        {
            return Math.Round(value / precision) * precision;
        }
    }
}
