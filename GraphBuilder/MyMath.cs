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

        public static Dictionary<int, double> GetMinusDs(double[] data, NormalDistribution distribution) {
            var sortedData = data.OrderBy(x => x).ToArray();
            var result = new Dictionary<int, double>();
            for (int i = 0; i < sortedData.Length; i++) {
                var m = i + 1;
                result.Add(m, distribution.LeftProbability(sortedData[i]) - ((double)m - 1.0) / sortedData.Length);
            }
            return result;
        }

        public static Dictionary<int, double> GetPlusDs(double[] data, Distribution distribution) {
            var sortedData = data.OrderBy(x => x).ToArray();
            var result = new Dictionary<int, double>();
            for (int i = 0; i < sortedData.Length; i++) {
                var m = i + 1;
                result.Add(m, ((double)m) / sortedData.Length - distribution.LeftProbability(sortedData[i]));
            }
            return result;
        }

        private static double GetMaxD(double[] data, NormalDistribution distribution) {
            var plusDs = GetPlusDs(data, distribution);
            var minusDs = GetMinusDs(data, distribution);
            var maxD = Math.Max(plusDs.Select(d => d.Value).Max(), minusDs.Select(d => d.Value).Max());
            return maxD;
        }

        public static double GetDn(double alpha, int n) {
            var y = -Math.Log(alpha);
            return Math.Pow(y / (2 * n), 0.5) - 1 / (6 * n);
        }

        public static Tuple<double, double> GetNormalDistributionProbability(double[] data, double alpha) {
            var distribution = GetNormalDistribution(data);
            var maxD = GetMaxD(data, distribution);
            var dn = GetDn(alpha, data.Length);
            return Tuple.Create(maxD, dn);
        }

        public static MyPoint[] WindowTestMyLogic(double[] data, int window) {
            if (window % 2 != 1) {
                window++;
            }
            var result = new MyPoint[data.Length - window + 1];
            for (int i = window / 2; i < data.Length - window / 2; i++) {
                var windowData = GetWindowData(data, window, i);
                var fitResult = NormalDistribution.FitToSample(new Sample(windowData));
                var point = new MyPoint(i, fitResult.GoodnessOfFit.Probability);
                result[i - window / 2] = point;
            }
            return result;
        }

        public static Tuple<MyPoint[], MyPoint[]> WindowTestNewLogic(double[] data, int window) {
            if (window % 2 != 1) {
                window++;
            }
            var result1 = new MyPoint[data.Length - window + 1];
            var result2 = new MyPoint[data.Length - window + 1];
            for (int i = window / 2; i < data.Length - window / 2; i++) {
                double[] windowData = GetWindowData(data, window, i);
                var result = GetNormalDistributionProbability(windowData, 0.05);
                var point1 = new MyPoint(i, result.Item1);
                result1[i - window / 2] = point1;
                var point2 = new MyPoint(i, result.Item2);
                result2[i - window / 2] = point2;
            }
            return Tuple.Create(result1, result2);
        }

        private static double[] GetWindowData(double[] data, int window, int i) {
            var windowData = new double[window];
            for (int j = 0; j < window; j++) {
                var x = i + j - window / 2;
                var value = data[x];
                windowData[j] = value;
            }

            return windowData;
        }

        public static Dictionary<double, double> GetF(double[] data) {
            var sortedData = data.OrderBy(x => x).ToArray();
            var result = new Dictionary<double, double>();
            for (int i = 0; i < sortedData.Length; i++) {
                var x = sortedData[i];
                var f = ((double)i + 1) / sortedData.Length;
                if (result.ContainsKey(x)) {
                    result[x] = f;
                } else {
                    result.Add(x, f);
                }
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
