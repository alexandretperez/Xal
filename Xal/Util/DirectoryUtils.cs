using System;
using System.IO;
using Xal.Extensions;

namespace Xal.Util
{
    /// <summary>
    /// A utility class to work with system directories.
    /// </summary>
    public static class DirectoryUtils
    {
        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <param name="absoluteDirPath">The absolute directory path.</param>
        /// <param name="recursive">If set to <c>true</c> deletes the directory recursively.</param>
        /// <param name="errorHandler">If an error occurs, this handler will be invoked.</param>
        /// <returns><c>true</c> if the directory is deleted; otherwise <c>false</c>.</returns>
        public static bool SafeDelete(string absoluteDirPath, bool recursive = true, Action<Exception> errorHandler = null)
        {
            try
            {
                if (absoluteDirPath.IsNullOrWhiteSpace())
                    return false;

                Directory.Delete(absoluteDirPath, recursive);
                return true;
            }
            catch (Exception e)
            {
                errorHandler?.Invoke(e);
            }

            return false;
        }
    }
}