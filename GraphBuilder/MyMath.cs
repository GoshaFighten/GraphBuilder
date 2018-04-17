using GraphBuilder.Models;
using Meta.Numerics.Statistics;
using Meta.Numerics.Statistics.Distributions;
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
            return result.ToArray().Select(kvp => new MyPoint(kvp.Key, kvp.Value)).OrderBy(p => p.X).ToArray();
        }

        static double Round(double value, double precision)
        {
            return Math.Round(value / precision) * precision;
        }

        public static MyPoint[] GetGraphDataForNormalDistribution(double mean, double sigma)
        {
            var list = new List<MyPoint>();
            var d = new NormalDistribution(mean, sigma);
            var min = mean - (3.1 * sigma);
            var max = mean + (3.1 * sigma);
            var step = (max - min) / 1000 > 0.1 ? 0.1 : (max - min) / 1000;
            for (double i = min; i < max; i += step)
            {
                list.Add(new MyPoint(i, d.ProbabilityDensity(i)));
            }
            return list.ToArray();
        }

        public static FitResult NormalDistributionFitToSample(double[] data)
        {
            var result = NormalDistribution.FitToSample(new Sample(data));
            return result;
        }

        public static MyPoint[] WindowTest(double[] data, int window)
        {
            if (window % 2 != 1)
            {
                window++;
            }
            var result = new MyPoint[data.Length - window + 1];
            for (int i = window / 2; i < data.Length - window / 2; i++)
            {
                var windowData = new double[window];
                for (int j = 0; j < window; j++)
                {
                    var x = i + j - window / 2;
                    var value = data[x];
                    windowData[j] = value;
                }
                var fitResult = NormalDistribution.FitToSample(new Sample(windowData));
                var point = new MyPoint(i, fitResult.GoodnessOfFit.Probability);
                result[i - window / 2] = point;
            }
            return result;
        }
    }
}
