using System;
using GraphBuilder;
using GraphBuilder.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        static PrivateType pt = new PrivateType(typeof(MyMath));
        static PrivateType GetPrivateType()
        {
            return pt;
        }

        static double Round(double value, double precision)
        {
            return (double)GetPrivateType().InvokeStatic("Round", value, precision);
        }

        [TestMethod]
        public void TestRound1()
        {
            var value = 120;
            var precision = 100;
            var expectation = 100;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound2()
        {
            var value = 170;
            var precision = 100;
            var expectation = 200;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound3()
        {
            var value = 150;
            var precision = 100;
            var expectation = 200;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound4()
        {
            var value = 150;
            var precision = 10;
            var expectation = 150;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound5()
        {
            var value = 1.2;
            var precision = 1;
            var expectation = 1;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound6()
        {
            var value = 1.7;
            var precision = 1;
            var expectation = 2;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound7()
        {
            var value = 1.5;
            var precision = 1;
            var expectation = 2;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound8()
        {
            var value = 1.5;
            var precision = 0.1;
            var expectation = 1.5;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestRound9()
        {
            var value = 1.55;
            var precision = 0.1;
            var expectation = 1.6;
            Assert.AreEqual(expectation, Round(value, precision));
        }

        [TestMethod]
        public void TestAggregation1()
        {
            var source = new double[] { 100, 101, 102, 103 };
            var expectation = new MyPoint[] {
                new MyPoint(100, 0.25),
                new MyPoint(101, 0.25),
                new MyPoint(102, 0.25),
                new MyPoint(103, 0.25)
            };
            var precision = 1;
            CollectionAssert.AreEqual(expectation, MyMath.AggregateData(source, precision));
        }

        [TestMethod]
        public void TestAggregation2()
        {
            var source = new double[] { 100, 101, 102, 103 };
            var expectation = new MyPoint[] {
                new MyPoint(100, 1)
            };
            var precision = 10;
            CollectionAssert.AreEqual(expectation, MyMath.AggregateData(source, precision));
        }

        [TestMethod]
        public void TestAggregation3()
        {
            var source = new double[] { 100, 104, 107, 113 };
            var expectation = new MyPoint[] {
                new MyPoint(100, 0.5),
                new MyPoint(110, 0.5)
            };
            var precision = 10;
            CollectionAssert.AreEqual(expectation, MyMath.AggregateData(source, precision));
        }

        [TestMethod]
        public void EnsureSorting()
        {
            var source = new double[] { 100, 107, 113, 104, 104 };
            var expectation = new MyPoint[] {
                new MyPoint(100, 0.2),
                new MyPoint(104, 0.4),
                new MyPoint(107, 0.2),
                new MyPoint(113, 0.2)
            };
            var precision = 1;
            CollectionAssert.AreEqual(expectation, MyMath.AggregateData(source, precision));
        }
    }
}
