using System;
using System.Data.SqlClient;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="SqlException"/> objects.
    /// </summary>
    public static class SqlExceptionExtensions
    {
        /// <summary>
        /// Determines whether the reference exception represents a foreign key violation of the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns></returns>
        public static bool IsForeignKeyViolation(this SqlException exception)
        {
            return exception.Number == 547;
        }

        /// <summary>
        /// Determines whether the reference exception refers to the timeout expired by the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns><c>true</c> if the exception refers to the timeout expired; otherwise, <c>false</c>.</returns>
        public static bool IsTimeoutExpired(this SqlException exception)
        {
            return exception.Number == -2
                || exception.Number == 53
                || exception.Message.IndexOf("timeout expired", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Determines whether the reference exception refers to a unique constraint violation on the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns><c>true</c> if the exception refers to a unique constraint violation; otherwise, <c>false</c>.</returns>
        public static bool IsUniqueConstraintViolation(this SqlException exception)
        {
            return exception.Number == 2601 || exception.Number == 2627;
        }
    }
}