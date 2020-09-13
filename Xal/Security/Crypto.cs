using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Xal.Security
{
    /// <summary>
    /// Provides mechanisms of encryption/decryption
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Decrypts the specified encrypted bytes with the <typeparamref name="T"/> algorithm.
        /// </summary>
        /// <param name="encryptedBytes">The encrypted bytes.</param>
        /// <param name="passwordBytes">The password bytes.</param>
        /// <param name="saltBytes">The salt.</param>
        /// <typeparam name="T">Algorithm type</typeparam>
        /// <returns>The decrypted byte array</returns>
        public static byte[] Decrypt<T>(byte[] encryptedBytes, byte[] passwordBytes, byte[] saltBytes) where T : SymmetricAlgorithm, new()
        {
            byte[] decryptedBytes;
            using (var ms = new MemoryStream())
            {
                using (var alg = new T())
                {
                    alg.Padding = PaddingMode.PKCS7;
                    using (var r = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000))
                    {
                        var key = r.GetBytes(alg.KeySize >> 3);
                        var IV = r.GetBytes(alg.BlockSize >> 3);
                        using (var cs = new CryptoStream(ms, alg.CreateDecryptor(key, IV), CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                            cs.FlushFinalBlock();
                        }
                    }
                }
                decryptedBytes = ms.ToArray();
            }
            return decryptedBytes;
        }

        /// <summary>
        /// Decrypts the specified encrypted text with the <typeparamref name="T"/> algorithm.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <typeparam name="T">Algorithm type</typeparam>
        /// <returns>The decrypted byte array</returns>
        public static string Decrypt<T>(string encryptedText, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var sha = SHA256.Create())
                passwordBytes = sha.ComputeHash(passwordBytes);

            var decryptedBytes = Decrypt<T>(encryptedBytes, passwordBytes, Encoding.UTF8.GetBytes(salt));
            const int saltSize = 4;

            var textBytes = new byte[decryptedBytes.Length - saltSize];
            for (int i = saltSize, j = decryptedBytes.Length; i < j; i++)
                textBytes[i - saltSize] = decryptedBytes[i];

            return Encoding.UTF8.GetString(textBytes);
        }

        /// <summary>
        /// Encrypts the specified text bytes with the <typeparamref name="T"/> algorithm.
        /// </summary>
        /// <param name="textBytes">The text bytes.</param>
        /// <param name="passwordBytes">The password bytes.</param>
        /// <param name="saltBytes">The salt.</param>
        /// <typeparam name="T">Algorithm type</typeparam>
        /// <returns>The encrypted byte array</returns>
        public static byte[] Encrypt<T>(byte[] textBytes, byte[] passwordBytes, byte[] saltBytes) where T : SymmetricAlgorithm, new()
        {
            byte[] encryptedBytes;
            using (var ms = new MemoryStream())
            {
                using (var alg = new T())
                {
                    alg.Padding = PaddingMode.PKCS7;
                    using (var r = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000))
                    {
                        var key = r.GetBytes(alg.KeySize >> 3);
                        var IV = r.GetBytes(alg.BlockSize >> 3);
                        using (var cs = new CryptoStream(ms, alg.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                        {
                            cs.Write(textBytes, 0, textBytes.Length);
                            cs.FlushFinalBlock();
                        }
                    }
                }
                encryptedBytes = ms.ToArray();
            }
            return encryptedBytes;
        }

        /// <summary>
        /// Encrypts the specified text with the <typeparamref name="T"/> algorithm.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <typeparam name="T">Algorithm type</typeparam>
        /// <returns>The encrypted byte array</returns>
        public static string Encrypt<T>(string text, string password, string salt) where T : SymmetricAlgorithm, new()
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var sha = SHA256.Create())
                passwordBytes = sha.ComputeHash(passwordBytes);

            var textBytesWithSalt = GetTextBytesSalted(textBytes);

            var encryptedBytes = Encrypt<T>(textBytesWithSalt, passwordBytes, Encoding.UTF8.GetBytes(salt));
            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypts the specified input using MD5CryptoServiceProvider
        /// </summary>
        /// <param name="input">The encrypted input string.</param>
        /// <param name="key">The key.</param>
        /// <returns>The decrypted string.</returns>
        public static string MD5Decrypt(string input, string key)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keyArr = md5.ComputeHash(Encoding.UTF8.GetBytes(key));

                using (var tds = new TripleDESCryptoServiceProvider { Key = keyArr, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var ict = tds.CreateDecryptor();

                    var inputBytes = Convert.FromBase64String(input);
                    var result = ict.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Encoding.UTF8.GetString(result);
                }
            }
        }

        /// <summary>
        /// Encrypts the specified input using MD5CryptoServiceProvider
        /// <para>
        /// ATTENTION: This encryption uses CipherMode.ECB which is not safe for encrypt sensitive data.
        /// </para>
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <returns>The encrypted string.</returns>
        public static string MD5Encrypt(string input, string key)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keyArr = md5.ComputeHash(Encoding.UTF8.GetBytes(key));

                using (var tds = new TripleDESCryptoServiceProvider { Key = keyArr, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var ict = tds.CreateEncryptor();

                    var inputBytes = Encoding.UTF8.GetBytes(input);
                    var result = ict.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Convert.ToBase64String(result, 0, result.Length);
                }
            }
        }

        /// <summary>
        /// Base64s the URL decrypt.
        /// </summary>
        /// <param name="input">The encrypted input string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc4648
        /// </remarks>
        [Obsolete("This method will be removed in future versions. Use the extension method CoreExtensions.DecodeUrl instead.")]
        public static string UrlDecrypt(string input, Encoding encoding)
        {
            var mod = input.Length % 4;
            var pad = mod == 0 ? 0 : 4 - mod;
            var s = new StringBuilder(input, input.Length + pad)
                .Append(string.Empty.PadRight(pad, '='))
                .Replace('-', '+')
                .Replace('_', '/');

            return encoding.GetString(Convert.FromBase64String(s.ToString()));
        }

        /// <summary>
        /// Base64s the URL decrypt.
        /// </summary>
        /// <param name="input">The encrypted input string.</param>
        /// <returns></returns>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc4648
        /// </remarks>
        [Obsolete("This method will be removed in future versions. Use the extension method CoreExtensions.DecodeUrl instead.")]
        public static string UrlDecrypt(string input)
        {
            return UrlDecrypt(input, Encoding.UTF8);
        }

        /// <summary>
        /// Base64s the URL encrypt.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc4648
        /// </remarks>
        [Obsolete("This method will be removed in future versions. Use the extension method CoreExtensions.EncodeUrl instead.")]
        public static string UrlEncrypt(string input, Encoding encoding)
        {
            var s = new StringBuilder(Convert.ToBase64String(encoding.GetBytes(input)).TrimEnd('='))
            .Replace('+', '-')
            .Replace('/', '_');

            return s.ToString();
        }

        /// <summary>
        /// Base64s the URL encrypt.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <remarks>
        /// See http://tools.ietf.org/html/rfc4648
        /// </remarks>
        [Obsolete("This method will be removed in future versions. Use the extension method CoreExtensions.EncodeUrl instead.")]
        public static string UrlEncrypt(string input)
        {
            return UrlEncrypt(input, Encoding.UTF8);
        }

        private static byte[] GetTextBytesSalted(IList<byte> textBytes)
        {
            // Generating salt bytes
            var saltBytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltBytes);

            // Appending salt bytes to original bytes
            var bytesToBeEncrypted = new byte[saltBytes.Length + textBytes.Count];
            for (var i = 0; i < saltBytes.Length; i++)
                bytesToBeEncrypted[i] = saltBytes[i];

            for (var i = 0; i < textBytes.Count; i++)
                bytesToBeEncrypted[i + saltBytes.Length] = textBytes[i];

            return bytesToBeEncrypted;
        }
    }
}