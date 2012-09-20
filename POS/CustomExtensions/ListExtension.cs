using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.CustomExtensions
{
    public static class ListExtension
    {
        //TODO: Make a unit test
        /// <summary>
        /// Computes the median of a sequence of System.Decimal values
        /// </summary>
        /// <param name="source">A sequence of System.Decimal values to calculate the median of.</param>
        /// <returns>The median of the sequence of the values</returns>
        public static decimal Median(this IEnumerable<decimal> source)
        {
            // Create a copy of the input, and sort the copy
            decimal[] temp = source.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                decimal a = temp[count / 2 - 1];
                decimal b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        }

        //TODO: Make a unit test
        /// <summary>
        /// Computes the median of a sequence of System.Double values
        /// </summary>
        /// <param name="source">A sequence of System.Double values to calculate the median of.</param>
        /// <returns>The median of the sequence of the values</returns>
        public static decimal Median(this IEnumerable<double> source)
        {
            // Create a copy of the input, and sort the copy
            double[] temp = source.ToArray();
            Array.Sort(temp);

            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                decimal a = (decimal) temp[count / 2 - 1];
                decimal b = (decimal) temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return (decimal) temp[count / 2];
            }
        }
    }
}