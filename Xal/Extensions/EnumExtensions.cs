using System;
using System.ComponentModel;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Enum"/> objects.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the value of <see cref="System.ComponentModel.DescriptionAttribute"/>.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns>A <see cref="string"/>.</returns>
        public static string GetDescription(this Enum @enum)
        {
            var field = @enum.GetType().GetField(@enum.ToString());
            if (field == null)
                return null;

            var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0
                ? (string)attrs[0].GetType().GetProperty("Description").GetValue(attrs[0], null)
                : @enum.ToString();
        }
    }
}