using System;
using System.Numerics;

namespace Xal
{
    public static class NumberExtensions
    {
        extension<T>(T n) where T : INumber<T>
        {
            public T Abs() => T.Abs(n);

            public T Cap(T max) => n > max ? max : n;

            public T Clamp(T min, T max)
            {
                if (min > max)
                    throw new ArgumentException($"'{nameof(min)}' cannot be greater than '{nameof(max)}'.");

                if (n < min)
                    return min;

                return n > max ? max : n;
            }

            public T Floor(T min) => n < min ? min : n;

            public bool IsBetween(T min, T max) => n >= min && n <= max;

            public int Sign() => T.Sign(n);
        }

        extension(decimal n)
        {
            public decimal Round(int decimals) => Math.Round(n, decimals);

            public decimal Truncate() => Math.Truncate(n);
        }

        extension(double n)
        {
            public double Round(int digits) => Math.Round(n, digits);

            public double Truncate() => Math.Truncate(n);
        }
    }
}