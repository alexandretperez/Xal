using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xal.Util
{
    /// <summary>
    /// A utility class for math operations.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Determines the MOD 11 check digit of a reference <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <returns>The check digit.</returns>
        /// <exception cref="System.NotSupportedException">Value not supported.</exception>
        public static int Mod11(long value)
        {
            var weights = new[] { 2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5, 6, 7, 8, 9 };
            var sValue = value.ToString(CultureInfo.InvariantCulture);

            if (sValue.Length > 16)
                throw new NotSupportedException("Value not supported.");

            var soma = 0;
            var index = 0;
            for (var i = sValue.Length - 1; i >= 0; i--)
            {
                soma += Convert.ToInt32(sValue[i].ToString(CultureInfo.InvariantCulture)) * weights[index];
                index++;
            }

            var rest = (soma * 10) % 11;
            return rest >= 10 ? 0 : rest;
        }

        /// <summary>
        /// Splits the reference <paramref name="value"/> in the number of specified <paramref name="installments"/>.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <param name="installments">The number of installments.</param>
        /// <returns>The installments resulting of the division.</returns>
        public static List<decimal> Split(decimal value, int installments)
        {
            var installment = Math.Round(value / installments, 2);
            var result = new List<decimal>();
            for (var i = 0; i < installments; i++)
                result.Add(installment);

            if (result.Count == 0)
                return result;

            var diff = value - (installment * installments);
            result[0] += diff;
            return result;
        }

        /// <summary>
        /// Truncates the reference <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The reference value.</param>
        /// <param name="decimals">The decimal length.</param>
        /// <returns>The <see cref="double"/>.</returns>
        public static double Truncate(double value, int decimals)
        {
            var f = Math.Pow(10, decimals);
            return Math.Truncate(value * f) / f;
        }

        /// <summary>
        /// Updates the list of <paramref name="values"/> equally after change the value of an specified <paramref name="index"/>.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="index">The index.</param>
        /// <param name="newValue">The new value.</param>
        public static void Update(ref List<decimal> values, int index, decimal newValue)
        {
            if (values[index] == newValue)
                return;

            var total = values.Sum();
            values[index] = newValue;

            var indexes = new List<int>();
            for (int i = index; i < values.Count; i++)
                indexes.Add(i);

            for (int i = 0; i < index; i++)
                indexes.Add(i);

            var sum = 0m;
            for (int i = 0; i <= index; i++)
                sum += values[i];

            var diff = (total - sum);
            if (diff < 0)
            {
                diff = (total - values[index]);
                var diffValues = Split(diff, values.Count - 1);
                for (int i = 1; i < values.Count; i++)
                {
                    index = indexes[i];
                    values[index] = diffValues[i - 1];
                }
            }
            else
            {
                var len = 0;
                if ((index + 1) == values.Count)
                {
                    index = 0;
                    len = values.Count - 1;
                }
                else
                {
                    index++;
                    len = values.Count - index;
                }

                var diffValues = Split(diff, len);
                for (int i = 0; i < diffValues.Count; i++)
                    values[index++] = diffValues[i];
            }
        }
    }
}