using GraphBuilder.Models;
using Meta.Numerics.Statistics;
using Meta.Numerics.Statistics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder {
    public static class MyMath {
        public static MyPoint[] AggregateData(double[] source, double precision) {
            var result = new Dictionary<double, double>();
            foreach (var record in source) {
                var value = Round(record, precision);
                if (result.ContainsKey(value)) {
                    result[value] += 1.0 / source.Length;
                } else {
                    result.Add(value, 1.0 / source.Length);
                }
            }
            return result.ToArray().Select(kvp => new MyPoint(kvp.Key, kvp.Value)).OrderBy(p => p.X).ToArray();
        }

        static double Round(double value, double precision) {
            return Math.Round(value / precision) * precision;
        }

        public static MyPoint[] GetGraphDataForNormalDistribution(double mean, double sigma) {
            var list = new List<MyPoint>();
            var d = new NormalDistribution(mean, sigma);
            var min = mean - (3.1 * sigma);
            var max = mean + (3.1 * sigma);
            var step = (max - min) / 1000 > 0.1 ? 0.1 : (max - min) / 1000;
            for (double i = min; i < max; i += step) {
                list.Add(new MyPoint(i, d.ProbabilityDensity(i)));
            }
            return list.ToArray();
        }

        public static FitResult NormalDistributionFitToSample(double[] data) {
            var result = NormalDistribution.FitToSample(new Sample(data));
            return result;
        }

        public static MyPoint[] WindowTest(double[] data, int window) {
            if (window % 2 != 1) {
                window++;
            }
            var result = new MyPoint[data.Length - window + 1];
            for (int i = window / 2; i < data.Length - window / 2; i++) {
                var windowData = new double[window];
                for (int j = 0; j < window; j++) {
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

        public static MyPoint[] GetAllowedFMaxGraph(MyPoint[] fData, double d, double alpha) {
            return MyPoint.GetPlusKGraph(fData, d, alpha);
        }

        public static MyPoint[] GetAllowedFMinGraph(MyPoint[] fData, double d, double alpha) {
            return MyPoint.GetMinusKGraph(fData, d, alpha);
        }

        public static double CalcD(double[] data) {
            var distributionTarget = GetNormalDistribution(data);
            var distributionSource = GetF(data);
            var d = double.MinValue;
            foreach (var val in data) {
                var newD = Math.Abs(distributionTarget.LeftProbability(val) - distributionSource[val]);
                if (newD > d) {
                    d = newD;
                }
            }
            return d * Math.Sqrt(data.Length);
        }

        public static Dictionary<double, double> GetF(double[] data) {
            var temp = new Dictionary<double, double>();
            foreach (var val in data) {
                if (temp.ContainsKey(val)) {
                    temp[val] = val;
                } else {
                    temp.Add(val, val);
                }
            }
            double y = 0;
            var sortedData = temp.OrderBy(kvp => kvp.Value);
            var sum = sortedData.Sum(kvp => kvp.Value);
            var result = new Dictionary<double, double>();
            foreach (var kvp in sortedData) {
                y += kvp.Key;
                result.Add(kvp.Key, y / sum);
            }
            return result;
        }

        public static MyPoint[] GetFGraph(double[] data) {
            return GetF(data).Select(kvp => new MyPoint() { X = kvp.Key, Y = kvp.Value }).ToArray();
        }

        public static MyPoint[] GetNormalDistributionGraphData(double[] data) {
            var d = GetNormalDistribution(data);
            var result = new List<MyPoint>();
            foreach (var val in data.OrderBy(val => val)) {
                result.Add(new MyPoint() { X = val, Y = d.LeftProbability(val) });
            }
            return result.ToArray();
        }

        public static NormalDistribution GetNormalDistribution(double[] data) {
            var sample = new Sample(data);
            return new NormalDistribution(sample.PopulationMean.Value, sample.PopulationStandardDeviation.Value);
        }
    }
}
