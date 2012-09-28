using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS.CustomExtensions;

namespace POS.Tests.WebUi.Controllers.CustomExtensions
{
    /// <summary>
    ///This is a test class for ListExtensionTest and is intended
    ///to contain all ListExtensionTest Unit Tests
    ///</summary>
    [TestClass]
    public class ListExtensionTest
    {
        /// <summary>
        ///Median using an Odd number of Decimals
        ///</summary>
        [TestMethod]
        public void MedianUsingDecimalWithOddLengthList()
        {
            IEnumerable<Decimal> source = new List<Decimal>
                {
                    5,
                    1,
                    3,
                    9,
                    10
                };
            var expected = new Decimal(5);
            Decimal actual;
            actual = source.Median();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Median using an Even number of Decimals
        ///</summary>
        [TestMethod]
        public void MedianUsingDecimalWithEvenLengthList()
        {
            IEnumerable<Decimal> source = new List<Decimal>
                {
                    5,
                    1,
                    3,
                    9,
                    10,
                    11
                };
            var expected = new Decimal(7);
            Decimal actual;
            actual = source.Median();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Median using an odd number of Doubles
        ///</summary>
        [TestMethod]
        public void MedianUsingDoubleWithOddLengthList()
        {
            IEnumerable<Double> source = new List<Double>
                {
                    5.55,
                    1.11,
                    3.33,
                    9.99,
                    10.10
                };
            var expected = new Decimal(5.55);
            Decimal actual;
            actual = source.Median();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Median using an even number of Doubles
        ///</summary>
        [TestMethod]
        public void MedianUsingDoubleWithEvenLengthList()
        {
            IEnumerable<Double> source = new List<Double>
                {
                    5.55,
                    1.11,
                    3.33,
                    9.99,
                    10.10,
                    11.11
                };
            var expected = new Decimal(7.77);
            Decimal actual;
            actual = source.Median();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Ensures the source list is not reordered after calling Median()
        ///</summary>
        [TestMethod]
        public void SourceListNotReOrdered()
        {
            IEnumerable<Decimal> source = new List<Decimal>
                {
                    5,
                    1,
                    3,
                    9,
                    10
                };
            var expected = new Decimal(5); // this is both the first item in the list and the median
            Decimal firstItemInList;
            Decimal medianResult;
            medianResult = source.Median();
            Assert.AreEqual(expected, medianResult);
            firstItemInList = source.First();
            Assert.AreEqual(expected, firstItemInList);
        }
    }
}