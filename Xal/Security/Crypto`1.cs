using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Xal.Security
{
    /// <summary>
    /// Provide methods to symmetric encryption and decryption.
    /// </summary>
    /// <typeparam name="T">The <see cref="SymmetricAlgorithm"/> to use in the data encryption or decryption.</typeparam>
    public class Crypto<T> : IDisposable where T : SymmetricAlgorithm, new()
    {
        private readonly SymmetricAlgorithm _algorithm;
        private readonly Encoding _encoding;
        private readonly int _iterations;
        private readonly int _saltSize;
        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private const int MINIMUM_ITERATIONS_RECOMMENDED = 1000;
        private const int MINIMUM_SALT_SIZE = 8;
        private const int MIX_SIZE = 4;

        /// <summary>
        /// Initializes a new instance of <see cref="Crypto{T}"/> class with default settings.
        /// </summary>
        public Crypto() : this(Encoding.UTF8) { }

        /// <summary>
        /// Initializes a new instance of <see cref="Crypto{T}"/> class.
        /// </summary>
        /// <param name="iterations">The number of iterations for the operation. The minimum recommended number is 1000.</param>
        /// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
        public Crypto(int iterations = MINIMUM_ITERATIONS_RECOMMENDED, int saltSize = MINIMUM_SALT_SIZE) : this(Encoding.UTF8, iterations, saltSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Crypto{T}"/> class.
        /// </summary>
        /// <param name="encoding">The encoding to be used in the input and password.</param>
        /// <param name="iterations">The number of iterations for the operation. The minimum recommended number is 1000.</param>
        /// <param name="saltSize">The size of the random salt that you want the class to generate.</param>
        public Crypto(Encoding encoding, int iterations = MINIMUM_ITERATIONS_RECOMMENDED, int saltSize = MINIMUM_SALT_SIZE)
        {
            _algorithm = new T();
            _encoding = encoding;
            _iterations = iterations;
            _saltSize = saltSize;
        }

        /// <summary>
        /// Encrypts the specified input string.
        /// </summary>
        /// <param name="input">The text to be encrypted.</param>
        /// <param name="password">The password to be used on encryption.</param>
        /// <returns>A string that represents the encrypted input string.</returns>
        public string Encrypt(string input, string password)
        {
            return Convert.ToBase64String(Encrypt(_encoding.GetBytes(input), _encoding.GetBytes(password)));
        }

        /// <summary>
        /// Encrypts the specified input string.
        /// </summary>
        /// <param name="input">The text to be encrypted.</param>
        /// <param name="password">The password to be used on encryption.</param>
        /// <param name="salt">The salt to be used on encryption operation.</param>
        /// <returns>A string that represents the encrypted input string.</returns>
        public string Encrypt(string input, string password, string salt)
        {
            var inputBytes = _encoding.GetBytes(input);
            var passBytes = _encoding.GetBytes(password);
            using (var sha = SHA256.Create())
                passBytes = sha.ComputeHash(passBytes);

            var mix = new byte[MIX_SIZE];
            _rng.GetBytes(mix);

            var mixedInput = new byte[mix.Length + inputBytes.Length];
            Array.Copy(mix, 0, mixedInput, 0, MIX_SIZE);
            Array.Copy(inputBytes, 0, mixedInput, MIX_SIZE, inputBytes.Length);

            return Convert.ToBase64String(Encrypt(mixedInput, passBytes, _encoding.GetBytes(salt)));
        }

        /// <summary>
        /// Encrypts the specified input bytes.
        /// </summary>
        /// <param name="inputBytes">The bytes to be encrypted.</param>
        /// <param name="passwordBytes">The password bytes to be used on encryption.</param>
        /// <returns>A byte array that represents the encrypted input bytes.</returns>
        public byte[] Encrypt(byte[] inputBytes, byte[] passwordBytes)
        {
            var saltBytes = new byte[_saltSize];
            _rng.GetBytes(saltBytes);

            EncryptBytes(inputBytes, passwordBytes, saltBytes, out var encryptedBytes, out var IV);

            var result = new byte[saltBytes.Length + IV.Length + encryptedBytes.Length];
            Array.Copy(saltBytes, 0, result, 0, saltBytes.Length);
            Array.Copy(IV, 0, result, saltBytes.Length, IV.Length);
            Array.Copy(encryptedBytes, 0, result, saltBytes.Length + IV.Length, encryptedBytes.Length);

            return result;
        }

        /// <summary>
        /// Encrypts the specified input bytes.
        /// </summary>
        /// <param name="inputBytes">The bytes to be encrypted.</param>
        /// <param name="passwordBytes">The password bytes to be used on encryption.</param>
        /// <param name="saltBytes">The salt bytes to be used on encryption operation.</param>
        /// <returns>A byte array that represents the encrypted input bytes.</returns>
        public byte[] Encrypt(byte[] inputBytes, byte[] passwordBytes, byte[] saltBytes)
        {
            EncryptBytes(inputBytes, passwordBytes, saltBytes, out var encryptedBytes, out var _);
            return encryptedBytes;
        }

        /// <summary>
        /// Decrypts the specified input string.
        /// </summary>
        /// <param name="input">The text to be decrypted.</param>
        /// <param name="password">The password to be used on decryption.</param>
        /// <returns>A string that represents the decrypted input string.</returns>
        public string Decrypt(string input, string password)
        {
            return _encoding.GetString(Decrypt(Convert.FromBase64String(input), _encoding.GetBytes(password)));
        }

        /// <summary>
        /// Decrypts the specified input string.
        /// </summary>
        /// <param name="input">The text to be decrypted.</param>
        /// <param name="password">The password to be used on decryption.</param>
        /// <param name="salt">The salt to be used on encryption operation.</param>
        /// <returns>A string that represents the decrypted input string.</returns>
        public string Decrypt(string input, string password, string salt)
        {
            var inputBytes = Convert.FromBase64String(input);
            var passBytes = _encoding.GetBytes(password);

            using (var sha = SHA256.Create())
                passBytes = sha.ComputeHash(passBytes);

            var decryptedBytes = Decrypt(inputBytes, passBytes, _encoding.GetBytes(salt));

            var textBytes = new byte[decryptedBytes.Length - MIX_SIZE];
            for (int i = MIX_SIZE, j = decryptedBytes.Length; i < j; i++)
                textBytes[i - MIX_SIZE] = decryptedBytes[i];

            return _encoding.GetString(textBytes);
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="inputBytes">The bytes to be decrypted.</param>
        /// <param name="passwordBytes">The password bytes to be used on decryption.</param>
        /// <returns>A byte array that represents the decrypted input bytes.</returns>
        public byte[] Decrypt(byte[] inputBytes, byte[] passwordBytes)
        {
            var saltBytes = new byte[_saltSize];
            var IV = new byte[_algorithm.BlockSize >> 3];

            var encryptedData = new byte[inputBytes.Length - IV.Length - saltBytes.Length];
            Array.Copy(inputBytes, 0, saltBytes, 0, saltBytes.Length);
            Array.Copy(inputBytes, saltBytes.Length, IV, 0, IV.Length);
            Array.Copy(inputBytes, saltBytes.Length + IV.Length, encryptedData, 0, encryptedData.Length);

            DecryptBytes(encryptedData, passwordBytes, saltBytes, IV, out var decryptedBytes);
            return decryptedBytes;
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="inputBytes">The bytes to be decrypted.</param>
        /// <param name="passwordBytes">The password bytes to be used on decryption.</param>
        /// <param name="saltBytes">The salt bytes to be used on decryption operation.</param>
        /// <returns>A string that represents the decrypted input string.</returns>
        public byte[] Decrypt(byte[] inputBytes, byte[] passwordBytes, byte[] saltBytes)
        {
            DecryptBytes(inputBytes, passwordBytes, saltBytes, null, out var decryptedBytes);
            return decryptedBytes;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _rng.Dispose();
            _algorithm.Dispose();
        }

        private void EncryptBytes(byte[] inputBytes, byte[] passwordBytes, byte[] saltBytes, out byte[] encryptedBytes, out byte[] IV)
        {
            using (var r = new Rfc2898DeriveBytes(passwordBytes, saltBytes, _iterations))
            {
                var key = r.GetBytes(_algorithm.KeySize >> 3);
                IV = r.GetBytes(_algorithm.BlockSize >> 3);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, _algorithm.CreateEncryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }
        }

        private void DecryptBytes(byte[] inputBytes, byte[] passwordBytes, byte[] saltBytes, byte[] IV, out byte[] decryptedBytes)
        {
            using (var r = new Rfc2898DeriveBytes(passwordBytes, saltBytes, _iterations))
            {
                var key = r.GetBytes(_algorithm.KeySize >> 3);
                if (IV == null)
                    IV = r.GetBytes(_algorithm.BlockSize >> 3);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, _algorithm.CreateDecryptor(key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }
        }
    }
}