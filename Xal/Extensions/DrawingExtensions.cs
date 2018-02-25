using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Xal.Extensions
{
    /// <summary>
    /// Provides extension methods for the objects of the <see cref="System.Drawing"/> namespace.
    /// </summary>
    public static class DrawingExtensions
    {
        /// <summary>
        /// Gets the Base64 string representation of the reference image.
        /// </summary>
        /// <param name="image">The reference image.</param>
        /// <returns>A string.</returns>
        /// <remarks>More info about Base URI Scheme at <a href="https://en.wikipedia.org/wiki/Data_URI_scheme" target="_blank">https://en.wikipedia.org/wiki/Data_URI_scheme</a>.</remarks>
        public static string GetBase64String(this Image image)
        {
            return GetBase64String(image, ImageFormat.Png);
        }

        /// <summary>
        /// Gets the Base64 string representation of the reference image.
        /// </summary>
        /// <param name="image">The reference image.</param>
        /// <param name="format">The desired image format to be represented into the string.</param>
        /// <returns>A string.</returns>
        /// <remarks>More info about Base URI Scheme at <a href="https://en.wikipedia.org/wiki/Data_URI_scheme" target="_blank">https://en.wikipedia.org/wiki/Data_URI_scheme</a>.</remarks>
        public static string GetBase64String(this Image image, ImageFormat format)
        {
            var codec = ImageCodecInfo.GetImageDecoders().SingleOrDefault(p => p.FormatID == format.Guid);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                var bytes = ms.ToArray();
                return $"data:{(codec == null ? "image/unknown" : codec.MimeType)};base64,{Convert.ToBase64String(bytes)}";
            }
        }

        /// <summary>
        /// Finds the dominant color of the reference image.
        /// </summary>
        /// <param name="bitmap">The reference image.</param>
        /// <returns>A <see cref="Color"/> object that represents the dominant color of the image.</returns>
        /// <remarks>For high resolution images, its highly recommended to resize the image first to get better performance. See <see cref="Resize(Image, double)"/> or <see cref="Resize(Image, int, int, bool)"/>.</remarks>
        public static Color GetDominantColor(this Bitmap bitmap)
        {
            // Lock the bitmap's bits
            var data = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var stride = Math.Abs(data.Stride);

            var bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            // Gets the bytes of the image
            var pixels = new byte[stride * bitmap.Height];

            // Gets the address of the first line
            var pointer = data.Scan0;

            // Copy the pixels (RGB values) into the array
            Marshal.Copy(pointer, pixels, 0, pixels.Length);

            var height = data.Height;
            var width = data.Width * bytesPerPixel;

            var red = 0;
            var green = 0;
            var blue = 0;
            var total = 0;

            for (var h = 0; h < height; h++)
            {
                var line = h * stride;
                for (var w = 0; w < width; w += bytesPerPixel)
                {
                    blue += pixels[line + w];
                    green += pixels[line + w + 1];
                    red += pixels[line + w + 2];

                    total++;
                }
            }

            red /= total;
            green /= total;
            blue /= total;

            bitmap.UnlockBits(data);
            return Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Finds the dominant color of the reference image.
        /// </summary>
        /// <param name="image">The reference image.</param>
        /// <returns>A <see cref="Color"/> object that represents the dominant color of the image.</returns>
        /// <remarks>For high resolution images, its highly recommended to resize the image first to get better performance. See <see cref="Resize(Image, double)"/> or <see cref="Resize(Image, int, int, bool)"/>.</remarks>
        public static Color GetDominantColor(this Image image)
        {
            using (var bitmap = new Bitmap(image))
                return GetDominantColor(bitmap);
        }

        /// <summary>
        /// Resizes the reference image by a specified percentage scale.
        /// </summary>
        /// <param name="image">The img.</param>
        /// <param name="scale">The percentage scale.</param>
        /// <returns>An <see cref="Image"/>.</returns>
        /// <remarks>Note that a scale of 1 does not cause any changes on the image.</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">The value must be greather than 0.</exception>
        public static Image Resize(this Image image, double scale)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale), "The value must be greather than 0.");

            if (scale == 1)
                return image;

            var width = image.Width * scale;
            var height = image.Height * scale;

            return Resize(image, (int)width, (int)height, true);
        }

        /// <summary>
        /// Resizes the reference image proportionally by the smallest dimension (width or height) value.
        /// </summary>
        /// <param name="image">The reference image.</param>
        /// <param name="width">The reference width to reach or to determine the maximum allowed.</param>
        /// <param name="height">The reference height to reach or to determine the maximum allowed.</param>
        /// <param name="allowsExpand">
        /// Determines whether the values of the parameters <paramref name="width"/> and <paramref name="height" /> should be reached or should be considered as the maximum value allowed.
        /// <c>true</c> to reach one of these values (which allows the expansion of the image); otherwise, <c>false</c>.</param>
        /// <returns>An <see cref="Image"/>.</returns>
        public static Image Resize(this Image image, int width, int height, bool allowsExpand = false)
        {
            if (!allowsExpand && (image.Width < width && image.Height < height))
                return image;

            var wr = (double)width / image.Width;
            var hr = (double)height / image.Height;

            var ratio = Math.Min(wr, hr);

            var imageWidth = (int)(image.Width * ratio);
            var imageHeight = (int)(image.Height * ratio);

            var nb = new Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(nb))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, imageWidth, imageHeight));
            }

            return nb;
        }
    }
}