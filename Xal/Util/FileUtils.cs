using System;
using System.IO;
using Xal.Extensions;

namespace Xal.Util
{
    /// <summary>
    /// A utility class to work with system files.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="absoluteFilePath">The absolute file path.</param>
        /// <param name="errorHandler">If an error occurs, this handler will be invoked.</param>
        /// <returns><c>true</c> if the directory is deleted; otherwise <c>false</c>.</returns>
        public static bool SafeDelete(string absoluteFilePath, Action<Exception> errorHandler = null)
        {
            try
            {
                if (absoluteFilePath.IsNullOrWhiteSpace())
                    return false;

                File.Delete(absoluteFilePath);
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