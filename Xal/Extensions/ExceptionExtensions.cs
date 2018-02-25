using System;
using System.Data.SqlClient;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Exception"/> objects.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Determines whether the reference exception represents a foreign key violation of the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns></returns>
        public static bool IsForeignKeyViolation(this Exception exception)
        {
            var sqlException = exception.GetBaseException() as SqlException;
            return sqlException != null && sqlException.Number == 547;
        }

        /// <summary>
        /// Determines whether the reference exception is a <see cref="SqlException"/>.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns></returns>
        public static bool IsSqlException(this Exception exception)
        {
            return exception.GetBaseException() is SqlException;
        }

        /// <summary>
        /// Determines whether the reference exception refers to the timeout expired by the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns><c>true</c> if the exception refers to the timeout expired; otherwise, <c>false</c>.</returns>
        public static bool IsTimeoutExpired(this Exception exception)
        {
            var sqlException = exception.GetBaseException() as SqlException;
            return sqlException != null
                   && (sqlException.Number == -2
                       || sqlException.Number == 53
                       || sqlException.Message.ToLower().Contains("timeout expired"));
        }

        /// <summary>
        /// Determines whether the reference exception refers to a unique constraint violation on the SQL Server.
        /// </summary>
        /// <param name="exception">The reference exception.</param>
        /// <returns><c>true</c> if the exception refers to a unique constraint violation; otherwise, <c>false</c>.</returns>
        public static bool IsUniqueConstraintViolation(this Exception exception)
        {
            var sqlException = exception.GetBaseException() as SqlException;
            return sqlException != null && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }
    }
}