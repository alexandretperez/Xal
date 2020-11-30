using System;
using System.Data;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for data objects.
    /// </summary>
    public static class DataExtensions
    {
        private static T FieldAs<T>(object value, T defaultValue = default, bool throws = true)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception) when (!throws)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnName"/>.</returns>
        public static T As<T>(this DataRow row, string columnName)
            => FieldAs<T>(row[columnName]);

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnIndex"/>.</returns>
        public static T As<T>(this DataRow row, int columnIndex)
            => FieldAs<T>(row[columnIndex]);

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="defaultValue">The default value to return in case of conversion fails.</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnName"/>.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-value is null and conversionType is a value type.-or-value does not implement the System.IConvertible interface.</exception>
        /// <exception cref="FormatException">value is not in a format recognized by conversionType.</exception>
        /// <exception cref="OverflowException">value represents a number that is out of the range of conversionType.</exception>
        public static T As<T>(this DataRow row, string columnName, T defaultValue)
            => FieldAs<T>(row[columnName], defaultValue, false);

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="defaultValue">The default value to return in case of conversion fails.</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnIndex"/>.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-value is null and conversionType is a value type.-or-value does not implement the System.IConvertible interface.</exception>
        /// <exception cref="FormatException">value is not in a format recognized by conversionType.</exception>
        /// <exception cref="OverflowException">value represents a number that is out of the range of conversionType.</exception>
        public static T As<T>(this DataRow row, int columnIndex, T defaultValue)
            => FieldAs<T>(row[columnIndex], defaultValue, false);

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnName"/>.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-value is null and conversionType is a value type.-or-value does not implement the System.IConvertible interface.</exception>
        /// <exception cref="FormatException">value is not in a format recognized by conversionType.</exception>
        /// <exception cref="OverflowException">value represents a number that is out of the range of conversionType.</exception>
        [Obsolete("This method is obsolete and will be removed in future releases. Use the DataExtensions.As<T> method instead.")]
        public static T FieldAs<T>(this DataRow row, string columnName)
        {
            return FieldAs<T>(row[columnName]);
        }

        /// <summary>
        /// Provides access to each of the column values in the specified row and converts its value to the specified type.
        /// </summary>
        /// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
        /// <param name="row">The input DataRow, which acts as the this instance for the extension method.</param>
        /// <param name="columnIndex">The column index</param>
        /// <returns>The value, of type T, of the <see cref="DataColumn"/> specified by <paramref name="columnIndex"/>.</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-value is null and conversionType is a value type.-or-value does not implement the System.IConvertible interface.</exception>
        /// <exception cref="FormatException">value is not in a format recognized by conversionType.</exception>
        /// <exception cref="OverflowException">value represents a number that is out of the range of conversionType.</exception>
        [Obsolete("This method is obsolete and will be removed in future releases. Use the DataExtensions.As<T> method instead.")]
        public static T FieldAs<T>(this DataRow row, int columnIndex)
        {
            return FieldAs<T>(row[columnIndex]);
        }
    }
}